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
    public partial class DealComprehensiveResult3 : BaseDealComprehensiveResult_Main
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
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    return StateComprehensive_enum.True;
                }
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera3Match1];
                if (result.Num != 1)
                {
                    ShowAlarm("相机3--双目定位拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_PreciseNG);
                    return StateComprehensive_enum.False;
                }
                biyMark2[(int)PtType_Binary.AutoMark] = new Point2D(result.X, result.Y);
                ShowState(string.Format("双目自动流程Mark2像素:X_{0},Y_{1}", result.X, result.Y));
                CalcAutoStPos(station);
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
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义

            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera3Match1];
                if (result.Num != 1)
                {
                    ShowAlarm("相机3--双目定位精度验证Mark已出视野!");                   
                    return StateComprehensive_enum.True;
                }
                if(!BaseDealComprehensiveResult.BlFinishPos2_Cam2)
                {
                    return StateComprehensive_enum.True;
                }
                biyMark2[(int)PtType_Binary.PreciseConfirmMark] = new Point2D(result.X, result.Y);
                ShowState(string.Format("双目精度验证流程Mark2像素:X_{0},Y_{1}", result.X, result.Y));
                CalcPreciseConfirmStPos(station);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                //发送工位信息
                LogicRobot.L_I.WriteRobotCMD(ModelParams.pPlaceAOI, PCToRbt.PRP_St);
                ShowState(string.Format("发送工位{0}放片坐标,X_{1},Y_{2},Z_{3},R_{4}", station, ModelParams.pPlaceAOI.DblValue1, ModelParams.pPlaceAOI.DblValue2, ModelParams.pPlaceAOI.DblValue3, ModelParams.pPlaceAOI.DblValue4));

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
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    return StateComprehensive_enum.True;
                }
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera3Match1];
                if (result.Num != 1)
                {
                    ShowAlarm("相机3--双目定位标定流程拍照NG!");
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.PCAlarm, (int)PCAlarm_Enum.标定失败);
                    return StateComprehensive_enum.False;
                }
                biyMark2[(int)PtType_Binary.Calib] = new Point2D(result.X, result.Y);
                ShowState(string.Format("双目标定流程Mark2像素:X_{0},Y_{1}", result.X, result.Y));
                CalcCalibPos(station);
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
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义

            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos4, out htResult);

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

                Display(Pos_enum.Pos4, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照

    }
}
