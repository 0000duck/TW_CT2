using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DealFile;
using System.IO;
using BasicClass;

namespace DealConfigFile
{
    /// <summary>
    /// 调整值
    /// </summary>
    public class ParBase
    {
        #region 定义

        protected virtual string ClassName
        {
            get;
            set;
        }

        protected virtual string columnName
        {
            get;
            set;
        }

        protected virtual string indexName
        { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public virtual string FilePath
        {
            get
            {
                return ParPathRoot.PathRoot + "Store\\AdjustStd\\" + ClassName + ".ini";
            }
        }
        #endregion 定义

        #region 索引器
        public Point4D this[int index]
        {
            get
            {
                string strIndex = indexName + index.ToString();
                return new Point4D(Value1(strIndex), Value2(strIndex), Value3(strIndex), Value4(strIndex));
            }
        }
        #endregion

        #region 读取参数
        public double Value1(string strSection)
        {
            try
            {
                if (strSection.Contains(ClassName))
                {
                    strSection = strSection.Replace(ClassName, columnName);
                }
                return AdjustIniBase.A_I.Value1(strSection, FilePath);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double Value2(string strSection)
        {
            try
            {
                if (strSection.Contains(ClassName))
                {
                    strSection = strSection.Replace(ClassName, columnName);
                }
                return AdjustIniBase.A_I.Value2(strSection, FilePath);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double Value3(string strSection)
        {
            try
            {
                if (strSection.Contains(ClassName))
                {
                    strSection = strSection.Replace(ClassName, columnName);
                }
                return AdjustIniBase.A_I.Value3(strSection, FilePath);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double Value4(string strSection)
        {
            try
            {
                if (strSection.Contains(ClassName))
                {
                    strSection = strSection.Replace(ClassName, columnName);
                }
                return AdjustIniBase.A_I.Value4(strSection, FilePath);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion 读取参数

        #region 写入参数
        public void SetValue1(string section, double value)
        {
            SetValue(section, value, "Value1");
        }

        public void SetValue2(string section, double value)
        {
            SetValue(section, value, "Value2");
        }

        public void SetValue3(string section, double value)
        {
            SetValue(section, value, "Value3");
        }

        public void SetValue4(string section, double value)
        {
            SetValue(section, value, "Value4");
        }

        void SetValue(string section,double value,string key)
        {
            IniFile.I_I.WriteIni(section, key, value.ToString(), FilePath);
        }
        #endregion 写入参数
    }
}
