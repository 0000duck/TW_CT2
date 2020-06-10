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
    /// add by xc-190401
    /// </summary>
    public class WinRecoverRobotStd : WinRecoverAdjust
    {
        #region 窗体单实例
        private static WinRecoverRobotStd g_WinRecover = null;
        public override string FileName { get { return "RobotStd"; } }
        public new static WinRecoverRobotStd GetWinInst(out bool blNew)
        {
            blNew = false;
            if (g_WinRecover == null)
            {
                blNew = true;
                g_WinRecover = new WinRecoverRobotStd();
            }
            return g_WinRecover;
        }
        #endregion 窗体单实例

        //定义事件
        /// <summary>
        /// 恢复参数
        /// </summary>
        public event Action RecoverPar_event;

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
