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
using BasicClass;
using DealRobot;
using Main_EX;
using StationDataManager;

namespace Main
{
    /// <summary>
    /// UCRbtTeachComprehensive.xaml 的交互逻辑
    /// </summary>
    public partial class UCRbtTeachComprehensive : UserControl
    {

        #region 变量
        public Point4D PosCur { get; set; } = new Point4D();
        public double XStep { get; set; } = 3;
        public double YStep { get; set; } = 3;
        public double ZStep { get; set; } = 3;

        public List<StInfo> g_StInfo = new List<StInfo>();

        bool BlTeachXY = false;
        bool blTeachZ = false;

        string NameClass = "UCRbtTeachComprehensive";
        #endregion


        public UCRbtTeachComprehensive()
        {
            InitializeComponent();
            DataContext = this;
            LogicRobot.L_I.JogPosCurr_event += GetCurPos;
            this.Dispatcher.Invoke(new Action(() =>
            {
                EnableDriveButton(false);
                btSaveCurXY.IsEnabled = false;
                btSaveCurZ.IsEnabled = false;
            }));
            dgStInfo.ItemsSource = g_StInfo;
            AddStInfo();
            dgStInfo.Items.Refresh();
        }

        public void AddStInfo()
        {
            try
            {
                g_StInfo.Clear();
                for (int i =0;i<6;++i)
                {
                    StInfo sp = new StInfo();
                    switch (i)
                    {
                        case 0:
                            sp.NameSt = "1-1";
                            break;
                        case 1:
                            sp.NameSt = "1-2";
                            break;
                        case 2:
                            sp.NameSt = "2-1";
                            break;
                        case 3:
                            sp.NameSt = "2-2";
                            break;
                        case 4:
                            sp.NameSt = "3-1";
                            break;
                        case 5:
                            sp.NameSt = "3-2";
                            break;
                    }
                    sp.XSt = StationDataMngr.instance.PlacePos_L[i].DblValue1;
                    sp.YSt = StationDataMngr.instance.PlacePos_L[i].DblValue2;
                    sp.ZSt = StationDataMngr.instance.PlacePos_L[i].DblValue3;
                    g_StInfo.Add(sp);
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void GetCurPos(double[] pos)
        {
            try
            {
                PosCur.DblValue1 = pos[0];
                PosCur.DblValue2 = pos[1];
                PosCur.DblValue3 = pos[2];
                this.Dispatcher.Invoke(new Action(() =>
                {
                    lblCurX.Content = pos[0];
                    lblCurY.Content = pos[1];
                    lblCurZ.Content = pos[2];
                    EnableDriveButton(true);

                    btXMinus.IsEnabled = !blTeachZ;
                    btXPlus.IsEnabled = !blTeachZ;
                    btYMinus.IsEnabled = !blTeachZ;
                    btYPlus.IsEnabled = !blTeachZ;

                    if (LogicRobot.L_I.BlPmtJog)
                    {
                        if (BlTeachXY)
                        {
                            btSaveCurXY.IsEnabled = true;
                        }
                        else
                        {
                            btSaveCurZ.IsEnabled = true;
                        }
                    }
                }));       
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void BtnCtrs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                string content = button.Content as string;
                Point4D pCmd = new Point4D();
                switch(content)
                {
                    case "Y↑":
                        pCmd = new Point4D(0, YStep, 0, 0);
                        break;
                    case "Y↓":
                        pCmd = new Point4D(0, -YStep, 0, 0);
                        break;
                    case "←X":
                        pCmd = new Point4D(-XStep, 0, 0, 0);
                        break;
                    case "X→":
                        pCmd = new Point4D(XStep, 0, 0, 0);
                        break;
                    case "Z↑":
                        pCmd = new Point4D(0, 0, ZStep, 0);
                        break;
                    case "Z↓":
                        pCmd = new Point4D(0, 0, -ZStep, 0);
                        break;
                }
                LogicRobot.L_I.WriteRobotCMD(pCmd, PCToRbt.PRP_Drive);
                EnableDriveButton(false);//使能控件,等待外部信号重新使能

            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void EnableDriveButton(bool isEnable)
        {
            try
            {
                btXMinus.IsEnabled = isEnable;
                btXPlus.IsEnabled = isEnable;
                btYMinus.IsEnabled = isEnable;
                btYPlus.IsEnabled = isEnable;
                btZMinus.IsEnabled = isEnable;
                btZPlus.IsEnabled = isEnable;
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void btStartTeachXY_Click(object sender, RoutedEventArgs e)
        {
            if (WinRobotCalib.GetWinInst.GetCurSt() < 0)
            {
                return;
            }
            if (MessageBox.Show("请确认是否示教XY_工位:"+ WinRobotCalib.GetWinInst.GetCurStName(),"确认信息",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (MessageBox.Show("请确认是否安装了示教针!","确认信息",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (MessageBox.Show("请确认机台内没有人员!","确认信息",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        WinRobotCalib.GetWinInst.EnableRb(false);
                        LogicRobot.L_I.WriteRobotCMD(new Point4D(WinRobotCalib.GetWinInst.GetCurSt(), 0, 0, 0), PCToRbt.PRP_StartTeachXY);
                        BaseDealComprehensiveResult.ShowState("PC->Rbt,开始示教XY_" + WinRobotCalib.GetWinInst.GetCurSt());
                        BlTeachXY = true;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            btStartTeachXY.IsEnabled = false;
                            btStartTeachZ.IsEnabled = false;
                        }));
                    }
                }
            }
        }

        private void btStartTeachZ_Click(object sender, RoutedEventArgs e)
        {
            if (WinRobotCalib.GetWinInst.GetCurSt() < 0)
            {
                return;
            }
            if (MessageBox.Show("请确认是否示教Z_工位:" + WinRobotCalib.GetWinInst.GetCurStName(), "确认信息", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (MessageBox.Show("请确认是否安装了示教针!", "确认信息", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (MessageBox.Show("请确认机台内没有人员!", "确认信息", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        WinRobotCalib.GetWinInst.EnableRb(false);
                        LogicRobot.L_I.WriteRobotCMD(new Point4D(WinRobotCalib.GetWinInst.GetCurSt(), 0, 0, 0), PCToRbt.PRP_StartTeachZ);
                        BaseDealComprehensiveResult.ShowState("PC->Rbt,开始示教Z_" + WinRobotCalib.GetWinInst.GetCurSt());
                        blTeachZ = true;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            btStartTeachXY.IsEnabled = false;
                            btStartTeachZ.IsEnabled = false;
                        }));
                    }
                }
            }
        }

        private void btSaveCurXY_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确认保存当前位置XY","确认信息",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                StationDataMngr.instance.PlacePos_L[WinRobotCalib.GetWinInst.GetCurSt() - 1].DblValue1 = PosCur.DblValue1;
                StationDataMngr.instance.PlacePos_L[WinRobotCalib.GetWinInst.GetCurSt() - 1].DblValue2 = PosCur.DblValue2;
                StationDataMngr.instance.WriteIniPlacePos(WinRobotCalib.GetWinInst.GetCurSt());
                LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_FinishTeachXY);
                BaseDealComprehensiveResult.ShowState("PC->Rbt,示教XY结束");
                WinRobotCalib.GetWinInst.EnableRb(true);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    EnableDriveButton(false);
                    AddStInfo();
                    dgStInfo.Items.Refresh();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        btSaveCurXY.IsEnabled = false;
                        btSaveCurZ.IsEnabled = false;
                        btStartTeachXY.IsEnabled = true;
                        btStartTeachZ.IsEnabled = true;
                    }));
                }));
                BlTeachXY = false;
            }
        }

        private void btSaveCurZ_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确认保存当前位置Z", "确认信息", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                StationDataMngr.instance.PlacePos_L[WinRobotCalib.GetWinInst.GetCurSt() - 1].DblValue3 = PosCur.DblValue3;
                StationDataMngr.instance.WriteIniPlacePos(WinRobotCalib.GetWinInst.GetCurSt());
                LogicRobot.L_I.WriteRobotCMD(PCToRbt.PRP_FinishTeachZ);
                BaseDealComprehensiveResult.ShowState("PC->Rbt,示教Z结束");
                WinRobotCalib.GetWinInst.EnableRb(true);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    EnableDriveButton(false);
                    AddStInfo();
                    dgStInfo.Items.Refresh();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        btSaveCurXY.IsEnabled = false;
                        btSaveCurZ.IsEnabled = false;
                        btStartTeachXY.IsEnabled = true;
                        btStartTeachZ.IsEnabled = true;

                        EnableDriveButton(false);
                    }));
                }));
                blTeachZ = false;
            }
        }
    }

    public class StInfo
    {
        public string NameSt { set; get; }
        public double XSt { set; get; }
        public double YSt { set; get; }
        public double ZSt { set; get; }
    }
}
