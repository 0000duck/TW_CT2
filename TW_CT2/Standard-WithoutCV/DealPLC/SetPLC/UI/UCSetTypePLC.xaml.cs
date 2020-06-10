﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BasicClass;
using Common;
using DealLog;

namespace DealPLC
{
    /// <summary>
    /// TypePLC.xaml 的交互逻辑
    /// </summary>
    public partial class UCSetTypePLC : BaseUCPLC
    {
        #region 定义

        #endregion 定义

        #region 初始化
        public UCSetTypePLC()
        {
            InitializeComponent();

            NameClass = "UCSetTypePLC";
        }

        public override void Init()
        {
            ShowPar_Invoke();
        }
        #endregion 初始化            

        #region 参数调整
        private void cboPLCModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboPLCModel.IsMouseOver)
                {
                    ParSetPLC.P_I.TypePLC_e = (TypePLC_enum)this.cboPLCModel.SelectedIndex;
                    ShowIPPort();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }            
        }
        #endregion 参数调整

        #region 保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string info = "保存成功";
            try
            {
                //备份文件到本地
                FunBackup.F_I.BackupPLC();

                ParSetPLC.P_I.NoStation = (int)dudNoStation.Value;
                ParSetPLC.P_I.TypePLC_e = (TypePLC_enum)this.cboPLCModel.SelectedIndex;
                ParSetPLC.P_I.TypePLCProtocol_e = (TypePLCProtocol_enum)this.cboPLCProtocol.SelectedIndex;

                ParSetPLC.P_I.BlReadCycReg = (bool)tsbReadCyc.IsChecked;

                ParSetPLC.P_I.BlRSingleTaskCamera = (bool)tsbSingleTaskCamera.IsChecked;//相机触发单线程
                ParSetPLC.P_I.Delay = (int)dudDelay.Value;//读取延迟

                ParSetPLC.P_I.BlClearAllTrigger = (bool)tsbClearAllTrigger.IsChecked;//清空触发信号

                ParSetPLC.P_I.BlAnnotherPLC = (bool)tsbAnnotherPLC.IsChecked;//PLC独立程序
                ParSetPLC.P_I.BlAnnotherPLCLog = (bool)tsbRecordLog.IsChecked;//PLC独立程序日志记录

                //IPPort
                ParSetPLC.P_I.IP = txtIP.Text;
                ParSetPLC.P_I.Port = (int)dudPort.Value;

                ParSetPLC.P_I.BlWritePort1 = (bool)tsbWriteChannel1.IsChecked;
                ParSetPLC.P_I.BlWritePort2 = (bool)tsbWriteChannel2.IsChecked;
                ParSetPLC.P_I.BlWritePort3 = (bool)tsbWriteChannel3.IsChecked;
                ParSetPLC.P_I.BlWritePort4 = (bool)tsbWriteChannel4.IsChecked;
                ParSetPLC.P_I.BlWritePort5 = (bool)tsbWriteChannel5.IsChecked;
                ParSetPLC.P_I.BlWritePort6 = (bool)tsbWriteChannel6.IsChecked;

                ParSetPLC.P_I.PortWrite1 = (int)dudPortWrite1.Value;
                ParSetPLC.P_I.PortWrite2 = (int)dudPortWrite2.Value;
                ParSetPLC.P_I.PortWrite3 = (int)dudPortWrite3.Value;
                ParSetPLC.P_I.PortWrite4 = (int)dudPortWrite4.Value;
                ParSetPLC.P_I.PortWrite5 = (int)dudPortWrite5.Value;
                ParSetPLC.P_I.PortWrite6 = (int)dudPortWrite6.Value;

                if (ParSetPLC.P_I.WrtiteIniTypePLC())
                {
                    this.btnSave.RefreshDefaultColor("保存成功", true);
                }
                else
                {
                    this.btnSave.RefreshDefaultColor("保存失败", false);
                    info = "保存失败";
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                this.btnSave.RefreshDefaultColor("保存失败", false);
                info = "保存失败";
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnSave保存", "设置PLC类型" + info);
            }
        }
        #endregion 保存

        #region 显示
        public override void ShowPar()
        {
            try
            {
                this.cboPLCModel.SelectedIndex = (int)ParSetPLC.P_I.TypePLC_e;
                dudNoStation.Value = ParSetPLC.P_I.NoStation;//逻辑站号

                tsbReadCyc.IsChecked = ParSetPLC.P_I.BlReadCycReg;
                this.cboPLCProtocol.SelectedIndex = (int)ParSetPLC.P_I.TypePLCProtocol_e;
                tsbReadCyc.IsChecked = ParSetPLC.P_I.BlReadCycReg;
                tsbSingleTaskCamera.IsChecked = ParSetPLC.P_I.BlRSingleTaskCamera;
                dudDelay.Value = ParSetPLC.P_I.Delay;

                //PLC独立通信
                tsbAnnotherPLC.IsChecked = ParSetPLC.P_I.BlAnnotherPLC;
                tsbRecordLog.IsChecked = ParSetPLC.P_I.BlAnnotherPLCLog;

                //IP Port
                txtIP.Text = ParSetPLC.P_I.IP;
                dudPort.Value = ParSetPLC.P_I.Port;

                //写入端口
                tsbWriteChannel1.IsChecked = ParSetPLC.P_I.BlWritePort1;
                tsbWriteChannel2.IsChecked = ParSetPLC.P_I.BlWritePort2;
                tsbWriteChannel3.IsChecked = ParSetPLC.P_I.BlWritePort3;
                tsbWriteChannel4.IsChecked = ParSetPLC.P_I.BlWritePort4;
                tsbWriteChannel5.IsChecked = ParSetPLC.P_I.BlWritePort5;
                tsbWriteChannel6.IsChecked = ParSetPLC.P_I.BlWritePort6;

                dudPortWrite1.Value = ParSetPLC.P_I.PortWrite1;
                dudPortWrite2.Value = ParSetPLC.P_I.PortWrite2;
                dudPortWrite3.Value = ParSetPLC.P_I.PortWrite3;
                dudPortWrite4.Value = ParSetPLC.P_I.PortWrite4;
                dudPortWrite5.Value = ParSetPLC.P_I.PortWrite5;
                dudPortWrite6.Value = ParSetPLC.P_I.PortWrite6;

                //控件是否显示
                ShowIPPort();

                //清空触发信号
                tsbClearAllTrigger.IsChecked = ParSetPLC.P_I.BlClearAllTrigger;

                #region 权限设置
                base.SetBtnEnable(this.gdLayout, Authority.Authority_e);
                #endregion 权限设置
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void ShowIPPort()
        {
            try
            {
                if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.MIT_Hls)
                {
                    gdIPPort.Visibility = Visibility.Visible;
                }
                else
                {
                    gdIPPort.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

    }
}
