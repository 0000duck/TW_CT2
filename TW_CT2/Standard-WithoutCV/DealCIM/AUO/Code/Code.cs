using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using BasicClass;
using System.Threading;

namespace DealCIM
{
    public partial class AUOCode
    {
        #region 静态类实例
        public static AUOCode A_I = new AUOCode();
        #endregion 静态类实例

        #region  定义
        string result = "";
        //换行回车符
        readonly byte[] CRLF = { Convert.ToByte("0D", 16), Convert.ToByte("0A", 16) };
        SerialPort serialPortCode = new SerialPort();

        public event StrAction ReadData_event;

        bool blCycRead = false;
        public bool blCodeReady = false;
        #endregion  定义

        #region 初始化
        public AUOCode()
        {
            this.serialPortCode.PortName = "COM2";
            this.serialPortCode.BaudRate = 9600;
            this.serialPortCode.DtrEnable = true;
            this.serialPortCode.Parity = System.IO.Ports.Parity.None;
            this.serialPortCode.RtsEnable = true;
        }
        #endregion 初始化

        #region 端口控制
        //打开
        public bool Open()
        {
            try
            {
                if (!serialPortCode.IsOpen)
                {
                    serialPortCode.Open();
                    Read_Task();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        //关闭
        public bool Close()
        {
            try
            {
                if (serialPortCode.IsOpen)
                {
                    AUOCode.A_I.blCycRead = false;
                    Thread.Sleep(100);
                    serialPortCode.Close();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion 端口控制
    }
}
