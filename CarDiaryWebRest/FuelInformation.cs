using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDiaryWebRest
{
    public class FuelInformation
    {

        public FuelInformation()
        {

        }

        public int car_id { get; set; }
        public int fill_ups { get; set; }
        public double min_cons { get; set; }
        public int distance { get; set; }
        public double price_per_km { get; set; }

    }
}