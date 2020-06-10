using System;
using System.Threading.Tasks;
using DealPLC;
using DealRobot;
using System.Threading;
using BasicClass;
using DealComprehensive;
using Main_EX;
using System.Windows;

namespace Main
{
    partial class MainWindow
    {
        #region 定义
        bool BlRobotToSafe = false;//通知机器人去安全位置
        #endregion 定义

        #region 超时
        /// <summary>
        /// 机器人接收PLC指令超时
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_Delay_event(int i)
        {

        }
        #endregion 超时

        #region 机器人HomeThrow
        /// <summary>
        /// 机器人复位完成
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_RobotReset_event(int i)
        {
            try
            {
                ShowState("机器人复位完成");
                MainCom.M_I.ResetRobot = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 机器人回到Home点
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_RobotHome_event(int i)
        {
            try
            {
                ShowState("机器人回到Home点");
                MainCom.M_I.HomeRobot = true;               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 抛料
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_RobotThrow_event(int i)
        {
            try
            {
                ShowState("机器人进行抛料");
                MainCom.M_I.HomeRobot = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 机器人HomeThrow

        #region 通知机器人去安全位置
        /// <summary>
        /// 通知机器人去安全位置
        /// </summary>
        void SendRobotSafe()
        {
            try
            {
                if (BlRobotToSafe)
                {
                    BlRobotToSafe = false;
                    LogicRobot.L_I.WriteRobotCMD("10003");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 通知机器人去安全位置

        #region 机器人触发T轴旋转事件
        protected override void RotateT_event(string str)
        {
            try
            {
                switch(str)
                {
                    //case "PICK":
                    //    SendTData(ModelParams.T_PickPlat1, (int)Type_TConfirm.UpStream, "上游平台取片");
                    //    break;
                    //case "PRECISE":
                    //    SendTData(ModelParams.T_stdPrecise, (int)Type_TConfirm.BinaryPrecise, "精定位自动流程第一次拍照");
                    //    break;
                    //case "PLACEAOI":
                    //    SendTData(ModelParams.T_realAOI, (int)Type_TConfirm.PlaceAOI, "去工位放片");
                    //    break;
                    //case "PICKAOI1":
                    //    SendTData(ModelParams.T_stdLeftAOI, (int)Type_TConfirm.PickAOI, "去工位1-1取片");
                    //    break;
                    //case "PICKAOI2":
                    //    SendTData(ModelParams.T_stdLeftAOI, (int)Type_TConfirm.PickAOI, "去工位1-2取片");
                    //    break;
                    //case "PICKAOI3":
                    //    SendTData(ModelParams.T_stdMidAOI, (int)Type_TConfirm.PickAOI, "去工位2-1取片");
                    //    break;
                    //case "PICKAOI4":
                    //    SendTData(ModelParams.T_stdMidAOI, (int)Type_TConfirm.PickAOI, "去工位2-2取片");
                    //    break;
                    //case "PICKAOI5":
                    //    SendTData(ModelParams.T_stdRightAOI, (int)Type_TConfirm.PickAOI, "去工位3-1取片");
                    //    break;
                    //case "PICKAOI6":
                    //    SendTData(ModelParams.T_stdRightAOI, (int)Type_TConfirm.PickAOI, "去工位3-2取片");
                    //    break;
                    //case "DUMP":
                    //    SendTData(ModelParams.T_stdDump, (int)Type_TConfirm.PickAOI, "去抛料盒");
                    //    break;
                    //case "PLACEPLAT2":
                    //    SendTData(ModelParams.T_stdDownStream, (int)Type_TConfirm.DownStream, "去下游平台交接");
                    //    break;
                    case "CalibRC":
                        BaseDealComprehensiveResult_Main.CalibData_L.Clear();
                        SendTData(ModelParams.T_stdPrecise, 1, "双目定位标定旋转中心第一次拍照");
                        break;
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        private void SendTData(double data, int confirmSig, string msg)
        {
            try
            {
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.TAngle_calib, data);
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.TAngleConfirm_calib, confirmSig);
                ShowState("PC->PLC:" + msg + "T轴角度:" + Math.Round(data, 2));
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
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
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        public void SendTData(DataRegister1 drTAngle, double data, string msg)
        {
            try
            {
                LogicPLC.L_I.WriteRegData1((int)drTAngle, data);
                ShowState("PC->PLC:" + msg + "T轴角度:" + Math.Round(data, 2));
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion

        #region 机器人反馈某工位还未点亮测试OK
        protected override void RemindLight_event()
        {
            try
            {
                MessageBox.Show("该工位尚未进行点亮测试，无法进行标定!");
                WinRobotCalib.GetWinInst.EnableRb(true);
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion
    }
}
