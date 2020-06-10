using BasicClass;
using CalibDataManager;
using DealAlgorithm;
using DealCalibrate;
using DealComprehensive;
using DealResult;
using Main_EX;
using ModulePackage;
using ParComprehensive;
using System;
using System.Collections;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 单相机双次定位
        #region 接口
        /// <summary>
        /// 将当前单目运行的结果记录到点位数组对应的位置
        /// </summary>
        /// <param name="index">数组存放该数据的索引</param>
        /// <param name="cellName">算子序号</param>
        /// <param name="pos">拍照位置</param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public bool DealLocation(int index, string cellName, Pos_enum pos, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState(string.Format("相机{0}第{1}次拍照空跑默认ok", g_NoCamera, index + 1));
                    FinishPhotoPLC(1);
                    return true;
                }
                #endregion
                //保存当前匹配结果到指定的点中
                if (!SaveMatchResult(index, cellName, pos, out htResult))
                {
                    ShowState(string.Format("相机{0}第{1}次拍照失败", g_NoCamera, index + 1));
                    FinishPhotoPLC(CameraResult.NG);
                    return false;
                }

                ShowState(string.Format("相机{0}第{1}次拍照成功", g_NoCamera, index + 1));
                FinishPhotoPLC(CameraResult.OK);
                return true;
            }
            catch (Exception ex)
            {
                FinishPhotoPLC(CameraResult.NG);
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 单目相机偏差计算
        /// </summary>
        /// <param name="index">基准值的索引，计算结果与该基准值进行计算，需要与标定时的index一致</param>
        /// <param name="baseParComprehensive">拍照相机的算子参数，用于索引旋转中心的算子</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool MonoCalc(int index, BaseParComprehensive baseParComprehensive, out double[] result)
        {
            result = new double[3] { 0, 0, 0 };
            try
            {
                //TODO 根据产品两角，以及旋转中心和基准值计算偏差        

                if (!Calc(pt2MarkArray[(int)PtType_Mono.AutoMark1],
                    pt2MarkArray[(int)PtType_Mono.AutoMark2], index, baseParComprehensive, out result))
                {
                    ShowState(string.Format("相机{0}偏差计算出错", g_NoCamera));
                    return false;
                }

                FinishPhotoPLC(CameraResult.OK);
                ShowState(string.Format("单目相机偏差：X:{0},Y:{1},R{2}", result[0], result[1], result[2]));
                return true;
            }
            catch (Exception ex)
            {
                FinishPhotoPLC(CameraResult.NG);
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 单目标定第3，4次拍照时，发送角度补偿给plc
        /// </summary>
        /// <param name="index">基准值的索引</param>
        /// <param name="IfAdjustAngle"></param>
        /// <returns></returns>
        public StateComprehensive_enum CalcCalibAngle(int index, bool IfAdjustAngle = true)
        {
            try
            {
                //根据calibmark计算当前角度，根据目前机台设计，此处全部用的deltaY进行角度计算，预留了X的接口
                double r = ModuleBase.GetCurAngleByY(ModelParams.MonoGlassX, ParCalibWorld.V_I[g_NoCamera],
                    pt2MarkArray[(int)PtType_Mono.CalibMark1], pt2MarkArray[(int)PtType_Mono.CalibMark2]);
                //此处未作镜像处理
                r = IfAdjustAngle ? r : 0;
                //将计算得到的角度，存在基准值当中，如果会做角度补偿，该值是0，预留不做角度补偿的计算
                CalibDataMngr.instance.CalibPos_L[index].DblValue4 = IfAdjustAngle ? 0 : r;
                CalibDataMngr.instance.WriteCalibResult(index);

                //发送角度结果
                //镜像角度取反
                WritePLC(2, (int)DataRegister2.CalibDeltaR, ModelParams.IfMirrior ? r : -r);
                ShowState(string.Format("相机{0}角度标定完成，r:{1}", g_NoCamera, (ModelParams.IfMirrior ? r : -r).ToString("f3")));
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        /// <summary>
        /// 标定基准值的xy
        /// </summary>
        /// <param name="index">基准值索引</param>
        /// <param name="cellName">算子序号，单目第二次拍照的M直线</param>
        /// <returns></returns>
        public StateComprehensive_enum CalibStdValue(int index, string cellName)
        {
            try
            {
                //根据算子序号，获取第二次拍照的M直线算子
                var par = GetParStdByCell(cellName);
                //基准值为两次标定点位的平均值
                par.X_BS.Value = (pt2MarkArray[(int)PtType_Mono.Verify2].DblValue1 + 
                    pt2MarkArray[(int)PtType_Mono.R180Mark2].DblValue1) / 2;
                par.Y_BS.Value = (pt2MarkArray[(int)PtType_Mono.Verify2].DblValue2 + 
                    pt2MarkArray[(int)PtType_Mono.R180Mark2].DblValue2) / 2;
                //将标定结果也保存到本地，实际计算采用基准值的话，可以避免因为误改算子基准导致问题
                CalibDataMngr.instance.CalibPos_L[index].DblValue1 = par.X_BS.Value;
                CalibDataMngr.instance.CalibPos_L[index].DblValue2 = par.Y_BS.Value;
                CalibDataMngr.instance.WriteCalibResult(index);
                //计算出当前的玻璃角度，用于进行校验交接精度损失是否在可控范围内
                double r = ModuleBase.GetCurAngleByY(ModelParams.MonoGlassX, AMP, 
                    pt2MarkArray[(int)PtType_Mono.R180Mark1], pt2MarkArray[(int)PtType_Mono.R180Mark2]);
                ShowState(string.Format("旋转180度之后，玻璃角度为:{0}", (ModelParams.IfMirrior ? r : -r).ToString("f3")));
                //计算理论旋转中心
                double r1 = ModelParams.IfMirrior ? r : -r;
                Point2D offset = new Point2D((ModelParams.IfMirrior ? -1 : 1) * ModelParams.MonoGlassX / 2 / AMP, 
                    ModelParams.MonoGlassY / 2 / AMP);
                offset = TransDelta(offset, r1, 0);

                double rcX = CalibDataMngr.instance.CalibPos_L[0].DblValue1 + 
                    (ModelParams.IfMirrior ? -1 : 1) * ModelParams.MonoGlassX / 2 / AMP;
                double rcY = CalibDataMngr.instance.CalibPos_L[0].DblValue2 - ModelParams.MonoGlassY / 2 / AMP; 
                
                ShowState(string.Format("理论旋转中心为:{0},{1}",
                    rcX.ToString(ReservDigits), rcY.ToString(ReservDigits)));
                if (!ModelParams.IfUseRealRC)
                    SetRotateCenter(strCamera5RC, new Point2D(rcX, rcY), ParComprehensive5.P_I);

                //输出基准值
                ShowState(string.Format("相机{0}标定基准位置成功", g_NoCamera));
                ShowState(string.Format(@"基准值标定完成：X：{0}/Y：{1}", par.X_BS.Value, par.Y_BS.Value));
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        /// <summary>
        /// 校验角度是否在可接受范围内，单目标定过程中，除了标定旋转中心，之前每次拍照理论上玻璃都应该是0度
        /// </summary>
        /// <param name="disMark">mark间距</param>
        /// <param name="thread">判断阈值</param>
        /// <returns></returns>
        public bool VerifyAngleCom(double disMark, double thread)
        {
            try
            {
                double r = ModuleBase.GetCurAngleByY(disMark, AMP,
                    pt2MarkArray[(int)PtType_Mono.Verify1], pt2MarkArray[(int)PtType_Mono.Verify2]);
                ShowState(string.Format("角度补正后，当前角度为:{0},阈值{1}", (ModelParams.IfMirrior ? r : -r).ToString(ReservDigits), thread));

                if (Math.Abs(r) > thread)
                    return false;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 标定旋转中心
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="baseParComprehensive"></param>
        /// <returns></returns>
        public bool CalibRC(string cellName, BaseParComprehensive baseParComprehensive)
        {
            

            ShowState(string.Format("开始计算旋转中心"));
            if (ModelParams.IfUseRealRC && !CalibRotateCenter(cellName, pt2MarkArray[(int)PtType_Mono.R180Mark2],
                pt2MarkArray[(int)PtType_Mono.RcMark2], -ModelParams.RCCalibAngle, baseParComprehensive))
            {
                ShowState("旋转中心计算失败");
                return false;
            }

            //验证计算旋转中心时PLC控制玻璃旋转的角度是否与设定的计算角度一致
            if (!VerifyRotateCenter(pt2MarkArray[(int)PtType_Mono.R180Mark1],
                pt2MarkArray[(int)PtType_Mono.R180Mark2],
                pt2MarkArray[(int)PtType_Mono.RcMark1],
                pt2MarkArray[(int)PtType_Mono.RcMark2], -ModelParams.RCCalibAngle, 0.2))
            {
                ShowAlarm("旋转中心角度校验失败，标定结果可能存在偏差，请人工确认");               
            }

            ShowState(string.Format("相机{0}标定结束", g_NoCamera));
            return true;
        }
        #endregion

        /// <summary>
        /// 保存对应单元格的匹配结果
        /// </summary>
        /// <param name="index">当前坐标对应的数组索引</param>
        /// <param name="cellName">M直线算子序号</param>
        /// <param name="pos">拍照位置</param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public bool SaveMatchResult(int index, string cellName, Pos_enum pos, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, pos, out htResult);
                ResultCrossLines result = htResult[cellName] as ResultCrossLines;

                if (!DealTypeResult(result))
                    return false;
                //保存当前匹配结果
                pt2MarkArray[index].DblValue1 = result.X;
                pt2MarkArray[index].DblValue2 = result.Y;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 计算偏差
        /// </summary>
        /// <param name="mark1">单目定位中的角1</param>
        /// <param name="mark2">单目定位中的角2，即用于标定以及计算偏差的角</param>
        /// <param name="index">基准值索引</param>
        /// <param name="baseParComprehensive"></param>
        /// <param name="pt3Result"></param>
        /// <returns></returns>
        public bool Calc(Point2D mark1, Point2D mark2, int index, BaseParComprehensive baseParComprehensive, out double[] pt3Result)
        {
            //return MonocularLation.CalcDeviationY(mark1, mark2, AMP,
            //    ModelParams.MonoGlassX, strCamera5RC, index, baseParComprehensive, out pt3Result);
            double r;
            if (!ModelParams.IfMirrior)
                r = ModuleBase.GetCurAngleByY(ModelParams.MonoGlassX, AMP, mark1, mark2);
            else
                r = ModuleBase.GetCurAngleByY(ModelParams.MonoGlassX, AMP, mark2, mark1);
            return MonocularLation.CalcDeviationY(mark2, r, AMP,
                strCamera5RC, index, baseParComprehensive, out pt3Result);
        }
        #endregion
    }
}
