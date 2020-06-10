using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DealConfigFile
{
    /// <summary>
    /// add by xc-190401
    /// </summary>
    public class ParBotAdj : ParBase
    {
        public static ParBotAdj P_I = new ParBotAdj();

        /// <summary>
        /// 类名/文件名
        /// </summary>
        protected override string ClassName
        {
            get { return "RobotAdj"; }
        }
        /// <summary>
        /// 调整栏名字
        /// </summary>
        protected override string columnName
        {
            get { return "radj"; }
        }
        /// <summary>
        /// 索引器
        /// </summary>
        protected override string indexName { get { return "radj"; } }
        /// <summary>
        /// ini路径
        /// </summary>
        public override string FilePath
        {
            get
            {
                string root = ComConfigPar.C_I.PathConfigIni.Replace("Product.ini", "调整值");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                return root + "\\" + ClassName + ".ini";
            }
        }
    }
}
