using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealCalibrate;
using ParComprehensive;
using DealAlgorithm;
using System.Collections;
using BasicComprehensive;
using System.Windows;
using DealComprehensive;


namespace Main_EX
{
    /// <summary>
    /// 多相机执行
    /// </summary>
    partial class BaseDealComprehensiveResult
    {
        /// <summary>
        /// 此相机算法单元结果
        /// </summary>
        /// <param name="par"></param>
        /// <param name="baseParCalibrate"></param>
        /// <param name="fun"></param>
        public void GetCellResultValue(ParGetResultFromCell par, BaseParCalibrate baseParCalibrate, HashAction fun)
        {
            try
            {
                Hashtable htResult = null;
                ParCellExeReference parCellExecuteReferenceForMult = baseParCalibrate.g_ParCellExecuteReferenceForMult;

                g_BaseDealComprehensive.DealComprehensivePosCell_ForTestRun(g_UCDisplayCamera, baseParCalibrate.g_ParCellExecuteReferenceForMult, parCellExecuteReferenceForMult.g_CellExecute_L, out htResult);//执行算法但不显示

                if (fun != null)
                {
                    fun(htResult);//刷新列表显示
                }

                ////刷新当前图像
                //g_BaseDealComprehensive.RefreshCurrImage();

                //foreach (CellReference cellReference in baseParCalibrate.g_ParCellExecuteReferenceForMult.g_CellExecute_L)
                //{
                //    g_BaseDealComprehensive.RefreshHobject(cellReference.NameCell);
                //    g_BaseDealComprehensive.RefreshShape(cellReference.NameCell);
                //    g_BaseDealComprehensive.RefreshResultInfo(cellReference.NameCell);
                //}
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 获取对应相机的执行类
        /// </summary>
        /// <param name="noCamera"></param>
        /// <returns></returns>
        BaseDealComprehensive GetBaseDealComprehensive(int noCamera)
        {
            try
            {
                BaseDealComprehensive baseDealComprehensive = null;
                switch (noCamera)
                {
                    case 1:
                        baseDealComprehensive = DealComprehensive1.D_I;
                        break;

                    case 2:
                        baseDealComprehensive = DealComprehensive2.D_I;
                        break;

                    case 3:
                        baseDealComprehensive = DealComprehensive3.D_I;
                        break;

                    case 4:
                        baseDealComprehensive = DealComprehensive4.D_I;
                        break;

                    case 5:
                        baseDealComprehensive = DealComprehensive5.D_I;
                        break;

                    case 6:
                        baseDealComprehensive = DealComprehensive6.D_I;
                        break;

                    case 7:
                        baseDealComprehensive = DealComprehensive7.D_I;
                        break;

                    case 8:
                        baseDealComprehensive = DealComprehensive8.D_I;
                        break;
                }
                return baseDealComprehensive;
            }
            catch (Exception ex)
            {
                 Log.L_I.WriteError(NameClass, ex);
                 return null;
            }
        }

        /// <summary>
        /// 相机综合设置Main_EX,获取多相机调用的类，在Main里面重写函数20181226-zx
        /// </summary>
        /// <param name="noCamera"></param>
        /// <returns></returns>
        public virtual BaseDealComprehensiveResult GetDealComprehensiveResult(int noCamera)
        {
            return null;
        }

        /// <summary>
        /// 获取哈希表结果
        /// </summary>
        /// <param name="noCamera"></param>
        /// <returns></returns>
        Hashtable GetHtResult(int noCamera)
        {
            try
            {
                Hashtable htResult = null;
                switch (noCamera)
                {
                    case 1:
                        htResult = DealComprehensive1.D_I.g_HtResult;
                        break;

                    case 2:
                        htResult = DealComprehensive2.D_I.g_HtResult;
                        break;

                    case 3:
                        htResult = DealComprehensive3.D_I.g_HtResult;
                        break;

                    case 4:
                        htResult = DealComprehensive4.D_I.g_HtResult;
                        break;

                    case 5:
                        htResult = DealComprehensive5.D_I.g_HtResult;
                        break;

                    case 6:
                        htResult = DealComprehensive6.D_I.g_HtResult;
                        break;

                    case 7:
                        htResult = DealComprehensive7.D_I.g_HtResult;
                        break;

                    case 8:
                        htResult = DealComprehensive8.D_I.g_HtResult;
                        break;
                }
                return htResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }
    }
}
