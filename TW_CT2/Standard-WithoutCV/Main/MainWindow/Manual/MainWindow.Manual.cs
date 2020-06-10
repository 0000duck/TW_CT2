using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using Main_EX;
using DealConfigFile;

namespace Main
{
    public partial class MainWindow
    {
        /// <summary>
        /// 调用手动模拟运行
        /// </summary>
        public override void TriggerPC()
        {
            try
            {
                bool blNew = false;

                if (ParCameraWork.NumCamera > 4)
                {
                    WinTrrigerComprehensive win = WinTrrigerComprehensive.GetWinInst(out blNew);
                    if (blNew)
                    {
                        BaseDealComprehensiveResult[] baseDealComprehensiveResult = new BaseDealComprehensiveResult[8] {
                        DealComprehensiveResult1.D_I,
                        DealComprehensiveResult2.D_I ,
                        DealComprehensiveResult3.D_I,
                        DealComprehensiveResult4.D_I,
                        DealComprehensiveResult5.D_I,
                        DealComprehensiveResult6.D_I,
                        DealComprehensiveResult7.D_I,
                        DealComprehensiveResult8.D_I};
                        win.Init(g_UCStateWork, baseDealComprehensiveResult);
                    }
                    win.Show();
                }
                else
                {
                    WinTrrigerComprehensiveSmall win = WinTrrigerComprehensiveSmall.GetWinInst(out blNew);
                    if (blNew)
                    {
                        BaseDealComprehensiveResult[] baseDealComprehensiveResult = new BaseDealComprehensiveResult[4] {
                        DealComprehensiveResult1.D_I,
                        DealComprehensiveResult2.D_I ,
                        DealComprehensiveResult3.D_I,
                        DealComprehensiveResult4.D_I,
};
                        win.Init(g_UCStateWork, baseDealComprehensiveResult);
                    }
                    win.Show();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
