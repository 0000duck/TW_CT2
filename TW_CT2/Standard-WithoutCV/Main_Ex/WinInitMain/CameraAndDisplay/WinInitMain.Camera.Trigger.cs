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

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 定义
        bool BlExCameraTrigger1 = false;
        bool BlExCameraTrigger2 = false;
        bool BlExCameraTrigger3 = false;
        bool BlExCameraTrigger4 = false;
        #endregion 定义

        #region 相机外触发线程
        public void Init_ExCameraTrigger()
        {
            try
            {
                if (ParCamera1.P_I.BlUsingTrigger
                    && ParCamera1.P_I.TriggerSource_e != TriggerSourceCamera_enum.Software)
                {
                    BlExCameraTrigger1 = true;
                    new Task(new Action(ExCameraTrigger1)).Start();
                }
                if (ParCamera2.P_I.BlUsingTrigger
                    && ParCamera2.P_I.TriggerSource_e != TriggerSourceCamera_enum.Software)
                {
                    BlExCameraTrigger2 = true;
                    new Task(new Action(ExCameraTrigger2)).Start();
                }
                if (ParCamera3.P_I.BlUsingTrigger
                    && ParCamera3.P_I.TriggerSource_e != TriggerSourceCamera_enum.Software)
                {
                    BlExCameraTrigger3 = true;
                    new Task(new Action(ExCameraTrigger3)).Start();
                }
                if (ParCamera4.P_I.BlUsingTrigger
                    && ParCamera4.P_I.TriggerSource_e != TriggerSourceCamera_enum.Software)
                {
                    BlExCameraTrigger4 = true;
                    new Task(new Action(ExCameraTrigger4)).Start();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机1外触发
        /// </summary>
        void ExCameraTrigger1()
        {
            try
            {
                while (BlExCameraTrigger1)
                {
                    if (ParCamera1.P_I.BlUsingTrigger)//如果使用外触发
                    {
                        Thread.Sleep(20);
                        Camera_BSLSDK cb = Camera1.C_I.g_CameraAbstract as Camera_BSLSDK;
                        if (cb.CountQ > 0)//外触发队列里面有图像
                        {
                            DealComprehensive_Camera1_event(TriggerSource_enum.Camera, 1, 0);
                        }
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机1外触发
        /// </summary>
        void ExCameraTrigger2()
        {
            try
            {
                while (BlExCameraTrigger2)
                {
                    if (ParCamera2.P_I.BlUsingTrigger)//如果使用外触发
                    {
                        Thread.Sleep(20);
                        Camera_BSLSDK cb = Camera2.C_I.g_CameraAbstract as Camera_BSLSDK;
                        if (cb.CountQ > 0)//外触发队列里面有图像
                        {
                            DealComprehensive_Camera2_event(TriggerSource_enum.Camera, 1, 0);
                        }
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机1外触发
        /// </summary>
        void ExCameraTrigger3()
        {
            try
            {
                while (BlExCameraTrigger3)
                {
                    if (ParCamera3.P_I.BlUsingTrigger)//如果使用外触发
                    {
                        Thread.Sleep(20);
                        Camera_BSLSDK cb = Camera3.C_I.g_CameraAbstract as Camera_BSLSDK;
                        if (cb.CountQ > 0)//外触发队列里面有图像
                        {
                            DealComprehensive_Camera3_event(TriggerSource_enum.Camera, 1, 0);
                        }
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机1外触发
        /// </summary>
        void ExCameraTrigger4()
        {
            try
            {
                while (BlExCameraTrigger4)
                {
                    if (ParCamera4.P_I.BlUsingTrigger)//如果使用外触发
                    {
                        Thread.Sleep(20);
                        Camera_BSLSDK cb = Camera4.C_I.g_CameraAbstract as Camera_BSLSDK;
                        if (cb.CountQ > 0)//外触发队列里面有图像
                        {
                            DealComprehensive_Camera4_event(TriggerSource_enum.Camera, 1, 0);
                        }
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 相机外触发线程
	}
}
