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
    
    public partial class News
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string LinkPage { get; set; }
        public string News1 { get; set; }
        public Nullable<System.DateTime> NewsDate { get; set; }
        public string MetatagKeywords { get; set; }
        public string MetatagDescription { get; set; }
        public string MetatagTitle { get; set; }
        public Nullable<bool> OpenNewWindow { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string IPAddress { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string DisplayUrl { get; set; }
        public Nullable<int> Active { get; set; }
    }
}
