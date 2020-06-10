using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using DealConfigFile;
using Common;
using BasicClass;
using DealCalibrate;
using DealImageProcess;
using DealPLC;

namespace Main_EX
{
    public partial class ParStateSoft
    {
        #region 静态类实例
       
        #endregion 静态类实例

        #region 定义

        public static StateMachine_enum StateMachine_e = StateMachine_enum.Auto;

      
        #endregion 定义

    }

    public enum StateMachine_enum
    {
        Idle = 1,
        Manual = 2,
        Auto = 3,
        NullRun = 4,//空跑
        Alarm = 5,
        Pause = 6,
        Stop = 7,
        Reset = 8,
        Calib = 9,
    }
}
