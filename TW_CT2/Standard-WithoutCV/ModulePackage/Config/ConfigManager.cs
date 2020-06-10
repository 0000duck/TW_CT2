using BasicClass;
using System.ComponentModel;

namespace ModulePackage
{
    public partial class ConfigManager : INotifyPropertyChanged
    {
        public static ConfigManager instance = new ConfigManager();
        const string ClassName = "ConfigManager";
        const string commonSection = "ConifgManager";
        string Path_Dir => ParPathRoot.PathRoot + @"Store\Custom\Module\";
        string Path_Config => Path_Dir + "config.ini";
        #region variables
        /// <summary>
        /// 机器人放置方向
        /// </summary>
        public DirBot_Enum DirBotEnum { get { return _dirBotEnum; } set { _dirBotEnum = value; NotifyPropertyChanged("DirBotEnum"); } }
        DirBot_Enum _dirBotEnum = DirBot_Enum.Forward;
        /// <summary>
        /// 画面显示方向
        /// </summary>
        public DirDisplay_Enum DirDisplayEnum { get { return _dirDisplayEnum; } set { _dirDisplayEnum = value; NotifyPropertyChanged("DirDisplayEnum"); } }
        DirDisplay_Enum _dirDisplayEnum = DirDisplay_Enum.正常;
        /// <summary>
        /// 画面X是否镜像
        /// </summary>
        public bool IsMirrorX { get { return _isMirrorX; } set { _isMirrorX = value; NotifyPropertyChanged("IsMirrorX"); } }
        bool _isMirrorX = true;
        /// <summary>
        /// 画面Y是否镜像
        /// </summary>
        public bool IsMirrorY { get { return _isMirrorY; } set { _isMirrorY = value; NotifyPropertyChanged("IsMirrorY"); } }
        public bool _isMirrorY = true;
        /// <summary>
        /// 背光放置方向
        /// </summary>
        public DirBL_Enum DirBLEnum { get { return _dirBLEnum; } set { _dirBLEnum = value; NotifyPropertyChanged("DirBLEnum"); } }
        DirBL_Enum _dirBLEnum = DirBL_Enum.正常;
        /// <summary>
        /// 工位1平台放片位置
        /// </summary>
        public PlatformPlacePos_Enum PlatformPlacePosEnum { get { return _platformPlacePosEnum; } set { _platformPlacePosEnum = value; NotifyPropertyChanged("PlatformPlacePosEnum"); } }
        PlatformPlacePos_Enum _platformPlacePosEnum = PlatformPlacePos_Enum.LeftTop;
        /// <summary>
        /// 单电极玻璃在平台1放置时，电极的朝向
        /// </summary>
        public bool IsHorizontal { get { return _isHorizontal; } set { _isHorizontal = value; NotifyPropertyChanged("IsHorizontal"); } }
        bool _isHorizontal = true;
        /// <summary>
        /// 卡塞相机安装方向
        /// </summary>
        public DirCstCamera_Enum DirCstCameraEnum { get { return _dirCstCameraEnum; } set { _dirCstCameraEnum = value; NotifyPropertyChanged("DirCstCameraEnum"); } }
        DirCstCamera_Enum _dirCstCameraEnum = DirCstCamera_Enum.Backward;
        /// <summary>
        /// 卡塞插栏方向
        /// </summary>
        public DirInsert_Enum DirInsertEnum { get { return _dirInsertEnum; } set { _dirInsertEnum = value; NotifyPropertyChanged("DirInsertEnum"); } }
        DirInsert_Enum _dirInsertEnum = DirInsert_Enum.NToP;
        /// <summary>
        /// z轴补偿所使用的机构种类
        /// </summary>
        public TypeModuleZ_Enum TypeModuleZEnum { get { return _typeModuleZEnum; } set { _typeModuleZEnum = value; NotifyPropertyChanged("TypeZEnum"); } }
        TypeModuleZ_Enum _typeModuleZEnum = TypeModuleZ_Enum.ModuleUp;
        /// <summary>
        /// 画面X是否镜像
        /// </summary>
        public bool CstIsMirrorX { get { return _cstIsMirrorX; } set { _cstIsMirrorX = value; NotifyPropertyChanged("CstIsMirrorX"); } }
        bool _cstIsMirrorX = true;

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
