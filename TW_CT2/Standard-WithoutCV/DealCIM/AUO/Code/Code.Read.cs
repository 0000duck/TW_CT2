using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using BasicClass;
using System.Threading;
using System.Threading.Tasks;

namespace DealCIM
{
    public partial class AUOCode
    {
        public void Read_Task()
        {
            AUOCode.A_I.blCycRead = true;
            Task task = new Task(Read);
            task.Start();
        }

        void Read()
        {
            while (AUOCode.A_I.blCycRead)
            {
                Thread.Sleep(50);
                try
                {
                    string value = serialPortCode.ReadExisting();
                    if (!value.Contains("\n"))
                    {
                        result += value;
                    }
                    else
                    {
                        //value = value.Substring(1, value.IndexOf('\r') - 1);
                        result += value;
                        //string str = result.Replace("\n", "").Replace("\r", "");//截取有效数据

                        ReadData_event(result);
                        result = string.Empty;
                        value = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError("Code", ex);
                }
            }
        }
    }
}
