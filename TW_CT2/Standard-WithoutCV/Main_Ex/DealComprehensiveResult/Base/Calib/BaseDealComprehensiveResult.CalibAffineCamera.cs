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
    /// <summary>
    /// 通过算法获取当前相机的基准坐标，并获取其他相机的坐标，对两个相机进行映射，得到投影变换的系数
    /// </summary>
    partial class BaseDealComprehensiveResult
    {
        /// <summary>
        /// 投影校准 1501   -20181207--zx
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="index"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public virtual StateComprehensive_enum DealComprehensiveResultFun_CalibAffineCamera(TriggerSource_enum trigerSource_e, int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            bool blResult = false;
            try
            {
                blResult = CalibAffineCamera(pos, index, out htResult);
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
        }




        /// <summary>
        /// 自动相机校准,并且调用多相机执行算子
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="index">拍照序号，99表示结束表达，或者点位引用是_L</param>
        ///  <param name="htResult">哈希表存储结果</param>
        /// <returns></returns>
        bool CalibAffineCamera(int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            try
            {
                ParCalibCameraAffine par = g_BaseParComprehensive.GetCellParCalibByType("相机投影校准", pos) as ParCalibCameraAffine;
                string nameCell = par.NameCell;//单元格名称
                int indexCell = int.Parse(nameCell.Replace("C", ""));//单元格索引

                //关联算法，用来获取基准点
                ParCellExeReference parCellExecuteReferenceForMult = par.g_ParCellExecuteReferenceForMult;
                ParGetResultFromCell parGetResultFromCellMult = par.g_ParGetResultFromCellForMult;
                BaseDealComprehensiveResult baseDealComprehensive = GetDealComprehensiveResult(parGetResultFromCellMult.NoCameraMult);//
                baseDealComprehensive.GetCellResultValue(parGetResultFromCellMult, par, null);//执行算法但不显示
                Hashtable htResult_Mult = GetHtResult(parGetResultFromCellMult.NoCameraMult);//获取不同相机的哈希表结果


                FunCellDataReferenc funMult = new FunCellDataReferenc();//将结果传入参数
                funMult.GetResultValue(htResult_Mult, parGetResultFromCellMult, index - 1);

                //关联算法，用来获取像素点
                ParCellExeReference parCellExeReference = par.g_ParCellExecuteReference;
                ParGetResultFromCell parGetResultFromCell = par.g_ParGetResultFromCell;

                //调用执行
                g_BaseDealComprehensive.DealComprehensivePosCell_ForTestRun(g_UCDisplayCamera, parCellExeReference, par.g_ParCellExecuteReference.g_CellExecute_L, out htResult);//执行算法且显示

                string cellError = "";
                if (!GetErrorCell(htResult, pos,out cellError))
                {
                    WinMsgBox.ShowMsgBox(string.Format("相机{0}位置{1}，单元格{2}校准计算错误", g_NoCamera.ToString(), pos.ToString(), par.NameCell));
                    return false;
                }
                FunCellDataReferenc fun = new FunCellDataReferenc();
                fun.GetResultValue(htResult, parGetResultFromCell, index - 1);

                string info = string.Format("相机{0}位置{1}，单元格{2}相机校准触发拍照", g_NoCamera.ToString(), pos.ToString(), par.NameCell);
                ShowState_Cam(info);

                //通过传入的坐标点，计算出投影变换的系数
                FunCalibCameraAffine funCalibCameraAffine = new FunCalibCameraAffine();
                ResultCalibCameraAffine result = funCalibCameraAffine.CalCalibCameraAffine(par, htResult);
                
                //参数保存到Par
                bool blSavePar = funCalibCameraAffine.SaveResultToPar(par, result);

                info = "将标定参数保存到结果";
                ShowState_Cam(info);

                //将参数保存到结果中
                bool blSaveResult = funCalibCameraAffine.SaveResultToStdResult(par, htResult);


                if (blSaveResult)
                {
                    ShowState_Cam(string.Format("将相机{0}参数{1}保存到结果成功", g_NoCamera.ToString(), par.NameCell));
                }
                else
                {
                    ShowAlarm_Cam(string.Format("将相机{0}参数{1}保存到结果失败", g_NoCamera.ToString(), par.NameCell));
                    return false;
                }

                //保存基准图片
                SaveStdImage(par, "相机投影校准");
                //保存此单元格
                bool blSave = g_BaseParComprehensive.WriteXmlDoc(par.NameCell);

                if (blSave)
                {
                    ShowState_Cam(string.Format("将相机{0}参数{1}保存到本地成功", g_NoCamera.ToString(), par.NameCell));
                }
                else
                {
                    ShowAlarm_Cam(string.Format("将相机{0}参数{1}保存到本地失败", g_NoCamera.ToString(), par.NameCell));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                WinMsgBox.ShowMsgBox(string.Format("相机{0}投影校准计算进入Catch", g_NoCamera.ToString()));
                ShowAlarm("相机投影校准计算进入Catch");
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
    }
}
