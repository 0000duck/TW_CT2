using BasicClass;
using DealCalibrate;
using DealComprehensive;
using DealResult;
using DealRobot;
using Main_EX;
using ModulePackage;
using ParComprehensive;
using System;
using System.Collections;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main 
    {
        #region 精定位
        double phi = 0;

        #region 接口
        /// <summary>
        /// 角度计算
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>        
        public StateComprehensive_enum BackLightAngleCalc(string cellName,out Hashtable htResult)
        { 
            if (BlobAngleCalc(strCamera2Match1, out htResult))
            {
                return StateComprehensive_enum.True;
            }
            else
            {
                RegeditMain.R_I.PreciseNG++;
                return StateComprehensive_enum.False;
            }
        }

        public bool BlobAngleCalc(string cellName, out Hashtable htResult)
        {
            htResult = null;
            double[] dblResult = new double[2];
            bool blResult = true;
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState(string.Format("相机{0}空跑，默认发送OK", g_NoCamera));
                    LogicRobot.L_I.WriteRobotCMD(ModelParams.PrecisePos, ModelParams.cmd_PreciseAngle);
                    return true;
                }
                #endregion

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                ResultBlob resultBlob = (ResultBlob)htResult[cellName];

                //算子基准值记录到基准值当中
                if (ModelParams.PreciseStdValue.DblValue1 == 0)
                    ModelParams.PreciseStdValue = new Point2D(resultBlob.StdX, resultBlob.StdY);
                //面积判断
                #region 面积检测
                if (!BlobAreaDetect(resultBlob.Area,out dblResult))
                {
                    ShowState("精定位第一次拍照失败");
                    blResult = false;
                    return false;
                }
                #endregion
                
                //一次拍照计算偏差，目前用于和二次拍照进行对比
                BackLightLocation.Verify(new Point2D(resultBlob.X, resultBlob.Y),
                     new Point2D(resultBlob.StdX, resultBlob.StdY),//不规则区域基准值，即旋转中心
                     resultBlob.R_J,//不规则区域角度
                     ParCalibWorld.V_I[g_NoCamera],//相机系数
                     ModelParams.BLDisplayType,//背光在相机显示中的方向
                     ModelParams.PreciseRobotAngle,//精定位机器人u轴角度
                     ModelParams.BotWastageAngle,//机器人残材平台放片角度
                     ModelParams.BotPlaceAngle,//机器人放置角度
                     ModelParams.DisplayAngle,//相机显示角度
                     ModelParams.BLPlaceAngle);//背光放置方向

                blResult = BackLightLocation.BlobAngleCalc(
                    resultBlob.R_J,//不规则区域角度
                    ModelParams.PrecisePos,//机器人精定位位置
                    ModelParams.cmd_PreciseAngle,//精定位角度调整指令
                    ModelParams.cmd_PreciseFailed,//精定位失败指令
                    ModelParams.PreciseThreadR,//精定位角度偏差阈值                    
                    ModelParams.BLDisplayType,//背光在相机显示中的方向
                    RegeditMain.R_I.ID,//UniqueID
                    ModelParams.IfRecordData,//是否进行数据记录
                    out phi);//输出实际给机器人补偿的角度
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                string strResult = "OK";
                if(!blResult )
                {
                    strResult = "NG";
                }
                g_UCDisplayCamera.ShowResult("面积比例：" + dblResult[1].ToString(ReservDigits) +
                                             "\n角度补正：" + phi.ToString(ReservDigits) +
                                             "\n角度阈值：" +ModelParams.PreciseThreadR.ToString(ReservDigits) +
                                             "\n" + strResult, blResult);
            }
        }

        /// <summary>
        /// 偏差计算
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum BackLightDeviationCalc(string cellName,out Hashtable htResult)
        {
            if (BlobDeviation(strCamera2Match1, out htResult))
            {
                return StateComprehensive_enum.True;
            }
            else
            {
                RegeditMain.R_I.PreciseNG++;
                return StateComprehensive_enum.False;
            }
        }
        /// <summary>
        /// 不规则区域偏差计算
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public bool BlobDeviation(string cellName, out Hashtable htResult)
        {
            htResult = null;
            Point2D pDelta = new Point2D();
            bool blResult = true;
            double[] dblResult = new double[2];
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState("空跑默认发送平台1坐标");
                    LogicRobot.L_I.WriteRobotCMD(ModelParams.PosWastagePlat1, ModelParams.cmd_PreciseResult);
                    return true;
                }
                #endregion

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                ResultBlob resultBlob = (ResultBlob)htResult[cellName];

                #region 面积检测
                if (!BlobAreaDetect(resultBlob.Area, out dblResult))
                {
                    blResult = false;
                    return false;
                }
                #endregion

                //获取像素偏差
                //Point2D delta = new Point2D((ModelParams.isMirrorX ? 1 : -1) * resultBlob.DeltaX * AMP,
                //    (ModelParams.isMirrorY ? 1 : -1) * resultBlob.DeltaY * AMP);
                double deltaX = resultBlob.X - ModelParams.PreciseStdValue.DblValue1;
                double deltaY = resultBlob.Y - ModelParams.PreciseStdValue.DblValue2;
                Point2D delta = new Point2D((ModelParams.isMirrorX ? 1 : -1) * deltaX * AMP,
                    (ModelParams.isMirrorY ? 1 : -1) * deltaY * AMP);
                //将像素偏差代入计算
                blResult = BolbDevationCalc(phi, delta, out pDelta);
                return blResult;
            }
            catch (Exception ex)
            {
                blResult = false;
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                string strResult = "OK";
                if (!blResult)
                {
                    strResult = "NG";
                }
                g_UCDisplayCamera.ShowResult(
                    "面积比例：" + dblResult[1].ToString("f3") +
                    "  阈值：" + ModelParams.AreaMin.ToString(ReservDigits) + "-" + ModelParams.AreaMax.ToString(ReservDigits) +
                    "\n补正值X：" + pDelta.DblValue1.ToString("f3") +
                    "  阈值：" + ModelParams.PreciseThreadX.ToString(ReservDigits) +
                    "\n补正值Y：" + pDelta.DblValue2.ToString("f3") +
                    "  阈值：" + ModelParams.PreciseThreadY.ToString(ReservDigits) +
                    "\n" + strResult, blResult);
            }
        }

        /// <summary>
        /// 背光不规则区域基准值保存
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cellName"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum CalibStdValue(int index, string cellName, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, 1, out htResult);
                ResultBlob resultBlob = (ResultBlob)htResult[cellName];

                if (!DealTypeResult(resultBlob))
                {
                    ShowState(string.Format("精定位自动标定第{0}点拍照失败", index + 1));
                    return StateComprehensive_enum.False;
                }
                //保存当前匹配结果
                pt2Calib[index] = new Point2D(resultBlob.X, resultBlob.Y);

                if (index == 1)
                {
                    LogicRobot.L_I.WriteRobotCMD(ModelParams.cmd_PreciseCalib2);

                    //第二次的话保存基准值到算子中
                    if (!(RecordStdValue(cellName, pt2Calib[0], pt2Calib[1])))
                        return StateComprehensive_enum.False;
                }
                else
                {
                    ShowState(string.Format("相机{0}基准值标定第1点计算成功", g_NoCamera));
                    LogicRobot.L_I.WriteRobotCMD(ModelParams.cmd_PreciseCalib1);
                }

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        #endregion
        /// <summary>
        /// 精定位偏差计算
        /// </summary>
        /// <param name="r">第一次定位取得的角度</param>
        /// <param name="delta">像素偏差</param>
        /// <returns></returns>
        private bool BolbDevationCalc(double r, Point2D delta, out Point2D pDelta)
        {
            //根据配方，选择平台放片目标点
            Point4D dst = ModelParams.confWastagePlatStation == 1 ? ModelParams.PosWastagePlat1 : ModelParams.PosWastagePlat2;

            return BackLightLocation.BolbDevationCalc(dst,
                delta,
                ModelParams.PreciseRobotAngle,
                ModelParams.BotWastageAngle,
                r,
                ModelParams.cmd_PreciseResult,
                ModelParams.cmd_PreciseFailed,
                ModelParams.PreciseThreadX,
                ModelParams.PreciseThreadY,
                ModelParams.BotPlaceAngle,
                ModelParams.DisplayAngle,
                ModelParams.BLPlaceAngle,
                RegeditMain.R_I.ID,
                ModelParams.IfRecordData,
                out pDelta);
        }

        private bool BlobAreaDetect(double area,out double[] dblResult)
        {
            return BackLightLocation.BlobAreaDetect(
                ModelParams.cmd_PreciseFailed,
                ModelParams.AreaMax,
                ModelParams.AreaMin,
                AMP,
                ModelParams.ProductArea,
                area,
                RegeditMain.R_I.ID,
                ModelParams.IfRecordData,
                out dblResult
                );
        }

        /// <summary>
        /// 判断是否存在额料
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="parComprehensive"></param>
        /// <returns></returns>
        public bool Judge(Point2D pos1, Point2D pos2, BaseParComprehensive parComprehensive)
        {
            Point2D dblMark1After = new Point2D();
            Point2D dblMark2After = new Point2D();
            Point2D result;
            try
            {
                //计算当前位置与标定位置的角度差，
                //此处有一个问题，当存在额料的时候，相当于mark实际距离边长，这导致计算出的角度其实有偏差，理论上额料越大，导致偏差越大
                double deltar = GetCurAngle(ModelParams.confGlassY, ParCalibWorld.V_I[g_NoCamera], pos1, pos2);
                //计算旋转之后的mark1位置
                if(!ModuleBase.GetPtAfterRotate(pos1, deltar, strRotateCalibCell1, parComprehensive,out result))
                    return false;
                dblMark1After = result;
                //计算旋转之后的mark2位置
                if(!ModuleBase.GetPtAfterRotate(pos2, deltar, strRotateCalibCell1, parComprehensive,out result))
                    return false;
                dblMark2After = result;
                //求出旋转后两个点的实际偏差
                double deltaX = (dblMark1After.DblValue1 - dblMark2After.DblValue1) * AMP;
                double deltaY = (dblMark1After.DblValue2 - dblMark2After.DblValue2) * AMP;
                //把判断标准和一次拍照统一，共用阈值
                double ratioX = (Math.Abs(deltaX) + ModelParams.GlassXInPrecise) / ModelParams.GlassXInPrecise;
                double ratioY = (Math.Abs(deltaY) + ModelParams.GlassYInPrecise) / ModelParams.GlassYInPrecise;
                //判断是否有额料，是否要做额料位置的区分？
                if (ratioX > ModelParams.AreaMax || ratioY > ModelParams.AreaMax)
                {
                    ShowState("有额料");
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
        /// 将计算结果保存到算子中
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="mark1"></param>
        /// <param name="mark2"></param>
        /// <returns></returns>
        public bool RecordStdValue(string cellName, Point2D mark1, Point2D mark2)
        {            
            return ModuleBase.RecordStdValue(GetParStdByCell(cellName), mark1, mark2);
        }

        /// <summary>
        /// 根据两点y坐标和mark间距计算当前玻璃角度
        /// </summary>
        /// <param name="disMark"></param>
        /// <param name="cameraNo"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public double GetCurAngle(double disMark, double ratio, Point2D pt1, Point2D pt2)
        {
            return ModuleBase.GetCurAngleByY(disMark, ratio, pt1, pt2);
        }

        public struct LocationResult
        {
            public bool IsValid;
            public Point2D result;
        }
        #endregion
    }
}
