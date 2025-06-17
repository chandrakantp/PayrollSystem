using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using edueTree.Models;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;

//using System.Linq.Dynamic;

namespace edueTree.Controllers.PayRoll
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        #region ----------- Db-context -------------
        private dbContainer _db = new dbContainer();
        #endregion

        #region ----------- AttendanceSheet --------

        public ActionResult AttendanceSheet(DateTime? datepicker, DateTime? datepicker2)
        {
            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("AttendanceSheet");
            }
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var staffId = LoginEmployeeId();
                var firmid = LoginUserFirmId();
                var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == staffId);
                var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus

                }).ToList();

                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var firmid = LoginUserFirmId();
                var staffid = LoginEmployeeId();
                var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == staffid);

                var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance
                    ,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();

                return View(atsheetModel);
            }

        }

        #endregion

        #region ---------- MachineData -------------

        public ActionResult MachineData(DateTime? datepicker, DateTime? datepicker2, int? empId, string AttendmodeList)
        {
            var firmId = LoginUserFirmId();
            ViewBag.staffid =
              _db.Staffs.ToList().Where(s => s.firmId == firmId && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });
            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("MachineData");
            }
            if (datepicker == null && datepicker2 == null && AttendmodeList == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                AttendmodeList = "All";
                var staffId = LoginEmployeeId();
                var data = _db.MachineDatanew(start, end, staffId, firmId, AttendmodeList);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + ": " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn,
                    inOutMode = a.inOutMode,
                    latitude = a.latitude,
                    longitude = a.longitude
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                if (datepicker == null)
                {
                    DateTime date = DateTime.UtcNow;
                    start = new DateTime(date.Year, date.Month, 1);
                }
                else
                {
                    start = (DateTime)datepicker;
                }
                if (datepicker2 == null)
                {
                    end = start.AddMonths(1).AddDays(-1);
                }
                else
                {
                    end = (DateTime)datepicker2;
                }

                if (empId == null) { empId = 0; }
                var data = _db.MachineDatanew(start, end, empId, firmId, AttendmodeList);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn,
                    inOutMode = a.inOutMode,
                    latitude = a.latitude,
                    longitude = a.longitude
                }).ToList();
                return View(atsheetModel);
            }
        }
        #endregion

        #region ---------- MachineDataformgr -------

        public ActionResult MachineDataForMgr(DateTime? datepicker, DateTime? datepicker2, int? empId, string AttendmodeList)
        {
            var firmId = LoginUserFirmId();
            var loginEmployee = LoginEmployeeId();
            var firm = LoginUserFirmId();
            ViewBag.staffid =
              _db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firm).Where(a => a.ReportingManagerId == loginEmployee).Select(a => new SelectListItem()
              {
                  Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                  Value = a.Staff.staffId.ToString()
              });

            if (empId == null || empId == 0)
            {
                var loginid = LoginEmployeeId();
                empId = loginid;
            }

            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("MachineData");
            }
            if (datepicker == null && datepicker2 == null && AttendmodeList == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                AttendmodeList = "All";
                var staffId = LoginEmployeeId();
                var data = _db.MachineDatanew(start, end, staffId, firmId, AttendmodeList);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + ": " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                if (datepicker == null)
                {
                    DateTime date = DateTime.UtcNow;
                    start = new DateTime(date.Year, date.Month, 1);
                }
                else
                {
                    start = (DateTime)datepicker;
                }
                if (datepicker2 == null)
                {
                    end = start.AddMonths(1).AddDays(-1);
                }
                else
                {
                    end = (DateTime)datepicker2;
                }
                var data = _db.MachineDatanew(start, end, empId, firmId, AttendmodeList);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn
                }).ToList();
                return View(atsheetModel);
            }
        }
        #endregion

        #region ------------ GetStaff --------------
        [HttpPost]
        public ActionResult GetStaff()
        {
            List<int> subcatId = new List<int>();
            List<string> subcat = new List<string>();
            var loginEmployee = LoginEmployeeId();

            var mgremai = _db.Staffs.FirstOrDefault(s => s.staffId == loginEmployee).email;
            Boolean flag = false;
            var firm = LoginUserFirmId();

            subcat.Add("All Employee");
            subcatId.Add(0);
            foreach (var VARIABLE in _db.Staffs.Where(a => a.firmId == firm && a.isActive == true && a.emailReportingTo == mgremai))
            {
                subcat.Add(VARIABLE.schoolCode + ": " + VARIABLE.firstName + " " + VARIABLE.middleName + " " +
                           VARIABLE.lastName);
                subcatId.Add(VARIABLE.staffId);
            }

            return Json(new { Subcat = subcat, SubcatId = subcatId });
        }
        #endregion

        #region ----------- GetStaffnew ------------
        [HttpPost]
        public ActionResult GetStaffNew()
        {
            List<int> subcatId = new List<int>();
            List<string> subcat = new List<string>();
            var loginEmployee = LoginEmployeeId();

            var id = GetUserId();
            var rroles = _db.UserRoles.FirstOrDefault(q => q.userId == id).RoleId;
            var asprole = _db.AspNetRoles.Where(a => a.Id == rroles);
            Boolean flag = false;

            subcat.Add("All Employee");
            var firm = LoginUserFirmId();

            subcatId.Add(0);
            foreach (var VARIABLE in _db.Staffs.Where(a => a.firmId == firm && a.isActive == true))
            {
                subcat.Add(VARIABLE.schoolCode + " : " + VARIABLE.firstName + " " + VARIABLE.middleName + " " +
                           VARIABLE.lastName);
                subcatId.Add(VARIABLE.staffId);
            }
            return Json(new { Subcat = subcat, SubcatId = subcatId });
        }
        #endregion

        #region ----- ManagerAttendanceSheet -------
        public ActionResult ManagerAttendanceSheet(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var loginEmployee = LoginEmployeeId();
            var firm = LoginUserFirmId();
            ViewBag.staffid =
              _db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firm).Where(a => a.ReportingManagerId == loginEmployee).Select(a => new SelectListItem()
              {
                  Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                  Value = a.Staff.staffId.ToString()
              });
            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("ManagerAttendanceSheet");
            }
            if (datepicker != null && datepicker2 != null && empId != null)
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var firmid = LoginUserFirmId();
                var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == empId);

                var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }
            else
            {

                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var firmid = LoginUserFirmId();
                var loginID = LoginEmployeeId();
                var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == loginID);

                var atsheetModel = getList.Where(s => s.ThatDay >= start && s.ThatDay <= end).Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }
        }
        #endregion

        #region ---- AllStaffAttendanceSheet -------
        public ActionResult AllStaffAttendanceSheet(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var firmid = LoginUserFirmId();
            ViewBag.staffid =
          _db.Staffs.ToList().Where(s => s.firmId == firmid && s.isActive == true).Select(a => new SelectListItem()
          {
              Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
              Value = a.staffId.ToString()
          });

            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("AllStaffAttendanceSheet");
            }
            if (datepicker == null && datepicker2 == null)
            {
               
                var data1 = _db.GetAttendance(null, null, null, null);
                var model1 = data1.Select(a => new AttendanceSheetModel()
                {

                }).ToList();
                TempData["notice"] = "ModelNull";
                return View(model1);
                // return View();
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var getList = empId != null ? _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == empId) : _db.GetAttendance(start, end, firmid, null);
                var atsheetModel = getList.Where(s => s.ThatDay >= start && s.ThatDay <= end).Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }
        }
        #endregion

        #region -------- Sr-Attendance-Sheet ---------
        public ActionResult SrAttendanceSheet(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var firmid = LoginUserFirmId();
            ViewBag.staffid =
          _db.Staffs.ToList().Where(s => s.firmId == firmid && s.isActive == true).Select(a => new SelectListItem()
          {
              Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
              Value = a.staffId.ToString()
          });

            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("SrAttendanceSheet");
            }
            if (datepicker != null && datepicker2 != null && empId != null)
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;

                var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == empId);

                //  var atsheetModel = getList.Where(s => s.ThatDay >= start && s.ThatDay < end).Select(a => new AttendanceSheetModel()
                var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                if (datepicker != null && datepicker2 != null && empId == null)
                {
                    var loginID = LoginEmployeeId();
                    var getList = _db.GetAttendance(datepicker, datepicker2, firmid, null);
                    var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                        ComponensationBalance = a.ComponensationBalance,
                        ShiftName = a.shiftname,
                        sandwitch = a.sandwitchstatus
                    }).ToList();
                    return View(atsheetModel);
                }
                else
                {
                    DateTime date = DateTime.UtcNow;
                    var start = new DateTime(date.Year, date.Month, 1);
                    var end = start.AddMonths(1).AddDays(-1);
                    var loginID = LoginEmployeeId();
                    var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == loginID);

                    var atsheetModel = getList.Where(s => s.ThatDay >= start && s.ThatDay <= end).Select(a => new AttendanceSheetModel()
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
                        ComponensationBalance = a.ComponensationBalance,
                        ShiftName = a.shiftname,
                        sandwitch = a.sandwitchstatus
                    }).ToList();
                    return View(atsheetModel);
                }
            }
        }
        #endregion

        #region -------- GenerateAttendance --------
        //below method will replace the antixsstags with normal character.       

        public ActionResult GenerateAttendance(int? month, int? year)
        {
            var modelList = new List<MonthllyAttendanceModel>();
            if (month == null && year == null)
            {
                return View(modelList.ToList());
            }
            try
            {
                if (month == null && year != null || month != null && year == null)
                {
                    TempData["AlreadyGenerate"] = "select";
                    return View(modelList.ToList());
                }
                DateTime date = DateTime.UtcNow;
                var start = new DateTime((int)year, (int)month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var firmid = LoginUserFirmId();

                var getList = _db.GetAttendance(start, end, firmid, null).GroupBy(a => a.StaffId);
                foreach (var lists in getList)
                {
                    var getAttendanceResult = lists.FirstOrDefault();
                    if (getAttendanceResult == null) continue;
                    var model = new MonthllyAttendanceModel
                    {
                        StaffId = getAttendanceResult.StaffId,
                        MonthDays = lists.Count(),
                        AttendDays = lists.Sum(a => a.PresentFlag),
                        PPR = lists.Count(s => s.ThatDayStatus != null),
                        //SC = lists.Count(s => s.shiftname != null),
                        SC = lists.Count(s => !string.IsNullOrEmpty(s.shiftname)),
                        EmployeeName = getAttendanceResult.EmployeeName,
                        CurrentMonthEarnComp = lists.Sum(a => a.Componensation),
                        Holidays = lists.Count(a => a.HolidayStatus == "Holiday")
                    };

                    //EventLog Net Duration Calculation
                    var netdur = _db.NetDurationAllEmp(start, end, firmid, getAttendanceResult.StaffId);
                    TimeSpan t9 = new TimeSpan();
                    t9 = TimeSpan.Parse("00:00:00");

                    if (netdur != null)
                    {
                        foreach (var temp in netdur)
                        {
                            if (temp.TotalDuration != null)
                            {
                                t9 = (TimeSpan)(t9 + TimeSpan.Parse(temp.TotalDuration));
                            }
                        }
                    }

                    var abc3 = t9.Days * 24 + Convert.ToDecimal(t9.ToString(@"hh\.mm"));
                    model.Eventnettime = Convert.ToString(abc3);
                    // Attendance Machine net Duration calculation
                    var data = _db.Brekingshift(start, end, firmid, getAttendanceResult.StaffId);
                    TimeSpan t3 = new TimeSpan();
                    foreach (var temp in data)
                    {
                        t3 = (TimeSpan)(t3 + temp.TotalnetDuration);
                    }
                    var abc1 = t3.Days * 24 + Convert.ToDecimal(t3.ToString(@"hh\.mm"));
                    model.Machinetime = Convert.ToString(abc1);

                    //Calculate Current Compensation
                    var comCurrentMonthBalance = lists.Sum(a => a.Componensation);
                    //Calculate Previous month Compensation
                    var comPreviousMonth = getAttendanceResult.ComponensationBalance;
                    var totalAbsent =
                        lists.Count(
                            item => item.HolidayStatus == null && item.ThatDayStatus == null && item.sesssion == null);
                    //model.EarnCompansation = (comCurrentMonthBalance + comPreviousMonth) - totalAbsent >= 0
                    //    ? (comCurrentMonthBalance + comPreviousMonth) - totalAbsent
                    //    : 0;

                    //sandwich leave calculation
                    var sandleave = lists.Count(x => x.sandwitchstatus == "sandwich");

                    model.EarnCompansation = comCurrentMonthBalance;
                    model.Absent = totalAbsent + sandleave; //- (comPreviousMonth + comCurrentMonthBalance) > 0 ? totalAbsent - (comPreviousMonth + comCurrentMonthBalance) : 0;
                    model.UsedCompansation = totalAbsent >= (comCurrentMonthBalance + comPreviousMonth) ? (comCurrentMonthBalance + comPreviousMonth) : totalAbsent;
                    model.PreviousMonthEarnComp = comPreviousMonth;
                    model.ForwordComp = (comPreviousMonth + comCurrentMonthBalance) - totalAbsent > 0
                        ? (comPreviousMonth + comCurrentMonthBalance) - totalAbsent
                        : 0;
                    //weekly of calculation
                    model.WeeklyOff = lists.Count(a => a.HolidayStatus == "Weekend");
                    model.WorkingDays = lists.Count() -
                                        (lists.Count(a => a.HolidayStatus == "Holiday") +
                                         lists.Count(a => a.HolidayStatus == "Weekend"));
                    //paid leave count
                    model.PaidLeaves = lists.Sum(a => a.LeaveDays);
                    model.MonthlyPaidLeaves = lists.Sum(a => a.LeaveDays);
                    model.Month = start.Month;
                    model.MonthString = start.ToString("MMMMMMMMMMMMM");
                    model.Year = start.Year;
                    var t6 = new TimeSpan();
                    var t2 = new TimeSpan();
                    var r1 = "00:00:00";
                    foreach (var temp in lists)
                    {
                        t2 = (TimeSpan)(t2 + temp.TotalTime);

                    }
                    var abc = t2.Days * 24 + Convert.ToDecimal(t2.ToString(@"hh\.mm"));
                    //if Employee declare Avg time in payrollconfign it will take this individual time but if you are not mentioned time in avgtime then it will 
                    var avgtimeemp = _db.TblAverageTimes.FirstOrDefault(a => a.staffid == getAttendanceResult.StaffId);
                    if (avgtimeemp != null)
                    {
                        t6 = (TimeSpan)(t6 + TimeSpan.Parse(avgtimeemp.hourtime));
                    }
                    else
                    {
                        var workinghrs = _db.PayrollConfigs.FirstOrDefault(a => a.firmId == firmid);
                        if (workinghrs != null)
                            if (workinghrs.WorkingHoursDay != null)
                                t6 = (TimeSpan)workinghrs.WorkingHoursDay;
                    }
                    var totaltime = Convert.ToDecimal(model.PPR) * Convert.ToDecimal(t6.ToString(@"hh\.mm"));
                    model.Overundertime = Convert.ToString(Convert.ToDecimal(abc) - totaltime, CultureInfo.InvariantCulture);
                    model.Totaltimestr = Convert.ToString(abc, CultureInfo.InvariantCulture);

                    model.WorkdayPresent = lists.Count(s => s.ThatDayStatus != null && s.HolidayStatus == null);
                    modelList.Add(model);

                }
                return View(modelList.ToList());
            }
            catch (Exception ex)
            {
                TempData["notice"] = "error";
                return View(modelList.ToList());
            }
        }
        #endregion

        #region ----- GenerateAttendanceself -------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult GenerateAttendanceself(int? staffid, int? dt1, int? dt2)
        {
            if (staffid == null)
            {
                staffid = LoginEmployeeId();
            }

            DateTime date = DateTime.UtcNow;
            var start = new DateTime((int)dt2, (int)dt1, 1);
            var end = start.AddMonths(1).AddDays(-1);


            var firmid = LoginUserFirmId();
            var modelList = new List<MonthllyAttendanceModel>();
            var getList = _db.GetAttendance(start, end, firmid, null).Where(a => a.StaffId == staffid).GroupBy(a => a.StaffId);
            foreach (var lists in getList)
            {
                var model = new MonthllyAttendanceModel();
                model.StaffId = lists.FirstOrDefault().StaffId;
                model.MonthDays = lists.Count();
                model.AttendDays = lists.Sum(a => a.PresentFlag);
                model.EmployeeName = lists.FirstOrDefault().EmployeeName;
                model.CurrentMonthEarnComp = lists.Sum(a => a.Componensation);
                model.Holidays = lists.Count(a => a.HolidayStatus == "Holiday");
                //Calculate Current Componensation
                var comCurrentMonthBalance = lists.Sum(a => a.Componensation);
                //Calculate Previous month Componensation
                var comPreviousMonth = lists.FirstOrDefault().ComponensationBalance;
                var totalAbsent =
                    lists.Count(
                        item => item.HolidayStatus == null && item.ThatDayStatus == null && item.sesssion == null);
                //model.EarnCompansation = (comCurrentMonthBalance + comPreviousMonth) - totalAbsent >= 0
                //    ? (comCurrentMonthBalance + comPreviousMonth) - totalAbsent
                //    : 0;
                model.EarnCompansation = comCurrentMonthBalance;


                model.Absent = totalAbsent - (comPreviousMonth + comCurrentMonthBalance) > 0 ? totalAbsent - (comPreviousMonth + comCurrentMonthBalance) : 0;
                model.UsedCompansation = totalAbsent >= (comCurrentMonthBalance + comPreviousMonth) ? (comCurrentMonthBalance + comPreviousMonth) : totalAbsent;
                model.PreviousMonthEarnComp = comPreviousMonth;
                model.ForwordComp = (comPreviousMonth + comCurrentMonthBalance) - totalAbsent > 0
                    ? (comPreviousMonth + comCurrentMonthBalance) - totalAbsent
                    : 0;
                model.WeeklyOff = lists.Count(a => a.HolidayStatus == "Weekend");
                model.WorkingDays = lists.Count() -
                                    (lists.Count(a => a.HolidayStatus == "Holiday") +
                                     lists.Count(a => a.HolidayStatus == "Weekend"));
                model.PaidLeaves = lists.Sum(a => a.LeaveDays);
                model.Month = start.Month;
                model.MonthString = start.ToString("MMMMMMMMMMMMM");
                model.Year = start.Year;
                modelList.Add(model);
            }
            return View(modelList.ToList());
        }

        #endregion

        #region --- GenerateAttendanceDataStore ----
        public ActionResult GenerateAttendanceDataStore(string staffIds, string monthDays, string presents, string physicalpresents, string absents, string preMonthCompansation, string currentMonthCompansation, string paidLeaves, string weeklyOff, string holidays, string month, string year, string shiftcount, string workdaypresent, string earnCompansation, string monthlyWorkingDays, string monthlyPaidLeave, string Totaltimestr, string Machinetime, string Overundertime)
        {
            var perListOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(staffIds);
            var perListEnum = perListOj as object[] ?? perListOj.Cast<object>().ToArray();

            var monthDaysOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(monthDays);
            var monthDaysEnum = monthDaysOj as object[] ?? monthDaysOj.Cast<object>().ToArray();

            var presentsOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(presents);
            var presentsEnu = presentsOj as object[] ?? presentsOj.Cast<object>().ToArray();

            var absentsOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(absents);
            var absentsEnumerable = absentsOjbect as object[] ?? absentsOjbect.Cast<object>().ToArray();


            var physicalpresentsOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(physicalpresents);
            var physicalpresentsEnumerable = physicalpresentsOjbect as object[] ?? physicalpresentsOjbect.Cast<object>().ToArray();

            var paidLeavesOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(paidLeaves);
            var paidLeavesEnumerable = paidLeavesOjbect as object[] ?? paidLeavesOjbect.Cast<object>().ToArray();

            var weeklyOffOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(weeklyOff);
            var weeklyOffEnumerable = weeklyOffOjbect as object[] ?? weeklyOffOjbect.Cast<object>().ToArray();

            var holidaysOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(holidays);
            var holidaysEnumerable = holidaysOjbect as object[] ?? holidaysOjbect.Cast<object>().ToArray();

            //var usedCompansationOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(usedCompansation);
            //var usedCompansationEnumerable = usedCompansationOjbect as object[] ?? usedCompansationOjbect.Cast<object>().ToArray();

            var currentMonthCompansationOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(currentMonthCompansation);
            var currentMonthCompansationEnumerable = currentMonthCompansationOjbect as object[] ?? currentMonthCompansationOjbect.Cast<object>().ToArray();

            var preMonthCompansationOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(preMonthCompansation);
            var preMonthCompansationEnumerable = preMonthCompansationOjbect as object[] ?? preMonthCompansationOjbect.Cast<object>().ToArray();

            var earnCompansationOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(earnCompansation);
            var earnCompansationEnumerable = earnCompansationOjbect as object[] ?? earnCompansationOjbect.Cast<object>().ToArray();

            var monthOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(month);
            var monthEnumerable = monthOjbect as object[] ?? monthOjbect.Cast<object>().ToArray();

            var yearOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(year);
            var yearEnumerable = yearOjbect as object[] ?? yearOjbect.Cast<object>().ToArray();

            var shiftcountOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(shiftcount);
            var shiftcountEnumerable = shiftcountOjbect as object[] ?? shiftcountOjbect.Cast<object>().ToArray();

            var workdaypresentOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(workdaypresent);
            var workdaypresentEnumerable = workdaypresentOjbect as object[] ?? workdaypresentOjbect.Cast<object>().ToArray();

            var MonthlyWorkingDaysOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(monthlyWorkingDays);
            var MonthlyWorkingDaysEnumerable = MonthlyWorkingDaysOjbect as object[] ?? MonthlyWorkingDaysOjbect.Cast<object>().ToArray();

            var TotaltimestrOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(Totaltimestr);
            var TotaltimestrEnumerable = TotaltimestrOjbect as object[] ?? TotaltimestrOjbect.Cast<object>().ToArray();

            var MachinetimeOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(Machinetime);
            var MachinetimeEnumerable = MachinetimeOjbect as object[] ?? MachinetimeOjbect.Cast<object>().ToArray();

            var monthlyPaidLeaveOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(monthlyPaidLeave);
            var monthlyPaidLeaveEnumerable = monthlyPaidLeaveOjbect as object[] ?? monthlyPaidLeaveOjbect.Cast<object>().ToArray();

            var OverundertimeOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(Overundertime);
            var OverundertimeEnumerable = OverundertimeOjbect as object[] ?? OverundertimeOjbect.Cast<object>().ToArray();

            int firmId = LoginUserFirmId();

            int alredy = 0;
            int inserted = 0;
            for (int i = 0; i < perListEnum.Count(); i++)
            {
                var firmid = LoginUserFirmId();
                int tranmonth = Convert.ToInt32(monthEnumerable[i]);
                int tranyear = Convert.ToInt32(yearEnumerable[i]);
                int staffid = Convert.ToInt32(perListEnum[i]);
                var staffDetail = _db.Staffs.FirstOrDefault(a => a.staffId == staffid);
                var checkAlredyExists =
                    _db.MonthlyAttenDetails.FirstOrDefault(
                        a => a.tranMonth == tranmonth && a.tranYearyear == tranyear && a.staffId == staffid);
                if (checkAlredyExists == null)
                {

                    var tblobj = new MonthlyAttenDetail
                    {
                        staffId = staffid,
                        totalMonthDays = Convert.ToInt32(monthDaysEnum[i]),
                        weeklyOff = Convert.ToInt32(weeklyOffEnumerable[i]),
                        PresentDays = Convert.ToDecimal(presentsEnu[i]),

                        absent = Convert.ToDecimal(absentsEnumerable[i]),
                        paidLeaves = Convert.ToDecimal(paidLeavesEnumerable[i]),
                        //usedCompansation = Convert.ToDecimal(usedCompansationEnumerable[i]),
                        currentMonthCompansation = Convert.ToDecimal(currentMonthCompansationEnumerable[i]),
                        preMonthCompansation = Convert.ToDecimal(preMonthCompansationEnumerable[i]),
                        earnCompansation = Convert.ToDecimal(earnCompansationEnumerable[i]),
                        monthlyPaidLeaves = Convert.ToDecimal(monthlyPaidLeaveEnumerable[1]),
                        // monthlyPaidLeaves = staffDetail.isActive == true ? (Convert.ToDecimal(0.00)),
                        tranDate = DateTime.UtcNow,
                        tranMonth = tranmonth,
                        tranYearyear = tranyear,
                        transactionBy = GetUserId(),
                        Holidays = Convert.ToInt32(holidaysEnumerable[i]),
                        shiftcount = Convert.ToInt32(shiftcountEnumerable[i]),
                        physicalpresents = Convert.ToDecimal(physicalpresentsEnumerable[i]),
                        workdaypresent = Convert.ToDecimal(workdaypresentEnumerable[i]),
                        MonthWorkingDay = Convert.ToDecimal(MonthlyWorkingDaysEnumerable[i]),
                        Totaltime = Convert.ToDecimal(TotaltimestrEnumerable[i]),
                        Machinetime = Convert.ToDecimal(MachinetimeEnumerable[i]),
                        OvertimeUndertime = Convert.ToDecimal(OverundertimeEnumerable[i]),
                        firmId = firmid
                    };
                    _db.MonthlyAttenDetails.Add(tblobj);
                    _db.SaveChanges();
                    inserted++;
                }
                else
                {
                    checkAlredyExists.staffId = staffid;
                    checkAlredyExists.totalMonthDays = Convert.ToInt32(monthDaysEnum[i]);
                    checkAlredyExists.weeklyOff = Convert.ToInt32(weeklyOffEnumerable[i]);
                    checkAlredyExists.PresentDays = Convert.ToDecimal(presentsEnu[i]);
                    checkAlredyExists.absent = Convert.ToDecimal(absentsEnumerable[i]);
                    checkAlredyExists.paidLeaves = Convert.ToDecimal(paidLeavesEnumerable[i]);
                    //usedCompansation = Convert.ToDecimal(usedCompansationEnumerable[i]),
                    checkAlredyExists.currentMonthCompansation = Convert.ToDecimal(currentMonthCompansationEnumerable[i]);
                    checkAlredyExists.preMonthCompansation = Convert.ToDecimal(preMonthCompansationEnumerable[i]);
                    checkAlredyExists.earnCompansation = Convert.ToDecimal(earnCompansationEnumerable[i]);
                    checkAlredyExists.monthlyPaidLeaves = Convert.ToDecimal(monthlyPaidLeaveEnumerable[1]);
                    // monthlyPaidLeaves = staffDetail.isActive == true ? (Convert.ToDecimal(0.00)),
                    checkAlredyExists.tranDate = DateTime.UtcNow;
                    checkAlredyExists.tranMonth = tranmonth;
                    checkAlredyExists.tranYearyear = tranyear;
                    checkAlredyExists.transactionBy = GetUserId();
                    checkAlredyExists.Holidays = Convert.ToInt32(holidaysEnumerable[i]);
                    checkAlredyExists.shiftcount = Convert.ToInt32(shiftcountEnumerable[i]);
                    checkAlredyExists.physicalpresents = Convert.ToDecimal(physicalpresentsEnumerable[i]);
                    checkAlredyExists.workdaypresent = Convert.ToDecimal(workdaypresentEnumerable[i]);
                    checkAlredyExists.MonthWorkingDay = Convert.ToDecimal(MonthlyWorkingDaysEnumerable[i]);
                    checkAlredyExists.Totaltime = Convert.ToDecimal(TotaltimestrEnumerable[i]);
                    checkAlredyExists.Machinetime = Convert.ToDecimal(MachinetimeEnumerable[i]);
                    checkAlredyExists.OvertimeUndertime = Convert.ToDecimal(OverundertimeEnumerable[i]);
                    checkAlredyExists.firmId = firmid;
                    _db.Entry(checkAlredyExists).State = EntityState.Modified;
                    _db.SaveChanges();
                    alredy++;
                }
            }

            var result = new { Success = "true", Message = "Successfully inserted rows:" + inserted.ToString() + ", Already exist rows:" + alredy.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
            //TempData["message"] ="Successfully inserted rows:" + inserted.ToString() + ", Already exist rows:" + alredy.ToString();
            //var redirectUrl = new UrlHelper(Request.RequestContext).Action("GenerateAttendance", "Attendance", new { /* params */ });
            //return Json(model: model, isValid: true, JsonRequestBehavior.AllowGet);   
            //return Json({isValid: true,    success: "Successfully inserted rows:" + inserted.ToString() + ", Already exist rows:" + alredy.ToString()"  }, JsonRequestBehavior.AllowGet); 
            //return Json(new { success = true, message = TempData["message"], url = redirectUrl });
            //TempData["message"] = "Successfully inserted rows:" + inserted.ToString() + ", Already exist rows:" + alredy.ToString();                    
            //return RedirectToAction("GenerateAttendance");
        }
        #endregion

        #region --------- GeneratedReport ----------
        public ActionResult GeneratedReport(int? month, int? year)
        {
            var firmid = LoginUserFirmId();
            var staffsallary = _db.SalaryReport(month, year, firmid);// _db.MonthlyAttenDetails.Include(s => s.Staff).Where(a => a.tranMonth == month && a.tranYearyear == year);

            var result = staffsallary.Select(a => new MonthlyAttenDetail()
            {
                absent = a.absent,
                currentMonthCompansation = a.currentMonthCompansation,
                DiductionAmount = a.Expr1,
                earnCompansation = a.earnCompansation,
                Holidays = a.Holidays,
                monthlyAttenDetailId = a.monthlyAttenDetailId,
                monthlyPaidLeaves = a.monthlyPaidLeaves,
                paidLeaves = a.paidLeaves,
                preMonthCompansation = a.preMonthCompansation,
                PresentDays = a.PresentDays,
                staffId = a.staffId,
                firstName = a.firstName,
                ctcId = a.ctcId,
                middleName = a.middleName,
                lastName = a.lastName,
                paybleDays = a.paybleDays,
                totalAmount = a.totalAmount,
                usedCompansation = a.usedCompansation,
                totalMonthDays = a.totalMonthDays,
                weeklyOff = a.weeklyOff,
                staffCode = a.staffCode,
                tranMonth = a.tranMonth,
                tranYearyear = a.tranYearyear
            });
            return View(result.ToList());
        }
        #endregion

        #region ----- Delete and Delete Confirm ----
        public ActionResult Delete(int? id, int month, int year)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyAttenDetail monthlyAttend = _db.MonthlyAttenDetails.Find(id);
            if (monthlyAttend == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(monthlyAttend);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int month, int year)
        {
            MonthlyAttenDetail monthlyAttend = _db.MonthlyAttenDetails.Find(id);
            var generate = _db.GeneratedSalaryVariables.Where(a => a.monthlyAttenDetailId == id);
            foreach (var monthlyids in generate)
            {
                _db.GeneratedSalaryVariables.Remove(monthlyids);
            }
            _db.MonthlyAttenDetails.Remove(monthlyAttend);
            _db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("GeneratedReport", new { month = month, year = year });
        }


        #endregion

        #region ------- GenerateSalary -------------
        public ActionResult GenerateSalary(int? month, int? year)
        {
            if (month == null && year != null || month != null && year == null)
            {
                TempData["AlreadyGenerate"] = "Select Month And Year Both !!!";
                return RedirectToAction("GenerateSalary");
            }

            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            DateTime date = DateTime.UtcNow;
            var start = new DateTime((int)year, (int)month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            var modelList = new List<MonthllyAttendanceModel>();
            var firmid = LoginUserFirmId();
            var getList = _db.GenarateSalary(month, year, firmid).GroupBy(a => a.monthlyattendanceId);
            foreach (var lists in getList)
            {
                var staffId = lists.FirstOrDefault().staffId;
                if (!_db.MonthlyAttenDetails.Any(m => m.generatedBySalary != null && m.tranMonth == month && m.tranYearyear == year && m.staffId == staffId))
                {
                    var model = new MonthllyAttendanceModel();
                    model.StaffId = staffId;
                    var genarateSalaryResult = lists.FirstOrDefault();
                    if (genarateSalaryResult != null)
                        if (genarateSalaryResult.monthlyattendanceId != null)
                            model.MonthlyAttenDetailId = (int)genarateSalaryResult.monthlyattendanceId;
                    var firstOrDefault = lists.FirstOrDefault();
                    if (firstOrDefault != null) model.Month = (int)firstOrDefault.tranMonth;
                    var salaryResult = lists.FirstOrDefault();
                    if (salaryResult != null) model.EmployeeName = salaryResult.FullName;
                    model.Year = (int)lists.FirstOrDefault().tranYearyear;
                    model.MonthDays = (int)lists.FirstOrDefault().totalMonthDays;
                    model.PaybleDays = lists.FirstOrDefault().PaybleDays;
                    model.TotalAmount = lists.FirstOrDefault().TotalSalary;
                    model.CtcId = lists.FirstOrDefault().ctcId == null ? (int?)null : (int)lists.FirstOrDefault().ctcId;
                    model.Absent = lists.FirstOrDefault().absence;
                    model.PaidLeaves = lists.FirstOrDefault().leaves;
                    model.Month = start.Month;
                    model.MonthString = start.ToString("MMMMMMMMMMMMM");
                    model.Year = start.Year;
                    model.AbsentAmount = lists.FirstOrDefault().absenceAmt;
                    if (lists.FirstOrDefault().ctcId != null)
                        modelList.Add(model);
                }
            }
            return View(modelList.ToList());
        }
        #endregion

        #region --------- PreSalaryReport ----------
        public ActionResult PreSalaryReport(int? month, int? year)
        {
            var firmid = LoginUserFirmId();
            if (month == null && year != null || month != null && year == null)
            {
                TempData["AlreadyGenerate"] = "Select Month And Year Both !!!";
                return RedirectToAction("GenerateSalary");
            }
            //if (_db.MonthlyAttenDetails.Any(m => m.generatedBySalary != null && m.tranMonth==month && m.tranYearyear==year))
            //{
            //    TempData["AlreadyGenerate"] = "This month salary already generated !!!";
            //    return RedirectToAction("GenerateSalary");
            //}
            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            DateTime date = DateTime.UtcNow;

            var start = new DateTime((int)year, (int)month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            var modelList = new List<MonthllyAttendanceModel>();

            var getList = _db.PreSalaryReport(month, year, firmid);
            //var getList = _db.PreSalaryMonthwise(month, year);
            foreach (var lists in getList)
            {
                var model = new MonthllyAttendanceModel();
                model.StaffId = lists.staffId;
                if (lists.tranMonth != null) model.Month = (int)lists.tranMonth;
                model.EmployeeName = lists.Employeename;
                if (lists.tranYear != null) model.Year = (int)lists.tranYear;
                if (lists.totalMonthDays != null) model.MonthDays = (int)lists.totalMonthDays;

                model.TotalAmount = lists.ctc;
                model.Absent = lists.absent;
                model.PaidLeaves = lists.paidLeaves;
                if (lists.tranMonth != null) model.Month = (int)lists.tranMonth;
                model.DeductionAmount = lists.Deduction + (lists.UT) + lists.AllowanceDebit;
                model.NetAmount = (lists.ctc + lists.AllowanceCredit - lists.AllowanceDebit - (lists.UT) - lists.Deduction);
                model.MonthString = start.ToString("MMMMMMMMMMMMM");
                model.Year = start.Year;
                model.AbsentAmount = lists.Deduction;
                model.CalculateOn = lists.CalculateOn;
                model.SalaryFormula = lists.SalaryFormula;
                model.PaybleDays = lists.payableDays;
                model.AllowanceCredit = lists.AllowanceCredit;
                model.AllowanceDebit = lists.AllowanceDebit;
                model.MonthworkingDays = lists.monthworkingDays.ToString();
                model.TotalEarning = lists.ctc + lists.AllowanceCredit;
                model.MonthlyAttenDetailId = lists.MonthlyAttendDetailId;
                model.StaffId = lists.staffId;
                model.ctcId = lists.ctcId;
                model.Undertime = (lists.UT);
                model.Overtime = lists.OT;
                modelList.Add(model);
            }
            return View(modelList.ToList());
        }
        #endregion

        #region ----- GenerateSalaryDataStore ------
        public ActionResult GenerateSalaryDataStore(string monthlyAttenDetailId, string ctcId, string month, string year, string monthDays, string absents, string absentsAmount, string paybleDays, string totalAmount, string AllowanceCredit, string AllowanceDebit, string TotalEarning, string DeductionAmount, string NetAmount, string staffId, string Overtime, string UnderTime)
        {
            var monthlyAttenDetailIdObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(monthlyAttenDetailId);
            var monthlyAttenDetailIdObjectEnum = monthlyAttenDetailIdObj as object[] ?? monthlyAttenDetailIdObj.Cast<object>().ToArray();

            var ctcIdObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(ctcId);
            var ctcIdEnum = ctcIdObj as object[] ?? ctcIdObj.Cast<object>().ToArray();

            var monthObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(month);
            var monthEnum = monthObj as object[] ?? monthObj.Cast<object>().ToArray();

            var yearObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(year);
            var yearEnum = yearObj as object[] ?? yearObj.Cast<object>().ToArray();

            var monthDaysObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(monthDays);
            var monthDaysEnum = monthDaysObj as object[] ?? monthDaysObj.Cast<object>().ToArray();

            var absentsObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(absents);
            var absentsEnum = absentsObj as object[] ?? absentsObj.Cast<object>().ToArray();

            var absentsAmtOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(absentsAmount);
            var absentsAmtEnumerable = absentsAmtOjbect as object[] ?? absentsAmtOjbect.Cast<object>().ToArray();

            var paybleDaysObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(paybleDays);
            var paybleDaysObjEnum = paybleDaysObj as object[] ?? paybleDaysObj.Cast<object>().ToArray();

            var totalAmountObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(totalAmount);
            var totalAmountEnum = totalAmountObj as object[] ?? totalAmountObj.Cast<object>().ToArray();

            var AllowanceCreditObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(AllowanceCredit);
            var AllowanceCreditEnum = AllowanceCreditObj as object[] ?? AllowanceCreditObj.Cast<object>().ToArray();

            var AllowanceDebitObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(AllowanceDebit);
            var AllowanceDebitEnum = AllowanceDebitObj as object[] ?? AllowanceDebitObj.Cast<object>().ToArray();

            var TotalEarningObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(TotalEarning);
            var TotalEarningEnum = TotalEarningObj as object[] ?? TotalEarningObj.Cast<object>().ToArray();

            var DeductionAmountObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(DeductionAmount);
            var DeductionAmountEnum = DeductionAmountObj as object[] ?? DeductionAmountObj.Cast<object>().ToArray();

            var NetAmountObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(NetAmount);
            var NetAmountEnum = NetAmountObj as object[] ?? NetAmountObj.Cast<object>().ToArray();


            var staffIdObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(staffId);
            var staffIdEnum = staffIdObj as object[] ?? staffIdObj.Cast<object>().ToArray();

            var OvertimeObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(Overtime);
            var OvertimeEnum = OvertimeObj as object[] ?? OvertimeObj.Cast<object>().ToArray();

            var UndertimeObj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(UnderTime);
            var UndertimeEnum = UndertimeObj as object[] ?? UndertimeObj.Cast<object>().ToArray();

            var alredy = 0;
            var inserted = 0;
            for (int i = 0; i < monthlyAttenDetailIdObjectEnum.Count(); i++)
            {
                var monthlyAttenId = Convert.ToInt32(monthlyAttenDetailIdObjectEnum[i]);
                var staffid = Convert.ToInt32(staffIdEnum[i]);
                var tranMonths = Convert.ToInt32(monthEnum[i]);
                int tranYearyears = Convert.ToInt32(yearEnum[i]);
                decimal absentsAmt = Convert.ToDecimal(absentsAmtEnumerable[i]);
                int ctcIds = Convert.ToInt32(ctcIdEnum[i]);
                decimal paybleDayss = Convert.ToDecimal(paybleDaysObjEnum[i]);
                decimal NetAmounts = Convert.ToDecimal(NetAmountEnum[i]);
                decimal AllowanceCredits = Convert.ToDecimal(AllowanceCreditEnum[i]);
                decimal AllowanceDebits = Convert.ToDecimal(AllowanceDebitEnum[i]);
                decimal TotalEarnings = Convert.ToDecimal(TotalEarningEnum[i]);
                decimal DeductionAmounts = Convert.ToDecimal(DeductionAmountEnum[i]);
                decimal Overtimes = Convert.ToDecimal(OvertimeEnum[i]);
                decimal Undertimes = Convert.ToDecimal(UndertimeEnum[i]);

                if (_db.MonthlyAttenDetails.Any(a => a.monthlyAttenDetailId == monthlyAttenId))
                {
                    var firmid = LoginUserFirmId();
                    _db.SalaryUpdateAndInsertEmployee(monthlyAttenId, staffid, tranMonths, tranYearyears, absentsAmt, ctcIds,
                        paybleDayss, NetAmounts, AllowanceCredits, AllowanceDebits, TotalEarnings, DeductionAmounts, Overtimes, Undertimes, firmid, GetUserId());

                }
            }
            var result = new { Success = "true", Message = "Record Save Successfully" };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("GenerateSalary");
        }
        #endregion

        #region ------------ Payslip ---------------
        public ActionResult Payslip(int? month, int? year)
        {
            var staffId = LoginEmployeeId();
            if (month != null && year != null)
            {
                var monthy =
                    _db.MonthlyAttenDetails.Where(
                        a => a.tranMonth == month && a.tranYearyear == year && a.staffId == staffId);
                if (!monthy.Any())
                {
                    TempData["EmptyMessage"] = "salary";
                    var data1 = _db.GetPaySlip(month, year, LoginEmployeeId());
                    var model1 = data1.Select(a => new PaySlipModel()
                    {

                    }).ToList();
                    return View(model1);
                }
            }
            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var start = new DateTime((int)year, (int)month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            var firmid = LoginUserFirmId();
            var data = _db.GetPaySlip(month, year, LoginEmployeeId());
            var firminfo = _db.FirmInfoes.FirstOrDefault(a => a.firmId == firmid);
            var model = data.Select(a => new PaySlipModel()
            {
                EmployeeCode = a.staffCode,
                EmployeeName = a.firstName + " " + a.middleName + " " + a.lastName,
                Designation = a.designation,
                DeptName = a.deptName,
                HeadName = a.payslipname,
                WeeklyOff = a.weeklyOff,
                Holidays = a.Holidays,
                TotalWoHolidays = (a.weeklyOff + a.Holidays),
                TotalWorkingDays = a.totalMonthDays - (a.weeklyOff + a.Holidays),
                TotalMonthDays = a.totalMonthDays,
                Absence = a.absent,
                PaidLeaves = a.paidLeaves,
                PAN = a.PAN,
                EarnCompansation = a.earnCompansation,
                HeadAmount = a.amount,
                GrossAmount = a.totalAmount,
                PaybleDays = a.paybleDays,
                IsDuductions = Convert.ToBoolean(a.isDeductions),
                IsEarnings = Convert.ToBoolean(a.isEarnings),
                JoiningDate = a.joingDate,
                Month = new DateTime((int)a.tranYearyear, (int)a.tranMonth, 1).ToString("MMM", CultureInfo.InvariantCulture),
                Year = (int)a.tranYearyear,
                FirmName = firminfo.firmName,
                Logo = firminfo.logo,
                Address = firminfo.flatNo + ", " + firminfo.street + ", " + firminfo.area,
                AddressPart = firminfo.city + ", " + firminfo.state + "- " + firminfo.pincode,
                Contact = firminfo.contact,
            }).ToList();
            return View(model);
        }
        #endregion

        #region --- AllEmpAttendSheetHorizontal ----
        public ActionResult AllEmpAttendSheetHorizontal(DateTime? datepicker, DateTime? datepicker2)
        {
            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("AllEmpAttendSheetHorizontal");
            }
            if (datepicker == null && datepicker2 == null)
            {
                //DateTime date = DateTime.UtcNow;
                //var start = new DateTime(date.Year, date.Month, 1);
                //var end = start.AddMonths(1).AddDays(-1);                
                //var firmid = LoginUserFirmId();
                //var getList = _db.GetAttendance(start, end, firmid, null);

                //var atsheetModel = getList.Select(a => new AttendanceSheetModel()
                //{
                //    EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                //    EnrollNumber = a.EnrollNumber,
                //    EmployeeCode = a.employeeCode,
                //    StaffId = a.StaffId,
                //    TranDate = a.ThatDay,
                //    ChekIn = a.ChekIn,
                //    ChekOut = a.ChekOut,
                //    TotalTime = a.TotalTime,
                //    ThatDayStatus = a.ThatDayStatus,
                //    HolidayStatus = a.HolidayStatus,
                //    NetDuration = a.totalNetDuration,
                //    Sesssion = a.sesssion,
                //    IsStart = a.isStart,
                //    LeaveDays = a.LeaveDays,
                //    PresentFlag = a.PresentFlag,
                //    Componensation = a.Componensation,
                //    ComponensationBalance = a.ComponensationBalance,
                //    ShiftName = a.shiftname
                //    ,
                //    sandwitch = a.sandwitchstatus
                //});
                //return View(atsheetModel.ToList());

                var data1 = _db.GetAttendance(null, null, null, null);
                var model1 = data1.Select(a => new AttendanceSheetModel()
                {

                }).ToList();
                TempData["notice"] = "ModelNull";
                return View(model1);
            }
            else
            {
                var firmid = LoginUserFirmId();
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var getList = _db.GetAttendance(start, end, firmid, null);
                var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                    // NetDuration=duration.Where(s=>s.staffId==a.StaffId&&s.dates==a.ThatDay).FirstOrDefault().totalDuration,
                    Sesssion = a.sesssion,
                    IsStart = a.isStart,
                    LeaveDays = a.LeaveDays,
                    PresentFlag = a.PresentFlag,
                    Componensation = a.Componensation,
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }

        }
        #endregion

        #region All Employee net Duration Horizontal
        public ActionResult AllEmpNetDurHorizontal(DateTime? datepicker)
        {
            if (datepicker == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, date.Day);
                var data = _db.NetDurationDaywiseAllEmp(start);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.EmployeeName,
                    TranDate = a.Time,
                    UserName = a.dates,
                    EventType = a.EventType
                }).ToList();

                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                var data = _db.NetDurationDaywiseAllEmp(start);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.EmployeeName,
                    TranDate = a.Time,
                    UserName = a.dates,
                    EventType = a.EventType
                }).ToList();

                return View(atsheetModel);
            }
        }

        public ActionResult AllMonthEmpNetDurHorizontal(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var staffId = LoginEmployeeId();
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var data = _db.NetDurationMonthwisesingleEmp(start, end, staffId);

                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.EmployeeName,
                    TranDate = a.Time,
                    UserName = a.dates,
                    EventType = a.EventType

                }).ToList();

                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;

                var data = _db.NetDurationMonthwisesingleEmp(start, end, empId);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.EmployeeName,
                    TranDate = a.Time,
                    UserName = a.dates,
                    EventType = a.EventType
                }).ToList();

                return View(atsheetModel);
            }
        }
        #endregion

        #region  ----- AllEmployeeMachineData ------
        public ActionResult AllEmpMachineData(DateTime? datepicker)
        {
            var firmId = LoginUserFirmId();
            if (datepicker == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, date.Day);

                var data = _db.AllEmpDateWIseMachineData(start, firmId);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + ": " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    Sesssion = a.Status,
                    UserName = a.dates
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                var data = _db.AllEmpDateWIseMachineData(start, firmId);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    Sesssion = a.Status,
                    UserName = a.dates
                }).ToList();

                return View(atsheetModel);
            }
        }
        #endregion

        #region ---------- EventLog ----------------
        public ActionResult EventLog(DateTime? date)
        {
            var loginId = LoginEmployeeId();
            if (date != null)
            {
                var dd = _db.EventLogTran(loginId, date);
                return View(dd.ToList());
            }
            return View();
        }
        #endregion

        #region ----------- Hr EventLog ------------
        public ActionResult EventLogHR(int? empId, DateTime? date)
        {
            if (empId != null && date != null)
            {
                var dd = _db.EventLogTran(empId, date);
                return View(dd.ToList());
            }
            return View();
        }
        #endregion

        #region --------- PayslipForAdmin ----------
        public ActionResult PayslipForAdmin(int? month, int? year, int? empId)
        {
            var firmids = LoginUserFirmId();
            ViewBag.staffid =
          _db.Staffs.ToList().Where(s => s.firmId == firmids && s.isActive == true).Select(a => new SelectListItem()
          {
              Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
              Value = a.staffId.ToString()
          });

            if (month != null && year != null && empId != null)
            {
                var monthy =
                    _db.MonthlyAttenDetails.Where(
                        a => a.tranMonth == month && a.tranYearyear == year && a.staffId == empId);
                if (!monthy.Any())
                {
                    var data1 = _db.GetPaySlip(month, year, LoginEmployeeId());
                    var model1 = data1.Select(a => new PaySlipModel()
                    {

                    }).ToList();
                    TempData["SalaryNotGenerate"] = "salary not generated or assign structure with pay heads! ";
                    return View(model1);

                }
                var firmid = LoginUserFirmId();
                var data = _db.GetPaySlip(month, year, empId);
                var firminfo = _db.FirmInfoes.FirstOrDefault(a => a.firmId == firmid);

                var model = data.Select(a => new PaySlipModel()
                {
                    EmployeeCode = a.staffCode,
                    EmployeeName = a.firstName + " " + a.middleName + " " + a.lastName,
                    Designation = a.designation,
                    DeptName = a.deptName,
                    HeadName = a.payslipname,
                    WeeklyOff = a.weeklyOff,
                    Holidays = a.Holidays,
                    TotalWoHolidays = (a.weeklyOff + a.Holidays),
                    TotalWorkingDays = a.totalMonthDays - (a.weeklyOff + a.Holidays),
                    TotalMonthDays = a.totalMonthDays,
                    Absence = a.absent,
                    PaidLeaves = a.paidLeaves,
                    EarnCompansation = a.earnCompansation,
                    HeadAmount = a.amount,
                    GrossAmount = a.totalAmount,
                    PaybleDays = a.paybleDays,
                    IsDuductions = Convert.ToBoolean(a.isDeductions),
                    IsEarnings = Convert.ToBoolean(a.isEarnings),
                    JoiningDate = a.joingDate,
                    Month = new DateTime((int)a.tranYearyear, (int)a.tranMonth, 1).ToString("MMM", CultureInfo.InvariantCulture),
                    Year = (int)a.tranYearyear,
                    FirmName = firminfo.firmName,
                    Logo = firminfo.logo,
                    Address = firminfo.flatNo + ", " + firminfo.street + ", " + firminfo.area,
                    AddressPart = firminfo.city + ", " + firminfo.state + "- " + firminfo.pincode,
                    Contact = firminfo.contact,
                    PAN = a.PAN,
                    PF = a.PF,
                }).ToList();
                return View(model);
            }
            else
            {
                var data1 = _db.GetPaySlip(month, year, LoginEmployeeId());
                var model1 = data1.Select(a => new PaySlipModel()
                {

                }).ToList();
                TempData["notice"] = "ModelNull";
                return View(model1);
            }
        }
        #endregion

        #region ----- AttendaceChekinCheckOut ------
        public ActionResult AttendaceChekinCheckOut(DateTime? selectedDate)
        {
            var firmid = LoginUserFirmId();
            if (selectedDate == null)
            {
                selectedDate = DateTime.Now;
            }
            var data = _db.AsignShiftBasedOnAttendaceInTime(firmid, selectedDate);

            var attendanceSheetModel =
                data.Select(
                    a =>
                        new AsignShiftBasedOnAttendaceInTimeModel()
                        {
                            attendDate = a.attendDate,
                            dt = a.dt,
                            staffId = a.shiftid,
                            CheckIn = a.CheckIn,
                            staffName = a.staffName,
                            shiftid = a.shiftid,
                            ShiftName = a.ShiftName,
                            Status = a.Status
                        });
            return View(attendanceSheetModel.ToList());
        }
        #endregion

        #region -------- CheckIn-Out test ----------
        //public ActionResult CheckInCheckOutTest(DateTime? fromDate, DateTime? toDate)
        //{
        //    var firm = LoginUserFirmId();
        //    if (fromDate == null && toDate ==null)
        //    {
        //        fromDate = DateTime.Now;
        //        toDate = DateTime.Now;
        //    }

        //    var data = _db.CheckInCheckOutTest(fromDate, toDate, firm,);
        //    var attendanceSheetModel =
        //                   data.Select(
        //                       a =>
        //                           new AsignShiftBasedOnAttendaceInTimeModel()
        //                           {
        //                               attendDate = a.attendDate,
        //                            staffId = a.staffId,
        //                               CheckIn = a.CheckIn,
        //                               CheckOut = a.CheckOut,
        //                               TotalDuration = a.Elapsed_Time,
        //                               staffName = a.staffName,
        //                             ShiftName = a.ShiftName

        //                           });


        //    return View(attendanceSheetModel.ToList());
        //}
        #endregion

        #region ------ CheckInCheckoutallemp -------
        public ActionResult CheckInCheckoutallemp(DateTime? dt)
        {
            var firm = LoginUserFirmId();
            if (dt == null)
            {
                dt = DateTime.Now;
            }
            var data = _db.AllCheckinandCkeckOutAllEmp(dt, firm);


            var StaffList =
                 _db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                 {
                     Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                     Value = a.staffId.ToString()
                 });

            ViewBag.stafflist = StaffList;
            var attendanceSheetModel =
                           data.Select(
                               a =>
                                   new AsignShiftBasedOnAttendaceInTimeModel()
                                   {
                                       attendDate = a.attendDate,
                                       staffId = a.staffId,
                                       inOutMode = a.inOutMode,
                                       ShiftName = a.ShiftName,
                                       staffName = a.StaffName
,
                                   });

            return View(attendanceSheetModel.ToList());
        }
        #endregion

        #region -------- GetAllAttendace -----------
        public ActionResult GetAllAttendace(DateTime? fromDate, DateTime? toDate, int? empId)
        {
            var firmid = LoginUserFirmId();
            if (fromDate == null && toDate == null && empId == null)
            {
                fromDate = DateTime.Now;
                toDate = DateTime.Now;
                empId = Convert.ToInt32(LoginEmployeeId());
            }

            var data = _db.CheckInCheckOutTest(fromDate, toDate, firmid, empId);

            var attendanceSheetModel =
                           data.Select(
                               a =>
                                   new AsignShiftBasedOnAttendaceInTimeModel()
                                   {
                                       attendDate = a.attendDate,
                                       staffId = a.staffId,
                                       CheckIn = a.CheckIn,
                                       CheckOut = a.CheckOut,
                                       TotalDuration = a.Elapsed_Time,
                                       staffName = a.staffName,
                                       ShiftName = a.ShiftName,
                                       Status = a.Present
                                   });


            return View(attendanceSheetModel.ToList());
        }
        #endregion

        #region ---------- ManuallyCheckin ---------
        public ActionResult ManuallyCheckin(int Attendid, bool Status)
        {
            if (Attendid != null && Status != null)
            {
                var data = _db.AttendanceStaffs.FirstOrDefault(x => x.attendId == Attendid);
                if (data != null)
                {
                    data.isManuallyCheckIn = Status;
                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region --------- MachineDataSelf ----------

        public ActionResult MachineDataself(DateTime? datepicker, DateTime? datepicker2, string AttendmodeList)
        {
            var firmId = LoginUserFirmId();
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var staffId = LoginEmployeeId();
                var data = _db.MachineDatanew(start, end, staffId, firmId, AttendmodeList);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + ": " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn,
                    inOutMode = a.inOutMode
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var staffId = LoginEmployeeId();
                var data = _db.MachineDatanew(start, end, staffId, firmId, AttendmodeList);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn,
                    inOutMode = a.inOutMode
                }).ToList();
                return View(atsheetModel);
            }
        }

        #endregion

        #region ---- NetDurationAllEmployeeAdmin ---
        public ActionResult NetDurationAllEmployee(DateTime? fromDate, DateTime? toDate, int? staffid)
        {
            var firm = LoginUserFirmId();
            ViewBag.staffid =
                _db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });

            if (fromDate == null && toDate == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);


                var data = _db.NetDurationAllEmp(start, end, firm, null);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                TotalDuration = a.TotalDuration,
                                Trandate = a.trandate,
                                UserNames = a.UserNames
                            });
                return View(AllEmployeeNetDuration.ToList());
            }
            else
            {
                var data = _db.NetDurationAllEmp(fromDate, toDate, firm, staffid);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                TotalDuration = a.TotalDuration,
                                Trandate = a.trandate,
                                UserNames = a.UserNames
                            });
                return View(AllEmployeeNetDuration.ToList());
            }
        }
        #endregion

        #region --- GetAllNetDuratuionEntryAdmin ---

        public ActionResult GetAllNetDuratuionEntry(DateTime? fromDate, DateTime? toDate, int? staffid)
        {
            var firm = LoginUserFirmId();

            ViewBag.staffid =
                _db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });

            if (fromDate == null && toDate == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = _db.GetAllNetDuratuionEntry(start, end, firm, null);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                Staffid = a.staffid,
                                Trandate = a.TranDate,
                                TransactionDate = a.transactionDate,
                                TransactionTime = a.transactionTime,
                                UserNames = a.Usernames,
                                LoginType = a.LogType,
                                FirmId = a.FirmId,

                            });
                return View(AllEmployeeNetDuration.ToList());
            }
            else
            {
                var data = _db.GetAllNetDuratuionEntry(fromDate, toDate, firm, staffid);
                var AllEmployeeNetDuration = data.Select(a => new NetDurationModel()
                {
                    Staffid = a.staffid,
                    Trandate = a.TranDate,
                    TransactionDate = a.transactionDate,
                    TransactionTime = a.transactionTime,
                    LoginType = a.LogType,
                    UserNames = a.Usernames,
                    FirmId = a.FirmId
                });
                return View(AllEmployeeNetDuration.ToList());
            }
        }
        #endregion

        #region -------- GetNetDurationSelf --------

        public ActionResult GetNetDurationEmployeeself(DateTime? fromDate, DateTime? toDate)
        {
            var firm = LoginUserFirmId();
            var staffid = LoginEmployeeId();
            if (fromDate == null && toDate == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = _db.GetAllNetDuratuionEntry(start, end, firm, staffid);
                var AllEmployeeNetDuration = data.Select(a => new NetDurationModel()
                {
                    Staffid = a.staffid,
                    Trandate = a.TranDate,
                    TransactionDate = a.transactionDate,
                    TransactionTime = a.transactionTime,
                    LoginType = a.LogType,
                    UserNames = a.Usernames,
                    FirmId = a.FirmId
                });
                return View(AllEmployeeNetDuration.ToList());
            }
            else
            {
                var data = _db.GetAllNetDuratuionEntry(fromDate, toDate, firm, staffid);
                var AllEmployeeNetDuration = data.Select(a => new NetDurationModel()
                {
                    Staffid = a.staffid,
                    Trandate = a.TranDate,
                    TransactionDate = a.transactionDate,
                    TransactionTime = a.transactionTime,
                    LoginType = a.LogType,
                    UserNames = a.Usernames,
                    FirmId = a.FirmId
                });
                return View(AllEmployeeNetDuration.ToList());
            }
        }
        #endregion

        #region ---- NetDurationAllEmployeeSelf ----
        public ActionResult NetDurationAllEmployeeSelf(DateTime? fromDate, DateTime? toDate)
        {
            var firm = LoginUserFirmId();
            var staffid = LoginEmployeeId();
            ViewBag.staffid =
                _db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
            if (fromDate == null && toDate == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var data = _db.NetDurationAllEmp(start, end, firm, staffid);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                TotalDuration = a.TotalDuration,
                                Trandate = a.trandate,
                                UserNames = a.UserNames
                            });
                return View(AllEmployeeNetDuration.ToList());
            }
            else
            {
                var data = _db.NetDurationAllEmp(fromDate, toDate, firm, staffid);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                TotalDuration = a.TotalDuration,
                                Trandate = a.trandate,
                                UserNames = a.UserNames
                            });
                return View(AllEmployeeNetDuration.ToList());
            }
        }
        #endregion

        #region --- GetAttendanceCheckInCheckOut ---
        public ActionResult AllAttendaceEntries(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var firmid = LoginUserFirmId();
            ViewBag.staffid =
              _db.Staffs.ToList().Where(s => s.firmId == firmid && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                // var staffid = LoginEmployeeId();
                var data = _db.AllAttendaceEntry(start, end, firmid, null);
                var result = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffname,
                    EmployeeCode = a.schoolCode,
                    TranDate = a.attendDate,
                    AttendMode = a.inOutmode
                });
                return View(result.ToList());
            }
            else
            {
                var start = datepicker;
                var end = datepicker2;
                //if (empId == null)
                //{
                //    empId = LoginEmployeeId();
                //}

                var data = _db.AllAttendaceEntry(start, end, firmid, empId);
                var result = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffname,
                    EmployeeCode = a.schoolCode,
                    TranDate = a.attendDate,
                    AttendMode = a.inOutmode
                });
                return View(result.ToList());
            }


            //var data = db.AttendanceRequestListForMgr(loginId, firm, 0, null, null);
            return View();
        }
        #endregion

        #region ------ AttendanceBrekingShift ------
        public ActionResult AttendanceBrekingShift(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var firmid = LoginUserFirmId();
            ViewBag.staffid =
              _db.Staffs.ToList().Where(s => s.firmId == firmid && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });

            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-31);

                var data = _db.Brekingshift(start, end, firmid, null);
                var result = data.Select(a => new NetDurationModel()
                {
                    Staffname = a.staffname,
                    Trandate = a.tranDate,
                    UserNames = a.Username,
                    TotalnetDuration = a.TotalnetDuration
                });
                return View(result.ToList());
            }
            else
            {
                var data = _db.Brekingshift(datepicker, datepicker2, firmid, empId);
                var result = data.Select(a => new NetDurationModel()
                {
                    Staffname = a.staffname,
                    Trandate = a.tranDate,
                    UserNames = a.Username,
                    TotalnetDuration = a.TotalnetDuration
                });
                return View(result.ToList());
            }
            return View();
        }
        #endregion

        #region ---- AttendanceBrekingShiftSelf ----

        public ActionResult Machinenetdurationself(DateTime? datepicker, DateTime? datepicker2)
        {
            var firmid = LoginUserFirmId();
            var empid = LoginEmployeeId();
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = _db.Brekingshift(start, end, firmid, empid);
                var result = data.Select(a => new NetDurationModel()
                {
                    Staffname = a.staffname,
                    Trandate = a.tranDate,
                    UserNames = a.Username,
                    TotalnetDuration = a.TotalnetDuration

                });
                return View(result.ToList());
            }
            else
            {
                var data = _db.Brekingshift(datepicker, datepicker2, firmid, empid);
                var result = data.Select(a => new NetDurationModel()
                {
                    Staffname = a.staffname,
                    Trandate = a.tranDate,
                    UserNames = a.Username,
                    TotalnetDuration = a.TotalnetDuration

                });
                return View(result.ToList());
            }
            return View();
        }

        #endregion

        #region ----- Breaking Shift For Mgr -------
        public ActionResult BrekingShiftForMgr(DateTime? datepicker, DateTime? datepicker2, int? empId)
        {
            var firmid = LoginUserFirmId();
            var loginuser = LoginEmployeeId();

            // var firm = LoginUserFirmId();
            ViewBag.staffid =
              _db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firmid).Where(a => a.ReportingManagerId == loginuser).Select(a => new SelectListItem()
              {
                  Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                  Value = a.Staff.staffId.ToString()
              });

            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = _db.BrekingshiftForMgr(loginuser, start, end, firmid, null);
                var result = data.Select(a => new NetDurationModel()
                {
                    Staffname = a.staffname,
                    Trandate = a.tranDate,
                    UserNames = a.Username,
                    TotalnetDuration = a.TotalnetDuration

                });
                return View(result.ToList());
            }
            else
            {
                var data = _db.BrekingshiftForMgr(loginuser, datepicker, datepicker2, firmid, empId);
                var result = data.Select(a => new NetDurationModel()
                {
                    Staffname = a.staffname,
                    Trandate = a.tranDate,
                    UserNames = a.Username,
                    TotalnetDuration = a.TotalnetDuration

                });
                return View(result.ToList());
            }
            return View();
        }
        #endregion

        #region ---- Get AllNetDuration for Mgr ----

        public ActionResult GetAllNetDuratuionEntryForMgr(DateTime? fromDate, DateTime? toDate, int? empId)
        {
            var loginuser = LoginEmployeeId();
            var firm = LoginUserFirmId();

            ViewBag.staffid =
         _db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firm).Where(a => a.ReportingManagerId == loginuser).Select(a => new SelectListItem()
         {
             Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
             Value = a.Staff.staffId.ToString()
         });
            if (fromDate == null && toDate == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = _db.GetAllNetDuratuionEntryForMgr(loginuser, start, end, firm, null);
                var allEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                Staffid = a.staffid,
                                Trandate = a.TranDate,
                                TransactionDate = a.transactionDate,
                                TransactionTime = a.transactionTime,
                                UserNames = a.Usernames,
                                LoginType = a.LogType,
                                FirmId = a.FirmId,

                            });
                return View(allEmployeeNetDuration.ToList());
            }
            else
            {
                var data = _db.GetAllNetDuratuionEntryForMgr(loginuser, fromDate, toDate, firm, empId);
                var allEmployeeNetDuration = data.Select(a => new NetDurationModel()
                {
                    Staffid = a.staffid,
                    Trandate = a.TranDate,
                    TransactionDate = a.transactionDate,
                    TransactionTime = a.transactionTime,
                    LoginType = a.LogType,
                    UserNames = a.Usernames,
                    FirmId = a.FirmId
                });
                return View(allEmployeeNetDuration.ToList());
            }
        }
        #endregion

        #region --- NetDurationAllEmployeeforMgr ---
        public ActionResult NetDurationAllEmployeeForMgr(DateTime? fromDate, DateTime? toDate, int? empId)
        {
            var loginuser = LoginEmployeeId();
            var firm = LoginUserFirmId();
            ViewBag.staffid =
            _db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firm).Where(a => a.ReportingManagerId == loginuser).Select(a => new SelectListItem()
            {
                Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                Value = a.Staff.staffId.ToString()
            });
            if (fromDate == null && toDate == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = _db.NetDurationAllEmpForMgr(loginuser, start, end, firm, null);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                TotalDuration = a.TotalDuration,
                                Trandate = a.trandate,
                                UserNames = a.UserNames
                            });
                return View(AllEmployeeNetDuration.ToList());
            }
            else
            {
                var data = _db.NetDurationAllEmpForMgr(loginuser, fromDate, toDate, firm, empId);
                var AllEmployeeNetDuration =
                    data.Select(
                        a =>
                            new NetDurationModel()
                            {
                                TotalDuration = a.TotalDuration,
                                Trandate = a.trandate,
                                UserNames = a.UserNames
                            });
                return View(AllEmployeeNetDuration.ToList());
            }
        }
        #endregion

        #region ----- AllStaffAttendDepttWise ------
        public ActionResult AllStaffAttendanceSheetDeptWise(DateTime? datepicker, DateTime? datepicker2, int? deptId)
        {
            var firmid = LoginUserFirmId();
            ViewBag.staffid =
          _db.Departments.ToList().Where(s => s.firmId == firmid).Select(a => new SelectListItem()
          {
              Text = a.deptName,
              Value = a.deptId.ToString()
          });

            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("AllStaffAttendanceSheetDeptWise");
            }
            if (datepicker == null && datepicker2 == null)
            {
                var date = DateTime.UtcNow;

                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var getList = deptId != 0 ? _db.GetAttendance(start, end, firmid, deptId).ToList() : _db.GetAttendance(start, end, firmid, deptId).ToList();
                var atsheetModel = getList.Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var getList = deptId != null ? _db.GetAttendance(start, end, firmid, deptId) : _db.GetAttendance(start, end, firmid, deptId);
                var atsheetModel = getList.Where(s => s.ThatDay >= start && s.ThatDay <= end).Select(a => new AttendanceSheetModel()
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
                    ComponensationBalance = a.ComponensationBalance,
                    ShiftName = a.shiftname,
                    sandwitch = a.sandwitchstatus
                }).ToList();
                return View(atsheetModel);
            }
        }
        #endregion

        #region --- Machine Data Department Wise ---

        public ActionResult MachineDataDeptWise(DateTime? datepicker, DateTime? datepicker2, int? deptId, string AttendmodeList)
        {
            var firmId = LoginUserFirmId();
            ViewBag.staffid =
            _db.Departments.ToList().Where(s => s.firmId == firmId).Select(a => new SelectListItem()
            {
                Text = a.deptName,
                Value = a.deptId.ToString()
            });

            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("MachineDataDeptWise");
            }
            if (datepicker == null && datepicker2 == null && AttendmodeList == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                AttendmodeList = "All";
                var staffId = LoginEmployeeId();
                var data = _db.MachineDataDepartmentWise(start, end, firmId, AttendmodeList, deptId);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + ": " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn,
                    inOutMode = a.inOutMode
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                if (datepicker == null)
                {
                    DateTime date = DateTime.UtcNow;
                    start = new DateTime(date.Year, date.Month, 1);
                }
                else
                {
                    start = (DateTime)datepicker;
                }
                if (datepicker2 == null)
                {
                    end = start.AddMonths(1).AddDays(-1);
                }
                else
                {
                    end = (DateTime)datepicker2;
                }

                var data = _db.MachineDataDepartmentWise(start, end, firmId, AttendmodeList, deptId);
                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                    AttendMode = a.AttendMode,
                    AttendId = a.attendId,
                    isManuallyCheckIn = a.isManuallyCheckIn,
                    inOutMode = a.inOutMode
                }).ToList();
                return View(atsheetModel);
            }
        }

        #endregion
    }
}