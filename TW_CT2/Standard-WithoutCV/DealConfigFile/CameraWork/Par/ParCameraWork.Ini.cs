using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealFile;
using BasicClass;
using Common;
using System.IO;

namespace DealConfigFile
{
    partial class ParCameraWork
    {
        #region 定义
        public string PathCameraWork
        {
            get
            {
                string path = ParPathRoot.PathRoot + "Store\\ParSysWork\\ParCameraWork.ini";
                if (!Directory.Exists(ParPathRoot.PathRoot + "Store\\ParSysWork\\"))
                {
                    Directory.CreateDirectory(ParPathRoot.PathRoot + "Store\\ParSysWork\\");
                }
                return path;
            }
        }
        #endregion 定义

        #region 读取Ini
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public bool ReadIniPar()
        {
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    //拍照次数
                    try
                    {
                        string numPhoto = IniFile.I_I.ReadIniStr("Cam" + (i + 1).ToString(), "NumPhoto", "Num4", PathCameraWork);
                        NumPhoto_e = (NumPhoto_enum)Enum.Parse(typeof(NumPhoto_enum), numPhoto);
                    }
                    catch (Exception ex)
                    {
                        NumPhoto_e = NumPhoto_enum.Num4;
                    }

                    //图像保存格式
                    try
                    {
                        string formatImage = IniFile.I_I.ReadIniStr("Cam" + (i + 1).ToString(), "FormatImage", "jpg90", PathCameraWork);
                        FormatImage_e = (FormatImage_enum)Enum.Parse(typeof(FormatImage_enum), formatImage);
                    }
                    catch (Exception ex)
                    {
                        FormatImage_e = FormatImage_enum.jpg100;
                    }

                    //坐标系
                    try
                    {
                        string typeImageCoord = IniFile.I_I.ReadIniStr("Cam" + (i + 1).ToString(), "TypeImageCoord", "LeftTop", PathCameraWork);
                        TypeImageCoord_e = (TypeImageCoord_enum)Enum.Parse(typeof(TypeImageCoord_enum), typeImageCoord);
                    }
                    catch (Exception ex)
                    {
                        FormatImage_e = FormatImage_enum.jpg90;
                    }


                    //采集类型
                    try
                    {
                        BlFull = IniFile.I_I.ReadIniBl("Cam" + (i + 1).ToString(), "BlFull", false, PathCameraWork);
                    }
                    catch (Exception ex)
                    {

                    }

                    //Bitmap金字塔分解
                    try
                    {
                        string fenjieBit = IniFile.I_I.ReadIniStr("Cam" + (i + 1).ToString(), "FenjieBit", "Num2", PathCameraWork);
                        FenjieBit_e = (NumPhoto_enum)Enum.Parse(typeof(NumPhoto_enum), fenjieBit);
                    }
                    catch (Exception ex)
                    {
                        FenjieBit_e = NumPhoto_enum.Num2;
                    }

                    Annotation = IniFile.I_I.ReadIniStr("Cam" + (i + 1).ToString(), "FormatImage", "Annotation", PathCameraWork);
                    ParCameraWork_L.Add(new ParCameraWork()
                    {
                        No = i + 1,
                        FormatImage_e = FormatImage_e,
                        NumPhoto_e = NumPhoto_e,
                        TypeImageCoord_e = TypeImageCoord_e,
                        BlFull = BlFull,
                        FenjieBit_e = FenjieBit_e,
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 读取相机个数
        /// </summary>
        public void ReadIniNumCamera()
        {
            try
            {

                NumCamera = IniFile.I_I.ReadIniInt("NumCamera", "Num", PathCameraWork);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 读取Ini

        #region 写入Ini
        /// <summary>
        /// 写入相机参数
        /// </summary>
        /// <returns></returns>
        public bool WriteIni()
        {
            try
            {
                for (int i = 0; i < ParCameraWork_L.Count; i++)
                {
                    IniFile.I_I.WriteIni("Cam" + (i + 1).ToString(), "NumPhoto", ParCameraWork_L[i].NumPhoto_e.ToString(), PathCameraWork);
                    IniFile.I_I.WriteIni("Cam" + (i + 1).ToString(), "FormatImage", ParCameraWork_L[i].FormatImage_e.ToString(), PathCameraWork);//图像保存类型
                    IniFile.I_I.WriteIni("Cam" + (i + 1).ToString(), "TypeImageCoord", ParCameraWork_L[i].TypeImageCoord_e.ToString(), PathCameraWork);//坐标类型
                    IniFile.I_I.WriteIni("Cam" + (i + 1).ToString(), "BlFull", ParCameraWork_L[i].BlFull.ToString(), PathCameraWork);//采集格式
                    IniFile.I_I.WriteIni("Cam" + (i + 1).ToString(), "FenjieBit", ParCameraWork_L[i].FenjieBit_e.ToString(), PathCameraWork);//Bitmap金字塔
                    IniFile.I_I.WriteIni("Cam" + (i + 1).ToString(), "Annotation", ParCameraWork_L[i].Annotation, PathCameraWork);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 相机个数
        /// </summary>
        /// <returns></returns>
        public bool WriteIniNumCamera()
        {
            try
            {
                IniFile.I_I.WriteIni("NumCamera", "Num", NumCamera.ToString(), PathCameraWork);

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 写入Ini
    }
}
