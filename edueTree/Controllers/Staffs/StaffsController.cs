using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using edueTree.Models;
using edueTree.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edueTree.Controllers.Staffs
{
    [Authorize]
    public class StaffsController : BaseController
    {
        #region ---------------------- Dbcontext ---------------

        private dbContainer db = new dbContainer();

        #endregion 

        #region ----------- Staff Controller Constructor -------

        public StaffsController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public StaffsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        #endregion 

        #region -------------- User Manager --------------------

        public UserManager<ApplicationUser> UserManager { get; private set; }

        #endregion 
        
        #region ------------------- Index ----------------------

        public ActionResult Index(int? designationId, int? deptId)
        {
            var firm = LoginUserFirmId();
            ViewBag.designationId = new SelectList(db.Designations.Where(a => a.firmId == firm && a.isActive == true),
                "designationId", "designation1");
            if (designationId != null)
            {
                var staff1 =
                    db.Staffs.Where(a => a.designationId == designationId && a.isActive == true && a.firmId == firm)
                        .Select(a => new StaffModel()
                        {
                            StaffId = a.staffId,
                            StaffCode = a.staffCode,
                            SchoolCode = a.schoolCode,
                            FirstName = a.firstName,
                            MiddleName = a.middleName,
                            LastName = a.lastName,
                            Gender = (int) a.gender,
                            Contact = a.contact,
                            Email = a.email,
                            DesignationId = (int) a.designationId,
                            Designation = a.Designation,
                            Department = a.Department,
                            Dob = a.dob,
                            ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                            ReportingId = a.reportingId,
                            EnrollNumber = (int) a.enrollNumber,
                        });
                return View(staff1.ToList());
            }
            else
            {
                var staffs = db.Staffs.Where(a => a.isActive == true && a.firmId == firm).Select(a => new StaffModel()
                {
                    StaffId = a.staffId,
                    StaffCode = a.staffCode,
                    SchoolCode = a.schoolCode,
                    FirstName = a.firstName,
                    MiddleName = a.middleName,
                    LastName = a.lastName,
                    Gender = (int) a.gender,
                    Contact = a.contact,
                    Email = a.email,
                    Designation = a.Designation,
                    Department = a.Department,
                    Dob = a.dob,
                    ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                    ReportingId = a.reportingId,
                    EnrollNumber = (int) a.enrollNumber,
                });
                return View(staffs.ToList());
            }
            
        }


        public ActionResult EmployeeDetailReport()
        {
            var firm = LoginUserFirmId();

            var staffs = db.Staffs.Where(a => a.isActive == true && a.firmId == firm).Select(a => new StaffModel()
            {
                StaffId = a.staffId,
                StaffCode = a.staffCode,
                SchoolCode = a.schoolCode,
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                Contact = a.contact,
                Email = a.email,
                Designation = a.Designation,
                Department = a.Department,
                Dob = a.dob,
                EnrollNumber = (int) a.enrollNumber,
                Probation = a.probation,
                JoiningDate = a.joingDate,
                PerAddress = a.perAddress,
                PerArea = a.perArea,
                PerPincode = a.perPincode,
                TempAddress = a.tempAddress,
                TempArea = a.tempArea,
                TempPincode = a.tempPincode,
                Personalmail = a.Personalmail,
                Bloodgroup = a.Bloodgroup,
                AdharNo = a.Adharcardno,
                PAN = a.PAN,
                Licence = a.Licence,
                VoteridNo = a.VoteridNo,
                Nationality = a.Nationality,
                CompanyId = a.CompanyId,
                EmergencyContact1 = a.emergencyContact1,
                ReferalDetail = a.ReferalDetail,

                Gender = (int) a.gender,
                Languageknown = a.Languageknown,
                IsMarried = a.isMarried,
                City = a.City,
                State = a.State
            });
            return View(staffs.ToList());
        }

        public ActionResult InactiveEmployeeList(string searchString, int? designationId, int? deptId)
        {
            var firm = LoginUserFirmId();
            ViewBag.designationId = new SelectList(db.Designations.Where(a => a.firmId == firm), "designationId",
                "designation1");
            //IQueryable<StaffModel> staffs = null;
            if (searchString != null || designationId != null)
            {
                if (searchString != "" && designationId != null)
                {
                    var staff1 =
                        db.Staffs.Where(
                            a =>
                                a.firstName.Contains(searchString) && a.designationId == designationId &&
                                a.isActive == false && a.firmId == firm)
                            .Select(a => new StaffModel()
                            {
                                StaffId = a.staffId,
                                StaffCode = a.staffCode,
                                SchoolCode = a.schoolCode,
                                FirstName = a.firstName,
                                MiddleName = a.middleName,
                                LastName = a.lastName,
                                Gender = (int) a.gender,
                                Contact = a.contact,
                                Email = a.email,
                                DesignationId = (int) a.designationId,
                                Designation = a.Designation,
                                Department = a.Department,
                                Dob = a.dob,
                                ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                                ReportingId = a.reportingId,
                                EnrollNumber = (int) a.enrollNumber,
                            });
                    return View(staff1.ToList());
                }
                if (searchString == "" && designationId != null)
                {
                    var staff2 =
                        db.Staffs.Where(a => a.designationId == designationId && a.isActive == false && a.firmId == firm)
                            .Select(a => new StaffModel()
                            {
                                StaffId = a.staffId,
                                StaffCode = a.staffCode,
                                SchoolCode = a.schoolCode,
                                FirstName = a.firstName,
                                MiddleName = a.middleName,
                                LastName = a.lastName,
                                Gender = (int) a.gender,
                                Contact = a.contact,
                                Email = a.email,
                                Designation = a.Designation,
                                Department = a.Department,
                                Dob = a.dob,
                                ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                                ReportingId = a.reportingId,
                                EnrollNumber = (int) a.enrollNumber,


                            });
                    return View(staff2.ToList());
                }
                if (searchString != "" && designationId == null)
                {
                    var staff3 =
                        db.Staffs.Where(
                            a =>
                                a.firstName.Contains(searchString) || a.lastName.Contains(searchString) ||
                                a.staffCode.Contains(searchString) && a.isActive == false && a.firmId == firm)
                            .Select(a => new StaffModel()
                            {
                                StaffId = a.staffId,
                                StaffCode = a.staffCode,
                                SchoolCode = a.schoolCode,
                                FirstName = a.firstName,
                                MiddleName = a.middleName,
                                LastName = a.lastName,
                                Gender = (int) a.gender,
                                Contact = a.contact,
                                Email = a.email,
                                Designation = a.Designation,
                                Department = a.Department,
                                Dob = a.dob,
                                ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                                ReportingId = a.reportingId,
                                EnrollNumber = (int) a.enrollNumber,
                            });
                    return View(staff3.ToList());
                }
            }

            var staffs = db.Staffs.Where(a => a.isActive == false && a.firmId == firm).Select(a => new StaffModel()
            {
                StaffId = a.staffId,
                StaffCode = a.staffCode,
                SchoolCode = a.schoolCode,
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                Gender = (int) a.gender,
                Contact = a.contact,
                Email = a.email,
                Designation = a.Designation,
                Department = a.Department,
                Dob = a.dob,
                ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                ReportingId = a.reportingId,
                EnrollNumber = (int) a.enrollNumber,
            });
            return View(staffs.ToList());
        }

        public ActionResult EmployeeStatus(int? staffId)
        {
            var staffStatus = db.Staffs.Find(staffId);
            ViewBag.InActiveResion = new SelectList(db.InActiveResions, "inActiveResionId", "resion");
            return View(staffStatus);
        }

        [HttpPost]
        public ActionResult EmployeeStatus(int? shiftid, int? staffid, Staff staff)
        {
            if (ModelState.IsValid)
            {
                Staff staffs = db.Staffs.Find(staffid);
                staffs.isActive = staff.isActive;
                staffs.inActiveResionId = staff.inActiveResionId;
                staffs.remark = staff.remark;
                staffs.lastWorkingDate = staff.lastWorkingDate;
                db.Staffs.AddOrUpdate(staffs);

                string userId = staffs.userId;
                if (staff.isActive == false)
                {
                    var loginDetail = db.AspNetUsers.FirstOrDefault(a => a.Id == userId);
                    if (loginDetail != null) loginDetail.Discriminator = "Block";
                    db.AspNetUsers.AddOrUpdate(loginDetail);
                }

                if (staff.isActive == true)
                {
                    var loginDetail = db.AspNetUsers.FirstOrDefault(a => a.Id == userId);
                    if (loginDetail != null) loginDetail.Discriminator = "ApplicationUser";
                    db.AspNetUsers.AddOrUpdate(loginDetail);
                }
                db.SaveChanges();
            }
            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = staffid});
        }

        public ActionResult ChangeShift(int? shiftid, int? staffId)
        {
            if (shiftid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var staffHistory = db.Staffs.Find(shiftid);
            var firm = LoginUserFirmId();
            ViewBag.shiftId = new SelectList(db.ShiftMasters.Where(a => a.firmId == firm), "shiftId", "shiftName");
            return View(staffHistory);
        }

        [HttpPost]
        public ActionResult ChangeShift(int? shiftid, int? staffid, ShiftHistory shmaster)
        {
            if (ModelState.IsValid)
            {
                shiftid = shmaster.shiftId;
                if (shmaster.staffId != null) staffid = (int) shmaster.staffId;
                shmaster.changeDate = DateTime.UtcNow;
                db.ShiftHistories.Add(shmaster);
            }
            Staff staffs = db.Staffs.Find(staffid);
            staffs.shiftId = shiftid;
            db.Staffs.AddOrUpdate(staffs);
            db.SaveChanges();
            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = staffid});
        }

        #endregion 

        #region ----------------- Details ----------------------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Details(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }
            if (staffId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var staff = db.Staffs.Find(staffId);

            if (staff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staff);
        }

        #endregion 

        #region -------------------- Create --------------------
        public ActionResult Create()
        {
            var firm = LoginUserFirmId();
            var staffModel = new StaffModel
            {
                FirmId = firm,

                DesignationList =
                    new SelectList(db.Designations.Where(a => a.firmId == firm && a.isActive == true), "designationId","designation1"),

                DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName"),

                RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name"),

                StaffList =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        }),

                RoleLists = db.AspNetRoles.Where(a => a.firmId == firm).Select(a => new StaffModel.Roleviewmodel
                {
                    RoleId = a.Id,
                    RoleName = a.Name,
                    FirmId = a.firmId
                }).ToList()
            };
            return View(staffModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StaffModel staff)
        {
            var firm = LoginUserFirmId();
            var dbsNew = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm && s.isActive == true);
            if (dbsNew == null)
            {
                TempData["notice"] = "emailnotexit";
                staff.DesignationList =
                    new SelectList(db.Designations.Where(a => a.firmId == firm && a.isActive == true), "designationId",
                        "designation1");
                staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId",
                    "deptName");

                staff.StaffList =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        });
                staff.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");

                return View(staff);
            }
            if (db.Staffs.Any(a => a.email == staff.Email && a.firmId == firm))
            {
                TempData["notice"] = "existEmailAddress";
                staff.DesignationList =
                    new SelectList(db.Designations.Where(a => a.firmId == firm && a.isActive == true), "designationId",
                        "designation1");
                staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId",
                    "deptName");

                staff.StaffList =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        });
                staff.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");

                return View(staff);
            }
            if (db.Staffs.Any(a => a.email == staff.SchoolCode && a.firmId == firm))
            {
                TempData["notice"] = "exist";
                staff.DesignationList = new SelectList(db.Designations.Where(a => a.firmId == firm), "designationId",
                    "designation1");
                staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId",
                    "deptName");
                staff.StaffList =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        });
                staff.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
                return View(staff);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var staffadmin = db.Staffs.Where(s => s.firmId == firm);
                    var stid = staffadmin.Max(s => s.schoolCode);
                    var prifixNew = Convert.ToDecimal(stid) + 1;
                    var strnew = prifixNew.ToString(CultureInfo.InvariantCulture);
                    var orDefault = db.Configs.FirstOrDefault(a => a.configId == 1);
                    if (orDefault != null)
                    {
                        var prifix = Convert.ToDecimal(orDefault.schoolCode) + 1;
                        var str = prifix.ToString(CultureInfo.InvariantCulture);

                        var user = new ApplicationUser()
                        {
                            UserName = strnew
                        };
                        var result = await UserManager.CreateAsync(user, strnew);

                        var firstOrDefault = db.Configs.FirstOrDefault(a => a.isStudent == true);
                        if (firstOrDefault != null)
                            firstOrDefault.schoolCode = prifixNew;
                        db.Configs.AddOrUpdate(firstOrDefault);

                        if (result.Succeeded)
                        {
                            var userDetail = db.AspNetUsers.FirstOrDefault(a => a.UserName == strnew);
                            if (userDetail != null)
                            {
                                var staffs = new Staff
                                {
                                    userId = userDetail.Id,
                                    staffCode = Convert.ToString(prifixNew, CultureInfo.InvariantCulture),
                                    schoolCode = prifixNew.ToString(CultureInfo.InvariantCulture),
                                    firstName = staff.FirstName,
                                    middleName = staff.MiddleName,
                                    lastName = staff.LastName,
                                    tempAddress = staff.TempAddress,
                                    tempPincode = staff.TempPincode,
                                    tempArea = staff.TempArea,
                                    perAddress = staff.PerAddress,
                                    perPincode = staff.PerPincode,
                                    perArea = staff.PerArea,
                                    gender = staff.Gender,
                                    contact = staff.Contact,
                                    email = staff.Email,
                                    isActive = true,                                 
                                    dob = (DateTime) staff.Dob,
                                    emergencyContact1 = staff.EmergencyContact1,                                 
                                    joingDate = (DateTime) staff.JoiningDate,
                                    designationId = staff.DesignationId,
                                    deptId = staff.DeptId,
                                    enrollNumber = staff.EnrollNumber,                                
                                    firmId = firm,                                   
                                    Personalmail = staff.Personalmail,
                                    Languageknown = staff.Languageknown,
                                    Adharcardno = staff.AdharNo,
                                    VoteridNo = staff.VoteridNo,
                                    Licence = staff.Licence,
                                    isMarried = staff.IsMarried,
                                    ReferalDetail = staff.ReferalDetail,
                                    Nationality = staff.Nationality,
                                    State = staff.State,
                                    City = staff.City,
                                    Bloodgroup = staff.Bloodgroup,
                                    Caste = staff.Caste,
                                    probation = staff.Probation,
                                    CompanyId = staff.CompanyId
                                };
                                var design = new StaffDesignation
                                {
                                    Staff = staffs,
                                    fromDate = staff.JoiningDate,
                                    designationId = staff.DesignationId,
                                    isActive = true
                                };
                                db.Staffs.Add(staffs);
                                db.StaffDesignations.Add(design);
                                var userRole = new UserRole {RoleId = staff.RoleId, userId = userDetail.Id};
                                db.UserRoles.Add(userRole);
                                db.SaveChanges();

                                var ltypePaid =
                                    db.LeaveTypes.Where(s => s.firmId == firm)
                                        .FirstOrDefault(s => s.LeaveType1 == "Paid Leaves" && s.firmId == firm);
                                var ltypeComp =
                                    db.LeaveTypes.Where(s => s.firmId == firm)
                                        .FirstOrDefault(s => s.LeaveType1 == "Compensation Leaves" && s.firmId == firm);
                                var ltypeInform =
                                    db.LeaveTypes.Where(s => s.firmId == firm)
                                        .FirstOrDefault(s => s.LeaveType1 == "Informed Absent" && s.firmId == firm);

                                if (ltypePaid != null && ltypeComp != null && ltypeInform != null)
                                {
                                    var ltype = new LeaveRecordNew()
                                    {
                                        LevetypeIds = ltypePaid.leaveTypeId,
                                        staffids = staffs.staffId,
                                        totalLeaves = 0,
                                        IsActiveLeave = true,
                                        CreatedDate = DateTime.UtcNow,
                                        firmsId = firm,
                                    };
                                    db.LeaveRecordNews.Add(ltype);
                                    db.SaveChanges();
                                    int lstid1 = ltype.LeaveRecordId;

                                    var lrp = new LeavePassbook
                                    {
                                        LpstaffId = staffs.staffId,
                                        LptotalCounts = 0,
                                        LpcreatedDate = DateTime.UtcNow,
                                        LplTypeId = ltype.LevetypeIds,
                                        LpfirmId = firm,
                                        //LeaveRecordIds = ltype.LeaveRecordId,
                                        LeaveRecordIds = lstid1,
                                    };
                                    db.LeavePassbooks.Add(lrp);
                                    db.SaveChanges();



                                    var ltype1 = new LeaveRecordNew()
                                    {
                                        LevetypeIds = ltypeComp.leaveTypeId,
                                        staffids = staffs.staffId,
                                        totalLeaves = 0,
                                        IsActiveLeave = true,
                                        CreatedDate = DateTime.UtcNow,
                                        firmsId = firm,
                                    };
                                    db.LeaveRecordNews.Add(ltype1);
                                    db.SaveChanges();
                                    int lstid2 = ltype1.LeaveRecordId;
                                    var lrp1 = new LeavePassbook
                                    {
                                        LpstaffId = staffs.staffId,
                                        LptotalCounts = 0,
                                        LpcreatedDate = DateTime.UtcNow,
                                        LplTypeId = ltype1.LevetypeIds,
                                        LpfirmId = firm,
                                        //LeaveRecordIds = ltype1.LeaveRecordId,
                                        LeaveRecordIds = lstid2,
                                    };
                                    db.LeavePassbooks.Add(lrp1);
                                    db.SaveChanges();


                                    var ltype2 = new LeaveRecordNew()
                                    {
                                        LevetypeIds = ltypeInform.leaveTypeId,
                                        staffids = staffs.staffId,
                                        totalLeaves = 0,
                                        IsActiveLeave = true,
                                        CreatedDate = DateTime.UtcNow,
                                        firmsId = firm,
                                    };
                                    db.LeaveRecordNews.Add(ltype2);
                                    db.SaveChanges();
                                    int lstid3 = ltype2.LeaveRecordId;

                                    var lrp2 = new LeavePassbook
                                    {
                                        LpstaffId = staffs.staffId,
                                        LptotalCounts = 0,
                                        LpcreatedDate = DateTime.UtcNow,
                                        LplTypeId = ltype2.LevetypeIds,
                                        LpfirmId = firm,
                                        //LeaveRecordIds = ltype2.LeaveRecordId,
                                        LeaveRecordIds = lstid3
                                    };
                                    db.LeavePassbooks.Add(lrp2);
                                    db.SaveChanges();
                                }

                                var lt = db.LeaveTypes.Where(s => s.firmId == firm);
                                foreach (var ex in lt.ToList())
                                {
                                    var data1 =
                                        db.LeaveRecordNews.Where(
                                            s => s.LevetypeIds == ex.leaveTypeId && s.staffids == staffs.staffId);
                                    if (data1.Any() == false)
                                    {
                                        var etype = new LeaveRecordNew()
                                        {
                                            LevetypeIds = ex.leaveTypeId,
                                            staffids = staffs.staffId,
                                            totalLeaves = 0,
                                            IsActiveLeave = true,
                                            CreatedDate = DateTime.UtcNow,
                                            firmsId = firm,
                                        };
                                        db.LeaveRecordNews.Add(etype);
                                        db.SaveChanges();

                                        int lstid4 = etype.LeaveRecordId;
                                        var lrp = new LeavePassbook
                                        {
                                            LpstaffId = staffs.staffId,
                                            LptotalCounts = 0,
                                            LpcreatedDate = DateTime.UtcNow,
                                            LplTypeId = etype.LevetypeIds,
                                            LpfirmId = firm,
                                            // LeaveRecordIds = etype.LeaveRecordId,
                                            LeaveRecordIds = lstid4,
                                        };
                                        db.LeavePassbooks.Add(lrp);
                                        db.SaveChanges();
                                    }
                                }

                                var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                                string mailTo = staff.Email, userId, password, smtpPort, host, firmName = db.FirmInfoes.FirstOrDefault().firmName;
                                string subject = "Welcome to " + firmName;
                                string body = "Welcome " + staff.FirstName +
                                              ",<br/> <p>Thank you for joining up our organization. Here is your login details,</p></br> Please click on below link to login : http://people.innovative-fusion.com/ <br>";
                                body = body + " <p><b> Username:</b> " + strnew + ",</p><p><b> Password:</b> " + strnew +
                                       "</p><br/><br/> <p><b> Thank you,</b></p><p>The HR Team.</p>";
                                var services = new MailServiceConfig();
                                var flag = services.SendMail(mailTo, body, subject, dbs.EmailAddress, dbs.Password,
                                    dbs.HostName, dbs.SMTPPortNo);
                            }

                            TempData["notice"] = "success";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var userDetail = db.AspNetUsers.FirstOrDefault(a => a.UserName == strnew);
                            if (userDetail != null)
                            {
                                var staffs = new Staff
                                {
                                    userId = userDetail.Id,
                                    staffCode = Convert.ToString(prifixNew, CultureInfo.InvariantCulture),
                                    schoolCode = prifixNew.ToString(CultureInfo.InvariantCulture),
                                    firstName = staff.FirstName,
                                    middleName = staff.MiddleName,
                                    lastName = staff.LastName,
                                    tempAddress = staff.TempAddress,
                                    tempPincode = staff.TempPincode,
                                    tempArea = staff.TempArea,
                                    perAddress = staff.PerAddress,
                                    perPincode = staff.PerPincode,
                                    perArea = staff.PerArea,
                                    gender = staff.Gender,
                                    contact = staff.Contact,
                                    email = staff.Email,
                                    isActive = true,
                                    dob = (DateTime)staff.Dob,
                                    emergencyContact1 = staff.EmergencyContact1,
                                    joingDate = (DateTime)staff.JoiningDate,
                                    designationId = staff.DesignationId,
                                    deptId = staff.DeptId,
                                    enrollNumber = staff.EnrollNumber,
                                    firmId = firm,
                                    Personalmail = staff.Personalmail,
                                    Languageknown = staff.Languageknown,
                                    Adharcardno = staff.AdharNo,
                                    VoteridNo = staff.VoteridNo,
                                    Licence = staff.Licence,
                                    isMarried = staff.IsMarried,
                                    ReferalDetail = staff.ReferalDetail,
                                    Nationality = staff.Nationality,
                                    State = staff.State,
                                    City = staff.City,
                                    Bloodgroup = staff.Bloodgroup,
                                    Caste = staff.Caste,
                                    probation = staff.Probation,
                                    CompanyId = staff.CompanyId
                                };
                                var design = new StaffDesignation
                                {
                                    Staff = staffs,
                                    fromDate = staff.JoiningDate,
                                    designationId = staff.DesignationId,
                                    isActive = true
                                };
                                db.Staffs.Add(staffs);
                                db.StaffDesignations.Add(design);
                                var userRole = new UserRole { RoleId = staff.RoleId, userId = userDetail.Id };
                                db.UserRoles.Add(userRole);
                                db.SaveChanges();

                                var ltypePaid =
                                    db.LeaveTypes.Where(s => s.firmId == firm)
                                        .FirstOrDefault(s => s.LeaveType1 == "Paid Leaves" && s.firmId == firm);
                                var ltypeComp =
                                    db.LeaveTypes.Where(s => s.firmId == firm)
                                        .FirstOrDefault(s => s.LeaveType1 == "Compensation Leaves" && s.firmId == firm);
                                var ltypeInform =
                                    db.LeaveTypes.Where(s => s.firmId == firm)
                                        .FirstOrDefault(s => s.LeaveType1 == "Informed Absent" && s.firmId == firm);

                                if (ltypePaid != null && ltypeComp != null && ltypeInform != null)
                                {
                                    var ltype = new LeaveRecordNew()
                                    {
                                        LevetypeIds = ltypePaid.leaveTypeId,
                                        staffids = staffs.staffId,
                                        totalLeaves = 0,
                                        IsActiveLeave = true,
                                        CreatedDate = DateTime.UtcNow,
                                        firmsId = firm,
                                    };
                                    db.LeaveRecordNews.Add(ltype);
                                    db.SaveChanges();
                                    int lstid1 = ltype.LeaveRecordId;

                                    var lrp = new LeavePassbook
                                    {
                                        LpstaffId = staffs.staffId,
                                        LptotalCounts = 0,
                                        LpcreatedDate = DateTime.UtcNow,
                                        LplTypeId = ltype.LevetypeIds,
                                        LpfirmId = firm,
                                        //LeaveRecordIds = ltype.LeaveRecordId,
                                        LeaveRecordIds = lstid1,
                                    };
                                    db.LeavePassbooks.Add(lrp);
                                    db.SaveChanges();



                                    var ltype1 = new LeaveRecordNew()
                                    {
                                        LevetypeIds = ltypeComp.leaveTypeId,
                                        staffids = staffs.staffId,
                                        totalLeaves = 0,
                                        IsActiveLeave = true,
                                        CreatedDate = DateTime.UtcNow,
                                        firmsId = firm,
                                    };
                                    db.LeaveRecordNews.Add(ltype1);
                                    db.SaveChanges();
                                    int lstid2 = ltype1.LeaveRecordId;
                                    var lrp1 = new LeavePassbook
                                    {
                                        LpstaffId = staffs.staffId,
                                        LptotalCounts = 0,
                                        LpcreatedDate = DateTime.UtcNow,
                                        LplTypeId = ltype1.LevetypeIds,
                                        LpfirmId = firm,
                                        //LeaveRecordIds = ltype1.LeaveRecordId,
                                        LeaveRecordIds = lstid2,
                                    };
                                    db.LeavePassbooks.Add(lrp1);
                                    db.SaveChanges();


                                    var ltype2 = new LeaveRecordNew()
                                    {
                                        LevetypeIds = ltypeInform.leaveTypeId,
                                        staffids = staffs.staffId,
                                        totalLeaves = 0,
                                        IsActiveLeave = true,
                                        CreatedDate = DateTime.UtcNow,
                                        firmsId = firm,
                                    };
                                    db.LeaveRecordNews.Add(ltype2);
                                    db.SaveChanges();
                                    int lstid3 = ltype2.LeaveRecordId;

                                    var lrp2 = new LeavePassbook
                                    {
                                        LpstaffId = staffs.staffId,
                                        LptotalCounts = 0,
                                        LpcreatedDate = DateTime.UtcNow,
                                        LplTypeId = ltype2.LevetypeIds,
                                        LpfirmId = firm,
                                        //LeaveRecordIds = ltype2.LeaveRecordId,
                                        LeaveRecordIds = lstid3
                                    };
                                    db.LeavePassbooks.Add(lrp2);
                                    db.SaveChanges();
                                }

                                var lt = db.LeaveTypes.Where(s => s.firmId == firm);
                                foreach (var ex in lt.ToList())
                                {
                                    var data1 =
                                        db.LeaveRecordNews.Where(
                                            s => s.LevetypeIds == ex.leaveTypeId && s.staffids == staffs.staffId);
                                    if (data1.Any() == false)
                                    {
                                        var etype = new LeaveRecordNew()
                                        {
                                            LevetypeIds = ex.leaveTypeId,
                                            staffids = staffs.staffId,
                                            totalLeaves = 0,
                                            IsActiveLeave = true,
                                            CreatedDate = DateTime.UtcNow,
                                            firmsId = firm,
                                        };
                                        db.LeaveRecordNews.Add(etype);
                                        db.SaveChanges();

                                        int lstid4 = etype.LeaveRecordId;
                                        var lrp = new LeavePassbook
                                        {
                                            LpstaffId = staffs.staffId,
                                            LptotalCounts = 0,
                                            LpcreatedDate = DateTime.UtcNow,
                                            LplTypeId = etype.LevetypeIds,
                                            LpfirmId = firm,
                                            // LeaveRecordIds = etype.LeaveRecordId,
                                            LeaveRecordIds = lstid4,
                                        };
                                        db.LeavePassbooks.Add(lrp);
                                        db.SaveChanges();
                                    }
                                }

                                var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                                string mailTo = staff.Email, userId, password, smtpPort, host, firmName = db.FirmInfoes.FirstOrDefault().firmName;
                                string subject = "Welcome to " + firmName;
                                string body = "Welcome " + staff.FirstName +
                                              ",<br/> <p>Thank you for joining up our organization. Here is your login details,</p></br> Please click on below link to login : http://people.innovative-fusion.com/ <br>";
                                body = body + " <p><b> Username:</b> " + strnew + ",</p><p><b> Password:</b> " + strnew +
                                       "</p><br/><br/> <p><b> Thank you,</b></p><p>The HR Team.</p>";
                                var services = new MailServiceConfig();
                                var flag = services.SendMail(mailTo, body, subject, dbs.EmailAddress, dbs.Password,
                                    dbs.HostName, dbs.SMTPPortNo);
                            }

                            TempData["notice"] = "success";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        staff.DesignationList = new SelectList(db.Designations.Where(a => a.firmId == firm),
                            "designationId", "designation1");
                        staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId",
                            "deptName");
                        staff.StaffList =
                            db.Staffs.ToList()
                                .Where(s => s.firmId == firm && s.isActive == true)
                                .Select(a => new SelectListItem()
                                {
                                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                                    Value = a.staffId.ToString()
                                });
                        staff.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
                        return View(staff);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw; //return View(staff);
                }
                catch (Exception)
                {
                    staff.DesignationList = new SelectList(db.Designations.Where(a => a.firmId == firm),
                        "designationId", "designation1");
                    staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId",
                        "deptName");
                    staff.StaffList =
                        db.Staffs.ToList()
                            .Where(s => s.firmId == firm && s.isActive == true)
                            .Select(a => new SelectListItem()
                            {
                                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                                Value = a.staffId.ToString()
                            });
                    staff.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
                    return View(staff);
                }
            }
            else
            {
                staff.DesignationList = new SelectList(db.Designations.Where(a => a.firmId == firm), "designationId",
                    "designation1");
                staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId",
                    "deptName");
                staff.StaffList =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        });
                staff.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
                return View(staff);
            }
        }

        #endregion 

        #region ---------------------- Edit --------------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var staff = db.Staffs.Find(id);
            var staffs = new StaffModel();

            if (staff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            staffs.StaffCode = staff.staffCode;
            staffs.FirstName = staff.firstName;
            staffs.MiddleName = staff.middleName;
            staffs.LastName = staff.lastName;
            staffs.TempAddress = staff.tempAddress;
            staffs.TempPincode = staff.tempPincode;
            staffs.TempArea = staff.tempArea;
            staffs.PerAddress = staff.perAddress;
            staffs.PerPincode = staff.perPincode;
            staffs.PerArea = staff.perArea;
            staffs.Gender = staff.gender;
            staffs.Contact = staff.contact;
            staffs.Email = staff.email;
            //staffs.IsMarried = staff.isMarried;
            staffs.IsActive = staff.isActive;
            if (staff.dob != null) staffs.Dob = (DateTime) staff.dob;
            if (staff.joingDate != null) staffs.JoiningDate = (DateTime) staff.joingDate;
            // userId = staff.UserId,
            staffs.ShiftId = staff.shiftId;
            staffs.EmergencyContact1 = staff.emergencyContact1;
            staffs.EmergencyContact2 = staff.emergencyContact2;
            staffs.DesignationId = staff.designationId;
            staffs.ReportingId = staff.reportingId;
            staffs.DeptId = staff.deptId;
            staffs.CompensationOpeningBalance = staff.compansationOpeningBalance;
            staffs.EnrollNumber = staff.enrollNumber;
            staffs.LeavesOpeningBalance = staff.leavesOpeningBalance;
            staffs.FirmId = firm;
            staffs.EmailReportingTo = staff.emailReportingTo;
            staffs.StaffId = staff.staffId;
            staffs.Personalmail = staff.Personalmail;
            staffs.Languageknown = staff.Languageknown;
            staffs.AdharNo = staff.Adharcardno;
            staffs.VoteridNo = staff.VoteridNo;
            staffs.Licence = staff.Licence;
            staffs.IsMarried = staff.isMarried;
            staffs.ReferalDetail = staff.ReferalDetail;
            staffs.Nationality = staff.Nationality;
            staffs.State = staff.State;
            staffs.PAN = staff.PAN;
            staffs.City = staff.City;
            staffs.Bloodgroup = staff.Bloodgroup;
            staffs.Caste = staff.Caste;
            staffs.CompanyId = staff.CompanyId;
            staffs.Probation = staff.probation;
            staffs.StaffList =
                db.Staffs.Where(a => a.firmId == firm && a.isActive == true).ToList().Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.userId);
            staffs.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName");
            staffs.DesignationList = new SelectList(db.Designations.Where(a => a.firmId == firm && a.isActive == true),
                "designationId", "designation1");
            staffs.ShiftList = new SelectList(db.ShiftMasters.Where(a => a.firmId == firm), "shiftId", "shiftName");
            staffs.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
            return View(staffs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffModel staff)
        {
            if (ModelState.IsValid)
            {
                var staffs = db.Staffs.Find(staff.StaffId);
                {
                    staffs.staffCode = staff.StaffCode;
                    staffs.firstName = staff.FirstName;
                    staffs.middleName = staff.MiddleName;
                    staffs.lastName = staff.LastName;
                    staffs.tempAddress = staff.TempAddress;
                    staffs.tempPincode = staff.TempPincode;
                    staffs.tempArea = staff.TempArea;
                    staffs.perAddress = staff.PerAddress;
                    staffs.perPincode = staff.PerPincode;
                    staffs.perArea = staff.PerArea;
                    staffs.gender = staff.Gender;
                    staffs.contact = staff.Contact;
                    staffs.email = staff.Email;
                    staffs.PAN = staff.PAN;
                    // staffs.isMarried = staff.IsMarried;
                    // staffs.isActive = staff.IsActive;
                    if (staff.Dob != null) staffs.dob = (DateTime) staff.Dob;
                    if (staff.JoiningDate != null) staffs.joingDate = (DateTime) staff.JoiningDate;
                    // userId = staff.UserId,
                    staffs.shiftId = staff.ShiftId;
                    staffs.emergencyContact1 = staff.EmergencyContact1;
                    staffs.emergencyContact2 = staff.EmergencyContact2;
                    staffs.designationId = staff.DesignationId;
                    // reportingId = staff.staffId,
                    staffs.deptId = staff.DeptId;
                    // staffs.designationId = staff.DesignationId;
                    staffs.compansationOpeningBalance = staff.CompensationOpeningBalance;
                    staffs.enrollNumber = staff.EnrollNumber;
                    staffs.leavesOpeningBalance = staff.LeavesOpeningBalance;
                    staffs.firmId = staff.FirmId;
                    staffs.emailReportingTo = staff.EmailReportingTo;
                    staffs.Personalmail = staff.Personalmail;
                    staffs.Languageknown = staff.Languageknown;
                    staffs.Adharcardno = staff.AdharNo;
                    staffs.reportingId = staff.ReportingId;
                    staffs.VoteridNo = staff.VoteridNo;
                    staffs.Licence = staff.Licence;
                    staffs.isMarried = staff.IsMarried;
                    staffs.ReferalDetail = staff.ReferalDetail;
                    staffs.Nationality = staff.Nationality;
                    staffs.State = staff.State;
                    staffs.City = staff.City;
                    staffs.Bloodgroup = staff.Bloodgroup;
                    staffs.Caste = staff.Caste;
                    staffs.probation = staff.Probation;
                    staffs.CompanyId = staff.CompanyId;
                }
                ;
                db.Staffs.AddOrUpdate(staffs);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = staff.StaffId});
                //   return RedirectToAction("StaffDashboard", new { q = Encrypt("id=" + staff.StaffId) });
                //    return RedirectToAction("Index");

            }
            staff.DesignationList =
                new SelectList(db.Designations.Where(a => a.firmId == staff.FirmId && a.isActive == true),
                    "designationId", "designation1", staff.DesignationId);
            return RedirectToAction("Edit", new {q = Encrypt("id=" + staff.StaffId)});
        }

        #endregion 

        #region ------ Staff update my profile code here -------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult UpdateMySelfProfile(int? id)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            ViewBag.designationId = new SelectList(db.Designations.Where(a => a.firmId == firm), "designationId",
                "designation1");
            // ViewBag.DesignationList = new SelectList(db.Designations, "designationId", "designation1");
            // ViewBag.deptId = new SelectList(db.Departments, "deptId", "deptName");
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.userId);
            staff.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName");
            return View(staff);
        }


        [HttpPost]
        public ActionResult UpdateMySelfProfile(Staff staff)
        {
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {
                    Staff staffs = db.Staffs.Find(staff.staffId);
                    {
                        staffs.staffCode = staff.staffCode;
                        staffs.firstName = staff.firstName;
                        staffs.middleName = staff.middleName;
                        staffs.lastName = staff.lastName;
                        staffs.tempAddress = staff.tempAddress;
                        staffs.tempPincode = staff.tempPincode;
                        staffs.tempArea = staff.tempArea;
                        staffs.perAddress = staff.perAddress;
                        staffs.perPincode = staff.perPincode;
                        staffs.perArea = staff.perArea;
                        staffs.gender = staff.gender;
                        staffs.contact = staff.contact;
                        staffs.email = staff.email;
                        //staffs.isMarried = staff.isMarried;
                        staffs.isActive = staff.isActive;
                        staffs.dob = staff.dob;
                        // userId = staff.UserId,
                        staffs.emergencyContact1 = staff.emergencyContact1;
                        staffs.emergencyContact2 = staff.emergencyContact2;
                        staffs.emergencyContact3 = staff.emergencyContact3;
                        staffs.designationId = staff.designationId;
                        // reportingId = staff.staffId,
                        staffs.deptId = staff.deptId;
                        staffs.enrollNumber = staff.enrollNumber;
                        staffs.leavesOpeningBalance = staff.leavesOpeningBalance;
                        staffs.firmId = firm;
                    }
                    ;
                    db.Staffs.AddOrUpdate(staffs);
                }
                db.SaveChanges();
                return RedirectToAction("StaffProfile");
            }
            catch (DbEntityValidationException e)
            {
                return View(staff);
            }
        }

        #endregion 

        #region --------------- Staff Delete Code here ---------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        //  [ValidateAntiForgeryToken]
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Staff staff = db.Staffs.Find(id);
                var designation = db.StaffDesignations.Where(a => a.staffId == id);
                foreach (var ab in designation)
                {
                    db.StaffDesignations.Remove(ab);
                }
                var shiftMaster = db.ShiftHistories.Where(a => a.staffId == id);

                foreach (var ex in shiftMaster)
                {
                    db.ShiftHistories.Remove(ex);
                }

                var aspnetuser = db.AspNetUsers.Find(staff.userId);

                db.Staffs.Remove(staff);
                var userRole = db.UserRoles.Where(a => a.userId == aspnetuser.Id);

                foreach (var ur in userRole)
                {
                    db.UserRoles.Remove(ur);
                }

                db.AspNetUsers.Remove(aspnetuser);

                var lr = db.TblLeaveRecords.Where(a => a.staffids == id);
                foreach (var ex1 in lr)
                {
                    db.TblLeaveRecords.Remove(ex1);
                }

                var lpass = db.LeavePassbooks.Where(a => a.LpstaffId == id);
                foreach (var expass in lpass)
                {
                    db.LeavePassbooks.Remove(expass);
                }

                db.SaveChanges();
                TempData["notice"] = "delete";
            }
            catch (Exception)
            {
                TempData["notice"] = "cantdelete";
            }
            return RedirectToAction("Index");

        }

        #endregion 

        #region  -------- Upload Staff Member Image Code here ---

        public ActionResult UploadImage(int? staffId)
        {
            var staffModel = new Staff
            {
                staffId = Convert.ToInt32(staffId)
            };
            return View(staffModel);
        }

        [HttpPost]
        public ActionResult UploadImage(int staffid, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var staffs1 = new Staff();
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var rondom = Guid.NewGuid() + fileName;
                    var path = Path.Combine(Server.MapPath("~/StaffDocument/"), rondom);
                    file.SaveAs(path);
                    staffs1.passportPhoto = rondom;
                    ViewBag.Path = String.Format("~/StaffDocument/", fileName);
                }
                var staffs = db.Staffs.Find(staffid);
                staffs.passportPhoto = staffs1.passportPhoto;
                db.Staffs.AddOrUpdate(staffs);
                db.SaveChanges();
                //return RedirectToAction("StaffDashboard", "Staffs", new { staffid = staffs.staffId });
                return RedirectToAction("StaffDashboard", "Staffs", new {q = Encrypt("Staffid=" + staffid)});
            }
            return View();
        }

        #endregion  

        #region --------------------- Despose ------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion 

        #region ------------------- StaffDashboard -------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult StaffDashboard(int? staffId)
        {
            if (staffId == null)
            {
                var login = LoginEmployeeId();
                var data = (from a in db.Staffs where a.staffId == login select a).FirstOrDefault();
                if (data != null) TempData["stafid"] = data.schoolCode;
                return View();
            }

            var data2 = (from a in db.Staffs where a.staffId == staffId select a).FirstOrDefault();
            if (data2 != null)
            {
                TempData["stafid"] = data2.schoolCode;


            }
            var role = 0;
            var userrole = db.UserRoles.FirstOrDefault(a => a.userId == data2.userId);
            if (userrole != null)
            {
                role = userrole.RoleId;
            }
            var rolename1 = "";
            var rolename = db.AspNetRoles.FirstOrDefault(a => a.Id == role);
            if (rolename != null)
            {
                rolename1 = rolename.Name;
            }
            ViewData["RoleName"] = rolename1;
            return View();

        }

        #endregion 

        #region ---------------- StaffProfile ------------------
        public ActionResult StaffProfile()
        {
           // var getuser = GetUserId();
          //  var firstOrDefault = db.AspNetUsers.FirstOrDefault(a => a.Id == getuser);
          //  if (firstOrDefault.UserName == "superadmin")
           // {
           //     return RedirectToAction("FirmsForSuperadmin", "Firm");
          //  }
          //  else
          //  {
                int firmid = LoginUserFirmId();
                int staffId = LoginEmployeeId();
                Staff staff = db.Staffs.Find(staffId);
                // var result = db.GetTotalCompansation(staffId).FirstOrDefault();
                //  if (result < 0)
                //  {
                //      result = 0;
                //   }
                //ViewData["result"] = result;

                var getList = db.EmployeeLeaveBucketFromNewTable(staffId, firmid);

                var atsheetModel =
                    getList.Where(s => s.LeaveType == "Compensation Leaves").Select(a => new EmployeebucketModel()
                    {
                        TotalLeaves = a.TotalCount
                    }).FirstOrDefault();

                if (atsheetModel == null)
                {
                    ViewData["result"] = "0.00";
                }
                else
                {
                    ViewData["result"] = atsheetModel.TotalLeaves;
                }

                var lb = db.GetBalanceLeaves(staffId);
                staff.Balanceleaves = lb;
                return View(staff);
           // }
        }

        #endregion 

        #region ---------------- Exportdata --------------------

        public void ExportData()
        {
            var stafffs = db.Staffs.ToList();

            WebGrid webGrid = new WebGrid(stafffs, canPage: false, canSort: false, rowsPerPage: 1000);
            int rowVal = 0;
            string gridData = webGrid.GetHtml(

                columns: webGrid.Columns(
                    webGrid.Column("Sr.No.", format: item => rowVal = rowVal + 1),
                    webGrid.Column(columnName: "StaffCode", header: "Staff Code"),
                    webGrid.Column(columnName: "EnrollNumber", header: "Enroll No"),
                    webGrid.Column(columnName: "FullName", header: "Full Name"),
                    webGrid.Column(columnName: "Contact", header: "Contact"),
                    webGrid.Column(columnName: "Email", header: "Email"),
                    webGrid.Column("Dob", "Date Of Birth",
                        format: (item) => item.Dob != null ? item.Dob.ToString("dd/MMM/yyyy") : "", canSort: true)
                    )).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Student-Details.xls");
            Response.ContentType = "applicatiom/excel";
            Response.Write(gridData);
            Response.End();
        }

        #endregion 

        #region --------------- ExportdatainPDF ----------------

        public ActionResult ExportDataInPDF()
        {
            List<Staff> staffs = new List<Staff>();
            staffs = db.Staffs.ToList();

            WebGrid webGrid = new WebGrid(staffs, canPage: false, canSort: false, rowsPerPage: 1000);
            int rowVal = 0;
            string gridData = webGrid.GetHtml(
                columns: webGrid.Columns(
                    webGrid.Column("Sr.No.", format: item => rowVal = rowVal + 1),
                    webGrid.Column(columnName: "StaffCode", header: "Staff Code"),
                    webGrid.Column(columnName: "EnrollNumber", header: "Enroll No"),
                    webGrid.Column(columnName: "FullName", header: "Full Name"),
                    webGrid.Column(columnName: "Contact", header: "Contact"),
                    webGrid.Column(columnName: "Email", header: "Email"),
                    //webGrid.Column(columnName: "Dob", header: "Dob"),
                    webGrid.Column("Dob", "Date Of Birth",
                        format: (item) => item.Dob != null ? item.Dob.ToString("dd/MMM/yyyy") : "", canSort: true)

                    )).ToString();

            StringBuilder pdfExportData = new StringBuilder();
            pdfExportData.AppendLine(
                "<html><head><style>.webgrid-table" +
                "{font-family:Arial,font-size:9px;font-weight: normal;width: 1000px;display:table ;border-collapse: collapse;border: solid px; #C5C5C5;background-color: white;}</style>");

            pdfExportData.AppendLine("</head><body>" + gridData + "</body></html>");

            var bytes = Encoding.UTF8.GetBytes(pdfExportData.ToString());
            using (var stream = new MemoryStream(bytes))
            {
                var outStream = new MemoryStream();
                var doc = new Document(PageSize.A4, 20, 20, 20, 20);
                var writer = PdfWriter.GetInstance(doc, outStream);
                writer.CloseStream = false;
                doc.Open();

                var xmlWorker = XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, doc, stream, Encoding.UTF8);
                doc.Close();
                outStream.Position = 0;
                return new FileStreamResult(outStream, "application/pdf");
            }
        }

        #endregion 

        #region ----------- UpdaloadStaffProfileImage ----------

        public ActionResult UploadStaffProfileImage()
        {
            string userName = User.Identity.GetUserName();
            var userExists = db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            int staffId = 0;
            Staff firstOrDefault = db.Staffs.FirstOrDefault(a => a.userId == userExists.Id);
            if (firstOrDefault != null)
            {
                staffId = firstOrDefault.staffId;
            }
            return View(firstOrDefault);
        }

        #endregion 

        #region ----- Upload Staff Profile Image Post ----------

        [HttpPost]
        public ActionResult UploadStaffProfileImage(int staffid, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                Staff staffs1 = new Staff();
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var rondom = Guid.NewGuid() + fileName;
                    var path = Path.Combine(Server.MapPath("~/StaffDocument/"), rondom);
                    file.SaveAs(path);
                    staffs1.passportPhoto = rondom;
                    ViewBag.Path = String.Format("~/StaffDocument/", fileName);
                }

                Staff staffs = db.Staffs.Find(staffid);
                staffs.passportPhoto = staffs1.passportPhoto;
                db.Staffs.AddOrUpdate(staffs);
                db.SaveChanges();
                return RedirectToAction("StaffProfile", "Staffs");
                //return RedirectToAction("StaffProfile", "Staffs", new { staffid = staffs.staffId });

            }

            return View();
        }

        #endregion 

        #region ---------------- Staff Search ------------------

        public ActionResult StaffSearch()
        {
            return View();
        }

        [HttpPost]
        public string StaffSearch(string str)
        {
            return str;
        }

        #endregion 

        #region -------------- Create Shift Get Code -----------

        public ActionResult CreateShift()
        {
            return View();
        }

        #endregion 

        #region ---------------- Create Shift post -------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShift(ShiftMasterModel shiftModel)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                ShiftMaster design = new ShiftMaster();

                design.shiftName = shiftModel.ShiftName;
                design.firmId = firm;
                design.startTime = new TimeSpan(shiftModel.Hour, shiftModel.Minute, 00);
                design.endTime = new TimeSpan(shiftModel.Hour1, shiftModel.Minute1, 00);
                if (design.startTime > design.endTime)
                {
                    design.TotolDuration = new TimeSpan(((24 - shiftModel.Hour) + shiftModel.Hour1) - 1,
                        (60 - shiftModel.Minute) + shiftModel.Minute1, 00);
                }
                else
                {
                    design.TotolDuration = new TimeSpan(shiftModel.Hour1, shiftModel.Minute1, 00) -
                                           new TimeSpan(shiftModel.Hour, shiftModel.Minute, 00);
                }
                db.ShiftMasters.Add(design);
                db.SaveChanges();
                return RedirectToAction("CreateShift");
            }
            return View(shiftModel);
        }

        #endregion 

        #region -------------------- Shift Index ---------------

        public ActionResult ShiftIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.ShiftMasters.Where(a => a.firmId == firm).ToList());
        }

        #endregion 

        #region ------------------ Edit Shift ------------------

        public ActionResult EditShift(int? id)
        {

            ShiftMaster @event = db.ShiftMasters.Find(id);
            var design = new ShiftMasterModel();
            TimeSpan st = (TimeSpan) @event.startTime;
            TimeSpan et = (TimeSpan) @event.endTime;

            design.ShiftId = (int) @event.shiftId;
            design.ShiftName = @event.shiftName;
            design.FirmId = (int) @event.firmId;
            design.StartTime = st;
            design.EndTime = et;
            design.Hour = st.Hours;
            design.Minute = st.Minutes;

            design.Hour1 = et.Hours;
            design.Minute1 = et.Minutes;

            return View(design);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShift(ShiftMasterModel shiftModel)
        {
            if (ModelState.IsValid)
            {
                ShiftMaster @event = db.ShiftMasters.Find(shiftModel.ShiftId);

                @event.shiftName = shiftModel.ShiftName;
                shiftModel.FirmId = (int) @event.firmId;
                @event.startTime = new TimeSpan(shiftModel.Hour, shiftModel.Minute, 00);

                @event.endTime = new TimeSpan(shiftModel.Hour1, shiftModel.Minute1, 00);

                if (@event.startTime > @event.endTime)
                {
                    @event.TotolDuration = shiftModel.Minute > shiftModel.Minute1
                        ? new TimeSpan((24 - shiftModel.Hour) + shiftModel.Hour1, shiftModel.Minute - shiftModel.Minute1,
                            00)
                        : new TimeSpan((24 - shiftModel.Hour) + shiftModel.Hour1, shiftModel.Minute1 - shiftModel.Minute,
                            00);
                }
                else
                {
                    @event.TotolDuration = new TimeSpan(shiftModel.Hour1, shiftModel.Minute1, 00) -
                                           new TimeSpan(shiftModel.Hour, shiftModel.Minute, 00);
                }

                db.ShiftMasters.AddOrUpdate(@event);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("CreateShift");
            }
            return View(shiftModel);
        }

        #endregion 

        #region --------------------- Edit Shift ---------------

        public ActionResult ShiftDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftMaster master = db.ShiftMasters.Find(id);
            if (master == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(master);
        }

        [HttpPost, ActionName("ShiftDelete")]
        public ActionResult ShiftDelete(int id)
        {
            try
            {
                ShiftMaster master = db.ShiftMasters.Find(id);
                db.ShiftMasters.Remove(master);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("CreateShift");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("CreateShift");
            }
        }

        #endregion 

        #region ------------------ Employee Tree ---------------

        public ActionResult EmployeeTree()
        {
            var treeView = GetTreeVeiwList();
            return View(treeView);
        }

        #endregion 

        #region ---------------- GetTreeviewList ---------------
        public TreeViewNodeVm GetTreeVeiwList()
        {
            Convert.ToInt32(LoginEmployeeId());
            var firm = LoginUserFirmId();
            var rootNode = (from e1 in db.Staffs
                where e1.reportingId == null && e1.firmId == firm 
                select new TreeViewNodeVm()
                {
                    Id = e1.staffId,
                    EmployeeCode = e1.staffCode,
                    EmployeeName = e1.firstName + " " + e1.lastName
                }).FirstOrDefault();
            BuildChildNode(rootNode);
            return rootNode;
        }

        #endregion  

        #region -------------- BuildClildnode ------------------

        private void BuildChildNode(TreeViewNodeVm rootNode)
        {
            if (rootNode != null)
            {
                List<TreeViewNodeVm> chidNode = (from e1 in db.Reportings.Include(s => s.Staff).Where(s=>s.Staff.isActive==true)
                    where e1.ReportingManagerId == rootNode.Id
                    select new TreeViewNodeVm()
                    {
                        Id = e1.Staff.staffId,
                        EmployeeCode = e1.Staff.staffCode,
                        EmployeeName = e1.Staff.firstName + " " + e1.Staff.lastName
                    }).ToList<TreeViewNodeVm>();
                if (chidNode.Count > 0)
                {
                    foreach (var childRootNode in chidNode)
                    {
                        BuildChildNode(childRootNode);
                        rootNode.ChildNode.Add(childRootNode);
                    }
                }
            }
        }

        #endregion 

        #region ------------------ Child Node ------------------

        public ActionResult ChildNode()
        {
            return PartialView();
        }

        #endregion 

        #region ------------------- Encrypt --------------------

        public string Encrypt(string plainText)
        {
            //string key = "jdsg432387#";
            const string key = "chan221988#";
            //byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] iv = {27, 98, 45, 23, 65, 173, 17, 88};
            var encryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var des = new DESCryptoServiceProvider();
            var inputByte = Encoding.UTF8.GetBytes(plainText);
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, des.CreateEncryptor(encryptKey, iv), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        #endregion 

        #region -------------------- InactiveEmployee ----------
        public ActionResult InactiveEmployee(string searchString, int? designationId, int? deptId)
        {
            ViewBag.designationId = new SelectList(db.Designations, "designationId", "designation1");
            //IQueryable<StaffModel> staffs = null;
            if (searchString != null || designationId != null)
            {
                if (searchString != "" && designationId != null)
                {
                    var staff1 =
                        db.Staffs.Where(
                            a =>
                                a.firstName.Contains(searchString) && a.designationId == designationId &&
                                a.isActive == false)
                            .Select(a => new StaffModel()
                            {
                                StaffId = a.staffId,
                                StaffCode = a.staffCode,
                                SchoolCode = a.schoolCode,
                                FirstName = a.firstName,
                                MiddleName = a.middleName,
                                LastName = a.lastName,
                                Gender = (int) a.gender,
                                Contact = a.contact,
                                Email = a.email,
                                DesignationId = (int) a.designationId,
                                Designation = a.Designation,
                                Department = a.Department,
                                Dob = a.dob,
                                ReportingTo = a.Staff2.firstName + " " + a.Staff2.lastName,
                                ReportingId = a.reportingId,
                                EnrollNumber = (int) a.enrollNumber,
                                PAN = a.PAN,
                                PF = a.PF,
                                UserId = a.userId
                            });
                    return View(staff1.ToList());
                }
                else if (searchString == "" && designationId != null)
                {
                    var staff2 =
                        db.Staffs.Where(a => a.designationId == designationId && a.isActive == false)
                            .Select(a => new StaffModel()
                            {
                                StaffId = a.staffId,
                                StaffCode = a.staffCode,
                                SchoolCode = a.schoolCode,
                                FirstName = a.firstName,
                                MiddleName = a.middleName,
                                LastName = a.lastName,
                                Gender = (int) a.gender,
                                Contact = a.contact,
                                Email = a.email,
                                Designation = a.Designation,
                                Department = a.Department,
                                Dob = a.dob,
                                ReportingTo = a.Staff2.firstName + " " + a.Staff2.lastName,
                                ReportingId = a.reportingId,
                                EnrollNumber = (int) a.enrollNumber,
                                PAN = a.PAN,
                                PF = a.PF,
                                UserId = a.userId

                            });
                    return View(staff2.ToList());
                }
                else if (searchString != "" && designationId == null)
                {
                    var staff3 =
                        db.Staffs.Where(
                            a => a.isActive == false &&
                                 a.firstName.Contains(searchString) || a.lastName.Contains(searchString) ||
                                 a.staffCode.Contains(searchString)).Select(a => new StaffModel()
                                 {
                                     StaffId = a.staffId,
                                     StaffCode = a.staffCode,
                                     SchoolCode = a.schoolCode,
                                     FirstName = a.firstName,
                                     MiddleName = a.middleName,
                                     LastName = a.lastName,
                                     Gender = (int) a.gender,
                                     Contact = a.contact,
                                     Email = a.email,
                                     Designation = a.Designation,
                                     Department = a.Department,
                                     Dob = a.dob,
                                     ReportingTo = a.Staff2.firstName + " " + a.Staff2.lastName,
                                     ReportingId = a.reportingId,
                                     EnrollNumber = (int) a.enrollNumber,
                                     PAN = a.PAN,
                                     PF = a.PF,
                                     UserId = a.userId
                                 });
                    return View(staff3.ToList());
                }
            }

            var staffs = db.Staffs.Where(a => a.isActive == false).Select(a => new StaffModel()
            {
                StaffId = a.staffId,
                StaffCode = a.staffCode,
                SchoolCode = a.schoolCode,
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                Gender = (int) a.gender,
                Contact = a.contact,
                Email = a.email,
                Designation = a.Designation,
                Department = a.Department,
                Dob = a.dob,
                ReportingTo = a.Staff2.firstName + " " + a.Staff2.lastName,
                ReportingId = a.reportingId,
                EnrollNumber = (int) a.enrollNumber,
                PAN = a.PAN,
                PF = a.PF,
                UserId = a.userId
            });
            return View(staffs.ToList());
        }

        #endregion 

        #region ------------------- Staff Esi Index ------------
        [HttpGet]
        public ActionResult staffEsiIndex()
        {
            var firm = LoginUserFirmId();
            var data = (from a in db.EsiInfoes
                join b in db.Staffs on a.staffid equals b.staffId
                where a.firmid == firm && a.isdeleted == false
                select
                    new
                    {
                        a.staffid,
                        a.branchoffice,
                        a.corpbranchoffice,
                        a.corpdob,
                        a.corpfathername,
                        a.corpinsno,
                        a.corponame,
                        a.dateofappoint,
                        a.dateofentry,
                        a.dispensary,
                        a.esiid,
                        a.fathername,
                        a.firmid,
                        a.nomaddress,
                        a.nomineename,
                        a.peraddress,
                        a.pin,
                        a.preempolyecode,
                        a.preinsuranceno,
                        a.relationship,
                        b.firstName,
                        b.lastName,
                        b.middleName,
                        b.gender,
                        b.isActive,
                        b.isMarried,
                        a.isdeleted
                    }).ToList();

            List<EsiinfoModel> esiinfomodel = new List<EsiinfoModel>();
            if (data.Count != 0)
            {
                esiinfomodel = data.Select(a => new EsiinfoModel()
                {
                    staffid1 = a.staffid,
                    branchoffice = a.branchoffice,
                    corpbranchoffice = a.corpbranchoffice,
                    corpdob = a.corpdob,
                    corpfathername = a.corpfathername,
                    corpinsno = a.corpinsno,
                    corponame = a.corponame,
                    dateofappoint = a.dateofappoint,
                    dateofentry = a.dateofentry,
                    dispensary = a.dispensary,
                    esiid = a.esiid,
                    fathername = a.fathername,
                    gender = a.gender,
                    Name = a.firstName + " " + a.middleName + " " + a.lastName,
                    marriedstatus = a.isMarried,
                    nomaddress = a.nomaddress,
                    nomineename = a.nomineename,
                    peraddress = a.peraddress,
                    pin = a.pin,
                    preempolyecode = a.preempolyecode,
                    preinsuranceno = a.preinsuranceno,
                    relationship = a.relationship
                }).ToList();
                return View(esiinfomodel);
            }
            return View(esiinfomodel);
        }

        #endregion 

        #region --------------- All-form partial view ----------

        [HttpPost]
        public ActionResult Allform(int SearchValue)
        {
            if (SearchValue == 1)
            {

            }
            if (SearchValue == 2)
            {
            }
            if (SearchValue == 3)
            {
            }
            if (SearchValue == 4)
            {
            }
            return View();
        }

        #endregion 

        #region ------------------ EsiForm ---------------------

        public ActionResult EsiForm()
        {
            var firm = LoginUserFirmId();
            var staffid = LoginEmployeeId();
            var data = (from a in db.EsiInfoes
                join b in db.Staffs on a.staffid equals b.staffId
                where a.firmid == firm && a.staffid == staffid
                select
                    new
                    {
                        a.staffid,
                        a.branchoffice,
                        a.corpbranchoffice,
                        a.corpdob,
                        a.corpfathername,
                        a.corpinsno,
                        a.corponame,
                        a.dateofappoint,
                        a.dateofentry,
                        a.dispensary,
                        a.esiid,
                        a.fathername,
                        a.firmid,
                        a.nomaddress,
                        a.nomineename,
                        a.peraddress,
                        a.pin,
                        a.preempolyecode,
                        a.preinsuranceno,
                        a.Employeraddressname,
                        a.relationship,
                        b.firstName,
                        b.lastName,
                        b.middleName,
                        b.gender,
                        b.isActive,
                        b.isMarried
                    }).ToList();
            // EsiinfoModel EsiinfoModel1 = new EsiinfoModelEsiinfoModel();
            if (data.Count() != 0)
            {
                var esiinfoModel = data.Select(a => new EsiinfoModel()
                {
                    //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                    branchoffice = a.branchoffice,
                    corpbranchoffice = a.corpbranchoffice,
                    corpdob = a.corpdob,
                    corpfathername = a.corpfathername,
                    corpinsno = a.corpinsno,
                    corponame = a.corponame,
                    dateofappoint = a.dateofappoint,
                    dateofentry = a.dateofentry,
                    dispensary = a.dispensary,
                    esiid = a.esiid,
                    fathername = a.fathername,
                    gender = (int) a.gender,
                    Name = a.firstName + " " + a.middleName + " " + a.lastName,
                    marriedstatus = a.isMarried,
                    nomaddress = a.nomaddress,
                    nomineename = a.nomineename,
                    peraddress = a.peraddress,
                    pin = a.pin,
                    preempolyecode = a.preempolyecode,
                    preinsuranceno = a.preinsuranceno,
                    relationship = a.relationship,
                    staffid1 = a.staffid,
                    Employeraddressname = a.Employeraddressname
                }).FirstOrDefault();
                return View(esiinfoModel);
            }
            return View();
        }

        #endregion 

        #region --------------- Esi Form Create ----------------
      
        public ActionResult EsiFormCreate(int? id)
        {

            if (id == null)
            {
                var staff = LoginEmployeeId();
                var data = db.Staffs.FirstOrDefault(a => a.staffId == staff);
                if (data == null) return View();
                var esiinfomodel = new EsiinfoModel
                {
                    staffid1 = Convert.ToInt32(data.staffId),
                    Name = data.firstName + " " + data.middleName + " " + data.lastName,
                    dateofbirth = data.dob,
                    fathername = data.middleName + " " + data.lastName,
                    marriedstatus = data.isMarried,
                    gender = data.gender,
                    peraddress = data.perAddress,
                    pin = data.perPincode,
                    Email = data.email
                };
                return View(esiinfomodel);
            }
            else
            {
                var data = db.Staffs.FirstOrDefault(a => a.staffId == id);
                if (data != null)
                {
                    var esiinfomodel = new EsiinfoModel
                    {
                        staffid1 = Convert.ToInt32(data.staffId),
                        Name = data.firstName + " " + data.middleName + " " + data.lastName,
                        dateofbirth = data.dob,
                        fathername = data.middleName + " " + data.lastName,
                        marriedstatus = data.isMarried,
                        gender = data.gender,
                        peraddress = data.perAddress,
                        pin = data.perPincode,
                        Email = data.email
                    };
                    return View(esiinfomodel);
                }
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult EsiCreatepost(EsiinfoModel esiinfomodel)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                EsiInfo esiinfo = new EsiInfo();
                esiinfo.branchoffice = esiinfomodel.branchoffice;
                esiinfo.corpbranchoffice = esiinfomodel.corpbranchoffice;
                esiinfo.corpDispensary = esiinfomodel.corpDispensary;
                esiinfo.corpdob = esiinfomodel.corpdob;
                esiinfo.corpfathername = esiinfomodel.corpfathername;
                esiinfo.corpinsno = esiinfomodel.corpinsno;
                esiinfo.corpnameaddresscode = esiinfo.corpnameaddresscode;
                esiinfo.corponame = esiinfomodel.corponame;
                esiinfo.CurEmployeraddressname = esiinfomodel.CurEmployeraddressname;
                esiinfo.dateofappoint = esiinfomodel.dateofappoint;
                esiinfo.dateofentry = esiinfomodel.dateofentry;
                esiinfo.dispensary = esiinfomodel.dispensary;
                esiinfo.Employeraddressname = esiinfomodel.Employeraddressname;
                esiinfo.fathername = esiinfomodel.fathername;
                esiinfo.firmid = firm;
                esiinfo.isdeleted = false;
                esiinfo.nomaddress = esiinfomodel.nomaddress;
                esiinfo.nomineename = esiinfomodel.nomineename;
                esiinfo.peraddress = esiinfomodel.peraddress;
                esiinfo.pin = esiinfomodel.pin;
                esiinfo.preempolyecode = esiinfomodel.preempolyecode;
                esiinfo.preinsuranceno = esiinfomodel.preinsuranceno;
                esiinfo.relationship = esiinfomodel.relationship;
                esiinfo.staffid = esiinfomodel.staffid1;
                esiinfo.corpnameaddresscode = esiinfomodel.corpnameaddresscode;
                db.EsiInfoes.Add(esiinfo);
                db.SaveChanges();
                TempData["notice"] = "success";
            }


            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = esiinfomodel.staffid1});
        }

        #endregion 

        #region ---------- Esi Delete and Confirm --------------

        
        [HttpGet]
        public ActionResult DeleteEsi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EsiInfo esiinfiid = db.EsiInfoes.Find(id);
            if (esiinfiid == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(esiinfiid);
        }

        
        [HttpPost, ActionName("DeleteEsi")]
        public ActionResult DeleteEsiconfirm(int? id)
        {
            try
            {
                EsiInfo esiinfo = db.EsiInfoes.Find(id);
                esiinfo.isdeleted = true;
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("Create");
            }
        }

        #endregion 

        #region --------------- Esi Edit Info ------------------

        [HttpGet]
        public ActionResult EsiinfoEdit(int id)
        {
            EsiinfoModel esiinfoModel;
            var firm = LoginUserFirmId();
            var data = (from a in db.EsiInfoes where a.staffid == id select a).FirstOrDefault();
            if (data != null)
            {
                var data1 = (from a in db.EsiInfoes
                    join b in db.Staffs on a.staffid equals b.staffId
                    where a.firmid == firm && a.staffid == id && a.isdeleted == false
                    select
                        new
                        {
                            a.branchoffice,
                            a.corpbranchoffice,
                            a.corpDispensary,
                            a.corpdob,
                            a.corpfathername,
                            a.corpinsno,
                            a.corpnameaddresscode,
                            a.corponame,
                            a.CurEmployeraddressname,
                            a.dateofappoint,
                            a.dateofentry,
                            a.dispensary,
                            a.Employeraddressname,
                            a.esiid,
                            a.fathername,
                            a.firmid,
                            a.isdeleted,
                            a.nomaddress,
                            a.nomineename,
                            a.peraddress,
                            a.pin,
                            a.preempolyecode,
                            a.preinsuranceno,
                            a.relationship,
                            a.staffid,
                            b.dob,
                            b.firstName,
                            b.lastName,
                            b.middleName,
                            b.gender,
                            b.isActive,
                            b.staffCode,
                            b.isMarried
                        }).ToList();
                esiinfoModel = data1.Select(a => new EsiinfoModel()
                {
                    //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                    staffid1 = a.staffid,
                    staffcode = a.staffCode,
                    dateofbirth = a.dob,
                    branchoffice = a.branchoffice,
                    corpbranchoffice = a.corpbranchoffice,
                    corpdob = a.corpdob,
                    corpfathername = a.corpfathername,
                    corpinsno = a.corpinsno,
                    corponame = a.corponame,
                    dateofappoint = a.dateofappoint,
                    dateofentry = a.dateofentry,
                    dispensary = a.dispensary,
                    esiid = a.esiid,
                    fathername = a.fathername,
                    gender = a.gender,
                    Name = a.firstName + " " + a.middleName + " " + a.lastName,
                    marriedstatus = a.isMarried,
                    nomaddress = a.nomaddress,
                    nomineename = a.nomineename,
                    peraddress = a.peraddress,
                    pin = a.pin,
                    preempolyecode = a.preempolyecode,
                    preinsuranceno = a.preinsuranceno,
                    relationship = a.relationship,
                    CurEmployeraddressname = a.CurEmployeraddressname,
                    corpDispensary = a.corpDispensary,
                    Employeraddressname = a.Employeraddressname,
                    corpnameaddresscode = a.corpnameaddresscode

                }).FirstOrDefault();

                return View(esiinfoModel);
            }
            else
            {
                return RedirectToAction("EsiFormCreate", "staffs", new {id});

            }
        }

      
        [HttpPost]

        public ActionResult esiinfoedit(EsiinfoModel esiinfomodel)
        {
            var firm = LoginUserFirmId();
            var esiinfo = new EsiInfo();
            //var result = db.EsiInfoes.SingleOrDefault(b => b.esiid == esiinfomodel.esiid);
            var data = (from a in db.EsiInfoes where a.esiid == esiinfomodel.esiid select a).FirstOrDefault();
            // var data= db.EsiInfoes.Find(esiinfomodel.esiid)

            if (data != null)
            {


                data.branchoffice = esiinfomodel.branchoffice;
                data.corpbranchoffice = esiinfomodel.corpbranchoffice;
                data.corpDispensary = esiinfomodel.corpDispensary;
                data.corpdob = esiinfomodel.corpdob;
                data.corpfathername = esiinfomodel.corpfathername;
                data.corpinsno = esiinfomodel.corpinsno;
                data.corpnameaddresscode = esiinfomodel.corpnameaddresscode;
                data.corponame = esiinfomodel.corponame;
                data.CurEmployeraddressname = esiinfomodel.CurEmployeraddressname;
                data.dateofappoint = esiinfomodel.dateofappoint;
                data.dateofentry = esiinfomodel.dateofentry;
                data.dispensary = esiinfomodel.dispensary;
                data.Employeraddressname = esiinfomodel.Employeraddressname;
                data.fathername = esiinfomodel.fathername;
                data.firmid = firm;
                data.isdeleted = false;
                data.nomaddress = esiinfomodel.nomaddress;
                data.nomineename = esiinfomodel.nomineename;
                data.peraddress = esiinfomodel.peraddress;
                data.pin = esiinfomodel.pin;
                data.preempolyecode = esiinfomodel.preempolyecode;
                data.preinsuranceno = esiinfomodel.preinsuranceno;
                data.relationship = esiinfomodel.relationship;
                data.staffid = esiinfomodel.staffid1;

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";

                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = esiinfomodel.staffid1});
            }
            //EsiInfo esiinfo = new EsiInfo();
            esiinfo.branchoffice = esiinfomodel.branchoffice;
            esiinfo.corpbranchoffice = esiinfomodel.corpbranchoffice;
            esiinfo.corpDispensary = esiinfomodel.corpDispensary;
            esiinfo.corpdob = esiinfomodel.corpdob;
            esiinfo.corpfathername = esiinfomodel.corpfathername;
            esiinfo.corpinsno = esiinfomodel.corpinsno;
            esiinfo.corpnameaddresscode = esiinfomodel.corpnameaddresscode;
            esiinfo.corponame = esiinfomodel.corponame;
            esiinfo.CurEmployeraddressname = esiinfomodel.CurEmployeraddressname;
            esiinfo.dateofappoint = esiinfomodel.dateofappoint;
            esiinfo.dateofentry = esiinfomodel.dateofentry;
            esiinfo.dispensary = esiinfomodel.dispensary;
            esiinfo.Employeraddressname = esiinfomodel.Employeraddressname;
            esiinfo.fathername = esiinfomodel.fathername;
            esiinfo.firmid = firm;
            esiinfo.isdeleted = false;
            esiinfo.nomaddress = esiinfomodel.nomaddress;
            esiinfo.nomineename = esiinfomodel.nomineename;
            esiinfo.peraddress = esiinfomodel.peraddress;
            esiinfo.pin = esiinfomodel.pin;
            esiinfo.preempolyecode = esiinfomodel.preempolyecode;
            esiinfo.preinsuranceno = esiinfomodel.preinsuranceno;
            esiinfo.relationship = esiinfomodel.relationship;
            esiinfo.staffid = esiinfomodel.staffid1;

            db.EsiInfoes.Add(esiinfo);
            db.SaveChanges();
            TempData["notice"] = "update";

            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = esiinfomodel.staffid1});
        }

        #endregion  

        #region --------------- Staff Esi EsiFormDetail --------

        /// <summary>
        /// Esi form -- See All Employee Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EsiFormDetail()
        {
            var firm = LoginUserFirmId();

            var data = (from a in db.EsiInfoes
                join b in db.Staffs on a.staffid equals b.staffId
                where a.firmid == firm && a.isdeleted == false
                select
                    new
                    {
                        a.staffid,
                        a.branchoffice,
                        a.corpbranchoffice,
                        a.corpdob,
                        a.corpfathername,
                        a.corpinsno,
                        a.corponame,
                        a.dateofappoint,
                        a.dateofentry,
                        a.dispensary,
                        a.esiid,
                        a.fathername,
                        a.firmid,
                        a.nomaddress,
                        a.nomineename,
                        a.peraddress,
                        a.pin,
                        a.preempolyecode,
                        a.preinsuranceno,
                        a.relationship,
                        b.firstName,
                        b.lastName,
                        b.middleName,
                        b.gender,
                        b.isActive,
                        b.isMarried,
                        a.isdeleted
                    }).ToList();
            List<EsiinfoModel> esiinfomodel = new List<EsiinfoModel>();
            if (data.Count != 0)
            {
                esiinfomodel = data.Select(a => new EsiinfoModel()
                {
                    //EmployeeName = a.employeeCode + ": " + a.EmployeeName,

                    branchoffice = a.branchoffice,
                    corpbranchoffice = a.corpbranchoffice,
                    corpdob = a.corpdob,
                    corpfathername = a.corpfathername,
                    corpinsno = a.corpinsno,
                    corponame = a.corponame,
                    dateofappoint = a.dateofappoint,
                    dateofentry = a.dateofentry,
                    dispensary = a.dispensary,
                    esiid = a.esiid,
                    fathername = a.fathername,
                    gender = a.gender,
                    Name = a.firstName + " " + a.middleName + " " + a.lastName,
                    marriedstatus = a.isMarried,
                    nomaddress = a.nomaddress,
                    nomineename = a.nomineename,
                    peraddress = a.peraddress,
                    pin = a.pin,
                    preempolyecode = a.preempolyecode,
                    preinsuranceno = a.preinsuranceno,
                    relationship = a.relationship
                }).ToList();
                return View(esiinfomodel);
            }
            return View(esiinfomodel);
        }

        #endregion

        #region ------------------ Esiinfodetails --------------

        public ActionResult Esiinfodetails(int esiid)
        {
            var data = (from a in db.EsiInfoes
                join b in db.Staffs on a.staffid equals b.staffId
                where a.isdeleted == false
                select
                    new
                    {
                        a.branchoffice,
                        a.corpbranchoffice,
                        a.corpDispensary,
                        a.corpdob,
                        a.corpfathername,
                        a.corpinsno,
                        a.corpnameaddresscode,
                        a.corponame,
                        a.CurEmployeraddressname,
                        a.dateofappoint,
                        a.dateofentry,
                        a.dispensary,
                        a.Employeraddressname,
                        a.esiid,
                        a.fathername,
                        a.firmid,
                        a.isdeleted,
                        a.nomaddress,
                        a.nomineename,
                        a.peraddress,
                        a.pin,
                        a.preempolyecode,
                        a.preinsuranceno,
                        a.relationship,
                        a.staffid,
                        b.firstName,
                        b.isMarried,
                        b.middleName,
                        b.lastName,
                        b.dob,
                        b.Designation,
                        b.gender
                    }).ToList();
            List<EsiinfoModel> esiinfomodel = new List<EsiinfoModel>();
            if (data.Count != 0)
            {
                esiinfomodel = data.Select(a => new EsiinfoModel()
                {

                    branchoffice = a.branchoffice,
                    corpbranchoffice = a.corpbranchoffice,
                    corpdob = a.corpdob,
                    corpnameaddresscode = a.corpnameaddresscode,
                    corpfathername = a.corpfathername,
                    corpDispensary = a.corpDispensary,
                    corpinsno = a.corpinsno,
                    corponame = a.corponame,
                    dateofappoint = a.dateofappoint,
                    dateofentry = a.dateofentry,
                    dispensary = a.dispensary,
                    esiid = a.esiid,
                    fathername = a.fathername,
                    gender = a.gender,
                    Name = a.firstName + " " + a.middleName + " " + a.lastName,
                    marriedstatus = a.isMarried,
                    nomaddress = a.nomaddress,
                    nomineename = a.nomineename,
                    peraddress = a.peraddress,
                    pin = a.pin,
                    preempolyecode = a.preempolyecode,
                    preinsuranceno = a.preinsuranceno,
                    relationship = a.relationship
                }).ToList();
            }
            return PartialView("_esiinfo", esiinfomodel);
        }

        #endregion 

        #region --------------------- Allpartial ---------------

        public ActionResult Allpartial(string id)
        {

            if (id == "esi")
            {

                var data = (from a in db.EsiInfoes
                    join b in db.Staffs on a.staffid equals b.staffId
                    where a.isdeleted == false
                    select
                        new
                        {
                            a.branchoffice,
                            a.corpbranchoffice,
                            a.corpDispensary,
                            a.corpdob,
                            a.corpfathername,
                            a.corpinsno,
                            a.corpnameaddresscode,
                            a.corponame,
                            a.CurEmployeraddressname,
                            a.dateofappoint,
                            a.dateofentry,
                            a.dispensary,
                            a.Employeraddressname,
                            a.esiid,
                            a.fathername,
                            a.firmid,
                            a.isdeleted,
                            a.nomaddress,
                            a.nomineename,
                            a.peraddress,
                            a.pin,
                            a.preempolyecode,
                            a.preinsuranceno,
                            a.relationship,
                            a.staffid,
                            b.firstName,
                            b.isMarried,
                            b.middleName,
                            b.lastName,
                            b.dob,
                            b.Designation,
                            b.gender
                        }).ToList();
                List<EsiinfoModel> esiinfomodel = new List<EsiinfoModel>();
                if (data.Count != 0)
                {
                    esiinfomodel = data.Select(a => new EsiinfoModel()
                    {

                        branchoffice = a.branchoffice,
                        corpbranchoffice = a.corpbranchoffice,
                        corpdob = a.corpdob,
                        corpfathername = a.corpfathername,
                        corpinsno = a.corpinsno,
                        corponame = a.corponame,
                        dateofappoint = a.dateofappoint,
                        dateofentry = a.dateofentry,
                        dispensary = a.dispensary,
                        esiid = a.esiid,
                        fathername = a.fathername,
                        gender = a.gender,
                        Name = a.firstName + " " + a.middleName + " " + a.lastName,
                        marriedstatus = a.isMarried,
                        nomaddress = a.nomaddress,
                        nomineename = a.nomineename,
                        peraddress = a.peraddress,
                        pin = a.pin,
                        preempolyecode = a.preempolyecode,
                        preinsuranceno = a.preinsuranceno,
                        relationship = a.relationship
                    }).ToList();
                }
                return PartialView("_esiinfo", esiinfomodel);
            }


            return PartialView(id);
        }

        #endregion 

        #region ------------------ EsifamilyInfo ---------------

        public ActionResult EsifamilyInfo()
        {

            return View();
        }

        #endregion 

        #region ---------------- EpfDetails --------------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EpfDetails(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }
            var epfmodel = new EPFModel();
            try
            {

                var data = (from a in db.EPFDetails where a.Staffid == staffId select a).FirstOrDefault();

                if (data != null)
                {
                    var data2 = (from a in db.Staffs
                        join b in db.EPFDetails on a.staffId equals b.Staffid
                        where b.Staffid == staffId
                        select
                            new
                            {
                                a.isMarried,
                                a.joingDate,
                                a.dob,
                                a.staffId,
                                a.staffCode,
                                a.tempAddress,
                                a.perAddress,
                                a.firstName,
                                a.lastName,
                                a.middleName,
                                a.gender,
                                b.EpfAccountno,
                                b.Createddate,
                                b.IsDeleted,
                                b.EpfId
                            }).FirstOrDefault();
                    if (data2 == null) return View(epfmodel);
                    epfmodel.dob = data2.dob;
                    epfmodel.doj = data2.joingDate;
                    epfmodel.gender = data2.gender;
                    epfmodel.marriedstatus = data2.isMarried;
                    epfmodel.Name = data2.firstName + " " + data2.middleName + " " + data2.lastName;
                    epfmodel.fathername = data2.middleName + " " + data2.lastName;
                    epfmodel.perAddress = data2.perAddress;
                    epfmodel.tempAddress = data2.tempAddress;
                    epfmodel.staffcode = data2.staffCode;
                    epfmodel.staffid = data2.staffId;
                    epfmodel.EPFAccountNo = data2.EpfAccountno;
                    return View(epfmodel);
                }
                else
                {
                    var data2 = (from a in db.Staffs where a.staffId == staffId select a).FirstOrDefault();
                    if (data2 == null) return View(epfmodel);
                    epfmodel.dob = data2.dob;
                    epfmodel.doj = data2.joingDate;
                    epfmodel.gender = data2.gender;
                    epfmodel.marriedstatus = data2.isMarried;
                    epfmodel.Name = data2.firstName + " " + data2.middleName + " " + data2.lastName;
                    epfmodel.fathername = data2.middleName + " " + data2.lastName;
                    epfmodel.perAddress = data2.perAddress;
                    epfmodel.tempAddress = data2.tempAddress;
                    epfmodel.staffcode = data2.staffCode;
                    epfmodel.staffid = data2.staffId;
                    return View(epfmodel);
                }



            }
            catch (Exception)
            {
                return RedirectToAction("pagenotfound", "Home");
                //   throw;
            }
        }

        #endregion 

        #region ------------------- EsiDetails -----------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EsiDetails(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }
            var data = (from a in db.EsiInfoes
                join b in db.Staffs on a.staffid equals b.staffId
                where a.isdeleted == false && a.staffid == staffId
                select
                    new
                    {
                        a.branchoffice,
                        a.corpbranchoffice,
                        a.corpdob,
                        a.corpfathername,
                        a.corpDispensary,
                        a.corpinsno,
                        a.corpnameaddresscode,
                        a.corponame,
                        a.CurEmployeraddressname,
                        a.dateofappoint,
                        a.dateofentry,
                        a.dispensary,
                        a.Employeraddressname,
                        a.esiid,
                        a.fathername,
                        a.firmid,
                        a.isdeleted,
                        a.nomaddress,
                        a.nomineename,
                        a.peraddress,
                        a.pin,
                        a.preempolyecode,
                        a.preinsuranceno,
                        a.relationship,
                        a.staffid,
                        b.firstName,
                        b.isMarried,
                        b.middleName,
                        b.lastName,
                        b.dob,
                        b.Designation,
                        b.staffCode,
                        b.gender
                    }).ToList();
            var esiinfomodel = new EsiinfoModel();
            if (data.Count != 0)
            {
                esiinfomodel = data.Select(a => new EsiinfoModel()
                {
                    staffid1 = a.staffid,
                    staffcode = a.staffCode,
                    branchoffice = a.branchoffice,
                    corpbranchoffice = a.corpbranchoffice,
                    corpnameaddresscode = a.corpnameaddresscode,
                    corpDispensary = a.corpDispensary,
                    corpdob = a.corpdob,
                    corpfathername = a.corpfathername,
                    corpinsno = a.corpinsno,
                    corponame = a.corponame,
                    dateofappoint = a.dateofappoint,
                    dateofentry = a.dateofentry,
                    dispensary = a.dispensary,
                    esiid = a.esiid,
                    fathername = a.fathername,
                    gender = a.gender,
                    Name = a.firstName + " " + a.middleName + " " + a.lastName,
                    marriedstatus = a.isMarried,
                    nomaddress = a.nomaddress,
                    nomineename = a.nomineename,
                    peraddress = a.peraddress,
                    pin = a.pin,
                    preempolyecode = a.preempolyecode,
                    preinsuranceno = a.preinsuranceno,
                    relationship = a.relationship
                }).FirstOrDefault();
            }
            else
            {
                var data2 = (from a in db.Staffs where a.staffId == staffId select a).FirstOrDefault();

                if (data2 == null) return View(esiinfomodel);
                esiinfomodel.staffid1 = data2.staffId;
                esiinfomodel.staffcode = data2.staffCode;
                esiinfomodel.Name = data2.firstName + " " + data2.middleName + " " + data2.lastName;
                esiinfomodel.dateofbirth = data2.dob;
                esiinfomodel.pin = data2.perPincode;
                esiinfomodel.peraddress = data2.perAddress;
            }
            return View(esiinfomodel);
        }

        #endregion 

        #region ------------------- Epf Info Get ---------------

        [HttpGet]
        public ActionResult EpfinfoEdit(int? id)
        {
            var epfModel = new EPFModel();
            var firm = LoginUserFirmId();
            var data = (from a in db.EPFDetails where a.Staffid == id select a).FirstOrDefault();
            if (data != null)
            {
                var data1 = (from a in db.EPFDetails
                    join b in db.Staffs on a.Staffid equals b.staffId
                    where a.IsDeleted == false && a.Staffid == id
                    select
                        new
                        {
                            a.firmid,
                            a.EpfAccountno,
                            a.EpfId,
                            a.IsDeleted,
                            a.Staffid,
                            b.joingDate,
                            b.dob,
                            b.perAddress,
                            b.tempAddress,
                            b.firstName,
                            b.lastName,
                            b.middleName,
                            b.gender,
                            b.isActive,
                            b.staffCode,
                            b.isMarried
                        }).FirstOrDefault();
                if (data1 == null) return View(epfModel);
                epfModel.Name = data1.firstName + " " + data1.middleName + " " + data1.lastName;
                epfModel.EPFAccountNo = data1.EpfAccountno;
                epfModel.EpFid = data1.EpfId;
                epfModel.dob = data1.dob;
                epfModel.fathername = data1.middleName + " " + data1.lastName;
                epfModel.doj = data1.joingDate;
                epfModel.gender = data1.gender;
                epfModel.marriedstatus = data1.isMarried;
                epfModel.perAddress = data1.perAddress;
                epfModel.tempAddress = data1.tempAddress;
                epfModel.staffcode = data1.staffCode;
                epfModel.staffid = (int) data1.Staffid;


                return View(epfModel);
            }
            else
            {
                return RedirectToAction("EpfCreate", "staffs", new {staffid = id});

            }





        }

        #endregion

        #region ------------- EPF info Edit --------------------

        [HttpPost]
        public ActionResult EpfinfoEdit(EPFModel epfModel)
        {

            var data = (from a in db.EPFDetails where a.Staffid == epfModel.staffid select a).FirstOrDefault();
            if (data != null)
            {

                data.EpfAccountno = epfModel.EPFAccountNo;
                data.IsDeleted = false;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = epfModel.staffid});
            }
            else
            {
                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = epfModel.staffid});
            }


        }

        #endregion

        #region --------------- EPF Info Create ----------------

        [HttpGet]
        public ActionResult EpfCreate(int? staffid)
        {
            var epfModel = new EPFModel();
            var data = db.Staffs.FirstOrDefault(a => a.staffId == staffid);
            if (data == null) return View(epfModel);
            epfModel.staffid = data.staffId;
            epfModel.staffcode = data.staffCode;
            epfModel.tempAddress = data.tempAddress;
            epfModel.perAddress = data.perAddress;
            epfModel.dob = data.dob;
            epfModel.doj = data.joingDate;
            epfModel.marriedstatus = data.isMarried;
            epfModel.gender = data.gender;
            epfModel.fathername = data.middleName + " " + data.lastName;
            epfModel.Name = data.firstName + " " + data.middleName + " " + data.lastName;
            return View(epfModel);

        }

        #endregion

        #region ------------------ EPF Create Post -------------

        [HttpPost]
        public ActionResult EpfCreatePost(EPFModel epfModel)
        {
            try
            {
                var firm = LoginUserFirmId();
                if (!ModelState.IsValid) return View();
                var epfDetail = new EPFDetail
                {
                    EpfAccountno = epfModel.EPFAccountNo,
                    IsDeleted = false,
                    Staffid = epfModel.staffid,
                    firmid = firm,
                    Createddate = DateTime.Now
                };
                db.EPFDetails.Add(epfDetail);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = epfModel.staffid});
            }
            catch (Exception)
            {
                return RedirectToAction("pagenotfound", "Home");
                // throw;
            }

        }

        #endregion

        #region ------------- Insurance List -------------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult StaffInsuranceIndex(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }
            var insinfomodelModel = new InsuranceinfoModel();
            var staffid = LoginEmployeeId();
            try
            {
                if (staffId != null)
                {
                    var data = (from a in db.Staffs
                        join b in db.insuranceinfoes on a.staffId equals b.staffid
                        where b.staffid == staffId
                        select
                            new
                            {
                                a.staffId,
                                a.staffCode,
                                a.firstName,
                                a.middleName,
                                a.lastName,
                                a.joingDate,
                                a.isMarried,
                                a.gender,
                                b.IsDeleted,
                                b.expirydate,
                                b.firmid,
                                b.insid,
                                b.policydate,
                                b.policyno,
                                b.staffid
                            }).FirstOrDefault();
                    if (data != null)
                    {
                        insinfomodelModel.Expirydate = data.expirydate;
                        insinfomodelModel.Insid = data.insid;
                        insinfomodelModel.IsDeleted = data.IsDeleted;
                        insinfomodelModel.Policydate = data.policydate;
                        insinfomodelModel.Policyno = data.policyno;
                        insinfomodelModel.staffid = data.staffid;
                        insinfomodelModel.Name = data.firstName + " " + data.middleName + " " + data.lastName;
                        insinfomodelModel.Staffcode = data.staffCode;

                        return View(insinfomodelModel);
                    }
                    var data1 = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                    if (data1 == null) return View(insinfomodelModel);
                    insinfomodelModel.Name = data1.firstName + " " + data1.middleName + " " + data1.lastName;
                    insinfomodelModel.Staffcode = data1.staffCode;
                    insinfomodelModel.staffid = data1.staffId;
                    return View(insinfomodelModel);
                }

                else
                {
                    var data = (from a in db.Staffs
                        join b in db.insuranceinfoes on a.staffId equals b.staffid
                        where b.staffid == staffid && a.isActive == true
                        select
                            new
                            {
                                a.staffId,
                                a.staffCode,
                                a.firstName,
                                a.middleName,
                                a.lastName,
                                a.joingDate,
                                a.isMarried,
                                a.gender,
                                b.IsDeleted,
                                b.expirydate,
                                b.firmid,
                                b.insid,
                                b.policydate,
                                b.policyno,
                                b.staffid
                            }).FirstOrDefault();
                    if (data != null)
                    {
                        insinfomodelModel.Expirydate = data.expirydate;
                        insinfomodelModel.Insid = data.insid;
                        insinfomodelModel.IsDeleted = data.IsDeleted;
                        insinfomodelModel.Policydate = data.policydate;
                        insinfomodelModel.Policyno = data.policyno;
                        insinfomodelModel.staffid = data.staffid;
                        insinfomodelModel.Name = data.firstName + " " + data.middleName + " " + data.lastName;
                        insinfomodelModel.Staffcode = data.staffCode;

                        return View(insinfomodelModel);
                    }
                    var data1 = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                    if (data1 == null) return View(insinfomodelModel);
                    insinfomodelModel.Name = data1.firstName + " " + data1.middleName + " " + data1.lastName;
                    insinfomodelModel.Staffcode = data1.staffCode;
                    insinfomodelModel.staffid = data1.staffId;
                    return View(insinfomodelModel);
                }

            }
            catch (Exception)
            {
                return RedirectToAction("pagenotfound", "Home");
                //throw;
            }

        }

        #endregion

        #region  ----------- Insurance Information Edit ---------
        public ActionResult Editinsuranceinfo(int? id)
        {
            InsuranceinfoModel insuranceinfoModel = new InsuranceinfoModel();
            var data = db.insuranceinfoes.FirstOrDefault(a => a.staffid == id);
            var data2 = db.Staffs.FirstOrDefault(a => a.staffId == id);
            if (data != null)
            {
                insuranceinfoModel.Insid = data.insid;
                insuranceinfoModel.Expirydate = data.expirydate;
                insuranceinfoModel.Policyno = data.policyno;
                insuranceinfoModel.Policydate = data.policydate;
                insuranceinfoModel.IsDeleted = data.IsDeleted;
                insuranceinfoModel.staffid = data.staffid;
                if (data2 == null) return View(insuranceinfoModel);
                insuranceinfoModel.Staffcode = data2.staffCode;
                insuranceinfoModel.Name = data2.firstName + " " + data2.middleName + " " + data2.lastName;
            }
            else
            {              
                return RedirectToAction("InsuranceCreate", "Staffs", new {staffid = id});
            }
            return View(insuranceinfoModel);
        }

        public ActionResult Editinsuranceinfopost(InsuranceinfoModel insuranceinfoModel)
        {
            var firm = LoginUserFirmId();
            insuranceinfo lb1 = db.insuranceinfoes.Find(insuranceinfoModel.Insid);
            {
                lb1.expirydate = insuranceinfoModel.Expirydate;
                lb1.policyno = insuranceinfoModel.Policyno;
                lb1.policydate = insuranceinfoModel.Policydate;
                lb1.IsDeleted = insuranceinfoModel.IsDeleted;
                lb1.staffid = insuranceinfoModel.staffid;
                lb1.firmid = firm;
            }
            db.insuranceinfoes.AddOrUpdate(lb1);
            db.SaveChanges();

            //var data = db.insuranceinfoes.FirstOrDefault(a => a.staffid == insuranceinfoModel.Insid);
            //if (data == null)
            //    return RedirectToAction("StaffDashboard", "Staffs", new { staffid = insuranceinfoModel.staffid });
            //insuranceinfoModel.Expirydate = data.expirydate;
            //insuranceinfoModel.Policyno = data.policyno;
            //insuranceinfoModel.Policydate = data.policydate;
            //insuranceinfoModel.IsDeleted = data.IsDeleted;
            //insuranceinfoModel.staffid = data.staffid;
            //insuranceinfoModel.Firmid = firm;
            //db.insuranceinfoes.AddOrUpdate(insuranceinfoModel);
            //db.SaveChanges();

            TempData["notice"] = "update";
            return RedirectToAction("StaffDashboard", "Staffs", new { staffid = insuranceinfoModel.staffid });
        }

        #endregion

        #region -------------- Insurance Create ----------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult InsuranceCreate(int? staffid)
        {
            var insuranceinfoModel = new InsuranceinfoModel();
            var data = db.Staffs.FirstOrDefault(a => a.staffId == staffid);
            if (data == null) return View(insuranceinfoModel);
            insuranceinfoModel.Staffcode = data.staffCode;
            insuranceinfoModel.staffid = data.staffId;
            insuranceinfoModel.Name = data.firstName + " " + data.middleName + " " + data.lastName;

            return View(insuranceinfoModel);
        }

        public ActionResult InsuranceCreatepost(InsuranceinfoModel insuranceinfoModel)
        {
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {
                    var insuranceinfo = new insuranceinfo
                    {
                        firmid = firm,
                        expirydate = insuranceinfoModel.Expirydate,
                        IsDeleted = false,
                        policydate = insuranceinfoModel.Policydate,
                        staffid = insuranceinfoModel.staffid,
                        policyno = insuranceinfoModel.Policyno


                    };
                    db.insuranceinfoes.Add(insuranceinfo);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("StaffDashboard", "Staffs", new {staffid = insuranceinfoModel.staffid});
                }


            }
            catch (Exception)
            {
                TempData["notice"] = "error";
                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = insuranceinfoModel.staffid});
                // throw;
            }
            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = insuranceinfoModel.staffid});
        }

        #endregion

        #region -------------- Undertaking Details -------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult UndertakingDetails(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }
            var undertakingModel = new UndertakingModel();
            var undertakingDetails = (from a in db.Staffs
                join b in db.Undertakings on a.staffId equals b.staffid
                where b.staffid == staffId
                select
                    new
                    {
                        a.staffId,
                        a.staffCode,
                        a.firstName,
                        a.middleName,
                        a.lastName,
                        a.firmId,
                        a.designationId,
                        a.gender,
                        a.isActive,
                        a.isMarried,
                        a.joingDate,
                        a.dob,
                        b.IsDeleted,
                        b.underid,
                        b.undertakingcheck
                    }).FirstOrDefault();
            if (undertakingDetails != null)
            {
                var comp = db.FirmInfoes.FirstOrDefault(a => a.firmId == undertakingDetails.firmId);
                undertakingModel.CompanyName = comp.firmName;
                undertakingModel.JoinDate = undertakingDetails.joingDate;
                var desg = db.Designations.FirstOrDefault(a => a.designationId == undertakingDetails.designationId);
                undertakingModel.Position = desg.designation1;
                undertakingModel.StaffId = undertakingDetails.staffId;
                undertakingModel.Underid = undertakingDetails.underid;
                undertakingModel.Name = undertakingDetails.firstName + " " + undertakingDetails.middleName + " " +
                                        undertakingDetails.lastName;
                undertakingModel.Undertakingcheck = (bool) undertakingDetails.undertakingcheck;
            }
            else
            {
                var data = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                if (data == null) return View(undertakingModel);
                undertakingModel.StaffId = data.staffId;
                undertakingModel.Name = data.firstName + " " + data.middleName + " " + data.lastName;
                undertakingModel.JoinDate = data.joingDate;
                var desgnation = db.Designations.FirstOrDefault(a => a.designationId == data.designationId);
                var comname = db.FirmInfoes.FirstOrDefault(a => a.firmId == data.firmId);
                if (comname != null) undertakingModel.CompanyName = comname.firmName;
                // undertakingModel.Position = data.Designation.designation1;
                if (desgnation != null) undertakingModel.Position = desgnation.designation1;
            }

            return View(undertakingModel);
        }

        #endregion

        #region ---------------- Undertaking Edit --------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EditUndertaking(int? id)
        {
            var undertakingModel = new UndertakingModel();
            var data = (from a in db.Undertakings
                join b in db.Staffs on a.staffid equals b.staffId
                where a.staffid == id
                select
                    new
                    {
                        a.undertakingcheck,
                        a.underid,
                        a.IsDeleted,
                        a.firmid,
                        a.staffid,
                        b.joingDate,
                        b.designationId,
                        b.firstName,
                        b.middleName,
                        b.lastName
                    }).FirstOrDefault();

            if (data != null)
            {
                var firm = LoginUserFirmId();

                undertakingModel.Underid = data.underid;
                undertakingModel.Name = data.firstName + " " + data.middleName + " " + data.lastName;
                if (data.undertakingcheck != null) undertakingModel.Undertakingcheck = (bool) data.undertakingcheck;
                undertakingModel.StaffId = data.staffid;
                undertakingModel.JoinDate = data.joingDate;
                var desgnation = db.Designations.FirstOrDefault(a => a.designationId == data.designationId);
                if (desgnation != null) undertakingModel.Position = desgnation.designation1;
                undertakingModel.IsDeleted = data.IsDeleted;
                var comname = db.FirmInfoes.FirstOrDefault(a => a.firmId == firm);
                if (comname != null) undertakingModel.CompanyName = comname.firmName;
                if (desgnation != null) undertakingModel.Position = desgnation.designation1;

            }
            else
            {
                return RedirectToAction("UndertakingCreate", "Staffs", new {staffid = id});
                //return RedirectToAction("UndertakingCreate", "staffs");
            }
            return View(undertakingModel);
        }

        public ActionResult EditUndertakingpost(UndertakingModel undertakingModel)
        {
            var firm = LoginUserFirmId();
            Undertaking undertaking = new Undertaking();
            var data = db.Undertakings.FirstOrDefault(a => a.staffid == undertakingModel.StaffId);
            if (data != null)
            {
                data.staffid = undertakingModel.StaffId;
                data.undertakingcheck = undertakingModel.Undertakingcheck;
                data.IsDeleted = false;
                data.firmid = firm;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("StaffDashboard", "Staffs", new {staffid = undertakingModel.StaffId});
            }
            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = undertakingModel.StaffId});
        }


        #endregion 

        #region ----------------- Undertaking Create -----------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult UndertakingCreate(int? staffid)
        {
            var firm = LoginUserFirmId();
            var firmname = db.FirmInfoes.Where(s => s.firmId == firm).FirstOrDefault().firmName;
            UndertakingModel undertaking = new UndertakingModel();
            var data = db.Staffs.FirstOrDefault(a => a.staffId == staffid);
            if (data != null)
            {
                undertaking.Name = data.firstName + " " + data.middleName + " " + data.lastName;
                undertaking.Position = data.Designation.designation1;
                undertaking.JoinDate = data.joingDate;
                undertaking.StaffId = data.staffId;
                undertaking.Firmid = data.firmId;
                undertaking.CompanyName = firmname;
            }
            return View(undertaking);
        }

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Undertakingpost(UndertakingModel undertakingModel)
        {
            Undertaking undertaking = new Undertaking();
            if (ModelState.IsValid)
            {
                undertaking.staffid = undertakingModel.StaffId;
                undertaking.undertakingcheck = undertakingModel.Undertakingcheck;
                undertaking.IsDeleted = false;
                db.Undertakings.Add(undertaking);
                db.SaveChanges();
                TempData["notice"] = "success";
            }
            return RedirectToAction("StaffDashboard", "Staffs", new {staffid = undertakingModel.StaffId});
        }

        #endregion

        #region --------------- Gratuity Details ---------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult GratuityDetails(int? staffId)
        {
            try
            {
            }
            catch (Exception)
            {
                //throw;
            }
            return View();
        }

        #endregion

        #region ---------------- Nominee Index -----------------

        public ActionResult NomineeIndex(string status, int? id)
        {

            var loginuser = LoginEmployeeId();
            if (loginuser == id)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == id);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }

            var data =
                (from a in db.NomineeDetails
                    where a.Staffid == id && a.nomineestatus == status && a.isdeleted == false
                    select a).ToList();
            return View(data);
        }

        #endregion 

        #region ------------------ Nominees Create -------------

        [HttpGet]

        public ActionResult NomineeCreate(int? id, string status)
        {

            var loginuser = LoginEmployeeId();
            if (loginuser == id)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == id);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }

            var nomineeDetail = new NomineeDetailModel {Nomineestatus = status, Staffid = id};

            switch (status)
            {
                case "ESI":
                    var data = db.EsiInfoes.FirstOrDefault(a => a.staffid == id);
                    if (data == null)
                    {
                        TempData["notice"] = "addform";
                        return RedirectToAction("StaffDashboard", "Staffs", new {staffid = id});
                    }

                    break;
                case "EPF":
                    var data1 = db.EPFDetails.FirstOrDefault(a => a.Staffid == id);
                    if (data1 == null)
                    {
                        TempData["notice"] = "addform";
                        return RedirectToAction("StaffDashboard", "Staffs", new {staffid = id});
                    }

                    break;
                case "Gratuity":
                    var data2 = db.Undertakings.FirstOrDefault(a => a.staffid == id);
                    if (data2 == null)
                    {
                        TempData["notice"] = "addform";
                        return RedirectToAction("StaffDashboard", "Staffs", new {staffid = id});
                    }

                    break;
                case "Insurance":
                    var data3 = db.insuranceinfoes.FirstOrDefault(a => a.staffid == id);
                    if (data3 == null)
                    {
                        TempData["notice"] = "addform";
                        return RedirectToAction("StaffDashboard", "Staffs", new {staffid = id});
                    }

                    break;
                default:
                    TempData["notice"] = "addform";
                    return RedirectToAction("StaffDashboard", "Staffs", new {staffid = id});


            }

            return View(nomineeDetail);
        }

        [HttpPost]
        public ActionResult NomineeCreatepost(NomineeDetailModel nomineeDetailModel)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                var nominee = new NomineeDetail
                {
                    firmid = firm,
                    isdeleted = false,
                    nomineeaddress = nomineeDetailModel.Nomineeaddress,
                    nomineeralation = nomineeDetailModel.Nomineeralation,
                    share = nomineeDetailModel.Share,
                    dob = nomineeDetailModel.Dob,
                    age = nomineeDetailModel.Age,
                    CreatedDate = DateTime.Now,
                    nomineename = nomineeDetailModel.Nomineename,
                    nomineestatus = nomineeDetailModel.Nomineestatus,
                    Staffid = nomineeDetailModel.Staffid
                };
                db.NomineeDetails.Add(nominee);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("NomineeCreate", "Staffs",
                    new {status = nomineeDetailModel.Nomineestatus, id = nomineeDetailModel.Staffid});
            }
            else
            {
                TempData["notice"] = "error";
                return RedirectToAction("NomineeCreate", "Staffs",
                    new {status = nomineeDetailModel.Nomineestatus, id = nomineeDetailModel.Staffid});
            }
            //   return View();
        }

        #endregion

        #region --------------- Nominee Edit -------------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Editnominee(int? id)
        {
            NomineeDetailModel nomineeDetailModel = new NomineeDetailModel();
            var data = db.NomineeDetails.FirstOrDefault(a => a.nomineeid == id && a.isdeleted == false);
            if (data != null)
            {
                nomineeDetailModel.Nomineeaddress = data.nomineeaddress;
                nomineeDetailModel.Nomineename = data.nomineename;
                nomineeDetailModel.Nomineestatus = data.nomineestatus;
                nomineeDetailModel.Nomineeralation = data.nomineeralation;
                nomineeDetailModel.Share = data.share;
                nomineeDetailModel.Dob = data.dob;
                nomineeDetailModel.Age = data.age;
                nomineeDetailModel.Staffid = data.Staffid;
                nomineeDetailModel.Isdeleted = data.isdeleted;
                nomineeDetailModel.Nomineeid = data.nomineeid;
            }
            return View(nomineeDetailModel);
        }

        public ActionResult NomineeEditpost(NomineeDetailModel nomineeDetailModel)
        {

            if (ModelState.IsValid)
            {

                var data =
                    (from a in db.NomineeDetails
                        where a.isdeleted == false && a.nomineeid == nomineeDetailModel.Nomineeid
                        select a).FirstOrDefault();
                if (data != null)
                {
                    data.nomineename = nomineeDetailModel.Nomineename;
                    data.nomineeaddress = nomineeDetailModel.Nomineeaddress;
                    data.nomineeralation = nomineeDetailModel.Nomineeralation;
                    data.nomineestatus = nomineeDetailModel.Nomineestatus;
                    data.share = nomineeDetailModel.Share;
                    data.age = nomineeDetailModel.Age;
                    data.dob = nomineeDetailModel.Dob;
                    data.nomineeid = nomineeDetailModel.Nomineeid;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                    return RedirectToAction("NomineeCreate", "Staffs",
                        new {status = nomineeDetailModel.Nomineestatus, id = nomineeDetailModel.Staffid});
                }
                else
                {
                    TempData["notice"] = "error";
                    return RedirectToAction("NomineeCreate", "Staffs",
                        new {status = nomineeDetailModel.Nomineestatus, id = nomineeDetailModel.Staffid});
                }
            }
            return RedirectToAction("NomineeCreate", "Staffs",
                new {status = nomineeDetailModel.Nomineestatus, id = nomineeDetailModel.Staffid});
        }

        #endregion

        #region ---------------- Nominee Delete ----------------

        public ActionResult DeleteNominee(int id)
        {
            var nomineeDetailModel = new NomineeDetailModel {Nomineeid = id};
            return View(nomineeDetailModel);
        }

        public ActionResult DeleteNomineeConfirm(NomineeDetailModel nomineeDetailModel)
        {
            var data = (db.NomineeDetails.Where(a => a.nomineeid == nomineeDetailModel.Nomineeid)).FirstOrDefault();
            if (data != null)
            {
                //   data.isdeleted = true;
                // db.Entry(data).State = EntityState.Modified;
                db.NomineeDetails.Remove(data);
                db.SaveChanges();
                TempData["notice"] = "delete";
            }

            return RedirectToAction("NomineeCreate", "Staffs", new {status = data.nomineestatus, id = data.Staffid});
        }


        #endregion 

        #region --------------- Nominee Details ----------------

        public ActionResult NomineesDetails(int id)
        {
            return View();
        }

        public ActionResult NomineesDetails()
        {

            return View();
        }

        #endregion 

        #region ------------ Esi Family Index [List] -----------

        public ActionResult EsiFamilyIndex(int? id)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == id)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == id);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }

            var data = db.EsiFamilies.Where(a => a.staffid == id && a.isdeleted == false).ToList();
            return View(data);
        }

        #endregion 

        #region ----------------- EsiFamily Create -------------

        [HttpGet]
        public ActionResult EsiFamilyCreate(int? id)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == id)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == id);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }

            EsiFamilyModel esiFamilyModel = new EsiFamilyModel();
            esiFamilyModel.staffid = id;
            return View(esiFamilyModel);
        }

        [HttpPost]
        public ActionResult EsiFamilyCreatepost(EsiFamilyModel esiFamilyModel)
        {
            //   var errors = ModelState.Values.SelectMany(v => v.Errors);
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                var esiFamily = new EsiFamily
                {
                    dob = esiFamilyModel.dob,
                    firmid = firm,
                    state = esiFamilyModel.state,
                    town = esiFamilyModel.town,
                    name = esiFamilyModel.name,
                    reside = esiFamilyModel.reside,
                    relationship = esiFamilyModel.relationship,
                    staffid = esiFamilyModel.staffid,
                    isdeleted = false

                };
                db.EsiFamilies.Add(esiFamily);
                db.SaveChanges();
                TempData["notice"] = "success";
            }
            else
            {
                TempData["notice"] = "error";
            }
            return RedirectToAction("EsiFamilyCreate", "Staffs", new {@id = esiFamilyModel.staffid});
        }

        #endregion 

        #region -------------- EsiFamilyEdit -------------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EsiFamilyEdit(int id)
        {

            EsiFamilyModel esiFamilyModel = new EsiFamilyModel();
            var data = db.EsiFamilies.FirstOrDefault(a => a.esifamid == id);
            if (data != null)
            {
                esiFamilyModel.dob = data.dob;
                esiFamilyModel.esifamid = data.esifamid;
                esiFamilyModel.firmid = data.firmid;
                esiFamilyModel.isdeleted = data.isdeleted;
                esiFamilyModel.name = data.name;
                esiFamilyModel.relationship = data.relationship;
                esiFamilyModel.staffid = data.staffid;
                if (data.reside != null) esiFamilyModel.reside = (bool) data.reside;
                esiFamilyModel.state = data.state;
                esiFamilyModel.town = data.town;

            }
            return View(esiFamilyModel);
        }

        public ActionResult EsiFamilyEditpost(EsiFamilyModel esiFamilyModel)
        {
            if (ModelState.IsValid)
            {
                var data =
                    db.EsiFamilies.FirstOrDefault(a => a.isdeleted == false && a.esifamid == esiFamilyModel.esifamid);
                if (data != null)
                {
                    data.dob = esiFamilyModel.dob;
                    data.firmid = esiFamilyModel.firmid;
                    data.isdeleted = esiFamilyModel.isdeleted;
                    data.name = esiFamilyModel.name;
                    data.relationship = esiFamilyModel.relationship;
                    data.reside = esiFamilyModel.reside;
                    data.state = esiFamilyModel.state;
                    data.town = esiFamilyModel.town;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                }

            }
            return RedirectToAction("EsiFamilyCreate", "Staffs", new {@id = esiFamilyModel.staffid});

        }

        #endregion

        #region --------------- EsiFamilyDelete ----------------

        public ActionResult EsiFamilyDelete(int id)
        {
            var family = db.EsiFamilies.FirstOrDefault(a => a.esifamid == id);
            return View(family);
        }

        public ActionResult EsiFamilyDeleteConfirm(EsiFamily family)
        {
            var data = db.EsiFamilies.FirstOrDefault(a => a.esifamid == family.esifamid);
            if (data != null)
            {
                data.isdeleted = true;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "delete";
            }
            if (data != null) return RedirectToAction("EsiFamilyCreate", "Staffs", new {@id = data.staffid});
            return View();
        }

        #endregion

        #region --------- Inactive Employee List ---------------

        public ActionResult InactiveResult(int? designationId)
        {
            var firm = LoginUserFirmId();

            ViewBag.designationId = new SelectList(db.Designations.Where(a => a.firmId == firm && a.isActive == true),
                "designationId", "designation1");
            if (designationId != null)
            {
                var staff1 =
                    db.Staffs.Where(a => a.designationId == designationId && a.isActive == false && a.firmId == firm)
                        .Select(a => new StaffModel()
                        {
                            StaffId = a.staffId,
                            StaffCode = a.staffCode,
                            SchoolCode = a.schoolCode,
                            FirstName = a.firstName,
                            MiddleName = a.middleName,
                            LastName = a.lastName,
                            Gender = (int) a.gender,
                            Contact = a.contact,
                            Email = a.email,
                            DesignationId = (int) a.designationId,
                            Designation = a.Designation,
                            Department = a.Department,
                            Dob = a.dob,
                            ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                            ReportingId = a.reportingId,
                            EnrollNumber = (int) a.enrollNumber,
                        });
                return View(staff1.ToList());
            }


            var staffs = db.Staffs.Where(a => a.isActive == false && a.firmId == firm).Select(a => new StaffModel()
            {
                StaffId = a.staffId,
                StaffCode = a.staffCode,
                SchoolCode = a.schoolCode,
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                Gender = (int) a.gender,
                Contact = a.contact,
                Email = a.email,
                Designation = a.Designation,
                Department = a.Department,
                Dob = a.dob,
                ReportingTo = a.Staff2.firstName + " " + a.Staff2.middleName + " " + a.Staff2.lastName,
                ReportingId = a.reportingId,
                EnrollNumber = (int) a.enrollNumber,
            });
            return View(staffs.ToList());
        }

        #endregion

        #region ------------ MachineDataForProfile -------------

        public ActionResult MachineDataForProfile()
        {
            var firmid = LoginUserFirmId();
            var staffId = LoginEmployeeId();
            DateTime start = DateTime.UtcNow.Date.AddDays(-3);
            DateTime end = DateTime.UtcNow;
            var data = db.GetAttendance(start, end, firmid,null);

            var atsheetModel = data.Where(s => s.StaffId == staffId).Select(a => new AttendanceSheetModel()
            {
                EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                EnrollNumber = a.EnrollNumber,
                EmployeeCode = a.employeeCode,
                StaffId = a.StaffId,
                TranDate = a.ThatDay,
                ChekIn = a.ChekIn,
                ChekOut = a.ChekOut,
                TotalTime = a.TotalTime,
                ThatDayStatus = a.ThatDayStatus,
                HolidayStatus = a.HolidayStatus,
                Sesssion = a.sesssion,
                IsStart = a.isStart,
                LeaveDays = a.LeaveDays,
                PresentFlag = a.PresentFlag,
                Componensation = a.Componensation,
            }).ToList();

            return View(atsheetModel);
        }

        #endregion

        #region ------ GetAllEmpInsuranceInfo Report -----------

        public ActionResult GetAllEmpInsuranceInfo()
        {
            var firmid = LoginUserFirmId();
            var data = db.GetAllEmpInsuranceInfo(firmid);
            var GetAllEmp = data.Select(a => new InsuranceinfoModel()
            {
                Name = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                Policyno = a.policyno,
                Expirydate = a.expirydate,
                Policydate = a.policydate,
                Nomineename = a.nomineename,
                Nomineeaddress = a.nomineeaddress,
                Nomineeralation = a.nomineeralation,
                Dob = a.dob,
                Age = a.age,
                Share = a.share
            });
            return View(GetAllEmp.ToList());
        }

        #endregion

        #region ------------ GetAllEmployeeESIDetail -----------

        public ActionResult GetAllEmployeeESIDetail()
        {
            var firmid = LoginUserFirmId();

            var data = db.GetAllEmployeeESIDetail(firmid);
            var GetEmpEsiDetails =
                data.Select(
                    a =>
                        new EsiinfoModel()
                        {
                            staffcode = a.staffCode,
                            Name = a.firstName + " " + a.middleName + " " + a.lastName,
                            fathername = a.middleName + " " + a.lastName,
                            peraddress = a.peraddress,
                            pin = a.pin,
                            branchoffice = a.branchoffice,
                            dateofappoint = a.dateofappoint,
                            preinsuranceno = a.preinsuranceno,
                            preempolyecode = a.preempolyecode,
                            relationship = a.relationship,
                            nomaddress = a.nomaddress,
                            corponame = a.corponame,
                            corpinsno = a.corpinsno,
                            corpfathername = a.corpfathername,
                            corpbranchoffice = a.corpbranchoffice,
                            dateofentry = a.dateofentry,
                            corpdob = a.corpdob,
                            dispensary = a.dispensary,
                            Employeraddressname = a.Employeraddressname,
                            CurEmployeraddressname = a.CurEmployeraddressname,
                            corpnameaddresscode = a.corpnameaddresscode,
                            corpDispensary = a.corpDispensary,
                            nomineename = a.nomineename,
                            Nomineeaddress = a.nomineeaddress,
                            Nomineeralation = a.nomineeralation,
                            Nomineestatus = a.nomineestatus,
                            Dob = a.dob,
                            Age = a.age,
                            Share = a.share
                        });
            return View(GetEmpEsiDetails.ToList());
        }

        #endregion

        #region ------------- GetEpfNomineeReport --------------

        public ActionResult GetEpfNomineeReport()
        {
            var firmid = LoginUserFirmId();

            var data = db.GetAllEpfNomineeDetail(firmid);
            var GetEmpEsiDetails =
                data.Select(
                    a =>
                        new EPFModel()
                        {
                            staffcode = a.staffCode,
                            Name = a.firstName + " " + a.middleName + " " + a.lastName,
                            fathername = a.middleName + " " + a.lastName,
                            tempAddress = a.tempAddress,
                            temppin = a.tempPincode,
                            perarea = a.perArea,
                            marriedstatus = a.isMarried,
                            perpincode = a.perPincode,
                            tempArea = a.tempArea,
                            perAddress = a.perAddress + " " + a.perArea + " " + a.perPincode,
                            JOD = a.joingDate,
                            EPFAccountNo = a.EpfAccountno,
                            Nomineename = a.nomineename,
                            Nomineeaddress = a.nomineeaddress,
                            Nomineeralation = a.nomineeralation,
                            dob = a.dob,
                            Share = a.share,
                            Age = a.age
                        });

            return View(GetEmpEsiDetails.ToList());
        }


        #endregion

        #region ------------ GetVisaPassportDetails ------------

        public ActionResult GetVisapassportReport()
        {
            var firmid = LoginUserFirmId();
            var data = db.GetVisaPassportDetails(firmid);
            var visadetailModel =
                data.Select(
                    a =>
                        new VisaPassportModel()
                        {
                            StaffCode = a.staffCode,
                            Staffname = a.firstName + " " + a.middleName + " " + a.lastName,
                            Num = a.Num,
                            Country = a.Country,
                            ExpiryDate = a.ExpiryDate,
                            Type = a.Type
                        });
            return View(visadetailModel.ToList());
        }

        #endregion

        #region -------------- Bank Info Report ----------------

        public ActionResult BankInfoReport()
        {
            var firm = LoginUserFirmId();
            var data = db.GetAllEmpBankInfo(firm);
            var Bankinfo =
                data.Select(
                    a =>
                        new BankInfoModel()
                        {
                            StaffName = a.firstName + " " + a.middleName + " " + a.lastName,
                            StaffCode = a.staffCode,
                            HolderName = a.HolderName,
                            AccountNo = a.AccountNo,
                            BankIfsc = a.BankIfsc,
                            BankName = a.BankName,
                            Branch = a.Branch
                        });
            return View(Bankinfo.ToList());
        }

        #endregion

        #region ------------ Net Duration UserName -------------

        [HttpGet]
        public ActionResult NetUserNameAssignForm()
        {
            var firm = LoginUserFirmId();
            NetUserNameModel fam = new NetUserNameModel();
            fam.StaffListForUser =
                db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });

            return View(fam);
        }

        [HttpPost]
        public ActionResult NetUserNameAssignForm(NetUserNameModel assignShiftModel)
        {
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {

                    var data = db.TblNetDurationUsersNames.FirstOrDefault(a => a.Staffid == assignShiftModel.empid);
                    if (data == null)
                    {
                        var assignShift = new TblNetDurationUsersName()
                        {
                            Staffid = assignShiftModel.empid,
                            Firmid = firm,
                            UserNames = assignShiftModel.EmpUsername.Trim()
                        };
                        db.TblNetDurationUsersNames.Add(assignShift);
                        db.SaveChanges();
                        TempData["notice"] = "success";
                    }
                    else
                    {
                        TempData["notice"] = "exist";
                    }

                }
                return RedirectToAction("NetUserNameAssignForm");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult NetUserNameIndex()
        {
            var firm = LoginUserFirmId();
            var data = db.TblNetDurationUsersNames.Where(d => d.Firmid == firm).Include(s => s.Staff);
            return View(data.ToList().Where(d => d.Staff.isActive == true));
        }

        public ActionResult DeleteUsername(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblNetDurationUsersName bts = db.TblNetDurationUsersNames.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeleteUsername(int id)
        {
            try
            {
                TblNetDurationUsersName bts = db.TblNetDurationUsersNames.Find(id);
                db.TblNetDurationUsersNames.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("NetUserNameAssignForm");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("NetUserNameAssignForm");
            }
        }


        [HttpGet]
        public ActionResult UserNameEdit(int? id)
        {
            var firm = LoginUserFirmId();
            NetUserNameModel model = new NetUserNameModel();
            var data = db.TblNetDurationUsersNames.FirstOrDefault(a => a.NetUsersNameId == id);
            if (data != null)
            {
                model.NetId = data.NetUsersNameId;
                model.EmpUsername = data.UserNames;
                model.firmId = (int) data.Firmid;
                model.empid = (int) data.Staffid;
                model.StaffListForUser =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UserNameEdit(NetUserNameModel mmodel)
        {
            if (ModelState.IsValid)
            {
                var data = db.TblNetDurationUsersNames.FirstOrDefault(a => a.Staffid == mmodel.empid && a.UserNames==mmodel.EmpUsername);
                if (data == null)
                {
                    TblNetDurationUsersName bts = db.TblNetDurationUsersNames.Find(mmodel.NetId);
                    {
                        bts.NetUsersNameId = (int)mmodel.NetId;
                        bts.Staffid = mmodel.empid;
                        bts.Firmid = mmodel.firmId;
                        bts.UserNames = mmodel.EmpUsername;
                    }
                    db.TblNetDurationUsersNames.AddOrUpdate(bts);
                    db.SaveChanges();
                    TempData["notice"] = "update";
                }
                else
                {
                    TempData["notice"] = "exist";
                }
                return RedirectToAction("NetUserNameAssignForm");
            }
            return View(mmodel);
        }

        #endregion

        #region -------- Staff Login Records All Methods -------
        public ActionResult StaffLoginRecords(DateTime? datepicker, DateTime? datepicker2)
        {
            var firm = LoginUserFirmId();
            if (datepicker == null && datepicker2 == null)
            {
                var date = DateTime.Now.Date;
                var data = db.GetStaffLoginRecordNew(firm, null, null);
                var datainModel = data.Select(a => new LoginRecordModel()
                {
                    loginid = a.LoginId,
                    StaffcodeLogin = a.staffCode,
                    FirstLogin = a.firstName,
                    MiddleLogin = a.middleName,
                    LastLogin = a.lastName,
                    CreatedDateLogin = (DateTime) a.createdDate,
                }).ToList();
                return View(datainModel);
            }
            else
            {
                DateTime start = (DateTime) datepicker;
                DateTime end = (DateTime) datepicker2;

                var stdate = start.Date;
                var stend = end.Date;

                var data = db.GetStaffLoginRecordNew(firm, stdate, stend);
                var datainModel = data.Select(a => new LoginRecordModel()
                {
                    loginid = a.LoginId,
                    StaffcodeLogin = a.staffCode,
                    FirstLogin = a.firstName,
                    MiddleLogin = a.middleName,
                    LastLogin = a.lastName,
                    CreatedDateLogin = (DateTime) a.createdDate,

                }).ToList();
                return View(datainModel);
            }
        }

        public ActionResult StaffLoginDetails(int? Logid)
        {
            var data = db.GetStaffLoginDetails(Logid);
            var datainModel = data.Select(a => new LoginRecordModel()
            {
                loginid = a.LoginId,
                StaffcodeLogin = a.staffCode,
                FirstLogin = a.firstName,
                MiddleLogin = a.middleName,
                LastLogin = a.lastName,
                CreatedDateLogin = (DateTime) a.createdDate,
                City = a.City,
                State = a.State,
                CountryName = a.CountryName,
                ZipCode = a.ZipCode,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                TimeZone = a.TimeZone,
                Ipaddress = a.IpAddress,
            }).ToList();
            return View(datainModel);
        }


        public ActionResult StaffLoginDetailsMember(DateTime? datepicker, DateTime? datepicker2)
        {
            var stid = LoginEmployeeId();

            if (datepicker == null && datepicker2 == null)
            {
                var data = db.GetStaffLoginDetailsMember(stid, null, null);
                var datainModel = data.Select(a => new LoginRecordModel()
                {
                    loginid = a.LoginId,
                    StaffcodeLogin = a.staffCode,
                    FirstLogin = a.firstName,
                    MiddleLogin = a.middleName,
                    LastLogin = a.lastName,
                    CreatedDateLogin = (DateTime) a.createdDate,
                    City = a.City,
                    State = a.State,
                    CountryName = a.CountryName,
                    ZipCode = a.ZipCode,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    TimeZone = a.TimeZone,
                    Ipaddress = a.IpAddress,
                }).ToList();
                return View(datainModel);

            }
            else
            {
                DateTime start = (DateTime) datepicker;
                DateTime end = (DateTime) datepicker2;

                var stdate = start.Date;
                var stend = end.Date;

                var data = db.GetStaffLoginDetailsMember(stid, stdate, stend);
                var datainModel = data.Select(a => new LoginRecordModel()
                {
                    loginid = a.LoginId,
                    StaffcodeLogin = a.staffCode,
                    FirstLogin = a.firstName,
                    MiddleLogin = a.middleName,
                    LastLogin = a.lastName,
                    CreatedDateLogin = (DateTime) a.createdDate,
                    City = a.City,
                    State = a.State,
                    CountryName = a.CountryName,
                    ZipCode = a.ZipCode,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    TimeZone = a.TimeZone,
                    Ipaddress = a.IpAddress,
                }).ToList();
                return View(datainModel);
            }

        }
        #endregion
    }
}
