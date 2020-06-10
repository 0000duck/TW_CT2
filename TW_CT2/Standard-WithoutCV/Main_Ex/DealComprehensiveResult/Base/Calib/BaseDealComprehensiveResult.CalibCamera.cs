using System;
using System.Threading;
using DealRobot;
using BasicClass;
using DealResult;
using DealConfigFile;
using DealGeometry;
using DealImageProcess;
using DealFile;
using Common;
using DealPLC;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DealCalibrate;
using ParComprehensive;
using BasicComprehensive;
using DealAlgorithm;
using DealComprehensive;

namespace Main_EX
{
    partial class BaseDealComprehensiveResult
    {
        /// <summary>
        /// 通过拍照位置来区分标定相机，采用标定板，1201表示位置1第一次拍照，
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="index"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public virtual StateComprehensive_enum DealComprehensiveResultFun_CalibCamera(TriggerSource_enum trigerSource_e, int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            bool blResult = false;
            try
            {
                blResult = CalibCamera(pos, index, out htResult);
                if (blResult)
                {
                    return StateComprehensive_enum.True;
                }
                return StateComprehensive_enum.False;
            }
            catch (Exception ex)
            {
                return StateComprehensive_enum.False;
            }
            finally
            {
                g_BaseDealComprehensive.DisplayComprehensivePos(g_UCDisplayCamera, g_HtUCDisplay, (Pos_enum)pos, htResult, blResult);
            }
        }

        /// <summary>
        /// 自动相机校准,1201表示位置1第一次拍照，
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        bool CalibCamera(int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            try
            {
                ParCalibCamera par = g_BaseParComprehensive.GetCellParCalibByType("相机校准", pos) as ParCalibCamera;
                string nameCell = par.NameCell;//单元格名称
                int indexCell = int.Parse(nameCell.Replace("C", ""));//单元格索引

                //关联算法，用来获取角点
                ParCellExeReference parCellExeReference = par.g_ParCellExecuteReference;
                ParGetResultFromCell parGetResultFromCell = par.g_ParGetResultFromCell;

                //调用执行
                g_BaseDealComprehensive.DealComprehensivePosCell_ForTestRun(g_UCDisplayCamera, parCellExeReference, par.g_ParCellExecuteReference.g_CellExecute_L, out htResult);//执行算法但不显示

                string cellError = "";
                if (!GetErrorCell(htResult, pos,out cellError))
                {
                    WinMsgBox.ShowMsgBox(string.Format("相机{0}位置{1}，单元格{2}校准计算错误", g_NoCamera.ToString(), pos.ToString(), par.NameCell));
                    return false;
                }
                FunCellDataReferenc fun = new FunCellDataReferenc();
                fun.GetResultValue(htResult, parGetResultFromCell, index - 1);

                string info = string.Format("相机{0}位置{1}，单元格{2}相机校准触发拍照", g_NoCamera.ToString(), pos.ToString(), par.NameCell);
                ShowState(info);

                FunCalibCamera funCalibCamera = new FunCalibCamera();
                ResultCalibCamera result = funCalibCamera.DealCalibCamera(par);

                info = "将标定参数保存到结果";
                ShowState(info);

                //参数保存到Par
                bool blSavePar = funCalibCamera.SaveResultToPar(par, result);
                //将参数保存到结果中
                bool blSaveResult = funCalibCamera.SaveResultToStdResult(par, htResult);


                if (blSaveResult)
                {
                    ShowState(string.Format("将相机{0}参数{1}保存到结果成功", g_NoCamera.ToString(), par.NameCell));
                }
                else
                {
                    ShowAlarm(string.Format("将相机{0}参数{1}保存到结果失败", g_NoCamera.ToString(), par.NameCell));
                    return false;
                }

                //保存基准图片
                SaveStdImage(par, "相机校准");
                //保存此单元格
                bool blSave = g_BaseParComprehensive.WriteXmlDoc(par.NameCell);

                if (blSave)
                {
                    ShowState(string.Format("将相机{0}参数{1}保存到本地成功", g_NoCamera.ToString(), par.NameCell));
                }
                else
                {
                    ShowAlarm(string.Format("将相机{0}参数{1}保存到本地失败", g_NoCamera.ToString(), par.NameCell));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                WinMsgBox.ShowMsgBox(string.Format("相机{0}校准计算进入Catch", g_NoCamera.ToString()));
                ShowAlarm("相机校准计算进入Catch");
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }          
        }
    }
}
