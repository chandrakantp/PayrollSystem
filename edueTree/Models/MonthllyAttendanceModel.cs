using System;
using System.Collections.Generic;

namespace edueTree.Models
{
    public class MonthllyAttendanceModel
    {
        public decimal Bonus { get; set; }
        public int DetailId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthString { get; set; }
        public DateTime TranDate { get; set; }
        public int MonthDays { get; set; }
        public int PPR { get; set; }
        public int SC { get; set; }
        public int WeeklyOff { get; set; }
        public int WorkingDays { get; set; }
        public int WorkdayPresent { get; set; }
        public decimal? PaidLeaves { get; set; }

        public decimal? Absent { get; set; }
        public decimal? AttendDays { get; set; }
        public decimal? UsedCompansation { get; set; }
        public decimal? EarnCompansation { get; set; }
        public decimal? CurrentMonthEarnComp { get; set; }
        public decimal? PreviousMonthEarnComp { get; set; }
        public decimal? ForwordComp { get; set; }
        public int? StaffId { get; set; }
        public string TransactionBy { get; set; }
        public string Narration { get; set; }
        public string HeadName { get; set; }
        public string EmployeeName { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public int Holidays { get; set; }
        public int? MonthlyAttenDetailId { get; set; }
        public Nullable<int> CtcId { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> DeductionAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> AbsentAmount { get; set; }
        public Nullable<decimal> PaybleDays { get; set; }
        public Nullable<decimal> LeavesAmtBasicWise { get; set; }
        public string GeneratedBySalary { get; set; }
        public Nullable<decimal> MonthlyPaidLeaves { get; set; }
        public Nullable<System.DateTime> GeneratedDateSalary { get; set; }
        public Nullable<System.DateTime> d1 { get; set; }
        public Nullable<System.DateTime> d2 { get; set; }
        public virtual ICollection<GeneratedSalaryVariable> GeneratedSalaryVariables { get; set; }
        public virtual Staff Staff { get; set; }
        public string Totaltimestr { get; set; }
        public int Sandwitch { get; set; }
        public string Machinetime { get; set; }
        public string Eventnettime { get; set; }
        public string Overundertime { get; set; }
        public string CalculateOn { get; set; }
        public string SalaryFormula { get; set; }
        public string MonthworkingDays { get; set; }
        public Nullable<decimal> AllowanceDebit { get; set; }
        public Nullable<decimal> AllowanceCredit { get; set; }
        public Nullable<decimal> TotalEarning { get; set; }
        public Nullable<decimal> Undertime { get; set; }
        public Nullable<decimal> Overtime { get; set; }
        public int? ctcId { get; set; }

    }
}
