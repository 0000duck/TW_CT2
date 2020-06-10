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
    /// UCDisplayConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UcDisplayConfig : Page
    {
        public UcDisplayConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ConfigManager.instance;
            lblInstruction.Content += "请按照图示，将本页中的参数设置成与软件中相机综合设置-相机图像显示-背光定位相机一致。";
        }
    }
}
