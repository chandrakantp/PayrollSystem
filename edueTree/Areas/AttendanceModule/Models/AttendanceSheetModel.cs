using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Areas.AttendanceModule.Models
{
    public class AttendanceSheetModel
    {
        public int? StaffId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string ThatDayStatus { get; set; }
        public string HolidayStatus { get; set; }
        public string EventType { get; set; }
        public string Sesssion { get; set; }
        public int? EnrollNumber { get; set; }
        public int? IsStart { get; set; }
        public decimal? Componensation { get; set; }
        public DateTime? TranDate { get; set; }
        public TimeSpan? ChekIn { get; set; }
        public TimeSpan? NetDuration { get; set; }
        public TimeSpan? ChekOut { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public decimal? LeaveDays { get; set; }
        public decimal? PresentFlag { get; set; }
        public string UserName { get; set; }
        public decimal? ComponensationBalance { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
}