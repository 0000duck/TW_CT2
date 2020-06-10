using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BasicClass;

namespace DealConfigFile
{
    /// <summary>
    /// add by xc-190401
    /// </summary>
    public partial class WinSetRobotPoints : BaseWindow
    {
        #region 单例
        private static WinSetRobotPoints g_WinSetRobotPoints = null;
        public static WinSetRobotPoints GetWinInst()
        {
            if (g_WinSetRobotPoints == null)
            {
                g_WinSetRobotPoints = new WinSetRobotPoints();
            }
            return g_WinSetRobotPoints;
        }

        public WinSetRobotPoints()
        {
            InitializeComponent();
        }
        #endregion

        #region  历史参数恢复
        private void BaseWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //非厂商登录
                if (Authority.Authority_e != Authority_enum.Manufacturer)
                {
                    return;
                }

                bool blNew = false;
                WinRecoverRobotStd win = WinRecoverRobotStd.GetWinInst(out blNew);
                win.Show();
                if (blNew)
                {
                    win.RecoverPar_event += Win_RecoverPar_event;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void Win_RecoverPar_event()
        {
            try
            {
                ShowPar();
                WinMsgBox.ShowMsgBox("基准值按照恢复的历史参数完成刷新", true, 2000);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion  历史参数恢复

        #region 显示
        public void ShowPar_Task()
        {
            Thread.Sleep(300);
            ShowPar_Invoke();
        }

        public override void ShowPar()
        {
            try
            {               
                foreach (BaseUCAdjust item in gdLayout.Children)
                {
                    ((BaseUCAdjust)item).ShowPar();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            new Task(ShowPar_Task).Start();
        }
        #endregion 显示

        #region 关闭
        private void BaseMetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                TriggerCloseEvent();
                //e.Cancel = true;
                // this.Visibility = Visibility.Hidden;
                g_WinSetRobotPoints = null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion
    }
}
