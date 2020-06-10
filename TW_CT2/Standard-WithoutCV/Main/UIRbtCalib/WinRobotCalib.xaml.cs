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
using System.Windows.Shapes;
using BasicClass;

namespace Main
{
    /// <summary>
    /// WinRobotCalib.xaml 的交互逻辑
    /// </summary>
    public partial class WinRobotCalib : Window
    {
        string NameClass = "WinRobotCalib";
        public WinRobotCalib()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static WinRobotCalib g_WinRobotCalib = null;

        public static WinRobotCalib GetWinInst
        {
            get
            {
                if (g_WinRobotCalib == null)
                {
                    g_WinRobotCalib = new WinRobotCalib();
                }
                return g_WinRobotCalib;
            }
        }

        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            g_WinRobotCalib = null;
        }

        #region 返回当前选择的radiobutton的工位号
        /// <summary>
        /// 返回当前的工位号
        /// </summary>
        /// <returns></returns>
        public int GetCurSt()
        {
            try
            {
                if ((bool)rbSelectSt1_1.IsChecked)
                {
                    return 1;
                }
                else if ((bool)rbSelectSt1_2.IsChecked)
                {
                    return 2;
                }
                else if ((bool)rbSelectSt2_1.IsChecked)
                {
                    return 3;
                }
                else if ((bool)rbSelectSt2_2.IsChecked)
                {
                    return 4;
                }
                else if ((bool)rbSelectSt3_1.IsChecked)
                {
                    return 5;
                }
                else if ((bool)rbSelectSt3_2.IsChecked)
                {
                    return 6;
                }
                else
                {
                    return -1;
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return -1;
            }
        }

        public string GetCurStName()
        {
            try
            {
                switch(GetCurSt())
                {
                    case 1:
                        return "1-1";
                    case 2:
                        return "1-2";
                    case 3:
                        return "2-1";
                    case 4:
                        return "2-2";
                    case 5:
                        return "3-1";
                    case 6:
                        return "3-2";
                    default:
                        return "-1";
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return "-1";
            }
        }
        #endregion

        #region 使能工位按钮
        public void EnableRb(bool isEnable)
        {
            try
            {
                rbSelectSt1_1.IsEnabled = isEnable;
                rbSelectSt1_2.IsEnabled = isEnable;
                rbSelectSt2_1.IsEnabled = isEnable;
                rbSelectSt2_2.IsEnabled = isEnable;
                rbSelectSt3_1.IsEnabled = isEnable;
                rbSelectSt3_2.IsEnabled = isEnable;
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion


    }


}
