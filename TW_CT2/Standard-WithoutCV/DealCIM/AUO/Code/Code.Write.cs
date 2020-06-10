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
        public void Write()
        {
            try
            {
                //发什么那边东西给二维码设备
                serialPortCode.WriteLine("READ\r");
                serialPortCode.Write(CRLF, 0, CRLF.Length);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Code", ex);
            }
        }
    }
}
