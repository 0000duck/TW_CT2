using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using BasicClass;

namespace DealPLC
{
    public partial class ParSetPLC:BaseClass
    {
        #region 静态类实例
        public static ParSetPLC P_I = new ParSetPLC();
        #endregion 静态类实例

        #region 定义

        //PLC类型
        public TypePLC_enum TypePLC_e = TypePLC_enum.MIT;
        public TypePLCProtocol_enum TypePLCProtocol_e = TypePLCProtocol_enum.Full;

        //string 
        public string IP = "";

        //int
        int noStation = 0;
        public int NoStation
        {
            get
            {
                if (noStation == 0)
                {
                    noStation = 1;
                }
                return noStation;
            }
            set
            {
                noStation = value;
            }
        }
        public int Delay = 0;

        /// <summary>
        /// 通用端口
        /// </summary>
        public int Port = 6000;

        /// <summary>
        /// 写入端口
        /// </summary>
        public int PortWrite1 = 6000;
        public int PortWrite2 = 6000;
        public int PortWrite3 = 6000;
        public int PortWrite4 = 6000;
        public int PortWrite5 = 6000;
        public int PortWrite6 = 6000;

        //bool
        public bool BlReadCycReg = false;
        public bool BlRSingleTaskCamera = false;//相机触发单线程
        public bool BlClearAllTrigger = false;//清空触发信号

        public bool BlAnnotherPLC = false;//PLC独立触发
        public bool BlAnnotherPLCLog = false;//PLC独立触发日志

        /// <summary>
        /// 写端口
        /// </summary>
        public bool BlWritePort1 = false;//写端口1
        public bool BlWritePort2 = false;//写端口1
        public bool BlWritePort3 = false;//写端口1
        public bool BlWritePort4 = false;//写端口1
        public bool BlWritePort5 = false;//写端口1
        public bool BlWritePort6 = false;//写端口1
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParSetPLC()
        {
            NameClass = "ParSetPLC";
        }
        #endregion 初始化
    }

    /// <summary>
    /// PLC类型枚举
    /// </summary>
    public enum TypePLC_enum
    {
        Null,
        MIT,//三菱       
        SEM,//西门子
        PAN,//松下

        MIT_NEW,//三菱最新驱动，支持最新的型号，如FX5U

        MIT_Hls,//三菱开源通信软件
    }

    //PLC协议版本
    public enum TypePLCProtocol_enum
    {
        Full,   
        FullNew,              
    }
}
