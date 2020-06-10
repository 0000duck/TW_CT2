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
using DealRobot;
using BasicClass;
using DealPLC;
using System.Threading;

namespace Main
{
    /// <summary>
    /// UCCalibSt.xaml 的交互逻辑
    /// </summary>
    public partial class UCCalibSt : UserControl
    {
        public UCCalibSt()
        {
            InitializeComponent();
        }

        private void btStartCalib_Click(object sender, RoutedEventArgs e)
        {
            if (WinRobotCalib.GetWinInst.GetCurSt()<0)
            {
                return;
            }
            if (MessageBox.Show("是否确认开始工位"+WinRobotCalib.GetWinInst.GetCurStName()+"的标定?","确认信息",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (MessageBox.Show("是否确认机台内没有人员，且标定针已经拆下?","确认信息",MessageBoxButton.OKCancel)== MessageBoxResult.OK)
                {
                    ////通知plc标定的工位号
                    //LogicPLC.L_I.WriteRegData1((int)DataRegister1.CalibStation, WinRobotCalib.GetWinInst.GetCurSt());
                    //Thread.Sleep(1000);
                    LogicRobot.L_I.WriteRobotCMD(new Point4D(WinRobotCalib.GetWinInst.GetCurSt(), 0, 0, 0), PCToRbt.PRP_StartCalibSt);
                    WinRobotCalib.GetWinInst.EnableRb(false);
                }
            }
        }

    }
}
