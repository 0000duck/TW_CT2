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

namespace DealIO
{
    /// <summary>
    /// WinDIO.xaml 的交互逻辑
    /// </summary>
    public partial class WinDIO : BaseMetroWindow
    {
        #region 初始化
        public WinDIO()
        {
            InitializeComponent();
        }

        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowPar_Invoke();
        }
        #endregion 初始化

        #region 写入IO
        private void btnSetIO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int channel = cboChannel.SelectedIndex;
                DIOControler.D_I.Write_DO_Bit((short)channel, (bool)tsbOn.IsChecked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入失败");
            }
        }
        #endregion 写入IO

        #region 保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParDIO.P_I.Port = cboPort.Text;
                ParDIO.P_I.WriteIniPar();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 保存

        #region 显示
        public override void ShowPar()
        {
            cboPort.Text = ParDIO.P_I.Port;
        }
        #endregion 显示

        #region 退出
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion 退出

        
    }
}
