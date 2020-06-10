using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealMath;
using DealImageProcess;
using BasicComprehensive;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;


namespace Main
{
    public partial class DealComprehensiveResult2 : BaseDealComprehensiveResult_Main
    {
        
        #region 定义

  
        #endregion 定义
       
        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, int station, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 1;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                BaseDealComprehensiveResult.BlFinishPos1_Cam2 = false;
                if(ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    LogicRobot.L_I.WriteRobotCMD(ModelParams.pStArr[station - 1], PCToRbt.PRP_St);
                    ShowState(string.Format("发送机器人去工位{0}放片坐标：" + ModelParams.pStArr[station - 1], station));
                    SendTData(DataRegister1.TAngle_placeAOI, DataRegister1.TAngleConfirm_placeAOI, ModelParams.T_realAOI, "机器人放工位");
                    return StateComprehensive_enum.True;
                }
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera2Match1];
                if (result.Num != 1)
                {
                    ShowAlarm("相机2--双目定位拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_PreciseNG);
                    return StateComprehensive_enum.False;
                }
                biyMark1[(int)PtType_Binary.AutoMark] = new Point2D(result.X, result.Y);
                ShowState(string.Format("双目自动流程Mark1像素:X_{0},Y_{1}", result.X, result.Y));
                BaseDealComprehensiveResult.BlFinishPos1_Cam2 = true;
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, int station, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 2;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                BaseDealComprehensiveResult.BlFinishPos2_Cam2 = false;
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera2Match1];
                if (result.Num != 1)
                {
                    ShowAlarm("相机2--双目定位精度验证Mark已出视野!");
                    
                    return StateComprehensive_enum.True;
                }
                biyMark1[(int)PtType_Binary.PreciseConfirmMark] = new Point2D(result.X, result.Y);
                ShowState(string.Format("双目精度验证流程Mark1像素:X_{0},Y_{1}", result.X, result.Y));
                BaseDealComprehensiveResult.BlFinishPos2_Cam2 = true;
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录

                Display(Pos_enum.Pos1, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }
        #endregion 位置2拍照

        #region 位置3拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, int station, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 3;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                BaseDealComprehensiveResult.BlFinishPos3_Cam2 = false;
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {

                    return StateComprehensive_enum.True;
                }
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera2Match1];
                if (result.Num != 1)
                {
                    ShowAlarm("相机2--双目定位标定流程拍照NG!");
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.PCAlarm, (int)PCAlarm_Enum.标定失败);
                    return StateComprehensive_enum.False;
                }
                biyMark1[(int)PtType_Binary.Calib] = new Point2D(result.X, result.Y);
                ShowState(string.Format("双目标定流程Mark1像素:X_{0},Y_{1}", result.X, result.Y));
                BaseDealComprehensiveResult.BlFinishPos3_Cam2 = true;
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置3拍照

        #region 位置4拍照
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e,out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 4;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera2Match1];
                if (!DealTypeResult(result))
                {
                    ShowAlarm("相机2--双目标定旋转中心流程拍照NG!");
                    ShowWinError_Invoke("旋转中心标定失败，请重新标定!");
                    LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_FinishCalibRC);
                    return StateComprehensive_enum.False;
                }
                CalibData_L.Add(new Point2D(result.X, result.Y));
                DealCalibRC(CalibData_L.Count);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照
    }
}
