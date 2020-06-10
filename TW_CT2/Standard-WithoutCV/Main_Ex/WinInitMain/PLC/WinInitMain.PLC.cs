using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using HalconDotNet;
using System.Threading.Tasks;
using DealPLC;
using Common;
using DealRobot;
using System.IO;
using DealFile;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using DealComprehensive;
using SetPar;
using ParComprehensive;
using BasicClass;
using DealConfigFile;
using Main_EX;
using System.Windows;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 定义
        //bool 
        protected bool BlPLCReset = false;//plc复位       
        protected bool BlPLCRun = false; //启动        
        protected bool BlPLCPause = false; //暂停   
        protected bool BlPLCAlarm = false; //报警       
        protected bool BlPLCEmergency = false; //急停 

        //int

        #endregion 定义

        #region 初始化
        /// <summary>
        /// 事件注册
        /// </summary>
        protected void LoginEvent_PLC()
        {
            try
            {
                //如果不使用PLC通信
                if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.Null)
                {
                    return;
                }

                //关机重启
                LogicPLC.L_I.SoftRestart_event += new TrrigerSourceAction_del(LogicPLC_Inst_SoftRestart_event);
                LogicPLC.L_I.PCShutdown_event += new TrrigerSourceAction_del(LogicPLC_Inst_PCShutdown_event);
                LogicPLC.L_I.PCRestart_event += new TrrigerSourceAction_del(LogicPLC_Inst_PCRestart_event);

                //PLC操作
                LogicPLC.L_I.PLCState_event += new TrrigerSourceAction_del(L_I_PLCState_event);
                LogicPLC.L_I.PLCAlarm_event += new TrrigerSourceAction_del(LogicPLC_Inst_PLCAlarm_event);
                LogicPLC.L_I.PLCMaterial_event += new TrrigerSourceAction_del(L_I_PLCMaterial_event);

                //型号
                LogicPLC.L_I.NewModel_event += new TrrigerSourceAction_del(LogicPLC_Inst_NewModel_event);
                LogicPLC.L_I.ChangeModel_event += new TrrigerSourceAction_del(LogicPLC_Inst_ChangeModel_event);
                LogicPLC.L_I.DelModel_event += new TrrigerSourceAction_del(LogicPLC_Inst_DelModel_event);

                //状态                ;
                LogicPLC.L_I.RobotState_event += new TrrigerSourceAction_del(LogicPLC_Inst_RobotState_event);
                LogicPLC.L_I.RestartCommunicate_event += new TrrigerSourceAction_del(L_I_RestartCommunicate_event);
                LogicPLC.L_I.VoiceState_event += new TrrigerSourceAction_del(L_I_VoiceState_event);

                //保留
                LogicPLC.L_I.Reserve1_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve1_event);
                LogicPLC.L_I.Reserve2_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve2_event);
                LogicPLC.L_I.Reserve3_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve3_event);
                LogicPLC.L_I.Reserve4_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve4_event);
                LogicPLC.L_I.Reserve5_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve5_event);
                LogicPLC.L_I.Reserve6_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve6_event);
                LogicPLC.L_I.Reserve7_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve7_event);
                LogicPLC.L_I.Reserve8_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve8_event);
                LogicPLC.L_I.Reserve9_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve9_event);
                LogicPLC.L_I.Reserve10_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve10_event);
                LogicPLC.L_I.Reserve11_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve11_event);
                LogicPLC.L_I.Reserve12_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve12_event);
                LogicPLC.L_I.Reserve13_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve13_event);
                LogicPLC.L_I.Reserve14_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve14_event);
                LogicPLC.L_I.Reserve15_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve15_event);
                LogicPLC.L_I.Reserve16_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve16_event);
                LogicPLC.L_I.Reserve17_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve17_event);
                LogicPLC.L_I.Reserve18_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve18_event);
                LogicPLC.L_I.Reserve19_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve19_event);
                LogicPLC.L_I.Reserve20_event += new TrrigerSourceAction_del(LogicPLC_Inst_Reserve20_event);

                //PLC读写状态
                LogicPLC.L_I.CommunicationState_event += new Str2Action(LogicPLC_Inst_CommunicationState_event);

                //配置参数错误
                LogicPLC.L_I.ConfigParError_event += new StrAction(LogicPLC_Inst_ConfigParError_event);

                //相机
                LogicPLC.L_I.Camera1_event += new TrrigerSourceAction_del(DealComprehensive_Camera1_event);
                LogicPLC.L_I.Camera2_event += new TrrigerSourceAction_del(DealComprehensive_Camera2_event);
                LogicPLC.L_I.Camera3_event += new TrrigerSourceAction_del(DealComprehensive_Camera3_event);
                LogicPLC.L_I.Camera4_event += new TrrigerSourceAction_del(DealComprehensive_Camera4_event);
                LogicPLC.L_I.Camera5_event += new TrrigerSourceAction_del(DealComprehensive_Camera5_event);
                LogicPLC.L_I.Camera6_event += new TrrigerSourceAction_del(DealComprehensive_Camera6_event);
                LogicPLC.L_I.Camera7_event += new TrrigerSourceAction_del(DealComprehensive_Camera7_event);
                LogicPLC.L_I.Camera8_event += new TrrigerSourceAction_del(DealComprehensive_Camera8_event);

                //数据超限
                LogicPLC.L_I.WriteDataOverFlow += new StrAction(L_I_WriteDataOverFlow);

                //如果使用了AnnotherPLC
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    LogicPLC.L_I.DisConnectAnnotherPLC_event += new Action(L_I_DisConnectAnnotherPLC_event);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 初始化PLC
        /// </summary>
        public void Init_PLC()
        {
            try
            {
                //如果不使用DealPLC通信
                if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.Null)
                {
                    return;
                }

                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ShowState_PLC("采用独立的PLC通信程序");
                }

                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    ShowAlarm_PLC("PLC设置位离线状态");
                }

                string error = "";
                if (LogicPLC.L_I.OpenPort(out error))//打开PLC通信
                {
                    ShowState_PLC("PLC连接成功！" + ParSetPLC.P_I.TypePLC_e.ToString());
                }
                else
                {
                    //显示报警窗口
                    ShowWinError_Invoke("PLC连接失败！" + ParSetPLC.P_I.TypePLC_e.ToString() + error);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化        

        #region PLC状态
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected virtual void LogicPLC_Inst_PLCState_event(TriggerSource_enum trrigerSource_e, int intState)
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
        /// 机器人状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected virtual void LogicPLC_Inst_RobotState_event(TriggerSource_enum trrigerSource_e, int intState)
        {

        }

        /// <summary>
        /// PLC通信出错
        /// </summary>
        protected virtual void LogicPLC_Inst_CommunicationState_event(string str, string color)
        {
            ShowAlarm(str.Replace("\n", "-"));
        }
        #endregion DealPLC状态

        #region 数据超限
        /// <summary>
        /// PC发送给PLC数据超限
        /// </summary>
        /// <param name="str"></param>
        protected virtual void L_I_WriteDataOverFlow(string str)
        {
            ShowAlarm("PC发送给PLC数据超限:" + str);

            LogicPLC.L_I.PCAlarm();
        }
        #endregion 数据超限

        #region PC控制
        /// <summary>
        /// 软件重启
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void LogicPLC_Inst_SoftRestart_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("PLC触发软件重启！");
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(RestartSoft));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 重启PC
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void LogicPLC_Inst_PCRestart_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("PLC触发PC重启！");
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(RestartPC));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 关机PC
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void LogicPLC_Inst_PCShutdown_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("PLC触发PC关机！");
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(ShutDownPC));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion PC控制

        #region PLC状态
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void L_I_PLCState_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                if (i > 0)
                {
                    ParStateSoft.StateMachine_e = (StateMachine_enum)i;
                    ShowStateMachine();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void LogicPLC_Inst_PLCAlarm_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("设备发送报警信息!");
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// PLC物料信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void L_I_PLCMaterial_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("设备发送物料信息!");
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// 和PLC独立通信软件断开连接
        /// </summary>
        protected virtual void L_I_DisConnectAnnotherPLC_event()
        {
            try
            {
                ShowWinError("和PLC独立通信软件断开连接!");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion PLC状态

        #region 型号
        /// <summary>
        /// 响应PLC新建型号的事件
        /// </summary>
        protected virtual void LogicPLC_Inst_NewModel_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                switch (i)
                {
                    case 1://新建型号
                        PLCNewModel();
                        break;                 

                    case 11://通过序号新建型号
                        PLCNewModelByNo();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// PLC触发新建型号
        /// </summary>
        protected virtual void PLCNewModel()
        {
            try
            {
                ShowState("PLC触发新建型号");

                //判断是否存在非法字符
                int num = ParLogicPLC.P_I.NameModel.IndexOfAny(Path.GetInvalidFileNameChars());
                if (num > -1)
                {
                    ShowWinError_Invoke("新建型号失败，名称中含有非法字符：\\ / : * ？ \" < > |");
                    return;
                }

                #region 将PLC中读取的参数复制到配置文件类
                ReadParProductFromPLC();               
                #endregion 将DealPLC中读取的参数复制到配置文件类

                ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;
                //新建文件路径
                ComConfigPar.C_I.PathConfigIni = ComValue.c_PathPar + ParLogicPLC.P_I.NameModel + "\\Product.ini";

                //如果新建的型号相同
                if (ComConfigPar.C_I.NameModel == ParLogicPLC.P_I.NameModel)
                {
                    g_BlModelSame = true;
                }
                else
                {
                    g_BlModelSame = false;
                    ComConfigPar.C_I.NameModel = ParLogicPLC.P_I.NameModel;//新的型号
                }

                //换型
                if (NewModel())
                {
                    ShowState("PLC触发新建型号");
                }
                else
                {
                    ShowAlarm("PLC触发新建型号失败");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// PLC触发新建，通过文件编号
        /// </summary>
        protected virtual void PLCNewModelByNo()
        {
            try
            {
                ShowState("PLC触发新建型号，使用文件编号");

                #region 将PLC中读取的参数复制到配置文件类
                ReadParProductFromPLC();              
                #endregion 将DealPLC中读取的参数复制到配置文件类

                //如果新建的型号相同
                if (ComConfigPar.C_I.NoConfig == ParLogicPLC.P_I.intNo)
                {
                    g_BlModelSame = true;
                }
                else
                {
                    g_BlModelSame = false;
                    ComConfigPar.C_I.NoConfig = ParLogicPLC.P_I.intNo;//新的型号
                }

                DirectoryInfo pathRoot = new DirectoryInfo(ParPathRoot.PathRoot + "Store\\产品参数\\");
                int numNull = 0;
                foreach (DirectoryInfo files in pathRoot.GetDirectories())
                {
                    string name = files.Name;
                    if (name.Contains("#"))
                    {
                        int no = int.Parse(name.Split('#')[0]);
                        if (no == ParLogicPLC.P_I.intNo)
                        {
                            //读取当前文件
                            ComConfigPar.C_I.NameModel = name;
                            numNull = 1;
                        }
                    }
                }

                if (numNull == 0)
                {
                    ShowWinError("没有此序号的参数");
                    return;
                }

                ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;
                //新建文件路径
                ComConfigPar.C_I.PathConfigIni = ComValue.c_PathPar + ParLogicPLC.P_I.NameModel + "\\Product.ini";
                
                //换型
                if (ChangeModel())
                {
                    ShowState("PLC触发新建型号");
                }
                else
                {
                    ShowAlarm("PLC触发新建型号失败");
                }
            }
            catch (Exception ex)
            {
                ShowWinError("换型出错，进入异常，请查看异常日志s");
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// PLC触发换型，通过文件编号
        /// </summary>
        protected virtual void PLCChangeModelByNo()
        {
            try
            {
                ShowState("PLC触发换型，使用文件编号");

                //如果新建的型号相同
                if (ComConfigPar.C_I.NoConfig == ParLogicPLC.P_I.intNo)
                {
                    g_BlModelSame = true;

                }
                else
                {
                    g_BlModelSame = false;
                    ComConfigPar.C_I.NoConfig = ParLogicPLC.P_I.intNo;//新的型号
                }

                DirectoryInfo pathRoot = new DirectoryInfo(ParPathRoot.PathRoot + "Store\\产品参数\\");
                int numNull = 0;
                foreach (DirectoryInfo files in pathRoot.GetDirectories())
                {
                    string name = files.Name;
                    if (name.Contains("#"))
                    {
                        int no = int.Parse(name.Split('#')[0]);
                        if (no == ParLogicPLC.P_I.intNo)
                        {
                            //读取当前文件
                            ComConfigPar.C_I.NameModel = name;
                            numNull = 1;
                        }
                    }
                }

                if (numNull == 0)
                {
                    ShowWinError("没有此序号的参数");
                    return;
                }

                ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;
                //新建文件路径
                ComConfigPar.C_I.PathConfigIni = ComValue.c_PathPar + ComConfigPar.C_I.NameModel + "\\Product.ini";


                //换型
                if (ChangeModel())
                {
                    ShowState("PLC触发新建型号");
                }
                else
                {
                    ShowAlarm("PLC触发新建型号失败");
                }
            }
            catch (Exception ex)
            {
                ShowWinError("换型出错，进入异常，请查看异常日志s");
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// PLC触发更新数据
        /// </summary>
        protected virtual void PLCRefreshPar()
        {
            try
            {
                ShowState("PLC触发更新参数数据");
                //如果新建的型号相同
                if (ComConfigPar.C_I.NameModel != ParLogicPLC.P_I.NameModel)
                {
                    WinMsgBox.ShowMsgBox("当前名称型号参数不存在，请新建型号！");
                    return;
                }

                #region 将PLC中读取的参数复制到配置文件类
                ReadParProductFromPLC();               
                #endregion 将DealPLC中读取的参数复制到配置文件类

                //换型
                if (RefreshPar())
                {
                    ShowState("PLC触发更新产品参数");
                }
                else
                {
                    ShowAlarm("PLC触发更新产品参数失败");
                }
                //每次换型时，需要写入PLC的值
                WritePLCModelPar();//回调
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 读取产品参数
        /// </summary>
        protected virtual void ReadParProductFromPLC()
        {
            try
            {
                ParConfigPar.P_I.No = ParLogicPLC.P_I.intNo;
                for (int i = 0; i < ParLogicPLC.P_I.ParProduct_L.Count; i++)
                {
                    ParConfigPar.P_I.ParProduct_L[i].DblValue = ParLogicPLC.P_I.ParProduct_L[i].DblValue;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
              

        /// <summary>
        /// 换型
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void LogicPLC_Inst_ChangeModel_event(TriggerSource_enum trrigerSource_e, int i)
        {
            switch (i)
            {
                case 2://新建型号
                    ShowState("PLC触发更换型号");

                    //判断是否存在非法字符
                    int num = ParLogicPLC.P_I.NameModel.IndexOfAny(Path.GetInvalidFileNameChars());
                    if (num > -1)
                    {
                        ShowWinError_Invoke("更换型号失败，名称中含有非法字符：\\ / : * ？ \" < > |");
                        return;
                    }
                    ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;
                    //新建文件路径
                    ComConfigPar.C_I.PathConfigIni = ComValue.c_PathPar + ParLogicPLC.P_I.NameModel + "\\Product.ini";

                    //如果新建的型号相同
                    if (ComConfigPar.C_I.NameModel == ParLogicPLC.P_I.NameModel)
                    {
                        g_BlModelSame = true;
                    }
                    else
                    {
                        g_BlModelSame = false;
                        ComConfigPar.C_I.NameModel = ParLogicPLC.P_I.NameModel;//新的型号
                    }

                    //换型
                    if (ChangeModel())
                    {
                        ShowState("PLC触发更换型号");
                    }
                    else
                    {
                        ShowAlarm("PLC触发更换型号失败");
                    }
                    break;

                case 12://通过序号换型
                    PLCChangeModelByNo();
                    break;
            }
        }

       
        /// <summary>
        /// 删除配置参数
        /// </summary>
        protected virtual void LogicPLC_Inst_DelModel_event(TriggerSource_enum trrigerSource_e, int i)
        {
            ShowState("删除型号成功");
        }

        /// <summary>
        /// 配置参数错误
        /// </summary>
        /// <param name="str"></param>
        void LogicPLC_Inst_ConfigParError_event(string str)
        {
            try
            {
                ShowAlarm("配置参数错误，序号：" + str);
                //PC报警
                LogicPLC.L_I.PCAlarm();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 每次换型时，需要写入PLC的值,在Main里面重载
        /// </summary>
        public virtual void WritePLCModelPar()
        {
            
        }
        #endregion 型号

        #region 语音信息
        /// <summary>
        /// 语音信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void L_I_VoiceState_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        #endregion 语音信息

        #region 重启外部通信
        protected virtual void L_I_RestartCommunicate_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                switch (i)
                {
                    case 1:
                        RobotReStart();
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
        #endregion 重启外部通信

        #region 关闭PLC
        /// <summary>
        /// 关闭PLC
        /// </summary>
        public void Close_PLC()
        {
            try
            {
                //如果不使用PLC通信
                if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.Null)
                {
                    return;
                }
                //关闭PLC通信
                LogicPLC.L_I.ClosePLC();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 关闭PLC
    }
}
