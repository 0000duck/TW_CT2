using BasicClass;
using DealConfigFile;
using System;


namespace Main
{
    public partial class ModelParams
    {
        #region packing
        #region 配方参数

        /// <summary>
        /// 配方-玻璃X
        /// </summary>
        public static double confGlassX
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassX].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃Y
        /// </summary>
        public static double confGlassY
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassY].DblValue;
            }
        }
        /// <summary>
        /// 配方-二维码X
        /// </summary>
        public static double confQrCodeX
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.QrCodeX].DblValue;
            }
        }
        /// <summary>
        /// 配方-二维码Y
        /// </summary>
        public static double confQrCodeY
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.QrCodeY].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃厚度
        /// </summary>
        public static double confGlassThicknes
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Thickness].DblValue;
            }
        }
        /// <summary>
        /// 配方-Mark1X
        /// </summary>
        public static double confMark1X
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Mark1X].DblValue;
            }
        }
        /// <summary>
        /// 配方-Mark1Y
        /// </summary>
        public static double confMark1Y
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Mark1Y].DblValue;
            }
        }
        /// <summary>
        /// 配方-Mark2X
        /// </summary>
        public static double confMark2X
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Mark2X].DblValue;
            }
        }
        /// <summary>
        /// 配方-Mark2Y
        /// </summary>
        public static int confMark2Y
        {
            get
            {                
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Mark2Y].DblValue;
            }
        }
        /// <summary>
        /// 配方-抛料方向
        /// </summary>
        public static int confDirDump
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DirDump].DblValue;
            }
        }
        /// <summary>
        /// 配方-AOI放料方向
        /// </summary>
        public static int confDirAOI
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DirAOI].DblValue;
            }
        }
        /// <summary>
        /// 配方-下游平台放料方向
        /// </summary>
        public static int confDirDownStream
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DirPlat2].DblValue;
            }
        }
        #endregion

        #region 运行模式设定
        /// <summary>
        /// 运行设定模式-是否记录中间数据
        /// </summary>
        public static bool IfRecordData
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.RecordData].BlSelect;
            }
        }
        /// <summary>
        /// 运行设定模式-是否进行精度验证拍照
        /// </summary>
        public static bool IfPreciseConfirm
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.PreciseConfirm].BlSelect;
            }
        }

        public static bool IfPassQrCode
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.PassQrCode].BlSelect;
            }
        }

        #endregion

        #region 普通调整值

        public static Point2D adjRollerLinePick
        {
            get
            {
                return new Point2D(ParAdjust.Value1(ES(ComAdj.AdjRollerLinePick)), ParAdjust.Value2(ES(ComAdj.AdjRollerLinePick)));
            }
        }
        public static Point2D adjQrCode
        {
            get
            {
                return new Point2D(ParAdjust.Value1(ES(ComAdj.QrCode)), ParAdjust.Value2(ES(ComAdj.QrCode)));
            }
        }
        public static double adjTPickPlat
        {
            get
            {
                return ParAdjust.Value1(ES(ComAdj.TPickPlat));
            }
        }

        public static Point2D rotateCenter
        {
            get
            {
                return new Point2D(ParAdjust.Value1(ES(ComAdj.RotateCenter)), ParAdjust.Value2(ES(ComAdj.RotateCenter)));
            }
        }

        #region
        ///// <summary>
        ///// 调整值-巡边平台1调整值xyzr
        ///// </summary>
        //static Point4D adjInspPosPlat1
        //{
        //    get
        //    {
        //        return ParBotAdj.P_I[(int)BotAdj.Platform1];
        //    }
        //}
        ///// <summary>
        ///// 调整值-巡边平台2调整值xyzr
        ///// </summary>
        //static Point4D adjInspPosPlat2
        //{
        //    get
        //    {
        //        return ParBotAdj.P_I[(int)BotAdj.Platform2];
        //    }
        //}

        ///// <summary>
        ///// 调整值-清晰度阈值
        ///// </summary>
        //static double adjSharpnessThread1
        //{
        //    get
        //    {               
        //        string key = key_adj_SharpnessRatio;
        //        return ParAdjust.Value1(key);
        //    }
        //}

        ///// <summary>
        ///// 调整值-清晰度阈值
        ///// </summary>
        //static double adjSharpnessThread2
        //{
        //    get
        //    {
        //        string key = key_adj_SharpnessRatio;
        //        return ParAdjust.Value2(key);
        //    }
        //}
        ///// <summary>
        ///// 调整值-残边平台1补偿Xyzr
        ///// </summary>
        //static Point4D adjPosWastagePlat1
        //{
        //    get
        //    {
        //        return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.Platform1]);
        //    }
        //}
        ///// <summary>
        ///// 调整值-残边平台1补偿Xyzr
        ///// </summary>
        //static Point4D adjPosWastagePlat2
        //{
        //    get
        //    {
        //        return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.Platform2]);
        //    }
        //}

        ///// <summary>
        ///// 调整值-抛料位补偿
        ///// </summary>
        //static Point4D adjDumpPos
        //{
        //    get
        //    {
        //        return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.DumpPos]);
        //    }
        //}

        ///// <summary>
        ///// 调整至-皮带线取片补偿
        ///// </summary>
        //static Point4D adjBeltPickPos
        //{
        //    get
        //    {
        //        return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.BeltPickPos]);
        //    }
        //}

        //public static Point3D adjInspCom
        //{
        //    get
        //    {
        //        string key = key_adj_insp;
        //        return new Point3D(ParAdjust.Value1(key_adj_insp), ParAdjust.Value2(key), ParAdjust.Value3(key));
        //    }
        //}

        //static double adjBeltRatio
        //{
        //    get
        //    {
        //        string key = key_adj_BeltRatio;
        //        return ParAdjust.Value1(key);
        //    }
        //}

        //static Point2D adjCodeCom
        //{
        //    get
        //    {
        //        string key = key_adj_CodeCom;
        //        return new Point2D(ParAdjust.Value1(key), ParAdjust.Value2(key));
        //    }
        //}
        #endregion
        #endregion

        #region 普通基准值
        public static Point2D stdPxlCamera1
        {
            get
            {
                return new Point2D(ParStd.Value1(ES(ComStd.StdPxlCamera1)), ParStd.Value2(ES(ComStd.StdPxlCamera1)));
            }
        }
        #endregion

        #region 机器人点位基准
        /// <summary>
        /// 基准值-上游平台取片基准值
        /// </summary>
        static Point4D stdPosUpStream
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.UpStreamPlat];
            }
        }
        /// <summary>
        /// 基准值-双目定位基准值
        /// </summary>
        static Point4D stdPosPrecise
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.PrecisePos];
            }
        }
        
        /// <summary>
        /// 基准值-下游平台放片基准值
        /// </summary>
        static Point4D stdPosDownStream
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.DownStreamPlat];
            }
        }

        /// <summary>
        /// 基准值-基准抛料位置
        /// </summary>
        static Point4D stdPosDump
        {
            get
            {
                 return ParBotStd.P_I[(int)BotStd.DumpPos];
            }
        }

        #endregion

        #region 机器人点位调整
        static Point4D adjPosUpStreamPlat
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.UpStreamPlat];
            }
        }
        static Point4D adjPosDownStreamPlat
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.DownStreamPlat];
            }
        }
        static Point4D adjPosDump
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.DumpPos];
            }
        }
        public static Point4D adjPosS1_1
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.St1_1];
            }
        }
        public static Point4D adjPosS1_2
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.St1_2];
            }
        }
        public static Point4D adjPosS2_1
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.St2_1];
            }
        }
        public static Point4D adjPosS2_2
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.St2_2];
            }
        }
        public static Point4D adjPosS3_1
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.St3_1];
            }
        }
        public static Point4D adjPosS3_2
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.St3_2];
            }
        }
        #endregion

        #endregion
    }
}
