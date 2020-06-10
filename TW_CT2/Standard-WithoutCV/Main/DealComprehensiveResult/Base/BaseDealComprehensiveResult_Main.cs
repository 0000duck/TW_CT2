using BasicClass;
using CalibDataManager;
using DealCalibrate;
using Main_EX;
using System;
using System.Collections;
using System.Threading;
using StationDataManager;
using DealPLC;
using DealRobot;
using System.Collections.Generic;
using DealConfigFile;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using DealLog;
using DealComprehensive;
using DealResult;

namespace Main
{
    /// <summary>
    /// 继承于Main_EX中基类
    /// </summary>
    public partial class BaseDealComprehensiveResult_Main : BaseDealComprehensiveResult
    {
        #region 变量定义            
        //粗定位空跑用
        static Random rd = new Random();
        int glassnum = rd.Next(1, 10);
        int cnt = 0;

        const string strRotateCalibCell1 = @"C10";
        const string strRotateCalibCell2 = @"C16";
        public Point2D[] pt2Calib = new Point2D[2] { new Point2D(), new Point2D() };

        public static Point2D[] biyMark1 = new Point2D[3] { new Point2D(), new Point2D(),new Point2D() };
        public static Point2D[] biyMark2 = new Point2D[3] { new Point2D(), new Point2D(), new Point2D() };
        public static Point2D[] pt2MarkArray = new Point2D[10] { new Point2D(), new Point2D(),
            new Point2D(), new Point2D(), new Point2D(), new Point2D(), new Point2D(), new Point2D(),new Point2D(),new Point2D() };
        public static double[] calcResultInsp = new double[3];
        public static double[] calcResultInsert = new double[3];

        public static Action StopBelt;
        public static Action RefreshStatistics = null;
        /// <summary>
        /// 放大系数 --YF
        /// </summary>
        public double AMP { get => ParCalibWorld.V_I[g_NoCamera]; }

        public const string ReservDigits = "f3";

        public Mutex g_precise = new Mutex();

        //标定旋转中心
        public static List<Point2D> CalibData_L = new List<Point2D>();
        public static Point2D RotateCenter { get; set; }

        #endregion

        #region 算子序号定义
        public string strCamera1Template1 = @"C2T";
        public string strCamera2Template1 = @"C2";
        public string strCamera3Template1 = @"C2";
        public string strCamera4Template1 = @"C2";
        public string strCamera1Template2 = @"C12";
        public string strCamera2Template2 = @"C12";
        public string strCamera3Template2 = @"C8";
        public string strCamera4Template2 = @"C12";
        public string strCamera3Template3 = @"C14";

        public string strCamera1Match1 = @"C2";//
        public string strCamera2Match1 = @"C2";//
        public string strCamera3Match1 = @"C2";//
        public string strCamera4Match1 = @"C4";//
        public string strCamera1Match2 = @"C14";
        public string strCamera2Match2 = @"C14";
        public string strCamera3Match2 = @"C12";
        public string strCamera4Match2 = @"C12";
        public string strCamera3Match3 = @"C10";

        public string strCamera5Match1 = @"C4";//
        public string strCamera5Match2 = @"C12";
        public string strCamera5Match3 = @"C2";

        public const string strCamera1RC = @"C8";//
        public const string strCamera5RC = @"C16";//

        public string strCamera6Match1 = @"C2";//
        public string strCamera6Match2 = @"C12";
        public string strCamera6Match3 = @"C10";

        public const string strCamera6RC = @"C16";//

        public string strCamera7Match1 = @"C2";//
        public string strCamera7Match2 = @"C2";
        public string strCamera7Match3 = @"C2";

        public string strCamera8Match1 = @"C2";//
        public string strCamera8Template1 = @"C2T";
        #endregion

        #region 双目自动流程
        public void CalcAutoStPos(int station)
        {
            try
            {
                g_precise.WaitOne();

                if (!BaseDealComprehensiveResult.BlFinishPos1_Cam2)
                {
                    return;
                }

                BaseDealComprehensiveResult.BlFinishPos1_Cam2 = false;
             
                double angle = Math.Atan((biyMark2[(int)PtType_Binary.AutoMark].DblValue2 - biyMark1[(int)PtType_Binary.AutoMark].DblValue2) * AMP / ModelParams.DisMark) * 180 / Math.PI - StationDataMngr.instance.CalibPos_L[station - 1].DblValue4;

                ShowState("工位" + station + "双目计算需要顺时针补偿角度: " + angle);

                //因为玻璃需要顺时针旋转angle角度，由于采用右手坐标系，所以mark1绕旋转中心顺时针旋转angle度，也就是mark1围绕旋转中心逆时针旋转-angle度
                FunCalibRotate fcr = new FunCalibRotate();
                Point2D MarkAfterRotate = fcr.GetPoint_AfterRotation(-angle / 180 * Math.PI, ModelParams.rotateCenter, biyMark1[(int)PtType_Binary.AutoMark]);
                //可以得到像素的偏差，因为是补偿且处于右手坐标系所以可以统一取负号wcy1118
                //xc 12-13,x取反
                double DeltaX = Math.Round((MarkAfterRotate.DblValue1 - StationDataMngr.instance.CalibPos_L[station - 1].DblValue1) * AMP, 3);
                double DeltaY = -Math.Round((MarkAfterRotate.DblValue2 - StationDataMngr.instance.CalibPos_L[station - 1].DblValue2) * AMP, 3);

                double stAngle = 0;
                double stT = 0;
                Point4D stAdj = new Point4D(0, 0, 0, 0);
                Point4D stStd = new Point4D(0, 0, 0, 0);
                switch(station)
                {
                    case 1:
                        stAngle = ModelParams.GlassAngle_LeftAOI;
                        stAdj = ModelParams.adjPosS1_1;
                        stStd = ModelParams.pSt1_1;
                        stT = ModelParams.T_stdLeftAOI;
                        break;
                    case 2:
                        stAngle = ModelParams.GlassAngle_LeftAOI;
                        stAdj = ModelParams.adjPosS1_2;
                        stStd = ModelParams.pSt1_2;
                        stT = ModelParams.T_stdLeftAOI;
                        break;
                    case 3:
                        stAngle = ModelParams.GlassAngle_MidAOI;
                        stAdj = ModelParams.adjPosS2_1;
                        stStd = ModelParams.pSt2_1;
                        stT = ModelParams.T_stdMidAOI;
                        break;
                    case 4:
                        stAngle = ModelParams.GlassAngle_MidAOI;
                        stAdj = ModelParams.adjPosS2_2;
                        stStd = ModelParams.pSt2_2;
                        stT = ModelParams.T_stdMidAOI;
                        break;
                    case 5:
                        stAngle = ModelParams.GlassAngle_RightAOI;
                        stAdj = ModelParams.adjPosS3_1;
                        stStd = ModelParams.pSt3_1;
                        stT = ModelParams.T_stdRightAOI;
                        break;
                    case 6:
                        stAngle = ModelParams.GlassAngle_RightAOI;
                        stAdj = ModelParams.adjPosS3_2;
                        stStd = ModelParams.pSt3_2;
                        stT = ModelParams.T_stdRightAOI;
                        break;
                }

                Point2D delta = new Point2D();
                delta = TransDelta(new Point2D(DeltaX, DeltaY), stAngle, ModelParams.GlassAngle_Precise);
                ShowState(string.Format("工位{0}纯视觉机器人补偿X_{1},Y_{2}", station, delta.DblValue1, delta.DblValue2));
                ModelParams.pPlaceAOI = new Point4D(0, 0, 0, 0);
                ModelParams.pPlaceAOI.DblValue1 = stStd.DblValue1 + stAdj.DblValue1 + delta.DblValue1;
                ModelParams.pPlaceAOI.DblValue2 = stStd.DblValue2 + stAdj.DblValue2 + delta.DblValue2;
                ModelParams.pPlaceAOI.DblValue3 = stStd.DblValue3;
                ModelParams.pPlaceAOI.DblValue4 = stStd.DblValue4;
                ModelParams.T_realAOI = (stT + angle + 360) % 360;

                if (ModelParams.IfPreciseConfirm)
                {
                    //发送精度验证信息
                    double t_preciseConfirm = (angle + ModelParams.T_stdPrecise + 360) % 360;
                    SendTData(DataRegister1.TAngle_preciseConfirm, DataRegister1.TAngleConfirm_preciseConfirm, t_preciseConfirm, "双目精度验证");

                    Point4D pPreciseConfirm = ModelParams.pPrecise;
                    pPreciseConfirm.DblValue1 += DeltaX;
                    pPreciseConfirm.DblValue2 += DeltaY;
                    LogicRobot.L_I.WriteRobotCMD(pPreciseConfirm, PCToRbt.PRP_PreciseConfirm);
                }
                else
                {
                    //发送工位信息
                    SendTData(DataRegister1.TAngle_placeAOI, DataRegister1.TAngleConfirm_placeAOI, ModelParams.T_realAOI, "机器人放工位");
                    LogicRobot.L_I.WriteRobotCMD(ModelParams.pPlaceAOI, PCToRbt.PRP_St);
                    ShowState(string.Format("发送工位{0}放片坐标,X_{1},Y_{2},Z_{3},R_{4}", station, ModelParams.pPlaceAOI.DblValue1, ModelParams.pPlaceAOI.DblValue2, ModelParams.pPlaceAOI.DblValue3, ModelParams.pPlaceAOI.DblValue4));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_precise.ReleaseMutex();
            }

        }

        public void SendTData(DataRegister1 drTAngle, DataRegister1 drTAngleConfirm, double data, string msg)
        {
            try
            {
                LogicPLC.L_I.WriteRegData1((int)drTAngle, data);
                LogicPLC.L_I.WriteRegData1((int)drTAngleConfirm, 1);
                ShowState("PC->PLC:" + msg + "T轴角度:" + Math.Round(data, 2));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion

        #region 双目验证流程
        public void CalcPreciseConfirmStPos(int station)
        {
            try
            {
                g_precise.WaitOne();

                double angle = Math.Atan((biyMark2[(int)PtType_Binary.PreciseConfirmMark].DblValue2 - biyMark1[(int)PtType_Binary.PreciseConfirmMark].DblValue2) * AMP / ModelParams.DisMark) * 180 / Math.PI - StationDataMngr.instance.CalibPos_L[station - 1].DblValue4;

                ShowState("工位" + station + "双目精度验证流程得到顺时针补偿角度: " + angle);

                double DeltaX = -Math.Round((biyMark1[(int)PtType_Binary.PreciseConfirmMark].DblValue1 - StationDataMngr.instance.CalibPos_L[station - 1].DblValue1) * AMP, 3);
                double DeltaY = -Math.Round((biyMark1[(int)PtType_Binary.PreciseConfirmMark].DblValue2 - StationDataMngr.instance.CalibPos_L[station - 1].DblValue2) * AMP, 3);

                ShowState("工位" + station + "双目精度验证流程得到X方向偏差_" + DeltaX + ",Y方向偏差_" + DeltaY);
                SendTData(DataRegister1.TAngleConfirm_placeAOI, DataRegister1.TAngleConfirm_placeAOI, ModelParams.T_realAOI, "机器人放工位");

                LogicRobot.L_I.WriteRobotCMD(ModelParams.pPlaceAOI, PCToRbt.PRP_St);
                ShowState(string.Format("发送工位{0}放片坐标,X_{1},Y_{2},Z_{3},R_{4}", station, ModelParams.pPlaceAOI.DblValue1, ModelParams.pPlaceAOI.DblValue2, ModelParams.pPlaceAOI.DblValue3, ModelParams.pPlaceAOI.DblValue4));

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_precise.ReleaseMutex();
            }

        }
        #endregion

        #region 双目标定流程
        public void CalcCalibPos(int station)
        {
            try
            {
                double angle = Math.Atan((biyMark2[(int)PtType_Binary.Calib].DblValue2 - biyMark1[(int)PtType_Binary.Calib].DblValue2) * AMP / ModelParams.DisMark) * 180 / Math.PI;

                StationDataMngr.instance.CalibPos_L[station - 1].DblValue1 = biyMark1[(int)PtType_Binary.Calib].DblValue1;
                StationDataMngr.instance.CalibPos_L[station - 1].DblValue2 = biyMark1[(int)PtType_Binary.Calib].DblValue2;
                StationDataMngr.instance.CalibPos_L[station - 1].DblValue4 = angle;
                StationDataMngr.instance.WriteIniCalibPos(station);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion

        #region 标定旋转中心
        public void DealCalibRC(int index)
        {
            try
            {
                double rcAngle = 0;
                double rcPxlGate = 0;
                int rcTryTimes = 0;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    rcAngle = WinRobotCalib.GetWinInst.ucCalibRC.RotateAngle;
                    rcPxlGate = WinRobotCalib.GetWinInst.ucCalibRC.RCPxlGate;
                    rcTryTimes = (int)WinRobotCalib.GetWinInst.ucCalibRC.TimesPhoto;
                }));
                if (index == 1)
                {
                    SendTData(Dir_Rotate.Positive, rcAngle);
                    return;
                }
                if (index == 2)
                {
                    CalcRotateCenter();
                    SendTData(Dir_Rotate.Negative, rcAngle);
                    return;
                }
                if (index%2 ==1)
                {
                    //这里为了形式好看，传入角度为逆时针旋转的结果，但是底层对其取反
                    if(AdjustRC(Dir_Rotate.Negative, rcAngle, rcPxlGate))
                    {
                        //标定结束
                        ParAdjust.SetValue1(ModelParams.ES(ComAdj.RotateCenter), RotateCenter.DblValue1);
                        ParAdjust.SetValue2(ModelParams.ES(ComAdj.RotateCenter), RotateCenter.DblValue2);
                        LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_FinishCalibRC);
                        ShowState(string.Format("标定得到最终旋转中心:", RotateCenter.DblValue1, RotateCenter.DblValue2));
                        return;
                    }
                    SendTData(Dir_Rotate.Positive, rcAngle);
                }
                if (index%2 ==0)
                {
                    if(AdjustRC(Dir_Rotate.Positive, rcAngle, rcPxlGate))
                    {
                        ParAdjust.SetValue1(ModelParams.ES(ComAdj.RotateCenter), RotateCenter.DblValue1);
                        ParAdjust.SetValue2(ModelParams.ES(ComAdj.RotateCenter), RotateCenter.DblValue2);
                        LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_FinishCalibRC);
                        ShowState(string.Format("标定得到最终旋转中心:", RotateCenter.DblValue1, RotateCenter.DblValue2));
                        return;
                    }
                    SendTData(Dir_Rotate.Negative, rcAngle);
                }
                if (index> rcTryTimes)
                {
                    ShowWinError_Invoke("旋转中心标定失败，请重新标定!");
                    LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_FinishCalibRC);
                    return;
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void ShowWinError_Invoke(string str)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new StrAction(ShowWinError), str);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        void ShowWinError(string str)
        {
            try
            {
                ShowAlarm(str);
                WinError.GetWinInst().ShowError(str);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private bool AdjustRC(Dir_Rotate dir, double angle, double gate)
        {
            try
            {
                Point2D pDest = new FunCalibRotate().GetPoint_AfterRotation(-(int)dir * angle / 180 * Math.PI, RotateCenter, CalibData_L[CalibData_L.Count - 2]);
                Point2D pReal = CalibData_L[CalibData_L.Count - 1];
                Point2D pOffset = pDest - pReal;

                ShowState(string.Format("标定得到旋转中心偏差像素X_{0},Y_{1}", pOffset.DblValue1, pOffset.DblValue2));

                if (Math.Abs(pOffset.DblValue1)<gate && Math.Abs(pOffset.DblValue2)<gate)
                {
                    return true;
                }

                RotateCenter.DblValue1 += pOffset.DblValue2 / (Math.Sin(-(int)dir * angle / 180 * Math.PI));
                RotateCenter.DblValue2 += -pOffset.DblValue1 / (Math.Sin(-(int)dir * angle / 180 * Math.PI));

                return false;
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        private void CalcRotateCenter()
        {
            try
            {
                RotateCenter = new FunCalibRotate().GetOriginPoint(-WinRobotCalib.GetWinInst.ucCalibRC.RotateAngle, CalibData_L[0], CalibData_L[1]);
                ShowState("得到第一次标定旋转中心:" + RotateCenter);
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void SendTData(Dir_Rotate dir, double angle)
        {
            int pn = 0;
            if ((int)dir ==1)
            {
                pn = 1;
            }
            LogicPLC.L_I.WriteRegData1((int)DataRegister1.TAngle_calib, (pn * WinRobotCalib.GetWinInst.ucCalibRC.RotateAngle + ModelParams.T_stdPrecise + 360) % 360);
            LogicPLC.L_I.WriteRegData1((int)DataRegister1.TAngleConfirm_calib, 1);
            ShowState(string.Format("标定旋转中心:PC->PLC,T轴顺时针旋转{0}度", (int)dir * angle));
        }
        #endregion

        #region CT2专用下游定位
        public StateComprehensive_enum CalcDownStream(string cellName,out Hashtable htResult)
        {
            htResult = null;
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.DownStreamYCompensate, 1);
                    //向下游PLC发送X轴补偿和R轴补偿，协议待定
                    FinishPhotoPLC(CameraResult.OK);
                    return StateComprehensive_enum.True;
                }
                #endregion

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                //ResultScaledShape result = (ResultScaledShape)htResult[cellName];
                //ResultTemplate template = (ResultTemplate)htResult[cellName + @"T"];
                ResultBlob result = (ResultBlob)htResult[cellName];
                if (result.X * result.Y == 0)
                {
                    g_UCDisplayCamera.ShowResult("未识别到产品", "red");
                    FinishPhotoPLC(CameraResult.NG);
                }
                else
                {
                    //下游角度
                    //double angle = -(result.R_J + template.RCenterProfile / Math.PI * 180);
                    double angle = -(result.R_J);
                    //下游X轴
                    double X = (result.X - ParStd.Value1(ModelParams.ES(ComStd.StdPxlCamera4))) * ParCalibWorld.V_I[4];
                    //上游Y轴
                    double Y = -(result.Y - ParStd.Value2(ModelParams.ES(ComStd.StdPxlCamera4))) * ParCalibWorld.V_I[4];
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.DownStreamYCompensate, Y);
                    ShowState("发送下料平台Y轴补偿数据_" + Y);
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
        #endregion
    }

    public enum PtType_Binary
    {
        AutoMark,
        PreciseConfirmMark,
        Calib
    }

    public enum Dir_Rotate
    {
        /// <summary>
        /// 正向
        /// </summary>
        Positive = 1,
        /// <summary>
        /// 逆向
        /// </summary>
        Negative = -1
    }
}
