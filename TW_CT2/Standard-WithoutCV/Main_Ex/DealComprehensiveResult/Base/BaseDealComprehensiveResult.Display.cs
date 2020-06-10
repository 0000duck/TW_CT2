using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealImageProcess;
using DealIO;
using System.Diagnostics;
using BasicDisplay;
using DealLog;

namespace Main_EX
{
    partial class BaseDealComprehensiveResult
    {
        #region 定义
        protected bool BlDisplayChange = false;

        Action FunDisplay = null;

        public ResultMainShow g_ResultMainShow = new DealResult.ResultMainShow();//存储Main_EX里面设定的图像和对象
        #endregion 定义

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="pos_e"></param>
        /// <param name="htResult"></param>
        /// <param name="blResult"></param>
        /// <param name="sw"></param>
        protected void Display(Pos_enum pos_e, Hashtable htResult, bool blResult, Stopwatch sw)
        {
            try
            {
                g_BaseDealComprehensive.DisplayComprehensivePos(g_UCDisplayCamera, g_HtUCDisplay, pos_e, htResult, blResult);
                //记录当前整体节拍
                RecordTact(sw, g_NoCamera, (int)pos_e, htResult);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// 使用新建线程显示
        /// </summary>
        protected void InitDisplay_Task()
        {
            try
            {
                new Task(new Action(() =>
                    {
                        while (ParSetDisplay.P_I[g_NoCamera - 1].BlSingleTaskForDisplay)
                        {
                            Thread.Sleep(10);
                            if (BlDisplayChange)
                            {
                                BlDisplayChange = false;

                                FunDisplay();
                            }
                        }

                    })).Start();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="htResult"></param>
        /// <param name="blResult"></param>
        /// <param name="sw"></param>
        /// <param name="funDisplay">显示的委托</param>
        protected void Display_Task(Pos_enum pos_e, Hashtable htResult, bool blResult, Stopwatch sw, Action funDisplay)
        {
            try
            {
                FunDisplay = new Action(() =>
                {
                    g_BaseDealComprehensive.DisplayComprehensivePos(g_UCDisplayCamera, g_HtUCDisplay, pos_e, htResult, blResult);
                    // funDisplay();
                    //记录当前整体节拍
                    RecordTact(sw, g_NoCamera, (int)pos_e, htResult);
                    if (funDisplay != null)
                    {
                        funDisplay();
                    }
                });

                BlDisplayChange = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #region 添加到ResultMainShow
        /// <summary>
        /// 将图像添加到哈希表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="im"></param>
        public void AddResultMainShow(string name, ImageAll im)
        {
            try
            {
                if (g_ResultMainShow.HtResultImage.Contains(name))
                {
                    ImageAll imOld = g_ResultMainShow.HtResultImage[name] as ImageAll;
                    imOld.Dispose();
                    g_ResultMainShow.HtResultImage.Remove(name);
                }

                g_ResultMainShow.HtResultImage.Add(name, im);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 添加到ResultMainShow

     
    }
}
