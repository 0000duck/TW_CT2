using BasicClass;
using DealPLC;
using Main_EX;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using DealCIM;
using System.Collections.Generic;

namespace Main
{
    /// <summary>
    /// 20181219-zx,
    /// </summary>
    partial class MainWindow
    {
        #region 定义
        public Task beltScan = null;
        CancellationTokenSource cts = new CancellationTokenSource();
        #endregion 定义

        /// <summary>
        /// 保留触发1 触发读码
        /// </summary>
        protected override void LogicPLC_Inst_Reserve1_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {
                ShowState("PLC触发读取二维码!");
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeResult, 0);
                Thread.Sleep(300);
                if (ModelParams.IfPassQrCode)
                {
                    ShowState("模拟读取二维码结束!");
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeResult, 1);
                }
                else
                {
                    AUOCode.A_I.blCodeReady = false;
                    AUOCode.A_I.Write();
                    ShowState("已发送二维码拍照请求，等待二维码设备反馈结果!");
                    new Task(WaitCodeOK).Start();

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发2 卡塞切换工位
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve2_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发3  插栏完成
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve3_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发4  卡塞插满
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve4_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //TODO: 插满，过账
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发5  换班
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve5_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //换班
                ShowState("PLC触发换班，产能重置");
                RegeditMain.R_I.ID = 0;
                ProductivityReport.WriteReportIni(i);
                ProductivityReport.ClearReportData();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发6  残材抛料
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve6_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //记录残材抛料
                if (i == 1)
                    RegeditMain.R_I.WastageNG1++;
                else if (i == 2)
                    RegeditMain.R_I.WastageNG2++;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 二维码拍照
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve7_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //TODO: 二维码拍照

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 语音
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve8_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ModelParams.T_Move = ModelParams.T_Roller;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发9
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve9_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                if (i == 7)
                {
                    DealComprehensive_Camera2_event(TriggerSource_enum.PLC, 4);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发10
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve10_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ModelParams.T_PickPlat1 = ModelParams.T_Move;
                //SendTData(DataRegister1.TAngle_pickPlat1, DataRegister1.TAngleConfirm_pickPlat1, ModelParams.T_PickPlat1, "机器人去平台取片");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 保留触发11,刷RunCard
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve11_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发12
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve12_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发13 第一拍照位
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve13_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发14
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve14_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发15
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve15_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发16
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve16_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发17
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve17_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发18
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve18_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发19
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve19_event(TriggerSource_enum trrigerSource_e, int i)
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
        /// 保留触发20
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve20_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void WaitCodeOK()
        {
            int i = 0;
            while (!AUOCode.A_I.blCodeReady)
            {
                i++;
                if (i > 100)
                {
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeResult, 2);
                    ShowAlarm("二维码设备反馈超时!请检查二维码设备连接!");
                    return;
                }
                Thread.Sleep(50);
            }
        }
    }
}
