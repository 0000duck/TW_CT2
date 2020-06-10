
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealComprehensive;
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using DealConfigFile;
using SetPar;
using HalconDotNet;
using Common;
using DealImageProcess;
using System.Windows;

namespace Main_EX
{
    partial class WinInitMain
    {
        /// <summary>
        /// 软件打开，换型的时候设置拍照参数
        /// </summary>
        void PreSetPhoto()
        {
            string str = "";
            try
            {     
                //相机1
                if(DealComprehensive1.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 2)
                {
                    return;
                }
                //相机2
                if (DealComprehensive2.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 3)
                {
                    return;
                }
                //相机3
                if (DealComprehensive3.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 4)
                {
                    return;
                }
                //相机4
                if (DealComprehensive4.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 5)
                {
                    return;
                }
                //相机5
                if (DealComprehensive5.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 6)
                {
                    return;
                }
                //相机6
                if (DealComprehensive6.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 7)
                {
                    return;
                }
                //相机7
                if (DealComprehensive7.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
                if (ParCameraWork.NumCamera < 8)
                {
                    return;
                }
                //相机8
                if (DealComprehensive8.D_I.PreSetPhoto(out str))
                {
                    ShowState(str);
                }
                else
                {
                    ShowAlarm(str);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
