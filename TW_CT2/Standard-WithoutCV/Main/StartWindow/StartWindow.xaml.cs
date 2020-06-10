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
using System.Windows.Shapes;
using Camera;
using System.Threading;
using System.Threading.Tasks;
using Common;
using DealRobot;
using DealFile;
using DealComprehensive;
using SetPar;
using System.IO;
using ParComprehensive;
using DealConfigFile;
using DealPLC;
using DealCalibrate;
using DealComInterface;
using BasicClass;
using DealDisplay;
using BasicDisplay;
using System.Diagnostics;
using DealImageProcess;
using DealMedia;
using DealHelp;
using DealLog;
using DealIO;
using Main_EX;
using StationDataManager;

namespace Main
{
    /// <summary>
    /// StartUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartUpWindow : BaseStartWin
    {        
        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public StartUpWindow()
        {
            InitializeComponent();
            
            HideImage();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetPriority();//设置程序优先级位高
            Init();
        }

        /// <summary>
        /// 设置程序优先级
        /// </summary>
        void SetPriority()
        {
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
      
        /// <summary>
        /// 初始化加载
        /// </summary>
        void Init()
        {
            try
            {
                #region 初始化配置参数
                Init_ConfigPar();//读取配置参数

                StationDataMngr.instance.read_station_data();
                new Task(Init_SetPar).Start();  
                #endregion 初始化配置参数

                //Others
                new Task(Init_Others).Start();

                //Camera
                new Task(OpenCamera).Start();

                //相机综合设置
                Init_Comprehensive();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 读取配置文件
        //配置参数
        void Init_ConfigPar()
        {
            ParCameraWork.P_I.ReadIniNumCamera();//读取相机个数,要先读，后面的文件都会用到 

            ParCameraWork.P_I.ReadIniPar();//相机工作文件  
            ParManageConfigPar.P_I.ReadIniPathConfigPar();//读取当前配置文件路径
            ParConfigPar.P_I.ReadIniConfigPar();//读取产品参数        
            ParSetWork.P_I.ReadIniPar(); //工作运行设置文件
                                
            ParSetDisplay.P_I.ReadIniPar();//显示设置           
            ReadCalib();      //读取校准文件     
        }

        /// <summary>
        /// 系统参数
        /// </summary>
        void Init_SetPar()
        {            
            ParSetLogin.P_I.ReadIniPar();//登陆权限设置
            new Task(new Action(()=>
            {
                ReadAssistantSharpe();//辅助绘图
                ParVoice.P_I.ReadParIni();//读取声音报警
                ParDelFolder.P_I.ReadIniPar();//删除文件设定  
                ParHardDisk.P_I.ReadIniPar();//存储空间监控
                ParMemory.P_I.ReadIniPar();//读取内存设置

            })).Start();
            
            //设定日志
            ParSetLog.P_I.ReadParIni();
            ParStateAndAlarm.P_I.ReadIni();
        }
        #endregion 读取配置文件

        #region 打开相机
        /// <summary>
        /// 打开相机
        /// </summary>
        void OpenCamera()
        {
            try
            {
                ParCamera1.P_I.ReadIni();//相机参数
                Camera1.C_I.Init(ParCamera1.P_I);
                if (Camera1.C_I.OpenCamera())
                {
                    ParCamera1.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 2)
                {
                    return;
                }
                ParCamera2.P_I.ReadIni();//相机参数
                Camera2.C_I.Init(ParCamera2.P_I);
                if (Camera2.C_I.OpenCamera())
                {
                    ParCamera2.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 3)
                {
                    return;
                }
                ParCamera3.P_I.ReadIni();//相机参数
                Camera3.C_I.Init(ParCamera3.P_I);
                if (Camera3.C_I.OpenCamera())
                {
                    ParCamera3.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 4)
                {
                    return;
                }
                ParCamera4.P_I.ReadIni();//相机参数
                Camera4.C_I.Init(ParCamera4.P_I);
                if (Camera4.C_I.OpenCamera())
                {
                    ParCamera4.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 5)
                {
                    return;
                }
                ParCamera5.P_I.ReadIni();//相机参数
                Camera5.C_I.Init(ParCamera5.P_I);
                if (Camera5.C_I.OpenCamera())
                {
                    ParCamera5.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 6)
                {
                    return;
                }
                ParCamera6.P_I.ReadIni();//相机参数
                Camera6.C_I.Init(ParCamera6.P_I);
                if (Camera6.C_I.OpenCamera())
                {
                    ParCamera6.P_I.BlOpenCamera = true;
                }
                ParCamera6.P_I.BlOpenCamera = true;
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 7)
                {
                    return;
                }
                ParCamera7.P_I.ReadIni();//相机参数
                Camera7.C_I.Init(ParCamera7.P_I);
                if (Camera7.C_I.OpenCamera())
                {
                    ParCamera7.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 8)
                {
                    return;
                }
                ParCamera8.P_I.ReadIni();//相机参数
                Camera8.C_I.Init(ParCamera8.P_I);
                if (Camera8.C_I.OpenCamera())
                {
                    ParCamera8.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                    {
                        WinError.GetWinInst().ShowError("相机加载报错:" + ex.Message);
                    }));
            }
            finally
            {
                if (RegeditCamera.R_I.BlOffLineCamera)
                {
                    ParCamera1.P_I.BlOpenCamera = true;
                    ParCamera2.P_I.BlOpenCamera = true;
                    ParCamera3.P_I.BlOpenCamera = true;
                    ParCamera4.P_I.BlOpenCamera = true;
                    ParCamera5.P_I.BlOpenCamera = true;
                    ParCamera6.P_I.BlOpenCamera = true;
                    ParCamera7.P_I.BlOpenCamera = true;
                    ParCamera8.P_I.BlOpenCamera = true;
                }
                g_BlFinishCamera = true;
                FinishInit();
            }
        }
        #endregion 打开相机

        #region 初始化其他
        /// <summary>
        /// 初始化其他设置
        /// </summary>
        void Init_Others()
        {
            //初始化通信
            Init_Communicate();
            //关于
            FunAbout.F_I.CopySoftLog_DebugRelease();
            g_BlFinishOthers = true;
            FinishInit();
        }

        #region 通信
        void Init_Communicate()
        {
            //仅仅只是读取机器人参数，不打开机器人
            Init_Robot();

            //PLC
            Init_PLC();

            //外部IO
            ParDIO.P_I.ReadIniPar();

            //ComInterface
            Task taskComInterface = new Task(Init_ComInterface);
            taskComInterface.Start();
        }
        #endregion 通信

        #region  注册dLL
        void RegeditDLL()
        {
            try
            {

                //Process p = new Process();
                //p.StartInfo.FileName = "Regsvr32.exe"; 
                //p.StartInfo.Arguments = "/s C:\\DllTest.dll";
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion  注册dLL
        #endregion 初始化其他

        /// <summary>
        /// 加载完成
        /// </summary>
        public override void FinishInit()
        {            
            if (g_BlFinishCamera
                && g_BlFinishComprehensive1
                && g_BlFinishComprehensive2
                && g_BlFinishComprehensive3
                && g_BlFinishComprehensive4
                && g_BlFinishComprehensive5
                && g_BlFinishComprehensive6
                && g_BlFinishComprehensive7
                && g_BlFinishComprehensive8
                && g_BlFinishOthers)
            {
                //通知主线程自己已经启动完毕
                Program.s_mre.Set();
            }
        }

        #region 调试状态隐藏图像
        void HideImage()
        {
            try
            {
                string path = new DirectoryInfo("../").FullName;
               
                //时间
                if (path.Contains("bin")
                    && path.Contains("Standard"))
                {
                    imStart.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 调试状态隐藏图像
    }
}
