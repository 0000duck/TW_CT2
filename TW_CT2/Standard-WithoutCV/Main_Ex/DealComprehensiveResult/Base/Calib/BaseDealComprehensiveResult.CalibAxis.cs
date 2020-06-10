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
        /// 通过拍照位置来区分标定轴，1301表示位置1第一次拍照，1302表示位置2第一次拍照
        /// </summary>
        /// <param name="pos">算子的序号，并非拍照位置</param>
        /// <param name="trigerSource_e">触发源</param>
        /// <param name="index"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public virtual StateComprehensive_enum DealComprehensiveResultFun_CalibAxis(TriggerSource_enum trigerSource_e, int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            bool blResult = false;
            try
            {
                blResult = CalibAxis(pos, index, out htResult);
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
        /// 自动轴,1301表示位置1第一次拍照，轴坐标的计算是双数点进行保存，轴计算是两个数据表示一根轴，20181111-zx
        /// </summary>
        /// <param name="pos">算子的序号，并非拍照位置</param>
        /// <param name="index"></param>
        /// <param name="htResult">哈希表结果</param>
        /// <returns></returns>
        bool CalibAxis(int pos, int index, out Hashtable htResult)
        {
            htResult = null;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            try
            {
                List<string> nameCell_L = g_BaseParComprehensive.GetAllNameCellByType("轴坐标校准");
                if (nameCell_L.Count < pos)
                {
                    WinMsgBox.ShowMsgBox("算子个数小于序号!");
                    return false;
                }
                string nameCell = nameCell_L[pos - 1];

                ParCalibAxis par = g_BaseParComprehensive.GetCellParCalibrate(nameCell) as ParCalibAxis;
                int indexCell = int.Parse(nameCell.Replace("C", ""));//单元格索引

                ParCellExeReference parCellExeReference = par.g_ParCellExecuteReference;
                ParGetResultFromCell parGetResultFromCell = par.g_ParGetResultFromCell;//用来计算轴

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

                //保存基准图片
                SaveStdImage(par, "轴校准" + index.ToString());

                if (index % 2 == 1)
                {
                    info += string.Format("轴校准第{0}次拍照", index);
                    //日志
                    ShowState_Cam(info);
                }
                else
                {
                    info += string.Format("轴校准第{0}次拍照", index);
                    //保存基准图片
                    SaveStdImage(par, "旋转中心位置2");
                    //日志
                    ShowState_Cam(info);

                    FunCalibAxis funCalibCoord = new FunCalibAxis();
                    Point4D pResult = funCalibCoord.GetAxisAngleDir(par);
                    par.XAxisAngle_J = pResult.DblValue1;
                    par.YAxisAngle_J = pResult.DblValue2;

                    par.XAxisDir = (int)pResult.DblValue3;
                    par.YAxisDir = (int)pResult.DblValue4;

                    //显示求取的轴角度
                    info = string.Format("轴角度:X轴{0},Y轴{1}", par.XAxisAngle_J.ToString(), par.YAxisAngle_J.ToString());
                    //日志
                    ShowState_Cam(info);

                    //显示求取的轴方向
                    info = string.Format("轴方向:X轴{0},Y轴{1}", par.XAxisDir.ToString(), par.YAxisDir.ToString());
                    //日志
                    ShowState_Cam(info);

                    //保存此单元格
                    bool blSave = g_BaseParComprehensive.WriteXmlDoc(par.NameCell);

                    if (blSave)
                    {
                        //日志
                        ShowState_Cam(string.Format("将参数{0}保存到本地成功", par.NameCell));
                    }
                    else
                    {
                        //日志
                        ShowAlarm_Cam(string.Format("将参数{0}保存到本地失败", par.NameCell));
                        return false;
                    }

                    //将参数保存到结果中
                    bool blSaveResult = g_BaseDealComprehensive.InitCalibAxis(indexCell);
                    if (blSaveResult)
                    {
                        //日志
                        ShowState_Cam(string.Format("将参数{0}保存到结果成功", par.NameCell));
                    }
                    else
                    {
                        //日志
                        ShowAlarm_Cam(string.Format("将参数{0}保存到结果失败", par.NameCell));
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //日志
                ShowAlarm_Cam("轴校准计算进入Catch");
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
    }
}
