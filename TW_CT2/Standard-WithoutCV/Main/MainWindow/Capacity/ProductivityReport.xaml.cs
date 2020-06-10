using System;
using System.Collections.Generic;
using System.Windows;
using BasicClass;
using DealFile;

namespace Main
{
    /// <summary>
    /// ProductivityReport.xaml 的交互逻辑
    /// </summary>
    public partial class ProductivityReport : Window
    {
        public static string ReportIniPath
        {
            get
            {
                return ParPathRoot.PathRoot + "软件运行记录\\Custom\\" + "Report.ini";
            }
        }

        static Queue<ReportData> Data_Q = new Queue<ReportData>();

        public ProductivityReport()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Data_Q.Clear();
            ReadReportIni();
            ProReportDG.ItemsSource = Data_Q;
        }

        private void ReadReportIni()
        {
            try
            {
                for (int i = 0; i < 30; ++i)
                {
                    string section = "Report" + i.ToString();
                    ReportData temp = new ReportData();
                    temp.Date = ReadIniStr(section, "Date");
                    temp.PreciseSUM = ReadIniStr(section, "PreciseSUM");
                    temp.PreciseNG = ReadIniStr(section, "PreciseNG");
                    temp.WastageSUM = ReadIniStr(section, "WastageSUM");
                    temp.WastageNG1 = ReadIniStr(section, "WastageNG1");
                    temp.WastageNG2 = ReadIniStr(section, "WastageNG2");

                    if (temp.Date != string.Empty)
                    {
                        Data_Q.Enqueue(temp);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch
            {

            }
        }

        public string ReadIniStr(string section, string key)
        {
            try
            {
                return IniFile.I_I.ReadIniStr(section, key, ReportIniPath);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void WriteReportIni(int i)
        {
            try
            {
                AddNew(i);
                int j = 0;
                foreach (object obj in Data_Q)
                {
                    string section = "Report" + j.ToString();
                    ReportData temp = (ReportData)obj;

                    WriteIniStr(section, "Date", temp.Date);
                    WriteIniStr(section, "PreciseSUM", temp.PreciseSUM);
                    WriteIniStr(section, "PreciseNG", temp.PreciseNG);
                    WriteIniStr(section, "WastageSUM", temp.WastageSUM);
                    WriteIniStr(section, "WastageNG1", temp.WastageNG1);
                    WriteIniStr(section, "WastageNG2", temp.WastageNG2);
                    j++;
                }
            }
            catch
            {
            }
        }

        public static void WriteIniStr(string section, string key, string value)
        {
            try
            {
                IniFile.I_I.WriteIni(section, key, value, ReportIniPath);
                return;
            }
            catch
            {
            }
        }

        static void AddNew(int i)
        {
            if (Data_Q.Count == 30)
            {
                Data_Q.Dequeue();
            }

            ReportData temp = new ReportData();
            if (i == 1)
            {
                temp.Date = DateTime.Today.ToShortDateString() + "Day";
            }
            else
            {
                temp.Date = DateTime.Today.AddDays(-1).ToShortDateString() + "Night";
            }
            //temp.Date = DateTime.Today.ToShortDateString() + ((i == 1) ? "Day" : "Night");
            temp.PreciseSUM = RegeditMain.R_I.PreciseSUM.ToString();
            temp.PreciseNG = RegeditMain.R_I.PreciseNG.ToString();
            temp.WastageNG1 = RegeditMain.R_I.WastageNG1.ToString();
            temp.WastageNG2 = RegeditMain.R_I.WastageNG2.ToString();

            Data_Q.Enqueue(temp);
        }

        public static void ClearReportData()
        {
            try
            {
                RegeditMain.R_I.PreciseSUM = 0;
                RegeditMain.R_I.PreciseNG = 0;
                RegeditMain.R_I.WastageNG1 = 0;
                RegeditMain.R_I.WastageNG2 = 0;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
    }

    public class ReportData
    {
        public string Date { get; set; }
        public string PreciseSUM { get; set; }
        public string PreciseNG { get; set; }
        public string WastageSUM { get; set; }
        public string WastageNG1 { get; set; }
        public string WastageNG2 { get; set; }
    }
}
