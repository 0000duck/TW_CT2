using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulePackage
{
    /// <summary>
    /// 单目拍照点位枚举
    /// </summary>
    public enum Mono
    {
        AutoMark1,
        AutoMark2,
        Calib1,
        Calib2,
        Verify1,
        Verify2,
        CalcStd,
        CalibRC,
        VerifyRC
    }

    /// <summary>
    /// 机器人角度
    /// </summary>
    public enum DirBot_Enum
    {
        Forward = 0,
        Left = 90,
        Backward = 180,
        Right = -90
    }
    /// <summary>
    /// 画面显示角度
    /// </summary>
    public enum DirDisplay_Enum
    {
        正常 = 0,
        逆时针旋转90度 = -90,
        顺时针旋转90度 = 90,
        逆时针旋转180度 = 180,
    }
    /// <summary>
    /// 背光角度
    /// </summary>
    public enum DirBL_Enum
    {
        正常 = 0,
        逆时针90度 = 90,
        逆时针180度 = 180,
        顺时针90度 = -90
    }
    /// <summary>
    /// 平台放片位置
    /// </summary>
    public enum PlatformPlacePos_Enum
    {
        LeftTop,
        LeftBottom,
        RightBottom,
        RightTop
    }
    /// <summary>
    /// 单电极放平台时，电极的朝向，电极在左右为水平，在上下为竖直
    /// </summary>
    public enum DirPlace_Enum
    {
        水平放置,
        竖直放置
    }

    #region CST
    /// <summary>
    /// 拍照方向，以面向机台操作面，分为相机背对操作面（正向）和相机朝向操作面(反向)，影响插栏偏差符号
    /// </summary>
    public enum DirCstCamera_Enum
    {
        //正向
        Forward = 1,
        //反向
        Backward = -1
    }
    /// <summary>
    /// 插栏方向，分为从PLC插栏轴负方向到正方向以及相反，影响插栏基准位计算方向
    /// </summary>
    public enum DirInsert_Enum
    {
        NToP = 1,
        PToN = -1
    }

    /// <summary>
    /// z轴补正是由CST机构（两轴）完成，还是由插栏模组（三轴）完成，影响z轴补正符号
    /// </summary>
    public enum TypeModuleZ_Enum
    {
        //两轴
        CSTUp = 1,
        //三轴
        ModuleUp = -1
    }
    #endregion
}
