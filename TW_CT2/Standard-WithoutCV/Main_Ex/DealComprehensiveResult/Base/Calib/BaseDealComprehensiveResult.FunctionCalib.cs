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
using DealAlgorithm;
using System.IO;
using DealConfigFile;
using System.Windows;
using DealCalibrate;


namespace Main_EX
{
    /// <summary>
    ///校准的触发响应函数 20181111-zx
    /// </summary>
    partial class BaseDealComprehensiveResult
    {
        #region 定义
        //相机1标定完成
        public static bool blCamera1CalibFinish = false;
        public static StateComprehensive_enum Camera1CalibResult_e = StateComprehensive_enum.Null;
        //相机2标定完成
        public static bool blCamera2CalibFinish = false;
        public static StateComprehensive_enum Camera2CalibResult_e = StateComprehensive_enum.Null;
        //相机3标定完成
        public static bool blCamera3CalibFinish = false;
        public static StateComprehensive_enum Camera3CalibResult_e = StateComprehensive_enum.Null;
        //相机4标定完成
        public static bool blCamera4CalibFinish = false;
        public static StateComprehensive_enum Camera4CalibResult_e = StateComprehensive_enum.Null;
        //相机5标定完成
        public static bool blCamera5CalibFinish = false;
        public static StateComprehensive_enum Camera5CalibResult_e = StateComprehensive_enum.Null;
        //相机6标定完成
        public static bool blCamera6CalibFinish = false;
        public static StateComprehensive_enum Camera6CalibResult_e = StateComprehensive_enum.Null;
        //相机7标定完成
        public static bool blCamera7CalibFinish = false;
        public static StateComprehensive_enum Camera7CalibResult_e = StateComprehensive_enum.Null;
        //相机8标定完成
        public static bool blCamera8CalibFinish = false;
        public static StateComprehensive_enum Camera8CalibResult_e = StateComprehensive_enum.Null;
        #endregion 定义

        /// <summary>
        /// 接收触发信号，计算校准
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public StateComprehensive_enum DealComprehensiveResultFun_Calib(TriggerSource_enum trigerSource_e, int i, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                if (ParStateSoft.StateMachine_e != StateMachine_enum.Calib)
                {
                    if (WinMsgBox.ShowMsgBox("软件非校准模式，不允许进行校准触发，是否切换到校准模式"))
                    {
                        //校准模式
                        ParStateSoft.StateMachine_e = StateMachine_enum.Calib;
                        if (Fun_SoftState != null)
                        {
                            Fun_SoftState();
                        }
                    }
                    return StateComprehensive_enum.False;
                }
                StateComprehensive_enum stateComprehensive_e = StateComprehensive_enum.False;

                int pos = i / 1000;//提取算子的相对序号，不是拍照位置
                int type = int.Parse(i.ToString().Substring(1, 1));//标定类型
                int index = int.Parse(i.ToString().Substring(2, 2));//序号

                TypeCalib_enum TypeCalib_e = (TypeCalib_enum)type;

                switch (TypeCalib_e)
                {
                    //相机校准（标定板）
                    case TypeCalib_enum.CalibCamera:
                        ShowState("相机校准");
                        stateComprehensive_e = DealComprehensiveResultFun_CalibCamera(trigerSource_e, pos, index, out htResult);
                        break;

                    //旋转中心校准
                    case TypeCalib_enum.CalibRotate:
                        ShowState("旋转中心校准");
                        stateComprehensive_e = DealComprehensiveResultFun_CalibRotate(trigerSource_e, pos, index, out htResult);
                        break;

                    //轴校准
                    case TypeCalib_enum.CalibAxis:
                        ShowState("轴校准");
                        stateComprehensive_e = DealComprehensiveResultFun_CalibAxis(trigerSource_e, pos, index, out htResult);
                        break;

                    //投影校准
                    case TypeCalib_enum.CalibAffineCamera:
                        ShowState("相机投影校准");
                        stateComprehensive_e = DealComprehensiveResultFun_CalibAffineCamera(trigerSource_e, pos, index, out htResult);
                        break;

                    //多目校准
                    case TypeCalib_enum.CalibMult:
                        ShowState("多目校准");
                        stateComprehensive_e = DealComprehensiveResultFun_CalibMult(trigerSource_e, pos, index, out htResult);
                        break;

                    //校准
                    case TypeCalib_enum.CalibHandEye:
                        ShowState("手眼校准");
                        stateComprehensive_e = DealComprehensiveResultFun_CalibHandEye(trigerSource_e, pos, index, out htResult);
                        break;
                }

                switch (g_NoCamera)
                {
                    case 1:
                        blCamera1CalibFinish = true;
                        Camera1CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam12_A2A2();
                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[6]);
                        break;
                    case 2:
                        blCamera2CalibFinish = true;
                        Camera2CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam12_A2A2();
                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[7]);
                        break;
                    case 3:
                        blCamera3CalibFinish = true;
                        Camera3CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam34_A2A2();

                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[8]);
                        break;
                    case 4:
                        blCamera4CalibFinish = true;
                        Camera4CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam34_A2A2();

                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[9]);
                        break;
                    case 5:
                        blCamera5CalibFinish = true;
                        Camera5CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam56_A2A2();
                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[10]);

                        break;
                    case 6:
                        blCamera6CalibFinish = true;
                        Camera6CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam56_A2A2();
                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[11]);
                        break;
                    case 7:
                        blCamera7CalibFinish = true;
                        Camera7CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam78_A2A2();
                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[12]);
                        break;
                    case 8:
                        blCamera8CalibFinish = true;
                        Camera8CalibResult_e = stateComprehensive_e;
                        WritePlcCalibResult_Cam78_A2A2();
                        LogicPLC.L_I.ClearPLC(RegMonitor.R_I[13]);
                        break;

                }

                return stateComprehensive_e;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        #region 写入目标和对象标定结果
        //相机12标定结果
        public void WritePlcCalibResult_Cam12_A2A2()
        {
            try
            {
                if (blCamera1CalibFinish && blCamera2CalibFinish)
                {
                    blCamera1CalibFinish = false;
                    blCamera2CalibFinish = false;
                    if (Camera1CalibResult_e == StateComprehensive_enum.True && Camera2CalibResult_e == StateComprehensive_enum.True)//只有相机12都标定成功，才是成功
                    {

                        LogicPLC.L_I.WriteRegData1(5, 1);
                    }
                    else
                    {
                        if (ParSetWork.P_I.WorkSelect_L[0].BlSelect)//标定结果默认OK
                        {
                            LogicPLC.L_I.WriteRegData1(5, 1);
                        }
                        else
                        {
                            LogicPLC.L_I.WriteRegData1(5, 2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //相机34标定结果
        public void WritePlcCalibResult_Cam34_A2A2()
        {
            try
            {
                if (blCamera3CalibFinish && blCamera4CalibFinish)
                {
                    blCamera3CalibFinish = false;
                    blCamera4CalibFinish = false;
                    if (Camera3CalibResult_e == StateComprehensive_enum.True && Camera4CalibResult_e == StateComprehensive_enum.True)//只有相机34都标定成功，才是成功
                    {
                        LogicPLC.L_I.WriteRegData1(6, 1);
                    }
                    else
                    {
                        if (ParSetWork.P_I.WorkSelect_L[0].BlSelect)//标定结果默认OK
                        {
                            LogicPLC.L_I.WriteRegData1(6, 1);
                        }
                        else
                        {
                            LogicPLC.L_I.WriteRegData1(6, 2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        //相机56标定结果
        public void WritePlcCalibResult_Cam56_A2A2()
        {
            try
            {
                if (blCamera5CalibFinish && blCamera6CalibFinish)
                {
                    blCamera5CalibFinish = false;
                    blCamera6CalibFinish = false;
                    if (Camera5CalibResult_e == StateComprehensive_enum.True && Camera6CalibResult_e == StateComprehensive_enum.True)//只有相机56都标定成功，才是成功
                    {
                        LogicPLC.L_I.WriteRegData1(7, 1);
                    }
                    else
                    {
                        if (ParSetWork.P_I.WorkSelect_L[0].BlSelect)//标定结果默认OK
                        {
                            LogicPLC.L_I.WriteRegData1(7, 1);
                        }
                        else
                        {
                            LogicPLC.L_I.WriteRegData1(7, 2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //相机78标定结果
        public void WritePlcCalibResult_Cam78_A2A2()
        {
            try
            {
                if (blCamera7CalibFinish && blCamera8CalibFinish)
                {
                    blCamera7CalibFinish = false;
                    blCamera8CalibFinish = false;
                    if (Camera7CalibResult_e == StateComprehensive_enum.True && Camera8CalibResult_e == StateComprehensive_enum.True)//只有相机78都标定成功，才是成功
                    {
                        LogicPLC.L_I.WriteRegData1(8, 1);
                    }
                    else
                    {
                        if (ParSetWork.P_I.WorkSelect_L[0].BlSelect)//标定结果默认OK
                        {
                            LogicPLC.L_I.WriteRegData1(8, 1);
                        }
                        else
                        {
                            LogicPLC.L_I.WriteRegData1(8, 2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 写入目标和对象标定结果


        #region 获取指定相机中校准算子的引用值列表
        /// <summary>
        /// 获取指定相机中校准算子的引用值列表，默认第一个算子
        /// </summary>
        /// <param name="noCamera_e">相机序号</param>
        /// <param name="nameType">算子名称:</param>
        /// 相机校准,相机投影校准,手眼校准,多目校准,旋转中心变换,坐标系变换,世界坐标变换,轴坐标校准
        /// <returns></returns>

        public static List<Point2D> GetCalibCellRefValue2(NoCamera_enum noCamera_e, string nameType)
        {
            try
            {
                return GetCalibCellRefValue2(noCamera_e, 1, nameType);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Main_EX", ex);
                return null;
            }
        }

        public static List<Point3D> GetCalibCellRefValue3(NoCamera_enum noCamera_e, string nameType)
        {
            try
            {
                return GetCalibCellRefValue3(noCamera_e, 1, nameType);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Main_EX", ex);
                return null;
            }
        }


        /// <summary>
        /// 获取指定相机中校准算子的引用值列表，
        /// </summary>
        /// <param name="noCamera_e">相机序号</param>
        /// <param name="pos">算子序号</param>
        /// <param name="nameType">算子名称</param>
        /// <returns></returns>
        public static List<Point3D> GetCalibCellRefValue3(NoCamera_enum noCamera_e, int pos, string nameType)
        {
            List<Point3D> p_L = new List<Point3D>();
            try
            {
                BaseParComprehensive par = GetParComphensive(noCamera_e);

                //获取对应算子的所有单元格名称
                List<string> nameCell_L = par.GetAllNameCellByType(nameType);
                if (nameCell_L.Count < pos)
                {
                    return null;
                }
                string nameCell = nameCell_L[pos - 1];

                //校准基类
                BaseParCalibrate baseParCalibrate = par.GetCellParCalibrate(nameCell);
                List<ResultforCalib> resultforCalib_L = baseParCalibrate.g_ParGetResultFromCell.ResultforCalib_L;
                if (nameType == "旋转中心变换")
                {
                    resultforCalib_L = ((ParCalibRotate)baseParCalibrate).g_ParGetResultFromCellForRC.ResultforCalib_L;
                }
                for (int i = 0; i < resultforCalib_L.Count; i++)
                {
                    if (resultforCalib_L[i].XResult * resultforCalib_L[i].YResult != 0)
                    {
                        p_L.Add(new Point3D(resultforCalib_L[i].XResult, resultforCalib_L[i].YResult, resultforCalib_L[i].RResult));
                    }
                }
                return p_L;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Main_EX", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定相机中校准算子的引用值列表，
        /// </summary>
        /// <param name="noCamera_e">相机序号</param>
        /// <param name="pos">算子序号</param>
        /// <param name="nameType">算子名称</param>
        /// <returns></returns>
        public static List<Point2D> GetCalibCellRefValue2(NoCamera_enum noCamera_e, int pos, string nameType)
        {
            List<Point2D> p_L = new List<Point2D>();
            try
            {
                BaseParComprehensive par = GetParComphensive(noCamera_e);

                //获取对应算子的所有单元格名称
                List<string> nameCell_L = par.GetAllNameCellByType(nameType);
                if (nameCell_L.Count < pos)
                {
                    return null;
                }
                string nameCell = nameCell_L[pos - 1];

                //校准基类
                BaseParCalibrate baseParCalibrate = par.GetCellParCalibrate(nameCell);
                List<ResultforCalib> resultforCalib_L = baseParCalibrate.g_ParGetResultFromCell.ResultforCalib_L;
                if (nameType == "旋转中心变换")
                {
                    resultforCalib_L = ((ParCalibRotate)baseParCalibrate).g_ParGetResultFromCellForRC.ResultforCalib_L;
                }
                for (int i = 0; i < resultforCalib_L.Count; i++)
                {
                    if (resultforCalib_L[i].XResult * resultforCalib_L[i].YResult != 0)
                    {
                        p_L.Add(new Point2D(resultforCalib_L[i].XResult, resultforCalib_L[i].YResult));
                    }
                }
                return p_L;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Main_EX", ex);
                return null;
            }
        }
        /// <summary>
        /// 获取相机综合设置参数顶层类
        /// </summary>
        /// <param name="noCamera_e"></param>
        /// <returns></returns>
        public static BaseParComprehensive GetParComphensive(NoCamera_enum noCamera_e)
        {
            try
            {
                BaseParComprehensive par = ParComprehensive1.P_I;
                switch (noCamera_e)
                {
                    case NoCamera_enum.Camera1:
                        par = ParComprehensive1.P_I;
                        break;
                    case NoCamera_enum.Camera2:
                        par = ParComprehensive2.P_I;
                        break;
                    case NoCamera_enum.Camera3:
                        par = ParComprehensive3.P_I;
                        break;
                    case NoCamera_enum.Camera4:
                        par = ParComprehensive4.P_I;
                        break;
                    case NoCamera_enum.Camera5:
                        par = ParComprehensive5.P_I;
                        break;
                    case NoCamera_enum.Camera6:
                        par = ParComprehensive6.P_I;
                        break;
                    case NoCamera_enum.Camera7:
                        par = ParComprehensive7.P_I;
                        break;
                    case NoCamera_enum.Camera8:
                        par = ParComprehensive8.P_I;
                        break;
                }

                
                return par;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion 获取指定相机中校准算子的引用值列表

        #region 保存基准图片
        /// <summary>
        /// 保存基准图片
        /// </summary>
        protected void SaveStdImage(ParAlgorithm par)
        {
            try
            {
                string path = ComValue.c_PathPar + ComConfigPar.C_I.NameModel + "\\Camera" + par.NoCamera.ToString() + "\\" + par.NameCell + "Std.jpg 100";
                g_UCDisplayCamera.GrabAndSaveImage(path);

                path = ParPathRoot.PathRoot + "Store\\GlobalPar" + "\\Camera" + par.NoCamera.ToString() + "\\" + par.NameCell + "Std.jpg 100";
                if (!Directory.Exists(ParPathRoot.PathRoot + "Store\\GlobalPar" + "\\Camera" + par.NoCamera.ToString()))
                {
                    Directory.CreateDirectory(ParPathRoot.PathRoot + "Store\\GlobalPar" + "\\Camera" + par.NoCamera.ToString());
                }
                if (par.g_ParGlobal.BlGlobal)
                {
                    g_UCDisplayCamera.GrabAndSaveImage(path);
                }

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保存基准图片
        /// </summary>
        /// <param name="par"></param>
        protected void SaveStdImage(ParAlgorithm par, string nameImage)
        {
            try
            {
                string path = ComValue.c_PathPar + ComConfigPar.C_I.NameModel + "\\Camera" + par.NoCamera.ToString() + "\\" + par.NameCell + nameImage + "Std.jpg 100";
                g_UCDisplayCamera.GrabAndSaveImage(path);

                path = ParPathRoot.PathRoot + "Store\\GlobalPar" + "\\Camera" + par.NoCamera.ToString() + "\\" + par.NameCell + nameImage + "Std.jpg 100";
                if (!Directory.Exists(ParPathRoot.PathRoot + "Store\\GlobalPar" + "\\Camera" + par.NoCamera.ToString()))
                {
                    Directory.CreateDirectory(ParPathRoot.PathRoot + "Store\\GlobalPar" + "\\Camera" + par.NoCamera.ToString());
                }
                if (par.g_ParGlobal.BlGlobal)
                {
                    g_UCDisplayCamera.GrabAndSaveImage(path);
                }

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 保存基准图片
    }


    /// <summary>
    /// 校准模式
    /// </summary>
    public enum TypeCalib_enum
    {
        CalibMult = 2,//多目校准
        CalibAxis = 3,//轴校准
        CalibRotate = 4,//旋转中心校准       
        CalibHandEye = 5,//手眼校准  

        CalibAffineCamera = 7,//相机投影校准
        CalibCamera = 8,//标定板
    }
}
