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
    /// UCCSTPhotoConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UcCstCameraConfig : Page
    {
        public UcCstCameraConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ConfigManager.instance;
            lblInstruction.Content += "请根据机构实际情况，选择卡塞相机的朝向。\n   以正对插栏轴且轴正方向靠右手方的位置进行判断。";
        }
    }
}
