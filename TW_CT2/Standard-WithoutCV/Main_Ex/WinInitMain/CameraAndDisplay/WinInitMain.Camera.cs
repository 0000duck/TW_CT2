
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using HalconDotNet;
using DealFile;
using BasicClass;
using DealConfigFile;
using SetPar;
using DealPLC;
using DealDisplay;

namespace Main_EX
{
    partial class WinInitMain
    {
        #region 初始化
        //事件注册
        protected void LoginEvent_Camera()
        {
            //相机
            Camera1.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera1Trigger_event);
            Camera2.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera2Trigger_event);
            Camera3.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera3Trigger_event);
            Camera4.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera4Trigger_event);
            Camera5.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera5Trigger_event);
            Camera6.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera6Trigger_event);
            Camera7.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera7Trigger_event);
            Camera8.C_I.Camera_event += new TrrigerSourceAction_del(DealComprehensive_Camera8Trigger_event);
        }

        /// <summary>
        /// 相机抓取图像
        /// </summary>
        public void Init_Camera()
        {
            try
            {
                if (RegeditCamera.R_I.BlOffLineCamera)
                {
                    return;
                }
                g_BaseUCDisplaySum.InitGrabImage();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化        

        #region 相机触发模式产生的事件
        /// <summary>
        /// 相机1
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera1Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机2
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera2Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机3
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera3Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机4
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera4Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机5
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera5Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机6
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera6Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机7
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera7Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机8
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected virtual void DealComprehensive_Camera8Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 相机触发模式产生的事件

        #region 关闭
        //停止实时显示
        public void StopReal()
        {
            g_BaseUCDisplaySum.StopRealImage();
        }

        //关闭相机
        public void Close_Camera()
        {
            g_BaseUCDisplaySum.CloseCamera();
        }       
        #endregion 关闭
    }
}
