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
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using DealFile;
using System.IO;
using System.Xml;
using System.Reflection;
using Common;

namespace Main_EX
{   
    /// <summary>
    /// UIInfoWork.xaml 的交互逻辑
    /// </summary>
    public partial class UCDisplayMainResult : BaseControl
    {
        #region 定义
        //bool 
        bool blChange = false;//运行状态发生变化

      
        //Mutex
        Mutex mutex = new Mutex();
        #endregion 定义

        #region 初始化
        public UCDisplayMainResult()
        {
            InitializeComponent();

            NameClass = "UCDisplayMainResult";
        }

        public void Init(string name)
        {
            try
            {
                FunDisplayMainResult.F_I.Init(name);

                blChange = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 增加信息
        public void AddInfo(string info,int index,bool blResult)
        {
            mutex.WaitOne();
            try
            {
                FunDisplayMainResult.F_I.AddInfo(info, index - 1, blResult);

                blChange = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            mutex.ReleaseMutex();
        }
        #endregion 增加信息      

        #region 清空
        private void lblClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mutex.WaitOne();
            try
            {
                for (int i = 0; i < FunDisplayMainResult.F_I.Count; i++)
                {
                    FunDisplayMainResult.F_I[i].Info = "";
                }
                
                blChange = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            mutex.ReleaseMutex();

        }
        #endregion 清空

        #region 查看本地日志
        private void lblOpenFolder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", FunDisplayMainResult.F_I.PathStateHour);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 查看本地日志        

        #region 显示
        public override void ShowPar_Invoke()
        {
            mutex.WaitOne();
            try
            {
                if (blChange)
                {
                    blChange = false;
                    this.Dispatcher.Invoke( new Action(ShowPar));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            mutex.ReleaseMutex();
        }
        //显示参数
        public override void ShowPar()
        {
            try
            {
                RefreshDgInfo();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //刷新产品参数
        void RefreshDgInfo()
        {
            try
            {
                dgInfo.ItemsSource = FunDisplayMainResult.F_I.FunDisplayMainResult_L;
                dgInfo.Items.Refresh();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示        

    }

    
}
