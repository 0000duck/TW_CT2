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
using Main_EX;

namespace Main_EX
{
    public partial class BaseDealComprehensiveResult
    {
        #region 定义
        public static bool BlResultPos_Cam1 = false;//相机1处理结果
        public static bool BlResultPos_Cam2 = false;//相机2处理结果
        public static bool BlResultPos_Cam3 = false;//相机3处理结果
        public static bool BlResultPos_Cam4 = false;//相机4处理结果
        public static bool BlResultPos_Cam5 = false;//相机5处理结果
        public static bool BlResultPos_Cam6 = false;//相机6处理结果
        public static bool BlResultPos_Cam7 = false;//相机7处理结果
        public static bool BlResultPos_Cam8 = false;//相机8处理结果

        //double

        //Int
        protected int g_NoCamera = 1;//相机序号
        protected NoCamera_enum NoCamera_e = NoCamera_enum.Camera1;//相机序号枚举

        protected Pos_enum PosNow_e = Pos_enum.Pos1;//当前位置

        //string 
        public List<RegPLC> g_regData_L = new List<RegPLC>();
        public string g_regFinishData = "";
        public string g_regClearCamera = "";//清除相机触发信号
        public string g_regFinishPhoto = "";//拍照OK或者NG

        //图像处理参数        
        public BaseParComprehensive g_BaseParComprehensive = null;
        //图像处理方法  
        public BaseDealComprehensive g_BaseDealComprehensive = null;
        public BaseDealComprehensive g_DealComprehensiveBase = null;//兼容旧版本

        //Class
        public UCResult g_UCResult = null;
        public UCDisplayMainResult g_UCDisplayMainResult = null;

        static UCAlarm uCAlarm = null;
        public static UCAlarm g_UCAlarm
        {
            get
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    return WinStateAndAlarm.GetWinInst().g_UCAlarm;
                }

                return uCAlarm;
            }
            set
            {
                uCAlarm = value;
            }
        }

        static UCStateWork uCStateWork = null;
        public static UCStateWork g_UCStateWork
        {
            get
            {
                if (WinStateAndAlarm.GetWinInst() != null)
                {
                    return WinStateAndAlarm.GetWinInst().g_UCStateWork;
                }
                return uCStateWork;
            }
            set
            {
                uCStateWork = value;
            }
        }
        public Hashtable g_HtUCDisplay = null;//显示窗体
        public BaseUCDisplayCamera g_UCDisplayCamera = null;
        public Hashtable g_HtResult = null;//算法结果类

        public Action Fun_SoftState = null;
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseDealComprehensiveResult()
        {
            NameClass = "BaseDealComprehensiveResult";
        }

        /// <summary>
        /// 初始化为Halcon窗体
        /// </summary>
        /// <param name="uICameraBase">halcon窗体界面</param>
        /// <param name="uCResult"></param>
        /// <param name="uCAlarm"></param>
        /// <param name="uCStateWork"></param>
        public virtual void Init(BaseUCDisplayCamera baseUCDisplayCamera, Hashtable htUCDisplay, UCResult uCResult, UCAlarm uCAlarm, UCStateWork uCStateWork)
        {
            try
            {
                g_UCDisplayCamera = baseUCDisplayCamera;
                g_HtUCDisplay = htUCDisplay;
                g_UCResult = uCResult;
                g_UCAlarm = uCAlarm;
                g_UCStateWork = uCStateWork;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 主界面的result
        /// </summary>
        /// <param name="baseUCDisplayCamera"></param>
        /// <param name="htUCDisplay"></param>
        /// <param name="uCResult"></param>
        /// <param name="uCAlarm"></param>
        /// <param name="uCStateWork"></param>
        //public virtual void Init(BaseUCDisplayCamera baseUCDisplayCamera, Hashtable htUCDisplay, UCDisplayMainResult uCResult, UCAlarm uCAlarm, UCStateWork uCStateWork)
        //{
        //    try
        //    {
        //        g_UCDisplayCamera = baseUCDisplayCamera;
        //        g_HtUCDisplay = htUCDisplay;
        //        g_UCDisplayMainResult = uCResult;
        //        g_UCAlarm = uCAlarm;
        //        g_UCStateWork = uCStateWork;

        //        LoginEvent();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.L_I.WriteError(NameClass, ex);
        //    }
        //}

        /// <summary>
        /// 初始化相机综合设置处理结果参数
        /// </summary>
        /// <param name="baseUCDisplayCamera"></param>
        /// <param name="htUCDisplay"></param>
        /// <param name="uCResult"></param>
        /// <param name="uCAlarm"></param>
        /// <param name="uCStateWork"></param>
        /// <param name="fun_State">切换软件状态</param>
        public virtual void Init(BaseUCDisplayCamera baseUCDisplayCamera, Hashtable htUCDisplay, UCDisplayMainResult uCResult, UCAlarm uCAlarm, UCStateWork uCStateWork, Action fun_State)
        {
            try
            {
                if (baseUCDisplayCamera == null)
                {
                    return;
                }
                g_UCDisplayCamera = baseUCDisplayCamera;
                g_BaseDealComprehensive.g_UCDisplayCamera = g_UCDisplayCamera;

                g_UCDisplayCamera.BlRefreshByTrigger = true;//20181414-zx,刷新通过触发事件
                g_UCDisplayCamera.MouseUpHalWin_event += new Action(g_UCDisplayCamera_HalWin_event);
                g_UCDisplayCamera.MouseWheelHalWin_event += new Action(g_UCDisplayCamera_HalWin_event);
                g_UCDisplayCamera.MouseMoveHalWin_event += new Action(g_UCDisplayCamera_HalWin_event);

                g_HtUCDisplay = htUCDisplay;
                g_UCDisplayMainResult = uCResult;
                g_UCAlarm = uCAlarm;
                g_UCStateWork = uCStateWork;

                Fun_SoftState = fun_State;//软件状态显示切换
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 响应外部触发
        /// <summary>
        /// 接收外部触发源的指令
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public StateComprehensive_enum DealComprehensiveResultFun(TriggerSource_enum trigerSource_e, int i)
        {
            Hashtable htResult = null;
            StateComprehensive_enum stateComprehensive_e = StateComprehensive_enum.False;
            try
            {
                //if (ParStateSoft.StateMachine_e == StateMachine_enum.Calib
                //    && i < 100)
                //{
                //    if (WinMsgBox.ShowMsgBox("校准模式，不允许触发运行，是否切换到运行模式"))
                //    {
                //        //校准模式
                //        ParStateSoft.StateMachine_e = StateMachine_enum.Auto;
                //        if (Fun_SoftState != null)
                //        {
                //            Fun_SoftState();
                //        }
                //    }

                //    return StateComprehensive_enum.False;
                //}
                switch (i)
                {
                    #region 正常拍照
                    case 1:
                        stateComprehensive_e = DealComprehensiveResultFun1(trigerSource_e, out htResult);
                        break;

                    case 2:
                        stateComprehensive_e = DealComprehensiveResultFun2(trigerSource_e, out htResult);
                        break;

                    case 3:
                        stateComprehensive_e = DealComprehensiveResultFun3(trigerSource_e, out htResult);
                        break;

                    case 4:
                        stateComprehensive_e = DealComprehensiveResultFun4(trigerSource_e, out htResult);
                        break;

                    case 5:
                        stateComprehensive_e = DealComprehensiveResultFun5(trigerSource_e, out htResult);
                        break;

                    case 6:
                        stateComprehensive_e = DealComprehensiveResultFun6(trigerSource_e, out htResult);
                        break;

                    case 7:
                        stateComprehensive_e = DealComprehensiveResultFun7(trigerSource_e, out htResult);
                        break;
                    case 8:
                        stateComprehensive_e = DealComprehensiveResultFun8(trigerSource_e, out htResult);
                        break;
                    case 9:
                        stateComprehensive_e = DealComprehensiveResultFun9(trigerSource_e, out htResult);
                        break;
                    case 10:
                        stateComprehensive_e = DealComprehensiveResultFun10(trigerSource_e, out htResult);
                        break;
                        #endregion 正常拍照
                }

                #region 校准触发
                if (i > 100)
                {
                    stateComprehensive_e = DealComprehensiveResultFun_Calib(trigerSource_e, i, out htResult);
                }
                #endregion 校准触发

                string cellError = "";
                GetErrorCell(htResult, i, out cellError);//显示错误信息
                return stateComprehensive_e;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        /// <summary>
        /// 传入参考值
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public StateComprehensive_enum DealComprehensiveResultFun(TriggerSource_enum trigerSource_e, int i, int index)
        {
            try
            {
                Hashtable htResult = null;
                StateComprehensive_enum stateComprehensive_e = StateComprehensive_enum.False;
                switch (i)
                {
                    case 1:
                        stateComprehensive_e = DealComprehensiveResultFun1(trigerSource_e, index, out htResult);
                        break;

                    case 2:
                        stateComprehensive_e = DealComprehensiveResultFun2(trigerSource_e, index, out htResult);
                        break;

                    case 3:
                        stateComprehensive_e = DealComprehensiveResultFun3(trigerSource_e, index, out htResult);
                        break;

                    case 4:
                        stateComprehensive_e = DealComprehensiveResultFun4(trigerSource_e, index, out htResult);
                        break;

                    case 5:
                        stateComprehensive_e = DealComprehensiveResultFun5(trigerSource_e, index, out htResult);
                        break;

                    case 6:
                        stateComprehensive_e = DealComprehensiveResultFun5(trigerSource_e, index, out htResult);
                        break;
                }
                string cellError = "";
                GetErrorCell(htResult, i, out cellError);//显示错误信息
                return stateComprehensive_e;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        /// <summary>
        /// 传入参考数组
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public StateComprehensive_enum DealComprehensiveResultFun(TriggerSource_enum trigerSource_e, int i, int[] index)
        {
            try
            {
                Hashtable htResult = null;
                StateComprehensive_enum stateComprehensive_e = StateComprehensive_enum.False;
                switch (i)
                {
                    case 1:
                        stateComprehensive_e = DealComprehensiveResultFun1(trigerSource_e, index, out htResult);
                        break;

                    case 2:
                        stateComprehensive_e = DealComprehensiveResultFun2(trigerSource_e, index, out htResult);
                        break;

                    case 3:
                        stateComprehensive_e = DealComprehensiveResultFun3(trigerSource_e, index, out htResult);
                        break;

                    case 4:
                        stateComprehensive_e = DealComprehensiveResultFun4(trigerSource_e, index, out htResult);
                        break;

                    case 5:
                        stateComprehensive_e = DealComprehensiveResultFun5(trigerSource_e, index, out htResult);
                        break;

                    case 6:
                        stateComprehensive_e = DealComprehensiveResultFun6(trigerSource_e, index, out htResult);
                        break;
                }

                string cellError = "";
                GetErrorCell(htResult, i, out cellError);//显示错误信息
                return stateComprehensive_e;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        #region 第1次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第1次拍照

        #region 第2次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        public virtual StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第1次拍照

        #region 第3次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第3次拍照

        #region 第4次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第4次拍照

        #region 第5次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun5(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun5(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun5(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        #endregion 第5次拍照

        #region 第6次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun6(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun6(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun6(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第6次拍照

        #region 第7次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun7(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun7(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun7(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第6次拍照

        #region 第8次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun8(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun8(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun8(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第6次拍照

        #region 第9次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun9(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun9(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun9(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第9次拍照

        #region 第10次拍照
        public virtual StateComprehensive_enum DealComprehensiveResultFun10(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun10(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }

        public virtual StateComprehensive_enum DealComprehensiveResultFun10(TriggerSource_enum trigerSource_e, int[] index, out Hashtable htResult)
        {
            htResult = null;
            return StateComprehensive_enum.False;
        }
        #endregion 第10次拍照

        #endregion 响应外部触发

        #region 显示结果Main_EX
        /// <summary>
        /// 显示结果到列表，默认相机序号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="blResult"></param>
        protected void ShowMainResult(string str, bool blResult)
        {
            try
            {
                g_UCDisplayMainResult.AddInfo(str, g_NoCamera - 1, blResult);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 显示结果到列表，传入相机序号参数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="noCamera"></param>
        /// <param name="blResult"></param>
        protected void ShowMainResult(string str, int noCamera, bool blResult)
        {
            try
            {
                g_UCDisplayMainResult.AddInfo(str, noCamera - 1, blResult);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 显示结果Main_EX

        #region 鼠标控制窗体事件响应
        /// <summary>
        /// halcon窗体事件响应
        /// </summary>
        void g_UCDisplayCamera_HalWin_event()
        {
            try
            {
                g_BaseDealComprehensive.RefreshImage_ByCurPos();//刷新图像
                g_BaseDealComprehensive.RefreshShape_ByCurPos();//刷新形状
                g_BaseDealComprehensive.RefreshHobject_MainShow_ByCurPos();//刷新对象
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 鼠标控制窗体事件响应
    }

    public class CameraResult
    {
        public const int OK = 1;
        public const int NG = 2;
    }
}
