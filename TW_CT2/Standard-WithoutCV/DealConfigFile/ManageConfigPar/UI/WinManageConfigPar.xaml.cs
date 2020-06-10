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
using System.IO;
using Common;
using DealFile;
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ControlLib;
using DealLog;

namespace DealConfigFile
{
    /// <summary>
    /// WinManageConfigPar.xaml 的交互逻辑
    /// </summary>
    public partial class WinManageConfigPar : BaseWindow
    {
        #region 定义
        public event FdBlAction_del NewModel_event;//新建型号
        public event FdBlStrAction_del DeleteModel_event;//删除文件
        public event Action ChangeModel_event;//换型
        #endregion 定义

        #region 初始化
        public WinManageConfigPar()
        {
            InitializeComponent();

            NameClass = "WinManageConfigPar";
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ParManageConfigPar.P_I.ReadFileConfigList();
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

        /// <summary>
        /// 是否显示列表序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowNoDg_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tsbShowNoDg.IsMouseOver)
                {
                    ParConfigPar.P_I.BlShowNo = true;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }      
        }

        private void tsbShowNoDg_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tsbShowNoDg.IsMouseOver)
                {
                    ParConfigPar.P_I.BlShowNo = false;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }      
        }

        #endregion 参数调整

        #region 保存新文件
        /// <summary>
        /// 保存新文件
        /// </summary>
        void btnSaveNew_Click(object sender, RoutedEventArgs e)
        {
            string info = "新建成功";
            try
            {
                if (MessageBox.Show("是否保存为新的型号？", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    return;
                }

                //保存新文件
                if (SaveNew())
                {
                    //重新刷新配置文件列表
                    ShowModel();//型号名称   
  
                    ParManageConfigPar.P_I.ReadFileConfigList();
                    RefreshDgFileConfigList();

                    btnSaveNew.RefreshDefaultColor("新建成功", true);
                }
                else
                {
                    btnSaveNew.RefreshDefaultColor("新建失败", false);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                info = "新建失败";
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnSaveNew新建型号",
                "新建配置文件" + info);
            }
        }

        /// <summary>
        /// 保存新文件
        /// </summary>
        /// <returns></returns>
        bool SaveNew()
        {
            try
            {
                if (ComConfigPar.C_I.NameModel == txtModel.Text.Trim())
                {
                    MessageBox.Show("新建的配置文件已经存在，禁止再次创建!");
                    return false;
                }
                string newName = txtModel.Text;
                //判断是否存在非法字符
                int num = newName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars());
                if (num > -1)
                {
                    MessageBox.Show("新建型号失败，名称中含有非法字符：\\ / : * ？ \" < > |");
                    return false;
                }

                //如果使用文件编号,则文件名称里面包含了序号
                if (ComConfigPar.C_I.BlUsingNo)
                {
                    ComConfigPar.C_I.NoConfig = (int)dudNoModel.Value;
                    newName = ComConfigPar.C_I.NoConfig.ToString() + "#" + newName;
                }

                if (Directory.Exists(ParPathRoot.PathRoot + "Store\\" + newName))
                {
                    if (MessageBox.Show("该文件已经存在，是否直接加载该参数？", "确认信息", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        ComConfigPar.C_I.NameModel = newName;
                        //设置系统路径
                        ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;//记录旧型号路径
                        ComConfigPar.C_I.PathConfigIni = ComConfigPar.C_I.PathRoot + "Product.ini";//新型号的产品参数路径               
                        ChangeModel_event();//触发切换型号事件
                        return true;
                    }
                }
                
                ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;//记录旧型号路径
                ComConfigPar.C_I.NameModel = newName;

                //设置系统路径
                ComConfigPar.C_I.PathOldConfigIni = ComConfigPar.C_I.PathConfigIni;//记录旧型号路径
                ComConfigPar.C_I.PathConfigIni = ComConfigPar.C_I.PathRoot + "Product.ini";//新型号的产品参数路径             

                new Task(new Action(() =>
                    {
                        //触发主窗体新建型号的事件
                        if (!NewModel_event())
                        {

                        }
                    })).Start();


                //重新读取文件
                ParManageConfigPar.P_I.ReadFileConfigList();
                RefreshDgFileConfigList();
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 保存新文件

        #region 换型
        /// <summary>
        /// 打开新型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeModel_Click(object sender, RoutedEventArgs e)
        {
            string info = "切换成功";
            try
            {
                base.IndexP = dgFileList.SelectedIndex;
                ComConfigPar.C_I.PathConfigIni = ParManageConfigPar.P_I.FileConfigPar_L[base.IndexP].PathPar;
                ComConfigPar.C_I.NameModel = ParManageConfigPar.P_I.FileConfigPar_L[base.IndexP].Model;
                btnChangeModel.RefreshDefaultColor("正在换型", true);               
                  
                ShowModel();//显示型号
                ChangeModel_event();//触发换型事件
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                info = "切换失败";
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnChangeModel切换型号",
                "切换型号" + info);
            }
        }
        #endregion 换型

        #region 删除文件
        /// <summary>
        /// 删除选择的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string info = "删除成功";
            try
            {
                if (MessageBox.Show("是否删除文件?", "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    return;                  
                }
                string name = ParManageConfigPar.P_I.FileConfigPar_L[base.IndexP].Model;
                if (name == ComConfigPar.C_I.NameModel)
                {
                    MessageBox.Show("禁止删除当前正在使用的配置文件!");
                    return;
                }
                //触发删除文件事件
                if (!DeleteModel_event(name))
                {
                    btnDelete.RefreshDefaultColor("删除失败", false);
                    info = "删除失败";
                    return;
                }
                btnDelete.RefreshDefaultColor("删除成功", true);

                //重新读取文件列表
                ParManageConfigPar.P_I.ReadFileConfigList();
                //刷新文件列表
                RefreshDgFileConfigList();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                info = "删除失败";
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnDelete删除配置文件",
                "删除配置文件" + info);
            }
        }
        #endregion 删除文件

        #region 选择配置文件
        private void dgFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                base.IndexP = dgFileList.SelectedIndex;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 选择配置文件

        #region 显示
        //显示参数
        public override void ShowPar()
        {
            try
            {
                #region 显示参数
                ShowModel();//型号名称      
                RefreshDgFileConfigList();//刷新配置文件列表
                #endregion 显示参数

                #region 权限设置
                //厂商权限
                if (Authority.Authority_e == Authority_enum.Manufacturer)
                {
                    lblShowNoDg.Visibility = Visibility.Visible;
                    tsbShowNoDg.Visibility = Visibility.Visible;

                    tsbShowNoDg.IsChecked = ParConfigPar.P_I.BlShowNo;
                }

                //如果不显示列表序号
                if (!ParConfigPar.P_I.BlShowNo)
                {
                    foreach (DataGridColumn item in dgFileList.Columns)
                    {
                        if (item.Header.ToString()=="序号")
                        {
                            item.Visibility = Visibility.Hidden;
                            break;
                        }
                    }
                }

                base.SetBtnEnable(this.gdLayout, Authority.Authority_e);
                #endregion 权限设置
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //显示型号，即配置文件名称
        void ShowModel()
        {
            try
            {
                if (ComConfigPar.C_I.NameModel.Contains("#"))
                {
                    txtModel.Text = ComConfigPar.C_I.NameModel.Split('#')[1];
                }
                else
                {
                    txtModel.Text = ComConfigPar.C_I.NameModel;//保存型号名称
                }
                tsbNoConfig.IsChecked = ComConfigPar.C_I.BlUsingNo;//是否使用产品编号
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

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //存储到本地
            ParConfigPar.P_I.WriteIniParProduct();
        }
        #endregion 退出

       

        
        

     
    }
}
