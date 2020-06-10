using BasicClass;
using DealCalibrate;
using DealComprehensive;
using DealPLC;
using DealResult;
using DealRobot;
using Main_EX;
using ModulePackage;
using System;
using System.Collections;
using System.Windows;
using DealLog;
namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 粗定位
        /// <summary>
        /// 粗定位
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum CalcPickPos(string cellName, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    WriteTPick(0);
                    WritePickDelta(new Point2D(1, -1));
                    WriteQrCodeDelta(new Point2D(1, -1));
                    FinishPhotoPLC(CameraResult.OK);
                    SendTData(DataRegister1.TAngle_pickPlat1, DataRegister1.TAngleConfirm_pickPlat1, ModelParams.T_Roller, "机器人去平台取片");
                    return StateComprehensive_enum.True;
                }
                #endregion

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                ResultScaledShape result = (ResultScaledShape)htResult[cellName];
                ResultTemplate template = (ResultTemplate)htResult[cellName + @"T"];

                if (result.X * result.Y == 0)
                {
                    g_UCDisplayCamera.ShowResult("未识别到产品" ,"red");
                    FinishPhotoPLC(CameraResult.NG);
                }
                else
                {
                    CalcPickDelta(new Point2D(result.X, result.Y), ModelParams.stdPxlCamera1, ParCalibWorld.V_I[1]);
                    CalcQrCodeDelta(result.R_J + template.RCenterProfile / Math.PI * 180);
                    WriteTPick(-(result.R_J + template.RCenterProfile / Math.PI * 180)+ModelParams.adjTPickPlat);
                    //直接给确认信号
                    SendTData(DataRegister1.TAngle_pickPlat1, DataRegister1.TAngleConfirm_pickPlat1, ModelParams.T_Roller, "机器人去平台取片");
                    FinishPhotoPLC(CameraResult.OK);
                }

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                FinishPhotoPLC(CameraResult.NG);
                return StateComprehensive_enum.False;
            }
        }

        private void WriteTPick(double angle)
        {
            try
            {
                ModelParams.T_Roller = angle;
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void CalcPickDelta(Point2D pxlReal, Point2D pxlStd, double amp)
        {
            try
            {
                double deltaX = Math.Round((pxlReal.DblValue1 - pxlStd.DblValue1) * amp + ModelParams.adjRollerLinePick.DblValue1, 3);
                double deltaY = Math.Round((pxlReal.DblValue2 - pxlStd.DblValue2) * amp + ModelParams.adjRollerLinePick.DblValue2, 3);
                WritePickDelta(new Point2D(deltaX, deltaY));
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void CalcQrCodeDelta(double angle)
        {
            try
            {
                Point2D f1 = new Point2D(ModelParams.confQrCodeX - 0.5 * ModelParams.confGlassX, ModelParams.confQrCodeY - 0.5 * ModelParams.confGlassY);
                Point2D f2 = TransDelta(f1, angle, 0);
                double deltaX = Math.Round(f1.DblValue1 - f2.DblValue1 + ModelParams.adjQrCode.DblValue1, 3);
                double deltaY = Math.Round(f1.DblValue2 - f2.DblValue2 + ModelParams.adjQrCode.DblValue2, 3);
                WriteQrCodeDelta(new Point2D(deltaX, deltaY));
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void WritePickDelta(Point2D pickDelta)
        {
            try
            {
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.PickCompensateX, pickDelta.DblValue1);
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.PickCompensateY, pickDelta.DblValue2);
                ShowState(string.Format("发送PLC搬运轴滚筒线取片调整值:X_{0},Y_{1}", pickDelta.DblValue1, pickDelta.DblValue2));
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void WriteQrCodeDelta(Point2D qrCodeDelta)
        {
            try
            {
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeCompensateX, qrCodeDelta.DblValue1);
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeCompensateY, qrCodeDelta.DblValue2);
                ShowState(string.Format("发送PLC二维码位置调整值:X_{0},Y_{1}", qrCodeDelta.DblValue1, qrCodeDelta.DblValue2));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion
    }
}
