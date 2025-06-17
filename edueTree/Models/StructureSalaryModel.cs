using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class StructureSalaryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Structure Name is required.")]
        public string StuctureName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? FirmId { get; set; }
    }
}