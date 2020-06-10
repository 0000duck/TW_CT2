using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealComprehensive;
using Camera;
using System.Collections;
using DealRobot;
using Main_EX;
using DealResult;
using DealCalibrate;

namespace ModulePackage
{
    public class BackLightLocation : ModuleBase
    {
        static readonly string ClassName = "BackLightLocation";

        /// <summary>
        /// 背光定位，不规则区域，计算并发送角度到robot1
        /// </summary>
        /// <param name="precisePos">精定位拍照位置</param>
        /// <param name="cmdAngle">与bot的协议，表示拍照ok</param>
        /// <param name="cmdError">与bot的协议，表示拍照失败</param>
        /// <param name="threadR">角度阈值</param>
        /// <param name="deltar">算子直接获得的角度</param>
        /// <param name="displayType">画面设置，影响角度计算</param>
        /// <param name="id">unique id</param>
        /// <param name="ifRecord">是否进行数据记录</param>
        /// <param name="r">输出计算后不要补正的角度偏差</param>
        /// <returns></returns>
        public static bool BlobAngleCalc(double deltar, Point4D precisePos, string cmdAngle, string cmdError,
            double threadR, BackLightDisplay_Enum displayType, int id, bool ifRecord, out double r)
        {
            r = 0;
            try
            {
                r = GetDeltaR(deltar, displayType);

                if (Math.Abs(r) > threadR)
                {
                    ShowAlarm("精确定位处角度:" + r.ToString(ReservDigits) + "超过设定阈值:" + threadR.ToString(ReservDigits));
                    LogicRobot.L_I.WriteRobotCMD(cmdError);
                    return false;
                }

                ShowState("玻璃精确定位处角度:" + r.ToString(ReservDigits));
                //发送角度让机器人调整,第一次拍照结果
                LogicRobot.L_I.WriteRobotCMD(precisePos.Add(3, r), cmdAngle);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            finally
            {
                if (ifRecord)
                    RecordPreciseData(id, "角度" + r.ToString());
            }
        }

        /// <summary>
        /// 背光，不规则区域，对面积进行判别
        /// </summary>
        /// <param name="cmdError">与bot的协议，表示面积超限</param>
        /// <param name="threadMax">面积阈值上限</param>
        /// <param name="threadMin">面积阈值下限</param>
        /// <param name="ratio">相机系数</param>
        /// <param name="tArea">理论面积</param>
        /// <param name="area">实际像素面积</param>
        /// <param name="id"></param>
        /// <param name="ifRecord"></param>
        /// <returns></returns>
        public static bool BlobAreaDetect(string cmdError, double threadMax, double threadMin, double ratio,
            double tArea, double area, int id, bool ifRecord,out double[] dblResult)
        {
            dblResult = new double[2];
            try
            {
                #region 面积判断
                double areaPixel = area * ratio * ratio;
                double areaRatio = Math.Round(areaPixel / tArea, 4);
                dblResult = new double[2] { areaPixel, areaRatio };
                ShowState(string.Format("玻璃面积比为[{0}]，阈值为[{1}~{2}]", areaRatio.ToString(ReservDigits), threadMax, threadMin));

                if (ifRecord)
                    RecordPreciseData(id, "Area:" + areaRatio.ToString());

                if (areaRatio > threadMax || areaRatio < threadMin)
                {
                    ShowAlarm(string.Format("ID:{0}面积比为{1}，超出阈值", id, areaRatio.ToString(ReservDigits)));
                    LogicRobot.L_I.WriteRobotCMD(cmdError);
                    return false;
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
        }

        /// <summary>
        /// 背光，不规则区域，偏差计算
        /// </summary>
        /// <param name="dst">目标点bot基准坐标</param>
        /// <param name="delta">画面实际偏差（mm）</param>
        /// <param name="srcAngle">当前位置机器人角度</param>
        /// <param name="dstAngle">目标点机器人角度</param>
        /// <param name="r">第一次拍照获得的角度偏差</param>
        /// <param name="cmdOK">与bot协议，表示偏差计算ok</param>
        /// <param name="cmdError">与bot协议，表示偏差计算失败</param>
        /// <param name="threadX">偏差阈值X</param>
        /// <param name="threadY">偏差阈值Y</param>
        /// <param name="botAngle">机器人放置角度,</param>
        /// <param name="displayAngle">画面角度</param>
        /// <param name="blAngle">背光放置角度</param>
        /// <param name="id"></param>
        /// <param name="ifRecord"></param>
        /// <returns></returns>
        public static bool BolbDevationCalc(Point4D dst, Point2D delta, double srcAngle,
            double dstAngle, double r, string cmdOK, string cmdError, double threadX, double threadY,
            double botAngle, double displayAngle, double blAngle, int id, bool ifRecord, out Point2D deltaDst)
        {
            deltaDst = new Point2D();
            try
            {
                deltaDst = GetDeviationForBot(delta, botAngle, displayAngle, blAngle, srcAngle, dstAngle);
                ShowState(string.Format("精定位偏差X:{0}，Y{1}", 
                    deltaDst.DblValue1.ToString("f3"),
                    deltaDst.DblValue2.ToString(ReservDigits)));

                if (deltaDst.DblValue1 > threadX || deltaDst.DblValue2 > threadY)
                {
                    ShowAlarm(string.Format("精定位偏差超出阈值[{0},{1}]", threadX, threadY));
                    LogicRobot.L_I.WriteRobotCMD(cmdError);
                    return false;
                }
                
                Point4D pt4Dst = new Point4D(deltaDst.DblValue1, deltaDst.DblValue2, 0, r) + dst;
                LogicRobot.L_I.WriteRobotCMD(pt4Dst, cmdOK);
                ShowState(string.Format("发送放片坐标:X:{0},Y:{1},R:{2}",
                    pt4Dst.DblValue1.ToString(ReservDigits), 
                    pt4Dst.DblValue2.ToString(ReservDigits), 
                    pt4Dst.DblValue4.ToString(ReservDigits)));

                if (ifRecord)
                    RecordPreciseData(id, "第二次精定位结果：X-->" + deltaDst.DblValue1.ToString(ReservDigits) + 
                        " Y-->" + deltaDst.DblValue2.ToString(ReservDigits) + " R-->" + r.ToString(ReservDigits));
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
        }

        /// <summary>
        /// 背光一次定位偏差计算
        /// </summary>
        /// <param name="pt2Src">像素坐标，精定位处获取的计算结果</param>
        /// <param name="pt2Rc">旋转中心</param>
        /// <param name="r">角度补偿，即需要旋转的角度</param>
        /// <param name="ratio">相机系数</param>
        /// <param name="type">背光方向</param>
        /// <param name="srcAngle">精定位处玻璃角度</param>
        /// <param name="dstAngle">目标点玻璃角度</param>
        /// <param name="botAngle">机器人放置角度</param>
        /// <param name="displayAngle">画面显示角度</param>
        /// <param name="blAngle">背光角度</param>
        public static void Verify(Point2D pt2Src, Point2D pt2Rc, double r, double ratio, BackLightDisplay_Enum type,
            double srcAngle, double dstAngle, double botAngle, double displayAngle, double blAngle)
        {
            try
            {
                r = GetDeltaR(r, type);
                Point2D pt2AfterR = new FunCalibRotate().GetPoint_AfterRotation(r / 180 * Math.PI, pt2Rc, pt2Src);
                Point2D delta = pt2AfterR - pt2Rc;
                delta = GetDeviationForBot(delta, botAngle, displayAngle, blAngle, srcAngle, dstAngle);
                delta = new Point2D(delta.DblValue1 * ratio, delta.DblValue2 * ratio);
                ShowState(string.Format("一次拍照计算的偏差结果：X:{0}/Y:{1}", 
                    delta.DblValue1.ToString(ReservDigits), 
                    delta.DblValue2.ToString(ReservDigits)));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 用于不规则区域角度补偿值的计算，将不规则区域直接获得的角度值转换成补正的偏差角度
        /// </summary>
        /// <param name="r">不规则区域获得的R_J</param>
        /// <param name="displayType">背光显示方向</param>
        /// <returns></returns>
        public static double GetDeltaR(double r, BackLightDisplay_Enum displayType)
        {
            double deltaR = Math.Round(r, 3);
            if (displayType == BackLightDisplay_Enum.Vertical)
            {
                deltaR += r < 0 ? 90 : -90;
            }
            return deltaR;
        }

        /// <summary>
        /// 根据机械布局，以及src/dst的角度差，计算实际补正偏差
        /// </summary>
        /// <param name="delta">需要进行转换计算的原始偏差</param>
        /// <param name="botAngle">机器人放置角度</param>
        /// <param name="displayAngle">图像显示角度</param>
        /// <param name="blAngle">背光角度</param>
        /// <param name="srcAngle">精定位处玻璃角度</param>
        /// <param name="dstAngle">目标点玻璃角度</param>
        /// <returns></returns>
        public static Point2D GetDeviationForBot(Point2D delta, double botAngle, double displayAngle, 
            double blAngle, double srcAngle, double dstAngle)
        {            
            return TransDelta(CalcBLDeviation(delta, botAngle, displayAngle, blAngle), dstAngle, srcAngle);
        }

        /// <summary>
        /// 根据画面偏差计算当前位置机器人的补正偏差
        /// </summary>
        /// <param name="delta">需要进行转换的偏差</param>
        /// <param name="botAngle">机器人角度</param>
        /// <param name="displayAngle">画面显示角度</param>
        /// <param name="blAngle">背光角度</param>
        /// <returns></returns>
        public static Point2D CalcBLDeviation(Point2D delta,
            double botAngle, double displayAngle, double blAngle)
        {
            Point2D pt2D = new Point2D();
            try
            {
                //确定初始符号
                //  pt2D = new Point2D(delta.DblValue2, -delta.DblValue1);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            //根据布局转换偏差
            return TransDelta(delta, botAngle + displayAngle + 270, blAngle);
        }
    }

    public enum BackLightDisplay_Enum
    {
        Vertical,
        Horizontal
    }
}
