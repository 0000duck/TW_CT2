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
using System.Drawing;
using DealRobot;
using DealPLC;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using DealComprehensive;
using SetPar;
using SetComprehensive;
using DealConfigFile;
using BasicClass;
using DealComInterface;
using ParComprehensive;
using DealDisplay;
using BasicDisplay;
using DealTool;
using DealLog;
using DealMedia;
using DealHelp;
using DealIO;
using System.Reflection;
using BasicComprehensive;
using DealAlgorithm;
using DealCalibrate;
using DealCommunication;
using DealDraw;
using DealGeometry;
using DealGrabImage;
using DealImageProcess;
using DealInOutput;
using DealMath;
using DealResult;
using DealWorkFlow;
using DealFile;
using System.Windows.Controls.Primitives;

namespace Main_EX
{
    public partial class WinInitMain
    {
        #region 记录版本号
        public void RecordVersion()
        {
            try
            {
                string path = new DirectoryInfo("../").FullName;
                string pathRoot = new DirectoryInfo("../").Parent.Parent.FullName + "\\Dll\\";

                if (!Directory.Exists(pathRoot))
                {
                    Directory.CreateDirectory(pathRoot);
                }
                //if (Directory.Exists(pathRoot))
                //{
                //    FileInfo[] fileInfos = (new DirectoryInfo(pathRoot)).GetFiles();
                //    foreach (FileInfo item in fileInfos)
                //    {
                //        if (item.FullName.Contains(".dll"))
                //        {
                //            File.Delete(item.FullName);
                //        }
                //    }
                //}

                if (!path.Contains("bin"))
                {
                    return;
                }
                string pathSave = path + "机器视觉控制处理软件\\Version.ini";
                string pathSaveXml = path + "机器视觉控制处理软件\\";

                IniFile inst = new IniFile();

                BasicClass.VerInfo v = BaseClass.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "1", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "1Path", v.PathDll + "\n", pathSave);
               
                v = BaseComprehensive.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "2", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "2Path", v.PathDll + "\n", pathSave);
               

                v = BaseUCDisplay.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "3", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "3Path", v.PathDll + "\n", pathSave);
                
                v = CameraBase.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "4", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "4Path", v.PathDll + "\n", pathSave);                

                v = ComFunction.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "5", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "5Path", v.PathDll + "\n", pathSave);
               

                v = BaseParAlgorithm.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "6", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "6Path", v.PathDll + "\n", pathSave);
               

                v = BaseFunCalibrate.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "7", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "7Path", v.PathDll + "\n", pathSave);
                

                v = BaseParComInterface.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "8", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "8Path", v.PathDll + "\n", pathSave);
               
                v = BaseParCommunication.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "9", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "9Path", v.PathDll + "\n", pathSave);
               
                v = BaseParConfigFile.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "10", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "10Path", v.PathDll + "\n", pathSave);
               

                v = BaseParDisplay.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "11", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "11Path", v.PathDll + "\n", pathSave);
                
                v = DrawDisplay.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "12", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "12Path", v.PathDll + "\n", pathSave);
                

                v = BaseParGeometry.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "13", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "13Path", v.PathDll + "\n", pathSave);
              

                v = BaseParGrabImage.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "14", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "14Path", v.PathDll + "\n", pathSave);
                
                v = BaseParHelp.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "15", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "15Path", v.PathDll + "\n", pathSave);
               

                v = BaseParImageProcess.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "16", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "16Path", v.PathDll, pathSave);
                

                v = BaseParInOutput.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "17", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "17Path", v.PathDll + "\n", pathSave);
                
                v = BaseParLog.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "18", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "18Path", v.PathDll + "\n", pathSave);
                
                v = BaseParMath.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "19", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "19Path", v.PathDll + "\n", pathSave);
                
                v = BaseParResult.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "20", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "20Path", v.PathDll + "\n", pathSave);
                
                v = BaseParTool.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "21", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "21Path", v.PathDll + "\n", pathSave);
                
                v = BaseParWorkFlow.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "22", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "22Path", v.PathDll + "\n", pathSave);
               
                v = BaseParCell.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "23", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "23Path", v.PathDll + "\n", pathSave);
                

                v = BaseUCSetComprehensive.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "24", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "24Path", v.PathDll + "\n", pathSave);
               

                v = BaseParSetPar.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "25", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "25Path", v.PathDll + "\n", pathSave);
               

                v = BaseDealComprehensive.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "26", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "26Path", v.PathDll + "\n", pathSave);
                

                Assembly assembly = Assembly.GetExecutingAssembly();
                inst.WriteIni("VerInfo", "Main27Path", assembly.Location, pathSave);

                v = BaseDealComprehensive.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "28", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "28Path", v.PathDll + "\n", pathSave);
                
                v = BaseFile.GetVersion();
                inst.WriteIni("VerInfo", v.Name + "29", v.Version, pathSave);
                inst.WriteIni("VerInfo", v.Name + "29Path", v.PathDll + "\n", pathSave);                

                ControlLib.VerInfo v1 = BaseControlLib.GetVersion();
                inst.WriteIni("VerInfo", v1.Name + "30", v1.Version, pathSave);
                inst.WriteIni("VerInfo", v1.Name + "30Path", v1.PathDll + "\n", pathSave);
               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 记录版本号
    }
}
