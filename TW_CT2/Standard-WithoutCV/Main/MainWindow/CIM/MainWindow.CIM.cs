using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using System.Threading;
using DealFile;
using DealConfigFile;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DealPLC;
using Common;
using System.Text.RegularExpressions;
using DealCIM;

namespace Main
{
    partial class MainWindow
    {
        /// <summary>
        /// 新建一个Task来初始化CIM端口
        /// </summary>
        public override void Init_CIM()
        {
            if (AUOCode.A_I.Open())
            {
                ShowState("二维码端口连接OK!");
            }
            else
            {
                ShowAlarm("二维码端口打开NG!");
            }

        }

        /// <summary>
        /// 注册
        /// </summary>
        public override void LoginEvent_CIM()
        {
            try
            {
                AUOCode.A_I.ReadData_event += new StrAction(C_I_ReadData_event);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 关闭CIM端口
        /// </summary>
        public override void CloseCIM()
        {
            try
            {
                AUOCode.A_I.ReadData_event -= new StrAction(C_I_ReadData_event);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 二维码读取事件处理程序
        /// </summary>
        /// <param name="strCode"></param>
        void C_I_ReadData_event(string strCode)
        {
            try
            {
                string codeReal = Regex.Match(strCode, "[A-z0-9]+-?[A-z0-9]+", RegexOptions.IgnoreCase).ToString();
                if (codeReal == "")
                {
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeResult, 2);
                    ShowState("二维码读取失败");
                    return;
                }


                SendCodeToPLCReg(codeReal);

                AUOCode.A_I.blCodeReady = true;

                ShowState("二维码读取结束，通知PLC继续运行");

                LogicPLC.L_I.WriteRegData1((int)DataRegister1.QrCodeResult, 1);


                ShowState("读取到二维码" + codeReal);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        private void ClearCodePLCReg()
        {
            try
            {
                int firstAddr = (int)DataRegister1.QrInfo;

                for (int i = 0; i < 5; i++)
                {
                    LogicPLC.L_I.WriteRegData1(firstAddr + i, 0);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        private void SendCodeToPLCReg(string StrCode)
        {
            try
            {

                ClearCodePLCReg();

                ShowState("已经清空二维码寄存器");

                int firstAddr = (int)DataRegister1.QrInfo;

                int length = StrCode.Length;

                int pair = length / 2;
                int single = length % 2;

                int[] RegData = new int[pair + single];

                for (int i = 0; i < RegData.Length-1; ++i)
                {
                    RegData[i] = (int)StrCode[2 * i + 1] * 256 + (int)StrCode[2 * i];
                }
                if (single == 1)
                {
                    RegData[pair] = (int)StrCode[length - 1];
                }
                else
                {
                    RegData[pair - 1] = (int)StrCode[2 * (pair - 1) + 1] * 256 + (int)StrCode[2 * (pair - 1)];
                }


                int solidAmp= 65536;
                int first = RegData[0];
                int second = length;
                int RealRegSend = first * solidAmp + second;

                LogicPLC.L_I.WriteRegData1(firstAddr, RealRegSend);

                for (int j = 0; j < (RegData.Length - 1) / 2; ++j)
                {
                    first = RegData[j * 2 + 2];
                    second = RegData[j * 2 + 1];
                    RealRegSend = first * solidAmp + second;
                    LogicPLC.L_I.WriteRegData1(firstAddr + j + 1, RealRegSend);
                }
                if ((RegData.Length - 1) % 2 == 1)
                {
                    second = RegData[RegData.Length - 1];
                    RealRegSend = second;
                    LogicPLC.L_I.WriteRegData1(firstAddr + (RegData.Length - 1) / 2 + 1, RealRegSend);
                }

                ShowState("向PLC寄存器写二维码结束!");

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

    }
}
