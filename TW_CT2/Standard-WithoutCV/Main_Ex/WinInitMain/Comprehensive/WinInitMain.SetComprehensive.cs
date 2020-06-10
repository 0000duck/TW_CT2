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
    partial class WinInitMain
    {
        /// <summary>
        /// 图像处理初始化
        /// </summary>
        public void Init_ImageProcess()
        {
            bool blScale = false;
            bool blNcc = false;
            bool blScaleT = false;
            bool blNccT = false;

            //统计窗体个数 
            CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);

            //初始化模板窗体
            new Task(new Action(() =>
            {
                int i = 0;
                while (i < 20
                    && !BlInitComprehensiveWin)
                {
                    i++;
                    Thread.Sleep(500);
                }

                InitTempWin(blScale, blNcc, blScaleT, blNccT);
                //表示初始化窗体完成
                BlInitComprehensiveTempWin = true;
                FinishInitWin();//结束初始化
            })).Start();

            InitCalib();//初始化校准 

            //显示模板加载结果20190112zx
            new Task(new Action(() =>
            {
                ShowResultTemp();
            })).Start();
        }
    }
}
