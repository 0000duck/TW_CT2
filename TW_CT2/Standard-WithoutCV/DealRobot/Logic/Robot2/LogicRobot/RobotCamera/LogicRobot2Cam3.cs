﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DealRobot
{
    public class LogicRobot2Cam3 : LogicRobot
    {
        #region 静态类实例
        public static new LogicRobot2 L_I
        {
            get
            {
                LogicRobot2.L_I.NoCamera = 3;
                return LogicRobot2.L_I;
            }
        }
        #endregion 静态类实例
    }
}
