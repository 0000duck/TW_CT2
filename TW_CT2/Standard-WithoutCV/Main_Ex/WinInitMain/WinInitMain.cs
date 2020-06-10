
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
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using HalconDotNet;
using DealFile;
using DealPLC;
using SetPar;
using BasicClass;
using DealLog;
using DealConfigFile;
using System.IO;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 初始化
        public WinInitMain()
        {
            NameClass = "Init_Main";
        }

        /// <summary>
        /// 初始化文件夹
        /// </summary>
        public void Init_DictionaryFiles()
        {
            #region
            //存储文件夹
            if (!Directory.Exists(ComValue.c_PathStore))
            {
                Directory.CreateDirectory(ComValue.c_PathStore);
            }
            //参数文件夹
            if (!Directory.Exists(ComValue.c_PathPar))
            {
                Directory.CreateDirectory(ComValue.c_PathPar);
            }
            //软件运行记录
            if (!Directory.Exists(ComValue.c_PathRecord))
            {
                Directory.CreateDirectory(ComValue.c_PathRecord);
            }
            //相机文件夹
            if (!Directory.Exists(ComValue.c_PathCamera))
            {
                Directory.CreateDirectory(ComValue.c_PathCamera);
            }
            //Calib
            if (!Directory.Exists(ComValue.c_PathCalib))
            {
                Directory.CreateDirectory(ComValue.c_PathCalib);
            }
            //PLC
            if (!Directory.Exists(ComValue.c_PathPLC))
            {
                Directory.CreateDirectory(ComValue.c_PathPLC);
            }
            //Robot
            if (!Directory.Exists(ComValue.c_PathRobot))
            {
                Directory.CreateDirectory(ComValue.c_PathRobot);
            }

            //SetPar
            if (!Directory.Exists(ComValue.c_PathSetPar))
            {
                Directory.CreateDirectory(ComValue.c_PathSetPar);
            }
            //调整值
            if (!Directory.Exists(ComValue.c_PathAdjustStd))
            {
                Directory.CreateDirectory(ComValue.c_PathAdjustStd);
            }
            //系统路径初始化
            if (!Directory.Exists(ComValue.c_PathSysInit))
            {
                Directory.CreateDirectory(ComValue.c_PathSysInit);
            }

            //图片记录
            if (!Directory.Exists(ComValue.c_PathImageLog))
            {
                Directory.CreateDirectory(ComValue.c_PathImageLog);
            }
            //Custom
            if (!Directory.Exists(ComValue.c_PathCustom))
            {
                Directory.CreateDirectory(ComValue.c_PathCustom);
            }
            //Custom
            if (!Directory.Exists(ComValue.c_PathCustomLog))
            {
                Directory.CreateDirectory(ComValue.c_PathCustomLog);
            }
            #endregion
        }


        public virtual void Init_Others()
        {

        }

        public virtual void Init_Custom()
        {

        }


        public virtual void Init_IO()
        {

        }

        public virtual void Init_CIM()
        {

        }
        #endregion 初始化
    }
}
