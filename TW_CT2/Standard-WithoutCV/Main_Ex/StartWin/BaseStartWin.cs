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

namespace Main_EX
{
    /// <summary>
    /// 启动窗口
    /// </summary>
    public class BaseStartWin : Window
    {
        #region 定义
        //bool
        public bool g_BlFinishOthers = false;

        public bool g_BlFinishCamera = false;

        public bool g_BlFinishComprehensive1 = false;
        public bool g_BlFinishComprehensive2 = false;
        public bool g_BlFinishComprehensive3 = false;
        public bool g_BlFinishComprehensive4 = false;
        public bool g_BlFinishComprehensive5 = false;
        public bool g_BlFinishComprehensive6 = false;
        public bool g_BlFinishComprehensive7 = false;
        public bool g_BlFinishComprehensive8 = false;

        public string NameClass = "BaseStartWin";
        #endregion 定义

        #region 校准文件
        /// <summary>
        /// 读取校准文件
        /// </summary>
        public void ReadCalib()
        {
            try
            {
                //Calib
                ParCalibWorld.V_I.ReadIniAmp();  //读取放大系数

                #region 机器人校准参数
                ParCalibRobot1.P_I.ReadIniCalib();
                ParCalibRobot2.P_I.ReadIniCalib();
                ParCalibRobot3.P_I.ReadIniCalib();
                ParCalibRobot4.P_I.ReadIniCalib();
                #endregion 机器人校准参数
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 校准文件

        #region 辅助绘图
        /// <summary>
        /// 读取辅助绘图
        /// </summary>
        public void ReadAssistantSharpe()
        {
            try
            {
                ParAssistantSharpe1.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 2)
                {
                    return;
                }
                ParAssistantSharpe2.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 3)
                {
                    return;
                }
                ParAssistantSharpe3.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 4)
                {
                    return;
                }
                ParAssistantSharpe4.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 5)
                {
                    return;
                }
                ParAssistantSharpe5.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 6)
                {
                    return;
                }
                ParAssistantSharpe6.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 7)
                {
                    return;
                }
                ParAssistantSharpe7.P_I.ReadIniPar();
                if (ParCameraWork.NumCamera < 8)
                {
                    return;
                }
                ParAssistantSharpe8.P_I.ReadIniPar();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 辅助绘图

        #region 相机综合设置参数
        /// <summary>
        /// 相机综合设置参数
        /// </summary>
        public void Init_Comprehensive()
        {
            try
            {
                new Task(Init_Comprehensive1).Start();

                new Task(Init_Comprehensive2).Start();

                new Task(Init_Comprehensive3).Start();

                new Task(Init_Comprehensive4).Start();

                new Task(Init_Comprehensive5).Start();

                new Task(Init_Comprehensive6).Start();

                new Task(Init_Comprehensive7).Start();

                new Task(Init_Comprehensive8).Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 相机1综合设置
        /// </summary>
        void Init_Comprehensive1()
        {
            try
            {
                ParComprehensive1.P_I.ReadXmlXDoc();//综合处理参数               
            }
            catch (Exception)
            {

            }
            finally
            {
                g_BlFinishComprehensive1 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }
        /// <summary>
        /// 相机2综合设置
        /// </summary>
        void Init_Comprehensive2()
        {
            try
            {
                if (ParCameraWork.NumCamera < 2)
                {
                    return;
                }
                ParComprehensive2.P_I.ReadXmlXDoc();//综合处理参数               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("StartUpWindow", ex);
            }
            finally
            {
                g_BlFinishComprehensive2 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }
        /// <summary>
        /// 相机3综合设置
        /// </summary>
        void Init_Comprehensive3()
        {
            try
            {
                if (ParCameraWork.NumCamera < 3)
                {
                    return;
                }
                ParComprehensive3.P_I.ReadXmlXDoc();//综合处理参数           
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_BlFinishComprehensive3 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }
        /// <summary>
        /// 相机4综合设置
        /// </summary>
        void Init_Comprehensive4()
        {
            try
            {
                if (ParCameraWork.NumCamera < 4)
                {
                    return;
                }
                ParComprehensive4.P_I.ReadXmlXDoc();//综合处理参数              
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_BlFinishComprehensive4 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }
        /// <summary>
        /// 相机5综合设置
        /// </summary>
        void Init_Comprehensive5()
        {
            try
            {
                if (ParCameraWork.NumCamera < 5)
                {
                    return;
                }
                ParComprehensive5.P_I.ReadXmlXDoc();//综合处理参数              
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_BlFinishComprehensive5 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }
        /// <summary>
        /// 相机6综合设置
        /// </summary>
        void Init_Comprehensive6()
        {
            try
            {
                if (ParCameraWork.NumCamera < 6)
                {
                    return;
                }
                ParComprehensive6.P_I.ReadXmlXDoc();//综合处理参数               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_BlFinishComprehensive6 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }

        /// <summary>
        /// 相机7综合设置
        /// </summary>
        void Init_Comprehensive7()
        {
            try
            {
                if (ParCameraWork.NumCamera < 7)
                {
                    return;
                }
                ParComprehensive7.P_I.ReadXmlXDoc();//综合处理参数               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_BlFinishComprehensive7 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }

        /// <summary>
        /// 相机8综合设置
        /// </summary>
        void Init_Comprehensive8()
        {
            try
            {
                if (ParCameraWork.NumCamera < 8)
                {
                    return;
                }
                ParComprehensive8.P_I.ReadXmlXDoc();//综合处理参数               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_BlFinishComprehensive8 = true;
                FinishInit();
                StartInitTemp();//调用初始化加载模板
            }
        }
        #endregion 相机综合设置参数   

        #region 通信 

        /// <summary>
        /// 初始化PLC
        /// </summary>
        public void Init_PLC()
        {
            try
            {
                //读取PLC相关参数,PLC在主界面打开
                ParSetPLC.P_I.ReadIni();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 初始化Robot 
        /// </summary>
        public void Init_Robot()
        {
            try
            {
                ParSetRobot.P_I.ReadIniPar(); //读取机器人配置文件  
                LogicRobot.L_I.Init();

                ParSetRobot2.P_I.ReadIniPar(); //读取机器人2配置文件  
                LogicRobot2.L_I.Init();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// ComInterface 
        /// </summary>
        public void Init_ComInterface()
        {
            try
            {
                ParComInterface.P_I.ReadIniStr(); //读取机器人配置文件    
                if (ParComInterface.P_I.TypeComInterface_e != TypeComInterface_enum.Null)
                {
                    LogicComInterface.L_I.OpenPort();//打开机器人
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 通信

        #region 加载模板
        /// <summary>
        /// 加载模板
        /// </summary>
        public void StartInitTemp()
        {
            try
            {
                if (g_BlFinishComprehensive1
                && g_BlFinishComprehensive2
                && g_BlFinishComprehensive3
                && g_BlFinishComprehensive4
                && g_BlFinishComprehensive5
                && g_BlFinishComprehensive6
                && g_BlFinishComprehensive7
                && g_BlFinishComprehensive8)
                {
                    CreateTemp();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 创建模板ID
        /// </summary>
        public void CreateTemp()
        {
            string cellError = "";

            try
            {
                new Task(new Action(() =>
                {
                    DealComprehensive1.D_I.CreateTemplate(out cellError);
                    if (ParCameraWork.NumCamera > 1)
                    {
                        DealComprehensive2.D_I.CreateTemplate(out cellError);
                    }
                    if (ParCameraWork.NumCamera > 2)
                    {
                        DealComprehensive3.D_I.CreateTemplate(out cellError);
                    }
                    if (ParCameraWork.NumCamera > 3)
                    {
                        DealComprehensive4.D_I.CreateTemplate(out cellError);
                    }
                    if (ParCameraWork.NumCamera > 4)
                    {
                        DealComprehensive5.D_I.CreateTemplate(out cellError);
                    }
                    if (ParCameraWork.NumCamera > 5)
                    {
                        DealComprehensive6.D_I.CreateTemplate(out cellError);
                    }
                    if (ParCameraWork.NumCamera > 6)
                    {
                        DealComprehensive7.D_I.CreateTemplate(out cellError);
                    }
                    if (ParCameraWork.NumCamera > 7)
                    {
                        DealComprehensive8.D_I.CreateTemplate(out cellError);
                    }

                    WinInitMain.BlFinishTemp = true;//模板加载完成
                })).Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 加载模板


        /// <summary>
        /// 加载结束
        /// </summary>
        public virtual void FinishInit()
        {

        }
    }
}
