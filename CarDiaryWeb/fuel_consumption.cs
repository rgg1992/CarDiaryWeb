//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarDiaryWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class fuel_consumption
    {
        public fuel_consumption()
        {

        }

        public fuel_consumption(int car_id, string date, int mileage, string fuel, int distance, double liters, double unit_price, double total, double avg)
        {
            this.car_id = car_id;
            this.refuel_date = date;
            this.mileage = mileage;
            this.fuel_type = fuel;
            this.distance = distance;
            this.liters = liters;
            this.unit_price = unit_price;
            this.total_cost = total;
            this.average_cons_per_100_km = avg;
        }

        public int id { get; set; }
        public int car_id { get; set; }
        public string refuel_date { get; set; }
        public Nullable<int> mileage { get; set; }
        public string fuel_type { get; set; }
        public int distance { get; set; }
        public double liters { get; set; }
        public double unit_price { get; set; }
        public double total_cost { get; set; }
        public Nullable<double> average_cons_per_100_km { get; set; }
    
        public virtual car car { get; set; }
    }
}
