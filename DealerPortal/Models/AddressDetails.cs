using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealerPortal.Models
{
    public class AddressDetails
    {
        public string AddrsTypeName { get; set; }
        public int AddrsTypeId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
    }
}