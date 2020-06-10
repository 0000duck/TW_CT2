using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using DealLog;
using System.IO;
using System.Collections;
using BasicClass;
using DealResult;
using System.Diagnostics;
using Common;

namespace Main_EX
{
    partial class BaseDealComprehensiveResult
    {
        #region 定义
        string NameCell = "";
        double[] NumRunInfo
        {
            get
            {
                double[] num = new double[3];
                try
                {
                    RegeditFile r_I = new RegeditFile();
                    string str = r_I.ReadRegedit(NameCell);
                    if (str.Contains(","))
                    {
                        string[] strNum = str.Split(',');
                        if (strNum.Length == 3)
                        {
                            num[0] = int.Parse(strNum[0]);
                            num[1] = int.Parse(strNum[1]);
                            num[2] = int.Parse(strNum[2]);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return num;
            }
            set
            {
                RegeditFile r_I = new RegeditFile();
                string str = value[0].ToString() + "," + value[1].ToString() + "," + value[2].ToString();
                r_I.WriteRegedit(NameCell, str.ToString());
            }
        }

        //记录节拍
        protected Stopwatch sw_Tact = new Stopwatch();
        #endregion 定义

        #region 记录节拍
        /// <summary>
        /// 记录图像处理的节拍和整体的节拍
        /// </summary>
        protected void RecordTact(int noCamera, int pos, Hashtable htResult)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                sw_Tact.Stop();
                long tFull = sw_Tact.ElapsedMilliseconds;

                string root = ComValue.c_PathRecord + "RecordCamera\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + "Camera" + noCamera.ToString() + "-Pos" + pos.ToString() + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                t_I.WriteText(path, name);//写入时间

                //记录每个单子算子的时间
                List<int> key_L = new List<int>();
                foreach (string str in htResult.Keys)
                {
                    if (str.Contains("C"))
                    {
                        key_L.Add(int.Parse(str.Replace("C", "").Replace("T", "")));
                    }
                }
                key_L.Sort();

                //记录单元格运行时间
                foreach (int index in key_L)
                {
                    string nameCell = "C" + index.ToString();
                    if (htResult[nameCell] is BaseResultHObject)
                    {
                        if (((BaseResultHObject)htResult[nameCell]).Pos != pos)
                        {
                            continue;
                        }
                        //单元格运行时间
                        string cell = ((BaseResultHObject)htResult[nameCell]).NameCell + ((BaseResultHObject)htResult[nameCell]).Type;
                        double cellTime = ((BaseResultHObject)htResult[nameCell]).TactTime;
                        t_I.WriteText(path, cell + "=" + cellTime.ToString() + "ms");

                        //运行状态
                        NameCell = nameCell + "_Main";
                        BaseResult result = (BaseResult)htResult[nameCell];
                        double[] num = NumRunInfo;
                        if (result.BlResult)
                        {
                            num[0] = num[0] + 1;//OK
                        }
                        else
                        {
                            num[1] = num[1] + 1;//NG
                        }
                        num[2] = num[2] + 1;//Sum
                        NumRunInfo = num;
                        double co = 0;
                        if (num[2] != 0)
                        {
                            co = Math.Round((double)num[0] / (double)num[2], 2);
                        }
                        string type = result.LevelError_e.ToString() + "-" + result.TypeErrorProcess_e.ToString() + "-" + result.Annotation;
                        string info = "NumOK=" + num[0].ToString() + ";NumNG=" + num[1].ToString() + ";Sum=" + num[2].ToString() + ";OK/Sum=" + co.ToString() + "**Result:" + type;
                        t_I.WriteText(path, info);
                    }
                }

                //记录图像处理时间
                long tImageP = long.Parse(htResult["TimeImageP-Pos" + pos.ToString()].ToString());
                t_I.WriteText(path, "图像处理=" + tImageP.ToString());

                //记录显示时间
                long tDisplayImage = long.Parse(htResult["TimeDisplayImage-Pos" + pos.ToString()].ToString());
                long tDisplayROI = long.Parse(htResult["TimeDisplayROI-Pos" + pos.ToString()].ToString());
                long tDisplayShape = long.Parse(htResult["TimeDisplayShape-Pos" + pos.ToString()].ToString());
                long tDisplayInfoImage = long.Parse(htResult["TimeDisplayInfoImage-Pos" + pos.ToString()].ToString());
                long tDisplayInfo = long.Parse(htResult["TimeDisplayInfo-Pos" + pos.ToString()].ToString());
                long tDisplayAll = long.Parse(htResult["TimeDisplayAllTime-Pos" + pos.ToString()].ToString());

                string infoDisplay = string.Format("显示时间={5}:画面={0};  ROI={1};  形状={2};  画面信息={3};  信息={4}", tDisplayImage, tDisplayROI, tDisplayShape, tDisplayInfoImage, tDisplayInfo, tDisplayAll);
                t_I.WriteText(path, infoDisplay);

                //记录内存处理时间
                long tMemory = long.Parse(htResult["Memory-Rubbish" + pos.ToString()].ToString());
                t_I.WriteText(path, "垃圾清除+内存监控=" + tMemory);

                //记录整体节拍
                t_I.WriteText(path, "整体节拍" + tFull + "\n\r");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        protected void RecordTact(Stopwatch sw, int noCamera, int pos, Hashtable htResult)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                sw.Stop();
                long tFull = sw.ElapsedMilliseconds;

                string root = ComValue.c_PathRecord + "RecordCamera\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + "Camera" + noCamera.ToString() + "-Pos" + pos.ToString() + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                t_I.WriteText(path, name);//写入时间

                //记录每个单子算子的时间
                List<int> key_L = new List<int>();
                foreach (string str in htResult.Keys)
                {
                    if (str.Contains("C"))
                    {
                        key_L.Add(int.Parse(str.Replace("C", "").Replace("T", "")));
                    }
                }
                key_L.Sort();

                //记录单元格运行时间
                foreach (int index in key_L)
                {
                    string nameCell = "C" + index.ToString();
                    if (htResult[nameCell] is BaseResultHObject)
                    {
                        if (((BaseResultHObject)htResult[nameCell]).Pos != pos)
                        {
                            continue;
                        }
                        //单元格运行时间
                        string cell = ((BaseResultHObject)htResult[nameCell]).NameCell + ((BaseResultHObject)htResult[nameCell]).Type;
                        double cellTime = ((BaseResultHObject)htResult[nameCell]).TactTime;
                        t_I.WriteText(path, cell + "=" + cellTime.ToString() + "ms");

                        //运行状态
                        NameCell = nameCell + "_Main";
                        BaseResult result = (BaseResult)htResult[nameCell];
                        double[] num = NumRunInfo;
                        if (result.BlResult)
                        {
                            num[0] = num[0] + 1;//OK
                        }
                        else
                        {
                            num[1] = num[1] + 1;//NG
                        }
                        num[2] = num[2] + 1;//Sum
                        NumRunInfo = num;
                        double co = 0;
                        if (num[2] != 0)
                        {
                            co = Math.Round((double)num[0] / (double)num[2], 2);
                        }
                        string type = result.LevelError_e.ToString() + "-" + result.TypeErrorProcess_e.ToString() + "-" + result.Annotation;
                        string info = "NumOK=" + num[0].ToString() + ";NumNG=" + num[1].ToString() + ";Sum=" + num[2].ToString() + ";OK/Sum=" + co.ToString() + "**Result:" + type;
                        t_I.WriteText(path, info);
                    }
                }

                //记录图像处理时间
                long tImageP = long.Parse(htResult["TimeImageP-Pos" + pos.ToString()].ToString());
                t_I.WriteText(path, "图像处理=" + tImageP.ToString());

                //记录显示时间
                long tDisplayImage = long.Parse(htResult["TimeDisplayImage-Pos" + pos.ToString()].ToString());
                long tDisplayROI = long.Parse(htResult["TimeDisplayROI-Pos" + pos.ToString()].ToString());
                long tDisplayShape = long.Parse(htResult["TimeDisplayShape-Pos" + pos.ToString()].ToString());
                long tDisplayInfoImage = long.Parse(htResult["TimeDisplayInfoImage-Pos" + pos.ToString()].ToString());
                long tDisplayInfo = long.Parse(htResult["TimeDisplayInfo-Pos" + pos.ToString()].ToString());
                long tDisplayAll = long.Parse(htResult["TimeDisplayAllTime-Pos" + pos.ToString()].ToString());

                string infoDisplay = string.Format("显示时间={5}:画面={0};  ROI={1};  形状={2};  画面信息={3};  信息={4}", tDisplayImage, tDisplayROI, tDisplayShape, tDisplayInfoImage, tDisplayInfo, tDisplayAll);
                t_I.WriteText(path, infoDisplay);

                //记录内存处理时间
                long tMemory = long.Parse(htResult["Memory-Rubbish" + pos.ToString()].ToString());
                t_I.WriteText(path, "垃圾清除+内存监控=" + tMemory);

                //记录整体节拍
                t_I.WriteText(path, "整体节拍" + tFull + "\n\r");
                t_I.WriteText(path, "\n\r");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 记录节拍

        #region 屏幕截图OK
        /// <summary>
        /// OK图像，包含新的存储路径，图片名称
        /// </summary>
        protected void DumpWimImage_OK(string name, string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveOKImagePath(im, name, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 短时间，不包含年月日
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        protected void DumpWimImageShort_OK(string name, string path)
        {
            try
            {

                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveOKImagePath_Short(im, name, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// OK图像，包含新的存储路径，图片名称
        /// </summary>
        protected void DumpWimImage_OK(string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveOKImagePath(im, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 短时间，不包含年月日
        /// </summary>
        /// <param name="path"></param>
        protected void DumpWimImageShort_OK(string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveOKImagePath_Short(im, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 屏幕截图OK

        #region 屏幕截图NG
        /// <summary>
        ///NG图像，包含新的存储路径，图片名称
        /// </summary>
        protected void DumpWimImage_NG(string name, string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveNGImagePath(im, name, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 短时间，不包含年月日
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        protected void DumpWimImageShort_NG(string name, string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveNGImagePath_Short(im, name, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 新的存储路径文件夹
        /// </summary>
        /// <param name="path"></param>
        protected void DumpWimImage_NG(string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveNGImagePath(im, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 短时间
        /// </summary>
        /// <param name="path"></param>
        protected void DumpWimImageShort_NG(string path)
        {
            try
            {
                ImageAll im = g_UCDisplayCamera.DumpWinImage();//截屏图片

                g_UCDisplayCamera.SaveNGImagePath_Short(im, path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 屏幕截图NG


        #region 记录日志
        /// <summary>
        /// 添加通用日志
        /// </summary>
        /// <param name="info"></param>
        public static void ShowState(string info)
        {
            try
            {
                g_UCStateWork.AddInfo(info, TypeLog_enum.Com);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
            }
        }

        /// <summary>
        /// 添加相机日志
        /// </summary>
        /// <param name="info"></param>
        public void ShowState_Cam(string info)
        {
            try
            {
                string cam = "Cam" + g_NoCamera.ToString();
                TypeLog_enum typeLog_e = (TypeLog_enum)Enum.Parse(typeof(TypeLog_enum), cam);
                g_UCStateWork.AddInfo(info, typeLog_e);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 添加相机日志
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="noCamera">相机序号</param>
        public void ShowState_Cam(string info,int noCamera)
        {
            try
            {
                string cam = "Cam" + noCamera.ToString();
                TypeLog_enum typeLog_e = (TypeLog_enum)Enum.Parse(typeof(TypeLog_enum), cam);
                g_UCStateWork.AddInfo(info, typeLog_e);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// PLC日志
        /// </summary>
        /// <param name="info"></param>
        public void ShowState_PLC(string info)
        {
            try
            {
                g_UCStateWork.AddInfo(info, TypeLog_enum.PLC);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// Robot日志
        /// </summary>
        /// <param name="info"></param>
        public void ShowState_Robot(string info)
        {
            try
            {
                g_UCStateWork.AddInfo(info, TypeLog_enum.Robot);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 添加报警
        /// </summary>
        /// <param name="info"></param>
        public static void ShowAlarm(string info)
        {
            try
            {
                g_UCAlarm.AddInfo(info, TypeLog_enum.Com);
                //g_UCStateWork.AddInfo(info, TypeLog_enum.Com);
                ShowState(info);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
            }
        }


        /// <summary>
        /// 添加相机报警日志
        /// </summary>
        /// <param name="info"></param>
        public void ShowAlarm_Cam(string info)
        {
            try
            {
                string cam = "Cam" + g_NoCamera.ToString();
                TypeLog_enum typeLog_e = (TypeLog_enum)Enum.Parse(typeof(TypeLog_enum), cam);
                g_UCAlarm.AddInfo(info, typeLog_e);
                g_UCStateWork.AddInfo(info, typeLog_e);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 添加相机报警日志
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="noCamera">相机序号</param>

        public void ShowAlarm_Cam(string info,int noCamera)
        {
            try
            {
                string cam = "Cam" + noCamera.ToString();
                TypeLog_enum typeLog_e = (TypeLog_enum)Enum.Parse(typeof(TypeLog_enum), cam);
                g_UCAlarm.AddInfo(info, typeLog_e);
                g_UCStateWork.AddInfo(info, typeLog_e);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 添加PLC报警日志
        /// </summary>
        /// <param name="info"></param>
        public void ShowAlarm_PLC(string info)
        {
            try
            {
                g_UCAlarm.AddInfo(info, TypeLog_enum.PLC);
                g_UCStateWork.AddInfo(info, TypeLog_enum.PLC);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 添加Robot报警日志
        /// </summary>
        /// <param name="info"></param>
        public void ShowAlarm_Robot(string info)
        {
            try
            {
                g_UCAlarm.AddInfo(info, TypeLog_enum.Robot);
                g_UCStateWork.AddInfo(info, TypeLog_enum.Robot);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 记录日志

    }
}
