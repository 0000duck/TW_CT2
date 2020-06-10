using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;

namespace Main
{
    partial class RegeditMain
    {
        //chipid list中保存的id数
        public int ChipIDCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("ChipIDCount"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("ChipIDCount", value.ToString());
            }
        }

        public int LotNum
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("LotNum"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("LotNum", value.ToString());
            }
        }

        public string CodeArm
        {
            get
            {
                try
                {
                    return ReadRegedit("CodeArm");
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                WriteRegedit("CodeArm", value);
            }
        }

        public string CodeFork
        {
            get
            {
                try
                {
                    return ReadRegedit("CodeFork");
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                WriteRegedit("CodeFork", value);
            }
        }
    }
}
