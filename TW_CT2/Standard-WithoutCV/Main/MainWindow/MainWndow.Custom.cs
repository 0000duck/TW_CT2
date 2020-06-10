using BasicClass;
using CalibDataManager;
using ModulePackage;
using System;
using System.Threading;
using System.Threading.Tasks;
using StationDataManager;

namespace Main
{
    public partial class MainWindow
    {
        #region 定义
        bool g_BlClearAuto = false;//是否已经清除
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 初始化参数
        /// </summary>
        public override void Init_Custom()
        {
            try
            {
                //StationDataMngr.instance.read_station_data();

                ConfigManager.instance.LoadParams();

                MsgManager.ShowState += new StrAction(ShowState);
                MsgManager.ShowAlarm += new StrAction(ShowAlarm);

                BaseDealComprehensiveResult_Main.StopBelt += new Action(StopBeltScan);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 换型时需要处理的
        /// </summary>
        public override void InitNewModel_Custom()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void StopBeltScan()
        {
            new Task(() =>
            {
                if (beltScan != null && !beltScan.IsCanceled)
                {
                    cts.Cancel();
                    Task.WaitAll(beltScan);
                    ShowState("皮带线扫描停止");
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                }
            }).Start();

        }
        #endregion 初始化

        #region 显示
        /// <summary>
        /// 自定义显示
        /// </summary>
        public override void ShowCustom()
        {

        }
        #endregion 显示

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        public override void Close_Custom()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        #endregion 关闭
    }
}
