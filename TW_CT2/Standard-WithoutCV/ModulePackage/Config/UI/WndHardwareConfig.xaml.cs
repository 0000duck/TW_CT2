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

namespace ModulePackage
{
    /// <summary>
    /// UCConfig.xaml 的交互逻辑
    /// </summary>
    public partial class WndHardwareConfig : Window
    {
        private static WndHardwareConfig instance = null;
        public static WndHardwareConfig GetInstance()
        {
            if (instance == null)
                instance = new WndHardwareConfig();
            return instance;
        }

        private WndHardwareConfig()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("是否确认保存当前配置？", "提示", MessageBoxButton.YesNo)== MessageBoxResult.Yes)
            {
                ConfigManager.instance.WriteConfig();
            }
            this.Close();
        }

        private void RbtnDisplay_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcDisplayConfig() };
        }
        private void RbtnBL_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcBLConfig() };
        }

        private void RbtnPlatform_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcPlatformConfig() };
        }

        private void RbtnCstPhoto_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcCstCameraConfig() };
        }

        private void RbtnCstInsert_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcCstInsertConfig() };
        }

        private void RbtnZ_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcCstZConfig() };
        }

        private void RbtnBot_Checked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Frame { Content = new UcBotConfig() };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
