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
    
    public partial class Structurepermission
    {
        public int Id { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<int> StructId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> FirmId { get; set; }
    
        public virtual SalaryStructure SalaryStructure { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual FirmInfo FirmInfo { get; set; }
    }
}
