using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;
using edueTree.Services;

namespace edueTree.Controllers.Staffs
{
    // [Authorize]
    public class AttendanceStaffController : BaseController
    {
        #region ----------------- DbContext --------------------

        private dbContainer db = new dbContainer();

        #endregion 

        #region ------------------- Index ----------------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var staffattendance = db.AttendanceStaffs.Where(a => a.isManuallyCheckOut == true & a.firmId == firm);
            return View(staffattendance.ToList());

        }

        #endregion 

        #region ------------------ Details ---------------------
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceStaff attendancestaff = db.AttendanceStaffs.Find(id);
            if (attendancestaff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(attendancestaff);
        }
        #endregion 

        #region ------------------ Create ----------------------

        // GET: /AttendanceStaff/Create
        public ActionResult Create()
        {
            var firm = LoginUserFirmId();
            var staffAttend = new StaffAttendenceModel
            {
                StaffList = db.Staffs.ToList().Where(a => a.firmId == firm).Select(a => new SelectListItem()
                {
                    Text = a.staffCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.enrollNumber.ToString()
                })
            };
            return View(staffAttend);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffAttendenceModel attendancestaff)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();

                if (db.AttendanceStaffs.Any(m => m.enrollNumber == attendancestaff.EnrollNumber && m.attendDate == attendancestaff.AttendDate && m.inOutMode == attendancestaff.InOutMode))
                {
                    TempData["notice"] = "attend";
                    return RedirectToAction("Create");
                }
                else
                {
                    var attenfInfo = new AttendanceStaff();
                    var date = Convert.ToDateTime(attendancestaff.AttendDate);
                    attenfInfo.enrollNumber = attendancestaff.EnrollNumber;
                    attenfInfo.attendDate = new DateTime(date.Year, date.Month, date.Day, attendancestaff.Hour, attendancestaff.Minute, 00);
                    attenfInfo.verifyMode = 0;
                    attenfInfo.inOutMode = attendancestaff.InOutMode;
                    attenfInfo.worCode = 0;
                    attenfInfo.attendTime = null;
                    attenfInfo.isManuallyCheckOut = true;
                    attenfInfo.firmId = firm;

                    db.AttendanceStaffs.Add(attenfInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(attendancestaff);
        }
        #endregion 

        #region -------------------- Edit ----------------------

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceStaff attendancestaff = db.AttendanceStaffs.Find(id);
            if (attendancestaff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(attendancestaff);
        }

        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "attendId,enrollNumber,attendDate,verifyMode,inOutMode,worCode,attendTime,isManuallyCheckOut,firmId")] AttendanceStaff attendancestaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendancestaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attendancestaff);
        }

        #endregion 

        #region -------- Delete and Delete Confirm -------------
        // GET: /AttendanceStaff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceStaff attendancestaff = db.AttendanceStaffs.Find(id);
            if (attendancestaff == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(attendancestaff);
        }

        // POST: /AttendanceStaff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttendanceStaff attendancestaff = db.AttendanceStaffs.Find(id);
            db.AttendanceStaffs.Remove(attendancestaff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion 

        #region --------------- Despose ------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion 

        #region ------------- AttendanceRequest ----------------
        public ActionResult AttendanceRequest()
        {
            var attends = new AttendanceRequestModel();
            {
                int staffId = LoginEmployeeId();
                attends.StaffId = staffId;
            }
            return View(attends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceRequest(AttendanceRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                int staffId = LoginEmployeeId();
                var checkAlredyExists =
                    db.AttendanceRequests.Where(
                        a =>
                            a.firmId == firm && a.attendDate == request.AttendanceDate && a.staffId == staffId && a.inOutMode == request.InOutMode);
                if (!checkAlredyExists.Any())
                {
                    //int hour = request.Hour;
                    //String ampm = request.Mode;
                    //hour = ampm == "AM" ? hour : (hour % 12) + 12;
                    //int hourFormat = hour;

                    var lr = new AttendanceRequest();
                    var date = Convert.ToDateTime(request.AttendanceDate);
                    lr.staffId = staffId;
                    lr.transDate = DateTime.UtcNow;

                    lr.attendDate = new DateTime(date.Year, date.Month, date.Day, request.Hour, request.Minute, 00);
                    lr.inOutMode = request.InOutMode;
                    lr.narration = request.Narration;
                    lr.isApproved = null;
                    lr.firmId = firm;

                    db.AttendanceRequests.Add(lr);
                    TempData["notice"] = "success";
                    db.SaveChanges();


                    var requstId = lr.transId;
                    var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                    string checkmode;
                    if (request.InOutMode == 0)
                    {
                        checkmode = "Check In";
                    }
                    else
                    {
                        checkmode = "Check Out";
                    }
                    string mailSendTo = userdetail.email;
                    var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                    var ccmail = db.Staffs.FirstOrDefault(a => a.email == mailSendTo && a.staffId == staffId);

                    string mto = ccmail.emailReportingTo;

                    var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);
                    string emailabc = "";
                    foreach (var ex in reportingmails)
                    {
                        var mgeremail = db.Staffs.Where(s => s.staffId == ex.ReportingManagerId).FirstOrDefault().email;
                        emailabc = emailabc + mgeremail + ",";
                    }

                    string s2 = emailabc;
                    string[] words = s2.Split(',');
                    foreach (string word in words)
                    {
                        var dbmail = word;

                        var lnkHref1 =
                            "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                            Url.Action("ManuallyAttendanceRequestApprove", "AttendanceStaff",
                                new { requestId = requstId, empId = staffId }, "http") + "' > Approve </a> ";
                        var rejecthref =
                            "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                            Url.Action("ManuallyAttendanceRequestRejected", "AttendanceStaff",
                                new { requestId = requstId },
                                "http") + "' > Reject </a> ";
                        string subject1 = "Attendance Request -" + userdetail.firstName + " " + userdetail.lastName;
                        string body1 =
                            "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request for manually attendance- </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                            userdetail.firstName + " " + userdetail.lastName + "<br> Date: " +
                            Convert.ToDateTime(request.AttendanceDate).ToString("dd-MMM-yyyy") + "<br> Time:" +
                            request.Hour +
                            ":" + request.Minute + " " + checkmode +
                            "<br> Narration: " +
                            request.Narration + "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" +
                            lnkHref1 +
                            "</td><td style='padding: 10px;'>" + rejecthref + "</td></tr></table>";
                        var services = new MailServiceConfig();
                        var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress, dbs.Password,
                            dbs.HostName,
                            dbs.SMTPPortNo);
                    }
                    return RedirectToAction("AttendRequestList", "AttendanceStaff");
                }
            }
            return View(request);
        }


        public ActionResult ManuallyAttendanceRequestApprove(int? requestId, int? empId)
        {
            var request = db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId && a.isApproved == null || a.isApproved == false);
            if (request != null)
            {
                string enrollno = null;
                var EnrollNumber = db.Staffs.FirstOrDefault(a => a.staffId == empId);
                if (EnrollNumber != null)
                {
                    enrollno = Convert.ToString(EnrollNumber.enrollNumber);
                }
                var design = new AttendanceRequest
                {
                    transId = request.transId,
                    transDate = request.transDate,
                    attendDate = request.attendDate,
                    inOutMode = request.inOutMode,
                    narration = request.narration,
                    staffId = request.staffId,
                    isApproved = true,
                    firmId = request.firmId,
                };
                var attetend = new AttendanceStaff()
                {
                    enrollNumber = enrollno,
                    attendDate = request.attendDate,
                    verifyMode = 0,
                    inOutMode = request.inOutMode,
                    worCode = 0,
                    attendTime = null,
                    isManuallyCheckOut = true,
                    firmId = request.firmId,
                    AttendMode = "MisPunch",
                    StaffId = request.staffId,
                };
                db.AttendanceRequests.AddOrUpdate(design);
                db.AttendanceStaffs.Add(attetend);
                db.SaveChanges();

                string inoutmode;
                if (request.inOutMode == 0)
                {
                    inoutmode = "Check-In";
                }
                else
                {
                    inoutmode = "Check-Out";
                }


                var staffEmail = db.Staffs.Where(s => s.staffId == request.staffId).FirstOrDefault().email;
                var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == EnrollNumber.firmId);

                string mailSendTo = staffEmail;
                string subject1 = "Your manually attendance request status";

                string body1 =
                    "Your manually attendance request for : " + inoutmode + " , date is: " + Convert.ToDateTime(request.attendDate).ToString("dd MMM yyyy HH:mm:ss") + " approved successfully";
                var services = new MailServiceConfig();
                var flag = services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName, dbs.SMTPPortNo);

                ViewData["ErrorMsg"] = " Manually attendance request approved successfully!";
                return View();
            }
            else if (db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId && a.isApproved == false) != null)
            {
                ViewData["ErrorMsg"] = "This manually attendance request already rejected!";
                return View();
            }
            else if (db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId && a.isApproved == true) != null)
            {
                ViewData["ErrorMsg"] = "Manually attendance request already approved !!!";
                return View();
            }
            ViewData["ErrorMsg"] = "Invalid attendance request !!!";
            return View();
        }

        public ActionResult ManuallyAttendanceRequestRejected(int? requestId)
        {
            var request = db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId && a.isApproved == null);

            if (request != null)
            {
                var design = new AttendanceRequest
                {
                    transId = request.transId,
                    transDate = request.transDate,
                    attendDate = request.attendDate,
                    inOutMode = request.inOutMode,
                    narration = request.narration,
                    staffId = request.staffId,
                    isApproved = false,
                    firmId = request.firmId,
                };
                db.AttendanceRequests.AddOrUpdate(design);
                db.SaveChanges();

                string inoutmode;
                if (request.inOutMode == 0)
                {
                    inoutmode = "Check-In";
                }
                else
                {
                    inoutmode = "Check-Out";
                }

                var staffEmail = db.Staffs.Where(s => s.staffId == request.staffId).FirstOrDefault().email;
                var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == request.firmId);
                string mailSendTo = staffEmail;
                string subject1 = "Your manually attendance request status";
                string body1 = "Your manually attendance request for : " + inoutmode + " , date is: " + Convert.ToDateTime(request.attendDate).ToString("dd MMM yyyy HH:mm:ss") + " rejected.";

                var services = new MailServiceConfig();
                var flag = services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName, dbs.SMTPPortNo);

                ViewData["ErrorReject"] = "Manually attendance request rejected !!!";
                return View();
            }
            else if (db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId && a.isApproved == true) != null)
            {
                var request1 = db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId);
                AttendanceRequest asr = db.AttendanceRequests.Find(requestId);
                {
                    asr.transId = request1.transId;
                    asr.transDate = request1.transDate;
                    asr.attendDate = request1.attendDate;
                    asr.inOutMode = request1.inOutMode;
                    asr.narration = request1.narration;
                    asr.staffId = request1.staffId;
                    asr.isApproved = false;
                    asr.firmId = request1.firmId;
                };
                db.AttendanceRequests.AddOrUpdate(asr);

                var attendidPk = db.AttendanceStaffs.FirstOrDefault(s => s.StaffId == request1.staffId && s.attendDate == request1.attendDate).attendId;
                AttendanceStaff ast = db.AttendanceStaffs.Find(attendidPk);
                db.AttendanceStaffs.Remove(ast);
                db.SaveChanges();

                string inoutmode;
                if (request1.inOutMode == 0)
                {
                    inoutmode = "Check-In";
                }
                else
                {
                    inoutmode = "Check-Out";
                }

                var staffEmail = db.Staffs.Where(s => s.staffId == request1.staffId).FirstOrDefault().email;
                var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == request1.firmId);
                string mailSendTo = staffEmail;
                string subject1 = "Your manually attendance request status";
                string body1 = "Your manually attendance request for : " + inoutmode + " , date is: " + Convert.ToDateTime(request1.attendDate).ToString("dd MMM yyyy HH:mm:ss") + " rejected.";

                var services = new MailServiceConfig();
                var flag = services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName, dbs.SMTPPortNo);

                ViewData["ErrorReject"] = "Manually attendance request rejected !!!";
                return View();
            }
            else if (db.AttendanceRequests.FirstOrDefault(a => a.transId == requestId && a.isApproved == false) != null)
            {
                ViewData["ErrorReject"] = "Manually attendance request already rejected!";
                return View();
            }
            ViewData["ErrorMsg"] = "Invalid attendance request !!!";

            return View();
        }
        #endregion 

        #region ---------- AttendanceRequestList ---------------
        public ActionResult AttendRequestList()
        {
            var firm = LoginUserFirmId();
            var staffid = LoginEmployeeId();
            var staffattendance = db.AttendanceRequests.Include(s => s.Staff).Where(a => a.staffId == staffid & a.firmId == firm).OrderByDescending(w => w.transDate);
            return View(staffattendance.ToList());
        }

        public ActionResult AttendApproveOrRejectList(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            var firm = LoginUserFirmId();
            //var firmId = LoginUserFirmId();
            //var firmid = LoginUserFirmId();
            ViewBag.staffid =
              db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });
            if (empId == null && datepicker != null && datepicker2 != null)
            {
                var staffattendanceNew =
                      db.AttendanceRequests.Include(s => s.Staff)
                        .Where(a => a.firmId == firm && a.transDate >= datepicker && a.transDate <= datepicker2 && a.Staff.isActive == true).OrderByDescending(s => s.transDate);
                return View(staffattendanceNew.ToList());
            }
            else if (empId == 0 && datepicker != null && datepicker2 != null)
            {
                var staffattendance =
                    db.AttendanceRequests.Include(s => s.Staff)
                        .Where(a => a.firmId == firm && a.transDate >= datepicker && a.transDate <= datepicker2 && a.Staff.isActive == true)
                        .OrderByDescending(w => w.transDate);
                return View(staffattendance.ToList());
            }
            else if (empId != null && datepicker != null && datepicker2 != null)
            {
                var staffattendance =
                  db.AttendanceRequests.Include(s => s.Staff)
                      .Where(a => a.firmId == firm && a.staffId == empId && a.transDate >= datepicker && a.transDate <= datepicker2 && a.Staff.isActive == true)
                      .OrderByDescending(w => w.transDate);
                return View(staffattendance.ToList());
            }
            else
            {
                var staffattendance =
                    db.AttendanceRequests.Include(s => s.Staff)
                        .Where(a => a.firmId == firm && a.Staff.isActive == true).OrderByDescending(w => w.transDate);
                return View(staffattendance.ToList());
            }
        }

        #endregion 

        #region ------- EditAttendanceRequest ------------------
        public ActionResult EditAttendRequest(int? id, bool? status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceRequest request = db.AttendanceRequests.Find(id);

            if (status == true)
            {
                TempData["Approve"] = "Approve";
            }
            else
            {
                TempData["Approve"] = "Reject";
            }
            if (request == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttendRequest([Bind(Include = "transId,transDate,attendDate,inOutMode,narration,staffId,firmId")] AttendanceRequest request, bool? status, string actionName)
        {
            if (ModelState.IsValid)
            {
                string enrollno = null;
                var EnrollNumber = db.Staffs.FirstOrDefault(a => a.staffId == request.staffId);
                if (EnrollNumber != null)
                {
                    enrollno = Convert.ToString(EnrollNumber.enrollNumber);
                }

                if (status == true)
                {
                    var design = new AttendanceRequest
                    {
                        transId = request.transId,
                        transDate = request.transDate,
                        attendDate = request.attendDate,
                        inOutMode = request.inOutMode,
                        narration = request.narration,
                        staffId = request.staffId,
                        isApproved = true,
                        firmId = request.firmId,
                    };

                    var attetend = new AttendanceStaff()
                    {
                        enrollNumber = enrollno,
                        attendDate = request.attendDate,
                        verifyMode = 0,
                        inOutMode = request.inOutMode,
                        worCode = 0,
                        attendTime = null,
                        isManuallyCheckOut = true,
                        firmId = request.firmId,
                        AttendMode = "MisPunch",
                        StaffId = request.staffId
                    };

                    db.AttendanceRequests.AddOrUpdate(design);
                    db.AttendanceStaffs.Add(attetend);
                    db.SaveChanges();
                    TempData["notice"] = "approved";
                    return RedirectToAction(actionName);
                }
                else
                {
                    var design1 = new AttendanceRequest
                    {
                        transId = request.transId,
                        staffId = request.staffId,
                        transDate = request.transDate,
                        attendDate = request.attendDate,
                        inOutMode = request.inOutMode,
                        narration = request.narration,
                        isApproved = false,
                        firmId = request.firmId,

                    };
                    db.AttendanceRequests.AddOrUpdate(design1);
                    db.SaveChanges();
                    TempData["notice"] = "reject";
                    return RedirectToAction(actionName);
                }
            }

            return View(request);

        }
        #endregion 

        #region --------- Self Attendance Request Create -------
        public ActionResult AttendanceRequestSelf()
        {
            //DateTime timeUtc = DateTime.UtcNow;
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            //var abc = Convert.ToString(localZone.StandardName);
            //var timezone = TimeZoneInfo.FindSystemTimeZoneById(abc);
            //DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timezone);
            //var dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);
            var loginuserid = LoginEmployeeId();

            var data = db.EditPermissions.FirstOrDefault(a => a.StaffId == loginuserid);
            if (data != null)
            {
                if (data.ManualAttendance == true)
                {
                    var attends = new AttendanceRequestModel();
                    {
                        int staffId = LoginEmployeeId();
                        attends.StaffId = staffId;
                    }
                    return View(attends);
                }
                return RedirectToAction("AccessDenied", "Home");
            }
            return RedirectToAction("AccessDenied", "Home");
        }

        [HttpPost]
        public ActionResult AttendanceRequestSelf(AttendanceRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var staffid = LoginEmployeeId();
                var firm = LoginUserFirmId();
                var attetend = new AttendanceStaff();
                attetend.enrollNumber = "";
                var date = Convert.ToDateTime(request.AttendanceDate);
                attetend.attendDate = new DateTime(date.Year, date.Month, date.Day, request.Hour, request.Minute, 00);
                attetend.verifyMode = 0;
                attetend.inOutMode = request.InOutMode;
                attetend.worCode = 0;
                attetend.attendTime = null;
                attetend.isManuallyCheckOut = true;
                attetend.firmId = firm;
                attetend.AttendMode = "HRorSelf";
                attetend.StaffId = staffid;

                db.AttendanceStaffs.Add(attetend);
                TempData["notice"] = "success";
                db.SaveChanges();
                return RedirectToAction("AttendRequestselfList", "AttendanceStaff");
            }
            return View(request);
        }
        #endregion 

        #region ---------- Self Attendance Request List --------
        public ActionResult AttendRequestselfList()
        {
            var firm = LoginUserFirmId();
            var staffid = LoginEmployeeId();
            var data = from a in db.AttendanceStaffs.Where(s => s.firmId == firm)
                       join b in db.Staffs on a.StaffId equals b.staffId
                       where a.AttendMode == "HRorSelf" && a.StaffId == staffid && b.isActive == true
                       select new { a.StaffId, a.attendTime, a.firmId, b.schoolCode, b.firstName, b.middleName, b.lastName, a.isManuallyCheckOut, a.inOutMode, a.attendId, a.verifyMode, a.attendDate };

            var at = data.Select(a => new AttendanceRequestModel() { Schoolcode = a.schoolCode, StaffName = a.firstName + " " + a.middleName + " " + a.lastName, AttendanceDate = a.attendDate, InOutMode = a.inOutMode, AttendId = a.attendId }).ToList();
            return View(at.ToList());
        }
        #endregion 

        #region -------- Manager or HR Self Attendance List ----
        public ActionResult AttendRequestAdminselfList()
        {
            var firm = LoginUserFirmId();

            var data = from a in db.AttendanceStaffs.Where(s => s.firmId == firm)
                       join b in db.Staffs.Where(s => s.isActive == true) on a.StaffId equals b.staffId
                       where a.AttendMode == "HRorSelf" || a.AttendMode == "SendFromHR"
                       select new
                       {
                           a.StaffId,
                           a.attendTime,
                           a.firmId,
                           b.schoolCode,
                           b.firstName,
                           b.middleName,
                           b.lastName,
                           a.isManuallyCheckOut,
                           a.inOutMode,
                           a.attendId,
                           a.verifyMode,
                           a.attendDate
                       };

            var at = data.Select(a => new AttendanceRequestModel()
            {
                Schoolcode = a.schoolCode,
                StaffName = a.firstName + " " + a.middleName + " " + a.lastName,
                AttendanceDate = a.attendDate,
                InOutMode = a.inOutMode,
                AttendId = a.attendId
            }).ToList();
            return View(at.ToList());
        }
        #endregion 

        #region ------ Manager or HR Self Attendance Edit ------
        public ActionResult AttendRequestAdminselfEdit(int id)
        {
            var data = db.AttendanceStaffs.FirstOrDefault(a => a.attendId == id);
            AttendanceRequestModel attRequestModel = new AttendanceRequestModel();
            if (data != null)
            {
                var date = Convert.ToDateTime(data.attendDate);
                attRequestModel.AttendanceDate = data.attendDate;
                attRequestModel.AttendId = data.attendId;
                attRequestModel.Hour = date.Hour;
                attRequestModel.Minute = date.Minute;
                attRequestModel.InOutMode = data.inOutMode;
                attRequestModel.StaffId = data.StaffId;
            }
            return View(attRequestModel);
        }
        #endregion 

        #region --- Manager or HR Self Attendance Edit Post ----
        [HttpPost]
        public ActionResult AttendRequestAdminselfEdit(AttendanceRequestModel request)
        {
            var data = db.AttendanceStaffs.FirstOrDefault(a => a.attendId == request.AttendId);

            var firm = LoginUserFirmId();
            if (data != null)
            {
                data.enrollNumber = "";
                var date = Convert.ToDateTime(request.AttendanceDate);
                data.attendDate = new DateTime(date.Year, date.Month, date.Day, request.Hour, request.Minute, 00);
                data.verifyMode = 0;
                data.inOutMode = request.InOutMode;
                data.worCode = 0;
                data.attendTime = null;
                data.isManuallyCheckOut = true;
                data.firmId = firm;
                data.AttendMode = "HRorSelf";
                data.StaffId = request.StaffId;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("AttendRequestAdminselfList", "AttendanceStaff");
            }
            return RedirectToAction("AttendRequestAdminselfList", "AttendanceStaff");
        }
        #endregion

        #region ------- Delete Attendance Manual Request -------
        [HttpGet]
        public ActionResult AttendanceDelete(int id)
        {
            AttendanceRequestModel att = new AttendanceRequestModel();
            att.AttendId = id;
            return View(att);
        }
        [HttpPost]
        public ActionResult AttendanceDeleteConfirm(AttendanceRequestModel attendanceRequest)
        {
            AttendanceStaff attendancestaff = db.AttendanceStaffs.Find(attendanceRequest.AttendId);
            db.AttendanceStaffs.Remove(attendancestaff);
            db.SaveChanges();
            TempData["Notice"] = "delete";
            return RedirectToAction("AttendRequestAdminselfList");
        }
        #endregion

        #region ------ Self Attendance Request Create ----------
        public ActionResult AttendanceRequestSendHR()
        {
            var firm = LoginUserFirmId();
            AttendanceRequestModel mmodel = new AttendanceRequestModel();

            mmodel.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });

            return View(mmodel);
        }

        [HttpPost]
        public ActionResult AttendanceRequestSendHR(AttendanceRequestModel request)
        {
            if (ModelState.IsValid)
            {
                //  var staffid = LoginEmployeeId();
                var firm = LoginUserFirmId();
                var attetend = new AttendanceStaff();

                attetend.enrollNumber = "";
                var date = Convert.ToDateTime(request.AttendanceDate);
                attetend.attendDate = new DateTime(date.Year, date.Month, date.Day, request.Hour, request.Minute, 00);
                attetend.verifyMode = 0;
                attetend.inOutMode = request.InOutMode;
                attetend.worCode = 0;
                attetend.attendTime = null;
                attetend.isManuallyCheckOut = true;
                attetend.firmId = firm;
                attetend.AttendMode = "SendFromHR";
                attetend.StaffId = request.StaffId;


                db.AttendanceStaffs.Add(attetend);
                TempData["notice"] = "success";
                db.SaveChanges();
                return RedirectToAction("AttendRequestAdminselfList", "AttendanceStaff");
            }
            return View(request);
        }

        #endregion

        #region ----------- EditAttendRequestMember ------------
        [Authorize]
        public ActionResult EditAttendRequestMember(int? id, string actionName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceRequest data = db.AttendanceRequests.Find(id);

            AttendanceRequestEditModel attRequestModel = new AttendanceRequestEditModel();
            if (data != null)
            {
                DateTime date = (DateTime)data.attendDate;
                attRequestModel.HiddenAttendDate = data.attendDate;
                attRequestModel.AttendDate = (DateTime)data.attendDate;
                attRequestModel.Hour = date.Hour;
                attRequestModel.Minute = date.Minute;
                attRequestModel.ActionNameBack = actionName;
                attRequestModel.TransId = data.transId;
                attRequestModel.InOutMode = data.inOutMode;
                attRequestModel.Narration = data.narration;
                attRequestModel.firmid = data.firmId;
                attRequestModel.StaffId = data.staffId;
                // attRequestModel.TransDate = DateTime.UtcNow;
            }
            return View(attRequestModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditAttendRequestMember(AttendanceRequestEditModel request, bool? status, string actionName)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                if (status == true)
                {
                    string enrollno = null;
                    var EnrollNumber = db.Staffs.FirstOrDefault(a => a.staffId == request.StaffId);
                    if (EnrollNumber != null)
                    {
                        enrollno = Convert.ToString(EnrollNumber.enrollNumber);
                    }
                    var dates = request.AttendDate.Date;

                    TimeSpan time = new TimeSpan(0, request.Hour, request.Minute, 0);
                    var combined = dates.Add(time);
                    var timeString = combined.ToString("yyyy-M-d HH:mm:ss");
                    var date = DateTime.Now;
                    AttendanceRequest atre = db.AttendanceRequests.Find(request.TransId);
                    {
                        atre.transId = request.TransId;
                        atre.transDate = DateTime.UtcNow;
                        atre.attendDate = Convert.ToDateTime(timeString);
                        atre.inOutMode = request.InOutMode;
                        atre.narration = request.Narration;
                        atre.staffId = request.StaffId;
                        atre.firmId = request.firmid;
                    };

                    var dtid = db.AttendanceStaffs.FirstOrDefault(s => s.StaffId == request.StaffId && s.attendDate == request.HiddenAttendDate);
                    if (dtid != null)
                    {
                        var dates1 = request.AttendDate.Date;
                        TimeSpan time1 = new TimeSpan(0, request.Hour, request.Minute, 0);
                        var combined1 = dates1.Add(time1);
                        var timeString1 = combined1.ToString("yyyy-M-d HH:mm:ss");

                        AttendanceStaff atstaff = db.AttendanceStaffs.Find(dtid.attendId);
                        {
                            atstaff.StaffId = request.StaffId;
                            atstaff.enrollNumber = enrollno;
                            atstaff.attendDate = Convert.ToDateTime(timeString1);
                            atstaff.verifyMode = 0;
                            atstaff.inOutMode = request.InOutMode;
                            atstaff.worCode = 0;
                            atstaff.attendTime = null;
                            atstaff.isManuallyCheckOut = true;
                            atstaff.firmId = request.firmid;
                            atstaff.AttendMode = "MisPunch";
                        }
                        db.AttendanceStaffs.AddOrUpdate(atstaff);
                    }
                    db.AttendanceRequests.AddOrUpdate(atre);
                    TempData["notice"] = "Attendance request updated successfully";
                    db.SaveChanges();
                }
                else
                {
                    var dates = request.AttendDate.Date;
                    TimeSpan time = new TimeSpan(0, request.Hour, request.Minute, 0);
                    var combined = dates.Add(time);
                    var timeString = combined.ToString("yyyy-M-d HH:mm:ss");
                    var date = DateTime.Now;
                    AttendanceRequest atstaff = db.AttendanceRequests.Find(request.TransId);
                    {
                        atstaff.transId = request.TransId;
                        atstaff.transDate = date;
                        atstaff.attendDate = Convert.ToDateTime(timeString);
                        atstaff.inOutMode = request.InOutMode;
                        atstaff.narration = request.Narration;
                        atstaff.staffId = request.StaffId;
                        atstaff.firmId = firm;
                        atstaff.transDate = DateTime.UtcNow;
                    };
                    db.AttendanceRequests.AddOrUpdate(atstaff);
                    db.SaveChanges();
                }
                TempData["notice"] = "Attendance request updated successfully";
                return RedirectToAction(actionName);
            }

            return View(request);
        }
        #endregion 

        #region ------------ DeleteAttendRequest ---------------
        [Authorize]
        public ActionResult DeleteAttendRequest(int? id, string actionName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceRequest ar = db.AttendanceRequests.Find(id);

            TempData["actionanme"] = actionName;
            if (ar == null)
            {
                return HttpNotFound();
            }
            return View(ar);
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteAttendRequest(int id, DateTime? date, string actionName)
        {
            AttendanceRequest ar = db.AttendanceRequests.Find(id);
            if (ar.isApproved != null && ar.isApproved != false)
            {
                var attendidPk =
                    db.AttendanceStaffs.Where(s => s.StaffId == ar.staffId && s.attendDate == date).FirstOrDefault().attendId;
                AttendanceStaff ast = db.AttendanceStaffs.Find(attendidPk);
                db.AttendanceStaffs.Remove(ast);
            }
            db.AttendanceRequests.Remove(ar);
            db.SaveChanges();
            TempData["notice"] = "Attendance request deleted successfully.";
            return RedirectToAction(actionName);
        }

        #endregion 

        #region ------------- AppOrRejectMultiple --------------
        [Authorize]
        public ActionResult AppOrRejectMultiple(string[] customerIDs)
        {
            if (customerIDs == null)
            {
                return Json("Please select at least one checkbox.");
            }
            else
            {
                var firm = LoginUserFirmId();
                int count = 0;

                foreach (string customerID in customerIDs)
                {
                    int traid = Int32.Parse(customerID);
                    var request = db.AttendanceRequests.Where(s => s.transId == traid).FirstOrDefault();
                    string enrollno = null;
                    var EnrollNumber = db.Staffs.FirstOrDefault(a => a.staffId == request.staffId);
                    if (EnrollNumber != null)
                    {
                        enrollno = Convert.ToString(EnrollNumber.enrollNumber);
                    }
                    var design = new AttendanceRequest
                    {
                        transId = request.transId,
                        transDate = request.transDate,
                        attendDate = request.attendDate,
                        inOutMode = request.inOutMode,
                        narration = request.narration,
                        staffId = request.staffId,
                        isApproved = true,
                        firmId = firm
                    };

                    var attetend = new AttendanceStaff()
                    {
                        StaffId = request.staffId,
                        enrollNumber = "",
                        attendDate = request.attendDate,
                        verifyMode = 0,
                        inOutMode = request.inOutMode,
                        worCode = 0,
                        attendTime = null,
                        AttendMode = "MisPunch",
                        isManuallyCheckOut = true,
                        firmId = firm
                    };
                    db.AttendanceRequests.AddOrUpdate(design);
                    db.AttendanceStaffs.Add(attetend);


                    string inoutmode;
                    if (request.inOutMode == 0)
                    {
                        inoutmode = "Check-In";
                    }
                    else
                    {
                        inoutmode = "Check-Out";
                    }

                    var staffEmail = db.Staffs.Where(s => s.staffId == request.staffId).FirstOrDefault().email;
                    string mailSendTo = staffEmail;
                    string subject1 = "Your manually attendance request status";

                    string body1 =
                        "Your manually attendance request for : " + inoutmode + " , date is: " + request.attendDate + " approved successfully";
                    var services = new MailService();
                    services.SendMail(mailSendTo, body1, subject1, "innovativefusiontest@gmail.com");
                    count++;

                }
                db.SaveChanges();
                return Json("Total :" + count + "Attendance request approved successfully!");
            }
        }
        #endregion 

        #region ------------- OnlyRejectMultiple ---------------
        [Authorize]
        public ActionResult OnlyRejectMultiple(string[] customerIDs)
        {
            if (customerIDs == null)
            {
                return Json("Please select at least one checkbox.");
            }
            else
            {
                int count = 0;
                foreach (string customerID in customerIDs)
                {
                    var firm = LoginUserFirmId();
                    int traid = Int32.Parse(customerID);
                    var request = db.AttendanceRequests.Where(s => s.transId == traid).FirstOrDefault();
                    var design1 = new AttendanceRequest
                    {
                        transId = request.transId,
                        staffId = request.staffId,
                        transDate = request.transDate,
                        attendDate = request.attendDate,
                        inOutMode = request.inOutMode,
                        narration = request.narration,
                        isApproved = false,
                        firmId = firm

                    };
                    db.AttendanceRequests.AddOrUpdate(design1);

                    count++;
                    string inoutmode;
                    if (request.inOutMode == 0)
                    {
                        inoutmode = "Check-In";
                    }
                    else
                    {
                        inoutmode = "Check-Out";
                    }

                    var staffEmail = db.Staffs.Where(s => s.staffId == request.staffId).FirstOrDefault().email;
                    string mailSendTo = staffEmail;
                    string subject1 = "Your manually attendance request status";

                    string body1 =
                        "Your manually attendance request for : " + inoutmode + " , date is: " + request.attendDate + " rejected !!!";
                    var services = new MailService();
                    services.SendMail(mailSendTo, body1, subject1, "innovativefusiontest@gmail.com");
                }
                db.SaveChanges();
                return Json("Total :" + count + "Attendance request rejected successfully!");
            }
        }
        #endregion 

        #region ------- AttendApproveOrRejectListManager -------
        [Authorize]
        public ActionResult AttendApproveOrRejectListManager(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            var loginId = LoginEmployeeId();
            //   var firm = LoginUserFirmId();
            var firm = LoginUserFirmId();
            ViewBag.staffid =
              db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firm).Where(a => a.ReportingManagerId == loginId).Select(a => new SelectListItem()
              {
                  Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                  Value = a.Staff.staffId.ToString()
              });
            if (datepicker == null && datepicker2 == null)
            {
                var data = db.AttendanceRequestListForMgr(loginId, firm, 0, null, null);
                var lrm = data.Select(a => new AttendanceRequestModel()
                {
                    StaffId = a.staffId,
                    Schoolcode = a.schoolCode,
                    Fullname = a.firstName + " " + a.middleName + " " + a.lastName,
                    TransId = a.transId,
                    TransDate = a.transDate,
                    AttendDate = (DateTime)a.attendDate,
                    InOutMode = a.inOutMode,
                    IsApproved = a.isApproved,
                }).ToList().OrderByDescending(s => s.TransDate);
                return View(lrm);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;

                var data = db.AttendanceRequestListForMgr(loginId, firm, empId, start, end);
                var lrm = data.Select(a => new AttendanceRequestModel()
                {
                    StaffId = a.staffId,
                    Schoolcode = a.schoolCode,
                    Fullname = a.firstName + " " + a.middleName + " " + a.lastName,
                    TransId = a.transId,
                    TransDate = a.transDate,
                    AttendDate = (DateTime)a.attendDate,
                    InOutMode = a.inOutMode,
                    IsApproved = a.isApproved,
                }).ToList().OrderByDescending(s => s.TransDate);
                return View(lrm);
            }

        }
        #endregion
    }
}


