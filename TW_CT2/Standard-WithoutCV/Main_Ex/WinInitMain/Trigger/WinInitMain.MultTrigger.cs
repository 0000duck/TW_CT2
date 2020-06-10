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
using DealFile;
using DealConfigFile;
using BasicClass;
using DealComInterface;
using ParComprehensive;
using DealAlgorithm;
using DealCalibrate;

namespace Main_EX
{
    /// <summary>
    /// 多相机触发
    /// </summary>
    partial class WinInitMain
    {
        /// <summary>
        /// 多相机执行
        /// </summary>
        /// <param name="par"></param>
        /// <param name="baseParCalibrate"></param>
        /// <param name="fun"></param>
        public void MainWindow_GetResultValueMult_event(ParGetResultFromCell par, BaseParCalibrate baseParCalibrate, HashAction fun)
        {
            try
            {
                BaseDealComprehensiveResult baseDealComprehensiveResult = null;
                int noCamera = par.NoCameraMult;//多相机序号
                switch (noCamera)
                {
                    case 1:
                        baseDealComprehensiveResult = DealComprehensiveResult1;
                        break;

                    case 2:
                        baseDealComprehensiveResult = DealComprehensiveResult2;
                        break;

                    case 3:
                        baseDealComprehensiveResult = DealComprehensiveResult3;
                        break;

                    case 4:
                        baseDealComprehensiveResult = DealComprehensiveResult4;
                        break;

                    case 5:
                        baseDealComprehensiveResult = DealComprehensiveResult5;
                        break;

                    case 6:
                        baseDealComprehensiveResult = DealComprehensiveResult6;
                        break;

                    case 7:
                        baseDealComprehensiveResult = DealComprehensiveResult7;
                        break;

                    case 8:
                        baseDealComprehensiveResult = DealComprehensiveResult8;
                        break;
                }
                baseDealComprehensiveResult.GetCellResultValue(par, baseParCalibrate, fun);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

    }
}
