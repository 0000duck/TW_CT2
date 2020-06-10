﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealMath;
using DealImageProcess;
using BasicComprehensive;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;

namespace Main
{
    partial class DealComprehensiveResult7
    {
        #region 静态类实例
        public static DealComprehensiveResult7 D_I = new DealComprehensiveResult7();
        #endregion 静态类实例

        object locker = new object();
        bool isvalid = false;

        bool isValid
        {
            get
            {
                lock (locker) { return isvalid; }
            }
            set
            {
                lock (locker) { isvalid = value; }
            }
        }

        double X1, X2;        

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public DealComprehensiveResult7()
        {
            try
            {
                base.NameClass = "DealComprehensiveResult7";
                //图像处理参数
                base.g_BaseParComprehensive = ParComprehensive7.P_I;
                base.g_BaseDealComprehensive = DealComprehensive7.D_I;
                base.g_DealComprehensiveBase = DealComprehensive7.D_I;

                g_NoCamera = 7;
                NoCamera_e = NoCamera_enum.Camera7;

                //初始化PLC寄存器
                InitPLCReg();

                //判断并设置显示的独立线程
                InitDisplay_Task();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 初始化DealPLC寄存器
        /// </summary>
        void InitPLCReg()
        {
            try
            {
                if (ParSetPLC.P_I.TypePLC_e != TypePLC_enum.Null)//三菱DealPLC
                {
                    if (ParCameraWork.NumCamera > 6)
                    {
                        base.g_regClearCamera = ParSetPLC.P_I.regClearCamera7;
                        base.g_regFinishPhoto = ParSetPLC.P_I.regFinishPhoto_Camera7;
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataX_Camera7);
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataY_Camera7);
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataZ_Camera7);
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataR_Jamera7);
                        base.g_regFinishData = ParSetPLC.P_I.regFinsihData_Camera7;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化
    }
}
