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
    
    public partial class CustomerAddressMaster
    {
        public int PartyAddressID { get; set; }
        public string ContactPersons { get; set; }
        public string Pin { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> StateID { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public Nullable<bool> IsMain { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public Nullable<int> AddressTypeID { get; set; }
        public Nullable<int> PartyID { get; set; }
        public string Website { get; set; }
        public Nullable<int> Cityid { get; set; }
        public string RealAddress { get; set; }
    
        public virtual AddressTypeMaster AddressTypeMaster { get; set; }
    }
}