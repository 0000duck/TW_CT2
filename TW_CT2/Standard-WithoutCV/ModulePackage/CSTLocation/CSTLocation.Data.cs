using BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulePackage
{
    partial class CSTLocation
    {
        #region lists
        /// <summary>
        /// 龙骨单列偏差
        /// </summary>
        public static List<List<double>> KeelDev_L = new List<List<double>>();
        /// <summary>
        /// 卡塞单列偏差
        /// </summary>
        public static List<double> ColDev_L = new List<double>();
        /// <summary>
        /// 龙骨间距偏差,pt2d,dbvalue1-上龙骨，dbvalue2-下龙骨
        /// </summary>
        public static List<Point2D> KeelSpacingDev_L = new List<Point2D>();
        /// <summary>
        /// 上龙骨偏差
        /// </summary>
        public static List<Point2D> TopDev_L = new List<Point2D>();
        /// <summary>
        /// 下龙骨偏差
        /// </summary>
        public static List<Point2D> BottomDev_L = new List<Point2D>();
        /// <summary>
        /// 插栏基准位
        /// </summary>
        public static List<double> StdInsert_L = new List<double>();
        /// <summary>
        /// 卡塞每列高度偏差
        /// </summary>
        public static List<double> HeightDev_L = new List<double>();
        /// <summary>
        /// 卡塞每列左右龙骨整列高度偏差
        /// </summary>
        public static List<Point2D> KeelHeightDev_L = new List<Point2D>();
        /// <summary>
        /// 卡塞每列层间距
        /// </summary>
        public static List<double> LayerSpacing_L = new List<double>();
        /// <summary>
        /// 插栏总偏差
        /// </summary>
        public static List<List<double>> InsertDev_L = new List<List<double>>();
        #endregion

        #region regedit
        /// <summary>
        /// 标记当前在做的卡塞号
        /// </summary>
        public static int CurrentCstNo
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("CurrentCstNo"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("CurrentCstNo", value.ToString());
            }
        }
        /// <summary>
        /// 上龙骨拍照计数
        /// </summary>
        public static int TopPhotoCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("TopPhotoCount"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("TopPhotoCount", value.ToString());
            }
        }
        /// <summary>
        /// 下龙骨拍照计数
        /// </summary>
        public static int BottomPhotoCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("BottomPhotoCount"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("BottomPhotoCount", value.ToString());
            }
        }

        #region 左右横拍参数
        /// <summary>
        /// 左侧拍照偏差
        /// </summary>
        static double leftDev
        {
            get
            {
                try
                {
                    return Convert.ToDouble(ReadRegedit("leftDev"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("leftDev", value.ToString());
            }
        }
        /// <summary>
        /// 左侧拍照偏差是否有效标志位
        /// </summary>
        static bool leftDev_Enable
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ReadRegedit("leftDev_Enable"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                WriteRegedit("leftDev_Enable", value.ToString());
            }
        }
        /// <summary>
        /// 右侧拍照偏差
        /// </summary>
        static double rightDev
        {
            get
            {
                try
                {
                    return Convert.ToDouble(ReadRegedit("rightDev"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("rightDev", value.ToString());
            }
        }
        /// <summary>
        /// 右侧拍照偏差是否有效标志位
        /// </summary>
        static bool rightDev_Enable
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ReadRegedit("rightDev_Enable"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                WriteRegedit("rightDev_Enable", value.ToString());
            }
        }
        #endregion
        #endregion

        #region Path
        public static string root => ParPathRoot.PathRoot + "软件运行记录\\Custom\\CST\\";
        public static string Path_ColDev_INI => root + CurrentCstNo + "卡塞列偏差" + ".ini";
        public static string Path_KeelDev_INI => root + CurrentCstNo + "龙骨列偏差" + ".ini";
        public static string Path_InsertDev_INI => root + CurrentCstNo + "偏差补偿" + ".ini";
        public static string Path_KeelSpacingDev_INI => root + CurrentCstNo + "龙骨间距偏差" + ".ini";
        public static string Path_StdInsert_INI => root + CurrentCstNo + "插栏基准位" + ".ini";
        public static string Path_LayerSpacing_INI => root + CurrentCstNo + "层间距" + ".ini";
        public static string Path_HeightDev_INI => root + CurrentCstNo + "高度偏差" + ".ini";
        public static string Path_KeelHeightDev_INI => root + CurrentCstNo + "左右龙骨偏差" + ".ini";
        #endregion Path

        #region dataManager
        /// <summary>
        /// 该函数在单纯清空当前保存的卡塞拍照数据时调用，load本地数据前
        /// 拍照次数不清，只在新上卡塞时清
        /// </summary>
        public static void ClearCstData()
        {
            KeelDev_L.Clear();
            ColDev_L.Clear();
            TopDev_L.Clear();
            BottomDev_L.Clear();
            StdInsert_L.Clear();
            HeightDev_L.Clear();
            LayerSpacing_L.Clear();
            ShowState("清空插栏拍照数据");
        }
        /// <summary>
        /// 该函数在上新卡塞时调用
        /// </summary>
        public static void ResetCstData()
        {
            KeelDev_L.Clear();
            ColDev_L.Clear();
            TopDev_L.Clear();
            BottomDev_L.Clear();
            StdInsert_L.Clear();
            HeightDev_L.Clear();
            LayerSpacing_L.Clear();
            
            TopPhotoCount = 0;
            BottomPhotoCount = 0;
            ShowState("重置插栏拍照数据");
        }
        #endregion
    }
}
