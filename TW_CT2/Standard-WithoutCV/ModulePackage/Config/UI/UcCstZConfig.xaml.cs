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
    /// UcCSTZConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UcCstZConfig : Page
    {
        public UcCstZConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ConfigManager.instance;
            lblInstruction.Content += "请根据实际机构，选择是两轴插栏还是三轴插栏";
        }
    }
}
