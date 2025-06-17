using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class TrainingModel
    {
        public int TrainigId { get; set; }
        [Required(ErrorMessage = "Topic is Required")]
        public string Topic { get; set; }
        [Required(ErrorMessage = "TrainerName Message is required.")]
        public string TrainerName { get; set; }
        [Required(ErrorMessage = "Trainer department is required.")]
        public int? TrainerDept { get; set; }
        [Required(ErrorMessage = "Egibility Criteria is Required.")]
        public string EgibilityCriteria { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public string Duration { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Training Date Start is required.")]
        public DateTime? TrainingDateStart { get; set; }
        [RegularExpression(@"^([01]\d|2[0-3]):?([0-5]\d)$", ErrorMessage = "Invalid Time.")]
        [Required(ErrorMessage = "Training Start Time is required.")]
        public string TrainingStartTime { get; set; }

        public int? NoofAttendant { get; set; }
        [Required(ErrorMessage = "TrainingType is required.")]
        public string TrainingType { get; set; }
        [Required(ErrorMessage = "Training Repetition is required.")]
        public string TrainingRepetition { get; set; }
        public string Narration { get; set; }
        public string TrainingContent { get; set; }
        [Required(ErrorMessage = "Training Location is required.")]
        
        public string TrainingLocation { get; set; }
        public bool? Isdeleted { get; set; }
        public DateTime? Createddate { get; set; }
        public int? FirmId { get; set; }
        public int? StaffId { get; set; }
        [Required(ErrorMessage = "Training End Date is required.")]
        public DateTime? TrainingEndDate { get; set; }
        [RegularExpression(@"^([01]\d|2[0-3]):?([0-5]\d)$", ErrorMessage = "Invalid Time.")]
         [Required(ErrorMessage = "Training End Time is required.")]
        public string TrainingEndTime { get; set; }

        public string DepartmentName { get; set; }
         public IEnumerable<SelectListItem> DeptListItems { get; set; }

         public string manpower { get; set; }

         public decimal? totaltraning { get; set; }
         public decimal? week1 { get; set; }
         public decimal? week2 { get; set; }
         public decimal? week3 { get; set; }
         public decimal? week4 { get; set; }
         public decimal? week5 { get; set; }
         public decimal? week6 { get; set; }
        public int? trgValue { get; set; }
        public List<Department> deptList { get; set; }
        public int? TotalManpower { get; set; }
        public decimal? YTD { get; set; }
        public decimal? short_excess { get; set; }
        public decimal? MonthOfHrs { get; set; }
        public decimal? Varience { get; set; }
        public int? YearNo { get; set; }
        public int? Monthno { get; set; }
        public int? premonth { get; set; }
    }


    public class AllTrainingModel
    {
        public string fullname { get; set; }
        public string status { get; set; }
        public string trainername { get; set; }
        public string topic { get; set; }
        public string duration { get; set; }
        public string trainingtype { get; set; }
        public string trainingrepition { get; set; }
        public string trainingstarttime { get; set; }
        public DateTime? trainingdate { get; set; }

    }
}