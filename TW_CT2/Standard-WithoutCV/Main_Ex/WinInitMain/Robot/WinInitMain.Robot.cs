using System;
using System.Threading.Tasks;
using DealPLC;
using DealRobot;
using System.Threading;
using BasicClass;
using DealComprehensive;
using System.IO;
using DealFile;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 定义
        bool g_blRobotShake = false;//机器人握手

        public bool g_blRobotNullRun = false;
        #endregion 定义

        #region 初始化

        /// <summary>
        /// 事件注册
        /// </summary>
        protected void LoginEvent_Robot()
        {
            try
            {
                //如果没有机器人通信
                if (ParSetRobot.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                LogicRobot.L_I.StateRobotInterface_event += new StrBlAction(L_I_StateRobotInterface_event);
                //配置参数
                LogicRobot.L_I.ConfigRobot_event += new StrBlAction(LogicRobot_Inst_ConfigRobot_event);

                //数据反馈
                LogicRobot.L_I.Shakehand_event += new Action(L_I_Shakehand_event);
                LogicRobot.L_I.FeedBackOK_event += new StrAction(R_Inst_FeedBackOK_event);
                LogicRobot.L_I.FeedBackNG_event += new StrAction(R_Inst_FeedBackNG_event);
                LogicRobot.L_I.RobotReset_event += new IntAction(L_I_RobotReset_event);
                LogicRobot.L_I.RobotHome_event += new IntAction(L_I_RobotHome_event);
                LogicRobot.L_I.RobotThrow_event += new IntAction(L_I_RobotThrow_event);
                LogicRobot.L_I.Monitor_event += new StrAction(L_I_Monitor_event);

                LogicRobot.L_I.Msg_event += new StrAction(LogMsg_event);
                LogicRobot.L_I.Alarm_event += new StrAction(LogAlarm_event);
                LogicRobot.L_I.Sig_event += new StrAction(LogSig_event);
                LogicRobot.L_I.TriggerT_event += new StrAction(RotateT_event);
                LogicRobot.L_I.NeedLight_event += new Action(RemindLight_event);

                #region 相机综合处理
                LogicRobot.L_I.Camera1_event += new TrrigerSourceAction_del(DealComprehensive_Camera1_event);
                LogicRobot.L_I.Camera2_event += new TrrigerSourceAction_del(DealComprehensive_Camera2_event);
                LogicRobot.L_I.Camera3_event += new TrrigerSourceAction_del(DealComprehensive_Camera3_event);
                LogicRobot.L_I.Camera4_event += new TrrigerSourceAction_del(DealComprehensive_Camera4_event);
                LogicRobot.L_I.Camera5_event += new TrrigerSourceAction_del(DealComprehensive_Camera5_event);
                LogicRobot.L_I.Camera6_event += new TrrigerSourceAction_del(DealComprehensive_Camera6_event);

                LogicRobot.L_I.Camera1_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera1_event);
                LogicRobot.L_I.Camera2_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera2_event);
                LogicRobot.L_I.Camera3_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera3_event);
                LogicRobot.L_I.Camera4_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera4_event);
                LogicRobot.L_I.Camera5_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera5_event);
                LogicRobot.L_I.Camera6_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera6_event);

                LogicRobot.L_I.Camera1_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera1_event);
                LogicRobot.L_I.Camera2_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera2_event);
                LogicRobot.L_I.Camera3_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera3_event);
                LogicRobot.L_I.Camera4_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera4_event);
                LogicRobot.L_I.Camera5_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera5_event);
                LogicRobot.L_I.Camera6_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera6_event);


                #endregion 相机综合处理

                #region 数据超限报警
                LogicRobot.L_I.DataError_event += new StrAction(RobotLCamera1_Inst_DataError_event);
                #endregion 数据超限报警

                #region Others
                LogicRobot.L_I.Others_event += new IntAction(R_Inst_Others_event);
                LogicRobot.L_I.Version_event += L_I_Version_event;
                #endregion Others

                #region Delay
                LogicRobot.L_I.Delay_event += new IntAction(L_I_Delay_event);
                #endregion Delay
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        /// <summary>
        /// 初始化机器人
        /// </summary>       
        public void Init_Robot()
        {
            try
            {
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    return;
                }
                //如果没有机器人通信
                if (ParSetRobot.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }

                switch (ParLogicRobot.P_I.StatePortRobot_e)
                {
                    case StatePortRobot_enum.AllTrue:
                        ShowState("机器人通信成功！");
                        //机器人握手
                        Task task = new Task(RobotShake);
                        task.Start();
                        break;

                    case StatePortRobot_enum.AllError:
                        //显示报警窗口
                        ShowWinError_Invoke("机器人通信失败！");
                        break;

                    case StatePortRobot_enum.Wait:
                        ShowAlarm("等待机器人连接通信");
                        break;
                }
                //自动连接机器人
                if (ParSetRobot.P_I.BlAutoConnect)
                {
                    RobotReStart();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 机器人通信状态
        /// <summary>
        /// 机器人接口状态
        /// </summary>
        /// <param name="str"></param>
        void L_I_StateRobotInterface_event(string str, bool blResult)
        {
            try
            {
                if (blResult)
                {
                    ParLogicRobot.P_I.StatePortRobot_e = StatePortRobot_enum.AllTrue;
                    ShowState("机器人通信成功");

                    //机器人握手
                    Task task = new Task(RobotShake);
                    task.Start();
                }
                else
                {
                    ParLogicRobot.P_I.StatePortRobot_e = StatePortRobot_enum.AllError;
                    ShowAlarm("机器人通信失败");
                    //显示报警窗口
                    ShowWinError_Invoke("机器人通信失败！");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 机器人通信状态

        #region 机器人复位重启
        /// <summary>
        /// 重启机器人通信
        /// </summary>
        public void RobotReStart()
        {
            try
            {
                //Thread.Sleep(500);
                ShowState("开始重启PC机器人通信！");
                //关闭数据接收和数据打开按钮         
                ShowAlarm("开始关闭机器人通信");
                if (!LogicRobot.L_I.CloseRobot())
                {
                    ShowWinError_Invoke("关闭机器人通信失败！");
                    return;
                }

                ShowState("关闭机器人通信成功！");

                Thread.Sleep(200);
                ShowAlarm("等待机器人和PC连接");
                LogicRobot.L_I.OpenInterface();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 机器人复位重启

        #region 设置机器人处于空跑状态
        public void SetRobotNullRun()
        {
            try
            {
                //如果没有机器人通信
                if (ParSetRobot.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }

                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    LogicRobot.L_I.RobotNullRun(true);
                    g_blRobotNullRun = true;
                    ShowState("机器人进入空跑模式");
                }
                else if (g_blRobotNullRun)
                {
                    LogicRobot.L_I.RobotNullRun(false);
                    ShowState("机器人退出空跑模式");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 设置机器人处于空跑状态

        #region 机器人握手
        public virtual void RobotShake()
        {
            try
            {
                ShowState("机器人开始握手！");
                Thread.Sleep(300);
                g_blRobotShake = false;
                LogicRobot.L_I.RobotShake();
                int i = 0;
                while (!g_blRobotShake)
                {
                    Thread.Sleep(400);
                    i++;
                    if (i > 10)
                    {
                        break;
                    }
                }

                if (!g_blRobotShake)
                {
                    ShowAlarm("机器人握手失败");
                    LogicPLC.L_I.PCConnectRobotNG();
                }
                else
                {
                    ShowState("机器人握手成功！");
                    LogicPLC.L_I.PCConnectRobotOK();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 机器人握手

        #region 机器人HomeThrow
        /// <summary>
        /// 机器人复位 在Main里面实现重载
        /// </summary>
        /// <param name="i"></param>
        protected virtual void L_I_RobotReset_event(int i)
        {

        }
        /// <summary>
        /// 机器人回到Home点，在Main里面实现重载
        /// </summary>
        /// <param name="i"></param>
        protected virtual void L_I_RobotHome_event(int i)
        {

        }

        /// <summary>
        /// 抛料，在Main里面实现重载
        /// </summary>
        /// <param name="i"></param>
        protected virtual void L_I_RobotThrow_event(int i)
        {

        }
        #endregion 机器人HomeThrow

        #region 机器人日志
        public void LogMsg_event(string msg)
        {
            RobotLog(msg);
        }

        public void LogAlarm_event(string alarm)
        {
            RobotLog(alarm);
        }

        public void LogSig_event(string sig)
        {
            RobotLog(sig);
        }

        public void RobotLog(string msg)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                string root = ParPathRoot.PathRoot + "软件运行记录\\RecordData\\Robot\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + "RobotLog" + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                t_I.WriteText(path, name + "---------" + msg);//写入时间
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion

        #region 机器人触发T轴旋转
        protected virtual void RotateT_event(string str)
        {

        }

        #endregion

        #region 提示该工位还未点亮测试，无法标定
        protected virtual void RemindLight_event()
        {

        }
        #endregion

        #region Others
        /// <summary>
        /// 其他
        /// </summary>
        /// <param name="index"></param>
        protected virtual void R_Inst_Others_event(int index)
        {

        }


        /// <summary>
        /// 机器人程序版本号
        /// </summary>
        /// <param name="str"></param>
        private void L_I_Version_event(string str)
        {
            ShowState("机器人程序版本号：" + str);
        }
        #endregion Others

        #region 超时
        /// <summary>
        /// 数据超时
        /// </summary>
        /// <param name="i"></param>
        protected virtual void L_I_Delay_event(int i)
        {

        }
        #endregion 超时

        #region 接收数据结果反馈
        void L_I_Shakehand_event()
        {
            try
            {
                g_blRobotShake = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        void R_Inst_FeedBackOK_event(string cmd)
        {
            ShowState("机器人接收指令：" + cmd + "成功！");
        }
        void R_Inst_FeedBackNG_event(string cmd)
        {
            try
            {
                ShowAlarm("机器人接收指令：" + cmd + "异常！请重新启动机器人");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 机器人监控
        /// </summary>
        /// <param name="str"></param>
        void L_I_Monitor_event(string str)
        {
            try
            {
                ShowAlarm(str);
                ShowWinError_Invoke(str);
                LogicPLC.L_I.PCAlarm();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        //相机数据超限
        void RobotLCamera1_Inst_DataError_event(string str)
        {
            try
            {
                ShowWinError_Invoke("数据超限:" + str);
                LogicPLC.L_I.PCAlarm();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 接收数据结果反馈

        #region 换型
        public virtual void ConfigRobot_Task()
        {

        }
        #endregion 换型

        #region 信息显示
        /// <summary>
        /// 机器人配置参数反馈情况
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        void LogicRobot_Inst_ConfigRobot_event(string str, bool result)
        {
            if (result)
            {
                ShowState(str);
            }
            else
            {
                ShowWinError_Invoke(str);
            }
        }
        #endregion 信息显示

        #region 关闭机器人
        /// <summary>
        /// 关闭机器人
        /// </summary>
        protected void Close_Robot()
        {
            try
            {
                if (ParSetRobot.P_I.TypeRobot_e != TypeRobot_enum.Null)
                {
                    LogicRobot.L_I.CloseRobot();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 关闭机器人
    }
}
