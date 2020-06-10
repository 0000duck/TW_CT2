using BasicClass;
using DealComprehensive;
using System;
using System.Collections;
using System.Diagnostics;

namespace Main
{
    public partial class DealComprehensiveResult1 : BaseDealComprehensiveResult_Main
    {
        #region 定义
        //double            



        #endregion 定义

        #region 位置1拍照
        /// <summary>
        /// 粗定位
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos1;//当前位置
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            //sw.Restart();
            #endregion 定义
            try
            {
                return CalcPickPos(strCamera1Match1, out htResult);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置1拍照
    }
}
