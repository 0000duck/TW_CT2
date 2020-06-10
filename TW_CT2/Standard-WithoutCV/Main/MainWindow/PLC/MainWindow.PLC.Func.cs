using BasicClass;
using DealPLC;
using Main_EX;
using ModulePackage;
using System;
using System.Collections.Generic;

namespace Main
{
    partial class MainWindow
    {
        #region 定义        
        public static List<List<double>> DeltaX_L = new List<List<double>>();
        public static List<List<Point2D>> CSTPosAll_L = new List<List<Point2D>>();

        public static bool IsDataValid = false;
        public static double VisionDelta = 0;

        public Action ClearTempCom;
        #endregion

        //void SendInsertData(int num)
        //{
        //    try
        //    {
        //        if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
        //        {
        //            ShowState("空跑，默认发送插栏数据0");
        //            LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertData, 145.5);
        //            LogicPLC.L_I.WriteRegData1((int)DataRegister1.InsertDataConfirm, 1);
        //            return;
        //        }

        //        double stdCom = 0;
        //        double com = 0;
        //        if (CSTLocation.CurrentCstNo == 1)
        //        {
        //            stdCom = ModelParams.InsertStdCom1;
        //            com = ModelParams.InsertCom1 + ModelParams.InsertTempComX;
        //        }
        //        else if (CSTLocation.CurrentCstNo == 2)
        //        {
        //            stdCom = ModelParams.InsertCom2;
        //            com = ModelParams.InsertCom2 + ModelParams.InsertTempComX;
        //        }
        //        ShowState(string.Format("插栏基准补偿：{0}，总补偿：{1}", stdCom, com));
        //        LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertStdCom, stdCom);

        //        ShowState(string.Format("即将发送卡塞{0}插栏坐标", CSTLocation.CurrentCstNo));
        //        //int numInsert = Convert.ToInt32(
        //        //    LogicPLC.L_I.ReadRegData1((int)DataRegister1.CurrentInsertSum));
        //        int numInsert = num;
        //        int intCol = numInsert / ModelParams.confCSTRow;
        //        int intRow = numInsert % ModelParams.confCSTRow;

        //        #region 选择偏差
        //        double curDev = CSTLocation.InsertDev_L[intCol][intRow];
        //        double realTimeDev = 0;
        //        if (CSTLocation.GetRealTimeDev(ModelParams.DirPhoto,ModelParams.CstIsMirrorX, out realTimeDev))
        //        {
        //            ShowState(string.Format("理论偏差:{0},实时偏差:{1}", curDev, realTimeDev));
        //            if (Math.Abs(curDev - realTimeDev) < 1)
        //            {
        //                curDev = realTimeDev;
        //                ShowState("实时偏差有效");
        //            }
        //            else
        //            {
        //                ShowState("实时偏差与理论偏差过大，不启用");
        //            }
        //        }
        //        else
        //        {
        //            ShowState(string.Format("实时偏差无效"));
        //        }
        //        #endregion

        //        //读取插栏坐标X方向
        //        double insertPos = CSTLocation.StdInsert_L[intCol] + curDev + com;
        //        ShowState(string.Format("计算偏差:{0}，补偿{1}",
        //            curDev.ToString("f3"), (com).ToString("f3")));
        //        ShowState(string.Format("当前插栏位置：第{0}列,第{1}行", intCol + 1, intRow + 1));
        //        LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertData, insertPos);
        //        LogicPLC.L_I.WriteRegData1((int)DataRegister1.InsertDataConfirm, 1);
        //        ShowState("已发送插栏坐标X：" + insertPos.ToString("f3"));
        //        if (intRow == 0)
        //        {
        //            ClearTempCom?.Invoke();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowAlarm("发送插栏数据异常");
        //        Log.L_I.WriteError(NameClass, ex);
        //    }
        //}

        //void SendInsertData()
        //{
        //    try
        //    {
        //        if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
        //        {
        //            ShowState("空跑，默认发送插栏数据0");
        //            LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertData, 145.5);
        //            LogicPLC.L_I.WriteRegData1((int)DataRegister1.InsertDataConfirm, 1);
        //            return;
        //        }

        //        double stdCom = 0;
        //        double com = 0;
        //        if (CSTLocation.CurrentCstNo == 1)
        //        {
        //            stdCom = ModelParams.InsertStdCom1;
        //            com = ModelParams.InsertCom1 + ModelParams.InsertTempComX;
        //        }
        //        else if (CSTLocation.CurrentCstNo == 2)
        //        {
        //            stdCom = ModelParams.InsertCom2;
        //            com = ModelParams.InsertCom2 + ModelParams.InsertTempComX;
        //        }
        //        ShowState(string.Format("插栏基准补偿：{0}，总补偿：{1}", stdCom, com));
        //        LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertStdCom, stdCom);

        //        ShowState(string.Format("即将发送卡塞{0}插栏坐标", CSTLocation.CurrentCstNo));
        //        int numInsert = Convert.ToInt32(
        //            LogicPLC.L_I.ReadRegData1((int)DataRegister1.CurrentInsertSum));
        //        int intCol = numInsert / ModelParams.confCSTRow;
        //        int intRow = numInsert % ModelParams.confCSTRow;

        //        #region 选择偏差
        //        double curDev = CSTLocation.InsertDev_L[intCol][intRow];
        //        double realTimeDev = 0;
        //        if (CSTLocation.GetRealTimeDev(ModelParams.DirPhoto, ModelParams.CstIsMirrorX, out realTimeDev))
        //        {
        //            ShowState(string.Format("理论偏差:{0},实时偏差:{1}", curDev, realTimeDev));
        //            if (Math.Abs(curDev - realTimeDev) < 1)
        //            {
        //                curDev = realTimeDev;
        //                ShowState("实时偏差有效");
        //            }
        //            else
        //            {
        //                ShowState("实时偏差与理论偏差过大，不启用");
        //            }
        //        }
        //        else
        //        {
        //            ShowState(string.Format("实时偏差无效"));
        //        }
        //        #endregion

        //        //读取插栏坐标X方向
        //        double insertPos = CSTLocation.StdInsert_L[intCol] + curDev + com;
        //        ShowState(string.Format("计算偏差:{0}，补偿{1}",
        //            curDev.ToString("f3"), (com).ToString("f3")));
        //        ShowState(string.Format("当前插栏位置：第{0}列,第{1}行", intCol + 1, intRow + 1));
        //        LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertData, insertPos);
        //        LogicPLC.L_I.WriteRegData1((int)DataRegister1.InsertDataConfirm, 1);
        //        ShowState("已发送插栏坐标X：" + insertPos.ToString("f3"));
        //        if (intRow == 0)
        //        {
        //            ClearTempCom?.Invoke();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowAlarm("发送插栏数据异常");
        //        Log.L_I.WriteError(NameClass, ex);
        //    }
        //}

        //void ChangCSTSationNum(int id)
        //{
        //    ShowState(string.Format("切换卡塞，当前卡塞{0}", id));
        //    //ParInsertRegData.P_I.CurStationNo = id;
        //    CSTLocation.CurrentCstNo = id;
        //    CSTLocation.ResetCstData();
        //}

        //void ChangeCol()
        //{
        //    CSTLocation.SetLeftDevEnable();
        //    CSTLocation.SetRightDevEnable();
        //    ClearTempCom?.Invoke();
        //}
    }
}
