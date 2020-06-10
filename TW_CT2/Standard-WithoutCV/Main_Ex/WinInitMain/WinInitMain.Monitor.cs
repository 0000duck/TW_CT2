
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
using Camera;
using HalconDotNet;
using System.Drawing;
using DealRobot;
using DealPLC;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using DealComprehensive;
using SetPar;
using SetComprehensive;
using DealFile;
using BasicClass;
using DealConfigFile;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 监控
        int time_Logout = 0;//注销登录开始计数计时

        //Timer
        System.Windows.Forms.Timer TM30000_LongMonitor = new System.Windows.Forms.Timer();//长时间监控
        System.Windows.Forms.Timer TM200_ShortMonitor = new System.Windows.Forms.Timer();//短时间监控
        #endregion 监控

        #region 初始化
        //事件注册
        public void LoginEvent_Monitor()
        {
            try
            {
                this.TM30000_LongMonitor.Tick += new System.EventHandler(this.TM30000_LongMonitor_Tick);
                this.TM200_ShortMonitor.Tick += new EventHandler(TM200_ShortMonitor_Tick);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void Init_Monitor()
        {
            try
            {
                EnableTimer_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //控件定时器
        void EnableTimer_Invoke()
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        //长时间监控
                        this.TM30000_LongMonitor.Interval = 3000;//30S
                        this.TM30000_LongMonitor.Enabled = true;
                        //短时间监控
                        this.TM200_ShortMonitor.Interval = 200;
                        this.TM200_ShortMonitor.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        Log.L_I.WriteError(NameClass, ex);
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 系统监控
        /// <summary>
        /// 长时间监控,30s监控一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TM30000_LongMonitor_Tick(object sender, EventArgs e)
        {
            try
            {
                FileMonitor();//监控文件夹删除
                MonitorLogout();//监控自动注销
                HardDisk();//存储空间监控
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 短时间监控,200ms循环一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TM200_ShortMonitor_Tick(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 系统监控

        #region 监控注销
        /// <summary>
        /// 自动退出
        /// </summary>
        public void MonitorLogout()
        {
            try
            {
                if (Authority.Authority_e != Authority_enum.Null)
                {
                    time_Logout++;
                    double time = time_Logout * 0.5;//0.5分钟为一个计时单位
                    if (ParSetLogin.P_I.TimeLogout < time
                        && !RegeditLogin.R_I.BlManufacturer)
                    {
                        Logout_Invoke();//退出登录
                        g_UCStateWork.AddInfo("自动退出登录");
                    }
                }
                else
                {
                    time_Logout = 0;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 监控注销

        #region 监控文件夹
        /// <summary>
        /// 监控文件夹，判断是否删除多余的文件
        /// </summary>
        public void FileMonitor()
        {
            try
            {
                FunDelFolder.F_I.DelteFolder(g_UCStateWork);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 监控文件夹

        #region 监控硬盘容量
        /// <summary>
        /// 当硬盘容量不足时报警
        /// </summary>
        public void HardDisk()
        {
            try
            {
                double[] space = FunHarDisk.F_I.GetSpace(ParHardDisk.P_I.NameDrive);
                if (space[1] < ParHardDisk.P_I.SpaceMin)
                {
                    ShowWinError_Invoke(string.Format("硬盘存储空间小于设置值{0}G，\n请删除多余文件（主要是图片）!", ParHardDisk.P_I.SpaceMin.ToString()));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 监控硬盘容量
    }
}
