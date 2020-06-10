using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using BasicClass;

namespace DealPLC
{
    partial class ParSetPLC
    {
        #region 循环读取寄存器
        //写入Ini
        public void WriteIniRegCycle()
        {
            //循环读取寄存器
            I_I.WriteIni("CycReg", "CycReg", RegCyc, ParSetPLC.c_PathCyc);
        }
        #endregion 循环读取寄存器

        #region PLC类型
        /// <summary>
        /// 写入参数
        /// </summary>
        /// <returns></returns>
        public bool WrtiteIniTypePLC()
        {
            try
            {
                I_I.WriteIni("PLCModel", "Model", TypePLC_e.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "BlReadCycReg", BlReadCycReg.ToString(), ParSetPLC.c_PathPLC);//是否循环读取PLC
                I_I.WriteIni("PLCModel", "Protocol", TypePLCProtocol_e.ToString(), ParSetPLC.c_PathPLC);//协议版本
                I_I.WriteIni("PLCModel", "BlRSingleTaskCamera", BlRSingleTaskCamera.ToString(), ParSetPLC.c_PathPLC);//单线程触发
                I_I.WriteIni("PLCModel", "Delay", Delay.ToString(), ParSetPLC.c_PathPLC);//循环读取延迟

                I_I.WriteIni("PLCModel", "BlAnnotherPLC", BlAnnotherPLC.ToString(), ParSetPLC.c_PathPLC);//PLC独立通信
                I_I.WriteIni("PLCModel", "BlAnnotherPLCLog", BlAnnotherPLCLog.ToString(), ParSetPLC.c_PathPLC);//PLC独立通信

                I_I.WriteIni("PLCModel", "BlClearAllTrigger", BlClearAllTrigger.ToString(), ParSetPLC.c_PathPLC);//清空触发信号

                //通用端口
                I_I.WriteIni("PLCModel", "IP", IP.ToString(), ParSetPLC.c_PathPLC);//IP
                I_I.WriteIni("PLCModel", "Port", Port.ToString(), ParSetPLC.c_PathPLC);//PLC独立通信

                //写入端口
                I_I.WriteIni("PLCModel", "BlWritePort1", BlWritePort1.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "BlWritePort2", BlWritePort2.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "BlWritePort3", BlWritePort3.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "BlWritePort4", BlWritePort4.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "BlWritePort5", BlWritePort5.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "BlWritePort6", BlWritePort6.ToString(), ParSetPLC.c_PathPLC);

                I_I.WriteIni("PLCModel", "PortWrite1", PortWrite1.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "PortWrite2", PortWrite2.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "PortWrite3", PortWrite3.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "PortWrite4", PortWrite4.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "PortWrite5", PortWrite5.ToString(), ParSetPLC.c_PathPLC);
                I_I.WriteIni("PLCModel", "PortWrite6", PortWrite6.ToString(), ParSetPLC.c_PathPLC);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion PLC类型
    }
    
}
