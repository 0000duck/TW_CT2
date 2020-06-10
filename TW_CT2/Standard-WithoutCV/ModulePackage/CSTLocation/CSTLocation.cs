using BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulePackage
{
    public partial class CSTLocation : ModuleBase
    {
        const string ClassName = "CSTLocation";

        #region 卡塞四角定位计算
        /// <summary>
        /// 计算所有龙骨的基准位，默认所有龙骨垂直，所以每列一个数据
        /// </summary>
        /// <param name="stdCSTPos">卡塞基准位</param>
        /// <param name="cstCols">龙骨列数</param>
        /// <param name="keelInterval">龙骨间距</param>
        /// <param name="keel1pos">第一列龙骨位置</param>
        /// <returns></returns>
        public static List<double> CreateKeelStdPos(double stdCSTPos, int cstCols,
            double keelInterval, double keel1pos, DirInsert_Enum dirInsert)
        {
            List<double> pLst = new List<double>();

            try
            {
                for (int i = 0; i < cstCols; i++)
                {
                    //基准位+第一列位置+1/2龙骨间距得到第一列基准插栏位，后面每列依次加一个龙骨间距即可
                    pLst.Add(stdCSTPos + (int)dirInsert * (keel1pos + (i + 0.5) * keelInterval));
                }
                return pLst;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return pLst;
        }
        /// <summary>
        /// 龙骨单列偏差，计算每一列龙骨因为倾斜导致的递进偏差
        /// </summary>
        /// <param name="keelCols">龙骨列数</param>
        /// <param name="keelRows">龙骨行数</param>
        /// <param name="listTop">上龙骨结果列表</param>
        /// <param name="listBottom">下龙骨结果列表</param>
        /// <returns></returns>
        public static List<List<double>> CalcKeelDeviation(int keelCols, int keelRows,
            List<Point2D> listTop, List<Point2D> listBottom)
        {
            List<List<double>> keelDev_L = new List<List<double>>();

            try
            {
                for (int i = 0; i < keelCols; i++)
                {
                    List<double> singleCol = new List<double>();
                    //龙骨两端的横向偏差，认为卡塞是直的，偏差线性递增
                    double offset = listBottom[i].DblValue1 - listTop[i].DblValue1;
                    //总偏差由rows-2行造成
                    double avgOffset = offset / (keelRows - 2);
                    for (int j = 0; j < keelRows; j++)
                    {
                        singleCol.Add(avgOffset * j);
                    }
                    keelDev_L.Add(singleCol);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return keelDev_L;
        }
        /// <summary>
        /// 卡塞单列偏差，计算卡塞每一列整列的横向偏差，同一列的每一层相同
        /// </summary>
        /// <param name="cstCols">卡塞列数</param>
        /// <param name="listTop">上龙骨结果列表</param>
        /// <returns></returns>
        public static List<double> CalcColDeviation(int cstCols, List<Point2D> listTop)
        {
            List<double> colDev_L = new List<double>();

            try
            {
                for (int i = 0; i < cstCols; ++i)
                {
                    //求取相对于两列龙骨的正中间，存在的偏差
                    colDev_L.Add((listTop[i].DblValue1 + listTop[i + 1].DblValue1) / 2.0);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return colDev_L;
        }
        /// <summary>
        /// 将龙骨单列偏差与卡塞单列偏差（卡塞1列由2列龙骨组成）加到一起，作为总偏差
        /// </summary>
        /// <param name="cstCols">卡塞列数</param>
        /// <param name="cstRows">卡塞行数</param>
        /// <param name="keelDev_L">龙骨单列偏差列表</param>
        /// <param name="colDev_L">卡塞单列偏差</param>
        /// <returns></returns>
        public static List<List<double>> MixDeviation(int cstCols, int cstRows,
            List<List<double>> keelDev_L, List<double> colDev_L, DirCstCamera_Enum dirPhoto,bool cstIsMirrorX)
        {
            List<List<double>> mixDev_L = new List<List<double>>();

            try
            {
                int dir = cstIsMirrorX ? -(int)dirPhoto : (int)dirPhoto;
                for (int i = 0; i < cstCols; ++i)
                {
                    List<double> lst = new List<double>();
                    for (int j = 0; j < cstRows; ++j)
                    {
                        lst.Add(dir * ((keelDev_L[i][j] + keelDev_L[i + 1][j]) / 2.0 + colDev_L[i]));
                    }
                    mixDev_L.Add(lst);
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            return mixDev_L;
        }
        /// <summary>
        /// 计算卡塞龙骨的间距是否符合规范，上下分别计算
        /// </summary>
        /// <param name="cstClos"></param>
        /// <param name="top_L"></param>
        /// <param name="bottom_L"></param>
        /// <returns></returns>
        public static List<Point2D> CalcKeelSpacingDeviation(int cstClos, List<Point2D> top_L, List<Point2D> bottom_L)
        {   
            List<Point2D> keelSpacingDev_L = new List<Point2D>();

            try
            {
                for (int i = 0; i < cstClos; ++i)
                {
                    keelSpacingDev_L.Add(new Point2D()
                    {
                        DblValue1 = top_L[i].DblValue1 - top_L[i + 1].DblValue1,
                        DblValue2 = bottom_L[i].DblValue1 - bottom_L[i + 1].DblValue1
                    });
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return keelSpacingDev_L;
        }
        /// <summary>
        /// 计算z轴补偿，第一位是卡塞单列高度补偿，第二位是单列层间距
        /// </summary>
        /// <param name="cstCols">卡塞列数</param>
        /// <param name="keelRows">龙骨行数</param>
        /// <param name="stdInterval">基准层间距</param>
        /// <param name="listTop">上龙骨结果列表</param>
        /// <param name="listBottom">下龙骨结果列表</param>
        /// <returns></returns>
        public static List<double> CalcHeightDeviation(int cstCols, List<Point2D> listTop, TypeModuleZ_Enum dirZ)
        {
            List<double> heightDev_L = new List<double>();
            try
            {
                for (int i = 0; i < cstCols; ++i)
                {
                    heightDev_L.Add((int)dirZ * (listTop[i].DblValue2 + listTop[i + 1].DblValue2) / 2);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return heightDev_L;
        }
        /// <summary>
        /// 计算左右两列龙骨的高度差，整列之间的高度差
        /// </summary>
        /// <param name="cstCols"></param>
        /// <param name="listTop"></param>
        /// <param name="listBottom"></param>
        /// <returns></returns>
        public static List<Point2D> CalcKeelHeightDeviation(int cstCols, List<Point2D> listTop, List<Point2D> listBottom)
        {
            List<Point2D> keelHeightDev_L = new List<Point2D>();

            try
            {
                for (int i = 0; i < cstCols; ++i)
                {
                    keelHeightDev_L.Add(new Point2D()
                    {
                        DblValue1 = listTop[i].DblValue2 - listTop[i + 1].DblValue2,
                        DblValue2 = listBottom[i].DblValue2 - listBottom[i + 1].DblValue2
                    });
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            return keelHeightDev_L;
        }
        /// <summary>
        /// 计算龙骨层间距
        /// </summary>
        /// <param name="cstCols"></param>
        /// <param name="keelRows"></param>
        /// <param name="stdInterval"></param>
        /// <param name="listTop"></param>
        /// <param name="listBottom"></param>
        /// <returns></returns>
        public static List<double> CalcLayerSpacing(int cstCols, int keelRows,
            double stdInterval, List<Point2D> listTop, List<Point2D> listBottom)
        {
            List<double> layerSpacing_L = new List<double>();
            try
            {
                for (int i = 0; i < cstCols; ++i)
                {
                    layerSpacing_L.Add(stdInterval + (listBottom[i].DblValue2 - listTop[i].DblValue2) / (keelRows - 2));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return layerSpacing_L;
        }
        #endregion

        #region 卡塞左右横拍
        /// <summary>
        /// 获取横拍结果，如果有效就使用，无效则不使用
        /// </summary>
        /// <param name="ratio"></param>
        /// <param name="dirPhoto"></param>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static bool GetRealTimeDev(DirCstCamera_Enum dirPhoto,bool cstIsMirrorX, out double dev)
        {
            dev = 0;
            try
            {
                if (TryGetLeftDev(out double left) && TryGetRightDev(out double right))
                {
                    int dir = cstIsMirrorX ? -(int)dirPhoto : (int)dirPhoto;
                    dev = dir * (left + right) / 2;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
        }
        /// <summary>
        /// 获取左侧偏差
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static bool TryGetLeftDev(out double dev)
        {
            dev = leftDev;
            return leftDev_Enable;
        }
        /// <summary>
        /// 获取右侧偏差
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static bool TryGetRightDev(out double dev)
        {
            dev = rightDev;
            return rightDev_Enable;
        }
        /// <summary>
        /// 设置左侧偏差值
        /// </summary>
        /// <param name="state"></param>
        public static void SetLeftDevEnable(double dev)
        {
            leftDev = dev;
            leftDev_Enable = true;
        }
        public static void SetLeftDevEnable()
        {
            leftDev_Enable = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public static void SetRightDevEnable(double dev)
        {
            rightDev = dev;
            rightDev_Enable = true;
        }
        /// <summary>
        /// 设置偏差无效
        /// </summary>
        public static void SetRightDevEnable()
        {
            rightDev_Enable = false;
        }
        #endregion
    }
}
