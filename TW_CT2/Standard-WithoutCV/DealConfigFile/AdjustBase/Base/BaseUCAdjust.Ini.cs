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
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Common;
using BasicClass;
using DealLog;

namespace DealConfigFile
{
    public partial class BaseUCAdjust
    {
        #region 写入Ini文件
        public bool WriteIni4(double dblValue1, double dblValue2, double dblValue3, double dblValue4, string Annotation)
        {
            try
            {
                //保存到本地
                Backup();

                IniFile.I_I.WriteIni(this.Name, "Value1", dblValue1.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Value2", dblValue2.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Value3", dblValue3.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Value4", dblValue4.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Annotation", Annotation.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Time", DateTime.Now.ToString(), PathValueIni);
                return true;
            }
            catch (Exception ex)
            {
                //Log.L_I.WriteError("AdjustIniBase", ex);
                return false;
            }
        }
        public bool WriteIni3(double dblValue1, double dblValue2, double dblValue3, string Annotation)
        {
            try
            {
                //保存到本地
                Backup();

                IniFile.I_I.WriteIni(this.Name, "Value1", dblValue1.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Value2", dblValue2.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Value3", dblValue3.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Annotation", Annotation.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Time", DateTime.Now.ToString(), PathValueIni);
                return true;
            }
            catch (Exception ex)
            {
                //Log.L_I.WriteError("AdjustIniBase", ex);
                return false;
            }
        }
        public bool WriteIni2(double dblValue1, double dblValue2, string Annotation)
        {
            try
            {
                //保存到本地
                Backup();

                IniFile.I_I.WriteIni(this.Name, "Value1", dblValue1.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Value2", dblValue2.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Annotation", Annotation.ToString(), PathValueIni);
                IniFile.I_I.WriteIni(this.Name, "Time", DateTime.Now.ToString(), PathValueIni);
                return true;
            }
            catch (Exception ex)
            {
                //Log.L_I.WriteError("AdjustIniBase", ex);
                return false;
            }
        }
        #endregion 写入Ini文件

        #region 保存文件到本地
        /// <summary>
        /// 备份参数
        /// </summary>
        void Backup()
        {
            try
            {
                if (PathValueIni.Contains("AdjustStd"))
                {
                    FunBackup.F_I.BackupStd();
                }
                else
                {
                    FunBackup.F_I.BackupAdjust();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("AdjustIniBase", ex);
            }
        }
        #endregion 保存文件到本地
    }
}
