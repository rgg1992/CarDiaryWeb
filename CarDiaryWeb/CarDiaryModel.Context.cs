﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<car> car { get; set; }
        public virtual DbSet<car_brands> car_brands { get; set; }
        public virtual DbSet<car_diary_log> car_diary_log { get; set; }
        public virtual DbSet<fuel_consumption> fuel_consumption { get; set; }
        public virtual DbSet<other_costs> other_costs { get; set; }
        public virtual DbSet<v_FuelCons> v_FuelCons { get; set; }
    
        public virtual ObjectResult<Nullable<double>> getAvgCons(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<double>>("getAvgCons", idParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> getCarIDsForUser(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("getCarIDsForUser", userParameter);
        }
    
        public virtual ObjectResult<fuel_consumption> getFuelConsumptionsForCar(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<fuel_consumption>("getFuelConsumptionsForCar", idParameter);
        }
    
        public virtual ObjectResult<fuel_consumption> getFuelConsumptionsForCar(Nullable<int> id, MergeOption mergeOption)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<fuel_consumption>("getFuelConsumptionsForCar", mergeOption, idParameter);
        }
    
        public virtual ObjectResult<other_costs> getOtherCostsForCar(string id)
        {
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<other_costs>("getOtherCostsForCar", idParameter);
        }
    
        public virtual ObjectResult<other_costs> getOtherCostsForCar(string id, MergeOption mergeOption)
        {
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<other_costs>("getOtherCostsForCar", mergeOption, idParameter);
        }
    
        public virtual ObjectResult<getTotalCostPerMonth_Result> getTotalCostPerMonth(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getTotalCostPerMonth_Result>("getTotalCostPerMonth", idParameter);
        }
    }
}