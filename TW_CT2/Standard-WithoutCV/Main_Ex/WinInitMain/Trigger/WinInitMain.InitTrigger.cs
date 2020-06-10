using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealComprehensive;
using Common;
using BasicClass;
using System.Collections;
using DealDisplay;
using BasicDisplay;
using Camera;
using Main_EX;

namespace Main_EX
{
    public partial class WinInitMain
    {
        #region 定义
        BaseDealComprehensiveResult DealComprehensiveResult1 = null;
        BaseDealComprehensiveResult DealComprehensiveResult2 = null;
        BaseDealComprehensiveResult DealComprehensiveResult3 = null;
        BaseDealComprehensiveResult DealComprehensiveResult4 = null;
        BaseDealComprehensiveResult DealComprehensiveResult5 = null;
        BaseDealComprehensiveResult DealComprehensiveResult6 = null;
        BaseDealComprehensiveResult DealComprehensiveResult7 = null;
        BaseDealComprehensiveResult DealComprehensiveResult8 = null;


        protected BaseUCDisplayCamera g_UCDisplayCamera1 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera2 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera3 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera4 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera5 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera6 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera7 = null;
        protected BaseUCDisplayCamera g_UCDisplayCamera8 = null;

        //线程互斥
        protected Mutex g_MtCamera1 = new Mutex();
        protected Mutex g_MtCamera2 = new Mutex();
        protected Mutex g_MtCamera3 = new Mutex();
        protected Mutex g_MtCamera4 = new Mutex();
        protected Mutex g_MtCamera5 = new Mutex();
        protected Mutex g_MtCamera6 = new Mutex();
        protected Mutex g_MtCamera7 = new Mutex();
        protected Mutex g_MtCamera8 = new Mutex();

        #endregion 定义

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init_TrrigerDealResult()
        {

        }

        protected void Init_TrrigerDealResult(BaseDealComprehensiveResult[] baseDealComprehensiveResults)
        {
            Hashtable g_HtUCDisplayImage = new Hashtable();
            try
            {
                DealComprehensiveResult1 = baseDealComprehensiveResults[0];
                DealComprehensiveResult2 = baseDealComprehensiveResults[1];
                DealComprehensiveResult3 = baseDealComprehensiveResults[2];
                DealComprehensiveResult4 = baseDealComprehensiveResults[3];
                DealComprehensiveResult5 = baseDealComprehensiveResults[4];
                DealComprehensiveResult6 = baseDealComprehensiveResults[5];
                DealComprehensiveResult7 = baseDealComprehensiveResults[6];
                DealComprehensiveResult8 = baseDealComprehensiveResults[7];


                //按照窗体顺序
                for (int i = 0; i < ParSetDisplay.P_I.NumWinDisplayImage; i++)
                {
                    BaseParSetDisplay baseParSetDisplay = ParSetDisplay.P_I[i];

                    if (baseParSetDisplay.TypeDisplayImage.Contains("Image")
                        && !g_HtUCDisplayImage.Contains(baseParSetDisplay.TypeDisplayImage))
                    {
                        g_HtUCDisplayImage.Add(baseParSetDisplay.TypeDisplayImage, g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i]);
                    }
                    else//相机显示窗体只包含一个
                    {
                        switch (baseParSetDisplay.TypeDisplayImage_e)
                        {
                            case TypeDisplayImage_enum.Camera1:
                                g_UCDisplayCamera1 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera2:
                                g_UCDisplayCamera2 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera3:
                                g_UCDisplayCamera3 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera4:
                                g_UCDisplayCamera4 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera5:
                                g_UCDisplayCamera5 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera6:
                                g_UCDisplayCamera6 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera7:
                                g_UCDisplayCamera7 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;

                            case TypeDisplayImage_enum.Camera8:
                                g_UCDisplayCamera8 = (BaseUCDisplayCamera)g_BaseUCDisplaySum.g_BaseUCDisplayCameras[i];
                                break;
                        }
                    }
                }
                DealComprehensiveResult1.Init(g_UCDisplayCamera1, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult2.Init(g_UCDisplayCamera2, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult3.Init(g_UCDisplayCamera3, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult4.Init(g_UCDisplayCamera4, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult5.Init(g_UCDisplayCamera5, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult6.Init(g_UCDisplayCamera6, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult7.Init(g_UCDisplayCamera7, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
                DealComprehensiveResult8.Init(g_UCDisplayCamera8, g_HtUCDisplayImage, g_UCDisplayMainResult, g_UCAlarm, g_UCStateWork, ShowStateMachine);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        #region 相机1
        /// <summary>
        /// 响应触发，调用对应相机的处理方法
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera1_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera1.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult1.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 1, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera1.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        public virtual void DealComprehensive_Camera1_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera1.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult1.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 1, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
            finally
            {
                g_MtCamera1.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        public virtual void DealComprehensive_Camera1_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera1.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult1.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 1, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera1.ReleaseMutex();
            }
        }
        #endregion 相机1

        #region 相机2
        protected virtual void DealComprehensive_Camera2_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera2.WaitOne();
            try
            {
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult2.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 2, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera2.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera2_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera2.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult2.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 2, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera2.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera2_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera2.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult2.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 2, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera2.ReleaseMutex();
            }
        }
        #endregion 相机2

        #region 相机3
        protected virtual void DealComprehensive_Camera3_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera3.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult3.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 3, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera3.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera3_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera3.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult3.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 3, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera3.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera3_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera3.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult3.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 3, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera3.ReleaseMutex();
            }
        }
        #endregion 相机3

        #region 相机4
        protected virtual void DealComprehensive_Camera4_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera4.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult4.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 4, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera4.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera4_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera4.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult4.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 4, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera4.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera4_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera4.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult4.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 4, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera4.ReleaseMutex();
            }
        }
        #endregion 相机4

        #region 相机5
        protected virtual void DealComprehensive_Camera5_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera5.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult5.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 5, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera5.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera5_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera5.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult5.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 5, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera5.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera5_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera5.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult5.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 5, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera5.ReleaseMutex();
            }
        }
        #endregion 相机5

        #region 相机6
        protected virtual void DealComprehensive_Camera6_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera6.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult6.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 6, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera6.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera6_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera6.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult6.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 6, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera6.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera6_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera6.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult6.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 6, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera6.ReleaseMutex();
            }
        }
        #endregion 相机6

        #region 相机7
        protected virtual void DealComprehensive_Camera7_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera7.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult7.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 7, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera7.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera7_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera7.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult7.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 7, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera7.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera7_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera7.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult7.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 7, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera7.ReleaseMutex();
            }
        }
        #endregion 相机7

        #region 相机8
        protected virtual void DealComprehensive_Camera8_event(TriggerSource_enum trigerSource_e, int i)
        {
            g_MtCamera8.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult8.DealComprehensiveResultFun(trigerSource_e, i);
                ShowInfo(trigerSource_e, stateComprehensive_e, 8, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera8.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考值index
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera8_event(TriggerSource_enum trigerSource_e, int i, int index)
        {
            g_MtCamera8.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult8.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 8, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera8.ReleaseMutex();
            }
        }

        /// <summary>
        /// 触发拍照并传入区分参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        protected virtual void DealComprehensive_Camera8_event(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            g_MtCamera8.WaitOne();
            try
            {
                //同一流程内，第几次拍照
                StateComprehensive_enum stateComprehensive_e = DealComprehensiveResult8.DealComprehensiveResultFun(trigerSource_e, i, index);

                ShowInfo(trigerSource_e, stateComprehensive_e, 8, i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtCamera8.ReleaseMutex();
            }
        }
        #endregion 相机8

        #region 显示
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="stateComprehensive_e"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void ShowInfo(TriggerSource_enum trigerSource_e, StateComprehensive_enum stateComprehensive_e, int i, int j)
        {
            try
            {
                switch (stateComprehensive_e)
                {
                    case StateComprehensive_enum.False:
                        ShowAlarm(trigerSource_e.ToString() + "触发相机" + i.ToString() + "第" + j.ToString() + "次图像处理异常！");
                        break;

                    case StateComprehensive_enum.NGTrue:
                        ShowAlarm(trigerSource_e.ToString() + "触发相机" + i.ToString() + "第" + j.ToString() + "次图像处理异常！但不产生警报！");
                        break;

                    case StateComprehensive_enum.True:
                        ShowState(trigerSource_e.ToString() + "触发相机" + i.ToString() + "第" + j.ToString() + "次图像处理成功！");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

    }
}
