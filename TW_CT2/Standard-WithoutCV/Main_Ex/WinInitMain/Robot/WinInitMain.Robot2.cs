using System;
using System.Threading.Tasks;
using DealPLC;
using DealRobot;
using System.Threading;
using BasicClass;
using DealComprehensive;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 定义
        public bool g_blRobot2Shake = false;//机器人握手

        public bool g_blRobot2NullRun = false;
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 事件注册
        /// </summary>
        public void LoginEvent_Robot2()
        {
            try
            {
                //如果没有机器人通信
                if (ParSetRobot2.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                LogicRobot2.L_I.StateRobotInterface_event += new StrBlAction(L_I_StateRobot2Interface_event);
                //配置参数
                LogicRobot2.L_I.ConfigRobot_event += new StrBlAction(LogicRobot2_Inst_ConfigRobot2_event);

                //数据反馈
                LogicRobot2.L_I.Shakehand_event += new Action(L_I_Shakehand2_event);
                LogicRobot2.L_I.FeedBackOK_event += new StrAction(R_Inst_FeedBackOK_event);
                LogicRobot2.L_I.FeedBackNG_event += new StrAction(R_Inst_FeedBackNG_event);
                LogicRobot2.L_I.RobotReset_event += new IntAction(L_I_Robot2Reset_event);
                LogicRobot2.L_I.RobotHome_event += new IntAction(L_I_Robot2Home_event);
                LogicRobot2.L_I.RobotThrow_event += new IntAction(L_I_Robot2Throw_event);
                LogicRobot2.L_I.Monitor_event += new StrAction(L_I_Monitor2_event);

                #region 相机综合处理
                LogicRobot2.L_I.Camera1_event += new TrrigerSourceAction_del(DealComprehensive_Camera1_event);
                LogicRobot2.L_I.Camera2_event += new TrrigerSourceAction_del(DealComprehensive_Camera2_event);
                LogicRobot2.L_I.Camera3_event += new TrrigerSourceAction_del(DealComprehensive_Camera3_event);
                LogicRobot2.L_I.Camera4_event += new TrrigerSourceAction_del(DealComprehensive_Camera4_event);
                LogicRobot2.L_I.Camera5_event += new TrrigerSourceAction_del(DealComprehensive_Camera5_event);
                LogicRobot2.L_I.Camera6_event += new TrrigerSourceAction_del(DealComprehensive_Camera6_event);

                LogicRobot2.L_I.Camera1_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera1_event);
                LogicRobot2.L_I.Camera2_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera2_event);
                LogicRobot2.L_I.Camera3_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera3_event);
                LogicRobot2.L_I.Camera4_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera4_event);
                LogicRobot2.L_I.Camera5_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera5_event);
                LogicRobot2.L_I.Camera6_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera6_event);

                LogicRobot2.L_I.Camera1_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera1_event);
                LogicRobot2.L_I.Camera2_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera2_event);
                LogicRobot2.L_I.Camera3_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera3_event);
                LogicRobot2.L_I.Camera4_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera4_event);
                LogicRobot2.L_I.Camera5_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera5_event);
                LogicRobot2.L_I.Camera6_IndexN_event += new TrrigerSourceIntNAction_del(DealComprehensive_Camera6_event);


                #endregion 相机综合处理

                #region 数据超限报警
                LogicRobot2.L_I.DataError_event += new StrAction(Robot2LCamera1_Inst_DataError_event);
                #endregion 数据超限报警

                #region Others
                LogicRobot2.L_I.Others_event += new IntAction(R_Inst_Others2_event);
                LogicRobot2.L_I.Version_event += L_I_Version2_event;
                #endregion Others

                #region Delay
                LogicRobot2.L_I.Delay_event += new IntAction(L_I_Delay2_event);
                #endregion Delay
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 初始化机器人
        /// </summary>       
        void Init_Robot2()
        {
            try
            {
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    return;
                }
                //如果没有机器人通信
                if (ParSetRobot2.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                //读取机器人标准规划             

                switch (ParLogicRobot2.P_I.StatePortRobot_e)
                {
                    case StatePortRobot_enum.AllTrue:
                        ShowState("机器人2通信成功！");
                        //机器人握手
                        Task task = new Task(Robot2Shake);
                        task.Start();
                        break;

                    case StatePortRobot_enum.AllError:
                        //显示报警窗口
                        ShowWinError_Invoke("机器人2通信失败！");
                        break;

                    case StatePortRobot_enum.Wait:
                        ShowAlarm("等待机器人2连接通信");
                        break;
                }
                //自动连接机器人
                if (ParSetRobot2.P_I.BlAutoConnect)
                {
                    Robot2ReStart();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion 初始化

        #region 机器人复位重启
        /// <summary>
        /// 重启机器人通信
        /// </summary>
        public void Robot2ReStart()
        {
            try
            {
                //Thread.Sleep(500);
                ShowState("开始重启PC机器人2通信！");
                //关闭数据接收和数据打开按钮         
                ShowAlarm("开始关闭机器人2通信");
                if (!LogicRobot2.L_I.CloseRobot())
                {
                    ShowWinError_Invoke("关闭机器人2通信失败！");
                    return;
                }

                ShowState("关闭机器人2通信成功！");

                Thread.Sleep(200);
                ShowAlarm("等待机器人2和PC连接");
                LogicRobot2.L_I.OpenInterface();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 机器人复位重启

        #region 机器人通信状态
        /// <summary>
        /// 机器人接口状态
        /// </summary>
        /// <param name="str"></param>
        void L_I_StateRobot2Interface_event(string str, bool blResult)
        {
            try
            {
                if (blResult)
                {
                    ParLogicRobot2.P_I.StatePortRobot_e = StatePortRobot_enum.AllTrue;
                    ShowState("机器人通信成功");

                    //机器人握手
                    Task task = new Task(Robot2Shake);
                    task.Start();
                }
                else
                {
                    ParLogicRobot2.P_I.StatePortRobot_e = StatePortRobot_enum.AllError;
                    ShowAlarm("机器人通信失败");
                    //显示报警窗口
                    ShowWinError_Invoke("机器人通信失败！");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 机器人通信状态

        #region 设置机器人处于空跑状态
        public void SetRobot2NullRun()
        {
            try
            {
                //如果没有机器人通信
                if (ParSetRobot2.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }

                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    LogicRobot2.L_I.RobotNullRun(true);
                    g_blRobot2NullRun = true;
                    ShowState("机器人2进入空跑模式");
                }
                else if (g_blRobot2NullRun)
                {
                    LogicRobot2.L_I.RobotNullRun(false);
                    ShowState("机器人2退出空跑模式");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 设置机器人处于空跑状态

        #region 机器人握手
        /// <summary>
        /// 机器人握手
        /// </summary>
        void Robot2Shake()
        {
            try
            {
                ShowState("机器人开始握手！");
                Thread.Sleep(300);
                g_blRobot2Shake = false;
                LogicRobot2.L_I.RobotShake();
                int i = 0;
                while (!g_blRobot2Shake)
                {
                    Thread.Sleep(400);
                    i++;
                    if (i > 10)
                    {
                        break;
                    }
                }

                if (!g_blRobot2Shake)
                {
                    ShowAlarm("机器人2握手失败");
                    LogicPLC.L_I.PCConnectRobotNG();
                }
                else
                {
                    ShowState("机器人2握手成功！");
                    LogicPLC.L_I.PCConnectRobotOK();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 机器人握手

        #region 机器人HomeThrow
        protected virtual void L_I_Robot2Reset_event(int i)
        {

        }

        /// <summary>
        /// 机器人回到Home点
        /// </summary>
        /// <param name="i"></param>
        protected virtual void L_I_Robot2Home_event(int i)
        {

        }

        /// <summary>
        /// 抛料
        /// </summary>
        /// <param name="i"></param>
        protected virtual void L_I_Robot2Throw_event(int i)
        {

        }
        #endregion 机器人HomeThrow

        #region Others
        protected virtual void R_Inst_Others2_event(int index)
        {

        }

        /// <summary>
        /// 机器人程序版本号
        /// </summary>
        /// <param name="str"></param>
        private void L_I_Version2_event(string str)
        {
            ShowState("机器人2程序版本号：" + str);
        }
        #endregion Others

        #region 超时
        protected virtual void L_I_Delay2_event(int i)
        {

        }
        #endregion 超时

        #region 接收数据结果反馈
        /// <summary>
        /// 握手
        /// </summary>
        void L_I_Shakehand2_event()
        {
            try
            {
                g_blRobot2Shake = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void R_Inst_FeedBackOK2_event(string cmd)
        {
            ShowState("机器人2接收指令：" + cmd + "成功！");
        }

        void R_Inst_FeedBackNG2_event(string cmd)
        {
            try
            {
                ShowAlarm("机器人2接收指令：" + cmd + "异常！请重新启动机器人");
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
        void L_I_Monitor2_event(string str)
        {
            try
            {
                ShowAlarm(str);
                ShowWinError_Invoke(str);
                LogicPLC.L_I.PCAlarm();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        //相机数据超限
        void Robot2LCamera1_Inst_DataError_event(string str)
        {
            try
            {
                ShowWinError_Invoke("数据超限:" + str);
                LogicPLC.L_I.PCAlarm();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 接收数据结果反馈

        #region 信息显示
        /// <summary>
        /// 机器人配置参数反馈情况
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        void LogicRobot2_Inst_ConfigRobot2_event(string str, bool result)
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
        public void Close_Robot2()
        {
            try
            {
                if (ParSetRobot2.P_I.TypeRobot_e != TypeRobot_enum.Null)
                {
                    LogicRobot2.L_I.CloseRobot();
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
