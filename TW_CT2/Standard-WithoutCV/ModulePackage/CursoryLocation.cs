using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealCalibrate;
using Main_EX;
using DealRobot;

namespace ModulePackage
{
    /// <summary>
    /// 粗定位
    /// </summary>
    public class CursoryLocation : ModuleBase
    {

        new static string ClassName = "CursoryLocation";

        /// <summary>
        /// 发送机器人取片坐标
        /// </summary>
        /// <param name="pt2Pixel">匹配的得到的计算结果</param>
        /// <param name="pt4Dst">取片基准点位（空运转取片点位）</param>
        /// <param name="pt4Adj">取片调整值</param>
        /// <param name="cmd">交互指令</param>
        /// <param name="parCalibrate"></param>
        /// <returns></returns>
        public static bool SendRobotPickPos(Point2D pt2Pixel, Point4D pt4Dst, Point4D pt4Adj, 
            string cmd, BaseParCalibRobot parCalibrate,out Point4D pResult)
        {
            pResult = new Point4D();
            try
            {
                if (!CalcRobotPickPos(pt2Pixel, pt4Dst, pt4Adj, parCalibrate, out Point4D result))
                    return false;
                pResult = result;
                ShowState(string.Format("发送给机器人坐标:X:{0},Y:{1},R:{2}", 
                    result.DblValue1.ToString(ReservDigits), 
                    result.DblValue2.ToString(ReservDigits), 
                    result.DblValue4.ToString(ReservDigits)));
                LogicRobot.L_I.WriteRobotCMD(result, cmd);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 计算机器人取片坐标，纯计算不带信息输出
        /// </summary>
        /// <param name="pt2Pixel">粗定位相机的像素结果</param>
        /// <param name="pt4Dst">机器人取片基准值</param>
        /// <param name="pt4Adj">机器人取片补偿</param>
        /// <param name="parCalibrate">机器人校准算子参数</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool CalcRobotPickPos(Point2D pt2Pixel, Point4D pt4Dst, Point4D pt4Adj, 
            BaseParCalibRobot parCalibrate, out Point4D result)
        {
            result = pt4Dst + pt4Adj;
            try
            {
                Point2D pt2Map = FunCalibRobot.F_I.GetWordCord(pt2Pixel, parCalibrate);
                if (pt2Map.DblValue1 * pt2Map.DblValue2 == 0)
                {
                    return false;
                }
                result = new Point4D(pt2Map.DblValue1 + pt4Adj.DblValue1, pt2Map.DblValue2 +
                    pt4Adj.DblValue2, pt4Dst.DblValue3 + pt4Adj.DblValue3, pt4Dst.DblValue4 + pt4Adj.DblValue4);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }

            return true;
        }
    }
}
