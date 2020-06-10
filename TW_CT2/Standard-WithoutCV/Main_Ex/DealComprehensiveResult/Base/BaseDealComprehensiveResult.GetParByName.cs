using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Common;
using HalconDotNet;
using DealFile;
using System.IO;
using BasicClass;
using Camera;
using DealImageProcess;
using BasicComprehensive;
using DealCalibrate;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using ParComprehensive;
using DealGrabImage;
using DealAlgorithm;

namespace Main_EX
{
    /// <summary>
    /// 通过名称获取参数，以及名称 20181127-zx
    /// </summary>
    partial class BaseDealComprehensiveResult
    {
        #region 算法类型名称返回相关信息
        /// <summary>
        /// 通过算法名称返回第一个单元格
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public string GetNameCellByType(string type, int pos)
        {
            try
            {
                return g_BaseParComprehensive.GetNameCellByType(type, pos);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }

        /// <summary>
        /// 通过算法名称返回所有元格，按照顺序
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public List<string> GetAllNameCellByType(string type, int pos)
        {
            try
            {
                return g_BaseParComprehensive.GetAllNameCellByType(type, pos);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }


        /// <summary>
        /// 通过算法名称，索引图像处理算法类-图像处理
        /// </summary>
        /// <param name="type">算法名称</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public BaseParImageProcess GetCellParImageProcessByType(string type, int pos)
        {
            try
            {
                return g_BaseParComprehensive.GetCellParImageProcessByType(type, pos);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }


        /// <summary>
        /// 通过算法名称，索引校准
        /// </summary>
        /// <param name="type">算法名称</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public BaseParCalibrate GetCellParCalibByType(string type, int pos)
        {
            try
            {
                return g_BaseParComprehensive.GetCellParCalibByType(type, pos);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }


        /// <summary>
        /// 通过算法名称，索引拍照
        /// </summary>
        /// <param name="type">算法名称</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public BaseParGrabImage GetCellParGrabImageByType(string type, int pos)
        {
            try
            {
                return g_BaseParComprehensive.GetCellParGrabImageByType(type, pos);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }

        /// <summary>
        /// 通过算法名称，反馈注释
        /// </summary>
        /// <param name="type">算法名称</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public string GetAnnotationByType(string type, int pos)
        {
            try
            {
                return g_BaseParComprehensive.GetAnnotationByType(type, pos);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }
        #endregion 算法类型名称返回相关信息

        #region 通过单元格名称索引信息
        /// <summary>
        /// 通过单元格索引图像处理算法类-图像处理
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns></returns>
        public BaseParImageProcess GetCellParImageProcess(string cell)
        {
            try
            {
                return g_BaseParComprehensive.GetCellParImageProcess(cell);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }


        /// <summary>
        ///  通过单元格索引算法类-校准
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns></returns>
        public BaseParCalibrate GetCellParCalibrate(string cell)
        {
            try
            {
                return g_BaseParComprehensive.GetCellParCalibrate(cell);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }

        /// <summary>
        /// 通过单元格索引算法类-拍照
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns></returns>
        public BaseParGrabImage GetCellParGrabImage(string cell)
        {
            try
            {
                return g_BaseParComprehensive.GetCellParGrabImage(cell);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }
        #endregion 通过单元格名称索引信息

        #region 通过算法名称索引基准值
        /// <summary>
        /// 通过单元格索引基准值类
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public BaseParStd GetParStdByCell(string cell)
        {
            try
            {
                BaseParImageProcess baseParImageProcess = GetCellParImageProcess(cell);

                BaseParStd baseParStd = baseParImageProcess.g_ParStd;
                return baseParStd;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }


        /// <summary>
        /// 通过名称索引基准值类
        /// </summary>
        /// <param name="name">算法名称</param>
        /// <param name="pos">拍照位置</param>
        /// <returns></returns>
        public BaseParStd GetParStdByName(string name, int pos)
        {
            try
            {
                BaseParImageProcess baseParImageProcess = GetCellParImageProcessByType(name, pos);

                BaseParStd baseParStd = baseParImageProcess.g_ParStd;
                return baseParStd;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }
        #endregion 通过算法名称索引基准值

        #region 保存参数
        /// <summary>
        /// 保存所有参数
        /// </summary>
        /// <returns></returns>
        public bool SaveAllParComprehensive()
        {
            try
            {
                return g_BaseParComprehensive.WriteXmlDoc();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }


        /// <summary>
        /// 保存指定单元格参数
        /// </summary>
        /// <param name="nameCell"></param>
        /// <returns></returns>
        public bool SaveParComprehensive(string nameCell)
        {
            try
            {
                return g_BaseParComprehensive.WriteXmlDoc(nameCell);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 将指定算法名称的参数保存到本地
        /// </summary>
        /// <param name="type">算法名称，如果形状匹配T</param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool SaveParComprehensive(string type, int pos)
        {
            try
            {
                string cell = GetNameCellByType(type, pos);
                return g_BaseParComprehensive.WriteXmlDoc(cell);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 保存参数
    }
}
