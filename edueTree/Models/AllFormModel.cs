using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace edueTree.Models
{
    #region LeaveMasterModel
    public partial class LeaveMasterModel
    {
        public int Id { get; set; }

         [Required(ErrorMessage = "Please Select Employee")]
        public string[] StaffId { get; set; }

        public int LeaveRecordId { get; set; }

        [Required(ErrorMessage = "Leave type is required")]
        public Nullable<int> LevetypeIds { get; set; }
        public Nullable<int> staffids { get; set; }

         [Required(ErrorMessage = "Days is required")]
        public Nullable<decimal> totalLeaves { get; set; }

        public Nullable<bool> IsActiveLeave { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
    
    #endregion

    #region EmployeebucketModel
    public partial class EmployeebucketModel
    {
        public int? StaffId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LeavesType { get; set; }
        public string LeavesTypeAndTotalLeaves { get; set; }
        public decimal? TotalLeaves { get; set; }


    } 
    #endregion

    #region HolidayModel Partial class
    public partial class HolidayModel
    {
        public int holidayId { get; set; }

        [Required(ErrorMessage = "Holiday Name is required")]
        public string holidayName { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public Nullable<System.DateTime> date { get; set; }

        public string resion { get; set; }

        public Nullable<int> yearId { get; set; }

        //public virtual Year Year { get; set; }
    }
    #endregion

    #region HolidayModel partial Class
    public partial class HolidayModel
    {
        public SelectList YearidList { get; set; }
    }
    #endregion

    #region LeaveTypeModel partial class
    public class LeaveTypeModel
    {
        public LeaveTypeModel()
        {
            LeaveBalances = new HashSet<LeaveBalance>();
            LeaveRequests = new HashSet<LeaveRequest>();
        }

        public int leaveTypeId { get; set; }

     //   [Required(ErrorMessage = "Department is required")]
        public int? deptId { get; set; }
        public int? firmId { get; set; }
        [Required(ErrorMessage = "Leave Type is required")]
        public string LeaveType1 { get; set; }
        
        public decimal? counts { get; set; }
        // [Required(ErrorMessage = "Year is required")]
        public int? yearId { get; set; }
        public decimal? perMonthLeavesAdd { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        // public virtual Year Year { get; set; }
    }
    #endregion

    #region DepartmentModel Partial Class
    public class DepartmentModel
    {
        public DepartmentModel()
        {
            this.VariableSettings = new HashSet<VariableSetting>();
            this.Staffs = new HashSet<Staff>();
            this.Weekends = new HashSet<Weekend>();
        }

        public int deptId { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public string deptName { get; set; }
        public int? firmId { get; set; }
        public virtual ICollection<VariableSetting> VariableSettings { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
        public virtual ICollection<Weekend> Weekends { get; set; }
    }
    #endregion

    #region EventModel Partial Class
    public class EventModel
    {
        public int eventId { get; set; }
        [Required(ErrorMessage = "Event is required")]
        public string event1 { get; set; }

        [Required(ErrorMessage = "From date is required")]
        public DateTime? dateFrom { get; set; }

        [Required(ErrorMessage = "To date is required")]
        public DateTime? dateTo { get; set; }

        [Required(ErrorMessage = "Total days is required")]
        public int? totalDays { get; set; }

        public int? firmId { get; set; }

        public virtual FirmInfo FirmInfo { get; set; }
    }
    #endregion

    #region LeaveRequestModel partial class
    public class LeaveRequestModel
    {
        public int tranId { get; set; }
        public int? lTypeId { get; set; }
        public int? staffId { get; set; }
        [Required(ErrorMessage = "From Date is required")]
        public DateTime? dateFrom { get; set; }
        public string sesionDateFrom { get; set; }
        [Required(ErrorMessage = "To Date is required")]
        public DateTime? dateTo { get; set; }
        public string sesionDateTo { get; set; }
        public decimal? totalCounts { get; set; }
        public DateTime? createdDate { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public string status { get; set; }
        public string approvedBY { get; set; }
        public DateTime? approvalDate { get; set; }
        public string LeaveTyp { get; set; }
        public string Narration { get; set; }
        public decimal? LeaveBalance { get; set; }
        public decimal? NoOfLeaves { get; set; }
        public decimal? UsedBalance { get; set; }
        public decimal? RemainingBalance { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual LeaveType LeaveType { get; set; }
        public string ActionName { get; set; }
        public DateTime? rejoing { get; set; }
        public int? firmId { get; set; }
        public string Staffcode { get; set; }
        public string Fullname { get; set; }
        public string LtypeName { get; set; }
    }
    #endregion

    #region DesignationModel partial class
    public partial class DesignationModel
    {
        public DesignationModel()
        {
            this.StaffDesignations = new HashSet<StaffDesignation>();
            this.Staffs = new HashSet<Staff>();
        }

        public int designationId { get; set; }
        public int? FirmId { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        public string designation1 { get; set; }
        public bool? isActive { get; set; }

        public virtual ICollection<StaffDesignation> StaffDesignations { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
    #endregion

    #region AspNetRoleModel partial class
    public partial class AspNetRoleModel
    {
        public AspNetRoleModel()
        {
            this.UserRoles = new HashSet<UserRole>();
            this.Permissions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; }
        public int? firmId { get; set; }

        public bool? IsParent { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
    #endregion

    #region PermissionModel partial class
    public partial class PermissionModel
    {
        public int permissionId { get; set; }
        //[Required(ErrorMessage = "Menu Item is required")]
        public int? menuItemId { get; set; }

         [Required(ErrorMessage = "Menu Item is required")]
        public string[] MId { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public int? RoleId { get; set; }

        public virtual AspNetRole AspNetRole { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
    #endregion

    #region UserRoleModel partial class
    public class UserRoleModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string userId { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public int RoleId { get; set; }
        public int TransId { get; set; }


        public virtual AspNetRole AspNetRole { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
    #endregion

    #region AdvanceRequestModel partial class
    public class AdvanceRequestModel
    {
        public AdvanceRequestModel()
        {
            this.AdvanceEMIs = new HashSet<AdvanceEMI>();
        }

        public int requestId { get; set; }
        public int? staffId { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "No of EMI is required")]
        public int? willPayEMI { get; set; }
        public DateTime? requestedDate { get; set; }
        public bool? isApprove { get; set; }
        public bool? isPaid { get; set; }
        public int? paidMonth { get; set; }
        public int? paidYear { get; set; }

        public virtual ICollection<AdvanceEMI> AdvanceEMIs { get; set; }
        public virtual Staff Staff { get; set; }
    }
    #endregion

    #region AdvancePendingRequestModel partial class
    public partial class AdvancePendingRequestModel
    {
        public AdvancePendingRequestModel()
        {
            this.AdvanceEMIs = new HashSet<AdvanceEMI>();
        }
        public string fullName { get; set; }
        public int requestId { get; set; }
        public int FirmId { get; set; }
        public int? staffId { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "No of EMI is required")]
        public int? willPayEMI { get; set; }
        public DateTime? requestedDate { get; set; }
        public bool? isApprove { get; set; }
        public bool? isPaid { get; set; }
        [Required(ErrorMessage = "Paid Month is required")]
        public int? paidMonth { get; set; }
        [Required(ErrorMessage = "Paid Year is required")]
        public int? paidYear { get; set; }

        public virtual ICollection<AdvanceEMI> AdvanceEMIs { get; set; }
        public virtual Staff Staff { get; set; }
    }
    #endregion

    #region MachineConfigureModel partial class
    public class MachineConfigureModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "IP accept digit only")]
        public string IP1 { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "IP accept digit only")]
        public string IP2 { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "IP accept digit only")]
        public string IP3 { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "IP accept digit only")]
        public string IP4 { get; set; }

        public string IPAddress { get; set; }
        [Required(ErrorMessage = "Port Number is required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Port Number accept digit only")]
        public string Port { get; set; }
    }
    #endregion

    #region AttendanceRequestModel
    public class AttendanceRequestModel
    {
        public int TransId { get; set; }
        public DateTime? TransDate { get; set; }
        //[Required(ErrorMessage = "Attend date is required")]
        public DateTime AttendDate { get; set; }
        public int? InOutMode { get; set; }
        [Required(ErrorMessage = "Attend date is required")]
        public DateTime? AttendanceDate { get; set; }
        public string Narration { get; set; }
        public bool? IsApproved { get; set; }
        public int? StaffId { get; set; }
        public int? firmid { get; set; }
        [Required(ErrorMessage = "Please select Hours")]
        public int Hour { get; set; }

       //  [Required(ErrorMessage = "Please Select AM/PM")]
        public string Mode { get; set; }

        [Required(ErrorMessage = "Please select Minute")]
        public int Minute { get; set; }
        public virtual Staff Staff { get; set; }
        public string Schoolcode { get; set; }
        public string StaffName { get; set; }
        public int AttendId { get; set; }
        public DateTime? HiddenAttendDate { get; set; }
        public string ActionNameBack { get; set; }
        public string Fullname { get; set; }       
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
    #endregion

    #region AttendanceRequestModel
    public class AttendanceRequestEditModel
    {
        public int TransId { get; set; }
        public DateTime? TransDate { get; set; }
        //[Required(ErrorMessage = "Attend date is required")]
        public DateTime AttendDate { get; set; }
        public int? InOutMode { get; set; }
        //[Required(ErrorMessage = "Attend date is required")]
        public DateTime? AttendanceDate { get; set; }
        public string Narration { get; set; }
        public bool? IsApproved { get; set; }
        public int? StaffId { get; set; }
        public int? firmid { get; set; }
        [Required(ErrorMessage = "Please select Hours")]
        public int Hour { get; set; }
        [Required(ErrorMessage = "Please select Minute")]
        public int Minute { get; set; }
        public virtual Staff Staff { get; set; }
        public string Schoolcode { get; set; }
        public string StaffName { get; set; }
        public int AttendId { get; set; }
        public DateTime? HiddenAttendDate { get; set; }
        public string ActionNameBack { get; set; }
        public string Fullname { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
    #endregion

    #region ComputerAssingedModel
    public partial class ComputerAssingedModel
    {
        public ComputerAssingedModel()
        {
            this.DurationTrans = new HashSet<DurationTran>();
        }

        public int pcAssignedId { get; set; }
        [Required(ErrorMessage = "Employee name is required")]
        public int? staffId { get; set; }
        [Required(ErrorMessage = "IP Address is required")]
        public string IPAddress { get; set; }
        [Required(ErrorMessage = "Machine name is required")]
        public string MachineName { get; set; }
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual ICollection<DurationTran> DurationTrans { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }
    #endregion

    #region LogTimeModel
    public class LogTimeModel
    {
        public int staffId { get; set; }
        public string MachineName { get; set; }
        public DateTime unLockTime { get; set; }
        public DateTime lockTime { get; set; }
        public TimeSpan totalDuration { get; set; }
    }
    #endregion 

    #region CompansationTranModel
    public class CompansationTranModel
    {
        public int compId { get; set; }

        public DateTime? date { get; set; }
        public int? staffid { get; set; }
        public bool? isApprove { get; set; }
        public bool? isRejected { get; set; }

        public virtual Staff Staff { get; set; }
    }
    #endregion

    #region PayrollConfigModel
    public class PayrollConfigModel
    {
        public int Id { get; set; }

        public int? FirmId { get; set; }

        //[Required(ErrorMessage = "Monthly Leave Is Required")]
        public decimal? MonthlyLeaves { get; set; }

        [Required]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-5]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Invalid Time. Time Format is HH:MM:SS")]
        public TimeSpan? WorkingHoursDay { get; set; }

       // [Required(ErrorMessage = "Half Day Min Is Required")]
        public TimeSpan? HalfDayMinWorkHours { get; set; }

       // [Required(ErrorMessage = "Working Day Per Month Is Required")]
        public decimal? WorkingdaysPermonth { get; set; }

        public bool? isActive { get; set; }


        public virtual FirmInfo FirmInfo { get; set; }
    } 
    #endregion

    public partial class TblLeaveRecordModel
    {
        public TblLeaveRecordModel()
        {
            this.LeavePassbooks = new HashSet<LeavePassbook>();
        }

        public int LeaveRecordId { get; set; }
        public Nullable<int> LevetypeIds { get; set; }
        public Nullable<int> staffids { get; set; }
        public Nullable<decimal> totalLeaves { get; set; }
        public Nullable<bool> IsActiveLeave { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> firmsId { get; set; }
        public string staffname { get; set; }
        public string leavetypes { get; set; }


        public virtual ICollection<LeavePassbook> LeavePassbooks { get; set; }
        public virtual LeaveType LeaveType { get; set; }
        public virtual Staff Staff { get; set; }
    }


    public partial class TblEmailConfigModel
    {

        public int EmailConfigId { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Configure password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "SMTP Port No is required")]
        public string SMTPPortNo { get; set; }

        [Required(ErrorMessage = "Host name is required")]
        public string HostName { get; set; }

        public Nullable<int> firmid { get; set; }
        public Nullable<bool> isActive { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string extraColumn { get; set; }
    }

    public partial class RoleAssignInfdexModel
    {        
        public int? Firmid { get; set; }
        public int? TranId { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
    }
    public partial class NetUserNameModel
    {

        public int? NetId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int empid { get; set; }

        public int firmId { get; set; }
        public virtual Staff Staff { get; set; }
        public IEnumerable<SelectListItem> StaffListForUser { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string EmpUsername { get; set; }
    }


    public partial class FirmInfoModel
    {
        public FirmInfoModel()
        {
            this.AdvanceEMIs = new HashSet<AdvanceEMI>();
            this.AdvanceRequests = new HashSet<AdvanceRequest>();
            this.AspNetRoles = new HashSet<AspNetRole>();
            this.AttendanceRequests = new HashSet<AttendanceRequest>();
            this.CompansationRequests = new HashSet<CompansationRequest>();
            this.CompansationTrans = new HashSet<CompansationTran>();
            this.ComputerAssingeds = new HashSet<ComputerAssinged>();
            this.Departments = new HashSet<Department>();
            this.Designations = new HashSet<Designation>();
            this.DurationTrans = new HashSet<DurationTran>();
            this.EventLogs = new HashSet<EventLog>();
            this.Events = new HashSet<Event>();
            this.GeneratedSalaries = new HashSet<GeneratedSalary>();
            this.Holidays = new HashSet<Holiday>();
            this.LeaveBalances = new HashSet<LeaveBalance>();
            this.LeaveRequests = new HashSet<LeaveRequest>();
            this.LeaveTypes = new HashSet<LeaveType>();
            this.notices = new HashSet<notice>();
            this.OptionalHolidays = new HashSet<OptionalHoliday>();
            this.PreviousEduInfoes = new HashSet<PreviousEduInfo>();
            this.SalaryVariables = new HashSet<SalaryVariable>();
            this.StaffCTCs = new HashSet<StaffCTC>();
            this.StaticIPs = new HashSet<StaticIP>();
            this.VariableSettings = new HashSet<VariableSetting>();
            this.Weekends = new HashSet<Weekend>();
            this.Staffs = new HashSet<Staff>();
            this.MachineConfigures = new HashSet<MachineConfigure>();
            this.ShiftMasters = new HashSet<ShiftMaster>();
            this.MonthlyAttenDetails = new HashSet<MonthlyAttenDetail>();
            this.PayrollConfigs = new HashSet<PayrollConfig>();
            this.MonthlyTTargets = new HashSet<MonthlyTTarget>();
            this.AttendanceStaffs = new HashSet<AttendanceStaff>();
        }

        public int firmId { get; set; }

         [Required]
        public string firmName { get; set; }
        public string logo { get; set; }
        public string flatNo { get; set; }
        public string street { get; set; }
        public string area { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string contact { get; set; }
        [Display(Name = "Status")]
        public bool isActivefirm { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public Nullable<bool> ipFiltering { get; set; }
        public Nullable<System.DateTime> validDate { get; set; }
        public Nullable<System.DateTime> renewalDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string UniversalId { get; set; }
        public string CompanyId { get; set; }

        public virtual ICollection<AdvanceEMI> AdvanceEMIs { get; set; }
        public virtual ICollection<AdvanceRequest> AdvanceRequests { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        public virtual ICollection<AttendanceRequest> AttendanceRequests { get; set; }
        public virtual ICollection<CompansationRequest> CompansationRequests { get; set; }
        public virtual ICollection<CompansationTran> CompansationTrans { get; set; }
        public virtual ICollection<ComputerAssinged> ComputerAssingeds { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Designation> Designations { get; set; }
        public virtual ICollection<DurationTran> DurationTrans { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<GeneratedSalary> GeneratedSalaries { get; set; }
        public virtual ICollection<Holiday> Holidays { get; set; }
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<LeaveType> LeaveTypes { get; set; }
        public virtual ICollection<notice> notices { get; set; }
        public virtual ICollection<OptionalHoliday> OptionalHolidays { get; set; }
        public virtual ICollection<PreviousEduInfo> PreviousEduInfoes { get; set; }
        public virtual ICollection<SalaryVariable> SalaryVariables { get; set; }
        public virtual ICollection<StaffCTC> StaffCTCs { get; set; }
        public virtual ICollection<StaticIP> StaticIPs { get; set; }
        public virtual ICollection<VariableSetting> VariableSettings { get; set; }
        public virtual ICollection<Weekend> Weekends { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
        public virtual ICollection<MachineConfigure> MachineConfigures { get; set; }
        public virtual ICollection<ShiftMaster> ShiftMasters { get; set; }
        public virtual ICollection<MonthlyAttenDetail> MonthlyAttenDetails { get; set; }
        public virtual ICollection<PayrollConfig> PayrollConfigs { get; set; }
        public virtual ICollection<MonthlyTTarget> MonthlyTTargets { get; set; }
        public virtual ICollection<AttendanceStaff> AttendanceStaffs { get; set; }
    }

    public partial class TblAverageTimeModel
    {
        //[Required]
        public string[] empid { get; set; }
        public int AverageId { get; set; }
        public Nullable<int> staffid { get; set; }
        [Required]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-4]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Invalid Time.Time Format is HH:MM:SS")]
        public string hourtime { get; set; }
        public Nullable<int> firmid { get; set; }
        public IEnumerable<SelectListItem> StaffListTime { get; set; }
        public virtual Staff Staff { get; set; }
    }

    public partial class LoginRecordModel
    {
        public int loginid { get; set; }
        public string FullnameLogin { get; set; }
        public string FirstLogin { get; set; }
        public string MiddleLogin { get; set; }
        public string LastLogin { get; set; }
        public string StaffcodeLogin { get; set; }
        public DateTime CreatedDateLogin { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public string TimeZone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ipaddress { get; set; }
    }

    public partial class TblAllowanceModel
    {
        public int AllowanceId { get; set; }
        [Required(ErrorMessage = "Allowances Title is required")]
        public string AllowanceName { get; set; }
        public Nullable<int> firmId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string AllowRemark { get; set; }
    }

    public partial class AllowRequestModel
    {
        public int AllowTranId { get; set; }
        [Required(ErrorMessage = "Select Allowances Type")]
        public Nullable<int> AllowId { get; set; }

        //[Required(ErrorMessage = "Select Employee")]
        public Nullable<int> staffid { get; set; }

        public Nullable<int> firmid { get; set; }
        public string AdminRemark { get; set; }
        public string Allowancename { get; set; }
        public string status { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Amount")]
        public string Amount { get; set; }

        public Nullable<System.DateTime> createdDate { get; set; }
        public string ApprovedBy { get; set; }
        public string StaffRemark { get; set; }

        
        [Required(ErrorMessage = "Expense date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-mm-yyyy}")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> Expensedate { get; set; }

        public string transactionType { get; set; }
        public SelectList AllowancesList { get; set; }
        public IEnumerable<SelectListItem> EmployeeAllowList { get; set; } 

    }

    #region Structure permission class
    public class StructurepermissionModel
    {
        public int Id { get; set; }
      
        public string[] staffid { get; set; }
        [Required(ErrorMessage = "Select Structure is required")]
        public int? StructId { get; set; }

        public int? Staffidnew { get; set; }


        public IEnumerable<SelectListItem> Staffidnewlist { get; set; }
    }
    #endregion

    public partial class NoticeBoardModel
    {
        public int StaffIdsp { get; set; }
        public string EmpFullnameSp { get; set; }
        public DateTime? DateSp { get; set; }
        public string StatusSp { get; set; }
    }

    public class FinancialyearModel
    {
        public int FinId { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? EndTo { get; set; }
        public bool? IsActive { get; set; }
        public int? Firmid { get; set; }
    }
    public class CalMonthModel
    {      
        public string Monthcalculate { get; set; }
        //[Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string SalaryRule { get; set; }
    }
    public class OvetimeCalculateModel
    {
        public int OvertimeCalId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<int> FirmId { get; set; }
        public Nullable<int> CalMonthId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public Nullable<decimal> PerHour { get; set; }
        public bool Overtime { get; set; }
        public bool Undertime { get; set; }
        public string calculationtype { get; set; }
        public IEnumerable<SelectListItem> StaffListForOvertime { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual TblCalculateMonth TblCalculateMonth { get; set; }

        public string EmployeeName { get; set; }
    }




}