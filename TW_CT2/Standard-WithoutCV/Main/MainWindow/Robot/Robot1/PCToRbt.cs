using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    public class PCToRbt
    {
        //PRP: PC Robot Protocol
        public const string PRP_PreciseConfirm = "21";
        public const string PRP_St = "22";
        public const string PRP_PreciseNG = "29";


        //RbtCalibSt
        public const string PRP_StartCalibSt = "200";
        public const string PRP_FinishCalibSt = "201";
        //RbtCalibRC
        public const string PRP_StartCalibRC = "205";
        public const string PRP_FinishCalibRC = "206";
        //RbtTeachXY
        public const string PRP_StartTeachXY = "210";
        public const string PRP_FinishTeachXY = "211";
        //RbtTeachZ
        public const string PRP_StartTeachZ = "215";
        public const string PRP_FinishTeachZ = "216";

        public const string PRP_Drive = "220";
    }
}
