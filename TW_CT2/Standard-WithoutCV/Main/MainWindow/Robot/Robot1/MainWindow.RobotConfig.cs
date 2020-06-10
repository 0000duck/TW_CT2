using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealRobot;
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using DealLog;
using DealPLC;
using DealConfigFile;

namespace Main
{
    partial class MainWindow
    {
        /// <summary>
        /// 给机器人发送配置参数
        /// </summary>
        public override void ConfigRobot_Task()
        {
            try
            {
                if (ParSetRobot.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    return;
                }
                #region 清空旧的参数
                LogicRobot.L_I.ParRobotCom_L.Clear();
                LogicRobot.L_I.ParRobot1_L.Clear();
                LogicRobot.L_I.ParRobot2_L.Clear();
                LogicRobot.L_I.ParRobot3_L.Clear();
                LogicRobot.L_I.ParRobot4_L.Clear();

                LogicRobot.L_I.ParRobotCom_P4L.Clear();
                LogicRobot.L_I.ParRobot1_P4L.Clear();
                LogicRobot.L_I.ParRobot2_P4L.Clear();
                LogicRobot.L_I.ParRobot3_P4L.Clear();
                LogicRobot.L_I.ParRobot4_P4L.Clear();
                #endregion 清空旧的参数

                LogicRobot.L_I.ParRobotCom_P4L.Add(ModelParams.pPickPlat1);//500
                LogicRobot.L_I.ParRobotCom_P4L.Add(ModelParams.pPrecise);//501
                LogicRobot.L_I.ParRobotCom_P4L.Add(ModelParams.pPlacePlat2);//502
                LogicRobot.L_I.ParRobotCom_P4L.Add(ModelParams.pDump);//503

                LogicRobot.L_I.ParRobot1_P4L.Add(ModelParams.pSt1_1);//550
                LogicRobot.L_I.ParRobot1_P4L.Add(ModelParams.pSt1_2);//551
                LogicRobot.L_I.ParRobot2_P4L.Add(ModelParams.pSt2_1);//600
                LogicRobot.L_I.ParRobot2_P4L.Add(ModelParams.pSt2_2);//601
                LogicRobot.L_I.ParRobot3_P4L.Add(ModelParams.pSt3_1);//650
                LogicRobot.L_I.ParRobot3_P4L.Add(ModelParams.pSt3_2);//651


                //发送参数
                Task task = new Task(LogicRobot.L_I.WriteConfigRobot);
                task.Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
       
    }
}
