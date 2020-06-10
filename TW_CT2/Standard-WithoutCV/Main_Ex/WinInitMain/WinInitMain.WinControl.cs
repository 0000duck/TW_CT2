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
using System.Drawing;
using DealRobot;
using DealPLC;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using DealComprehensive;
using SetPar;
using SetComprehensive;
using DealConfigFile;
using BasicClass;
using DealComInterface;
using ParComprehensive;
using DealDisplay;
using BasicDisplay;
using DealTool;
using DealLog;
using DealMedia;
using DealHelp;
using DealIO;
using System.Reflection;
using BasicComprehensive;
using DealAlgorithm;
using DealCalibrate;
using DealCommunication;
using DealDraw;
using DealGeometry;
using DealGrabImage;
using DealImageProcess;
using DealInOutput;
using DealMath;
using DealResult;
using DealWorkFlow;
using DealFile;
using System.Windows.Controls.Primitives;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 定义

        //完成初始化相机综合设置
        public bool BlInitComprehensiveWin = false;
        public bool BlInitComprehensiveTempWin = false;

        bool BlFinishAllInit = false;//完成所有的初始化

        WinComprehensiveFull WinComprehensiveFull_I
        {
            get
            {
                return WinComprehensiveFull.GetWinInst();
            }
        }

        //控件
        public Grid g_GdDisplay = null;
        public Grid g_GdCamera = null;
        public Grid g_GdInfo = null;
        public Grid g_GdMenu = null;

        public MenuItem g_CmiMaxWin = null;

        //相机综合设置
        public MenuItem g_CmiCamera1 = null;
        public MenuItem g_CmiCamera2 = null;
        public MenuItem g_CmiCamera3 = null;
        public MenuItem g_CmiCamera4 = null;
        public MenuItem g_CmiCamera5 = null;
        public MenuItem g_CmiCamera6 = null;
        public MenuItem g_CmiCamera7 = null;
        public MenuItem g_CmiCamera8 = null;
        public MenuItem g_CimSetCameraPar = null;
        public MenuItem g_CimCameraWork = null;
        public MenuItem g_CimDisplayImage = null;


        //配置参数
        public MenuItem g_CmiConfigPar = null;
        public MenuItem g_CimAdjust = null;
        public MenuItem g_CimStd = null;
        public MenuItem g_CimTypeWork = null;
        public MenuItem g_CimManageConfigPar = null;

        public MenuItem g_CimNewModel = null;
        public MenuItem g_CimChangeModel = null;

        //通信
        public MenuItem g_CmiPLC = null;
        public MenuItem g_CmiRobot = null;
        public MenuItem g_CmiRobot2 = null;
        public MenuItem g_CmiComInterface = null;
        public MenuItem g_CmiIO = null;

        //系统设置
        public MenuItem g_CimSetLogin = null;
        public MenuItem g_CmiFolder = null;
        public MenuItem g_CmiMemory = null;
        public MenuItem g_CmiPathRoot = null;
        public MenuItem g_CmiMonitorSpace = null;
        public MenuItem g_CmiClearSpace = null;
        public MenuItem g_CmiRecover = null;

        //手动运行
        public MenuItem g_CmiManualPC = null;
        public MenuItem g_CmiManualPLC = null;
        public MenuItem g_CmiManualComInterface = null;
        public MenuItem g_CmiManualRobot = null;
        public MenuItem g_CmiManualRobot2 = null;

        //离线模式
        public MenuItem g_CmiOffline = null;
        public MenuItem g_CmiPLCOffline = null;
        public MenuItem g_CmiRobotOffline = null;
        public MenuItem g_CmiComPortOffline = null;
        public MenuItem g_CmiCameraOffline = null;

        //登陆
        public MenuItem g_CimLogin = null;
        public System.Windows.Controls.Image g_ImLogin = null;
        public Label g_LbLogin = null;
        public Popup g_PPLogin = null;//登录弹框
        public UCLogin g_UCLogin = null;//登录控件


        //软件状态
        public Popup g_PPState = null;//状态弹框
        public UCStateSoft g_UCStateSoft = null; //软件状态
        #endregion 定义

        #region 事件注册
        /// <summary>
        /// 注册事件
        /// </summary>
        public void Event_Init()
        {
            LoginEvent_PLC();
            LoginEvent_Robot();
            LoginEvent_ComInterface();
            LoginEvent_Others();
            LoginEvent_ShowInfo();
            LoginEvent_CIM();

            LoginEvent_Camera();

            LoginEvent_Monitor();

            LoginEvent_Login();
            LoginEvent_SoftState();
        }

        /// <summary>
        /// 注册通用端口事件
        /// </summary>
        public virtual void LoginEvent_ComInterface()
        {

        }

        /// <summary>
        /// 注册CIM事件
        /// </summary>

        public virtual void LoginEvent_CIM()
        {

        }

        /// <summary>
        /// 其他
        /// </summary>
        public virtual void LoginEvent_Others()
        {

        }
        #endregion 事件注册

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        public new void Init()
        {
            //图像处理初始化
            Init_ImageProcess();

            //初始化文件夹
            Init_DictionaryFiles();
            
            //监控
            Init_Monitor();

            //初始化显示
            Init_Display();

            //IO模块
            Init_IO();

            //初始化CIM
            Init_CIM();

            //Others
            Init_Others();

            //Custum
            Init_Custom();

            //延迟
            Thread.Sleep(300);
            //打开相机综合设置窗体
            this.Dispatcher.Invoke(new Action(InitWinComprehensive));

            Thread.Sleep(1000);
            ////初始化报警窗口
            this.Dispatcher.Invoke(new Action(() =>
            {
                WinError.GetWinInst().BlInit = true;
                WinError.GetWinInst().ShowError_Invoke();

                //初始化显示
                Init_ShowInfo();

                //创建相机图像显示窗口
                CreateUIDisplay(g_GdCamera);

                new Task(new Action(() =>
                {
                    //初始化相机
                    Init_Camera();

                    //触发，处理，输出结果初始化
                    Init_TrrigerDealResult();

                    //通信
                    Init_Communicate();

                })).Start();
            }));

            //求取版本号
            new Task(RecordVersion).Start();
        }

        #region 主窗体相关
        /// <summary>
        /// 初始化窗体尺寸
        /// </summary>
        public void InitWinSize()
        {
            try
            {
                if (ParSetDisplay.P_I.TypeScreen_e == TypeScreen_enum.S800
                    ||ParSetDisplay.P_I.BlSlideWin)
                {
                    return;
                }
                if (RegeditMain_EX.R_I.Width_gdCamera != 0)
                {
                    g_GdDisplay.ColumnDefinitions[0].Width = new GridLength((int)RegeditMain_EX.R_I.Width_gdCamera, GridUnitType.Star);
                }
                if (RegeditMain_EX.R_I.Width_gdInfo != 0)
                {
                    g_GdDisplay.ColumnDefinitions[2].Width = new GridLength((int)RegeditMain_EX.R_I.Width_gdInfo, GridUnitType.Star);
                }
                else
                {
                    g_GdDisplay.ColumnDefinitions[2].Width = new GridLength(420, GridUnitType.Pixel);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 分隔匡调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void gdsMain_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            try
            {
                if (g_GdCamera.ActualWidth != 0)
                {
                    RegeditMain_EX.R_I.Width_gdCamera = g_GdCamera.ActualWidth;
                }
                else
                {
                    RegeditMain_EX.R_I.Width_gdCamera = 1;
                }

                if (g_GdInfo == null)
                {
                    return;
                }
                if (g_GdInfo.ActualWidth != 0)
                {
                    RegeditMain_EX.R_I.Width_gdInfo = g_GdInfo.ActualWidth;
                }
                else
                {
                    RegeditMain_EX.R_I.Width_gdInfo = 1;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

            /// <summary>
        /// 主窗体取消最前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void muSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Topmost)
            {
                this.Topmost = false;
            }
        }

        /// <summary>
        /// 窗体尺寸变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BaseWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    RegeditMain_EX.R_I.BlMaxWin = true;
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    RegeditMain_EX.R_I.BlMaxWin = false;
                }
                g_CmiMaxWin.IsChecked = RegeditMain_EX.R_I.BlMaxWin;

                RegeditMain_EX.R_I.Width_Win = this.ActualWidth;
                RegeditMain_EX.R_I.Height_Win = this.ActualHeight;

                RegeditMain_EX.R_I.Width_gdCamera = g_GdCamera.ActualWidth;
                RegeditMain_EX.R_I.Height_gdCamera = g_GdCamera.ActualHeight;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #region 窗体最大化
        public void maxBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    RegeditMain_EX.R_I.BlMaxWin = true;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    RegeditMain_EX.R_I.BlMaxWin = false;
                }
                g_CmiMaxWin.IsChecked = RegeditMain_EX.R_I.BlMaxWin;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void MaxWinMain()
        {
            try
            {
                if (RegeditMain_EX.R_I.BlMaxWin)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                g_CmiMaxWin.IsChecked = RegeditMain_EX.R_I.BlMaxWin;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 窗体最大化

        #region 小窗体
        public void ShowSmallWin()
        {
            try
            {
                //根据分辨率确认窗体大小，小窗体
                if (ParSetDisplay.P_I.TypeScreen_e == TypeScreen_enum.S1024)
                {
                    this.Height = 760;
                    this.Width = 1024;
                    g_GdDisplay.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
                    g_GdDisplay.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                    g_GdMenu.ColumnDefinitions[0].Width = new GridLength(10, GridUnitType.Star);
                    g_GdMenu.ColumnDefinitions[2].Width = new GridLength(4.5, GridUnitType.Star);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 小窗体

        #region 调试状态取消主窗体TopMost
        public void TopFalse()
        {
            try
            {
                string path = new DirectoryInfo("../").FullName;
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo dir1 = dir.Parent;
                DirectoryInfo dir2 = dir1.Parent;
                DirectoryInfo dir3 = dir2.Parent;

                //时间
                if (path.Contains("bin")
                    && path.Contains("Standard"))
                {
                    this.Topmost = false;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 调试状态取消主窗体TopMost

        #region Popup处理
        /// <summary>
        /// 窗体的状态发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Minimized)
                {
                    g_BaseUCDisplaySum.HidePPResult();
                }
                else
                {
                    g_BaseUCDisplaySum.VisiblePPResult();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 窗体的位置发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WinInitMain_LocationChanged(object sender, EventArgs e)
        {
            try
            {
                if (g_BaseUCDisplaySum != null)
                {
                    g_BaseUCDisplaySum.MovePPResult();
                }             
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 主窗体被激活，隐藏相机综合设置的pp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WinInitMain_Activated(object sender, EventArgs e)
        {
            try
            {
                if (BlFinishAllInit)
                {
                    g_BaseUCDisplaySum.VisiblePPResult();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion Popup处理

        #endregion 主窗体相关

        #region 初始化相机综合设置窗体
        /// <summary>
        /// 预打开相机综合设置窗体
        /// </summary>
        public void InitWinComprehensive()
        {
            try
            {
                this.Topmost = true;

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinComprehensiveFull.GetWinInst().Show();
                    WinComprehensiveFull.GetWinInst().Hide();
                    //this.Topmost = false;
                    WinComprehensiveFull.GetWinInst().GetResultValueMult_event += new ExeAndGetResultValue_del(MainWindow_GetResultValueMult_event);
                    WinComprehensiveFull.GetWinInst().ActiveWinComp_event += new Action(MainWindow_ActiveWinComp_event);
                    WinComprehensiveFull.GetWinInst().CloseWin_event += new Action(WinInitMain_CloseWin_event);
                }
                else//最小窗体
                {
                    WinComprehensiveSmall.GetWinInst().Show();
                    WinComprehensiveSmall.GetWinInst().Hide();
                    //this.Topmost = false;
                    WinComprehensiveSmall.GetWinInst().GetResultValueMult_event += new ExeAndGetResultValue_del(MainWindow_GetResultValueMult_event);
                    WinComprehensiveSmall.GetWinInst().ActiveWinComp_event += new Action(MainWindow_ActiveWinComp_event);
                    WinComprehensiveSmall.GetWinInst().CloseWin_event += new Action(WinInitMain_CloseWin_event);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                BlInitComprehensiveWin = true;
                FinishInitWin();//结束初始化
            }
        }
        #endregion 初始化相机综合设置窗体

        #region 控件使能
        /// <summary>
        /// 初始化主界面显示
        /// </summary>
        public void Init_UIMainShow()
        {
            try
            {
                #region 相机综合设置
                if (ParCameraWork.NumCamera < 8)
                {
                    g_CmiCamera8.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 7)
                {
                    g_CmiCamera7.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 6)
                {
                    g_CmiCamera6.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 5)
                {
                    g_CmiCamera5.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 4)
                {
                    g_CmiCamera4.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 3)
                {
                    g_CmiCamera3.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 2)
                {
                    g_CmiCamera2.IsEnabled = false;
                }
                if (ParCameraWork.NumCamera < 1)
                {
                    g_CmiCamera1.IsEnabled = false;
                }
                #endregion 相机综合设置
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 控件使能


        /// <summary>
        /// 通信
        /// </summary>
        public void Init_Communicate()
        {
            //PLC 
            new Task(new Action(Init_PLC)).Start();

            //Robot
            new Task(new Action(Init_Robot)).Start();

            //相机外触发监控
            Init_ExCameraTrigger();
        }


        /// <summary>
        /// 结束初始化
        /// </summary>
        public void FinishInitWin()
        {
            try
            {
                if (BlInitComprehensiveTempWin
                    && BlInitComprehensiveWin)
                {
                    BlFinishAllInit = true;//完成所有的初始化

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Topmost = false;
                        //菜单的Grid使能
                        g_GdMenu.IsEnabled = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 配置参数
        /// <summary>
        /// 配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiConfigPar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool blNew = false;
                WinConfigPar winConfigPar = WinConfigPar.GetWinInst(out blNew);
                //注册事件
                if (blNew)
                {
                    LoginEvent_ConfigPar(winConfigPar);
                }
                //显示窗体
                winConfigPar.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiConfigPar" + g_CmiConfigPar.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void LoginEvent_ConfigPar(WinConfigPar winConfigPar)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 打开调整值窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimAdjust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinSetAdjustWorkFull winSetAdjustWork = WinSetAdjustWorkFull.GetWinInst();
                    winSetAdjustWork.Show();
                }
                else
                {
                    WinSetAdjustWorkSmall winSetAdjustWork = WinSetAdjustWorkSmall.GetWinInst();
                    winSetAdjustWork.Show();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimAdjust" + g_CimAdjust.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void CimRobotPoints_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinSetRobotPoints winSetAdjustWork = WinSetRobotPoints.GetWinInst();
                    winSetAdjustWork.Show();
                }
                else
                {
                    WinSetRobotPoints winSetAdjustWork = WinSetRobotPoints.GetWinInst();
                    winSetAdjustWork.Show();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimAdjust" + g_CimAdjust.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void CimRobotAdj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinSetRobotAdj winSetAdjustWork = WinSetRobotAdj.GetWinInst();
                    winSetAdjustWork.Show();
                }
                else
                {
                    WinSetRobotAdj winSetAdjustWork = WinSetRobotAdj.GetWinInst();
                    winSetAdjustWork.Show();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimAdjust" + g_CimAdjust.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 设置基准值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimStd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinSetStdWork winSetStdWork = WinSetStdWork.GetWinInst();
                    winSetStdWork.Show();
                }
                else
                {
                    WinSetStdWorkSmall winSetStdWork = WinSetStdWorkSmall.GetWinInst();
                    winSetStdWork.Show();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimStd" + g_CimStd.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 打开工作模式设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimTypeWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                WinSetWorkType winSetWorkType = new WinSetWorkType();
                winSetWorkType.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cimTypeWork" + g_CimTypeWork.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #region 配置文件
        /// <summary>
        /// 打开配置文件管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimManageConfigPar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                WinManageConfigPar winManageConfigPar = new WinManageConfigPar();
                winManageConfigPar.DeleteModel_event += new FdBlStrAction_del(DelModel);//删除文件响应事件
                winManageConfigPar.NewModel_event += new FdBlAction_del(NewModel);//新建型号
                winManageConfigPar.ChangeModel_event += new Action(ChangeModel_event);//换型
                winManageConfigPar.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cimManageConfigPar" + g_CimManageConfigPar.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 新建型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimNewModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                WinSaveNewModel winSaveNewModel = WinSaveNewModel.GetWinInst();
                winSaveNewModel.NewModel_event += new FdBlAction_del(NewModel);
                winSaveNewModel.ChangeModel_event += new Action(ChangeModel_event);
                winSaveNewModel.Init();
                winSaveNewModel.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cimNewModel" + g_CimNewModel.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 切换型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimChangeModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                if (OpenFileDialog())
                {
                    new Task<bool>(ChangeModel).Start();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimChangeModel" + g_CimChangeModel.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //切换型号
        void ChangeModel_event()
        {
            new Task<bool>(ChangeModel).Start();
        }
        #endregion 型号相关

        /// <summary>
        /// 设置声音 报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimSetVoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WinSetVoice.GetWinInst().Show();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 配置参数

        #region 相机综合设置
        /// <summary>
        /// 相机1
        /// </summary>
        public void cmiCamera1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(1);
                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera1" + g_CmiCamera1.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(2);
                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera2" + g_CmiCamera2.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(3);

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera3" + g_CmiCamera3.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(4);
                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera4" + g_CmiCamera4.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(5);

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera5" + g_CmiCamera5.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(6);

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera6" + g_CmiCamera6.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(7);

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera7" + g_CmiCamera7.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiCamera8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenWinWinComprehensive(8);
                //按钮日志
                FunLogButton.P_I.AddInfo("cmiCamera8" + g_CmiCamera8.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 打开相机综合设置窗体
        /// </summary>
        public void OpenWinWinComprehensive(int noCameraNew)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                if (g_BaseUCDisplaySum.BlRealImage)
                {
                    StopReal();
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    int noCamera = WinComprehensiveFull_I.NoCamera;
                    if (noCamera != noCameraNew
                        && WinComprehensiveFull_I.Visibility == Visibility.Visible)
                    {
                        MessageBox.Show("请先关闭已打开的相机综合设置窗体" + noCamera.ToString());
                        return;
                    }
                    WinComprehensiveFull_I.Init(noCameraNew);
                    WinComprehensiveFull_I.ShowPar_Invoke();
                    WinComprehensiveFull_I.Visibility = Visibility.Visible;
                    WinComprehensiveFull_I.Focus();
                    WinComprehensiveFull_I.Topmost = true;

                    new Task(new Action(() =>
                    {
                        Thread.Sleep(1000);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            WinComprehensiveFull_I.Topmost = false;
                        }));

                    })).Start();
                }
                else
                {
                    int noCamera = WinComprehensiveSmall.GetWinInst().NoCamera;
                    if (noCamera != noCameraNew
                        && WinComprehensiveSmall.GetWinInst().Visibility == Visibility.Visible)
                    {
                        MessageBox.Show("请先关闭已打开的相机综合设置窗体" + noCamera.ToString());
                        return;
                    }
                    WinComprehensiveSmall.GetWinInst().Init(noCameraNew);
                    WinComprehensiveSmall.GetWinInst().ShowPar_Invoke();
                    WinComprehensiveSmall.GetWinInst().Visibility = Visibility.Visible;
                    WinComprehensiveSmall.GetWinInst().Focus();
                    WinComprehensiveSmall.GetWinInst().Topmost = true;

                    new Task(new Action(() =>
                    {
                        Thread.Sleep(1000);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            WinComprehensiveSmall.GetWinInst().Topmost = false;
                        }));

                    })).Start();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机综合设置窗体被激活
        /// </summary>
        public void MainWindow_ActiveWinComp_event()
        {
            try
            {
                if (BlFinishAllInit)//窗体预加载完成
                {
                    g_BaseUCDisplaySum.UnVisiblePPResult();//使控件不可见
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 相机综合设置关闭
        /// </summary>
        void WinInitMain_CloseWin_event()
        {
            if (BlFinishAllInit)//窗体预加载完成
            {
                g_BaseUCDisplaySum.VisiblePPResult();//使控件不可见
            }
        }


        /// <summary>
        /// 相机参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiSetCameraPar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                if (g_BaseUCDisplaySum.BlRealImage)
                {
                    StopReal();
                    return;
                }

                WinSetCamera.GetWinInst().Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiSetCameraPar" + g_CimSetCameraPar.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机拍摄次数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimCameraWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }
                if (g_BaseUCDisplaySum.BlRealImage)
                {
                    StopReal();
                    return;
                }

                WinCameraWork winCameraWork = new WinCameraWork();
                winCameraWork.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cimCameraWork" + g_CimCameraWork.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 图像显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimDisplayImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }
                if (g_BaseUCDisplaySum.BlRealImage)
                {
                    StopReal();
                    return;
                }

                WinSetDisplayImage winSetDisplayImage = new WinSetDisplayImage();
                winSetDisplayImage.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cimDisplayImage" + g_CimDisplayImage.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 相机综合设置

        #region 通信设定
        /// <summary>
        /// 设定PLC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiPLC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                bool blNew = false;
                WinSetPLC winSetPLC = WinSetPLC.GetWinInst(out blNew);
                winSetPLC.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiPLC" + g_CmiPLC.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        /// <summary>
        /// 设定机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiRobot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                WinSetRobot winSetRobot = new WinSetRobot();
                winSetRobot.ShowDialog();
                //再次读取参数配置,确保重新打开的时候，已经完成加载
                ParSetRobot.P_I.ReadIniPar();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiRobot" + g_CmiRobot.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 第二个机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiRobot2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                WinSetRobot2 winSetRobot = new WinSetRobot2();
                winSetRobot.ShowDialog();
                //再次读取参数配置,确保重新打开的时候，已经完成加载
                ParSetRobot2.P_I.ReadIniPar();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiRobot" + g_CmiRobot2.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 通用端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiComInterface_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                WinSetComInterface winSetComInterface = new WinSetComInterface();
                winSetComInterface.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiComInterface" + g_CmiComInterface.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //IO模块
        public void cmiIO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }
                WinDIO win = new WinDIO();
                win.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiIO" + g_CmiIO.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 通信设定

        #region 系统设置
        /// <summary>
        /// 登录权限设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cimSetLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                WinSetLogin winSetLogin = new WinSetLogin();
                winSetLogin.ShowDialog();
                SetDefaultLogin();//设定默认的登录权限

                //按钮日志
                FunLogButton.P_I.AddInfo("cimSetLogin" + g_CimSetLogin.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 设定默认的登录权限
        /// </summary>
        public void SetDefaultLogin()
        {
            try
            {
                //默认厂商权限
                if (RegeditLogin.R_I.BlManufacturer)
                {
                    Authority.Authority_e = Authority_enum.Manufacturer;
                    Login();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 运行&报警信息
        /// </summary>
        public void cmiStateAlarm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                bool blNew = false;
                WinStateAndAlarm win = WinStateAndAlarm.GetWinInst(out blNew);
                win.Show();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 文件删除设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                WinSetFolder winSetFolder = new WinSetFolder();
                winSetFolder.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiFolder" + g_CmiFolder.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 内存管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiMemory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }

                WinMemory winMemory = new WinMemory();
                winMemory.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiMemory" + g_CmiMemory.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 系统根目录
        /// </summary>
        public void cmiPathRoot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }
                WinPathRoot winPathRoot = new WinPathRoot();
                winPathRoot.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiPathRoot" + g_CmiPathRoot.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 设置存储空间监控
        /// </summary>
        public void cmiMonitorSpace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }
                WinHardDisk winHardDisk = new WinHardDisk();
                winHardDisk.ShowDialog();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiMonitorSpace" + g_CmiMonitorSpace.Header.ToString(), "Main窗体");

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 清理存储空间
        /// </summary>
        public void cmiClearSpace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                WinClearHardDisk.GetWinInst().Show();
                //按钮日志
                FunLogButton.P_I.AddInfo("cmiClearSpace" + g_CmiClearSpace.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 历史数据恢复
        /// </summary>
        public void cmiRecover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }
                bool blNew = false;
                WinRecover winRecover = WinRecover.GetWinInst(out blNew);
                winRecover.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiRecover" + g_CmiRecover.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 日志设置
        /// </summary>
        public void cmiLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }
                WinSetLog win = new WinSetLog();
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 系统设置

        #region 手动运行
        /// <summary>
        /// 模拟PC触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiManualPC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                TriggerPC();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiManualPC" + g_CmiManualPC.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        /// <summary>
        /// 模拟PLC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiManualPLC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }

                bool blNew = false;
                WinTrrigerPLC inst = WinTrrigerPLC.GetWinInst(out blNew);
                inst.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiManualPLC" + g_CmiManualPLC.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        /// <summary>
        /// 模拟通用端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiManualComInterface_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                WinTrrigerComInterface inst = new WinTrrigerComInterface();
                inst.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiManualComInterface" + g_CmiManualComInterface.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        /// <summary>
        /// 模拟机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiManualRobot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                bool blNew = false;
                WinTrrigerRobot winTrrigerRobot = WinTrrigerRobot.GetWinInst(out blNew);
                winTrrigerRobot.ManualRobot_event += new ManualRobot_del(winTrrigerRobot_ManualRobot_event);
                winTrrigerRobot.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiManualRobot" + g_CmiManualRobot.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiManualRobot2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!WorkerReturn())
                {
                    return;
                }
                bool blNew = false;
                WinTrrigerRobot2 winTrrigerRobot = WinTrrigerRobot2.GetWinInst(out blNew);
                winTrrigerRobot.ManualRobot_event += new ManualRobot_del(winTrrigerRobot_ManualRobot_event);
                winTrrigerRobot.Show();

                //按钮日志
                FunLogButton.P_I.AddInfo("cmiManualRobot" + g_CmiManualRobot2.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 机器人示教
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiTechRobot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool blNew = false;
                WinManualRobot winManualRobot = WinManualRobot.GetWinInst(out blNew);
                winManualRobot.Show();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 手动操作机器人
        /// </summary>
        /// <param name="manualRobot_e"></param>
        public void winTrrigerRobot_ManualRobot_event(ManualRobot_enum manualRobot_e)
        {
            try
            {
                switch (manualRobot_e)
                {
                    case ManualRobot_enum.ConfigPar://配置机器人参数
                        ConfigRobot_Task();
                        break;

                    case ManualRobot_enum.Close:
                        break;

                    case ManualRobot_enum.Restart:
                        break;

                    case ManualRobot_enum.Reset:
                        break;

                    case ManualRobot_enum.Home:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 手动发送机器人配置参数
        /// </summary>
        void winTrrigerRobot_ConfigRobot_event()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 重启机器人通信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiRestartRobot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!WorkerReturn())
                {
                    return;
                }
                new Task(new Action(RobotReStart)).Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        public void cmiRestartRobot2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!WorkerReturn())
                {
                    return;
                }
                new Task(new Action(Robot2ReStart)).Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 手动运行

        #region 工具
        /// <summary>
        /// 计算器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiCal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SysTool.S_I.OpenCal();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 打开记事本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SysTool.S_I.OpenTxt();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 绘图板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiPaint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SysTool.S_I.OpenPaint();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        public void cmiKeyboard_Click(object sender, RoutedEventArgs e)
        {
            SysTool.S_I.OpenKeyboard();
        }
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiCopyFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool blNew = false;
                WinCopyFile winCopyFile = WinCopyFile.GetWinInst(out blNew);
                winCopyFile.Show();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 重启网卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiRestartNet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WinRestartNet winRestartNet = WinRestartNet.GetWinInst();
                winRestartNet.Show();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 窗体最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiMaxWin_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RegeditMain_EX.R_I.BlMaxWin = true;
                this.WindowState = WindowState.Maximized;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiMaxWin_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                RegeditMain_EX.R_I.BlMaxWin = false;
                this.WindowState = WindowState.Normal;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 工具

        #region 离线模式
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiOffline_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {             
                if (g_CmiOffline.IsMouseOver)
                {
                    if (Authority.Authority_e == Authority_enum.Null
                    || Authority.Authority_e == Authority_enum.Worker)
                    {
                        g_CmiPLCOffline.IsEnabled = false;
                        g_CmiRobotOffline.IsEnabled = false;
                        g_CmiComPortOffline.IsEnabled = false;
                        g_CmiCameraOffline.IsEnabled = false;
                    }
                    else
                    {
                        g_CmiPLCOffline.IsEnabled = true;
                        g_CmiRobotOffline.IsEnabled = true;
                        g_CmiComPortOffline.IsEnabled = true;
                        g_CmiCameraOffline.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiCameraOffline_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
          
                if (g_CmiCameraOffline.IsMouseOver)
                {
                    //权限返回
                    if (!EngineerReturn())
                    {
                        return;
                    }
                    RegeditCamera.R_I.BlOffLineCamera = true;
                    ChangeColorOffLine();
                    ShowAlarm("相机设置为离线状态");

                    //按钮日志
                    FunLogButton.P_I.AddInfo("cmiCameraOffline" + g_CmiCameraOffline.Header.ToString() + "Checked", "Main窗体");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        public void cmiCameraOffline_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {            
                if (g_CmiCameraOffline.IsMouseOver)
                {
                    //权限返回
                    if (!EngineerReturn())
                    {
                        return;
                    }
                    RegeditCamera.R_I.BlOffLineCamera = false;
                    ChangeColorOffLine();
                    ShowAlarm("相机设置为离线状态");
                    //按钮日志
                    FunLogButton.P_I.AddInfo("cmiCameraOffline" + g_CmiCameraOffline.Header.ToString() + "Unchecked", "Main窗体");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// PLC离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiPLCOffline_Checked(object sender, RoutedEventArgs e)
        {
            try
            {           
                if (g_CmiPLCOffline.IsMouseOver)
                {
                    //权限返回
                    if (!EngineerReturn())
                    {
                        return;
                    }

                    RegeditPLC.R_I.BlOffLinePLC = true;
                    ChangeColorOffLine();
                    ShowAlarm("PLC设置为离线状态");
                    //按钮日志
                    FunLogButton.P_I.AddInfo("cmiPLCOffline" + g_CmiPLCOffline.Header.ToString() + "Checked", "Main窗体");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiPLCOffline_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {              
                if (g_CmiPLCOffline.IsMouseOver)
                {
                    //权限返回
                    if (!EngineerReturn())
                    {
                        return;
                    }
                    RegeditPLC.R_I.BlOffLinePLC = false;
                    ChangeColorOffLine();
                    ShowAlarm("PLC恢复在线状态");
                    //按钮日志
                    FunLogButton.P_I.AddInfo("cmiPLCOffline" + g_CmiPLCOffline.Header.ToString() + "Unchecked", "Main窗体");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 机器人处于离线状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmiRobotOffline_Checked(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (g_CmiRobotOffline.IsMouseOver)
                {
                    //权限返回
                    if (!EngineerReturn())
                    {
                        return;
                    }

                    RegeditRobot.R_I.BlOffLineRobot = true;
                    ChangeColorOffLine();
                    ShowAlarm("机器人设置为离线状态");

                    //按钮日志
                    FunLogButton.P_I.AddInfo("cmiRobotOffline" + g_CmiRobotOffline.Header.ToString() + "Checked", "Main窗体");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void cmiRobotOffline_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {        
                if (g_CmiRobotOffline.IsMouseOver)
                {
                    //权限返回
                    if (!EngineerReturn())
                    {
                        return;
                    }

                    RegeditRobot.R_I.BlOffLineRobot = false;
                    ChangeColorOffLine();
                    ShowAlarm("机器人恢复在线状态");

                    //按钮日志
                    FunLogButton.P_I.AddInfo("cmiRobotOffline" + g_CmiRobotOffline.Header.ToString() + "Unchecked", "Main窗体");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 改变菜单字体颜色
        /// </summary>
        public void ChangeColorOffLine()
        {
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC
                    || RegeditRobot.R_I.BlOffLineRobot
                    || RegeditMain_EX.R_I.BlOffLineComPort
                    || RegeditCamera.R_I.BlOffLineCamera)
                {
                    g_CmiOffline.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    g_CmiOffline.Foreground = System.Windows.Media.Brushes.Black;
                }
                g_CmiRobotOffline.IsChecked = RegeditRobot.R_I.BlOffLineRobot;
                g_CmiPLCOffline.IsChecked = RegeditPLC.R_I.BlOffLinePLC;
                g_CmiComPortOffline.IsChecked = RegeditMain_EX.R_I.BlOffLineComPort;
                g_CmiCameraOffline.IsChecked = RegeditCamera.R_I.BlOffLineCamera;

                if(RegeditCamera.R_I.BlOffLineCamera)
                {
                    ShowAlarm("相机设置为离线状态，相机未打开");
                }
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    ShowAlarm("PLC设置为离线状态，PLC通信未打开");
                }
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    ShowAlarm("机器人设置为离线状态，机器人通信未打开");
                }
                if (RegeditMain_EX.R_I.BlOffLineComPort)
                {
                    ShowAlarm("通用端口设置为离线状态，端口通信未打开");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 离线模式

        #region 帮助
        public void cmiAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WinAbout winAbout = new WinAbout();
                winAbout.Show();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 帮助

        #region 登陆权限判断
        /// <summary>
        /// 登录
        /// </summary>
        public void imLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //没有登录
                if (Authority.Authority_e == Authority_enum.Null)
                {
                    //WinLogin winLogin = new WinLogin();
                    //winLogin.ShowDialog();
                    g_PPLogin.IsOpen = true;
                    g_UCLogin.ShowPar_Invoke();

                    //判断是否登录
                    Login();

                    ShowState("打开登录界面");
                }
                else
                {
                    //退出登录
                    Logout();
                    ShowState("退出登录");
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("imLogin", "Main窗体登陆");

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 显示登陆权限
        /// </summary>
        void Login()
        {
            try
            {
                if (Authority.Authority_e != Authority_enum.Null)
                {
                    g_ImLogin.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Login.jpg"));
                }
                switch (Authority.Authority_e)
                {
                    case Authority_enum.Worker:
                        g_LbLogin.Content = "技术员";
                        break;
                    case Authority_enum.Engineer:
                        g_LbLogin.Content = "工程师";
                        break;
                    case Authority_enum.Manufacturer:
                        g_LbLogin.Content = "厂商";
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        void Logout_Invoke()
        {
            try
            {
                this.Dispatcher.Invoke(new Action(Logout));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        void Logout()
        {
            try
            {
                Authority.Authority_e = Authority_enum.Null;
                RegeditLogin.R_I.BlManufacturer = false;//退出默认厂商权限
                g_ImLogin.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Logout.jpg"));
                g_LbLogin.Content = "Logout";
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 无权限返回
        /// </summary>
        /// <returns></returns>
        public bool NullReturn()
        {
            if (Authority.Authority_e == Authority_enum.Null)
            {
                MessageBox.Show("需技术员及以上权限");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 技术员权限返回
        /// </summary>
        /// <returns></returns>
        public bool WorkerReturn()
        {
            if (Authority.Authority_e == Authority_enum.Null
                || Authority.Authority_e == Authority_enum.Worker)
            {
                MessageBox.Show("需工程师及以上权限");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 工程师权限返回
        /// </summary>
        /// <returns></returns>
        public bool EngineerReturn()
        {
            if (Authority.Authority_e != Authority_enum.Manufacturer)
            {
                MessageBox.Show("需厂商权限");
                return false;
            }
            return true;
        }
              

        /// <summary>
        /// 登录相关事件注册
        /// </summary>
        void LoginEvent_Login()
        {
            try
            {
                g_UCLogin.Close_event += G_UCLogin_Close_event;
            }
            catch (Exception ex)
            {

            }
        }
       
        /// <summary>
        /// 退出登录弹框
        /// </summary>
        private void G_UCLogin_Close_event()
        {
            try
            {
                g_PPLogin.IsOpen = false;
                Login();//重绘登录
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 权限判断

        #region 运行状态
        /// <summary>
        /// 运行状态相关事件注册
        /// </summary>
        void LoginEvent_SoftState()
        {
            try
            {
                g_UCStateSoft.Close_Event += G_UCStateSoft_Close_Event;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 退出状态设置
        /// </summary>
        private void G_UCStateSoft_Close_Event()
        {
            try
            {
                g_PPState.IsOpen = false;//弹框打开
                //更新状态
                ShowStateMachine();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 双击运行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lbStateMachine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //WinStateMachine win = new WinStateMachine();
                //win.ShowDialog();
                g_PPState.IsOpen = true;//弹框打开
                g_UCStateSoft.ShowPar_Invoke();

                //更新状态
                ShowStateMachine();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 运行状态

        

        #region 便捷操作
        /// <summary>
        /// 打开软件运行记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lblStateRun_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", ComValue.c_PathRecord);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 便捷操作
    }
}
