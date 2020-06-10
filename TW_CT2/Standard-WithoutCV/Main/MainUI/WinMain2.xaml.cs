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
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using HalconDotNet;
using System.Drawing;
using DealRobot;
using DealPLC;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using DealComprehensive;
using SetPar;
using SetComprehensive;
using DealConfigFile;
using BasicClass;
using DealComInterface;
using ParComprehensive;
using DealDisplay;
using BasicDisplay;
using DealTool;
using DealLog;
using DealMedia;
using DealHelp;
using DealIO;
using System.Reflection;
using BasicComprehensive;
using DealAlgorithm;
using DealCalibrate;
using DealCommunication;
using DealDraw;
using DealGeometry;
using DealGrabImage;
using DealImageProcess;
using DealInOutput;
using DealMath;
using DealResult;
using DealWorkFlow;
using DealFile;
using Main_EX;
using NLog;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinMain2 : MainWindow
    {
        #region 定义


        #endregion 定义


        #region 其他
        /// <summary>
        /// CIM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiCim_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
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


        #region 右侧边栏

        #region 运行状态

        private void dpState_MouseEnter(object sender, MouseEventArgs e)
        {
            dpState.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void dpState_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ppState.IsOpen = !ppState.IsOpen;
            ppState.StaysOpen = ppState.IsOpen;
        }

        private void dpState_MouseLeave(object sender, MouseEventArgs e)
        {
            dpState.Background = new SolidColorBrush(Colors.White);
        }
        #endregion 运行状态

        #region 运行报警
        private void dpAlarm_MouseEnter(object sender, MouseEventArgs e)
        {
            dpAlarm.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void dpAlarm_MouseLeave(object sender, MouseEventArgs e)
        {
            dpAlarm.Background = new SolidColorBrush(Colors.White);
        }

        private void dpAlarm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ppAlarm.IsOpen = !ppAlarm.IsOpen;
            ppAlarm.StaysOpen = ppAlarm.IsOpen;
        }

        #endregion 运行报警

        #region 操作设置

        private void dpCom_MouseEnter(object sender, MouseEventArgs e)
        {
            dpCom.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void dpCom_MouseLeave(object sender, MouseEventArgs e)
        {
            dpCom.Background = new SolidColorBrush(Colors.White);
        }

        private void dpCom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ppManual.IsOpen = !ppManual.IsOpen;
            ppManual.StaysOpen = ppManual.IsOpen;
        }

        #endregion 操作设置

        #region 运行数据
        private void dpData_MouseEnter(object sender, MouseEventArgs e)
        {
            dpData.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void dpData_MouseLeave(object sender, MouseEventArgs e)
        {
            dpData.Background = new SolidColorBrush(Colors.White);
        }

        private void dpData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ppData.IsOpen = !ppData.IsOpen;
            ppData.StaysOpen = ppData.IsOpen;
        }
        #endregion 运行数据

        #endregion 右侧边栏


        private void InsertTempComR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                ModelParams.InsertTempComR = Math.Round((double)InsertTempComR.Value, 2);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void InsertTempComX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                ModelParams.InsertTempComX = Math.Round((double)InsertTempComX.Value, 2);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void ClearInsertTempCom()
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    InsertTempComX.Value = 0;
                    InsertTempComR.Value = 0;
                }));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!EngineerReturn())
            {
                return;
            }
            //LogicPLC.L_I.WriteRegData1((int)DataRegister1.RestartBot, 1);
        }

        private void BtnAutoCalib_Click(object sender, RoutedEventArgs e)
        {
            if (!EngineerReturn())
            {
                return;
            }
            // LogicRobot.L_I.WriteRobotCMD(ModelParams.cmd_PreciseAutoCalib);
        }

    }
}
