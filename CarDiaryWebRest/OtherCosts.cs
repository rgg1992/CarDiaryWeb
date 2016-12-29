using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDiaryWebRest
{
    public class OtherCosts
    {
        public OtherCosts() { }


        public int id { get; set; }
        public int car_id { get; set; }
        public string category { get; set; }
        public string cost_date { get; set; }
        public Nullable<int> mileage { get; set; }
        public double total_cost { get; set; }
        public string notes { get; set; }
    }
}