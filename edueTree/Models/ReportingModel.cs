using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class ReportingModel
    {
        public int ReportingId { get; set; }
        [Required(ErrorMessage = "Staff Is Reqruired.")]
        public int? StaffId { get; set; }
      //  public int? StaffId { get; set; }

        [Required(ErrorMessage = "Reporting is important")]
        public string[] ReportingManagerId { get; set; }
        public string[] ReportingManagerId2 { get; set; }
        public bool? Isdeleted { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string StaffName { get; set; }
        public DateTime? CreatedBy { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
        public IEnumerable<SelectListItem> StaffListNew { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> RoleListnew { get; set; }
        [Required(ErrorMessage = "Email reporting to Can not be empty")]
        public string EmailReportingTo { get; set; }
        public List<Staff> StaffList3 { get; set; }
        public string RptMgrName { get; set; }
    }

    //[MetadataType(typeof(ReportingModel))]
    //public partial class Reporting
    //{
    //}

}