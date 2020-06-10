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
using SetPar;
using System.Threading;
using System.Threading.Tasks;
using BasicComprehensive;
using DealConfigFile;
using DealPLC;
using DealLog;

namespace Main_EX
{
    /// <summary>
    /// WinTrrigerComprehensive.xaml 的交互逻辑
    /// </summary>
    public partial class WinTrrigerComprehensive : BaseWinComprehensive
    {
        #region 窗体单实例
        private static WinTrrigerComprehensive g_WinTrrigerComprehensive = null;
        public static WinTrrigerComprehensive GetWinInst(out bool blNew)//是否新创建实例
        {
            blNew = false;
            if (g_WinTrrigerComprehensive == null)
            {
                blNew = true;
                g_WinTrrigerComprehensive = new WinTrrigerComprehensive();
            }
            return g_WinTrrigerComprehensive;
        }
        #endregion 窗体单实例

        #region 定义
        //List
        public List<CmdTrringer> CmdTrringer_L = new List<CmdTrringer>();

        //bool 
       
        //int 
     

        //定义事件      
       
        #endregion 定义

        #region 初始化
        public WinTrrigerComprehensive()
        {
            InitializeComponent();

            //初始化控件位置
            LocationRight();
            Login_Event();//事件注册

            NameClass = "WinTrrigerComprehensive";
        }

        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void Init(UCStateWork uCStateWork, BaseDealComprehensiveResult[] baseDealComprehensiveResult)
        {
            try
            {
                baseUCTrrigerComprehensive1.Init(uCStateWork, baseDealComprehensiveResult[0]);
                baseUCTrrigerComprehensive2.Init(uCStateWork, baseDealComprehensiveResult[1]);
                baseUCTrrigerComprehensive3.Init(uCStateWork, baseDealComprehensiveResult[2]);
                baseUCTrrigerComprehensive4.Init(uCStateWork, baseDealComprehensiveResult[3]);
                baseUCTrrigerComprehensive5.Init(uCStateWork, baseDealComprehensiveResult[4]);
                baseUCTrrigerComprehensive6.Init(uCStateWork, baseDealComprehensiveResult[5]);
                baseUCTrrigerComprehensive7.Init(uCStateWork, baseDealComprehensiveResult[6]);
                baseUCTrrigerComprehensive8.Init(uCStateWork, baseDealComprehensiveResult[7]);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #region 事件注册
        void Login_Event()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 事件注册
        #endregion 初始化

        #region 触发响应方法          
     
        #endregion 触发响应方法

        #region 显示

        #endregion 显示

        #region 退出
        /// <summary>
        /// 退出窗体，先退出实时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }           
        }

        private void BaseMetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (
                       baseUCTrrigerComprehensive1.Close()
                    && baseUCTrrigerComprehensive2.Close()
                    && baseUCTrrigerComprehensive3.Close()
                    && baseUCTrrigerComprehensive4.Close()
                    && baseUCTrrigerComprehensive5.Close()
                    && baseUCTrrigerComprehensive6.Close()
                    && baseUCTrrigerComprehensive7.Close()
                    && baseUCTrrigerComprehensive8.Close()
                    )
                {
                    g_WinTrrigerComprehensive = null;
                }
                else
                {
                    e.Cancel = true;                    
                }
                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 退出

    }

    public class CmdTrringer
    {
        public int No { get; set; }
        public int NoCamera { get; set; }
        public int NoPos { get; set; }
        public string Annotation { get; set; }
    }
}
