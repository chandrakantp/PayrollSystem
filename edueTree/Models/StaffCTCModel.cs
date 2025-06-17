using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class StaffCtcModel
    {
        public int ctcId { get; set; }

        [Required(ErrorMessage = "Please enter CTC.")]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public decimal? ctc { get; set; }

        [Required(ErrorMessage = "Please select employee.")]
        public int? staffId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? incrementDate { get; set; }

        public string status { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public bool? isActive { get; set; }
        public string narration { get; set; }
        public int? firmId { get; set; }

        //[Required(ErrorMessage = "Please select structure")]
        public int? structId { get; set; }
      
        
        public IEnumerable<SelectListItem> StaffList { get; set; }
        public IEnumerable<SelectListItem> StructureList { get; set; }
        
    }
}