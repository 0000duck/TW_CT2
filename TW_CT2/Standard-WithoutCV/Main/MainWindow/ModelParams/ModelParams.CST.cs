using BasicClass;
using DealConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    partial class ModelParams
    {
        #region 卡塞CST
        /// <summary>
        /// 插栏位置
        /// </summary>
        public static Point4D InsertPos
        {
            get
            {
                return Point4D.Add(stdPosInsert, new Point4D(-InsertY / 2, 0, 0, 0));
            }
        }
        /// <summary>
        /// 插栏处玻璃Y
        /// </summary>
        public static double InsertY
        {
            get
            {
                return InsertAngle % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        /// <summary>
        /// 插栏处机器人u轴角度
        /// </summary>
        public static double InsertRobotAngle
        {
            get
            {
                return InsertAngle + PickAngle;
            }
        }
        /// <summary>
        /// 玻璃插栏角度
        /// </summary>
        public static double InsertAngle
        {
            get
            {
                //if (confInsertDirection == 1)
                //    return 0;
                //else if (confInsertDirection == 2)
                //    return 180;
                //else if (confInsertDirection == 4)
                //    return -90;
                //else if (confInsertDirection == 8)
                //    return 90;
                //else
                    return 0;
            }
        }
        /// <summary>
        /// 插栏补偿1
        /// </summary>
        public static double InsertCom1
        {
            get
            {
                return InsertStdCom1 + adjInsertComX1;
            }
        }

        public static double InsertStdCom1
        {
            get
            {
                return stdInsertComX1;
            }
        }

        public static double InsertStdCom2
        {
            get
            {
                return stdInsertComX2;
            }
        }
        /// <summary>
        /// 插栏补偿2
        /// </summary>
        public static double InsertCom2
        {
            get
            {
                return InsertStdCom2 + adjInsertComX2;
            }
        }
        /// <summary>
        /// 插栏X临时补偿
        /// </summary>
        public static double InsertTempComX { get; set; }
        /// <summary>
        /// 插栏R临时补偿
        /// </summary>
        public static double InsertTempComR { get; set; }
        /// <summary>
        /// 插栏宽度检测阈值
        /// </summary>
        public static double CSTThread_DevX
        {
            get
            {
                //要有0值保护
                return stdCSTThreadX == 0 ? 5 : stdCSTThreadX;
            }
        }
        public static double CSTThread_HeightDev
        {
            get
            {
                return stdCSTThread_HeightDev == 0 ? 2 : stdCSTThread_HeightDev;
            }
        }
        /// <summary>
        /// 插栏层间距检测阈值
        /// </summary>
        public static double CSTThread_LayerSpacing
        {
            get
            {
                return stdCSTThreadInterval == 0 ? 0.2 : stdCSTThreadInterval;
            }
        }
        /// <summary>
        /// 左右龙骨高度差
        /// </summary>
        public static double CSTThread_KeelHeight
        {
            get
            {
                return 1.2;
            }
        }
        public static double CSTThread_KeelSpacing
        {
            get
            {
                return 2;
            }
        }

        public static Point2D CstStdValue
        {
            get
            {
                return new Point2D(ParStd.Value1(key_std_CstStdValue), ParStd.Value2(key_std_CstStdValue));
            }
            set
            {
                ParStd.SetValue1(key_std_CstStdValue, value.DblValue1);
                ParStd.SetValue2(key_std_CstStdValue, value.DblValue2);
            }
        }
        #endregion

        #region adj
        /// <summary>
        /// 卡塞X偏差阈值
        /// </summary>
        static double stdCSTThreadX
        {
            get
            {
                string key = key_std_cstthread;
                return ParStd.Value1(key);
            }
        }
        /// <summary>
        /// 卡塞Z偏差阈值
        /// </summary>
        static double stdCSTThread_HeightDev
        {
            get
            {
                string key = key_std_cstthread;
                return ParStd.Value2(key);
            }
        }
        /// <summary>
        /// 卡塞层间距偏差阈值
        /// </summary>
        static double stdCSTThreadInterval
        {
            get
            {
                string key = key_std_cstthread;
                return ParStd.Value3(key);
            }
        }

        /// <summary>
        /// 调整值-插栏1补偿X
        /// </summary>
        static double adjInsertComX1
        {
            get
            {
                string key = key_adj_InsertX1;
                return ParAdjust.Value1(key);
            }
        }
        /// <summary>
        /// 调整值-插栏2补偿X
        /// </summary>
        static double adjInsertComX2
        {
            get
            {
                string key = key_adj_InsertX2;
                return ParAdjust.Value1(key);
            }
        }
        #endregion

        #region std
        /// <summary>
        /// 基准值-Robot插栏基准位
        /// </summary>
        static Point4D stdPosInsert
        {
            get
            {
                return new Point4D();
            }
        }

        /// <summary>
        /// 基准值-插栏1基准补偿
        /// </summary>
        static double stdInsertComX1
        {
            get
            {
                string key = key_std_cominsert1;
                return ParStd.Value1(key);
            }
        }
        /// <summary>
        /// 基准值-插栏2基准补偿
        /// </summary>
        static double stdInsertComX2
        {
            get
            {
                string key = key_std_cominsert2;
                return ParStd.Value2(key);
            }
        }
        #endregion
    }
}
