using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace CarDiaryWeb
{
    public class CarInformation
    {
        public int car_id { get; set; }
        public int fill_ups { get; set; }
        public double min_cons { get; set; }
        public int distance { get; set; }
        public string price_per_km { get; set; }

        public static CarInformation GetCarInformationFromDB(int carId)
        {
            CarInformation infoDB = new CarInformation();
            var context = new Entities();
            var info = context.Database.SqlQuery<CarInformation>("SELECT car_id,fill_ups,min_cons,distance,price_per_km FROM dbo.v_FuelCons WHERE car_id = @id", new SqlParameter("id", carId));

            foreach (CarInformation carInfo in info)
            {
                infoDB = carInfo;
            }

            return infoDB;
        }
    }
}