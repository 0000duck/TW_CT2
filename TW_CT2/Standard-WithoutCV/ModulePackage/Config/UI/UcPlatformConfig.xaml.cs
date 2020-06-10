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
    /// UCPlaceConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UcPlatformConfig : Page
    {
        public UcPlatformConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ConfigManager.instance;
            lblInstruction.Content += "此处只选择工位1的放片位置，以及单电极时电极的朝向。\n    假如单电极放片时电极边在左侧或者右侧，则勾选单电极水平放置.";
        }
    }
}
