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
using System.IO;
using DealLog;
using DealFile;
using System.Threading;
using System.Threading.Tasks;

namespace DealConfigFile
{
    /// <summary>
    /// WinRecoverAdjust.xaml 的交互逻辑 20190123-zx
    /// </summary>
    public partial class WinRecoverAdjust : BaseWindow
    {
        #region 窗体单实例
        private static WinRecoverAdjust g_WinRecover = null;
        //xc-190401,为了子类可以直接调用相关函数，将Filename作为统一的参数传到虚函数中
        public virtual string FileName
        {
            get { return "Adjust"; }            
        }
        public static WinRecoverAdjust GetWinInst(out bool blNew)
        {
            blNew = false;
            if (g_WinRecover == null)
            {
                blNew = true;
                g_WinRecover = new WinRecoverAdjust();
            }
            return g_WinRecover;
        }
        #endregion 窗体单实例

        #region 定义
        public List<Adjust_Temp> Adjust_Temp_L = new List<Adjust_Temp>();//调整值临时值       

        //定义事件
        public event Action RecoverPar_event;//恢复参数

        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public WinRecoverAdjust()
        {
            InitializeComponent();

            NameClass = "WinRecover";
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        public virtual void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //edit by xc-190401，传入filename
            ReadDir(FileName);
        }
        #endregion 初始化

        /// <summary>
        /// 读取目录
        /// </summary>
        public void ReadDir(string type)
        {
            try
            {
                ParRecover.P_I.g_BaseParRecoverDir_L.Clear();
                DirectoryInfo rootInfo = new DirectoryInfo(FunBackup.F_I.RootBackup);

                foreach (DirectoryInfo dirRoot in rootInfo.GetDirectories())
                {
                    string path = dirRoot.FullName + "\\";
                    path += type; 

                    if (!Directory.Exists(path))
                    {
                        continue;
                    }
                    DirectoryInfo pathInfo = new DirectoryInfo(path);

                    foreach (DirectoryInfo item in pathInfo.GetDirectories())
                    {
                        BaseParRecoverDir inst = new BaseParRecoverDir();
                        inst.Date = dirRoot.Name;
                        inst.Time = item.Name;
                        inst.Path = item.FullName;
                        inst.Reason = ReadExplain(item.FullName)[2] + ":" + ReadExplain(item.FullName)[0];
                        inst.PathSource = ReadExplain(item.FullName)[1];
                        ParRecover.P_I.g_BaseParRecoverDir_L.Add(inst);
                    }
                }

                for (int i = 0; i < ParRecover.P_I.g_BaseParRecoverDir_L.Count; i++)
                {
                    ParRecover.P_I.g_BaseParRecoverDir_L[i].No = i + 1;
                }

                //如果有备份文件夹，显示文件
                if (ParRecover.P_I.g_BaseParRecoverDir_L.Count > 0)
                {
                    ReadFile(ParRecover.P_I.g_BaseParRecoverDir_L[0], type);                
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                ShowDir();
            }
        }

        /// <summary>
        /// 读取注释
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string[] ReadExplain(string path)
        {
            try
            {
                string pathSource = "";
                string reason = "";
                string model = "";
                if (File.Exists(path + "\\备份说明.ini"))
                {
                    reason = IniFile.I_I.ReadIniStr("Explain", "Reason", path + "\\备份说明.ini");
                    pathSource = IniFile.I_I.ReadIniStr("Explain", "Source", path + "\\备份说明.ini");
                    model = IniFile.I_I.ReadIniStr("Explain", "Model", path + "\\备份说明.ini");
                }
                return new string[3] { reason, pathSource, model };
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }

        /// <summary>
        /// 选择文件目录查看文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public virtual void dgDir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (dgDir.IsMouseOver)
                {
                    //edit by xc-190401,传入filename
                    ReadFile(ParRecover.P_I.g_BaseParRecoverDir_L[dgDir.SelectedIndex], FileName);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 读取文件参数
        /// </summary>
        public virtual void ReadFile(BaseParRecoverDir baseParRecoverDir, string type)
        {
            try
            {
                new Task(new Action(() =>
                {
                    ParRecover.P_I.g_BaseParRecoverFile_L.Clear();
                    if (baseParRecoverDir == null)
                    {
                        return;
                    }

                    string path = baseParRecoverDir.Path + "\\" + type + ".ini";
                    Adjust_Temp_L.Clear();

                    string typeSection = "adj";

                    //edit by xc-190401，增加两个选择
                    switch (type)
                    {
                        case "Adjust":
                            typeSection = "adj";
                            break;
                        case "Std":
                            typeSection = "std";
                            break;
                        case "RobotStd":
                            typeSection = "rpstd";
                            break;
                        case "RobotAdj":
                            typeSection = "rpadj";
                            break;
                    }

                    for (int i = 0; i < 18; i++)
                    {
                        string section = typeSection + (i + 1).ToString();
                        double value1 = IniFile.I_I.ReadIniDbl(section, "Value1", path);
                        double value2 = IniFile.I_I.ReadIniDbl(section, "Value2", path);
                        double value3 = IniFile.I_I.ReadIniDbl(section, "Value3", path);
                        string anno = IniFile.I_I.ReadIniStr(section, "Annotation", path);
                        Adjust_Temp_L.Add(new Adjust_Temp()
                        {
                            No = i,
                            Name = section,
                            Value1 = value1,
                            Value2 = value2,
                            Value3 = value3,
                            Annotation = anno,
                        });
                    }

                    //显示文件
                    ShowFile();

                })).Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        #region 恢复参数
        /// <summary>
        /// 刷新历史参数的目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //edit by xc-190401，传入filename
                ReadDir(FileName);
                btnRefresh.RefreshDefaultColor("刷新成功", true);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 恢复参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnRecover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FunBackup.F_I.BackupAdjust();//备份调整值

                base.IndexP = this.dgDir.SelectedIndex;
                if (WinMsgBox.ShowMsgBox("使用备份参数永久覆盖当前所有调整值，是否继续？"))
                {
                    DirectoryInfo theFolder = new DirectoryInfo(ParRecover.P_I.g_BaseParRecoverDir_L[base.IndexP].Path);
                    foreach (FileInfo item in theFolder.GetFiles())
                    {
                        //edit by xc-190401,传入filename
                        if (item.Name == FileName + ".ini")
                        {
                            File.Copy(item.FullName, ParAdjust.PathAdjust, true);
                        }
                    }

                    RecoverPar_event();//刷新参数
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                WinMsgBox.ShowMsgBox("参数恢复失败", false);
            }
        }
        #endregion 恢复参数

        #region 显示
        /// <summary>
        /// 显示目录
        /// </summary>
        public void ShowDir()
        {
            try
            {
                dgDir.ItemsSource = ParRecover.P_I.g_BaseParRecoverDir_L;
                dgDir.Items.Refresh();
                dgDir.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }



        /// <summary>
        /// 显示文件
        /// </summary>
        public void ShowFile()
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    dgFile.ItemsSource = Adjust_Temp_L;
                    dgFile.Items.Refresh();
                }));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示    

        #region 关闭
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                g_WinRecover = null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 关闭

       
    }

    /// <summary>
    /// 调整值临时变量
    /// </summary>
    public class Adjust_Temp : BaseClass
    {
        public string Name { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
    }
}
