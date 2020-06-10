using BasicClass;
using DealCalibrate;
using DealComprehensive;
using DealPLC;
using DealResult;
using DealRobot;
using System;
using System.Collections;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 皮带定位
        /// <summary>
        /// 开始进行皮带线扫描
        /// </summary>
        /// <param name="htResult"></param>
        public void BeltScan(out Hashtable htResult)
        {
            htResult = null;
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera8Match1];

                if (result.X * result.Y != 0)
                {
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.StopBelt, 1);
                    StopBelt?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 去皮带线取片，采用基准值+像素偏差*系数进行计算
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum BeltPick(out Hashtable htResult)
        {
            htResult = null;
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = (BaseResult)htResult[strCamera8Match1];
                ResultTemplate temp = (ResultTemplate)htResult[strCamera8Template1];

                if (result.X * result.Y == 0)
                {
                    FinishPhotoPLC(2);
                    ShowState("皮带线定位失败");
                    return StateComprehensive_enum.False;
                }

                Point4D dst = ModelParams.BeltPickPos;
                dst.DblValue1 += result.DeltaY * ParCalibWorld.V_I[g_NoCamera];
                dst.DblValue2 += result.DeltaX * ParCalibWorld.V_I[g_NoCamera];

                LogicRobot.L_I.WriteRobotCMD(dst, ModelParams.cmd_BeltPick);
                FinishPhotoPLC(1);
                ShowState(string.Format("皮带线机器人取片位置：{0}", dst.ToString()));
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }
        #endregion
    }
}
