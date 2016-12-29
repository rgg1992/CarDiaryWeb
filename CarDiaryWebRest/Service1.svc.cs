using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace CarDiaryWebRest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        /*public string GetData(int value)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string strOutput;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {


                SqlCommand sql = new SqlCommand("SELECT brand FROM car where id=1", conn);

                conn.Open();

                strOutput = (string)sql.ExecuteScalar();

                conn.Close();

            }

            //return string.Format("You entered: {0}", value);
            return "Brand selected from db : " + strOutput;
        }
        */
        public Car readCar(int value)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            Car carResult = new Car();
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {


                SqlCommand sql = new SqlCommand("SELECT * FROM car where id=" + value, conn);

                conn.Open();

                SqlDataReader dr = sql.ExecuteReader();

                if (dr.Read())
                {
                    carResult.brand = dr[1].ToString();
                    carResult.model = dr[2].ToString();
                    carResult.year = Int32.Parse(dr[3].ToString());
                    carResult.engine = dr[4].ToString();
                    carResult.fuel = dr[5].ToString();
                    carResult.h_powers = Int32.Parse(dr[6].ToString());
                    carResult.image = dr[7].ToString();
                    carResult.user_name = dr[8].ToString();
                    carResult.id = value;
                }

                conn.Close();

            }

            //return string.Format("You entered: {0}", value);
            return carResult;
        }

        public int createCar(Car car)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            int car_id = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(constring))
                {

                    SqlCommand cmd = new SqlCommand("createCar", conn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter outPutVal = new SqlParameter("@New_car_ID", SqlDbType.Int);


                    outPutVal.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outPutVal);
                    cmd.Parameters.Add("@brand", SqlDbType.NVarChar).Value = car.brand;
                    cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = car.model;
                    cmd.Parameters.Add("@year", SqlDbType.Int).Value = car.year;
                    cmd.Parameters.Add("@engine", SqlDbType.NVarChar).Value = car.engine;
                    cmd.Parameters.Add("@fuel", SqlDbType.NVarChar).Value = car.fuel;
                    cmd.Parameters.Add("@h_powers", SqlDbType.Int).Value = car.h_powers;
                    if (string.IsNullOrEmpty(car.image))
                        cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = DBNull.Value;
                    else
                    cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = car.image;

                    cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = car.user_name;


                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    if (outPutVal.Value != DBNull.Value) car_id = Convert.ToInt32(outPutVal.Value);
                    else car_id = -999;
                    return car_id;

                }
            }
            catch (Exception ex)
            {
                car_id = -999;
            }

            return car_id;
            
        }

        public int readAllCars(String user)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            int carCount=0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT count(*) FROM car where user_name='" + user + "'", conn);

                    conn.Open();

                    carCount = (int)sql.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return carCount;
        }

        public List<String> getCarBrands()
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<String> carBrandsList = new List<String>();
            String carBrand;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT distinct(car_brand) FROM car_brands", conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        for (int i=0; i<dr.FieldCount;i++)
                        {
                            carBrand = dr.GetValue(i).ToString();
                            carBrandsList.Add(carBrand);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return carBrandsList;
        }

        public List<String> getCarModels(String brand)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<String> carModelsList = new List<String>();
            String carModel;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT distinct(car_model) FROM car_brands where car_brand = '" + brand + "';", conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            carModel = dr.GetValue(i).ToString();
                            carModelsList.Add(carModel);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return carModelsList;
        }

/*        public List<String> getInsertedBrands(String user)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<String> insertedBrandsList = new List<String>();
            String insertedBrand;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT brand FROM car where user_name = '" + user + "' order by id;", conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            insertedBrand = dr.GetValue(i).ToString();
                            insertedBrandsList.Add(insertedBrand);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return insertedBrandsList;
        }

        public List<String> getInsertedModels(String user)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<String> insertedModelsList = new List<String>();
            String insertedModel;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT model FROM car where user_name = '" + user + "' order by id;", conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            insertedModel = dr.GetValue(i).ToString();
                            insertedModelsList.Add(insertedModel);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return insertedModelsList;
        }

        public List<String> getSavedImages(String user)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<String> savedImagesList = new List<String>();
            String savedImage;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT image FROM car where user_name = '" + user + "' order by id;", conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            savedImage = dr.GetValue(i).ToString();
                            savedImagesList.Add(savedImage);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return savedImagesList;
        }
        */
        public List<Car> getInsertedCars(String user)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<Car> insertedCarsList = new List<Car>();
            Car insertedCar = new Car();
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT * FROM car where user_name = '" + user + "' order by id;", conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        insertedCar.brand = dr[1].ToString();
                        insertedCar.model = dr[2].ToString();
                        insertedCar.year = Int32.Parse(dr[3].ToString());
                        insertedCar.engine = dr[4].ToString();
                        insertedCar.fuel = dr[5].ToString();
                        insertedCar.h_powers = Int32.Parse(dr[6].ToString());
                        insertedCar.image = dr[7].ToString();
                        insertedCar.user_name = dr[8].ToString();
                        insertedCar.id = Int32.Parse(dr[0].ToString());
                        insertedCarsList.Add(insertedCar);
                        insertedCar = new Car();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return insertedCarsList;
        }

        public Boolean removeCar(int car_id)
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("DELETE FROM car where id=" + car_id, conn);

                    conn.Open();

                    sql.ExecuteNonQuery();
                    
                    conn.Close();
                   // return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //return string.Format("You entered: {0}", value);
            return true;
        }

        public Boolean removeCarFuels(int car_id)
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("DELETE FROM fuel_consumption where car_id=" + car_id, conn);

                    conn.Open();

                    sql.ExecuteNonQuery();

                    conn.Close();
                    // return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //return string.Format("You entered: {0}", value);
            return true;
        }

        public Boolean removeCarOthers(int car_id)
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("DELETE FROM other_costs where car_id=" + car_id, conn);

                    conn.Open();

                    sql.ExecuteNonQuery();

                    conn.Close();
                    // return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //return string.Format("You entered: {0}", value);
            return true;
        }

        public Boolean addCarBrand(String brand, String model)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
           // int car_id = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(constring))
                {

                    SqlCommand cmd = new SqlCommand("INSERT INTO car_brands (car_brand,car_model) VALUES (@brand, @model)", conn);

                    cmd.Parameters.Add("@brand", SqlDbType.NVarChar).Value = brand;
                    cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = model;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
                
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        public Boolean addRefueling(FuelConsumption refueling)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            // int car_id = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(constring))
                {

                    SqlCommand cmd = new SqlCommand("INSERT INTO fuel_consumption (car_id,refuel_date,mileage,fuel_type,distance,liters,unit_price,total_cost,average_cons_per_100_km) VALUES (@car_id, @date,@mileage,@fuel,@distance,@liters,@unit_price,@total,@avg)", conn);

                    cmd.Parameters.Add("@car_id", SqlDbType.Int).Value = refueling.car_id;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar).Value = refueling.refuel_date;
                    cmd.Parameters.Add("@mileage", SqlDbType.Int).Value = refueling.mileage;
                    cmd.Parameters.Add("@fuel", SqlDbType.NVarChar).Value = refueling.fuel_type;
                    cmd.Parameters.Add("@distance", SqlDbType.Int).Value = refueling.distance;
                    cmd.Parameters.Add("@liters", SqlDbType.Float).Value = refueling.liters;
                    cmd.Parameters.Add("@unit_price", SqlDbType.Float).Value = refueling.unit_price;
                    cmd.Parameters.Add("@total", SqlDbType.Float).Value = refueling.total_cost;
                    cmd.Parameters.Add("@avg", SqlDbType.Float).Value = refueling.average_cons_per_100_km;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        public double getAvgCons(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            double avg = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT AVG(average_cons_per_100_km) FROM fuel_consumption where car_id=" + car_id, conn);

                    conn.Open();

                    avg = (double)sql.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }
            
            return avg;
        }

        public FuelInformation getFuelInformation(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            FuelInformation fuelInfo = new FuelInformation();
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT * FROM v_FuelCons where car_id=" + car_id, conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    if (dr.Read())
                    {
                        fuelInfo.car_id = Int32.Parse(dr[0].ToString());
                        fuelInfo.fill_ups = Int32.Parse(dr[1].ToString());
                        fuelInfo.min_cons = Double.Parse(dr[2].ToString());
                        fuelInfo.distance = Int32.Parse(dr[3].ToString());
                        String price = dr[4].ToString();
                        if (price.Contains("."))
                            price = price.Replace(".", ",");
                        fuelInfo.price_per_km = Double.Parse(price);
                    }


                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }
            
            //return string.Format("You entered: {0}", value);
            return fuelInfo;
        }

        public int getPreviousMileage(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            int prevMileage = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT MAX(mileage) FROM fuel_consumption where car_id=" + car_id, conn);

                    conn.Open();

                    prevMileage = (int)sql.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            return prevMileage;
        }

        public List<FuelConsumption> getFuelConsumptionList(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<FuelConsumption> fuelConsumptionsList = new List<FuelConsumption>();
            FuelConsumption fuelConsumption = new FuelConsumption();
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT * FROM fuel_consumption where car_id = " + car_id, conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        fuelConsumption.car_id = car_id;
                        fuelConsumption.refuel_date = dr[2].ToString();
                        fuelConsumption.mileage = Int32.Parse(dr[3].ToString());
                        fuelConsumption.fuel_type = dr[4].ToString();
                        fuelConsumption.distance = Int32.Parse(dr[5].ToString());
                        string temp = dr[6].ToString();
                        if (temp.Contains("."))
                            temp = temp.Replace(".", ",");
                        fuelConsumption.liters = Double.Parse(temp);
                        temp = dr[7].ToString();
                        if (temp.Contains("."))
                            temp = temp.Replace(".", ",");
                        fuelConsumption.unit_price = Double.Parse(temp);
                        temp = dr[8].ToString();
                        if (temp.Contains("."))
                            temp = temp.Replace(".", ",");
                        fuelConsumption.total_cost = Double.Parse(temp);
                        temp = dr[9].ToString();
                        if (temp.Contains("."))
                            temp = temp.Replace(".", ",");
                        fuelConsumption.average_cons_per_100_km = Double.Parse(temp);
                        fuelConsumptionsList.Add(fuelConsumption);
                        fuelConsumption = new FuelConsumption();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return fuelConsumptionsList;
        }

        public Boolean addOtherCost(OtherCosts cost)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            // int car_id = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(constring))
                {

                    SqlCommand cmd = new SqlCommand("INSERT INTO other_costs (car_id,category,cost_date,mileage,total_cost,notes) VALUES (@car_id, @category,@date,@mileage,@total,@notes)", conn);

                    cmd.Parameters.Add("@car_id", SqlDbType.Int).Value = cost.car_id;
                    cmd.Parameters.Add("@category", SqlDbType.NVarChar).Value = cost.category;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar).Value = cost.cost_date;
                    if (cost.mileage == null)
                        cmd.Parameters.Add("@mileage", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@mileage", SqlDbType.Int).Value = cost.mileage;
                    cmd.Parameters.Add("@total", SqlDbType.Float).Value = cost.total_cost;
                    cmd.Parameters.Add("@notes", SqlDbType.NVarChar).Value = cost.notes;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        public List<OtherCosts> getOtherCostsList(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            List<OtherCosts> otherCostsList = new List<OtherCosts>();
            OtherCosts otherCost = new OtherCosts();
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT * FROM other_costs where car_id = " + car_id, conn);

                    conn.Open();

                    SqlDataReader dr = sql.ExecuteReader();

                    while (dr.Read())
                    {
                        otherCost.car_id = car_id;
                        otherCost.category = dr[2].ToString();
                        otherCost.cost_date = dr[3].ToString();
                        otherCost.mileage = Int32.Parse(dr[4].ToString());
                        string temp = dr[5].ToString();
                        if (temp.Contains("."))
                            temp = temp.Replace(".", ",");
                        otherCost.total_cost = Double.Parse(temp);
                        otherCost.notes = dr[6].ToString();
                        otherCostsList.Add(otherCost);
                        otherCost = new OtherCosts();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            //return string.Format("You entered: {0}", value);
            return otherCostsList;
        }

        public String getFuelConsumptionLastDate(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            String lastDate="";
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT refuel_date from fuel_consumption WHERE car_id = " + car_id + " and id = (SELECT max(id) from fuel_consumption where car_id = " + car_id + ")", conn);

                    conn.Open();

                    lastDate = (String)sql.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            return lastDate;
        }

        public int getNumberOfRefuelings(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            int numRefuel = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT count(*) from fuel_consumption WHERE car_id = " + car_id, conn);

                    conn.Open();

                    numRefuel = (int)sql.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            return numRefuel;
        }

        public int getNumberOfOtherCosts(int car_id)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            int numOtherCost = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("SELECT count(*) from other_costs WHERE car_id = " + car_id, conn);

                    conn.Open();

                    numOtherCost = (int)sql.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }

            return numOtherCost;
        }

        public Boolean updateCar(Car car)
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            // int car_id = 0;
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(constring))
                {

                    SqlCommand cmd = new SqlCommand("UPDATE car SET brand = @brand, model = @model, year = @year, engine = @engine, fuel = @fuel, h_powers = @h_powers, image = @image where id = "+ car.id, conn);

                    cmd.Parameters.Add("@brand", SqlDbType.NVarChar).Value = car.brand;
                    cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = car.model;
                    cmd.Parameters.Add("@year", SqlDbType.Int).Value = car.year;
                    cmd.Parameters.Add("@engine", SqlDbType.NVarChar).Value = car.engine;
                    cmd.Parameters.Add("@fuel", SqlDbType.NVarChar).Value = car.fuel;
                    cmd.Parameters.Add("@h_powers", SqlDbType.Int).Value = car.h_powers;
                    cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = car.image;
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        public Boolean deleteFuelCons(int id)
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("DELETE FROM fuel_consumption where id=" + id, conn);

                    conn.Open();

                    sql.ExecuteNonQuery();

                    conn.Close();
                    // return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //return string.Format("You entered: {0}", value);
            return true;
        }

        public Boolean deleteOtherCosts(int id)
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constring))
            {
                try
                {

                    SqlCommand sql = new SqlCommand("DELETE FROM other_costs where id=" + id, conn);

                    conn.Open();

                    sql.ExecuteNonQuery();

                    conn.Close();
                    // return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //return string.Format("You entered: {0}", value);
            return true;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
