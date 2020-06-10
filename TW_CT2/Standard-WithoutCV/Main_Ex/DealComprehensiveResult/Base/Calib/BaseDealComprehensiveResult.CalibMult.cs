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
    /// 多目校准
    /// </summary>
    partial class BaseDealComprehensiveResult
    {
        /// <summary>
        /// 通过拍照位置来区分标定相机，采用标定板，
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="index"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public virtual StateComprehensive_enum DealComprehensiveResultFun_CalibMult(TriggerSource_enum trigerSource_e, int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            bool blResult = false;
            try
            {
                if (index != 99)//触发获取点位
                {
                    blResult = CalibMult_GetPoint(pos, index, out htResult);
                }
                else//触发计算校准结果
                {

                }
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
        /// 多目校准，获取坐标
        /// </summary>
        /// <param name="pos">算子的序号，并非拍照位置</param>
        /// <param name="index">拍照次序</param>
        /// <param name="htResult">哈希表结果</param>
        /// <returns></returns> 
        bool CalibMult_GetPoint(int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            try
            {
                List<string> nameCell_L = g_BaseParComprehensive.GetAllNameCellByType("多目校准");
                if (nameCell_L.Count < pos)
                {
                    WinMsgBox.ShowMsgBox("算子个数小于序号!");
                    return false;
                }
                string nameCell = nameCell_L[pos - 1];
                ParCalibMult par = g_BaseParComprehensive.GetCellParCalibrate(nameCell) as ParCalibMult;

                int indexCell = int.Parse(nameCell.Replace("C", ""));//单元格索引

                //关联算法，用来获取角点
                ParCellExeReference parCellExeReference = par.g_ParCellExecuteReference;
                ParGetResultFromCell parGetResultFromCell = par.g_ParGetResultFromCell;

                //第一次获取坐标的时候，清空历史记录,不能Clear，只能赋值0
                if (index == 1)
                {
                    for (int i = 0; i < parGetResultFromCell.ResultforCalib_L.Count; i++)
                    {
                        parGetResultFromCell.ResultforCalib_L[i].XResult = 0;
                        parGetResultFromCell.ResultforCalib_L[i].YResult = 0;
                    }
                }

                //调用执行
                g_UCDisplayCamera.GrabImageAndShow();
                g_BaseDealComprehensive.DealComprehensivePosCell_ForTestRun(g_UCDisplayCamera, parCellExeReference, par.g_ParCellExecuteReference.g_CellExecute_L, out htResult);//执行算法且显示

                //比对对应的单元格名称，判断结果是否正确
                string cellError = "";
                if (!GetErrorCell(htResult, par.g_ParCellExecuteReference.NameCellExecute_L,out cellError))
                {
                    WinMsgBox.ShowMsgBox(string.Format("相机{0}位置{1}，单元格{2}校准计算错误", g_NoCamera.ToString(), pos.ToString(), par.NameCell));
                    return false;
                }

                FunCellDataReferenc fun = new FunCellDataReferenc();
                fun.GetResultValue(htResult, parGetResultFromCell, index - 1);

                string info = string.Format("相机{0}位置{1}，单元格{2}相机多目校准触发拍照", g_NoCamera.ToString(), pos.ToString(), par.NameCell);
                ShowState(info);

                //保存基准图片
                SaveStdImage(par, "多目校准");

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
                WinMsgBox.ShowMsgBox(string.Format("相机{0}多目校准计算进入Catch", g_NoCamera.ToString()));
                ShowAlarm("相机校准计算进入Catch");
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }


        /// <summary>
        /// 计算相机的映射系数
        /// </summary>
        /// <param name="par1">基准相机参数</param>
        /// <param name="index1">算子索引号</param>
        /// <param name="par2"></param>
        /// <param name="index2">算子索引号</param>
        /// <returns></returns>
        bool CalibMult_Co(BaseParComprehensive par1, int index1, BaseParComprehensive par2, int index2)
        {
            try
            {
                //第一个相机
                List<string> name1_L = par1.GetAllNameCellByType("多目校准");
                if (name1_L.Count < index1)
                {
                    WinMsgBox.ShowMsgBox("算子个数小于序号!");
                    return false;
                }
                string name1 = name1_L[index1 - 1];
                ParCalibMult parCalibMult1 = par1.GetCellParCalibrate(name1) as ParCalibMult;

                //第二个相机
                List<string> name2_L = par2.GetAllNameCellByType("多目校准");
                if (name2_L.Count < index1)
                {
                    WinMsgBox.ShowMsgBox("算子个数小于序号!");
                    return false;
                }
                string name2 = name2_L[index1 - 1];
                ParCalibMult parCalibMult2 = par2.GetCellParCalibrate(name2) as ParCalibMult;

                //计算系数
                FunCalibMult fun = new FunCalibMult();
                fun.CalCalibMult(parCalibMult1, parCalibMult2);

                //保存此单元格
                bool blSave = par1.WriteXmlDoc(par1.NameCell);
                if (blSave)
                {
                    ShowState(string.Format("将相机{0}参数{1}保存到本地成功", par1.NoCamera.ToString(), par1.NameCell));
                }
                else
                {
                    ShowAlarm(string.Format("将相机{0}参数{1}保存到本地失败", par1.NoCamera.ToString(), par1.NameCell));
                    return false;
                }

                blSave = par2.WriteXmlDoc(par2.NameCell);
                if (blSave)
                {
                    ShowState(string.Format("将相机{0}参数{1}保存到本地成功", par2.NoCamera.ToString(), par2.NameCell));
                }
                else
                {
                    ShowAlarm(string.Format("将相机{0}参数{1}保存到本地失败", par2.NoCamera.ToString(), par2.NameCell));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {           
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 保存多目校准的系数
        /// </summary>
        /// <param name="noCamera_e">相机序号</param>
        /// <param name="index">算子序号，不是拍照位置</param>
        /// <param name="calib"></param>
        /// <returns></returns>
        public static bool SaveCalibMultCo(NoCamera_enum noCamera_e, int index, double[] calib)
        {
            try
            {
                BaseParComprehensive par = GetParComphensive(noCamera_e);
                //index 获取对应算子的所有单元格名称
                List<string> nameCell_L = par.GetAllNameCellByType("多目校准");
                if (nameCell_L.Count < index)
                {
                    return false;
                }
                string nameCell = nameCell_L[index - 1];
                ParCalibMult parCalibMult = par.GetCellParCalibrate(nameCell) as ParCalibMult;
                for (int i = 0; i < calib.Length; i++)
                {
                    parCalibMult.CoCalib[i] = calib[i];
                }
                bool blSave = par.WriteXmlDoc(nameCell);
                if (blSave)
                {
                    ShowState(string.Format("将多目校准{0}参数保存到本地成功", nameCell));
                }
                else
                {
                    ShowAlarm(string.Format("将多目校准{0}参数保存到本地失败", nameCell));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
                return false;
            }
        }
    }
}
