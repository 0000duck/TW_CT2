
using BasicClass;
using ModulePackage;
using StationDataManager;
using System.Text.RegularExpressions;

namespace Main
{
    public partial class ModelParams
    {

        #region 机器人实际点位
        //机器人角度固定不变，永不修改
        public const double RU_Precise = 0;

        public const double RU_LeftAOI = -90;

        public const double RU_MidAOI = -90;

        public const double RU_RightAOI = -135;

        public const double RU_PickPlat1 = 0;

        public const double RU_PlacePlat2 = -180;

        public const double RU_Dump = 90;

        public static Point4D pPickPlat1
        {
            get
            {
                return (stdPosUpStream + adjPosUpStreamPlat);
            }
        }
        public static Point4D pPrecise
        {
            get
            {
                Point4D p = stdPosPrecise;
                p.DblValue1 += (GlassX_Precise / 2 - DisMark / 2 - MarkL_Precise.DblValue1);
                p.DblValue2 += (GlassY_Precise / 2 - MarkL_Precise.DblValue2);
                return p;
            }
        }
        public static Point4D pPlacePlat2
        {
            get
            {
                return (stdPosDownStream + adjPosDownStreamPlat);
            }
        }
        public static Point4D pDump
        {
            get
            {
                return (stdPosDump + adjPosDump);
            }
        }
        public static Point4D pPlaceAOI
        {
            set;
            get;
        }

        public static Point4D[] pStArr = new Point4D[] { pSt1_1, pSt1_2, pSt2_1, pSt2_2, pSt3_1, pSt3_2 };

        public static Point4D pSt1_1
        {
            get
            {
                Point4D p = StationDataMngr.instance.PlacePos_L[0];
                p.DblValue3 += adjPosS1_1.DblValue3;
                p.DblValue4 = RU_LeftAOI;
                return p;
            }
        }

        public static Point4D pSt1_2
        {
            get
            {
                Point4D p = StationDataMngr.instance.PlacePos_L[1];
                p.DblValue3 += adjPosS1_2.DblValue3;
                p.DblValue4 = RU_LeftAOI;
                return p;
            }
        }

        public static Point4D pSt2_1
        {
            get
            {
                Point4D p = StationDataMngr.instance.PlacePos_L[2];
                p.DblValue3 += adjPosS2_1.DblValue3;
                p.DblValue4 = RU_MidAOI;
                return p;
            }
        }

        public static Point4D pSt2_2
        {
            get
            {
                Point4D p = StationDataMngr.instance.PlacePos_L[3];
                p.DblValue3 += adjPosS2_2.DblValue3;
                p.DblValue4 = RU_MidAOI;
                return p;
            }
        }

        public static Point4D pSt3_1
        {
            get
            {
                Point4D p = StationDataMngr.instance.PlacePos_L[4];
                p.DblValue3 += adjPosS3_1.DblValue3;
                p.DblValue4 = RU_RightAOI;
                return p;
            }
        }

        public static Point4D pSt3_2
        {
            get
            {
                Point4D p = StationDataMngr.instance.PlacePos_L[5];
                p.DblValue3 += adjPosS3_2.DblValue3;
                p.DblValue4 = RU_RightAOI;
                return p;
            }
        }
        #endregion

        #region 玻璃需要逆时针转的度数，使得玻璃的某一边朝前
        public static double GlassAngle_Precise
        {
            get
            {
                switch(MarkDirection)
                {
                    case 1:
                        return 180;
                    case 2:
                        return 0;
                    case 3:
                        return 90;
                    case 4:
                        return 270;
                    default:
                        return 0;
                }
            }
        }

        public static double GlassAngle_MidAOI
        {
            get
            {
                switch (confDirAOI)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 270;
                    case 3:
                        return 180;
                    case 4:
                        return 90;
                    default:
                        return 0;
                }
            }
        }

        public static double GlassAngle_LeftAOI
        {
            get
            {
                //return (GlassAngle_MidAOI + 45);
                return (GlassAngle_MidAOI );
            }
        }

        public static double GlassAngle_RightAOI
        {
            get
            {
                //return (GlassAngle_MidAOI - 45);
                return (GlassAngle_MidAOI );
            }
        }

        public static double GlassAngle_DownStream
        {
            get
            {
                switch (confDirDownStream)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 270;
                    case 3:
                        return 180;
                    case 4:
                        return 90;
                    default:
                        return 0;
                }
            }
        }

        public static double GlassAngle_Dump
        {
            get
            {
                switch (confDirDump)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 270;
                    case 3:
                        return 180;
                    case 4:
                        return 90;
                    default:
                        return 0;
                }
            }
        }
        #endregion

        #region T轴
        const string strT_roller = "T_Roller";
        public static double T_Roller
        {
            set
            {
                RegeditMain.R_I.WriteRegedit(strT_roller, value.ToString());
            }
            get
            {
                return RegeditMain.R_I.ReadRegeditDbl(strT_roller);
            }
        }
        const string strT_move = "T_Move";
        public static double T_Move
        {
            set
            {
                RegeditMain.R_I.WriteRegedit(strT_move, value.ToString());
            }
            get
            {
                return RegeditMain.R_I.ReadRegeditDbl(strT_move);
            }
        }
        const string strT_pickplat1 = "T_PickPlat1";
        public static double T_PickPlat1
        {
            set
            {
                RegeditMain.R_I.WriteRegedit(strT_pickplat1, value.ToString());
            }
            get
            {
                return RegeditMain.R_I.ReadRegeditDbl(strT_pickplat1);
            }
        }

        public static double T_stdDownStream
        {
            get
            {
                return (GlassAngle_DownStream + RU_PlacePlat2 + 720) % 360;
            }
        }

        /// <summary>
        /// T轴精定位角度。
        /// </summary>
        public static double T_stdPrecise
        {
            get
            {
                //需要逆时针转的度数-机器人已经逆时针转的度数 = 还需要逆时针转的度数，加负号 = 还需要顺时针转的度数
                return (GlassAngle_Precise + RU_Precise + 720) % 360;
            }
        }
        /// <summary>
        /// T轴左侧工位基准角度
        /// </summary>
        public static double T_stdLeftAOI
        {
            get
            {
                return (GlassAngle_LeftAOI + RU_LeftAOI + 720) % 360;
            }
        }
        /// <summary>
        /// T轴中间工位基准角度
        /// </summary>
        public static double T_stdMidAOI
        {
            get
            {
                return (GlassAngle_MidAOI + RU_MidAOI + 720) % 360;
            }
        }
        /// <summary>
        /// T轴右侧工位基准角度
        /// </summary>
        public static double T_stdRightAOI
        {
            get
            {
                return (GlassAngle_RightAOI + RU_RightAOI + 720) % 360;
            }
        }
        public static double T_stdDump
        {
            get
            {
                return (GlassAngle_Dump + RU_Dump + 720) % 360;
            }
        }
        public static double T_realAOI
        {
            set;
            get;
        }

        #endregion
    }
}
