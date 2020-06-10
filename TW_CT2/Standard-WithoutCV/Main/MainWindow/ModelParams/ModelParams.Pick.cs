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
        #region 粗定位取片pick
        /// <summary>
        /// 粗定位取片角度
        /// </summary>
        public static double PickAngle
        {
            get
            {
                return confGlassX > confGlassY ? 0 : -90;
            }
        }
        /// <summary>
        /// 粗定位取片高度
        /// </summary>
        public static double PickHeight
        {
            get
            {
                return _stdPickPos.DblValue3 + _adjPickPos.DblValue3 + ThicknessOffset;
            }
        }
        /// <summary>
        /// 粗定位取片位，发送给机器人的基准位
        /// </summary>
        //public static Point4D PickPos
        //{
        //    get
        //    {
        //        return _stdPickPos.Add(3, PickAngle);
        //    }
        //}
        /// <summary>
        /// 粗定位取片调整值
        /// </summary>
        //public static Point4D AdjPickPos
        //{
        //    get
        //    {
        //        return _adjPickPos;
        //    }
        //}
        /// <summary>
        /// 粗定位取片后偏移值
        /// </summary>
        public static Point4D AdjPickOffset
        {
            get
            {
                return UnifyAdj(_adjPickOffset);
            }
        }

        /// <summary>
        /// 中片取片行数
        /// </summary>
        public static int PickArrayRows
        {
            get
            {
                return _stdPickRows;
            }
        }
        /// <summary>
        /// 中片取片列数
        /// </summary>
        public static int PickArrayCols
        {
            get
            {
                return _stdPickCols;
            }
        }
        #endregion

        #region adj
        /// <summary>
        /// 调整值-中片取片调整
        /// </summary>
        static Point4D _adjPickPos
        {
            get
            {
                return new Point4D();
            }
        }
        /// <summary>
        /// 取片后偏移，调整值
        /// </summary>
        static Point4D _adjPickOffset
        {
            get
            {
                string key = key_adj_PickOffset;
                return new Point4D(ParAdjust.Value1(key), ParAdjust.Value2(key), ParAdjust.Value3(key), 0);
            }
        }
        #endregion

        #region std
        /// <summary>
        /// 基准值-中片基准取片位置
        /// </summary>
        static Point4D _stdPickPos
        {
            get
            {
                return new Point4D();
            }
        }

        static int _stdPickRows
        {
            get
            {
                string key = key_std_PickRowCol;
                return (int)ParStd.Value1(key);
            }
        }

        static int _stdPickCols
        {
            get
            {
                string key = key_std_PickRowCol;
                return (int)ParStd.Value2(key);
            }
        }
        #endregion
    }
}
