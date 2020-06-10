using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using Main_EX;
using DealComprehensive;

namespace ModulePackage
{
    public class WastageDetection : ModuleBase
    {
        static readonly string ClassName = "WastageDetection";
        public static int[] WastageInvalidCount = new int[8];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">清晰度结果列表</param>
        /// <param name="thread">清晰度判断阈值</param>
        /// <param name="cameraNo">相机编号</param>
        /// <returns></returns>
        public static bool CalcSharpnessRatio(List<double> list, double thread, int cameraNo, 
            int id, bool ifRecord, out double[] dblResult)
        {
            dblResult = new double[3];
            try
            {
                if (list.Count != 2)
                {
                    ShowAlarm(string.Format("相机{0}清晰度ROI数量错误", cameraNo));
                    return false;
                }
                double ratio = list[0] / (list[1] == 0 ? 0.001 : list[1]);
                if (ifRecord)
                    RecordSharpnessData(id, string.Format("{0}清晰度比例：{1}", id, ratio));
                dblResult = new double[3] { list[0], list[1], ratio };
                if (list[0] < 30 && list[1] < 30)
                {
                    ShowAlarm("残材比例均小于100，视为未拍到产品");
                    return false;
                }
                if (ratio > thread)
                    ShowState(string.Format("相机{0}检测无残材，清晰度：{1},{2},比例{3}>{4}", 
                        cameraNo, list[0].ToString(ReservDigits), list[1].ToString(ReservDigits), Math.Round(ratio, 2), thread));
                else
                {
                    ShowAlarm(string.Format("相机{0}检测有残材，清晰度：{1},{2},比例{3}<{4}", 
                        cameraNo, list[0].ToString(ReservDigits), list[1].ToString(ReservDigits), Math.Round(ratio, 2), thread));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                ShowAlarm(string.Format("相机{0}残材检测失败", cameraNo));
                return false;
            }
        }

        /// <summary>
        /// 残材比例检测
        /// </summary>
        /// <param name="list">清晰度结果列表</param>
        /// <param name="thread">清晰度判断阈值</param>
        /// <param name="cameraNo">相机编号</param>
        /// <param name="defaultCamNo">默认OK的相机编号</param>
        /// <param name="singleE">是否单电极</param>
        /// <returns></returns>
        public static bool CalcSharpnessRatio(List<double> list, double thread, int cameraNo, int defaultCamNo, 
            bool singleE, int id, bool ifRecord, out double[] dblResult)
        {
            dblResult = new double[3];
            try
            {
                if (cameraNo == defaultCamNo && singleE == true)
                {
                    ShowState(string.Format("单电极产品，相机{0}默认OK", cameraNo));
                    return true;
                }

                return CalcSharpnessRatio(list, thread, cameraNo, id, ifRecord, out dblResult);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
        }

        /// <summary>
        /// 残材平台处角度计算
        /// </summary>
        /// <param name="array">配方中电极数据，上左下右-0123</param>
        /// <param name="platDir">放在平台上时，电极需要朝向的位置，上左下右-0123，最多两个朝向</param>
        /// <param name="angle">计算得到的角度</param>
        /// <param name="isSingleE">是否单电极</param>
        /// <returns></returns>
        public static bool GetWastageAngle(double[] array, int[] platDir, out double angle, bool isSingleE)
        {
            angle = 0;
            try
            {
                if (array.Length != 4)
                    return false;

                //计算方式为，把来料的电极方向和放到平台上时电极需要对应的朝向，抽象成两个数组，
                //固定其中一个数组，另一个数组的值进行位移操作，直到两个数组中有值的索引能够对应，累加得到的角度即为玻璃旋转的角度
                int platDir1 = platDir[0];
                int platDir2 = platDir[1];
                int i = 0;
                if (isSingleE)
                {
                    while (array[platDir1] == 0)
                    {
                        platDir1 = (--platDir1 + 4) % 4;
                        angle += 90;
                        if (++i > 4)
                            return false;
                    }
                }
                else
                {
                    while (array[platDir1] == 0 || array[platDir2] == 0)
                    {
                        platDir1 = (--platDir1 + 4) % 4;
                        platDir2 = (--platDir2 + 4) % 4;
                        angle += 90;
                        if (++i > 4)
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据配方电极，和残材处玻璃角度，计算残材处电极宽度
        /// </summary>
        /// <param name="array">配方中电极数据，上左下右-0123</param>
        /// <param name="index">所需要获得的，在指定角度情况下的电极索引，上左下右-0123</param>
        /// <param name="r">角度差</param>
        /// <returns></returns>
        public static double GetCurElectordeWidth(double[] array, int index, int r)
        {
            return array[(index - r / 90 + 4) % 4];
        }
    }
}
