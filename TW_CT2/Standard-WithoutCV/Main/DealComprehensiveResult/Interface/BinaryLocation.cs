using BasicClass;
using CalibDataManager;
using DealCalibrate;
using DealComprehensive;
using DealPLC;
using ModulePackage;
using ParComprehensive;
using System;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 双目定位

        #region 定义
        public static bool blValid1_BinCam1 = false;
        public static bool blValid2_BinCam1 = false;
        public static bool blValid3_BinCam1 = false;
        public static bool blValid4_BinCam1 = false;
        public static bool blValid1_BinCam2 = false;
        public static bool blValid2_BinCam2 = false;
        public static bool blValid3_BinCam2 = false;
        public static bool blValid4_BinCam2 = false;
        #endregion

        public StateComprehensive_enum BinLocation(Point2D std)
        {
            if (blValid2_BinCam1 && blValid2_BinCam2)
            {
                blValid2_BinCam1 = blValid2_BinCam2 = false;

                double r = GetCurAngle(ModelParams.confDisMark, AMP, 
                    pt2MarkArray[(int)PtType_Mono.AutoMark1], pt2MarkArray[(int)PtType_Mono.AutoMark2]);
                if (!ModuleBase.GetPtAfterRotate(pt2MarkArray[(int)PtType_Mono.AutoMark1], -r, 
                    strCamera5RC, ParComprehensive5.P_I, out Point2D ptAfterR))
                {
                    ShowState("旋转中心计算失败");
                    return StateComprehensive_enum.False;
                }

                Point2D delta = ptAfterR - std;
                calcResultInsp[0] = delta.DblValue1 * AMP;
                calcResultInsp[1] = delta.DblValue2 * AMP;
                calcResultInsp[2] = r;
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.InspDeltaX, 3, calcResultInsp);
                //发送确认信号，相机5对应巡边确认，相机6对应插栏确认，巡边确认对应索引13，插栏对应14
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.InspDeviationConfirm, 1);
                ShowState(string.Format("发送巡边前补偿X:{0}/Y:{1}/R:{2}", calcResultInsp[0], calcResultInsp[1], calcResultInsp[2]));
            }
            return StateComprehensive_enum.False;
        }

        /// <summary>
        /// 计算单工位角度
        /// </summary>
        /// <param name="index">工位号</param>
        /// <param name="rc">旋转中心</param>
        /// <param name="cameraNo">相机编号</param>
        /// <param name="disMark">mark间距</param>
        /// <param name="srcAngle">起始角度</param>
        /// <param name="dstAngle">终点角度</param>
        /// <param name="botAngle">相机朝上，铭牌和机器人Y轴的夹角（逆时针）</param>
        /// <returns></returns>
        public Point3D Calc(int index, Point2D rc, int cameraNo, double disMark, double srcAngle, double dstAngle, double botAngle)
        {
            Point3D pt3Result = new Point3D();
            //数组索引从0开始
            --index;
            try
            {
                //用于计算结果的工具类
                FunCalibRotate funCalibRotate = new FunCalibRotate();
                //计算当前位置与标定位置的角度差
                double deltar = CalibDataMngr.instance.CurAngle(disMark, cameraNo) - CalibDataMngr.instance.CalibPos_L[index].DblValue4;
                //计算旋转之后的mark位置
                Point2D dblMark1AfterR = funCalibRotate.GetPoint_AfterRotation(deltar / 180 * Math.PI, rc, CalibDataMngr.instance.pt2Mark1);
                //计算xy和标定位置的偏差
                double dblDeltaX = dblMark1AfterR.DblValue1 - CalibDataMngr.instance.CalibPos_L[index].DblValue1;
                double dblDeltaY = dblMark1AfterR.DblValue2 - CalibDataMngr.instance.CalibPos_L[index].DblValue2;
                //根据当前角度和目标位置角度计算实际偏差
                Point2D delta = TransDelta(new Point2D(dblDeltaX * AMP, dblDeltaY * AMP), srcAngle, dstAngle);
                //给计算结果赋值
                pt3Result.DblValue1 = delta.DblValue1;
                pt3Result.DblValue2 = delta.DblValue2;
                pt3Result.DblValue3 = deltar;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }

            return pt3Result;
        }

        /// <summary>
        /// 标定
        /// </summary>
        /// <param name="index">当前标定的工位号</param>
        /// <returns></returns>
        public bool Calibration(Point2D pt2Mark1, Point2D pt2Mark2, double disMark, double ratio, int index)
        {
            try
            {
                //判断是否两次拍照完成
                if (blValid2_BinCam1 && blValid2_BinCam2)
                {
                    blValid2_BinCam1 = blValid2_BinCam2 = false;
                    //数组索引从0开始
                    //y1-y2/dis求角度,角度
                    double deltay = (pt2Mark2.DblValue2 - pt2Mark1.DblValue2) * ratio;
                    double r = Math.Atan(deltay / disMark) * 180 / Math.PI;
                    //保存标定位置
                    CalibDataMngr.instance.CalibPos_L[index].DblValue1 = pt2Mark1.DblValue1;
                    CalibDataMngr.instance.CalibPos_L[index].DblValue2 = pt2Mark1.DblValue2;
                    CalibDataMngr.instance.CalibPos_L[index].DblValue4 = r;
                    //输出标定位置
                    ShowState(string.Format("工位{0}标定X:{1}", index, CalibDataMngr.instance.CalibPos_L[index].DblValue1));
                    ShowState(string.Format("工位{0}标定Y:{1}", index, CalibDataMngr.instance.CalibPos_L[index].DblValue2));
                    ShowState(string.Format("工位{0}标定R:{1}", index, CalibDataMngr.instance.CalibPos_L[index].DblValue4));
                    //把标定结果写入本地
                    CalibDataMngr.instance.WriteCalibResult(index);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        public bool Calibration()
        {
            return Calibration(pt2MarkArray[(int)PtType_Mono.CalibMark1], pt2MarkArray[(int)PtType_Mono.CalibMark2],
                ModelParams.confDisMark, ParCalibWorld.V_I[g_NoCamera], 0);
        }

        public bool CalibStd()
        {
            try
            {
                if (blValid3_BinCam1 && blValid3_BinCam2)
                {
                    blValid3_BinCam1 = blValid3_BinCam2 = false;
                    double r1 = ModuleBase.GetCurAngleByX(ModelParams.confDisMark, AMP, 
                        pt2MarkArray[(int)PtType_Mono.CalibMark1], pt2MarkArray[(int)PtType_Mono.CalibMark2]);
                    double r2 = ModuleBase.GetCurAngleByX(ModelParams.confDisMark, AMP, 
                        pt2MarkArray[(int)PtType_Mono.R180Mark1], pt2MarkArray[(int)PtType_Mono.R180Mark2]);
                    if (Math.Abs(r1 - r2) > 0.2)
                    {
                        ShowAlarm(string.Format("相机{0}两次标定角度差别过大", g_NoCamera));
                    }

                    return RecordStdValue(strCamera5Match1, pt2MarkArray[(int)PtType_Mono.CalibMark1], 
                        pt2MarkArray[(int)PtType_Mono.R180Mark1]);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            return true;
        }

        /// <summary>
        /// 计算旋转中心
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pt2Mark1"></param>
        /// <param name="pt2Mark2"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="parComprehensive"></param>
        /// <returns></returns>
        public void CalibRotateCenter()
        {
            if (blValid4_BinCam1 && blValid4_BinCam2)
            {
                blValid4_BinCam1 = blValid4_BinCam2 = false;
                ShowState(string.Format("开始计算旋转中心"));
                if (!CalibRotateCenter(strCamera5RC, pt2MarkArray[(int)PtType_Mono.R180Mark2],
                    pt2MarkArray[(int)PtType_Mono.RcMark2], ModelParams.RCCalibAngle,
                    ParComprehensive5.P_I))
                {
                    ShowState("旋转中心计算失败");
                    return;
                }

                VerifyRotateCenter(pt2MarkArray[(int)PtType_Mono.R180Mark1],
                    pt2MarkArray[(int)PtType_Mono.R180Mark2],
                    pt2MarkArray[(int)PtType_Mono.RcMark1],
                    pt2MarkArray[(int)PtType_Mono.RcMark2], ModelParams.RCCalibAngle, 0.2);
            }
        }

       
        /// <summary>
        /// 对计算旋转中心的角度进行校验，避免plc或机器人旋转角度与计算使用角度不符
        /// </summary>
        /// <param name="pt2MarkSrc1">标定点1</param>
        /// <param name="pt2MarkSrc2">标定点2</param>
        /// <param name="pt2MarkDst1">计算旋转中心点1</param>
        /// <param name="pt2Mark2Dst2">计算旋转中心点2</param>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool VerifyRotateCenter(Point2D pt2MarkSrc1, Point2D pt2MarkSrc2, 
            Point2D pt2MarkDst1, Point2D pt2Mark2Dst2, double r, double thread)
        {
            //求取旋转前和旋转后的玻璃角度
            double srcR = ModuleBase.GetCurAngleByY(ModelParams.MonoGlassX, AMP, pt2MarkSrc1, pt2MarkSrc2);
            double dstR = ModuleBase.GetCurAngleByY(ModelParams.MonoGlassX, AMP, pt2MarkDst1, pt2Mark2Dst2);
            ShowState(string.Format("旋转前玻璃角度:{0}", srcR));
            ShowState(string.Format("当前玻璃角度:{0}", dstR));
            //镜像机符号相反
            double verifyR = ModelParams.IfMirrior ? dstR - srcR : srcR - dstR;
            double deviation = Math.Abs(verifyR - r);
            if (deviation > thread)
            {
                ShowState(string.Format("旋转中心校验失败，使用角度:{0},校验角度：{1}", r, verifyR));
                return false;
            }
            else
                ShowState(string.Format("旋转中心校验成功，使用角度:{0},校验角度：{1}", r, verifyR));
            return true;
        }
        #endregion

    }
}
