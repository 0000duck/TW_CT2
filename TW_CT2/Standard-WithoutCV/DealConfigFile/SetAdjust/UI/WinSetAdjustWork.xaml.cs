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
using DealFile;
using BasicClass;
using Common;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using DealLog;

namespace DealConfigFile
{
    /// <summary>
    /// SetCom.xaml 的交互逻辑
    /// </summary>
    public partial class WinSetAdjustWork : BaseWindow
    {
        #region 窗体单实例
        private static WinSetAdjustWork g_WinSetAdjustWork = null;
        public static WinSetAdjustWork GetWinInst()
        {
            if (g_WinSetAdjustWork == null)
            {
                g_WinSetAdjustWork = new WinSetAdjustWork();
            }
            return g_WinSetAdjustWork;
        }
        #endregion 窗体单实例

        #region 定义

        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public WinSetAdjustWork()
        {
            InitializeComponent();

            NameClass = "WinSetAdjustWork";
        }
        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Authority.Authority_e!=Authority_enum.Manufacturer
                    && Authority.Authority_e != Authority_enum.Debug)
                {
                    foreach (BaseUCAdjust item in gdLayout.Children)
                    {
                        if (ParSetAdjust.P_I[item.Name, -1].BlHidden)
                        {
                            ((BaseUCAdjust)item).Visibility = Visibility.Hidden;
                        }
                    }
                }

                new Task(new Action(ShowPar_Task)).Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 历史参数恢复
        /// <summary>
        /// 历史参数恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                WinRecoverAdjust win= WinRecoverAdjust.GetWinInst(out blNew);
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

                WinMsgBox.ShowMsgBox("调整值按照恢复的历史参数完成刷新", true, 2000);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 历史参数恢复

        #region 显示      
        /// <summary>
        /// 显示线程
        /// </summary>
        public void ShowPar_Task()
        {
            Thread.Sleep(300);
            ShowPar_Invoke();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void ShowPar()
        {
            try
            {
                //adjust1.ShowPar(ParAdjust.PathAdjust);
                //adjust2.ShowPar(ParAdjust.PathAdjust);
                //adjust3.ShowPar(ParAdjust.PathAdjust);
                //adjust4.ShowPar(ParAdjust.PathAdjust);
                //adjust5.ShowPar(ParAdjust.PathAdjust);
                //adjust6.ShowPar(ParAdjust.PathAdjust);
                //adjust7.ShowPar(ParAdjust.PathAdjust);
                //adjust8.ShowPar(ParAdjust.PathAdjust);
                //adjust9.ShowPar(ParAdjust.PathAdjust);
                //adjust10.ShowPar(ParAdjust.PathAdjust);
                //adjust11.ShowPar(ParAdjust.PathAdjust);
                //adjust12.ShowPar(ParAdjust.PathAdjust);
                //adjust13.ShowPar(ParAdjust.PathAdjust);

                if (!File.Exists(ParAdjust.PathAdjust))
                {
                    MessageBox.Show("调整值文件不存在:" + ParAdjust.PathAdjust);
                    //return;
                }
                foreach (BaseUCAdjust item in gdLayout.Children)
                {
                    if (item.Visibility == Visibility.Hidden)
                    {
                        continue;
                    }
                    ((BaseUCAdjust)item).ShowPar();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

        #region 退出窗体
        private void BaseMetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                TriggerCloseEvent();
                g_WinSetAdjustWork = null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinSetAdjustWork", ex);
            }
        }

        #endregion 退出窗体

    }
}
