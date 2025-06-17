using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace edueTree.Models
{
    public class BonusModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Bonus Month field is required.")]
        public int BonusMonth { get; set; }
        [Required(ErrorMessage = "The Bonus Year field is required.")]
        public int BonusYear { get; set; }
        [Required(ErrorMessage = "The Employee name field is required.")]
        public int StaffId { get; set; }
        [Required(ErrorMessage = "The Amount field is required.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "The Narration field is required.")]
        public string Narration { get; set; }
        public int TranDate { get; set; }

        public IEnumerable<SelectListItem> StaffList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; } 
    }

    public class CompansationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The date field is required.")]
        public DateTime date { get; set; }

        [Required(ErrorMessage = "The Narration field is required.")]
        public string Narration { get; set; }

    }

}