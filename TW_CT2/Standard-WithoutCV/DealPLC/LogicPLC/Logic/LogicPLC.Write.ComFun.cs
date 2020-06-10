#define MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Common;
using BasicClass;
using DealPLC;

#if MIT
using ACTETHERLib;
using ACTMULTILib;
#endif

namespace DealPLC
{
    partial class LogicPLC
    {
        #region 通用端口
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC.WriteReg(reg, value, "WritePLC");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        bool WriteBlockReg(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 通用端口


        #region 写入端口1
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg_Write1(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC1.WriteReg(reg, value, "WritePLC1");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <param name="nameFun"></param>
        /// <returns></returns>
        bool WriteBlockReg_Write1(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC1.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg1写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 写入端口1


        #region 写入端口2
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg_Write2(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC2.WriteReg(reg, value, "WritePLC2");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg2写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <param name="nameFun"></param>
        /// <returns></returns>
        bool WriteBlockReg_Write2(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC2.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg2写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 写入端口2

        #region 写入端口3
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg_Write3(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC3.WriteReg(reg, value, "WritePLC3");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg3写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <param name="nameFun"></param>
        /// <returns></returns>
        bool WriteBlockReg_Write3(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC3.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg3写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 写入端口2

        #region 写入端口4
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg_Write4(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC4.WriteReg(reg, value, "WritePLC4");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg4写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <param name="nameFun"></param>
        /// <returns></returns>
        bool WriteBlockReg_Write4(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC4.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg4写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 写入端口4

        #region 写入端口5
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg_Write5(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC5.WriteReg(reg, value, "WritePLC5");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg5写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <param name="nameFun"></param>
        /// <returns></returns>
        bool WriteBlockReg_Write5(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC5.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg5写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 写入端口5

        #region 写入端口6
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteReg_Write6(string reg, int value)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, 1, "", value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC6.WriteReg(reg, value, "WritePLC6");
                }

                if (!blResult)
                {
                    CommunicationState_event("WriteReg6写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }


        /// <summary>
        /// 写批量寄存器
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <param name="nameFun"></param>
        /// <returns></returns>
        bool WriteBlockReg_Write6(string reg, int num, int[] value, string nameFun)
        {
            g_mtWriteBlock.WaitOne();
            try
            {
                bool blResult = false;
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    ComDataPLC c_I = new ComDataPLC(FunPLC_enum.WriteBlockReg, reg, num, nameFun, value);
                    blResult = WriteData(c_I);
                }
                else
                {
                    blResult = PortPLC_I.g_BasePortPLC6.WriteBlockReg(reg.Replace("\\n", "\n"), num, value, nameFun);
                }
                if (!blResult)
                {
                    CommunicationState_event("WriteBlockReg6写入寄存器失败:" + reg, "red");
                }
                return blResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            finally
            {
                g_mtWriteBlock.ReleaseMutex();
            }
        }
        #endregion 写入端口6
    }
}
