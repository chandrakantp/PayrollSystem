using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class MemberattendanceConfigModel
    {
        public int ConfigId { get; set; }
        [Required(ErrorMessage = "Select Employee")]
        public int? StaffId { get; set; }
        [Required(ErrorMessage = "Select Machine Id")]
        public int? Machineid { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        
        public int? Firmid { get; set; }
        [Required(ErrorMessage = "Finger Id is required.")]
        public int? FingureId { get; set; }
        public string StaffName { get; set; }
        public string SerialKey { get; set; }


        public IEnumerable<SelectListItem> StaffList { get; set; }
        public IEnumerable<SelectListItem> MachineList { get; set; }
    }
}