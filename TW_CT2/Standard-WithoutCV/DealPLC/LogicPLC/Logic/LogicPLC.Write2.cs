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

namespace DealPLC
{
    /// <summary>
    /// 
    /// </summary>
    partial class LogicPLC
    {
        #region 清除寄存器
        /// <summary>
        /// 清除PLC寄存器
        /// </summary>
        public void ClearPLC_Write2(RegPLC regPLC)
        {
            g_mtClear.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return;
                }
                ClearPLC_Write2(regPLC.NameReg.Replace("\\n", "\n"));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_mtClear.ReleaseMutex();
            }
        }

        /// <summary>
        /// 清除PLC
        /// </summary>
        /// <param name="strReg"></param>
        void ClearPLC_Write2(string strReg)
        {
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return;
                }
                string[] str = strReg.Split('\n');
                int[] intValue = new int[str.Length - 1];
                bool blResult = WriteBlockReg_Write2(strReg, str.Length - 1, intValue, "ClearPLC_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("ClearPLC_Write2清零寄存器失败:" + strReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 清除寄存器

        #region 拍照完成以及结果
        /// <summary>
        /// 拍照完成以及结果
        /// </summary>
        /// <param name="strReg"></param>
        /// <param name="intResult"></param>
        public void FinishPhoto_Write2(string strReg, int intResult)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return;
                }
                string[] str = null;
                int[] intValue = null;
                if (ParSetPLC.P_I.TypePLCProtocol_e == TypePLCProtocol_enum.FullNew)
                {
                    intValue = new int[2];
                    str = strReg.Split('\n');

                    intValue[0] = 0;//拍照完成清零
                    intValue[1] = intResult;//拍照结果
                }
                else
                {
                    intValue = new int[4];
                    str = strReg.Split('\n');

                    intValue[0] = 0;//拍照完成清零
                    intValue[2] = intResult;//拍照结果
                }

                bool blResult = false;
                if (ParSetPLC.P_I.TypePLCProtocol_e == TypePLCProtocol_enum.FullNew)
                {
                    blResult = WriteReg_Write2(str[0], 0);//清空拍照和结果寄存器是不连续的
                    WriteReg_Write2(str[1], intResult);
                }
                else
                {
                    blResult = WriteBlockReg_Write2(strReg, str.Length - 1, intValue, "FinishPhoto_Write2");
                }
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("FinishPhoto_Write2写入寄存器失败:" + strReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }
        #endregion 拍照完成以及结果

        #region 写入计算数据和数据传输确认信号
        /// <summary>
        /// 写入相机寄存器
        /// </summary>
        /// <param name="Reg_L"></param>
        /// <param name="dblData"></param>
        /// <param name="strFinishDataReg"></param>
        /// <param name="intFinish"></param>
        public void WriteCalData_Write2(List<RegPLC> Reg_L, double[] dblData, string strFinishDataReg, int intFinish)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return;
                }

                //获取寄存器值
                string strReg = GetRegName(Reg_L);
                strReg += strFinishDataReg;

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData(Reg_L, dblData, out realData))
                {
                    return;
                }

                int[] intValue = new int[10];
                intValue[0] = ConvertWriteData(realData[0])[0];//X
                intValue[1] = ConvertWriteData(realData[0])[1];
                intValue[2] = ConvertWriteData(realData[1])[0];//Y
                intValue[3] = ConvertWriteData(realData[1])[1];
                intValue[4] = ConvertWriteData(realData[2])[0];//Z
                intValue[5] = ConvertWriteData(realData[2])[1];
                intValue[6] = ConvertWriteData(realData[3])[0];//R
                intValue[7] = ConvertWriteData(realData[3])[1];
                intValue[8] = intFinish;
                intValue[9] = 0;

                bool blResult = WriteBlockReg_Write2(strReg, 10, intValue, "WriteCalData_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteCalData_Write2:" + strReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }

        /// <summary>
        /// 写入计算数据道PLC
        /// </summary>
        /// <param name="Reg_L"></param>
        /// <param name="dblData"></param>
        /// <param name="strFinishDataReg"></param>
        public void WriteCalData_Write2(List<RegPLC> Reg_L, double[] dblData, string strFinishDataReg)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return;
                }

                //获取寄存器值
                string strReg = GetRegName(Reg_L);
                strReg += strFinishDataReg;

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData(Reg_L, dblData, out realData))
                {
                    return;
                }

                int[] intValue = new int[10];
                intValue[0] = ConvertWriteData(realData[0])[0];//X
                intValue[1] = ConvertWriteData(realData[0])[1];
                intValue[2] = ConvertWriteData(realData[1])[0];//Y
                intValue[3] = ConvertWriteData(realData[1])[1];
                intValue[4] = ConvertWriteData(realData[2])[0];//Z
                intValue[5] = ConvertWriteData(realData[2])[1];
                intValue[6] = ConvertWriteData(realData[3])[0];//R
                intValue[7] = ConvertWriteData(realData[3])[1];
                intValue[8] = 1;
                intValue[9] = 0;

                bool blResult = WriteBlockReg_Write2(strReg, 10, intValue, "WriteCalData_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteCalData_Write2写入寄存器失败:" + strReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_mtWrite.ReleaseMutex();
            }
        }
        #endregion 写入计算数据和数据传输确认信号

        #region 写入数据
        /// <summary>
        /// 将数据写入PLC
        /// </summary>
        /// <param name="reg">reg里面不包含\n</param>
        /// <param name="co">系数</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public bool WriteData_Write2(string[] reg, double[] co, double[] data)
        {
            try
            {
                int numReg = reg.Length;
                int numCo = co.Length;
                int numData = data.Length;
                string strReg = "";

                if (numReg == numCo
                    || numCo == numData)
                {
                    double[] value = new double[numData];
                    for (int i = 0; i < numData; i++)
                    {
                        value[i] = Math.Round(data[i] * co[i], 3);
                    }

                    for (int i = 0; i < numReg; i++)
                    {
                        strReg += reg[i].Replace("\n", "") + "\n";
                    }

                    int[] valueReg = new int[numReg];
                    for (int i = 0; i < numData; i++)
                    {
                        valueReg[2 * i] = ConvertWriteData(value[i])[0];
                        valueReg[2 * i + 1] = ConvertWriteData(value[i])[1];
                    }

                    bool blResult = WriteBlockReg_Write2(strReg, numReg, valueReg, "WriteData_Write2");
                    if (blResult)
                    {
                        return true;
                    }
                    CommunicationState_event("WriteData_Write2写入寄存器失败:" + strReg, "red");
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex, "WriteData寄存器，系数，或数值的数组个数不一样");
                return false;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="resultforWritePLC_L"></param>
        /// <returns></returns>
        public bool WriteData_Write2(List<ResultforWritePLC> resultforWritePLC_L)
        {
            g_mtWrite.WaitOne();
            try
            {
                //离线，不写入PLC
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    return true;
                }

                //获取寄存器值
                string strReg = GetRegName(resultforWritePLC_L);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData(resultforWritePLC_L, out realData))
                {
                    return false;
                }

                //数据个数
                int count = resultforWritePLC_L.Count;
                int[] intValue = new int[count * 2];
                for (int i = 0; i < resultforWritePLC_L.Count; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(strReg, count * 2, intValue, "WriteData_Write2");
                if (blResult)
                {
                    return true;
                }
                CommunicationState_event("WriteData_Write2写入寄存器失败:" + strReg, "red");
                return false;
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
        #endregion 写入数据

        #region 写数据寄存器
        #region 写入数据寄存器1
        /// <summary>
        /// 写入指定位置的数据寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteRegData1_Write2(int index, double data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //数据判断
                double realData = 0;
                if (!ConvertJudgeData_Write(RegData.R_I[index], data, out realData))
                {
                    return;
                }

                int[] intValue = new int[2];
                intValue[0] = ConvertWriteData(realData)[0];//X
                intValue[1] = ConvertWriteData(realData)[1];

                bool blResult = WriteBlockReg_Write2(RegData.R_I[index].NameReg.Replace("\\n", "\n"), 2, intValue, "WriteRegData1_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData1_Write2写入寄存器失败:" + RegData.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }

        /// <summary>
        /// 批量写入寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="data"></param>
        public void WriteRegData1_Write2(int index, int num, double[] data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC//不写入PLC
                {
                    return;
                }
                //获取寄存器
                List<RegPLC> regData_L = RegData.R_I.Reg_L.GetRange(index, num);
                //批量读取寄存器名称     
                string reg = GetRegName(RegData.R_I.Reg_L, index, num);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData_Write(regData_L, data, out realData))
                {
                    return;
                }

                int[] intValue = new int[num * 2];
                for (int i = 0; i < num; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];//X
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(reg.Replace("\\n", "\n"), num * 2, intValue, "WriteRegData1_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData1_Write2写入寄存器失败:" + RegData.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }
        #endregion 写入数据寄存器1

        #region 写入数据寄存器2
        /// <summary>
        /// 写入指定位置的数据寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteRegData2_Write2(int index, double data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //数据判断
                double realData = 0;
                if (!ConvertJudgeData_Write(RegData2.R_I[index], data, out realData))
                {
                    return;
                }

                int[] intValue = new int[2];
                intValue[0] = ConvertWriteData(realData)[0];//X
                intValue[1] = ConvertWriteData(realData)[1];

                bool blResult = WriteBlockReg_Write2(RegData2.R_I[index].NameReg.Replace("\\n", "\n"), 2, intValue, "WriteRegData2_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData2_Write2写RegData2寄存器失败:" + RegData2.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }

        /// <summary>
        /// 批量写入寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="data"></param>
        public void WriteRegData2_Write2(int index, int num, double[] data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC//不写入PLC
                {
                    return;
                }
                //获取寄存器
                List<RegPLC> regData_L = RegData2.R_I.Reg_L.GetRange(index, num);
                //批量读取寄存器名称     
                string reg = GetRegName(RegData2.R_I.Reg_L, index, num);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData_Write(regData_L, data, out realData))
                {
                    return;
                }

                int[] intValue = new int[num * 2];
                for (int i = 0; i < num; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];//X
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(reg.Replace("\\n", "\n"), num * 2, intValue, "WriteRegData3_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData3_Write2写RegData2批量寄存器失败:" + RegData2.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }
        #endregion 写入数据寄存器2

        #region 写入数据寄存器3
        /// <summary>
        /// 写入指定位置的数据寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteRegData3_Write2(int index, double data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //数据判断
                double realData = 0;
                if (!ConvertJudgeData_Write(RegData3.R_I[index], data, out realData))
                {
                    return;
                }

                int[] intValue = new int[2];
                intValue[0] = ConvertWriteData(realData)[0];//X
                intValue[1] = ConvertWriteData(realData)[1];

                bool blResult = WriteBlockReg_Write2(RegData3.R_I[index].NameReg.Replace("\\n", "\n"), 2, intValue, "WriteRegData3_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData3_Write2写RegData3寄存器失败:" + RegData3.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }

        /// <summary>
        /// 批量写入寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="data"></param>
        public void WriteRegData3_Write2(int index, int num, double[] data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //获取寄存器
                List<RegPLC> regData_L = RegData3.R_I.Reg_L.GetRange(index, num);
                //批量读取寄存器名称     
                string reg = GetRegName(RegData3.R_I.Reg_L, index, num);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData_Write(regData_L, data, out realData))
                {
                    return;
                }

                int[] intValue = new int[num * 2];
                for (int i = 0; i < num; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(reg.Replace("\\n", "\n"), num * 2, intValue, "WriteRegData3_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData3_Write2写RegData3批量寄存器失败:" + RegData3.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }
        #endregion 写入数据寄存器3

        #region 写入数据寄存器4
        /// <summary>
        /// 写入指定位置的数据寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteRegData4_Write2(int index, double data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //数据判断
                double realData = 0;
                if (!ConvertJudgeData_Write(RegData4.R_I[index], data, out realData))
                {
                    return;
                }

                int[] intValue = new int[2];
                intValue[0] = ConvertWriteData(realData)[0];
                intValue[1] = ConvertWriteData(realData)[1];

                bool blResult = WriteBlockReg_Write2(RegData4.R_I[index].NameReg.Replace("\\n", "\n"), 2, intValue, "WriteRegData4_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData4_Write2写RegData6寄存器失败:" + RegData4.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }

        /// <summary>
        /// 批量写入寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="data"></param>
        public void WriteRegData4_Write2(int index, int num, double[] data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //获取寄存器
                List<RegPLC> regData_L = RegData4.R_I.Reg_L.GetRange(index, num);
                //批量读取寄存器名称     
                string reg = GetRegName(RegData4.R_I.Reg_L, index, num);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData_Write(regData_L, data, out realData))
                {
                    return;
                }

                int[] intValue = new int[num * 2];
                for (int i = 0; i < num; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(reg.Replace("\\n", "\n"), num * 2, intValue, "WriteRegData4_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData4_Write2写RegData4批量寄存器失败:" + RegData4.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("LogicPLC", ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }
        #endregion 写入数据寄存器4

        #region 写入数据寄存器5
        /// <summary>
        /// 写入指定位置的数据寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteRegData5_Write2(int index, double data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //数据判断
                double realData = 0;
                if (!ConvertJudgeData_Write(RegData5.R_I[index], data, out realData))
                {
                    return;
                }

                int[] intValue = new int[2];
                intValue[0] = ConvertWriteData(realData)[0];
                intValue[1] = ConvertWriteData(realData)[1];

                bool blResult = WriteBlockReg_Write2(RegData5.R_I[index].NameReg.Replace("\\n", "\n"), 2, intValue, "WriteRegData5_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData5_Write2写RegData寄存器失败:" + RegData5.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }

        /// <summary>
        /// 批量写入寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="data"></param>
        public void WriteRegData5_Write2(int index, int num, double[] data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //获取寄存器
                List<RegPLC> regData_L = RegData5.R_I.Reg_L.GetRange(index, num);
                //批量读取寄存器名称     
                string reg = GetRegName(RegData5.R_I.Reg_L, index, num);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData_Write(regData_L, data, out realData))
                {
                    return;
                }

                int[] intValue = new int[num * 2];
                for (int i = 0; i < num; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(reg.Replace("\\n", "\n"), num * 2, intValue, "WriteRegData5_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData5_Write2写RegData5批量寄存器失败:" + RegData5.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }
        #endregion 写入数据寄存器5

        #region 写入数据寄存器6
        /// <summary>
        /// 写入指定位置的数据寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteRegData6_Write2(int index, double data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //数据判断
                double realData = 0;
                if (!ConvertJudgeData_Write(RegData6.R_I[index], data, out realData))
                {
                    return;
                }

                int[] intValue = new int[2];
                intValue[0] = ConvertWriteData(realData)[0];
                intValue[1] = ConvertWriteData(realData)[1];

                bool blResult = WriteBlockReg_Write2(RegData6.R_I[index].NameReg.Replace("\\n", "\n"), 2, intValue, "WriteRegData6_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData6_Write2写RegData6寄存器失败:" + RegData6.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }

        /// <summary>
        /// 批量写入寄存器
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="data"></param>
        public void WriteRegData6_Write2(int index, int num, double[] data)
        {
            g_MtData.WaitOne();
            try
            {
                if (RegeditPLC.R_I.BlOffLinePLC)//不写入PLC
                {
                    return;
                }
                //获取寄存器
                List<RegPLC> regData_L = RegData6.R_I.Reg_L.GetRange(index, num);
                //批量读取寄存器名称     
                string reg = GetRegName(RegData6.R_I.Reg_L, index, num);

                //数据判断和转换（乘以系数）
                double[] realData = null;
                if (!ConvertJudgeData_Write(regData_L, data, out realData))
                {
                    return;
                }

                int[] intValue = new int[num * 2];
                for (int i = 0; i < num; i++)
                {
                    intValue[2 * i] = ConvertWriteData(realData[i])[0];//X
                    intValue[2 * i + 1] = ConvertWriteData(realData[i])[1];
                }

                bool blResult = WriteBlockReg_Write2(reg.Replace("\\n", "\n"), num * 2, intValue, "WriteRegData6_Write2");
                if (blResult)
                {
                    return;
                }
                CommunicationState_event("WriteRegData6_Write2写RegData6批量寄存器失败:" + RegData6.R_I[index].NameReg, "red");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {
                g_MtData.ReleaseMutex();
            }
        }
        #endregion 写入数据寄存器6
        #endregion 写数据寄存器
    }
}
