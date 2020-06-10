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
using System.Windows.Shapes;
using BasicClass;
using System.IO;
using DealLog;
using DealFile;
using System.Threading;
using System.Threading.Tasks;

namespace DealConfigFile
{
    /// <summary>
    /// 恢复基准值
    /// </summary>
    public class WinRecoverStd: WinRecoverAdjust
    {
        #region 窗体单实例
        private static WinRecoverStd g_WinRecover = null;
        public static WinRecoverStd GetWinInst(out bool blNew)
        {
            blNew = false;
            if (g_WinRecover == null)
            {
                blNew = true;
                g_WinRecover = new WinRecoverStd();
            }
            return g_WinRecover;
        }
        #endregion 窗体单实例

        //定义事件
        /// <summary>
        /// 恢复参数
        /// </summary>
        public event Action RecoverPar_event;

        #region 初始化
        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReadDir("Std");
        }
        #endregion 初始化

        /// <summary>
        /// 选择文件目录查看文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public override void dgDir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (dgDir.IsMouseOver)
                {
                    ReadFile(ParRecover.P_I.g_BaseParRecoverDir_L[dgDir.SelectedIndex], "Std");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        #region 恢复参数
        /// <summary>
        /// 刷新历史参数的目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadDir("Std");
                btnRefresh.RefreshDefaultColor("刷新成功", true);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 恢复参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void btnRecover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FunBackup.F_I.BackupStd();//备份基准值

                base.IndexP = this.dgDir.SelectedIndex;
                if (WinMsgBox.ShowMsgBox("使用备份参数永久覆盖当前所有基准值，是否继续？"))
                {
                    DirectoryInfo theFolder = new DirectoryInfo(ParRecover.P_I.g_BaseParRecoverDir_L[base.IndexP].Path);
                    foreach (FileInfo item in theFolder.GetFiles())
                    {
                        if (item.Name == "Std.ini")
                        {
                            File.Copy(item.FullName, ParStd.PathStd, true);
                        }
                    }

                    RecoverPar_event();//刷新参数
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                WinMsgBox.ShowMsgBox("参数恢复失败", false);
            }
        }
        #endregion 恢复参数

        #region 关闭
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                g_WinRecover = null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 关闭
    }
}
