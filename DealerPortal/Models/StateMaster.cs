//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DealerPortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StateMaster
    {
        public int StateID { get; set; }
        public string State { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> RegionID { get; set; }
        public Nullable<bool> IsUT { get; set; }
        public string StateCode { get; set; }
        public Nullable<decimal> TRatePerCarton { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
    }
}
