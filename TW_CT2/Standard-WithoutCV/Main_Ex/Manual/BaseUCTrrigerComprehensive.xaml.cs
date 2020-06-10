﻿using System;
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
using SetPar;
using System.Threading;
using System.Threading.Tasks;
using BasicComprehensive;
using DealConfigFile;
using Camera;
using DealImageProcess;
using DealPLC;
using DealLog;
using DealResult;
using DealAlgorithm;

namespace Main_EX
{
    /// <summary>
    /// BaseUCTrrigerComprehensive.xaml 的交互逻辑
    /// </summary>
    public partial class BaseUCTrrigerComprehensive : BaseUCTestRun
    {
        #region 定义
        //List       

        //int 
        int g_NoCamera = 0;
        int interval = 100;
        public override int Interval//循环时间间隔
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
            }
        }

        //bool
      

        //string
        string g_Pos = "1";
        int g_NoPos = 0;//当前位置不按照顺序触发

        //Class
        BaseDealComprehensiveResult g_BaseDealComprehensiveResult = null;

        UCStateWork g_UCStateWork = null;
        //定义事件    

        #endregion 定义

        #region 初始化
        public BaseUCTrrigerComprehensive()
        {
            InitializeComponent();

            NameClass = "BaseUCTrrigerComprehensive";

            //初始化控件
            InitDoubleUpDown(dudInterval);
            InitButton(btnSingle, btnRepeat);
            InitCheckBox(chkAutoNextLocal, chkAutoPreLocal);
            //事件注册
            LoginEvent();
        }


        public void Init(UCStateWork uCStateWork, BaseDealComprehensiveResult baseDealComprehensiveResult)
        {
            try
            {
                g_UCStateWork = uCStateWork;
                g_NoCamera = int.Parse(this.Tag.ToString());
                g_BaseDealComprehensiveResult = baseDealComprehensiveResult;
              
                base.g_UCDisplayCamera = g_BaseDealComprehensiveResult.g_UCDisplayCamera;

                //初始化所有的拍照位置
                InitCobPos();

                //初始化单次循环线程
                InitSingleRun();

                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 事件注册
        /// </summary>
        void LoginEvent()
        {
            try
            {
                uCTriggerCalib.TriggerCalib_event += new IntAction(uCTriggerCalib_TriggerCalib_event);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion 初始化

        #region 参数调整
        private void cboPos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboPos.IsMouseOver)
                {
                    g_Pos = (cboPos.SelectedValue.ToString()).Split(':')[1].Trim();                   
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }           
        }

        private void dudNoPos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudNoPos.IsMouseOver)
                {
                    g_NoPos = (int)dudNoPos.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }           
        }
        #endregion 参数调整

        #region 触发
        /// <summary>
        /// 单次触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void btnSingle_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                g_Pos = cboPos.Text;
                g_NoPos = (int)dudNoPos.Value;
                base.btnSingle_Click(sender, e);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 连续触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                g_Pos = cboPos.Text;//当前位置
                g_NoPos = (int)dudNoPos.Value;
                base.btnRepeat_Click(sender, e);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 选择运行当前相机的相关位置的算法
        /// </summary>
        public override void RunSingle()
        {
            try
            {
                switch (g_Pos)
                {
                    case "All":
                        for (int i = 0; i < ParCameraWork.P_I[g_NoCamera]; i++)
                        {
                            SelectPos(i + 1);
                            Thread.Sleep(Interval);
                        }
                        break;

                    default:
                        int pos = int.Parse(g_Pos);
                        SelectPos(pos);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void SelectPos(int pos)
        {
            try
            {
                if (g_NoPos == 0)
                {
                    g_BaseDealComprehensiveResult.DealComprehensiveResultFun(TriggerSource_enum.Null, pos);
                }
                else
                {
                    g_BaseDealComprehensiveResult.DealComprehensiveResultFun(TriggerSource_enum.Null, pos, g_NoPos);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 触发

        #region 显示
        //初始化位置
        void InitCobPos()
        {
            try
            {
                g_NoCamera = int.Parse(this.Tag.ToString());
                int numPos = ParCameraWork.P_I[g_NoCamera];//拍照位置总数
                cboPos.Items.Clear();
                for (int i = 0; i < numPos; i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = (i + 1).ToString();
                    cboPos.Items.Add(comboBoxItem);
                }
                ComboBoxItem cboItemAll = new ComboBoxItem();
                cboItemAll.Content = "All";
                cboPos.Items.Add(cboItemAll);
                cboPos.Text = "1";//默认为所有位置一起运行
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("BaseUCTrrigerComprehensive", ex);
            }
        }

        /// <summary>
        /// 显示参数
        /// </summary>
        public override void ShowPar()
        {
            try
            {
                this.gpRun.Header = "相机" + this.Tag.ToString();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("BaseUCTrrigerComprehensive", ex);
            }
        }
        #endregion 显示

        #region 循环停止   
        public override void CheckResult()
        {
            try
            {
                g_HtResult = g_BaseDealComprehensiveResult.g_BaseDealComprehensive.g_HtResult;
                int num = 0;
                foreach (string item in g_HtResult.Keys)
                {
                    if (g_HtResult[item] != null)
                    {
                        if (!(g_HtResult[item] is BaseResult))
                        {
                            continue;
                        }
                        BaseResult result = (BaseResult)g_HtResult[item];
                        if (result.LevelError_e == LevelError_enum.Error
                            && result.Pos.ToString() == g_Pos)//如果出差
                        {
                            num++;
                        }
                    }
                }
                if (num > 0
                    && StopRepeat_e == StopRepeat_enum.StopNG)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        btnRepeat_Click(null, null);
                    }));
                }

                if (num == 0
                    && StopRepeat_e == StopRepeat_enum.StopOK)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        btnRepeat_Click(null, null);
                    }));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        //NG停止
        private void chkNGStop_Checked(object sender, RoutedEventArgs e)
        {
            if (chkNGStop.IsMouseOver)
            {
                StopRepeat_e = StopRepeat_enum.StopNG;
                chkOKStop.IsChecked = false;
            }
        }
        private void chkNGStop_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkNGStop.IsMouseOver)
            {
                StopRepeat_e = StopRepeat_enum.Null;
            }
        }

        //OK停止
        private void chkOKStop_Checked(object sender, RoutedEventArgs e)
        {
            if (chkOKStop.IsMouseOver)
            {
                StopRepeat_e = StopRepeat_enum.StopOK;
                chkNGStop.IsChecked = false;
            }
        }

        private void chkOKStop_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkOKStop.IsMouseOver)
            {
                StopRepeat_e = StopRepeat_enum.Null;
            }
        }
        #endregion 循环停止

        #region 校准触发
        /// <summary>
        /// 打开关闭Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCalib_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (lblCalib.IsMouseOver)
                {
                    if (ppShow.IsOpen)
                    {
                        ppShow.IsOpen = false;
                    }
                    else
                    {
                        ppShow.IsOpen = true;

                        uCTriggerCalib.Init(g_BaseDealComprehensiveResult, ParCameraWork.P_I[g_NoCamera]);//初始化                        
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 触发校准事件响应
        /// </summary>
        /// <param name="index"></param>
        void uCTriggerCalib_TriggerCalib_event(int index)
        {
            try
            {
                g_Pos = index.ToString();
                base.btnSingle_Click(null, null);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 校准触发

    }
}
