using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Common;
using DealFile;
using BasicClass;
using System.IO;

namespace DealConfigFile
{
    public class ParManageConfigPar
    {
        #region 静态类实例
        public static ParManageConfigPar P_I = new ParManageConfigPar();
        #endregion 静态类实例

        #region 定义
        #region Path
        //保存了当前配置文件路径
        public string PathConfigPar
        {
            get
            {
                string root = ParPathRoot.PathRoot+"Store\\ConfigFile\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                return root + "PathConfigPar.ini";
            }
        }
        #endregion Path

        string NameClass = "ParManageConfigPar";

        //List
        public List<FileConfigPar> FileConfigPar_L = new List<FileConfigPar>();//文件路径
        #endregion 定义

        #region 初始化
        public ParManageConfigPar()
        {
            
        }
        #endregion 初始化

        /// <summary>
        /// 读取当前配置参数路径,以及是否使用文件编号进行判断
        /// </summary>
        public bool ReadIniPathConfigPar()
        {
            try
            {
                if (!File.Exists(PathConfigPar))
                {
                    string root = ParPathRoot.PathRoot + "Store\\产品参数\\default\\";
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                    string path = root + "Product.ini";
                    IniFile.I_I.WriteIni("ConfigParIni", "Path", "default", PathConfigPar);
                    ComConfigPar.C_I.PathConfigIni = path;
                }
                else
                {
                    string model = IniFile.I_I.ReadIniStr("ConfigParIni", "Path", PathConfigPar);
                    //是否使用序号
                    ComConfigPar.C_I.BlUsingNo = IniFile.I_I.ReadIniBl("ConfigParIni", "BlUsingNo", PathConfigPar);

                    if (model.Trim() != "")
                    {
                        ComConfigPar.C_I.PathConfigIni = ParPathRoot.PathRoot + "Store\\产品参数\\" + model + "\\Product.ini";
                    }
                    else
                    {
                        ComConfigPar.C_I.PathConfigIni = ParPathRoot.PathRoot + "Store\\产品参数\\default\\Product.ini";
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        #region 读取文件列表
        /// <summary>
        /// 读取配置文件列表
        /// </summary>
        public bool ReadFileConfigList()
        {
            int i = 0;
            try
            {
                FileConfigPar_L.Clear();

                DirectoryInfo pathRoot = new DirectoryInfo(ParPathRoot.PathRoot+"Store\\产品参数\\");
               
                foreach (DirectoryInfo files in pathRoot.GetDirectories())
                {
                    i++;
                    FileConfigPar fileConfigPar = new FileConfigPar();
                    fileConfigPar.No = i;
                    fileConfigPar.Model = files.Name;
                    fileConfigPar.PathPar = files.FullName + "\\Product.ini";
                    FileConfigPar_L.Add(fileConfigPar);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 读取文件列表

        #region 保存产品参数路径
        /// <summary>
        /// 将当前配置文件路径进行保存
        /// </summary>
        public bool WriteIniPathConfigPar()
        {
            try
            {
                //产品名称
                IniFile.I_I.WriteIni("ConfigParIni", "Path", ComConfigPar.C_I.NameModel, PathConfigPar);
                //产品编号
                IniFile.I_I.WriteIni("ConfigParIni", "BlUsingNo", ComConfigPar.C_I.BlUsingNo, PathConfigPar);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 保存产品参数路径
    }
}
