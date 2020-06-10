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
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using DealFile;
using System.Diagnostics;
using BasicClass;

namespace Main_EX
{
    partial class WinInitMain
    {
        //关闭软件各个功能
        public void CloseWork()
        {
            //关闭相机
            Close_Camera();
            //关闭机器人
            Close_Robot();
            //关闭DealPLC
            Close_PLC();

            //关闭Custom
            Close_Custom();

            //关闭CIM
            CloseCIM();

            CloseIO();
            Thread.Sleep(200);
            //this.Close();
        }


        /// <summary>
        /// 关闭CIM端口
        /// </summary>
        public virtual void CloseCIM()
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
        /// 关闭Custom
        /// </summary>
        public virtual void Close_Custom()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 关闭IO
        /// </summary>
        public virtual void CloseIO()
        {
            try
            {
               
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// 软件重启
        /// </summary>
        public void RestartSoft()
        {
            try
            {
                if (MessageBox.Show("是否重启软件？", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    return;
                }
                //关闭软件工作
                CloseWork();

                System.Windows.Forms.Application.Restart();
                //主界面退出的时候关闭程序
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 电脑关机
        /// </summary>
        public void ShutDownPC()
        {
            try
            {
                if (MessageBox.Show("是否关闭电脑？", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    return;
                }
                //关闭软件
                CloseWork();

                Process p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "cmd";
                p.StartInfo.Arguments = "/c shutdown -s -t 0";
                p.Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 电脑重启
        /// </summary>
        public void RestartPC()
        {
            try
            {
                if (MessageBox.Show("是否关闭电脑？", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    return;
                }
                Process p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "cmd";
                p.StartInfo.Arguments = "/c shutdown -r -t 0";
                p.Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 软件关闭
        /// </summary>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否关闭软件？", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                    return;
                }

                //关闭软件工作
                CloseWork();

                ////主界面退出的时候关闭程序
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowState("软件关闭");
        }
    }
}
