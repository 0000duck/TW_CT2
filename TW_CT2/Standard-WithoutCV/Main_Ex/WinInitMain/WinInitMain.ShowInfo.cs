
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using HalconDotNet;
using DealFile;
using DealPLC;
using SetPar;
using BasicClass;
using DealLog;
using DealConfigFile;
using System.IO;

namespace Main_EX
{
    public partial class WinInitMain : BaseWindow
    {
        #region 定义
        protected static UCStateWork g_UCStateWork = null;
        protected static UCAlarm g_UCAlarm = null;

        protected UCParProduct g_UCParProduct = null;
        protected UCDisplayMainResult g_UCDisplayMainResult = null;

        protected Label g_LblStateRun = null;
        int NumStateRun = 0;

        int NumLogin = 0;//鼠标进入登录区域计时

        //Timer
        System.Windows.Forms.Timer Tm300_Refresh = new System.Windows.Forms.Timer();//刷新定时器
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 事件注册
        /// </summary>
        public void LoginEvent_ShowInfo()
        {
            try
            {
                this.Tm300_Refresh.Tick += new EventHandler(TM300_Refresh_Tick);

                g_UCStateWork.ImageInfo_event += new StateInfo_del(uCStateWork_ImageInfo_event);
                g_UCAlarm.ImageInfo_event += new StateInfo_del(uCStateWork_ImageInfo_event);
                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 初始化显示
        /// </summary>
        public void Init_ShowInfo()
        {
            //产品参数初始化
            g_UCParProduct.Init();

            //结果显示
            Init_MainResult();

            this.Dispatcher.Invoke(new Action(() =>
            {
                InitTimer_ShowInfo();

                if (ParStateAndAlarm.P_I.BlAutoShow)
                {
                    bool blNew = false;
                    WinStateAndAlarm win = WinStateAndAlarm.GetWinInst(out blNew);
                    win.Show();
                }
            }));
        }
        void InitTimer_ShowInfo()
        {
            try
            {
                Tm300_Refresh.Interval = 300;
                Tm300_Refresh.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 定时器刷新
        /// <summary>
        /// 刷新显示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TM300_Refresh_Tick(object sender, EventArgs e)
        {
            try
            {
                //是否主界面显示
                if (ParStateAndAlarm.P_I.BlShowMain)
                {
                    g_UCStateWork.ShowPar_Invoke();//显示运行状态
                    g_UCAlarm.ShowPar_Invoke();//显示报警信息
                }

                if (ParStateAndAlarm.P_I.BlAutoShow)
                {
                    //刷新日志
                    if (WinStateAndAlarm.GetWinInst() != null)
                    {
                        WinStateAndAlarm.GetWinInst().ShowStateAndAlarm();
                    }
                }

                //显示结果数据
                if (g_UCDisplayMainResult != null)
                {
                    g_UCDisplayMainResult.ShowPar_Invoke();
                }

                #region 指示软件运行状态
                NumStateRun++;
                if (NumStateRun == 4)
                {
                    ShowStateRun(Visibility.Hidden);
                }
                else if (NumStateRun == 8)
                {
                    ShowStateRun(Visibility.Visible);
                    NumStateRun = 0;
                }
                #endregion 指示软件运行状态
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 定时器刷新

        #region 刷新图片
        /// <summary>
        /// 刷新记录在日志里面的图片
        /// </summary>
        /// <param name="stateInfo"></param>
        public void uCStateWork_ImageInfo_event(StateInfo stateInfo)
        {
            try
            {
                int noCamera = stateInfo.NoCameraImage;
                int pos = stateInfo.PosCameraImage;
                string path = stateInfo.PathImage;

                switch (noCamera)
                {
                    case 1:
                        g_UCDisplayCamera1.LoadLocalImage(path);
                        break;

                    case 2:
                        g_UCDisplayCamera2.LoadLocalImage(path);
                        break;

                    case 3:
                        g_UCDisplayCamera3.LoadLocalImage(path);
                        break;

                    case 4:
                        g_UCDisplayCamera4.LoadLocalImage(path);
                        break;

                    case 5:
                        g_UCDisplayCamera5.LoadLocalImage(path);
                        break;

                    case 6:
                        g_UCDisplayCamera6.LoadLocalImage(path);
                        break;

                    case 7:
                        g_UCDisplayCamera7.LoadLocalImage(path);
                        break;

                    case 8:
                        g_UCDisplayCamera8.LoadLocalImage(path);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 刷新图片

        #region 软件运行状态
        public  void ShowStateRun(Visibility visible)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    g_LblStateRun.Visibility = visible;
                }));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 软件运行状态

        #region State
        /// <summary>
        /// 通用state
        /// </summary>
        /// <param name="info"></param>
        public static void ShowState(string info)
        {
            try
            {
                if (info == "")
                {
                    return;
                }
                if (!ParStateAndAlarm.P_I.BlAutoShow)
                {
                    ParStateAndAlarm.P_I.BlShowMain = true;
                }

                if (ParStateAndAlarm.P_I.BlShowMain)
                {
                    g_UCStateWork.AddInfo(info);
                }
                if (ParStateAndAlarm.P_I.BlAutoShow)
                {
                    if (WinStateAndAlarm.GetWinInst() != null)
                    {
                        WinStateAndAlarm.GetWinInst().AddStateInfo(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinInitMain", ex);
            }            
        }

        /// <summary>
        /// 相机日志
        /// </summary>
        /// <param name="info"></param>
        public static void ShowState_Cam(string info)
        {
            try
            {
                if (info == "")
                {
                    return;
                }
                if (!ParStateAndAlarm.P_I.BlAutoShow)
                {
                    ParStateAndAlarm.P_I.BlShowMain = true;
                }

                if (ParStateAndAlarm.P_I.BlShowMain)
                {
                    g_UCStateWork.AddInfo(info, TypeLog_enum.Cam);
                }
                if (ParStateAndAlarm.P_I.BlAutoShow)
                {
                    if (WinStateAndAlarm.GetWinInst() != null)
                    {
                        WinStateAndAlarm.GetWinInst().AddStateInfo(info, TypeLog_enum.Cam);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinInitMain", ex);
            }
        }

        /// <summary>
        /// PLC日志
        /// </summary>
        /// <param name="info"></param>
        public static void ShowState_PLC(string info)
        {
            if (info == "")
            {
                return;
            }
            if (!ParStateAndAlarm.P_I.BlAutoShow)
            {
                ParStateAndAlarm.P_I.BlShowMain = true;
            }

            if (ParStateAndAlarm.P_I.BlShowMain)
            {
                g_UCStateWork.AddInfo(info, TypeLog_enum.PLC);
            }
            if (ParStateAndAlarm.P_I.BlAutoShow)
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    WinStateAndAlarm.GetWinInst().AddStateInfo(info, TypeLog_enum.PLC);
                }
            }
        }


        /// <summary>
        /// Robot日志
        /// </summary>
        /// <param name="info"></param>
        public static void ShowState_Robot(string info)
        {
            if (info == "")
            {
                return;
            }
            if (!ParStateAndAlarm.P_I.BlAutoShow)
            {
                ParStateAndAlarm.P_I.BlShowMain = true;
            }

            if (ParStateAndAlarm.P_I.BlShowMain)
            {
                g_UCStateWork.AddInfo(info, TypeLog_enum.Robot);
            }
            if (ParStateAndAlarm.P_I.BlAutoShow)
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    WinStateAndAlarm.GetWinInst().AddStateInfo(info, TypeLog_enum.Robot);
                }
            }
        }
        #endregion State

        #region Alarm
        /// <summary>
        /// 将报警信息显示出来，同时显示在状态区域
        /// </summary>
        /// <param name="alarm"></param>
        public static void ShowAlarm(string alarm)
        {
            if (alarm == "")
            {
                return;
            }
            if (!ParStateAndAlarm.P_I.BlAutoShow)
            {
                ParStateAndAlarm.P_I.BlShowMain = true;
            }
            if (ParStateAndAlarm.P_I.BlShowMain)
            {
                g_UCStateWork.AddInfo(alarm);
                g_UCAlarm.AddInfo(alarm);
            }

            if (ParStateAndAlarm.P_I.BlAutoShow)
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    WinStateAndAlarm.GetWinInst().AddAlarmInfo(alarm);
                }
            }
        }

        /// <summary>
        /// PLC日志
        /// </summary>
        /// <param name="alarm"></param>
        public static void ShowAlarm_PLC(string alarm)
        {
            if (alarm == "")
            {
                return;
            }
            if (!ParStateAndAlarm.P_I.BlAutoShow)
            {
                ParStateAndAlarm.P_I.BlShowMain = true;
            }
            if (ParStateAndAlarm.P_I.BlShowMain)
            {
                g_UCStateWork.AddInfo(alarm, TypeLog_enum.PLC);
                g_UCAlarm.AddInfo(alarm, TypeLog_enum.PLC);
            }

            if (ParStateAndAlarm.P_I.BlAutoShow)
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    WinStateAndAlarm.GetWinInst().AddAlarmInfo(alarm, TypeLog_enum.PLC);
                }
            }
        }

        /// <summary>
        /// Robot日志
        /// </summary>
        /// <param name="alarm"></param>
        public static void ShowAlarm_Robot(string alarm)
        {
            if (alarm == "")
            {
                return;
            }
            if (!ParStateAndAlarm.P_I.BlAutoShow)
            {
                ParStateAndAlarm.P_I.BlShowMain = true;
            }
            if (ParStateAndAlarm.P_I.BlShowMain)
            {
                g_UCStateWork.AddInfo(alarm, TypeLog_enum.Robot);
                g_UCAlarm.AddInfo(alarm, TypeLog_enum.Robot);
            }

            if (ParStateAndAlarm.P_I.BlAutoShow)
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    WinStateAndAlarm.GetWinInst().AddAlarmInfo(alarm, TypeLog_enum.Robot);
                }
            }
        }
        #endregion Alarm

        #region 显示结果
        /// <summary>
        /// 运行数据列表初始化
        /// </summary>
        public virtual void Init_MainResult()
        {
            try
            {
                if(g_UCDisplayMainResult==null)
                {
                    return;
                }
                for (int i = 0; i < ParCameraWork.NumCamera; i++)
                {
                    g_UCDisplayMainResult.Init("相机" + (i + 1).ToString());
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示结果

        #region 产品参数
        /// <summary>
        /// 显示换型之后的型号名称
        /// </summary>
        public void ShowParProduct()
        {
            try
            {
                g_UCParProduct.ShowModelName_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion  产品参数

        #region 显示基方法
        public void ShowLabel_Invoke(Label lbl, string str, string color)
        {
            try
            {
                LabelStr2Action inst = new LabelStr2Action(ShowLabel);
                this.Dispatcher.Invoke(inst, lbl, str, color);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void ShowLabel(Label lbl, string str, string strColor)
        {
            try
            {
                if (strColor == "red")
                {
                    lbl.Foreground = Brushes.Red;
                }
                else
                {
                    lbl.Foreground = Brushes.Blue;
                }
                lbl.Content = str;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void ShowLabel_Invoke(Label lbl, string str)
        {
            try
            {
                LabelStrAction inst = new LabelStrAction(ShowLabel);
                this.Dispatcher.Invoke(inst, lbl, str);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void ShowLabel(Label lbl, string str)
        {
            try
            {
                lbl.Foreground = Brushes.Blue;
                lbl.Content = str;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示基方法

        #region WinError
        /// <summary>
        /// 显示报警窗体
        /// </summary>
        /// <param name="str"></param>
        public void ShowWinError_Invoke(string str)
        {
            try
            {
                this.Dispatcher.Invoke(new StrAction(ShowWinError), str);
            }
            catch (Exception ex)
            {

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

            }
        }
        #endregion WinError
    }
}
