using BasicClass;
using DealComprehensive;
using DealPLC;
using DealResult;
using Main_EX;
using ModulePackage;
using System;
using System.Collections;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 残材检测
        /// <summary>
        /// 处理残材检测
        /// </summary>
        /// <param name="cellName">算子序号</param>
        /// <param name="pos">表明用的第几次拍照的结果</param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum DealSharpness(string cellName, Pos_enum pos, out Hashtable htResult)
        {
            htResult = null;
            double[] dblResult;
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState(string.Format("相机{0}空跑默认无残边", g_NoCamera));
                    FinishPhotoPLC(1);
                    return StateComprehensive_enum.True;
                }
                #endregion
                //获取算子计算结果
                StateComprehensive_enum StateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, pos, out htResult);
                ResultSharpness result = (ResultSharpness)htResult[cellName];
                double dblThread = ModelParams.WastageThread1;
                if (g_NoCamera == 4)
                {
                    dblThread = ModelParams.WastageThread2;
                }
                //对计算结果进行判别
                if (WastageDetection.CalcSharpnessRatio(result.Deviation_L, dblThread,
                    g_NoCamera, ModelParams.DefaultOKCamera, ModelParams.IfSingleElectrode, 
                    RegeditMain.R_I.ID, ModelParams.IfRecordData, out dblResult))
                {
                    FinishPhotoPLC(1);
                    g_UCDisplayCamera.ShowResult("拍照位置：" + pos +
                        "\nCF清晰度：" + dblResult[0].ToString(ReservDigits) +
                        "\nTFT清晰度：" + dblResult[1].ToString(ReservDigits) +
                        "\n比例：" + dblResult[2].ToString(ReservDigits) +
                        "\n阈值：" + dblThread.ToString(ReservDigits) +
                        "\nOK");
                    return StateComprehensive_enum.True;
                }
                else
                {   
                    //连续低清晰度报警
                    if (++WastageDetection.WastageInvalidCount[g_NoCamera] > 5)
                    {
                        ShowAlarm(string.Format("相机{0}连续低清晰度报警", g_NoCamera));
                        LogicPLC.L_I.WriteRegData1((int)DataRegister1.WastageAlarm, 1);
                    }
                    else
                    {
                        WastageDetection.WastageInvalidCount[g_NoCamera] = 0;
                    }
                    FinishPhotoPLC(2);
                    g_UCDisplayCamera.ShowResult("拍照位置：" + pos +
                        "\nTFT清晰度：" + dblResult[0].ToString(ReservDigits) +
                        "\nCF清晰度：" + dblResult[1].ToString(ReservDigits) +
                        "\n比例：" + dblResult[2].ToString(ReservDigits) +
                        "\n阈值：" + dblThread.ToString(ReservDigits) +
                        "\nNG", "red");
                    return StateComprehensive_enum.False;
                }
            }
            catch
            {
                FinishPhotoPLC(2);
                ShowAlarm("残才相机处理过程中发生异常");
                return StateComprehensive_enum.False;
            }
        }
        #endregion
    }
}
