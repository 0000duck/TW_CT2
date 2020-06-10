﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DealFile;
using BasicClass;

namespace DealPLC
{
    /// <summary>
    /// 设置PLC
    /// </summary>
    partial class ParSetPLC
    {
        #region 定义
        IniFile I_I = new IniFile();
        #endregion 定义

        /// <summary>
        /// 读取参数
        /// </summary>
        public void ReadIni()
        {
            //读取PLC类型
            ReadIniTypePLC();

            //如果采用PLC通信，则读取PLC相关寄存器
            if (ParSetPLC.P_I.TypePLC_e != TypePLC_enum.Null)
            {
                ReadIniRegCycle();
                RegMonitor.R_I.ReadIni();
                RegCameraData.R_I.ReadIni();
                RegConfigPar.R_I.ReadIni();

                //数据寄存器
                RegData.R_I.ReadIni();
                RegData2.R_I.ReadIni();
                RegData3.R_I.ReadIni();
                RegData4.R_I.ReadIni();
                RegData5.R_I.ReadIni();
                RegData6.R_I.ReadIni();

                //读取Common数据
                ReadCommonReserve();
            }
        }

        #region PLC基本参数
        /// <summary>
        /// 读取参数
        /// </summary>
        public void ReadIniTypePLC()
        {
            try
            {
                string strModel = I_I.ReadIniStr("PLCModel", "Model", "Null", ParSetPLC.c_PathPLC);
                TypePLC_e = (TypePLC_enum)Enum.Parse(typeof(TypePLC_enum), strModel);

                //逻辑站号
                NoStation = I_I.ReadIniInt("PLCModel", "NoStation", 1, ParSetPLC.c_PathPLC);

                //是否读取PLC监控寄存器
                BlReadCycReg = I_I.ReadIniBl("PLCModel", "BlReadCycReg", true, ParSetPLC.c_PathPLC);

                //协议版本
                string protocol = I_I.ReadIniStr("PLCModel", "Protocol", "Full", ParSetPLC.c_PathPLC);
                TypePLCProtocol_e = (TypePLCProtocol_enum)Enum.Parse(typeof(TypePLCProtocol_enum), protocol);

                //是否单线程读取,默认单线程读取
                BlRSingleTaskCamera = I_I.ReadIniBl("PLCModel", "BlRSingleTaskCamera", true, ParSetPLC.c_PathPLC);

                //是否PL独立通信
                BlAnnotherPLC = I_I.ReadIniBl("PLCModel", "BlAnnotherPLC", false, ParSetPLC.c_PathPLC);
                BlAnnotherPLCLog = I_I.ReadIniBl("PLCModel", "BlAnnotherPLCLog", false, ParSetPLC.c_PathPLC);

                //清空触发信号
                BlClearAllTrigger = I_I.ReadIniBl("PLCModel", "BlClearAllTrigger", false, ParSetPLC.c_PathPLC);

                //读取延迟
                Delay = I_I.ReadIniInt("PLCModel", "Delay", 30, ParSetPLC.c_PathPLC);

                //IP和Port
                IP = I_I.ReadIniStr("PLCModel", "IP", "192.168.0.11", ParSetPLC.c_PathPLC);
                Port = I_I.ReadIniInt("PLCModel", "Port", 6000, ParSetPLC.c_PathPLC);

                //写入端口
                BlWritePort1 = I_I.ReadIniBl("PLCModel", "BlWritePort1", false, ParSetPLC.c_PathPLC);
                BlWritePort2 = I_I.ReadIniBl("PLCModel", "BlWritePort2", false, ParSetPLC.c_PathPLC);
                BlWritePort3 = I_I.ReadIniBl("PLCModel", "BlWritePort3", false, ParSetPLC.c_PathPLC);
                BlWritePort4 = I_I.ReadIniBl("PLCModel", "BlWritePort4", false, ParSetPLC.c_PathPLC);
                BlWritePort5 = I_I.ReadIniBl("PLCModel", "BlWritePort5", false, ParSetPLC.c_PathPLC);
                BlWritePort6 = I_I.ReadIniBl("PLCModel", "BlWritePort6", false, ParSetPLC.c_PathPLC);

                PortWrite1 = I_I.ReadIniInt("PLCModel", "PortWrite1", 6000, ParSetPLC.c_PathPLC);
                PortWrite2 = I_I.ReadIniInt("PLCModel", "PortWrite2", 6000, ParSetPLC.c_PathPLC);
                PortWrite3 = I_I.ReadIniInt("PLCModel", "PortWrite3", 6000, ParSetPLC.c_PathPLC);
                PortWrite4 = I_I.ReadIniInt("PLCModel", "PortWrite4", 6000, ParSetPLC.c_PathPLC);
                PortWrite5 = I_I.ReadIniInt("PLCModel", "PortWrite5", 6000, ParSetPLC.c_PathPLC);
                PortWrite6 = I_I.ReadIniInt("PLCModel", "PortWrite6", 6000, ParSetPLC.c_PathPLC);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion PLC基本参数

        #region 读取循环读取寄存器
        //读取Ini
        public bool ReadIniRegCycle()
        {
            bool blRight = true;
            try
            {
                //循环读取寄存器
                string str = IniFile.I_I.ReadIniStr("CycReg", "CycReg", ParSetPLC.c_PathCyc);
                RegCyc = str.Replace("\\n", "\n");
            }
            catch (Exception ex)
            {
                blRight = false;
                Log.L_I.WriteError(NameClass, ex);
            }
            return blRight;
        }
        #endregion 读取循环读取寄存器

        #region 初始化Common数据
        public void ReadCommonReserve()
        {
            try
            {
                for (int i = 0; i < RegMonitor.R_I.Annotation2.Length - 20; i++)
                {
                    ComValue.C_I.ReserveValue_L.Add(new PLCReserveValue()
                        {
                            No = i,
                            Name = "PLCValue" + i.ToString(),
                            Annotation = RegMonitor.R_I.Annotation2[i + 20],
                        });
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化Common数据
    }  

}
