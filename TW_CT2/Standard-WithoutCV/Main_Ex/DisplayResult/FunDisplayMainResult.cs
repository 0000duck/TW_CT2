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
using Common;
using DealFile;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Main_EX
{
    public class FunDisplayMainResult : BaseClass
    {
        #region 静态类实例
        public static FunDisplayMainResult F_I = new FunDisplayMainResult();
        #endregion 静态类实例

        #region 定义
        #region Path
        string PathState
        {
            get
            {
                string path = ComValue.c_PathRecord + "运行及报警日志\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
        public string PathStateHour
        {
            get
            {
                return Log.CreateAllTimeFile(PathState);
            }
        }
        #endregion Path

        //名称
        string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        string info = "";
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
            }
        }

        public int Count
        {
            get
            {
                return FunDisplayMainResult_L.Count;
            }
        }

        //结果数值
        bool blResult = true;
        public bool BlResult
        {
            get
            {
                return blResult;
            }
            set
            {
                blResult = value;
            }
        }

        //时间
        string time = "";
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        public string GetTime
        {
            get
            {
                return DateTime.Now.Hour.ToString().PadLeft(2, ' ') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, ' ') + ":" + DateTime.Now.Second.ToString().PadLeft(2, ' ') + ":" + DateTime.Now.Millisecond.ToString().PadLeft(3, ' ');//时间
            }
        }

        public SolidColorBrush BrCell
        {
            get
            {
                SolidColorBrush colorInfo = Brushes.Green;
                if (!BlResult)
                {
                    colorInfo = Brushes.Red;
                }               
                return colorInfo;
            }
        }

        public List<FunDisplayMainResult> FunDisplayMainResult_L = new List<FunDisplayMainResult>();


        //索引器
        public FunDisplayMainResult this[int index]
        {
            get
            {
                return FunDisplayMainResult_L[index];
            }
        }
        #endregion 定义

        #region 添加日志
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        public void Init(string name)
        {
            try
            {
                FunDisplayMainResult_L.Add(new FunDisplayMainResult()
                    {
                        Name = name,
                        Time = GetTime,
                    });
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 添加显示信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="index"></param>
        /// <param name="blResult"></param>
        public void AddInfo(string info, int index, bool blResult)
        {
            try
            {
                if (Count > index)
                {
                    
                }
                else
                {
                    index = Count - 1;
                    Log.L_I.WriteError(NameClass, "索引超过行数");
                }
                FunDisplayMainResult_L[index].Info = info;
                FunDisplayMainResult_L[index].BlResult = blResult;
                FunDisplayMainResult_L[index].Time = GetTime;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 添加日志

        #region 记录日志到本地
        /// <summary>
        /// 写入文本
        /// </summary>
        public void WriteTxt()
        {
            try
            {
                TxtFile t_I = new TxtFile();
                //文件
                string strFileLog = PathStateHour + "Result.txt";

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 记录日志
    }
}
