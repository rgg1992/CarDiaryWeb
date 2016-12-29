using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDiaryWebRest
{
    public class Car
    {

        public Car()
        {

        }

        public Car(string brand, string model, int year, string engine, string fuel, int horsePowers, string image, string user)
        {
            this.brand = brand;
            this.model = model;
            this.year = year;
            this.engine = engine;
            this.fuel = fuel;
            this.h_powers = horsePowers;
            this.image = image;
            this.user_name = user;
        }


        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string engine { get; set; }
        public string fuel { get; set; }
        public int h_powers { get; set; }
        public string image { get; set; }
        public string user_name { get; set; }


    }
}