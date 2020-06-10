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
using HslCommunication;
using HslCommunication.Profinet;

namespace DealPLC
{
    public partial class LogicPLC : BaseClass
    {
        #region 静态类实例
        public static LogicPLC L_I = new LogicPLC();
        #endregion 静态类实例

        #region 定义
        //Class
        public PortPLC PortPLC_I = new PortPLC();

        //bool 
        bool BlOpen = false;

        //Mutex
        Mutex g_MtGetRegName = new Mutex();
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogicPLC()
        {
            NameClass = "LogicPLC";
        }
        #endregion 初始化

        #region 连接和断开PLC
        /// <summary>
        /// 打开通信接口
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool OpenPort(out string error)
        {
            error = "";
            try
            {
                //如果为离线模式
                if (RegeditPLC.R_I.BlOffLinePLC)
                {
                    if (ParSetPLC.P_I.BlReadCycReg)//开启读取触发寄存器线程
                    {
                        ParLogicPLC.P_I.BlRead = true;
                        SingleTaskRead();
                    }
                    return true;
                }

                //非离线模式
                bool blResult = false;
                //PLC独立软件
                if (ParSetPLC.P_I.BlAnnotherPLC)
                {
                    blResult = OpenClient();//打开以太网端口
                }
                else
                {
                    blResult = PortPLC_I.OpenPort(out error);
                }

                BlOpen = blResult;
                try
                {
                    PortPLC_I.g_BasePortPLC.BlOpen = BlOpen;
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError(NameClass, ex);
                }
              
                if (blResult)//PLC打开成功
                {
                    if (ParSetPLC.P_I.BlClearAllTrigger)//清空触发信号
                    {
                        ClearTrigger();
                    }

                    if (ParSetPLC.P_I.BlReadCycReg)//开启读取触发寄存器线程
                    {
                        ReadCycReg_Task();
                    }

                    PCConnectPLC();//发送给PLC连接确认
                    return true;
                }
                else//PLC打开失败
                {
                    ParLogicPLC.P_I.BlRead = true;
                    //打开失败，则默认启动线程
                    SingleTaskRead();
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }


        /// <summary>
        /// 关闭通信接口
        /// </summary>
        /// <returns></returns>
        public bool ClosePLC()
        {
            try
            {
                //首先关闭实时读取
                ParLogicPLC.P_I.BlRead = false;

                //关闭独立PLC软件通信
                CloseClient();

                PCDisConnectPLC();//断开和PLC的连接信号
                if (PortPLC_I.ClosePort())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 连接LogicPLC

        #region 获取寄存器的名称
        /// <summary>
        /// 获取寄存器的名称
        /// </summary>
        /// <param name="reg_L"></param>
        /// <returns></returns>
        string GetRegName(List<RegPLC> reg_L)
        {
            g_MtGetRegName.WaitOne();
            try
            {
                string strReg = "";
                for (int i = 0; i < reg_L.Count; i++)
                {
                    strReg += reg_L[i].NameReg;
                }
                return strReg.Replace("\\n", "\n");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return "";
            }
            finally
            {
                g_MtGetRegName.ReleaseMutex();
            }
        }

        string GetRegName(List<RegPLC> reg_L, int Num)
        {
            g_MtGetRegName.WaitOne();
            try
            {
                string strReg = "";
                for (int i = 0; i < Num; i++)
                {
                    strReg += reg_L[i].NameReg;
                }
                return strReg.Replace("\\n", "\n");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return "";
            }
            finally
            {
                g_MtGetRegName.ReleaseMutex();
            }
        }

        string GetRegName(List<RegPLC> reg_L, int index, int num)
        {
            g_MtGetRegName.WaitOne();
            try
            {
                string strReg = "";
                for (int i = index; i < index + num; i++)
                {
                    strReg += reg_L[i].NameReg;
                }
                return strReg.Replace("\\n", "\n");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return "";
            }
            finally
            {
                g_MtGetRegName.ReleaseMutex();
            }
        }

        string GetRegName(List<ResultforWritePLC> reg_L)
        {
            g_MtGetRegName.WaitOne();
            try
            {
                string strReg = "";
                for (int i = 0; i < reg_L.Count; i++)
                {
                    strReg += reg_L[i].NameReg;
                }
                return strReg.Replace("\\n", "\n");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return "";
            }
            finally
            {
                g_MtGetRegName.ReleaseMutex();
            }
        }
        #endregion 获取寄存器的名称
    }
}
