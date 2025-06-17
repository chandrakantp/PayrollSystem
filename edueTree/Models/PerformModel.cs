using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class PerformModel
    {
        public PerformModel()
        {
            this.QuestionAssigments = new HashSet<QuestionAssigment>();
        }

        public int QuestionId { get; set; }
        public int  SectionsId { get; set; }
        public string QuestionName { get; set; }
        public Nullable<decimal> weightage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string firmid { get; set; }

        public virtual ICollection<QuestionAssigment> QuestionAssigments { get; set; }
        public virtual QuestionMaster QuestionMaster1 { get; set; }
        public virtual QuestionMaster QuestionMaster2 { get; set; }

        public IEnumerable<SelectListItem> SectionList { get; set; }
    }

    public partial class FeedbackFormModel
    {
        public FeedbackFormModel()
        {
            this.QuestionAssigments = new HashSet<QuestionAssigment>();
        }

        public int FeedbackId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public Nullable<int> firmid { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }

        public virtual ICollection<QuestionAssigment> QuestionAssigments { get; set; }
    }

    public partial class QuestionAssigmentModel
    {
        public int QuestionAssId { get; set; }        
        public Nullable<int> qustId { get; set; }
        public Nullable<int> feedbackId { get; set; }
        public string firmId { get; set; }

        public virtual FeedbackFormMaster FeedbackFormMaster { get; set; }
        public virtual QuestionMaster QuestionMaster { get; set; }
        public List<QuestionMaster> QueList { get; set; }
        public SelectList FormTitleList { get; set; }
    }

    public partial class FormAssignModel
    {
        public int FormAssignmentId { get; set; }
        public int firmId { get; set; }
        public Nullable<int> feedformId { get; set; }
        public string[] Empid { get; set; }
        public int EmployeeassignId { get; set; }
        public virtual FeedbackFormMaster FeedbackFormMaster { get; set; }
        public virtual Staff Staff { get; set; }

        public IEnumerable<SelectListItem> FormNameList { get; set; }

        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
    public partial class FormEvaluatorModel
    {
        public int EvaluatorId { get; set; }
        public string[] empid { get; set; }
        public string[] evaluatorEmpid { get; set; }

        public int Formempid { get; set; }
        public int FormevaluatorEmpid { get; set; }
        public int firmId { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual Staff Staff1 { get; set; }

        public IEnumerable<SelectListItem> StaffListTwo { get; set; }

        public IEnumerable<SelectListItem> StaffListOne { get; set; }

        public string Empname { get; set; }
    }


    public partial class SectionModel
    {
        public SectionModel()
        {
            this.QuestionMasters = new HashSet<QuestionMaster>();
        }

        public int SectionId { get; set; }
        public int firmid { get; set; }
        public string SectionName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public virtual ICollection<QuestionMaster> QuestionMasters { get; set; }
    }

    public partial class EmpQuestionlistModel
    {
        public string Title { get; set; }
        public string Questions { get; set; }
        public decimal? Wightage { get; set; }
        public decimal? Rating { get; set; }
        public int? QuestionId { get; set; }
        public int? FeedbacFId { get; set; }
        public int? firmid { get; set; }
        public int? staffid { get; set; }
    }

    public partial class EvaluatorratingModel
    {
        public string Title { get; set; }    
        public decimal? Rating { get; set; }
        public string EmpFName { get; set; }
        public string Staffcode { get; set; }
        public string EmplName { get; set; }
        public int? FeedbacFId { get; set; }
        public int? Firmid { get; set; }
        public int? Formid { get; set; }
        public int? Staffid { get; set; }
        public string EvaluatorFName { get; set; }
        public int? Evalutid { get; set; }
    }

    public partial class EmailReportingModel
    {
        public int ReportingId { get; set; }
        public int firmid { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<int> ReportingManagerId { get; set; }
        public Nullable<bool> Isdeleted { get; set; }
        public Nullable<System.DateTime> CreatedBy { get; set; }
        public string Empname { get; set; }
        public IEnumerable<SelectListItem> Reportinglist { get; set; }
        public virtual Staff Staff { get; set; }
        
    }
}
