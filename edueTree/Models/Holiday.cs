//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace edueTree.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Holiday
    {
        public int holidayId { get; set; }
        public string holidayName { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string resion { get; set; }
        public Nullable<bool> isOptional { get; set; }
        public Nullable<int> firmId { get; set; }
    
        public virtual FirmInfo FirmInfo { get; set; }
    }
}
