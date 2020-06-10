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
    /// UCCSTInsertConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UcCstInsertConfig : Page
    {
        public UcCstInsertConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ConfigManager.instance;
            lblInstruction.Content += "请根据实际功能需要，选择插栏起始列的位置。\n   以正对插栏轴且轴正方向靠右手方的位置进行判断。";
        }
    }
}
