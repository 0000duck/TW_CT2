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

namespace DealConfigFile
{
    /// <summary>
    /// UCSetParAdjust.xaml 的交互逻辑
    /// </summary>
    public partial class WinSetParAdjust : BaseWindow
    {
        #region 定义
        public string g_Name = "";

        //定义事件
        public event Action ChangeInfo_event;
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public WinSetParAdjust()
        {
            InitializeComponent();
        }

        public void Init(string name)
        {
            try
            {
                g_Name = name;
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 保存
        /// <summary>
        /// 将修改后的参数保存到本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < ParSetAdjust.P_I.g_ParSetAdjust_L.Count; i++)
                {
                    BaseParSetAdjust baseParSetAdjust = (BaseParSetAdjust)dgSetAdjust.Items[i];
                    ParSetAdjust.P_I[g_Name][i] = baseParSetAdjust;
                }

                ParSetAdjust.P_I[g_Name,-1].Title = txtTitle.Text.Trim();//当前控件的标题
                ParSetAdjust.P_I[g_Name, -1].BlHidden = (bool)chbHidden.IsChecked;//是否隐藏控件

                if (g_Name.Contains("adj"))
                {
                    if ((bool)rdoSamll.IsChecked)
                    {
                        ParSetAdjust.P_I.TypeWinAdjust_e = TypeWinAdjust_enum.small;
                    }
                    else if ((bool)rdoNormal.IsChecked)
                    {
                        ParSetAdjust.P_I.TypeWinAdjust_e = TypeWinAdjust_enum.normal;
                    }
                    else
                    {
                        ParSetAdjust.P_I.TypeWinAdjust_e = TypeWinAdjust_enum.full;
                    }
                }
                else
                {
                    if ((bool)rdoSamll.IsChecked)
                    {
                        ParSetAdjust.P_I.TypeWinStd_e = TypeWinAdjust_enum.small;
                    }
                    else
                    {
                        ParSetAdjust.P_I.TypeWinStd_e = TypeWinAdjust_enum.normal;
                    }
                }

                ParSetAdjust.P_I.WriteIni(g_Name);

                btnSave.RefreshDefaultColor("保存成功", true);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 保存

        #region 显示
        /// <summary>
        /// 显示参数
        /// </summary>
        public override void ShowPar()
        {
            RefreshDatagrid();

            txtTitle.Text = ParSetAdjust.P_I[g_Name, -1].Title;//标题
            chbHidden.IsChecked = ParSetAdjust.P_I[g_Name, -1].BlHidden;//是否隐藏控件

            if (g_Name.Contains("std"))
            {
                chbHidden.Visibility = Visibility.Hidden;
            }

            if(g_Name.Contains("adj"))
            {
                if (ParSetAdjust.P_I.TypeWinAdjust_e == TypeWinAdjust_enum.small)
                {
                    rdoSamll.IsChecked = true;
                }
                else if (ParSetAdjust.P_I.TypeWinAdjust_e == TypeWinAdjust_enum.normal)
                {
                    rdoNormal.IsChecked = true;
                }
                else
                {
                    rdoFull.IsChecked = true;
                }
            }
            else
            {
                if (ParSetAdjust.P_I.TypeWinStd_e == TypeWinAdjust_enum.normal)
                {
                    rdoNormal.IsChecked = true;
                }
                else
                {
                    rdoSamll.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        public void RefreshDatagrid()
        {
            try
            {
                dgSetAdjust.ItemsSource = ParSetAdjust.P_I[g_Name];
                dgSetAdjust.Items.Refresh();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

        #region 退出
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ChangeInfo_event();//触发重置调整控件
        }

        #endregion 退出

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否确定重置当前单元内容？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ParSetAdjust.P_I[g_Name, -1].Title = txtTitle.Text = "";
                    ParSetAdjust.P_I.Reset(g_Name);
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }

    /// <summary>
    /// 小数点个数
    /// </summary>
    public enum TypeIncrement_e
    {
        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
    }
}
