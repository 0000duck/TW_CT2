using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using BasicClass;
using System.Collections;

namespace DealConfigFile
{
    public class ParSetAdjust : BaseClass
    {
        #region 静态类实例
        public static ParSetAdjust P_I = new ParSetAdjust();
        #endregion 静态类实例

        #region 定义
        //Path
        public static string C_PathSavePar = ParPathRoot.PathRoot + "Store\\AdjustStd\\SetParAdjust.ini";
        //int
        const int C_NumValue = 4;//总共四个调整值

        //string
        public string g_Section = "";

        public string g_Title = "";
        public string Title = "";//标题
       

        //是否隐藏
        public bool BlHidden = false;        

        public TypeWinAdjust_enum TypeWinAdjust_e = TypeWinAdjust_enum.normal;//调整值窗体大小
        public TypeWinAdjust_enum TypeWinStd_e = TypeWinAdjust_enum.normal;//基准值窗体大小

        //List
        /// <summary>
        ///四个调整值相关的设置参数
        /// </summary>
        public List<BaseParSetAdjust> g_ParSetAdjust_L = new List<BaseParSetAdjust>();

        //索引器
        public List<BaseParSetAdjust> this[string section]
        {
            get
            {                
                if(g_HtAdjConfig.Contains(section))
                {
                    return ((ParSetAdjust)g_HtAdjConfig[section]).g_ParSetAdjust_L;
                }
                return null;
            }
        }

        public ParSetAdjust this[string section, int index]
        {
            get
            {
                if (g_HtAdjConfig.Contains(section))
                {
                    return ((ParSetAdjust)g_HtAdjConfig[section]);
                }
                return null;
            }
        }

        //哈希表调整值配置
        Hashtable g_HtAdjConfig = new Hashtable();
        #endregion 定义

        #region 初始化
        public ParSetAdjust()
        {
            NameClass = "ParSetAdjust";

            try
            {
                int type = IniFile.I_I.ReadIniInt("Adjust", "TypeWinAdjust", 2, C_PathSavePar);
                TypeWinAdjust_e = (TypeWinAdjust_enum)type;

                int typeStd = IniFile.I_I.ReadIniInt("Adjust", "TypeWinStd", 2, C_PathSavePar);
                TypeWinStd_e = (TypeWinAdjust_enum)type;
            }
            catch (Exception ex)
            {

            }           
        }
        #endregion 初始化

        #region 读取参数
        /// <summary>
        /// 从Ini文件中读取参数
        /// </summary>
        public void ReadIniStr()
        {
            try
            {
                g_ParSetAdjust_L.Clear();  //title

                for (int i = 0; i < C_NumValue; i++)
                {
                    string basekey = "Value" + (i + 1).ToString();

                    //名称
                    string name = IniFile.I_I.ReadIniStr(g_Section, "Name" + basekey, C_PathSavePar);
                    if (name == "")
                    {
                        name = "Value" + (i + 1).ToString();
                    }

                    //小数点个数
                    string strIncrement = IniFile.I_I.ReadIniStr(g_Section, "Increment" + basekey, C_PathSavePar);
                    if (!strIncrement.Contains("Num"))
                    {
                        strIncrement = "Num2";
                    }


                    //最大最小值
                    base.StrDouble = IniFile.I_I.ReadIniStr(g_Section, "Min" + basekey, C_PathSavePar);
                    double min = Convert.ToDouble(base.StrDouble);
                    base.StrDouble = IniFile.I_I.ReadIniStr(g_Section, "Max" + basekey, C_PathSavePar);
                    double max = Convert.ToDouble(base.StrDouble);
                    if (min == 0
                        && max == 0)
                    {
                        min = int.MinValue;
                        max = int.MaxValue;
                    }

                    //权限
                    bool blWorker = true;
                    string strWorker = IniFile.I_I.ReadIniStr(g_Section, "Worker" + basekey, C_PathSavePar);
                    if (strWorker == "")
                    {

                    }
                    else
                    {
                        try
                        {
                            blWorker = Boolean.Parse(strWorker);
                        }
                        catch (Exception)
                        {

                            blWorker = true;
                        }
                    }

                    bool blEngineer = true;
                    string strEngineer = IniFile.I_I.ReadIniStr(g_Section, "Engineer" + basekey, C_PathSavePar);
                    if (strEngineer == "")
                    {

                    }
                    else
                    {
                        try
                        {
                            blEngineer = Boolean.Parse(strEngineer);
                        }
                        catch (Exception)
                        {
                            blEngineer = true;
                        }
                    }

                    BaseParSetAdjust baseParSetAdjust = new BaseParSetAdjust()
                    {
                        No = i,
                        Name = name,
                        StrIncrement = strIncrement,
                        Min = min,
                        Max = max,

                        Worker = blWorker,
                        Engineer = blEngineer
                    };
                    g_ParSetAdjust_L.Add(baseParSetAdjust);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 读取调整值配置
        /// </summary>
        public void ReadIniAdj()
        {
            try
            {
                #region adj
                for (int j = 0; j < 24; j++)
                {
                    List<BaseParSetAdjust> parSetAdjust_L = new List<BaseParSetAdjust>();
                    string section = "adj" + (j + 1).ToString();
                    for (int i = 0; i < C_NumValue; i++)
                    {
                        string basekey = "Value" + (i + 1).ToString();

                        //名称
                        string name = IniFile.I_I.ReadIniStr(section, "Name" + basekey, C_PathSavePar);
                        if (name == "")
                        {
                            name = "Null";
                        }

                        //小数点个数
                        string strIncrement = IniFile.I_I.ReadIniStr(section, "Increment" + basekey, C_PathSavePar);
                        if (!strIncrement.Contains("Num"))
                        {
                            strIncrement = "Num2";
                        }

                        //最大最小值
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Min" + basekey, C_PathSavePar);
                        double min = Convert.ToDouble(base.StrDouble);
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Max" + basekey, C_PathSavePar);
                        double max = Convert.ToDouble(base.StrDouble);
                        if (min == 0
                            && max == 0)
                        {
                            min = int.MinValue;
                            max = int.MaxValue;
                        }

                        #region  权限
                        bool blWorker = true;
                        //string strWorker = IniFile.I_I.ReadIniStr(section, "Worker" + basekey, C_PathSavePar);
                        //if (strWorker == "")
                        //{

                        //}
                        //else
                        //{
                        //    try
                        //    {
                        //        blWorker = Boolean.Parse(strWorker);
                        //    }
                        //    catch (Exception)
                        //    {

                        //        blWorker = true;
                        //    }
                        //}

                        bool blEngineer = true;
                        //string strEngineer = IniFile.I_I.ReadIniStr(section, "Engineer" + basekey, C_PathSavePar);
                        //if (strEngineer == "")
                        //{

                        //}
                        //else
                        //{
                        //    try
                        //    {
                        //        blEngineer = Boolean.Parse(strEngineer);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        blEngineer = true;
                        //    }
                        //}
                        #endregion  权限

                        BaseParSetAdjust baseParSetAdjust = new BaseParSetAdjust()
                        {
                            No = i,
                            Name = name,
                            StrIncrement = strIncrement,
                            Min = min,
                            Max = max,

                            Worker = blWorker,
                            Engineer = blEngineer
                        };
                        parSetAdjust_L.Add(baseParSetAdjust);
                    }

                    ParSetAdjust par = new ParSetAdjust();

                    //标题
                    par.Title = IniFile.I_I.ReadIniStr(section, "Title", C_PathSavePar);
                    //控件隐藏
                    par.BlHidden = IniFile.I_I.ReadIniBl(section, "BlHidden", C_PathSavePar);

                    par.g_ParSetAdjust_L = parSetAdjust_L;

                    //将配置添加到哈希表
                    g_HtAdjConfig.Add(section, par);
                }
                #endregion adj
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 读取基准值的配置
        /// </summary>
        public void ReadIniStd()
        {
            try
            {
                #region std
                for (int j = 0; j < 18; j++)
                {
                    List<BaseParSetAdjust> parSetAdjust_L = new List<BaseParSetAdjust>();
                    string section = "std" + (j + 1).ToString();
                    for (int i = 0; i < C_NumValue; i++)
                    {
                        string basekey = "Value" + (i + 1).ToString();

                        //名称
                        string name = IniFile.I_I.ReadIniStr(section, "Name" + basekey, C_PathSavePar);
                        if (name == "")
                        {
                            name = "Null";
                        }

                        //小数点个数
                        string strIncrement = IniFile.I_I.ReadIniStr(section, "Increment" + basekey, C_PathSavePar);
                        if (!strIncrement.Contains("Num"))
                        {
                            strIncrement = "Num2";
                        }

                        //最大最小值
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Min" + basekey, C_PathSavePar);
                        double min = Convert.ToDouble(base.StrDouble);
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Max" + basekey, C_PathSavePar);
                        double max = Convert.ToDouble(base.StrDouble);
                        if (min == 0
                            && max == 0)
                        {
                            min = int.MinValue;
                            max = int.MaxValue;
                        }

                        #region  权限
                        bool blWorker = true;
                        //string strWorker = IniFile.I_I.ReadIniStr(section, "Worker" + basekey, C_PathSavePar);
                        //if (strWorker == "")
                        //{

                        //}
                        //else
                        //{
                        //    try
                        //    {
                        //        blWorker = Boolean.Parse(strWorker);
                        //    }
                        //    catch (Exception)
                        //    {

                        //        blWorker = true;
                        //    }
                        //}

                        bool blEngineer = true;
                        //string strEngineer = IniFile.I_I.ReadIniStr(section, "Engineer" + basekey, C_PathSavePar);
                        //if (strEngineer == "")
                        //{

                        //}
                        //else
                        //{
                        //    try
                        //    {
                        //        blEngineer = Boolean.Parse(strEngineer);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        blEngineer = true;
                        //    }
                        //}
                        #endregion  权限

                        BaseParSetAdjust baseParSetAdjust = new BaseParSetAdjust()
                        {
                            No = i,
                            Name = name,
                            StrIncrement = strIncrement,
                            Min = min,
                            Max = max,

                            Worker = blWorker,
                            Engineer = blEngineer
                        };
                        parSetAdjust_L.Add(baseParSetAdjust);
                    }

                    ParSetAdjust par = new ParSetAdjust();

                    //标题
                    par.Title = IniFile.I_I.ReadIniStr(section, "Title", C_PathSavePar);

                    par.g_ParSetAdjust_L = parSetAdjust_L;

                    //将配置添加到哈希表
                    g_HtAdjConfig.Add(section, par);
                }
                #endregion std
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 读取基准值的配置
        /// </summary>
        public void ReadIniRptStd()
        {
            try
            {
                #region std
                for (int j = 0; j < 18; j++)
                {
                    List<BaseParSetAdjust> parSetAdjust_L = new List<BaseParSetAdjust>();
                    string section = "rstd" + (j + 1).ToString();
                    for (int i = 0; i < C_NumValue; i++)
                    {
                        string basekey = "Value" + (i + 1).ToString();

                        //名称
                        string name = IniFile.I_I.ReadIniStr(section, "Name" + basekey, C_PathSavePar);
                        if (name == "")
                        {
                            name = "Null";
                        }

                        //小数点个数
                        string strIncrement = IniFile.I_I.ReadIniStr(section, "Increment" + basekey, C_PathSavePar);
                        if (!strIncrement.Contains("Num"))
                        {
                            strIncrement = "Num2";
                        }

                        //最大最小值
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Min" + basekey, C_PathSavePar);
                        double min = Convert.ToDouble(base.StrDouble);
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Max" + basekey, C_PathSavePar);
                        double max = Convert.ToDouble(base.StrDouble);
                        if (min == 0
                            && max == 0)
                        {
                            min = int.MinValue;
                            max = int.MaxValue;
                        }

                        #region  权限
                        bool blWorker = true;
                        bool blEngineer = true;

                        #endregion  权限

                        BaseParSetAdjust baseParSetAdjust = new BaseParSetAdjust()
                        {
                            No = i,
                            Name = name,
                            StrIncrement = strIncrement,
                            Min = min,
                            Max = max,

                            Worker = blWorker,
                            Engineer = blEngineer
                        };
                        parSetAdjust_L.Add(baseParSetAdjust);
                    }

                    ParSetAdjust par = new ParSetAdjust();

                    //标题
                    par.Title = IniFile.I_I.ReadIniStr(section, "Title", C_PathSavePar);

                    par.g_ParSetAdjust_L = parSetAdjust_L;

                    //将配置添加到哈希表
                    g_HtAdjConfig.Add(section, par);
                }
                #endregion std
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 读取基准值的配置
        /// </summary>
        public void ReadIniRptAdj()
        {
            try
            {
                #region std
                for (int j = 0; j < 18; j++)
                {
                    List<BaseParSetAdjust> parSetAdjust_L = new List<BaseParSetAdjust>();
                    string section = "radj" + (j + 1).ToString();
                    for (int i = 0; i < C_NumValue; i++)
                    {
                        string basekey = "Value" + (i + 1).ToString();

                        //名称
                        string name = IniFile.I_I.ReadIniStr(section, "Name" + basekey, C_PathSavePar);
                        if (name == "")
                        {
                            name = "Null";
                        }

                        //小数点个数
                        string strIncrement = IniFile.I_I.ReadIniStr(section, "Increment" + basekey, C_PathSavePar);
                        if (!strIncrement.Contains("Num"))
                        {
                            strIncrement = "Num2";
                        }

                        //最大最小值
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Min" + basekey, C_PathSavePar);
                        double min = Convert.ToDouble(base.StrDouble);
                        base.StrDouble = IniFile.I_I.ReadIniStr(section, "Max" + basekey, C_PathSavePar);
                        double max = Convert.ToDouble(base.StrDouble);
                        if (min == 0
                            && max == 0)
                        {
                            min = int.MinValue;
                            max = int.MaxValue;
                        }

                        #region  权限
                        bool blWorker = true;
                        bool blEngineer = true;

                        #endregion  权限

                        BaseParSetAdjust baseParSetAdjust = new BaseParSetAdjust()
                        {
                            No = i,
                            Name = name,
                            StrIncrement = strIncrement,
                            Min = min,
                            Max = max,

                            Worker = blWorker,
                            Engineer = blEngineer
                        };
                        parSetAdjust_L.Add(baseParSetAdjust);
                    }

                    ParSetAdjust par = new ParSetAdjust();

                    //标题
                    par.Title = IniFile.I_I.ReadIniStr(section, "Title", C_PathSavePar);

                    par.g_ParSetAdjust_L = parSetAdjust_L;

                    //将配置添加到哈希表
                    g_HtAdjConfig.Add(section, par);
                }
                #endregion std
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 读取参数

        #region 写入参数
        /// <summary>
        /// 将参数写入Ini
        /// </summary>
        public void WriteIni(string section)
        {
            try
            {
                for (int i = 0; i < C_NumValue; i++)
                {
                    string basekey = "Value" + (i + 1).ToString();
                    IniFile.I_I.WriteIni(section, "Name" + basekey, ParSetAdjust.P_I[section][i].Name, C_PathSavePar);
                    IniFile.I_I.WriteIni(section, "Increment" + basekey, ParSetAdjust.P_I[section][i].StrIncrement, C_PathSavePar);
                    IniFile.I_I.WriteIni(section, "Min" + basekey, ParSetAdjust.P_I[section][i].Min.ToString(), C_PathSavePar);
                    IniFile.I_I.WriteIni(section, "Max" + basekey, ParSetAdjust.P_I[section][i].Max.ToString(), C_PathSavePar);

                    IniFile.I_I.WriteIni(section, "Worker" + basekey, ParSetAdjust.P_I[section][i].Worker.ToString(), C_PathSavePar);
                    IniFile.I_I.WriteIni(section, "Engineer" + basekey, ParSetAdjust.P_I[section][i].Engineer.ToString(), C_PathSavePar);
                }

                IniFile.I_I.WriteIni(section, "Title", ParSetAdjust.P_I[section, -1].Title, C_PathSavePar);//控件标题名称

                IniFile.I_I.WriteIni(section, "BlHidden", ParSetAdjust.P_I[section, -1].BlHidden, C_PathSavePar);//隐藏控件

                //保存窗体大小设置
                WriteSelectWinIni();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void Reset(string section)
        {
            try
            {
                for (int i = 0; i < C_NumValue; ++i)
                {
                    this[section][i].Name = "Null";
                    this[section][i].StrIncrement = TypeIncrement_e.Num2.ToString();
                    this[section][i].Max = int.MaxValue;
                    this[section][i].Min = int.MinValue;
                    this[section][i].Worker = true;
                    this[section][i].Engineer = true;                    
                }
                WriteIni(section);
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 写入窗体选择
        /// </summary>
        public void WriteSelectWinIni()
        {
            try
            {                
                if(g_Section=="adj")
                {
                    IniFile.I_I.WriteIni("Adjust", "TypeWinAdjust", ((int)ParSetAdjust.P_I.TypeWinAdjust_e).ToString(), C_PathSavePar);//隐藏控件
                }
                else
                {
                    IniFile.I_I.WriteIni("Std", "TypeWinAdjust", ((int)ParSetAdjust.P_I.TypeWinStd_e).ToString(), C_PathSavePar);//隐藏控件
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 写入参数
    }
        

    /// <summary>
    /// 设置调整值基类
    /// </summary>

    public class BaseParSetAdjust
    {
        //序号
        public int No { set; get; }
        //数据类型
        public string Type
        {
            get
            {
                return "Value" + (No + 1).ToString();
            }
        }
        public string Name { set; get; }

        //小数点
        public int Increment
        {
            get
            {
                try
                {
                    return int.Parse(StrIncrement.Replace("Num",""));
                }
                catch
                {
                    return 0;
                }
            }
        }
        public string StrIncrement { set; get; }
        //最小最大值
        public double Min { set; get; }
        public double Max { set; get; }

        //权限
        public bool Worker { set; get; }
        public bool Engineer { set; get; }

        public bool BlChange//是否设置过
        {
            get
            {
                if (Name == "Value" + (No + 1).ToString()
                    && StrIncrement == "Num2"
                    && Min == int.MinValue
                    && Max == int.MaxValue
                    && Worker == true
                    && Engineer == true)
                {
                    return false;
                }
                return true;
            }
        }
    }

    /// <summary>
    /// 窗体大小选择
    /// </summary>
    public enum TypeWinAdjust_enum
    {
        small = 1,
        normal = 2,
        full = 3,
    }

}
