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
using Camera;
using DealImageProcess;
using DealPLC;
using DealLog;
using DealResult;
using DealAlgorithm;

namespace Main_EX
{
    /// <summary>
    /// UCTriggerCalib.xaml 的交互逻辑
    /// </summary>
    public partial class UCTriggerCalib : BaseControl
    {
        #region 定义
        BaseDealComprehensiveResult g_BaseDealComprehensiveResult = null;

        //事件
        public event IntAction TriggerCalib_event;
        #endregion 定义

        #region 初始化
        public UCTriggerCalib()
        {
            InitializeComponent();

            NameClass = "UCTriggerCalib";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="par">参数</param>
        /// <param name="numPos">拍照位置</param>
        public void Init(BaseDealComprehensiveResult par, int numPos)
        {
            try
            {
                g_BaseDealComprehensiveResult = par;

                cboPos.Items.Clear();
                for (int i = 0; i < 6; i++)//算法相对位置，非拍照位置
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = (i + 1).ToString();
                    cboPos.Items.Add(comboBoxItem);
                }

                cboPos.Text = "1";//默认为所有位置一起运行
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 触发
        private void btnTrigger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int noPos = cboPos.SelectedIndex;//算法序号，不是拍照位置
                int index = (int)dudNoTrigger.Value;//拍照顺序
                string type = cboType.Text;
                switch (type)
                {
                    case "旋转中心":
                        TriggerRotate(noPos, index);
                        break;

                    case "相机校准":
                        TriggerCamera(noPos, index);
                        break;

                    case "轴校准":
                        TriggerAxis(noPos, index);
                        break;

                    case "相机投影校准":
                        TriggerAffineCamera(noPos, index);
                        break;

                    case "多目校准":
                        TriggerMult(noPos, index);
                        break;

                    case "手眼校准":
                        TriggerHandEye(noPos, index);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 多目校准 1200
        /// </summary>
        void TriggerMult(int pos, int index)
        {
            try
            {
                int cmd = 1200 + index + pos * 1000;

                TriggerCalib_event(cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 触发相机和轴校准1300
        /// </summary>
        void TriggerAxis(int pos, int index)
        {
            try
            {
                int cmd = 1300 + index + pos * 1000;

                TriggerCalib_event(cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 触发旋转中心校准1400
        /// </summary>
        void TriggerRotate(int pos, int index)
        {
            try
            {
                int cmd = 1400 + index + pos * 1000;

                TriggerCalib_event(cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 手眼校准
        /// </summary>
        void TriggerHandEye(int pos, int index)
        {
            try
            {
                int cmd = 1500 + index + pos * 1000;

                TriggerCalib_event(cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机投影校准 1700
        /// </summary>
        void TriggerAffineCamera(int pos, int index)
        {
            try
            {
                int cmd = 1700 + index + pos * 1000;

                TriggerCalib_event(cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 触发相机标定板校准1800
        /// </summary>
        void TriggerCamera(int pos, int index)
        {
            try
            {
                int cmd = 1800 + index + pos * 1000;

                TriggerCalib_event(cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

      
        #endregion 触发
    }
}
