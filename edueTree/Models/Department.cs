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
    
    public partial class Department
    {
        public Department()
        {
            this.LeaveTypes = new HashSet<LeaveType>();
            this.Weekends = new HashSet<Weekend>();
            this.Trainings = new HashSet<Training>();
            this.MonthlyTTargets = new HashSet<MonthlyTTarget>();
            this.Staffs = new HashSet<Staff>();
        }
    
        public int deptId { get; set; }
        public string deptName { get; set; }
        public Nullable<int> firmId { get; set; }
    
        public virtual ICollection<LeaveType> LeaveTypes { get; set; }
        public virtual ICollection<Weekend> Weekends { get; set; }
        public virtual ICollection<Training> Trainings { get; set; }
        public virtual ICollection<MonthlyTTarget> MonthlyTTargets { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
        public virtual FirmInfo FirmInfo { get; set; }
    }
}
