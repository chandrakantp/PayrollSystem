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
    
    public partial class AttendanceStaff
    {
        public int attendId { get; set; }
        public string enrollNumber { get; set; }
        public Nullable<System.DateTime> attendDate { get; set; }
        public Nullable<int> verifyMode { get; set; }
        public Nullable<int> inOutMode { get; set; }
        public Nullable<int> worCode { get; set; }
        public Nullable<System.TimeSpan> attendTime { get; set; }
        public Nullable<bool> isManuallyCheckOut { get; set; }
        public Nullable<int> firmId { get; set; }
        public Nullable<int> machineId { get; set; }
        public string AttendMode { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<bool> isManuallyCheckIn { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    
        public virtual FirmInfo FirmInfo { get; set; }
    }
}
