using BasicClass;
using DealFile;
using System;
using System.IO;

namespace ModulePackage
{
    partial class ConfigManager
    {

        #region 读写接口
        /// <summary>
        /// 从配置文件读取保存在本地的配置
        /// </summary>
        public void LoadParams()
        {
            try
            {
                DirBotEnum = (DirBot_Enum)Enum.Parse(typeof(DirBot_Enum), GetConfig(ConfigParams.DirBotEnum.ToString()));
                DirDisplayEnum = (DirDisplay_Enum)Enum.Parse(typeof(DirDisplay_Enum), GetConfig(ConfigParams.DirDisplayEnum.ToString()));
                IsMirrorX = Boolean.Parse(GetConfig(ConfigParams.IsMirrorX.ToString()));
                IsMirrorY = Boolean.Parse(GetConfig(ConfigParams.IsMirrorY.ToString()));
                DirBLEnum = (DirBL_Enum)Enum.Parse(typeof(DirBL_Enum), GetConfig(ConfigParams.DirBLEnum.ToString()));
                PlatformPlacePosEnum = (PlatformPlacePos_Enum)Enum.Parse(typeof(PlatformPlacePos_Enum), GetConfig(ConfigParams.PlatformPlacePosEnum.ToString()));
                IsHorizontal = Boolean.Parse(GetConfig(ConfigParams.IsHorizontal.ToString()));
                DirCstCameraEnum = (DirCstCamera_Enum)Enum.Parse(typeof(DirCstCamera_Enum), GetConfig(ConfigParams.DirCstCameraEnum.ToString()));
                DirInsertEnum = (DirInsert_Enum)Enum.Parse(typeof(DirInsert_Enum), GetConfig(ConfigParams.DirInsertEnum.ToString()));
                TypeModuleZEnum = (TypeModuleZ_Enum)Enum.Parse(typeof(TypeModuleZ_Enum), GetConfig(ConfigParams.TypeModuleZEnum.ToString()));
                CstIsMirrorX = Boolean.Parse(GetConfig(ConfigParams.CstIsMirrorX.ToString()));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 封装个接口，少写几个变量，方便统一改动
        /// </summary>
        /// <param name="key">需要读取的变量</param>
        /// <param name="section">部分参数根据mode保存，也要根据mode读取</param>
        /// <returns></returns>
        private string GetConfig(string key, string section = commonSection)
        {
            try
            {
                return IniFile.I_I.ReadIniStr(section, key, "", Path_Config);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 将所有参数写入到ini，部分以mode为section保存，其余是公共参数
        /// </summary>
        public void WriteConfig()
        {
            if (!Directory.Exists(Path_Dir))
                Directory.CreateDirectory(Path_Dir);
            WriteConfig(ConfigParams.DirBotEnum.ToString(), DirBotEnum.ToString());
            WriteConfig(ConfigParams.DirDisplayEnum.ToString(), DirDisplayEnum.ToString());
            WriteConfig(ConfigParams.IsMirrorX.ToString(), IsMirrorX.ToString());
            WriteConfig(ConfigParams.IsMirrorY.ToString(), IsMirrorY.ToString());
            WriteConfig(ConfigParams.DirBLEnum.ToString(), DirBLEnum.ToString());
            WriteConfig(ConfigParams.PlatformPlacePosEnum.ToString(), PlatformPlacePosEnum.ToString());
            WriteConfig(ConfigParams.IsHorizontal.ToString(), IsHorizontal.ToString());
            WriteConfig(ConfigParams.DirCstCameraEnum.ToString(), DirCstCameraEnum.ToString());
            WriteConfig(ConfigParams.DirInsertEnum.ToString(), DirInsertEnum.ToString());
            WriteConfig(ConfigParams.TypeModuleZEnum.ToString(), TypeModuleZEnum.ToString());
            WriteConfig(ConfigParams.CstIsMirrorX.ToString(), CstIsMirrorX.ToString());
        }

        /// <summary>
        /// 将CIM参数记录到本地
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void WriteConfig(string key, string value, string section = commonSection)
        {
            try
            {
                IniFile.I_I.WriteIni(section, key, value, Path_Config);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }
        #endregion
    }
}
