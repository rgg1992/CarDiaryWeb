﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CarDiaryWebRest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        Car readCar(int value,string user);

        [OperationContract]
        int createCar(String brand, String model, String engine, String fuel, int year, int h_powers, String image, String user);

        [OperationContract]
        int readAllCars(String user);

        [OperationContract]
        List<String> getCarBrands();

        [OperationContract]
        List<String> getCarModels(String brand);
       
        [OperationContract]
        List<Car> getInsertedCars(String user);

        [OperationContract]
        Boolean removeCar(int car_id);

        [OperationContract]
        Boolean removeCarFuels(int car_id);

        [OperationContract]
        Boolean removeCarOthers(int car_id);

        [OperationContract]
        Boolean addCarBrand(String brand, String model);

        [OperationContract]
        double getAvgCons(int car_id);

        [OperationContract]
        FuelInformation getFuelInformation(int car_id);

        [OperationContract]
        Boolean addRefueling(int car_id, string date, int mileage, string fuel_type, int distance, double liters, double unit_price, double total_cost, double average_cons_per_100_km);

        [OperationContract]
        int getPreviousMileage(int car_id);

        [OperationContract]
        List<FuelConsumption> getFuelConsumptionList(int car_id);

        [OperationContract]
        Boolean addOtherCost(int car_id, string category, string cost_date, int mileage, double total_cost, string notes);

        [OperationContract]
        List<OtherCosts> getOtherCostsList(int car_id);

        [OperationContract]
        String getFuelConsumptionLastDate(int car_id);

        [OperationContract]
        int getNumberOfRefuelings(int car_id);

        [OperationContract]
        int getNumberOfOtherCosts(int car_id);

        [OperationContract]
        Boolean updateCar(int car_id, String brand, String model, String engine, String fuel, int year, int h_powers, String image, String user);

        [OperationContract]
        Boolean deleteFuelCons(int id);

        [OperationContract]
        Boolean deleteOtherCosts(int id);

        [OperationContract]
        Boolean checkUser(string user, string pass);

        [OperationContract]
        Boolean registerUser(string user, string pass);

        [OperationContract]
        Boolean userExists(string user);

        [OperationContract]
        string getFuelType(int car_id);

        [OperationContract]
        Boolean updateCarImage(byte[] image, int car_id);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
