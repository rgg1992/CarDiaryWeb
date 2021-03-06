﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CarDiaryWeb
{
    public class DatabaseUtility
    {



        public int getCarCountForUser(string user)
        {
            int count = -99;
            try
            {

                using (var context = new CarDiaryWebEF())
                {
                    var countLQ = (from cr in context.car
                                   where (cr.user_name == user)
                                   select cr.id).Count();

                    count = countLQ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return count;
        }

        public int insertCar(car car, string user)
        {
            int car_id = 0;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var cars = context.Set<car>();
                    cars.Add(car);

                    context.SaveChanges();

                    car_id = car.id;
                }

            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }

            return car_id;
        }

        public car readCar(int carId, string user)
        {
            var carDb = new car();
            using (var context = new CarDiaryWebEF())
            {
                var carLQ = from cr in context.car
                            where (cr.id == carId)
                            where (cr.user_name == user)
                            select cr;

                carDb = carLQ.FirstOrDefault<car>();
            }
            return carDb;
        }

        public double getAvgCons(int carId)
        {
            double avgCons = 0;
            double average;

            using (var context = new CarDiaryWebEF())
            {
                var courseList = context.getAvgCons(carId);
                try
                {
                    foreach (var cs in courseList)
                        avgCons = (double)cs;
                }
                catch (Exception ex)
                {
                    // Log the exception.
                    ExceptionUtility.LogException(ex, "DB.cs");
                    avgCons = 0;
                }

            }
            average = Double.Parse(avgCons.ToString());
            return average;
        }

        public List<int> getCarIDsForUser(string user)
        {
            List<int> carIDs = new List<int>();

            using (var context = new CarDiaryWebEF())
            {
                var carsList = context.getCarIDsForUser(user);

                foreach (int id in carsList)
                    carIDs.Add(id);
            }

            return carIDs;
        }

        public bool deleteCar(int carId)
        {
            
            bool delete = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                        context.fuel_consumption.RemoveRange(context.fuel_consumption.Where(x=> x.car_id == carId));
                        context.SaveChanges();
                        //delete = true;
                    

                }
            }
            catch (Exception ex)
            {
                delete = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
                return delete;
            }

            //bool delete = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    context.other_costs.RemoveRange(context.other_costs.Where(x => x.car_id == carId));
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                delete = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
                return delete;
            }

            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var itemToRemove = context.car.SingleOrDefault(c => c.id == carId);

                    if (itemToRemove != null)
                    {
                        context.car.Remove(itemToRemove);
                        context.SaveChanges();
                        delete = true;
                    }

                }
            }
            catch (Exception ex)
            {
                delete = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
                return delete;
            }
            return delete;
        }

        public int getPreviousMileage(int carId)
        {
            int prevMileage = 0;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var prevMileageLQ = (from fc in context.fuel_consumption
                                         where (fc.car_id == carId)
                                         select fc.mileage).Max();

                    prevMileage = (int)prevMileageLQ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return prevMileage;
        }

        public string getFuelType(int carId)
        {
            string fuel = "";
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var fuelTypeLQ = (from cr in context.car
                                      where (cr.id == carId)
                                      select cr.fuel);

                    fuel = fuelTypeLQ.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return fuel;
        }

        public string getFuelConsumptionLastDate(int carId)
        {
            string lastDate = "";
            int fuelId = 0;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    fuelId = (from fc in context.fuel_consumption
                              where (fc.car_id == carId)
                              select fc.id).Max();

                    var lastDateLQ = (from fc in context.fuel_consumption
                                      where (fc.car_id == carId)
                                      where (fc.id == fuelId)
                                      select fc.refuel_date);

                    lastDate = lastDateLQ.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return lastDate;
        }

        public bool addRefueling(fuel_consumption fuelCons)
        {
            bool insert = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var fuels = context.Set<fuel_consumption>();
                    fuels.Add(fuelCons);

                    context.SaveChanges();

                    insert = true;
                }

            }
            catch (Exception ex)
            {
                insert = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }

            return insert;
        }

        public bool updateCar(int carId, car car)
        {
            bool update = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var result = context.car.SingleOrDefault(c => c.id == carId);

                    if (result != null)
                    {
                        result.brand = car.brand;
                        result.model = car.model;
                        result.fuel = car.fuel;
                        result.engine = car.engine;
                        result.year = car.year;
                        result.h_powers = car.h_powers;
                        result.image = car.image;
                        result.user_name = car.user_name;
                        context.SaveChanges();
                    }

                }

                update = true;
            }
            catch (Exception ex)
            {
                update = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }

            return update;
        }

        public double getLitersConsumed(int carId)
        {
            double liters = 0;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    liters = (from fc in context.fuel_consumption
                              where (fc.car_id == carId)
                              select fc.liters).Sum();

                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return liters;
        }

        public List<fuel_consumption> getFuelConsumptionsForCar(int carId)
        {
            List<fuel_consumption> fuelCons = new List<fuel_consumption>();
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var fuelConsList = context.getFuelConsumptionsForCar(carId);

                    foreach (fuel_consumption item in fuelConsList)
                        fuelCons.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return fuelCons;
        }

        public bool removeFuelRecord(int fuelID)
        {
            bool delete = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var itemToRemove = context.fuel_consumption.SingleOrDefault(fc => fc.id == fuelID);

                    if (itemToRemove != null)
                    {
                        context.fuel_consumption.Remove(itemToRemove);
                        context.SaveChanges();
                        delete = true;
                    }

                }
            }
            catch (Exception ex)
            {
                delete = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return delete;
        }

        public bool addOtherCost(other_costs input)
        {
            bool insert = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var others = context.Set<other_costs>();
                    others.Add(input);

                    context.SaveChanges();

                    insert = true;
                }
            }
            catch (Exception ex)
            {
                insert = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }

            return insert;
        }

        public List<other_costs> getOtherCostsForCar(int carId)
        {
            List<other_costs> others = new List<other_costs>();
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var otherCostsList = context.getOtherCostsForCar(carId.ToString());

                    foreach (other_costs item in otherCostsList)
                        others.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return others;
        }

        public bool removeOtherCost(int id)
        {
            bool delete = false;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var itemToRemove = context.other_costs.SingleOrDefault(oc => oc.id == id);

                    if (itemToRemove != null)
                    {
                        context.other_costs.Remove(itemToRemove);
                        context.SaveChanges();
                        delete = true;
                    }

                }
            }
            catch (Exception ex)
            {
                delete = false;
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }
            return delete;
        }

        public List<Tuple<Double?, DateTime?>> getTotalCostPerMonth(int carId)
        {

            Tuple<Double?, DateTime?> item;
            List<Tuple<Double?, DateTime?>> list = new List<Tuple<Double?, DateTime?>>();
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var totalCostList = context.getTotalCostPerMonth(carId);

                    foreach (var res in totalCostList)
                    {
                        item = Tuple.Create(res.TOTAL, res.PERIOD);
                        list.Add(item);
                    }

                }

            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }

            return list;
        }

        public int addCarBrandModel(string brand, string model)
        {
            int brand_id = 0;
            car_brands item = new car_brands();
            item.car_brand = brand;
            item.car_model = model;
            try
            {
                using (var context = new CarDiaryWebEF())
                {
                    var carBrands = context.Set<car_brands>();
                    carBrands.Add(item);

                    context.SaveChanges();

                    brand_id = item.id;
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "DB.cs");
            }

            return brand_id;
        }

        //public Boolean updateCarImage(int car_id, byte[] image)
        //{
        //    bool update = false;
        //    try
        //    {
        //        using (var context = new CarDiaryWebEF())
        //        {
        //            var result = context.car.SingleOrDefault(c => c.id == car_id);

        //            if (result != null)
        //            {
        //                result.blobimage = image;
        //                context.SaveChanges();
        //            }

        //        }

        //        update = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        update = false;
        //        // Log the exception.
        //        ExceptionUtility.LogException(ex, "DB.cs");
        //    }

        //    return update;
        //}

    }
}