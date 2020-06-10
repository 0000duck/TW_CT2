using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Common;
using DealFile;
using BasicClass;
using System.IO;

namespace DealConfigFile
{
    public partial class ParConfigPar : BaseClass
    {
        #region 静态类实例
        public static ParConfigPar P_I = new ParConfigPar();
        #endregion 静态类实例

        #region 定义      
        //int       
        //配置参数个数
        public const int c_NumParProduct = 60; //产品参数最多30个

        //bool
        public bool BlShowNo = false;//是否显示列表序号

        //List
        public List<ParProduct> ParProduct_L = new List<ParProduct>();//产品参数
        //public List<PosPhoto> PosPhoto_L = new List<PosPhoto>();//拍照位置

        // <summary>
        /// 索引器,获取产品参数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ParProduct this[int index]
        {
            get
            {
                try
                {
                    return ParProduct_L[index];
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError("ParConfigPar", ex);
                    return null;
                }
            }
        }

        ///// <summary>
        ///// 索引器来获取拍照位置
        ///// </summary>
        ///// <param name="noCamera"></param>
        ///// <param name="pos"></param>
        ///// <returns></returns>
        //public virtual PosPhoto this[int noCamera, int pos]
        //{
        //    get
        //    {
        //        try
        //        {
        //            foreach (PosPhoto item in PosPhoto_L)
        //            {
        //                if (item.NoCamera == noCamera
        //                    && item.Pos == pos)
        //                {
        //                    return item;
        //                }
        //            }
        //            return null;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.L_I.WriteError("ParConfigPar", ex);
        //            return null;
        //        }
        //    }
        //}


        //配置文件错误
        public event Action ConfigParError_Event;
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParConfigPar()
        {
            NameClass = "ParConfigPar";
        }
        #endregion 初始化

    }

    ////产品参数类
    //public class PosPhoto
    //{
    //    public int No { get; set; }
    //    public double X { get; set; }
    //    public double Y { get; set; }
    //    public double Z { get; set; }
    //    public double R { get; set; }
    //    public int NoCamera { get; set; }
    //    public int Pos { get; set; }
    //    public string Annotation { get; set; }    
    //}

    //配置文件列表
    public class FileConfigPar
    {
        public int No { get; set; }
        public string Model { get; set; }//型号
        public string PathPar { get; set; } //路径    
    }
}
