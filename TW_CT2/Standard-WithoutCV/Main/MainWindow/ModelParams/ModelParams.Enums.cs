namespace Main
{
    #region robot std
    public enum BotStd
    {
        /// <summary>
        /// 上游平台取片基准值
        /// </summary>
        UpStreamPlat = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 下游平台位置
        /// </summary>
        DownStreamPlat,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DumpPos
    }
    #endregion

    #region robot adj
    public enum BotAdj
    {
        /// <summary>
        /// 上游平台取片调整
        /// </summary>
        UpStreamPlat = 1,
        /// <summary>
        /// 下游平台放片调整
        /// </summary>
        DownStreamPlat = 3,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DumpPos,
        /// <summary>
        /// 工位1-1放片微调
        /// </summary>
        St1_1 = 5,
        /// <summary>
        /// 工位2-1放片微调
        /// </summary>
        St2_1,
        /// <summary>
        /// 工位3-1放片微调
        /// </summary>
        St3_1,
        /// <summary>
        /// 工位1-2放片微调
        /// </summary>
        St1_2 = 9,
        /// <summary>
        /// 工位2-2放片微调
        /// </summary>
        St2_2,
        /// <summary>
        /// 工位3-2放片微调
        /// </summary>
        St3_2
    }
    #endregion

    #region common std
    public enum ComStd
    {
        /// <summary>
        /// 相机1像素基准
        /// </summary>
        StdPxlCamera1 = 1,
        /// <summary>
        /// 相机4像素基准
        /// </summary>
        StdPxlCamera4
    }
    #endregion

    #region common adj
    public enum ComAdj
    {
        /// <summary>
        /// 滚筒线取片调整
        /// </summary>
        AdjRollerLinePick = 1,
        /// <summary>
        /// 二维码调整
        /// </summary>
        QrCode,
        /// <summary>
        /// 取料平台T轴补偿
        /// </summary>
        TPickPlat,
        /// <summary>
        /// 旋转中心
        /// </summary>
        RotateCenter = 24
    }
    #endregion

    #region plc寄存器
    /// <summary>
    /// 配方寄存器 d2020
    /// </summary>
    public enum RecipeRegister
    {
        /// <summary>
        /// 玻璃X
        /// </summary>
        GlassX,
        /// <summary>
        /// 玻璃Y
        /// </summary>
        GlassY,
        /// <summary>
        /// 玻璃厚度
        /// </summary>
        Thickness,
        /// <summary>
        /// 二维码X
        /// </summary>
        QrCodeX,
        /// <summary>
        /// 二维码Y
        /// </summary>
        QrCodeY,
        /// <summary>
        /// Mark1X
        /// </summary>
        Mark1X,
        /// <summary>
        /// Mark1Y
        /// </summary>
        Mark1Y,
        /// <summary>
        /// Mark2X
        /// </summary>
        Mark2X,
        /// <summary>
        /// Mark2Y
        /// </summary>
        Mark2Y,
        /// <summary>
        /// 抛料方向
        /// </summary>
        DirDump =10,
        /// <summary>
        /// AOI放料方向
        /// </summary>
        DirAOI,
        /// <summary>
        /// 下游平台放料方向
        /// </summary>
        DirPlat2,
        /// <summary>
        /// 一个工位做1片还是2片
        /// </summary>
        QuantityInAOI
    }
    /// <summary>
    /// 数据寄存器1，d2500
    /// </summary>
    public enum DataRegister1
    {
        /// <summary>
        /// 软件报警
        /// </summary>
        PCAlarm = 1,
        /// <summary>
        /// 二维码结果
        /// </summary>
        QrCodeResult = 4,
        /// <summary>
        /// 标定双目定位旋转中心T轴角度写入确认信号
        /// </summary>
        TAngleConfirm_calib,
        /// <summary>
        /// 二维码信息d2512-d2521
        /// </summary>
        QrInfo,
        /// <summary>
        /// 平台取片t轴角度写入确认信号
        /// </summary>
        TAngleConfirm_pickPlat1 = 11,
        /// <summary>
        /// 放工位T轴角度写入确认信号
        /// </summary>
        TAngleConfirm_placeAOI,
        /// <summary>
        /// 双目定位精度验证T轴角度写入确认信号
        /// </summary>
        TAngleConfirm_preciseConfirm,
        /// <summary>
        /// 标定时通知plc建立在籍
        /// </summary>
        CalibStation,
        /// <summary>
        /// 标定旋转中心时T轴旋转角度
        /// </summary>
        TAngle_calib = 25,
        /// <summary>
        /// 滚筒线取片补偿X
        /// </summary>
        PickCompensateX,
        /// <summary>
        /// 滚筒线取片补偿Y
        /// </summary>
        PickCompensateY,
        /// <summary>
        /// 二维码补偿X
        /// </summary>
        QrCodeCompensateX,
        /// <summary>
        /// 二维码补偿Y
        /// </summary>
        QrCodeCompensateY,
        /// <summary>
        /// 下游平台Y轴补偿
        /// </summary>
        DownStreamYCompensate,
        /// <summary>
        /// 平台取片T轴角度
        /// </summary>
        TAngle_pickPlat1,
        /// <summary>
        /// 双目定位T轴角度
        /// </summary>
        TAngle_precise,
        /// <summary>
        /// 放工位T轴角度
        /// </summary>
        TAngle_placeAOI,
        /// <summary>
        /// 取工位1-1/1-2的T轴角度
        /// </summary>
        TAngle_pickAOI1,
        /// <summary>
        /// 取工位2-1/2-2的T轴角度
        /// </summary>
        TAngle_pickAOI2,
        /// <summary>
        /// 取工位3-1/3-2的T轴角度
        /// </summary>
        TAngle_pickAOI3,
        /// <summary>
        /// 放下游平台的T轴角度
        /// </summary>
        TAngle_placePlat2,
        /// <summary>
        /// 双目定位精度验证的T轴角度
        /// </summary>
        TAngle_preciseConfirm
    }
    /// <summary>
    /// 数据寄存器2,暂不使用
    /// </summary>
    public enum DataRegister2
    {
        /// <summary>
        /// 插栏1基准值
        /// </summary>
        StdCSTPos1,
        /// <summary>
        /// 插栏2基准值
        /// </summary>
        StdCSTPos2,
        /// <summary>
        /// 插栏3基准值
        /// </summary>
        StdCSTPos3,
        /// <summary>
        /// 插栏4基准值
        /// </summary>
        StdCSTPos4,
        /// <summary>
        /// 平台处玻璃X
        /// </summary>
        WidthAtPlat,
        /// <summary>
        /// 平台处玻璃Y
        /// </summary>
        HeightAtPlat,
        /// <summary>
        /// 巡边交接补偿X
        /// </summary>
        InspDeltaX,
        /// <summary>
        /// 巡边交接补偿Y
        /// </summary>
        InspDeltaY,
        /// <summary>
        /// 巡边交接补偿R
        /// </summary>
        InspDeltaR,
        /// <summary>
        /// 翻转平台处玻璃角度
        /// </summary>
        PlatAngle,
        /// <summary>
        /// 标定时补正角度
        /// </summary>
        CalibDeltaR,
        /// <summary>
        /// 二维码补偿X
        /// </summary>
        CodeComX,
        /// <summary>
        /// 二维码补偿Y
        /// </summary>
        CodeComY,
        /// <summary>
        /// 平台处二维码X
        /// </summary>
        CodeXAtPlat = 13,
        /// <summary>
        /// 平台处二维码Y
        /// </summary>
        CodeYAtPlat,
        /// <summary>
        /// 平台处MarkX
        /// </summary>
        MarkXAtPlat,
        /// <summary>
        /// 平台处MarkY
        /// </summary>
        MarkYAtPlat,
        /// <summary>
        /// 平台处上电极宽度
        /// </summary>
        TopEAtPlat,
        /// <summary>
        /// 平台处下电极宽度
        /// </summary>
        BottomEAtPlat,
        /// <summary>
        /// 平台处左电极宽度
        /// </summary>
        LeftEAtPlat,
        /// <summary>
        /// 平台处右电极宽度
        /// </summary>
        RightEAtPlat,
      
    }
    /// <summary>
    /// 数据寄存器3,暂不使用
    /// </summary>
    public enum DataRegister3
    {
        InsertStdCom,
        InsertData,
        InsertComZ1,
        KeelSpacing1 = 8,
    }
    #endregion

    #region plc报警代码
    public enum PCAlarm_Enum
    {
        标定失败 = 1,
        卡塞计算失败 = 2,
    }
    #endregion

    #region 运行模式设定
    public enum RunningMode
    {
        RecordData,
        PreciseConfirm,
        PassQrCode
    }
    #endregion

    //#region t轴确认分类
    //public enum Type_TConfirm
    //{
    //    UpStream = 1,
    //    BinaryPrecise,
    //    PlaceAOI,
    //    PickAOI,
    //    DownStream,
    //    Dump,
    //    CalibRC
    //}
    //#endregion
}
