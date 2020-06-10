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
using BasicClass;


namespace Main_EX
{
    /// <summary>
    /// UCStateSoft.xaml 的交互逻辑
    /// </summary>
    public partial class UCStateSoft : BaseControl
    {
        #region 定义
        public event Action Close_Event;
        #endregion 定义

        #region  初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCStateSoft()
        {
            InitializeComponent();

            NameClass = "BaseControl";
        }
        #endregion  初始化


        #region 保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)rdbReset.IsChecked)//复位
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Reset;
                }
                if ((bool)rdbRun.IsChecked)//自动运行
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Auto;
                }
                if ((bool)rdbMannual.IsChecked)//手动运行
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Manual;
                }
                if ((bool)rdbIdle.IsChecked)//待机
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Idle;
                }
                if ((bool)rdbPause.IsChecked)//暂停
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Pause;
                }
                if ((bool)rdbAlarm.IsChecked)//报警
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Alarm;
                }
                if ((bool)rdbStop.IsChecked)//停止
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Stop;
                }
                if ((bool)rdbNull.IsChecked)//空运转
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.NullRun;
                }

                if ((bool)rdbCalib.IsChecked)//空运转
                {
                    ParStateSoft.StateMachine_e = StateMachine_enum.Calib;
                }

                btnSave.RefreshDefaultColor("保存成功", true);

                Close_Event();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                btnSave.RefreshDefaultColor("保存失败", false);
            }
        }
        #endregion 保存


        #region 显示
        /// <summary>
        /// 显示参数
        /// </summary>
        public override void ShowPar()
        {
            try
            {
                switch (ParStateSoft.StateMachine_e)
                {
                    case StateMachine_enum.Idle:
                        rdbIdle.IsChecked = true;
                        break;
                    case StateMachine_enum.Manual:
                        rdbMannual.IsChecked = true;
                        break;
                    case StateMachine_enum.Auto:
                        rdbRun.IsChecked = true;
                        break;
                    case StateMachine_enum.NullRun:
                        rdbNull.IsChecked = true;
                        break;
                    case StateMachine_enum.Alarm:
                        rdbAlarm.IsChecked = true;
                        break;
                    case StateMachine_enum.Pause:
                        rdbPause.IsChecked = true;
                        break;
                    case StateMachine_enum.Stop:
                        rdbStop.IsChecked = true;
                        break;
                    case StateMachine_enum.Reset:
                        rdbReset.IsChecked = true;
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
        #endregion 显示

        #region 退出
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close_Event();
        }
        #endregion 退出
    }
}
