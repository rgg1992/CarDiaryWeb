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
    
    public partial class other_costs
    {
        public int id { get; set; }
        public int car_id { get; set; }
        public string category { get; set; }
        public string cost_date { get; set; }
        public Nullable<int> mileage { get; set; }
        public double total_cost { get; set; }
        public string notes { get; set; }
    
        public virtual car car { get; set; }
    }
}