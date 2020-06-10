using BasicClass;
using DealFile;
using System;

namespace DealConfigFile
{
    /// <summary>
    /// add by xc-190401
    /// </summary>
    public class ParBotStd : ParBase
    {
        public static ParBotStd P_I = new ParBotStd();
        /// <summary>
        /// 类名/文件名
        /// </summary>
        protected override string ClassName
        {
            get { return "RobotStd"; }
        }
        /// <summary>
        /// 调整栏名字
        /// </summary>
        protected override string columnName
        {
            get { return "rstd"; }
        }
        /// <summary>
        /// 索引名
        /// </summary>
        protected override string indexName { get { return "rstd"; } }
    }
}
