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
    
    public partial class CoumputedValue
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<decimal> amountgreaterthan { get; set; }
        public Nullable<decimal> Amountupto { get; set; }
        public Nullable<decimal> Slabtype { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> StructureId { get; set; }
        public Nullable<int> PayheadId { get; set; }
    
        public virtual SalaryStructure SalaryStructure { get; set; }
    }
}
