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
using DealFile;
using BasicClass;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using Common;
using System.Threading;
using System.Threading.Tasks;
using DealLog;

namespace DealConfigFile
{   
    /// <summary>
    /// CreatNcc.xaml 的交互逻辑
    /// </summary>
    public partial class WinSaveNewModel : BaseMetroWindow
    {
        #region 窗体单实例
        private static WinSaveNewModel g_WinSaveNewModel = null;
        public static WinSaveNewModel GetWinInst()
        {
            if (g_WinSaveNewModel == null)
            {
                g_WinSaveNewModel = new WinSaveNewModel();
            }
            return g_WinSaveNewModel;
        }
        #endregion 窗体单实例

        #region 定义     
        //定义事件
        public event FdBlAction_del NewModel_event;//新建型号
        public event Action ChangeModel_event;//切换型号
        #endregion 定义

        #region 初始化
        public WinSaveNewModel()
        {
            InitializeComponent();
            NameClass = "WinSaveNewModel";

            g_GdLayout = this.gdLayout;
        }

        public override void Init()
        {
            try
            {
                ParManageConfigPar.P_I.ReadFileConfigList();
               
                //显示参数
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化   
        
     
        #region 参数调整
        private void tsbNoConfig_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tsbNoConfig.IsMouseOver)
                {
                    ComConfigPar.C_I.BlUsingNo = true;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }            
        }

        private void tsbNoConfig_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tsbNoConfig.IsMouseOver)
                {
                    ComConfigPar.C_I.BlUsingNo = false;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }           
        }

        private void dgFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion 参数调整

        #region 保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string info = "新建成功";
            try
            {
                if (MessageBox.Show("是否保存为新的型号？", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    return;
                }
                string newName = txtNameModel.Text;
                //判断是否存在非法字符
                int num = newName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars());
                if (num > -1)
                {
                    MessageBox.Show("新建型号失败，名称中含有非法字符：\\ / : * ？ \" < > |");
                    return;
                }
                //如果使用文件编号,则文件名称里面包含了序号
                if (ComConfigPar.C_I.BlUsingNo)
                {
                    ComConfigPar.C_I.NoConfig = (int)dudNoModel.Value;
                    newName = ComConfigPar.C_I.NoConfig.ToString() + "#" + newName;
                }

                if (Directory.Exists(ParPathRoot.PathRoot+"Store\\" + newName))
                {
                    if (MessageBox.Show("该文件已经存在，是否直接加载该参数？", "确认信息", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        ComConfigPar.C_I.NameModel = newName;
                        //设置系统路径
                        ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;//记录旧型号路径
                        ComConfigPar.C_I.PathConfigIni = ComConfigPar.C_I.PathRoot + "Product.ini";//新型号的产品参数路径               
                        ChangeModel_event();//触发切换型号事件
                        return;
                    }                   
                }

                ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;//记录旧型号路径
                ComConfigPar.C_I.NameModel = newName;

                //设置系统路径
                ComConfigPar.C_I.PathConfigIni = ComConfigPar.C_I.PathRoot + "Product.ini";//新型号的产品参数路径               

                //触发主窗体新建型号的事件
                if (NewModel_event())
                {
                    btnSave.RefreshDefaultColor("新建型号成功！", true);

                    //重新读取文件
                    ParManageConfigPar.P_I.ReadFileConfigList();
                    RefreshDgFileConfigList();
                }
                else
                {
                    btnSave.RefreshDefaultColor("新建型号失败！", false);
                    info = "新建失败";
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                btnSave.RefreshDefaultColor("新建型号失败！", false);
                info = "保存失败";
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnSave新建型号","新建配置文件" + info);
            }
        }

        #endregion 保存

        #region 显示       
        public override void ShowPar()
        {
            try
            {
                RefreshDgFileConfigList();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }       

         /// <summary>
        /// 刷新配置文件列表
        /// </summary>
        void RefreshDgFileConfigList()
        {
            try
            {
                dgFileList.ItemsSource = ParManageConfigPar.P_I.FileConfigPar_L;
                dgFileList.Items.Refresh();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

        #region 退出
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
                g_WinSaveNewModel = null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 退出

    }
}
