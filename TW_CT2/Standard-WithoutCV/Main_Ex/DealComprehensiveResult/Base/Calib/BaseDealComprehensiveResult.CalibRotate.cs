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
        /// 通过拍照位置来区分标定旋转中心，
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="pos">算子序号,不表示拍照位置</param>
        /// <param name="index"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public virtual StateComprehensive_enum DealComprehensiveResultFun_CalibRotate(TriggerSource_enum trigerSource_e, int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            bool blResult = false;
            try
            {
                blResult = CalibRotate(pos, index, out htResult);
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
        /// 自动校准旋转中心,1401表示位置1第一次拍照，
        /// </summary>
        /// <param name="pos">算子序号，不表示拍照位置</param>
        /// <param name="index"></param>
        /// <param name="htResult">哈希表结果</param>
        /// <returns></returns>
        bool CalibRotate(int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            Stopwatch sw = new Stopwatch();
            try
            {
                //获取该算子所有的单元格
                List<string> nameCell_L = g_BaseParComprehensive.GetAllNameCellByType("旋转中心变换");
                if (nameCell_L.Count < pos)
                {
                    WinMsgBox.ShowMsgBox("算子个数小于序号!");
                    return false;
                }
                string nameCell = nameCell_L[pos - 1];//单元格名称

                ParCalibRotate par = g_BaseParComprehensive.GetCellParCalibrate(nameCell) as ParCalibRotate;

                int indexCell = int.Parse(nameCell.Replace("C", ""));//单元格索引

                ParCellExeReference parCellExeReference = par.g_ParCellExecuteReference;
                ParGetResultFromCell parGetResultFromCell = par.g_ParGetResultFromCellForRC;//用来计算旋转中心

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

                string info = string.Format("相机{0}位置{1}，单元格{2}", g_NoCamera.ToString(), pos.ToString(), par.NameCell);
                if (index == 1)
                {
                    info += "旋转中心校准第一次拍照";
                    ShowState(info);

                    SaveStdImage(par, "旋转中心位置1");
                }
                else if (index == 2)
                {
                    info += "旋转中心校准第二次拍照";
                    ShowState(info);
                    SaveStdImage(par, "旋转中心位置2");

                    FunCalibRotate funCalibRotate = new FunCalibRotate();
                    Point2D point = funCalibRotate.GetOriginPoint(par);
                    par.XRC = point.DblValue1;
                    par.YRC = point.DblValue2;

                    //显示求取的旋转中心
                    info = string.Format("旋转中心:X{0},Y{1}", par.XRC.ToString(), par.YRC.ToString());

                    ShowState(info);

                    //保存此单元格
                    bool blSave = g_BaseParComprehensive.WriteXmlDoc(par.NameCell);

                    if (blSave)
                    {
                        ShowState(string.Format("将参数旋转中心校准{0}保存到本地成功", par.NameCell));
                    }
                    else
                    {
                        ShowAlarm(string.Format("将参数旋转中心校准{0}保存到本地失败", par.NameCell));
                        return false;
                    }

                    //将参数保存到结果中
                    bool blSaveResult = g_BaseDealComprehensive.InitCalibRotate(indexCell);
                    if (blSaveResult)
                    {
                        ShowState(string.Format("将参数旋转中心校准{0}保存到结果成功", par.NameCell));
                    }
                    else
                    {
                        ShowAlarm(string.Format("将参数旋转中心校准{0}保存到结果失败", par.NameCell));
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowAlarm(string.Format("相机{0}旋转中心计算进入Catch", g_NoCamera.ToString()));
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }           
        }
    }
}
