using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using BasicClass;

namespace DealPLC
{
    /// <summary>
    /// PLC端口
    /// </summary>
    public class PortPLC : BaseClass
    {
        #region 定义
        /// <summary>
        /// 通用端口类
        /// </summary>
        public BasePortPLC g_BasePortPLC = null;

        /// <summary>
        /// 写端口
        /// </summary>
        public BasePortPLC g_BasePortPLC1 = null;
        public BasePortPLC g_BasePortPLC2 = null;
        public BasePortPLC g_BasePortPLC3 = null;
        public BasePortPLC g_BasePortPLC4 = null;
        public BasePortPLC g_BasePortPLC5 = null;
        public BasePortPLC g_BasePortPLC6 = null;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public PortPLC()
        {
            NameClass = "PortPLC";

            //初始化PLC端口
            InitPort();
        }
        /// <summary>
        /// PLC端口
        /// </summary>
        void InitPort()
        {
            try
            {
                #region 通用端口
                switch (ParSetPLC.P_I.TypePLC_e)
                {
                    case TypePLC_enum.MIT:
                        g_BasePortPLC = new PortPLC_MIT();
                        break;

                    case TypePLC_enum.SEM:
                        break;

                    case TypePLC_enum.PAN:
                        break;

                    case TypePLC_enum.MIT_NEW:
                        g_BasePortPLC = new PortPLC_MITNew();
                        break;

                    case TypePLC_enum.MIT_Hls:
                        g_BasePortPLC = new PortPLC_MITHls();
                        break;

                    default:
                        
                        break;
                }
                #endregion 通用端口

                #region 写端口
                //端口1
                if (ParSetPLC.P_I.BlWritePort1)
                {
                    g_BasePortPLC1 = new PortPLC_MITHls(ParSetPLC.P_I.PortWrite1);
                }
                //端口2
                if (ParSetPLC.P_I.BlWritePort2)
                {
                    g_BasePortPLC2 = new PortPLC_MITHls(ParSetPLC.P_I.PortWrite2);
                }
                //端口3
                if (ParSetPLC.P_I.BlWritePort3)
                {
                    g_BasePortPLC3 = new PortPLC_MITHls(ParSetPLC.P_I.PortWrite3);
                }
                //端口4
                if (ParSetPLC.P_I.BlWritePort4)
                {
                    g_BasePortPLC4 = new PortPLC_MITHls(ParSetPLC.P_I.PortWrite4);
                }
                //端口5
                if (ParSetPLC.P_I.BlWritePort5)
                {
                    g_BasePortPLC5 = new PortPLC_MITHls(ParSetPLC.P_I.PortWrite5);
                }
                //端口6
                if (ParSetPLC.P_I.BlWritePort6)
                {
                    g_BasePortPLC6 = new PortPLC_MITHls(ParSetPLC.P_I.PortWrite6);
                }
                #endregion 写端口
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public bool OpenPort(out string info)
        {
            info = "";
            string str = "";
            bool blResult = false;
            int numError = 0;
            try
            {
                blResult = g_BasePortPLC.OpenPLC(out str);
                if (!blResult)
                {
                    numError++;
                    info += "通用端口:" + str + "\n";
                }

                //打开端口1
                if (ParSetPLC.P_I.BlWritePort1)
                {
                    blResult = g_BasePortPLC1.OpenPLC(out str);
                    if (!blResult)
                    {
                        numError++;
                        info += "写入端口1:" + str + "\n";
                    }
                }
                //打开端口2
                if (ParSetPLC.P_I.BlWritePort2)
                {
                    blResult = g_BasePortPLC3.OpenPLC(out str);
                    if (!blResult)
                    {
                        numError++;
                        info += "写入端口2:" + str + "\n";
                    }
                }

                //打开端口3
                if (ParSetPLC.P_I.BlWritePort3)
                {
                    blResult = g_BasePortPLC1.OpenPLC(out str);
                    if (!blResult)
                    {
                        numError++;
                        info += "写入端口3:" + str + "\n";
                    }
                }

                //打开端口4
                if (ParSetPLC.P_I.BlWritePort4)
                {
                    blResult = g_BasePortPLC4.OpenPLC(out str);
                    if (!blResult)
                    {
                        numError++;
                        info += "写入端口4:" + str + "\n";
                    }
                }

                //打开端口5
                if (ParSetPLC.P_I.BlWritePort5)
                {
                    blResult = g_BasePortPLC5.OpenPLC(out str);
                    if (!blResult)
                    {
                        numError++;
                        info += "写入端口5:" + str + "\n";
                    }
                }

                //打开端口6
                if (ParSetPLC.P_I.BlWritePort6)
                {
                    blResult = g_BasePortPLC6.OpenPLC(out str);
                    if (!blResult)
                    {
                        numError++;
                        info += "写入端口6:" + str + "\n";
                    }
                }

                if (numError > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        /// <returns></returns>
        public bool ClosePort()
        {
            int numError = 0;
            try
            {
                if (!g_BasePortPLC.ClosePLC())
                {
                    numError++;
                }

                if (!g_BasePortPLC1.ClosePLC())
                {
                    numError++;
                }
                if (!g_BasePortPLC2.ClosePLC())
                {
                    numError++;
                }
                if (!g_BasePortPLC3.ClosePLC())
                {
                }
                if (!g_BasePortPLC4.ClosePLC())
                {
                    numError++;
                }
                if (!g_BasePortPLC5.ClosePLC())
                {
                    numError++;
                }
                if (!g_BasePortPLC6.ClosePLC())
                {
                    numError++;
                }
                if (numError > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
    }
}
