using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using BasicClass;
using System.IO;

namespace DealIO
{
    public class ParDIO:BaseClass
    {
        #region 静态类实例
        public static ParDIO P_I = new ParDIO();
        #endregion 静态类实例

        #region 定义
        public static string c_PathCyc
        {
            get
            {
                string path = ParPathRoot.PathRoot + "Store\\DIO\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path + "DIO.ini";
            }
        }

        public string Port = "Com1";
        public int IntPort
        {
            get
            {
                if (Port.Contains("Com"))
                {
                    base.StrInt = Port.Replace("Com","");
                    return int.Parse(base.StrInt);
                }
                return 0;
            }
        }
        #endregion 定义

        #region 读Ini
        public void ReadIniPar()
        {
            //循环读取寄存器
            Port = IniFile.I_I.ReadIniStr("DIO", "Port", "Null", c_PathCyc);
        }
        #endregion 读Ini

        #region 写Ini
        public void WriteIniPar()
        {
            //循环读取寄存器
            IniFile.I_I.WriteIni("DIO", "Port", Port, c_PathCyc);
        }
        #endregion 写Ini
    }
}
