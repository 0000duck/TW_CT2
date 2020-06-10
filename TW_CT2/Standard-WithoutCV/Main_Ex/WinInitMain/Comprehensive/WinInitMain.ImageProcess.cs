
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealComprehensive;
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using DealConfigFile;
using SetPar;
using HalconDotNet;
using Common;
using DealImageProcess;
using System.Windows;

namespace Main_EX
{
    public partial class WinInitMain
    {
        #region 定义
        public static bool BlFinishTemp = false;
        #endregion 定义

        #region 模板
        /// <summary>
        /// 统计预加载窗体类型
        /// </summary>
        /// <param name="blScale"></param>
        /// <param name="blNcc"></param>
        /// <param name="blScaleT"></param>
        /// <param name="blNccT"></param>
        public void CountWinTemp(out bool blScale, out bool blNcc, out bool blScaleT, out bool blNccT)
        {
            int numScale = 0;
            int numNcc = 0;
            int numScaleT = 0;
            int numNccT = 0;

            blScale = false;
            blNcc = false;
            blScaleT = false;
            blNccT = false;
            try
            {
                #region 相机1
                DealComprehensive1.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                if (blScale)
                {
                    numScale++;
                }
                if (blNcc)
                {
                    numNcc++;
                }
                if (blScaleT)
                {
                    numScaleT++;
                }
                if (blNccT)
                {
                    numNccT++;
                }
                #endregion 相机1

                #region 相机2
                if (ParCameraWork.NumCamera > 1)
                {
                    DealComprehensive2.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机2

                #region 相机3
                if (ParCameraWork.NumCamera > 2)
                {
                    DealComprehensive3.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机3

                #region 相机4
                if (ParCameraWork.NumCamera > 3)
                {
                    DealComprehensive4.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机4

                #region 相机5
                if (ParCameraWork.NumCamera > 4)
                {
                    DealComprehensive5.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机5

                #region 相机6
                if (ParCameraWork.NumCamera > 5)
                {
                    DealComprehensive6.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机6

                #region 相机7
                if (ParCameraWork.NumCamera > 6)
                {
                    DealComprehensive7.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机7

                #region 相机8
                if (ParCameraWork.NumCamera > 7)
                {
                    DealComprehensive8.D_I.CountWinTemp(out blScale, out blNcc, out blScaleT, out blNccT);
                    if (blScale)
                    {
                        numScale++;
                    }
                    if (blNcc)
                    {
                        numNcc++;
                    }
                    if (blScaleT)
                    {
                        numScaleT++;
                    }
                    if (blNccT)
                    {
                        numNccT++;
                    }
                }
                #endregion 相机8

                if (numScale > 0)
                {
                    blScale = true;
                }
                if (numNcc > 0)
                {
                    blNcc = true;
                }
                if (numScaleT > 0)
                {
                    blScaleT = true;
                }
                if (numNccT > 0)
                {
                    blNccT = true;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 加载对应的模板
        /// </summary>
        public void CreateTemp()
        {
            string cellError = "";

            try
            {
                #region 相机1
                switch (DealComprehensive1.D_I.CreateTemplate(out cellError))
                {
                    case StateTemplate_enum.Null:
                        break;

                    case StateTemplate_enum.False:
                        ShowWinError_Invoke("相机1模板加载失败,模板单元:" + cellError + "可重启软件！");
                        break;

                    case StateTemplate_enum.True:
                        ShowState_Cam("相机1模板加载成功");
                        break;
                }
                #endregion 相机1

                #region 相机2
                if (ParCameraWork.NumCamera > 1)
                {
                    switch (DealComprehensive2.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机2模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机2模板加载成功");
                            break;
                    }
                }
                #endregion 相机2

                #region 相机3
                if (ParCameraWork.NumCamera > 2)
                {
                    switch (DealComprehensive3.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机3模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机3模板加载成功");
                            break;
                    }
                }
                #endregion 相机3

                #region 相机4
                if (ParCameraWork.NumCamera > 3)
                {
                    switch (DealComprehensive4.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机4模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机4模板加载成功");
                            break;
                    }
                }

                #endregion 相机4

                #region 相机5
                if (ParCameraWork.NumCamera > 4)
                {
                    switch (DealComprehensive5.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机5模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机5模板加载成功");
                            break;
                    }
                }
                #endregion 相机5

                #region 相机6
                if (ParCameraWork.NumCamera > 5)
                {
                    switch (DealComprehensive6.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机6模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机6模板加载成功");
                            break;
                    }
                }
                #endregion 相机6

                #region 相机7
                if (ParCameraWork.NumCamera > 6)
                {
                    switch (DealComprehensive7.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机7模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机7模板加载成功");
                            break;
                    }
                }

                #endregion 相机7

                #region 相机8
                if (ParCameraWork.NumCamera > 7)
                {
                    switch (DealComprehensive8.D_I.CreateTemplate(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机8模板加载失败,模板单元:" + cellError + "可重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机8模板加载成功");
                            break;
                    }
                }
                #endregion 相机8
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 显示模板加载结果
        /// </summary>
        void ShowResultTemp()
        {
            try
            {
                int count = 0;
                while (!BlFinishTemp)
                {
                    Thread.Sleep(500);
                    count++;

                    if (count > 30)
                    {
                        ShowWinError_Invoke("模板加载超时");
                        break;
                    }
                }

                switch (DealComprehensive1.D_I.StateTemplate_e)
                {
                    case StateTemplate_enum.Null:
                        break;

                    case StateTemplate_enum.False:
                        ShowWinError_Invoke("相机1模板加载失败,模板单元:" + DealComprehensive1.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                        break;

                    case StateTemplate_enum.True:
                        ShowState_Cam("相机1模板加载成功");
                        break;
                }

                if (ParCameraWork.NumCamera > 1)
                {
                    switch (DealComprehensive2.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机2模板加载失败,模板单元:" + DealComprehensive2.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机2模板加载成功");
                            break;
                    }
                }


                if (ParCameraWork.NumCamera > 2)
                {
                    switch (DealComprehensive3.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机3模板加载失败,模板单元:" + DealComprehensive3.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机3模板加载成功");
                            break;
                    }
                }

                if (ParCameraWork.NumCamera > 3)
                {
                    switch (DealComprehensive4.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机4模板加载失败,模板单元:" + DealComprehensive4.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机4模板加载成功");
                            break;
                    }
                }

                if (ParCameraWork.NumCamera > 4)
                {
                    switch (DealComprehensive5.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机5模板加载失败,模板单元:" + DealComprehensive5.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机5模板加载成功");
                            break;
                    }
                }

                if (ParCameraWork.NumCamera > 5)
                {
                    switch (DealComprehensive6.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机6模板加载失败,模板单元:" + DealComprehensive6.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机6模板加载成功");
                            break;
                    }
                }

                if (ParCameraWork.NumCamera > 6)
                {
                    switch (DealComprehensive7.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机7模板加载失败,模板单元:" + DealComprehensive7.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机7模板加载成功");
                            break;
                    }
                }

                if (ParCameraWork.NumCamera > 7)
                {
                    switch (DealComprehensive8.D_I.StateTemplate_e)
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机8模板加载失败,模板单元:" + DealComprehensive8.D_I.ErrorTemp + "检查模板图片,并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机8模板加载成功");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 初始化模板窗体
        /// </summary>
        /// <param name="blScale"></param>
        /// <param name="blNcc"></param>
        /// <param name="blScaleT"></param>
        /// <param name="blNccT"></param>
        public void InitTempWin(bool blScale, bool blNcc, bool blScaleT, bool blNccT)
        {
            try
            {
                ShowState("预加载模板窗体");
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    bool blNew = false;
                    if (blScale)
                    {
                        WinTempScaledShape.GetWinInst(out blNew).Show();
                        WinTempScaledShape.GetWinInst().Hide();
                        //WinTempScaledShape.GetWinInst().Topmost = true;
                    }
                    if (blNcc)
                    {
                        WinTempNcc.GetWinInst(out blNew).Show();
                        WinTempNcc.GetWinInst().Hide();
                        //WinTempNcc.GetWinInst().Topmost = true;
                    }
                    if (blScaleT)
                    {
                        WinScaledShapeT.GetWinInst(out blNew).Show();
                        WinScaledShapeT.GetWinInst().Hide();
                        //WinTempScaledShape.GetWinInst().Topmost = true;
                    }
                    if (blNccT)
                    {
                        WinNccT.GetWinInst(out blNew).Show();
                        WinNccT.GetWinInst().Hide();
                        //WinTempNcc.GetWinInst().Topmost = true;
                    }
                }));

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {

            }
        }
        #endregion 模板

        #region 校准
        /// <summary>
        /// 加载校准
        /// </summary>
        public void InitCalib()
        {
            try
            {
                string cellError = "";
                #region 相机1
                switch (DealComprehensive1.D_I.InitCalib(out cellError))
                {
                    case StateTemplate_enum.Null:
                        break;

                    case StateTemplate_enum.False:
                        ShowWinError_Invoke("相机1校准加载失败:" + cellError + "检查校准文件，并重启软件！");
                        break;

                    case StateTemplate_enum.True:
                        ShowState_Cam("相机1校准加载成功");
                        break;
                }
                #endregion 相机1

                #region 相机2
                if (ParCameraWork.NumCamera > 1)
                {
                    switch (DealComprehensive2.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机2校准加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机2校准加载成功");
                            break;
                    }
                }
                #endregion 相机2

                #region 相机3
                if (ParCameraWork.NumCamera > 2)
                {
                    switch (DealComprehensive3.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机3模板加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机3校准加载成功");
                            break;
                    }
                }
                #endregion 相机3

                #region 相机4
                if (ParCameraWork.NumCamera > 3)
                {
                    switch (DealComprehensive4.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机4模板加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机4校准加载成功");
                            break;
                    }
                }
                #endregion 相机4

                #region 相机5
                if (ParCameraWork.NumCamera > 4)
                {
                    switch (DealComprehensive5.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机5校准加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机5校准加载成功");
                            break;
                    }
                }
                #endregion 相机5

                #region 相机6
                if (ParCameraWork.NumCamera > 5)
                {
                    switch (DealComprehensive6.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机6校准加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机6校准加载成功");
                            break;
                    }
                }
                #endregion 相机6

                #region 相机7
                if (ParCameraWork.NumCamera > 6)
                {
                    switch (DealComprehensive7.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机7校准加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机7校准加载成功");
                            break;
                    }
                }
                #endregion 相机7

                #region 相机8
                if (ParCameraWork.NumCamera > 7)
                {
                    switch (DealComprehensive8.D_I.InitCalib(out cellError))
                    {
                        case StateTemplate_enum.Null:
                            break;

                        case StateTemplate_enum.False:
                            ShowWinError_Invoke("相机8校准加载失败:" + cellError + "检查校准文件，并重启软件！");
                            break;

                        case StateTemplate_enum.True:
                            ShowState_Cam("相机8校准加载成功");
                            break;
                    }
                }
                #endregion 相机8
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 校准

        #region 事件注销
        public void EventLogout_ImageProcess()
        {

        }
        #endregion 事件注销

    }
}
