using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class AssignShiftModel
    {
        public int ShiftassignId { get; set; }
        [Required(ErrorMessage = "This field is Required")]
        public string[] ShiftId { get; set; }
      
        public int? ShiftId1 { get; set; }
        [Required(ErrorMessage = "This field is Required")]
        public string[] StaffId { get; set; }
         
        public int? StaffId1 { get; set; }
        public int? CreatedBy { get; set; }
        [Required(ErrorMessage = "This field is Required")]
        public int? Fromdate { get; set; }
        [Required(ErrorMessage = "This field is Required")]
        public int? Todate { get; set; }
        public string StaffName { get; set; }
        public string ShiftName { get; set; }
        public bool? Isdeleted { get; set; }
        public int? Firmid { get; set; }
        public bool? IsAllot { get; set; }
        public List<ShiftMasterModel> ShiftList1 { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
    }


}