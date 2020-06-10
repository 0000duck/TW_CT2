using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using BasicClass;
using System.Windows;
using System.Windows.Controls;

namespace Main_EX
{
    public partial class WinInitMain
    {
        #region 定义
        protected Label g_LbStateMachine = null;
        #endregion 定义

        public void ShowStateMachine()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        switch (ParStateSoft.StateMachine_e)
                        {
                            case StateMachine_enum.Idle:
                                g_LbStateMachine.Content = "待机中";
                                g_LbStateMachine.Foreground = Brushes.Brown;
                                ShowState("待机中");
                                break;

                            case StateMachine_enum.Alarm:
                                g_LbStateMachine.Content = "设备报警";
                                g_LbStateMachine.Foreground = Brushes.Red;
                                ShowState("设备报警");
                                break;

                            case StateMachine_enum.Auto:
                                g_LbStateMachine.Content = "自动运行";
                                g_LbStateMachine.Foreground = Brushes.Green;
                                ShowState("自动运行");
                                break;

                            case StateMachine_enum.Manual:
                                g_LbStateMachine.Content = "手动运行";
                                g_LbStateMachine.Foreground = Brushes.Blue;
                                ShowState("手动运行");
                                break;

                            case StateMachine_enum.NullRun:
                                g_LbStateMachine.Content = "空运转";
                                g_LbStateMachine.Foreground = Brushes.SkyBlue;
                                ShowState("空运转");
                                break;

                            case StateMachine_enum.Pause:
                                g_LbStateMachine.Content = "暂停";
                                g_LbStateMachine.Foreground = Brushes.Orange;
                                ShowState("暂停");
                                break;

                            case StateMachine_enum.Stop:
                                g_LbStateMachine.Content = "停止";
                                g_LbStateMachine.Foreground = Brushes.Red;
                                ShowState("停止");
                                break;

                            case StateMachine_enum.Reset:
                                g_LbStateMachine.Content = "复位";
                                g_LbStateMachine.Foreground = Brushes.LimeGreen;
                                ShowState("复位");
                                break;

                            case StateMachine_enum.Calib:
                                g_LbStateMachine.Content = "校准";
                                g_LbStateMachine.Foreground = Brushes.Blue;
                                ShowState("校准");
                                break;
                        }
                    }
               ));
                //设置机器人是否处于空跑模式
                SetRobotNullRun();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }

}
