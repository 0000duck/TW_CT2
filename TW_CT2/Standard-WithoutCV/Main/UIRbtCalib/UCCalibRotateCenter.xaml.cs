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
using DealPLC;
using DealRobot;
using BasicClass;

namespace Main
{
    /// <summary>
    /// UCCalibRotateCenter.xaml 的交互逻辑
    /// </summary>
    public partial class UCCalibRotateCenter : UserControl
    {
        public double RotateAngle { get; set; } = 0.5;
        public double RCPxlGate { get; set; } = 10;
        public double TimesPhoto { get; set; } = 10;
        public UCCalibRotateCenter()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btCalibRotateCenter_Click(object sender, RoutedEventArgs e)
        {
            if(WinRobotCalib.GetWinInst.GetCurSt()<0)
            {
                return;
            }
            if (MessageBox.Show("确认是否进行旋转中心标定?", "确认信息", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LogicRobot.L_I.WriteRobotCMD(new Point4D(WinRobotCalib.GetWinInst.GetCurSt(), 0, 0, 0), PCToRbt.PRP_StartCalibRC);
            }
        }
    }
}
