using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using DealRobot;
using Common;
using ParComprehensive;
using BasicClass;
using BasicComprehensive;
using Camera;
using System.Collections;
using DealComInterface;
using DealResult;
using DealLog;

namespace Main_EX
{
    partial class BaseDealComprehensiveResult
    {
        #region 定义
        //bool
        public static bool BlFinishPos_Cam1 = false;//相机1处理完成

        public static bool BlFinishPos1_Cam2 = false;//相机2位置1处理完成
        public static bool BlFinishPos2_Cam2 = false;//相机2位置2处理完成
        public static bool BlFinishPos3_Cam2 = false;//相机2位置3处理完成

        public static bool BlFinishPos1_Cam3 = false;//相机3位置1处理完成
        public static bool BlFinishPos2_Cam3 = false;//相机3位置2处理完成
        public static bool BlFinishPos3_Cam3 = false;//相机3位置3处理完成


        public static bool BlFinishPos_Cam4 = false;//相机4处理完成
        public static bool BlFinishPos_Cam5 = false;//相机5处理完成
        public static bool BlFinishPos_Cam6 = false;//相机6处理完成

        //Hashtable
        protected static Hashtable HtResult_Cam1 = null;//相机1的结果
        protected static Hashtable HtResult_Cam2 = null;
        protected static Hashtable HtResult_Cam3 = null;
        protected static Hashtable HtResult_Cam4 = null;
        protected static Hashtable HtResult_Cam5 = null;
        protected static Hashtable HtResult_Cam6 = null;

       
        #endregion 定义

        #region 拍照结果
        /// <summary>
        /// 结束拍照
        /// </summary>
        /// <param name="stateComprehensive_e"></param>
        public void FinishPhotoPLC(StateComprehensive_enum stateComprehensive_e)
        {
            if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.Null)
            {
                return;
            }
            int result = 2;
            if (stateComprehensive_e == StateComprehensive_enum.False)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }
            LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, result);
        }

        /// <summary>
        /// 结束拍照并发送拍照结果
        /// </summary>
        /// <param name="result"></param>

        public void FinishPhotoPLC(int result)
        {
            //if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.Null)
            //{
            //    return;
            //}

            LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, result);
        }
        #endregion 拍照结果
     
        #region 算法结果类型处理
        /// <summary>
        /// 判断算法处理结果的正确性
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool DealTypeResult(BaseResult result)
        {
            try
            {
                //如果为错误
                if (result.LevelError_e == LevelError_enum.Error)
                {
                    string level = result.LevelError_e.ToString();
                    string type = result.TypeErrorProcess_e.ToString();
                    string annotation = result.Annotation;
                    string info = string.Format("{0},类型:{1};{2}", level, type, annotation);                    

                    switch(result.TypeErrorProcess_e)
                    {
                        case TypeErrorProcess_enum.OutMemory:
                             WinError.GetWinInst().ShowError(info + ",请重新启动软件!");
                            break;

                        case TypeErrorProcess_enum.CameraImageError:
                            WinError.GetWinInst().ShowError(info + ",请重新启动软件，检测相机连接!");
                            break;

                        default:
                            g_UCAlarm.AddInfo(info);
                            break;
                    }
                    
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
        /// 搜索当前位置的错误单元格
        /// </summary>
        /// <param name="htResult">结果哈希表</param>
        /// <param name="pos">拍照位置</param>
        /// <param name="name">错误单元格</param>
        /// <returns></returns>
        protected bool GetErrorCell(Hashtable htResult, int pos,out string name)
        {
            int numError = 0;
            name = "";
            try
            {
                if (htResult == null)
                {
                    return false;
                }
                if (htResult.Keys == null)
                {
                    return false;
                }
                foreach (string key in htResult.Keys)
                {
                    if (htResult[key] == null)
                    {
                        string str = "相机" + g_NoCamera.ToString() + "处理异常:";
                        BaseParComprehensive baseParComprehensive = g_BaseParComprehensive.GetCellClass(key);
                        string annotation = baseParComprehensive.Annotation;
                        ShowAlarm(str + key + ":" + annotation);
                        numError++;
                        name += key + ",";
                    }
                    else
                    {
                        if (htResult[key] is BaseResult)
                        {
                            BaseResult result = htResult[key] as BaseResult;
                            if (result.LevelError_e == LevelError_enum.Error//单元格错误，并且位置相同
                                && result.Pos == pos
                                && !result.Type.Contains("模板"))
                            {
                                numError++;
                                string str = string.Format("相机{0}{1}处理异常:{2}", g_NoCamera.ToString(), result.NameCell + result.Type, result.Annotation);
                                ShowAlarm(str);
                            }
                            name += key + ",";
                        }
                    }
                }
                if (numError > 0)
                {
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
        /// 判断指定单元格结果
        /// </summary>
        /// <param name="htResult">哈希表结果</param>
        /// <param name="cell_L">单元格名称</param>
        /// <param name="name">错误单元格名称</param>
        /// <returns></returns>

        protected bool GetErrorCell(Hashtable htResult, List<string> cell_L, out string name)
        {
            int numError = 0;
            name = "";
            try
            {
                if (htResult == null)
                {
                    return false;
                }
                if (htResult.Keys == null)
                {
                    return false;
                }
                foreach (string key in cell_L)
                {
                    if (htResult[key] == null)
                    {
                        string str = "相机" + g_NoCamera.ToString() + "处理异常:";
                        BaseParComprehensive baseParComprehensive = g_BaseParComprehensive.GetCellClass(key);
                        string annotation = baseParComprehensive.Annotation;
                        ShowAlarm(str + key + ":" + annotation);
                        numError++;

                        name += key + ",";
                    }
                    else
                    {
                        if (htResult[key] is BaseResult)
                        {
                            BaseResult result = htResult[key] as BaseResult;
                            if (result.LevelError_e == LevelError_enum.Error//单元格错误，并且位置相同
                                && !result.Type.Contains("模板"))
                            {
                                numError++;
                                string str = string.Format("相机{0}{1}处理异常:{2}", g_NoCamera.ToString(), result.NameCell + result.Type, result.Annotation);
                                ShowAlarm(str);
                            }
                        }
                        name += key + ",";
                    }
                }
                if (numError > 0)
                {
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
        #endregion 算法结果类型处理
    }
}
