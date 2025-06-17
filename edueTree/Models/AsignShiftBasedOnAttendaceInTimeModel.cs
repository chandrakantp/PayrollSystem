using System;
using System.Collections.Generic;
using System.Linq;
using System.util;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class AsignShiftBasedOnAttendaceInTimeModel
    {
        public long? rownum { get; set; }
        public DateTime? dt { get; set; }
        public int? staffId { get; set; }
        public DateTime? attendDate { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public int? attendId { get; set; }
        public string staffName { get; set; }
        public int? shiftid { get; set; }
        public int? Fromdate { get; set; }
        public int? Todate { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string Status { get; set; }
        public DateTime? SelectedDate { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string inOutMode { get; set; }
        public string TotalDuration { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
}