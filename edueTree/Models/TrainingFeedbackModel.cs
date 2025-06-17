using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class TrainingFeedbackModel
    {
        public int Id { get; set; }
        public string[] StaffId { get; set; }
        public int? TrainingId { get; set; }
        public int? TrainingId2 { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
        public string TrainerName { get; set; }
        public string Duration { get; set; }
        public int? TrainerDept { get; set; }
        public string Deptname { get; set; }
        public string Topic { get; set; }
        public DateTime? TrainingDateStart { get; set; }
        public DateTime? TrainingEndDate { get; set; }
        public string StaffNamea { get; set; }
        public string StaffId1 { get; set; }
        public string FirmLogo { get; set; }
        public string category { get; set; }
        public MultiSelectList Categories { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    


    }

    public class TrainingFeedbackViewModel
    {
        public TrainingFeedbackModel TrainingFeedback { get; set; }
        public IEnumerable<TrainingFeedbackModel> TrainingFeedbackModelList { get; set; }
    }
}