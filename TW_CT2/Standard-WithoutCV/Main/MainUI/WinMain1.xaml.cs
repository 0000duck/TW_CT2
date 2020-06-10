using BasicClass;
using DealCIM;
using System;
using System.Windows;
using DealPLC;
using DealRobot;
using ModulePackage;
using Main_EX;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinMain1 : MainWindow
    {
        #region 定义        

        #endregion 定义

        #region 其他

        #endregion 其他

        #region CIM模拟

        #endregion CIM模拟

        #region 操作设置
        private void epSetWork_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void epSetWork_Expanded(object sender, RoutedEventArgs e)
        {

        }
        #endregion 操作设置         


     

        private void cmiRbtCalib_Click(object sender, RoutedEventArgs e)
        {
            if (ParStateSoft.StateMachine_e == StateMachine_enum.Calib)
            {
                WinRobotCalib.GetWinInst.Show();
            }
            else
            {
                ShowWinError_Invoke("请在PLC面板切至标定模式!");
            }
        }

        private void MHardwareConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!EngineerReturn())
            {
                return;
            }

            WndHardwareConfig wnd = WndHardwareConfig.GetInstance();
            if (!wnd.IsVisible)
                wnd.Show();
        }
    }
}
