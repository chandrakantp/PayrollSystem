using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class VariableSettingModel
    {
        public int[] CategoryId { get; set; }
        public MultiSelectList Categories { get; set; }

        public int VariableId { get; set; }
        public int Firmid { get; set; }

        [Required(ErrorMessage = "Variable name is required")]
        public string VariableName { get; set; }


        public string DeptName { get; set; }

        public int deptId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        public string compute { get; set; }
        public bool IsCalculateFromCtc { get; set; }
        public string[] CalculateFrom1 { get; set; }
        public int? CalculateFrom2 { get; set; }
        public int? CalculateFrom3 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Percentage should be accepted proper format. For ex: 10, 10.5 & 10.50")]
        public decimal? Persentage { get; set; }

        public string Incometaxdetails { get; set; }
        public bool IsEarnings { get; set; }
        public bool IsDuductions { get; set; }
        public decimal? GraterThanLimit { get; set; }
        public decimal? GraterLimitAmt { get; set; }
        public decimal? LessThanLimit { get; set; }
        public decimal? LessLimitAmt { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CalMultiVariables { get; set; }
        public bool slabtype { get; set; }
        public string Calculationperiod { get; set; }
        public string Val1 { get; set; }
        public string Val2 { get; set; }
        public string Val3 { get; set; }
        public string calculatemulti { get; set; }
        public int roundinglimit { get; set; }
        public string attendaceleavenew { get; set; }
        public string EarnDeduct { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public Decimal? AmountUpto { get; set; }
        public Decimal? fromamount { get; set; }
        public bool? Formulaslabtype { get; set; }

        public List<TblSpecifiedFormula> TblSpecifiedList { get; set; }
        public List<TblGratuityCalculation> TblgratuityList{ get; set; }

        public int fromMonthGratuity { get; set; }
        public int toMonthGratuity { get; set; }
        public int eligibleDaysGratuity { get; set; }                               
        public Decimal? Valuebasis { get; set; }

        public string CalculateFrom
        {
            get
            {
                if (IsCalculateFromCtc) { return "CTC"; }
                else if (Val1 != "" || Val2 != "" || Val3 != "") { return Val1 + Val2 + Val3; }
                else { return "NA"; }
            }
        }

        public string Type
        {
            get
            {
                if (IsEarnings)
                {
                    return "Earnings";
                }
                else
                {
                    return "Deductions";
                }
            }
        }

        public IEnumerable<SelectListItem> CalculateList1 { get; set; }
        public IEnumerable<SelectListItem> CalculateList2 { get; set; }
        public IEnumerable<SelectListItem> CalculateList3 { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public virtual Department Department { get; set; }
        public virtual SalaryStructure SalaryStructure { get; set; }
        public IEnumerable<SelectListItem> StructureList { get; set; }

        [Required(ErrorMessage = "Pay slip display name is required")]
        public string payslipname { get; set; }

        public string payheadtype { get; set; }

        public int? Structureid { get; set; }
        public string affnetsalary { get; set; }
        public string Graduity { get; set; }

        [Required(ErrorMessage = "Under is required")]
        public string Under { get; set; }

        [Required(ErrorMessage = "Calculation type is required")]
        public string calculationtype { get; set; }

        public string attendaceleave { get; set; }
        public string Roundingmethod { get; set; }
        public string productiontype { get; set; }
        public string statutorypaytype { get; set; }
        public string Structname { get; set; }
        public string RegistrationNo { get; set; }
        public string IsearningDeduction { get; set; }

        [Required(ErrorMessage = "Select Slab Type")]
        public string PerValue { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Required(ErrorMessage = "Value is required")]
        public decimal? Value { get; set; }

        public string Percentage { get; set; }
    }

    public class CtcModel
    {
        public int CtcId { get; set; }

        [Required(ErrorMessage = "CTC is required")]
        public decimal Ctc { get; set; }
        public int StaffId { get; set; }


        [DisplayName("Increment Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Increment date is required")]
        public DateTime? IncrementDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
        public string StaffName { get; set; }
        public string Designation { get; set; }
        public int? Id { get; set; }
        public string Narration { get; set; }

        public DateTime JoingDate { get; set; }
        public int DeptId { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> StructureList { get; set; }
        public IEnumerable<SelectListItem> SatffList { get; set; }

    }

    public class WeekendsModel
    {
        public int weekendId { get; set; }

        // [Required(ErrorMessage = "Date Is Required")]
        //public Nullable<System.DateTime> Dates { get; set; }

        public string Dates1 { get; set; }

        public string Day { get; set; }

        [Required(ErrorMessage = "select Department")]
        public int DeptId { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

    }

    public class AttendanceSheetModel
    {
        public int? StaffId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string ThatDayStatus { get; set; }
        public string HolidayStatus { get; set; }
        public string EventType { get; set; }
        public string Sesssion { get; set; }
        public int? EnrollNumber { get; set; }
        public int? IsStart { get; set; }
        public decimal? Componensation { get; set; }
        public DateTime? TranDate { get; set; }
        public TimeSpan? ChekIn { get; set; }
        public TimeSpan? NetDuration { get; set; }
        public TimeSpan? ChekOut { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public decimal? LeaveDays { get; set; }
        public decimal? PresentFlag { get; set; }
        public string UserName { get; set; }
        public string AttendMode { get; set; }
        public decimal? ComponensationBalance { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
        public string ShiftName { get; set; }
        public int? AttendId { get; set; }
        public bool? IsManuallyCheckOut { get; set; }
        public Nullable<bool> isManuallyCheckIn { get; set; }
        public string sandwitch { get; set; }
        public int? inOutMode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

    }
    public class PaySlipModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string DeptName { get; set; }

        public string HeadName { get; set; }
        public decimal? WeeklyOff { get; set; }
        public int? Holidays { get; set; }
        public decimal? TotalWoHolidays { get; set; }
        public decimal? TotalWorkingDays { get; set; }
        public int? TotalMonthDays { get; set; }
        public decimal? Absence { get; set; }
        public decimal? PaidLeaves { get; set; }
        public decimal? UsedCompansation { get; set; }
        public decimal? EarnCompansation { get; set; }
        public decimal? HeadAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? PaybleDays { get; set; }
        public Boolean? IsDuductions { get; set; }
        public Boolean? IsEarnings { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public DateTime? JoiningDate { get; set; }

        public string FirmName { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string AddressPart { get; set; }
        public string Contact { get; set; }
        public string PAN { get; set; }
        public string PF { get; set; }
        public decimal LeaveBalance { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }

        public int? tranid { get; set; }
        public int? variablesId { get; set; }
        public int? Ctcid { get; set; }
        public int? tranmonth { get; set; }

    }

    public partial class UserRole
    {
        public string UserName { get; set; }
    }
}