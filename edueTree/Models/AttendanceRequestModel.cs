using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class AttendanceRequestModel
    {
        public int transId { get; set; }
        public Nullable<System.DateTime> transDate { get; set; }
        public DateTime attendDate { get; set; }
        public Nullable<int> inOutMode { get; set; }
        public string narration { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<int> firmId { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime HiddenAttendDate { get; set; }
        public string ActionNameBack { get; set; }
        public virtual FirmInfo FirmInfo { get; set; }
        public virtual Staff Staff { get; set; }

    }
}