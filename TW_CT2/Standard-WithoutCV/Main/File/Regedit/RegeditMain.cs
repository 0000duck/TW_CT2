using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;

namespace Main
{
    public partial class RegeditMain:RegeditFile
    {
        #region 静态类实例
        public static new RegeditMain R_I = new RegeditMain();
        #endregion 静态类实例

        public int PickCnt
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("PickCnt"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("PickCnt", value.ToString());
            }
        }

        #region id
        public int ID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("ID"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("ID", value.ToString());
            }
        }
        #endregion

        #region 报表
        public int PreciseNG
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("PreciseNG"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("PreciseNG", value.ToString());
            }
        }

        public int PreciseSUM
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("PreciseSUM"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("PreciseSUM", value.ToString());
            }
        }

        public int WastageNG1
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("WastageNG1"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("WastageNG1", value.ToString());
            }
        }

        public int WastageNG2
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ReadRegedit("WastageNG2"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                WriteRegedit("WastageNG2", value.ToString());
            }
        }
        #endregion
    }
}
