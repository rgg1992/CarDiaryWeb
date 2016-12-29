using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDiaryWebRest
{
    public class FuelConsumption
    {

        public FuelConsumption(int carID, string refuelDate, int mileage, string fuelType, int distance, double liters, double unitPrice, double totalCost, double avgCons)
        {
            this.car_id = carID;
            this.refuel_date = refuelDate;
            this.mileage = mileage;
            this.fuel_type = fuelType;
            this.distance = distance;
            this.liters = liters;
            this.unit_price = unitPrice;
            this.total_cost = totalCost;
            this.average_cons_per_100_km = avgCons;
        }

        public FuelConsumption()
        {

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
        
    }
}