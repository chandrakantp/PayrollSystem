using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class EditPermissionModel
    {
       
        public int PermissionId { get; set; }
        public bool GeneralInfo { get; set; }
        public bool AppAttendance { get; set; }
        public bool ManualAttendance { get; set; }
        public bool IsActive { get; set; }
        public bool? LeaveApproval { get; set; }
        public int? StaffId { get; set; }
        public int? CreatedBy { get; set; }
        public string StaffName { get; set; }
        public bool IsProbationLeaveApp { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }


}