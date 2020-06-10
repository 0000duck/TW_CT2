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
        /// <summary>
        /// 玻璃Mark间距
        /// </summary>
        public static double DisMark
        {
            get
            {
                if (Math.Abs(confMark1X - confMark2X) < 0.001)
                {
                    return Math.Abs(confMark1Y - confMark2Y);
                }
                else
                {
                    return Math.Abs(confMark1X - confMark2X);
                }
            }
        }
        /// <summary>
        /// 两个Mark再玻璃上的位置，上1下2左3右4
        /// </summary>
        public static int MarkDirection
        {
            get
            {
                if (Math.Abs(confMark1X - confMark2X) < 0.001)
                {
                    if (confMark1X < confGlassX / 2)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (confMark1Y < confGlassY / 2)
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }
        /// <summary>
        /// 双目定位时玻璃X方向的长度
        /// </summary>
        public static double GlassX_Precise
        {
            get
            {
                if (MarkDirection<3)
                {
                    return confGlassX;
                }
                else
                {
                    return confGlassY;
                }
            }
        }
        /// <summary>
        /// 双目定位时玻璃Y方向的长度
        /// </summary>
        public static double GlassY_Precise
        {
            get
            {
                if (MarkDirection < 3)
                {
                    return confGlassY;
                }
                else
                {
                    return confGlassX;
                }
            }
        }
        /// <summary>
        /// 双目定位时左边的Mark坐标
        /// </summary>
        public static Point2D MarkL_Precise
        {
            get
            {
                if (Mark1_Precise.DblValue1<Mark2_Precise.DblValue1)
                {
                    return Mark1_Precise;
                }
                else
                {
                    return Mark2_Precise;
                }
            }
        }
        /// <summary>
        /// 双目定位时右边的Mark坐标
        /// </summary>
        public static Point2D MarkR_Precise
        {
            get
            {
                if (Mark1_Precise.DblValue1 < Mark2_Precise.DblValue1)
                {
                    return Mark2_Precise;
                }
                else
                {
                    return Mark1_Precise;
                }
            }
        }
        /// <summary>
        /// 双目定位时Mark1相对于左下角坐标
        /// </summary>
        public static Point2D Mark1_Precise
        {
            get
            {
                return MarkRotate(confMark1X, confMark1Y);
            }
        }
        /// <summary>
        /// 双目定位时Mark2相对于左下角坐标
        /// </summary>
        public static Point2D Mark2_Precise
        {
            get
            {
                return MarkRotate(confMark2X, confMark2Y);
            }
        }
        /// <summary>
        /// 玻璃旋转至Mark边朝上时Mark相对于左下角的坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point2D MarkRotate(double x, double y)
        {

            Point2D p = new Point2D(0, 0);
            switch (MarkDirection)
            {
                case 1:
                    p.DblValue1 = x;
                    p.DblValue2 = y;
                    break;
                case 2:
                    p.DblValue1 = confGlassX - x;
                    p.DblValue2 = confGlassY - y;
                    break;
                case 3:
                    p.DblValue1 = y;
                    p.DblValue2 = confGlassX - x;
                    break;
                case 4:
                    p.DblValue1 = confGlassY - y;
                    p.DblValue2 = x;
                    break;
            }
            return p;
        }
    }
}