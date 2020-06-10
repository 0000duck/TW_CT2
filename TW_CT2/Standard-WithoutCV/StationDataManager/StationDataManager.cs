using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealCalibrate;

namespace StationDataManager
{
    public partial class StationDataMngr
    {
        public static StationDataMngr instance = new StationDataMngr();

        public List<Point4D> CalibPos_L = new List<Point4D>();
        public List<Point4D> PlacePos_L = new List<Point4D>();

    }
}
