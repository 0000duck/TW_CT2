using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DCON_PC_DOTNET;
using BasicClass;

namespace DealIO
{
    public class DIOControler
    {
        #region 静态类实例
        public static DIOControler D_I = new DIOControler();
        #endregion 静态类实例

        #region 定义

        public bool IsOpen = false;         //串口连接状态

        public bool IsWhileRead = false;


        Mutex Mt_Write = new Mutex();
        public static DIOControler DIOControler_Inst = new DIOControler();  //静态类实例化

        public event InputAction PCBButtonEvent;   //PCB按钮事件
        public event InputAction FPCButtonEvent;   //FPC按钮事件
        public event InputAction AutoButtonEvent;  //auto按钮事件
        public event InputAction AfterButtonEvent; //按压完成后事件

        public event Action ErrorDI_DIO;
        public event Action ErrorDO_DIO;

        public byte oldPCBButtonState = 0xff;
        public byte oldFPCButtonState = 0xff;
        public byte oldAutoButtonState = 0xff;
        public byte oldAfterButtonState = 0xff;
        #endregion 定义

        #region 连接串口模块
        public bool Open_Com()
        {
            try
            {
                short iRet = DCON.UART.Open_Com((byte)ParDIO.P_I.IntPort, 9600, 8, 0, 1);
                if (Convert.ToBoolean(iRet))
                {                    
                    IsOpen = false;
                    return false;
                }
                else
                {
                    IsOpen = true;
                    IsWhileRead = true;
                    Task task = new Task(Read_DI);
                    task.Start();
                    return true;
                }
            }
            catch (Exception)
            {               
                return false;
            }
        }
        #endregion

        #region 断开串口连接
        public void Close_Com()
        {
            try
            {
                IsWhileRead = false;
                Thread.Sleep(100);
                short iRet = DCON.UART.Close_Com(1);
                if (Convert.ToBoolean(iRet))
                {
                    IsOpen = false;                   
                    return;
                }
                else
                {
                    IsOpen = false;
                }
            }
            catch (Exception)
            {
               
            }
        }
        #endregion

        #region 读取io
        public void Read_DI()
        {
            while (IsWhileRead)
            {

                Thread.Sleep(10);
                try
                {
                    uint DIValue = 0;
                    Mt_Write.WaitOne();
                    short iRet = DCON.IO_Function.Read_DI(1, 1, -1, 16, 0, 100, out DIValue);
                    Mt_Write.ReleaseMutex();
                    if (Convert.ToBoolean(iRet))
                    {
                        ErrorDI_DIO();                        
                        IsWhileRead = false;
                        IsOpen = false;
                        return;
                    }
                    else
                    {
                        //判断是那个di触发,以上升沿作为判断依据

                        //PCB建立真空按钮
                        byte ui = (byte)((DIValue ^ 0xffff) & 0x02);
                        byte ui1 = (byte)((DIValue ^ 0xffff) & 0x02);
                        if (ui != oldPCBButtonState && ui > 0)
                        {
                            oldPCBButtonState = ui;
                            PCBButtonEvent(1);
                        }
                        else if (ui != oldPCBButtonState && ui == 0)
                        {
                            //Thread.Sleep(1500);
                            oldPCBButtonState = ui;
                            PCBButtonEvent(0);
                        }

                        //FPC建立真空按钮
                        ui = (byte)((DIValue ^ 0xffff) & 0x10);
                        if (ui != oldFPCButtonState && ui > 0)
                        {
                            oldFPCButtonState = ui;
                            FPCButtonEvent(1);
                        }
                        else if (ui != oldFPCButtonState && ui == 0)
                        {
                            oldFPCButtonState = ui;
                            FPCButtonEvent(0);
                        }

                        //自动运行按钮按下
                        ui = (byte)((DIValue ^ 0xff) & 0x0c);
                        ui1 = (byte)((DIValue ^ 0xffff) & 0x0c);
                        if (ui != oldAutoButtonState &&  ui>0)
                        {
                            oldAutoButtonState = ui;
                            //if ((ui & 0x0c) == 12 )
                            //{
                                AutoButtonEvent(1);
                            //}
                            
                        }
                        else if (ui != oldAutoButtonState && ui == 0)
                        {
                            oldAutoButtonState = ui;
                            AutoButtonEvent(0);
                        }


                        //热压完成信号
                        ui = (byte)((DIValue ^ 0xffff) & 0x80);
                        if (ui != oldAfterButtonState && ui > 0)
                        {
                            oldAfterButtonState = ui;
                            AfterButtonEvent(1);
                        }
                        else if (ui != oldAfterButtonState && ui == 0)
                        {
                            oldAfterButtonState = ui;
                            AfterButtonEvent(0);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }
        #endregion

        #region 按位写输出信号
        public void Write_DO_Bit(short channel, bool state)
        {
            try
            {
                short iRet = DCON.IO_Function.Write_DIO_Bit(1, 1, -1, channel, 16, state, 0, 100);
                if (Convert.ToBoolean(iRet))
                {                   
                    IsWhileRead = false;
                    IsOpen = false;
                    return;
                }
            }
            catch (Exception)
            {
               
            }
            finally
            {
                
            }
        }
        #endregion
    }

    public delegate void InputAction(int val);

}
