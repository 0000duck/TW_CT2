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
using Common;
using BasicClass;

namespace DealConfigFile
{
    /// <summary>
    /// SetWork.xaml 的交互逻辑
    /// </summary>
    public partial class WinSetWorkType : BaseWindow
    {
        #region 窗体单实例
        private static WinSetWorkType g_WinSetWorkType = null;
        public static WinSetWorkType GetWinInst()
        {
            if (g_WinSetWorkType == null)
            {
                g_WinSetWorkType = new WinSetWorkType();
            }
            return g_WinSetWorkType;
        }
        #endregion 窗体单实例

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public WinSetWorkType()
        {
            InitializeComponent();
            NameClass = "WinSetWorkType";
        }

        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowPar_Invoke();
        }
        #endregion 初始化
      
        #region 保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.Content = "保  存";
                for (int i = 0; i < ParSetWork.C_NumWork; i++)
                {
                    WorkSelect inst = dgSetWork.Items[i] as WorkSelect;
                    ParSetWork.P_I.WorkSelect_L[i] = inst;                
                }

                ParSetWork.P_I.WriteIniPar();
                this.btnSave.RefreshDefaultColor("保存成功", true);
            }
            catch (Exception ex)
            {
                this.btnSave.RefreshDefaultColor("保存失败", false);
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 保存

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void BaseMetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            g_WinSetWorkType = null;           
        }
        #endregion 关闭

        #region 显示
        public override void ShowPar()
        {
            try
            {
                dgSetWork.ItemsSource = ParSetWork.P_I.WorkSelect_L;
                dgSetWork.Items.Refresh();

                //非工程师和厂商权限
                if (Authority.Authority_e != Authority_enum.Engineer
                    && Authority.Authority_e != Authority_enum.Manufacturer)
                {
                    foreach (DataGridColumn item in dgSetWork.Columns)
                    {
                        if (item.Header.ToString().Contains("注释"))
                        {
                            item.IsReadOnly = true;
                        }
                    }
                }
                else
                {
                    foreach (DataGridColumn item in dgSetWork.Columns)
                    {
                        if (item.Header.ToString().Contains("注释"))
                        {
                            item.IsReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }            
        }
        #endregion 显示

        
    }    
}
