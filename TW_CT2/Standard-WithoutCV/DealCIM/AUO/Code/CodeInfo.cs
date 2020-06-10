using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using BasicClass;
using System.Threading;
using DealFile;

namespace DealCIM
{
    /// <summary>
    /// 用这个类来存储机台流动中的所有的二维码
    /// </summary>
    public class CodeInfo
    {
        #region 定义

        #region 刚读出来
        public static CodeInfo g_CodeRaw = new CodeInfo("CodeRaw");
        #endregion

        #region 交接到平台
        public static CodeInfo g_CodePrecisePlat = new CodeInfo("CodePrecisePlat");
        #endregion

        public string NameClass = "";
        public string StrCode
        {
            get
            {
                return RegeditFile.R_I.ReadRegedit(NameClass);
            }
            set
            {
                RegeditFile.R_I.WriteRegedit(NameClass,value);
            }
        }
        public int TargetIndex { get; set; }
        public bool BlResult { get; set; }

        public static event StrAction RefreshCode_Event;

        #endregion 定义

        #region 初始化
        public CodeInfo()
        {

        }
        public CodeInfo(string name)
        {
            NameClass = name;
        }

        #endregion 初始化

        public void SetCode(string code)
        {
            try
            {
                StrCode = code;
                if (StrCode != "" && !StrCode.Contains('\n')&& !StrCode.Contains('\r')&& !StrCode.Contains('?'))
                {
                    BlResult = true;
                }
         //       RefreshCode_Event(StrCode);                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CodeInfo", ex);
            }
        }

        public void TransCode(CodeInfo codeInfo)
        {
            try
            {
                if (StrCode == "Code")
                {
                    System.Windows.Forms.MessageBox.Show("二维码为空");
                }

                StrCode = codeInfo.StrCode;
                TargetIndex = codeInfo.TargetIndex;
                BlResult = codeInfo.BlResult;
                codeInfo.Init();

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CodeInfo", ex);
            }
        }

        void Init()
        {
            try
            {
                StrCode = "";
                BlResult = false;
                TargetIndex = 0;

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CodeInfo", ex);
            }
        }
    }
}
