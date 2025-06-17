using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc;

namespace edueTree.Models
{
    public class StaffModel
    {
        public string StaffReportingName { get; set; }

        public IList<Roleviewmodel> RoleLists { get; set; }

        //[Required(ErrorMessage = "This Field is required")]
        public int RoleId { get; set; }

        public class Roleviewmodel
        {
            public int RoleId { get; set; }
            public int? FirmId { get; set; }
            public string RoleName { get; set; }
            public string Userid { get; set; }
            public bool Checked { get; set; }
        }

        public int FirmId { get; set; }

        public int StaffId { get; set; }

        public string StaffCode { get; set; }

        public string SchoolCode { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string TempAddress { get; set; }

        public int TempCityId { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pincode must be numeric")]
        public string TempPincode { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string TempArea { get; set; }

        [Required(ErrorMessage = "Address  is required")]
        public string PerAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int PerCityId { get; set; }

        public string EmailReportingTo { get; set; }

        [Required(ErrorMessage = "Pincode  is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pincode must be numeric")]
        public string PerPincode { get; set; }

        public string CompanyId { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string PerArea { get; set; }

        public bool? IsMarried { get; set; }
        //[Required(ErrorMessage = "select gender")]
        public int? Gender { get; set; }

        public string State { get; set; }

        public string GenderSet
        {
            get
            {
                if (Gender.Equals(1))
                    return "Male";
                return Gender.Equals(2) ? "Female" : "Not defined";
            }
        }

        public string PassportPhoto { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        // public bool IsMarried { get; set; }
        public bool? IsActive { get; set; }

        // [Required(ErrorMessage = "Cast is required")]
        public int CastId { get; set; }

        // [Required(ErrorMessage = "Sub Cast is required")]
        public int SubCastId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int? DeptId { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DisplayName("Date of birth")]
        public DateTime? Dob { get; set; }

        public int? ReportingId { get; set; }

        public string ReportingTo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? JoiningDate { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public int? DesignationId { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Emergency Contact is required")]
        public string EmergencyContact1 { get; set; }

        public string EmergencyContact2 { get; set; }

        public string EmergencyContact3 { get; set; }
        public DateTime? Probation { get; set; }
        public string Designations { get; set; }
        public string PAN { get; set; }
        public string PF { get; set; }

        public int? EnrollNumber { get; set; }

        public decimal? LeavesOpeningBalance { get; set; }

        public string City { get; set; }
        public decimal? CompensationOpeningBalance { get; set; }

       // [Required(ErrorMessage = "Shift is required")]
        public int? ShiftId { get; set; }

        public string Languageknown { get; set; }
        public string AdharNo { get; set; }
        public string VoteridNo { get; set; }
        public string Licence { get; set; }
        public string ReferalDetail { get; set; }
        public string Nationality { get; set; }
        public string Bloodgroup { get; set; }
        public string Personalmail { get; set; }
        public string Caste { get; set; }
        public virtual ShiftMaster ShiftMaster { get; set; }

        public IEnumerable<SelectListItem> CastList { get; set; }
        public IEnumerable<SelectListItem> SubCastList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> RoleListnew { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<AssignClassSubject> AssignClassSubjects { get; set; }

        //public virtual City City { get; set; }
        public virtual City City1 { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Department Department { get; set; }
        public virtual EsiinfoModel esiinfo { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual ICollection<EsiinfoModel> Esiinfomodels { get; set; }
        public virtual ICollection<StaffDesignation> StaffDesignations { get; set; }
        public virtual ICollection<WorkExperiance> WorkExperiances { get; set; }
        public virtual ICollection<StaffDocument> StaffDocuments { get; set; }
        public ICollection<BankInfo> BankList { get; set; }
        public IEnumerable<BankInfo> BankInfolist { get; set; }
        public BankInfoModel BankInfo { get; set; }
        public virtual ICollection<StaffEducation> StaffEducations { get; set; }

        public string Fullname
        {
            get
            {
                return string.Format("{0} {1} {2}", SchoolCode, FirstName,LastName);
            }
        }

        public string FullnameNew
        {
            get
            {
                return string.Format("{0} {1} {2}", SchoolCode, FirstName, LastName);
            }
        }
    }

    public partial class StaffEducation
    {
        public IEnumerable<SelectListItem> DegreeList { get; set; }
        public IEnumerable<SelectListItem> DegreeSubjectList { get; set; }
    }

    public partial class Staff
    {
        public ObjectResult<decimal?> Balanceleaves { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
    }

    public partial class StaffDesignation
    {
        public IEnumerable<SelectListItem> Design1List { get; set; }
    }

    public class StaffDesignModel
    {
        public string StaffName { get; set; }

        [Required(ErrorMessage = "From Date is required.")]
        [DisplayName("Designation allocated from")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateFrom { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateEnd { get; set; }

        public int StaffId { get; set; }
        [Required(ErrorMessage = "Designation is required.")]
        public int DesignId { get; set; }
        public int transId { get; set; }
        public string StaffCode { get; set; }
        public string Designation { get; set; }

        public IEnumerable<SelectListItem> DesignList { get; set; }
    }



    public class AssignSubjectModel
    {
        public string StaffName { get; set; }
        public int TranId { get; set; }
        public int StaffId { get; set; }
        public int Subjectid { get; set; }
        public int ClassId { get; set; }
        public int YearsId { get; set; }

        public IEnumerable<SelectListItem> AssignList { get; set; }


    }

    public class StaffDocumentModel
    {
        public string StaffName { get; set; }
        public int DocId { get; set; }
        public int StaffId { get; set; }
        [Required]
        public string Title { get; set; }
        public string AttachedDoc { get; set; }

        public IEnumerable<SelectListItem> StafDocList { get; set; }
    }

    /// <summary>
    /// Esi Model for Store Esi Information
    /// </summary>
    public class EsiinfoModel
    {
        public int esiid { get; set; }
        [Required(ErrorMessage = "Father Name is Required")]
        public string fathername { get; set; }
        [Required(ErrorMessage = "Address  is Required")]
        public string peraddress { get; set; }
        [Required(ErrorMessage = "pin is Required")]
        public string pin { get; set; }
        [Required(ErrorMessage = "Branch office Name is Required")]
        public string branchoffice { get; set; }
        [Required(ErrorMessage = "Appointment date is Required")]
        public DateTime? dateofappoint { get; set; }

        public string preinsuranceno { get; set; }

        public string preempolyecode { get; set; }
        public string nomineename { get; set; }

        public string relationship { get; set; }

        public string nomaddress { get; set; }

        public bool isdeleted { get; set; }
        [Required(ErrorMessage = "Corp Name is Required")]
        public string corponame { get; set; }
        [Required(ErrorMessage = "Corp. Insurance no is Required")]
        public string corpinsno { get; set; }
        public string corpfathername { get; set; }
        [Required(ErrorMessage = "Corp Dispensary is Required")]
        public string corpDispensary { get; set; }
        [Required(ErrorMessage = "Corp Address is Required")]
        public string corpnameaddresscode { get; set; }
        [Required(ErrorMessage = "Corp Branch Office is Required")]

        public string corpbranchoffice { get; set; }
        public DateTime? dateofentry { get; set; }
        public DateTime? corpdob { get; set; }
        [Required(ErrorMessage = "Dispensary is Required")]
        public string dispensary { get; set; }

        public string Name { get; set; }
        public string insuranceno { get; set; }
        public DateTime? dateofbirth { get; set; }
        public bool? marriedstatus { get; set; }
        public Nullable<int> gender { get; set; }
        public int? staffid1 { get; set; }

        public string Employeraddressname { get; set; }
        public string CurEmployeraddressname { get; set; }
        public string Email { get; set; }
        public string staffcode { get; set; }


      //  public string Nomineename { get; set; }
        public string Nomineeaddress { get; set; }
        public string Nomineeralation { get; set; }
        public string Nomineestatus { get; set; }
      
        public DateTime? Dob { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Staffid { get; set; }
        public string Age { get; set; }
        public string Share { get; set; }
        public string GenderSet
        {
            get
            {
                if (gender.Equals(1))
                    return "Male";
                return gender.Equals(0) ? "Female" : "Not defined";
            }
        }

        public string marriedset
        {
            get
            {
                if (marriedstatus.Equals(true))
                    return "Married";
                return marriedstatus.Equals(false) ? "Unmarried" : "Not defined";
            }
        }


    }

    /// <summary>
    /// Esi Family Member info 
    /// </summary>
    public class EsiFamilyModel
    {
        public int esifamid { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string name { get; set; }
        [Required(ErrorMessage = "Birth Date is Required")]
        public DateTime? dob { get; set; }
        [Required(ErrorMessage = "Relationship is Required")]
        public string relationship { get; set; }
        [Required(ErrorMessage = "Reside is Required")]
        public bool reside { get; set; }
        [Required(ErrorMessage = "Town is Required")]
        public string town { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public string state { get; set; }
        public int? staffid { get; set; }
        public int? firmid { get; set; }
        public bool? isdeleted { get; set; }
        public int esiinfoid { get; set; }

    }

    public class ClassTeacher
    {
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public string StaffName { get; set; }
        public string ClassDivName { get; set; }
        public string Caption { get; set; }
        public string YearName { get; set; }
    }


    public class AssignClassSubject
    {
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> SubjectList { get; set; }

        public string StaffName { get; set; }
        public string ClassDivName { get; set; }
        public string YearName { get; set; }
        public string SubjectName { get; set; }
    }


    public class StaffAttendenceModel
    {
        public int AttendId { get; set; }


        public string EnrollNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Attend Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<DateTime> AttendDate { get; set; }

        public Nullable<int> VerifyMode { get; set; }

        [Required(ErrorMessage = "Check In/Out Mode is required")]
        public Nullable<int> InOutMode { get; set; }
        public Nullable<int> WorCode { get; set; }
        public Nullable<TimeSpan> AttendTime { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public Nullable<bool> IsManuallyCheckOut { get; set; }

        public IEnumerable<SelectListItem> StaffList { get; set; }


    }

    public class MonthlyAttenDetailModel
    {
        public int MonthlyAttenDetailId { get; set; }
        public int? TranMonth { get; set; }
        public int? TranYearyear { get; set; }
        public DateTime? TranDate { get; set; }
        public int? TotalMonthDays { get; set; }
        public decimal? PresentDays { get; set; }
        public decimal? UsedCompansation { get; set; }
        public decimal? EarnCompansation { get; set; }
        public string TransactionBy { get; set; }
        public int? StaffId { get; set; }
        public decimal? WeeklyOff { get; set; }
        public decimal? Absent { get; set; }
        public decimal? PaidLeaves { get; set; }
        public decimal? CurrentMonthCompansation { get; set; }
        public decimal? PreMonthCompansation { get; set; }
        public decimal? MonthlyPaidLeaves { get; set; }

        public decimal? Holidays { get; set; }
        public decimal staffBonus { get; set; }
        public virtual Staff Staff { get; set; }
    }

    public partial class MonthlyAttenDetail
    {
        public Nullable<decimal> DiductionAmount { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string staffCode { get; set; }
    }

    public class ShiftMasterModel
    {
        public int ShiftId { get; set; }
        [Required(ErrorMessage = "Shift Name is required")]
        public string ShiftName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? IsDateChange { get; set; }
        public bool IsAllot { get; set; }
        public int FirmId { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public TimeSpan? TotolDuration { get; set; }
        public int Hour1 { get; set; }
        public int Minute1 { get; set; }

        public virtual ICollection<ShiftHistory> ShiftHistories { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }

    public class TreeViewNodeVm
    {
        public TreeViewNodeVm()
        {
            ChildNode = new List<TreeViewNodeVm>();
        }

        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string NodeName
        {
            get { return GetNodeName(); }
        }
        public IList<TreeViewNodeVm> ChildNode { get; set; }

        public string GetNodeName()
        {
            //string temp = ChildNode.Count > 0 ? "    This employee manages " +
            //ChildNode.Count : "    This employee is working without westing time in managing.";
            return EmployeeCode + "  " + EmployeeName;// + temp;
        }
    }

    //public class StaffInsuranceModel
    //{
    //    public int InsId { get; set; }
    //    public string policyno { get; set; }
    //    public string expdate { get; set; }
    //    public DateTime policydate { get; set; }
    //    public bool IsDeleted { get; set; }
    //    public int staffid { get; set; }


    //}

    public class NomineeDetailModel
    {
        public int Nomineeid { get; set; }
        [Required(ErrorMessage = "Nominee Name is required")]
        public string Nomineename { get; set; }
        [Required(ErrorMessage = "Nominee Address is required")]

        public string Nomineeaddress { get; set; }
        [Required(ErrorMessage = "Nominee Relation is required")]

        public string Nomineeralation { get; set; }

        public string Nomineestatus { get; set; }
        public bool? Isdeleted { get; set; }
        [Required(ErrorMessage = "Nominee Birth Date is required")]
        [DisplayName("Date of birth")]
        public DateTime? Dob { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? Staffid { get; set; }
        public int? Firmid { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, 150, ErrorMessage = "Please use values between 1 to 150")]
        public string Age { get; set; }
        [Range(1, 100, ErrorMessage = "Please use values between 1 to 150")]
        public string Share { get; set; }
    }


}