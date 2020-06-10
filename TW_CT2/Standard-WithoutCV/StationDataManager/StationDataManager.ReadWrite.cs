using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealFile;
using Common;
using DealConfigFile;

namespace StationDataManager
{
    partial class StationDataMngr
    {
        string NameClass = "StationDataMngr";

        #region 路径
        public string PathLogDelta
        {
            get
            {
                string path = Log.CreateAllTimeFile(ParPathRoot.PathRoot + "软件运行记录\\Custom\\" + ComConfigPar.C_I.NameModel);
                return path + "LogPosMarkDelta.log";
            }
        }

        public string PathStationData
        {
            get
            {
                string path = ParPathRoot.PathRoot + "Store\\产品参数\\" + ComConfigPar.C_I.NameModel;
                return path + "\\StationData.ini";
            }
        }

        #endregion

        #region 工位放片标定值读写
        public void ReadIniCalibPos()
        {
            try
            {
                CalibPos_L.Clear();
                for (int i = 0; i < 6; i++)
                {
                    string section = "Pos" + (i + 1).ToString();
                    double xStdCalib = IniFile.I_I.ReadIniDbl(section, "xStdCalib", PathStationData);
                    double yStdCalib = IniFile.I_I.ReadIniDbl(section, "yStdCalib", PathStationData);
                    double zStdCalib = IniFile.I_I.ReadIniDbl(section, "zStdCalib", PathStationData);
                    double rStdCalib = IniFile.I_I.ReadIniDbl(section, "rStdCalib", PathStationData);

                    CalibPos_L.Add(new Point4D(xStdCalib, yStdCalib, zStdCalib, rStdCalib));

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void WriteIniCalibPos(int i)
        {
            try
            {
                string section = "Pos" + (i--).ToString();

                //标定的时间
                IniFile.I_I.WriteIni(section, "Time", DateTime.Now.ToString(), PathStationData);

                IniFile.I_I.WriteIni(section, "xStdCalib", CalibPos_L[i].DblValue1.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "yStdCalib", CalibPos_L[i].DblValue2.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "zStdCalib", CalibPos_L[i].DblValue3.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "rStdCalib", CalibPos_L[i].DblValue4.ToString(), PathStationData);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion

        #region 工位示教值读写
        public void ReadIniPlacePos()
        {
            try
            {
                PlacePos_L.Clear();
                for (int i = 0; i < 6; i++)
                {
                    string section = "Pos" + (i + 1).ToString();
                    double xStdCalib = IniFile.I_I.ReadIniDbl(section, "xStdAOI", PathStationData);
                    double yStdCalib = IniFile.I_I.ReadIniDbl(section, "yStdAOI", PathStationData);
                    double zStdCalib = IniFile.I_I.ReadIniDbl(section, "zStdAOI", PathStationData);
                    double rStdCalib = IniFile.I_I.ReadIniDbl(section, "rStdAOI", PathStationData);

                    PlacePos_L.Add(new Point4D(xStdCalib, yStdCalib, zStdCalib, rStdCalib));

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void WriteIniPlacePos(int i)
        {
            try
            {
                string section = "Pos" + (i--).ToString();

                //标定的时间
                IniFile.I_I.WriteIni(section, "Time", DateTime.Now.ToString(), PathStationData);

                IniFile.I_I.WriteIni(section, "xStdAOI", PlacePos_L[i].DblValue1.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "yStdAOI", PlacePos_L[i].DblValue2.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "zStdAOI", PlacePos_L[i].DblValue3.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "rStdAOI", PlacePos_L[i].DblValue4.ToString(), PathStationData);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion

        public void read_station_data()
        {
            ReadIniCalibPos();
            ReadIniPlacePos();
        }

    }
}
