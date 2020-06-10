using BasicClass;
using DealPLC;
using System;

namespace Main
{
    /// <summary>
    /// PLC的相关定义和实现都在WinInitMain类里面 20181219-zx
    /// </summary>
    partial class MainWindow
    {
        #region PLC触发响应
        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_PLCAlarm_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("设备发送报警信息!");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 物料信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void L_I_PLCMaterial_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

                ShowState("设备发送物料信息!");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 语音信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void L_I_VoiceState_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowVoice(i);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected override void LogicPLC_Inst_PLCState_event(TriggerSource_enum trrigerSource_e, int intState)
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
        /// 机器人状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected override void LogicPLC_Inst_RobotState_event(TriggerSource_enum trrigerSource_e, int intState)
        {

        }

        /// <summary>
        /// 数据超限
        /// </summary>
        /// <param name="str"></param>
        protected override void L_I_WriteDataOverFlow(string str)
        {
            ShowAlarm("PLC输出数据超出范围");

            LogicPLC.L_I.PCAlarm();
        }
        #endregion PLC触发响应

        #region PLC换型相关
        /// <summary>
        /// 换型的时候写入PLC的值
        /// </summary>
        public override void WritePLCModelPar()
        {
            try
            {

                //判断配方有没有输错
                //VerifyRecipe();

                SendTData(DataRegister1.TAngle_precise, ModelParams.T_stdPrecise, "双目定位");
                SendTData(DataRegister1.TAngle_pickAOI1, ModelParams.T_stdLeftAOI, "一号工位");
                SendTData(DataRegister1.TAngle_pickAOI2, ModelParams.T_stdMidAOI, "二号工位");
                SendTData(DataRegister1.TAngle_pickAOI3, ModelParams.T_stdRightAOI, "三号工位");
                SendTData(DataRegister1.TAngle_placePlat2, ModelParams.T_stdDownStream, "放下游平台");

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void VerifyRecipe()
        {
            //int cnt = 0;
            //bool blError = false;
            //foreach (var item in ModelParams.ElectrodeArray)
            //{
            //    if (item != 0)
            //        cnt++;
            //    if (cnt > 2)
            //    {
            //        blError = true;
            //        ShowAlarm("配方中电极宽度出错");
            //        break;
            //    }
            //}

            //if(!(ModelParams.confLayerSpacing>0))
            //{
            //    blError = true;
            //    ShowAlarm("配方中卡塞层间距错误");
            //}

            //if(blError)
            //{                
            //    LogicPLC.L_I.PCAlarm();
            //}
                
        }
        #endregion PLC换型相关
    }
}
