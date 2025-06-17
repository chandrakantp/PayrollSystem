using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Services;
using edueTree.Models;
using edueTree.Services;
using Microsoft.AspNet.Identity;

namespace edueTree.Controllers.Payroll
{

    public class LeaveRequestController : BaseController
    {
        #region ------------- Db-context -----------
        private dbContainer db = new dbContainer();
        #endregion

        #region ------- GetStaffUnderMgr -----------

        [HttpPost]
        public ActionResult GetStaffUnderMgr()
        {
            List<int> subcatId = new List<int>();
            List<string> subcat = new List<string>();
            var loginEmployee = LoginEmployeeId();

            var report = db.Reportings.Where(s => s.ReportingManagerId == loginEmployee).FirstOrDefault();

            var mgremai = db.Staffs.Where(s => s.staffId == loginEmployee).FirstOrDefault().email;
            Boolean flag = false;
            var firm = LoginUserFirmId();
            subcat.Add("Select Employee");
            subcatId.Add(0);
            foreach (var ex in db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true).Where(a => a.ReportingManagerId == loginEmployee))
            {
                subcat.Add(ex.Staff.schoolCode + ": " + ex.Staff.firstName + " " + ex.Staff.middleName + " " +
                           ex.Staff.lastName);
                subcatId.Add(ex.Staff.staffId);
            }

            //foreach (var VARIABLE in db.Staffs.Where(a => a.firmId == firm && a.isActive == true && a.staffId==report.StaffId))
            //{
            //    subcat.Add(VARIABLE.schoolCode + ": " + VARIABLE.firstName + " " + VARIABLE.middleName + " " +
            //               VARIABLE.lastName);
            //    subcatId.Add(VARIABLE.staffId);
            //}
            return Json(new { Subcat = subcat, SubcatId = subcatId });           
        }

#endregion

        #region -------------- Index ---------------

        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            int staffId = LoginEmployeeId();
            var leaverequests =
                db.LeaveRequests.Include(l => l.AspNetUser).Include(l => l.LeaveType).Include(l => l.Staff);
            return View(leaverequests.Where(a => a.staffId == staffId && a.firmId == firm).ToList());
        }

        #endregion

        #region ------- Leave request --------------
        public ActionResult LeaveRequestList(int? empId, DateTime? datefrom, DateTime? dateto)
        {
            return View("LeaveRequestList", new { empId = empId, datefrom = datefrom, dateto = dateto });
        }

        public ActionResult LeaveRequestListForSeniours(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            int firm = LoginUserFirmId();
          
            ViewBag.staffid =
              db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });

            if (empId == 0)
                empId = null;
            var monthFirstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthLastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            if (empId == null && datepicker != null && datepicker2 != null)
            {
                var leaverequestsd =
                    db.LeaveRequests.Include(l => l.AspNetUser)
                        .Include(l => l.LeaveType)
                        .Include(l => l.Staff).Where(s => s.Staff.isActive == true)
                        .Where(a => a.firmId == firm && (a.dateFrom >= datepicker && a.dateFrom <= datepicker2) || (a.dateTo >= datepicker && a.dateFrom <= datepicker2 && a.firmId == firm)).OrderByDescending(s => s.createdDate);
                return View(leaverequestsd.ToList());
            }
            else if (empId != null && datepicker != null && datepicker2 != null)
            {
                var leaverequestsd =
                    db.LeaveRequests.Include(l => l.AspNetUser)
                        .Include(l => l.LeaveType)
                        .Include(l => l.Staff).Where(s => s.Staff.isActive == true)
                        .Where(
                            a =>
                                a.staffId == empId && a.firmId == firm &&
                                ((a.dateFrom >= datepicker && a.dateFrom <= datepicker2) ||
                                 (a.dateTo >= datepicker && a.dateFrom <= datepicker2 && a.firmId == firm))).OrderByDescending(s => s.createdDate);
                return View(leaverequestsd.ToList());
            }
            else if (empId != null && datepicker == null && datepicker2 == null)
            {
                var leaverequestsd =
                    db.LeaveRequests.Where(
                        a =>
                            a.staffId == empId && a.firmId == firm).OrderByDescending(s => s.createdDate);
                return View(leaverequestsd.ToList());
            }
            else
            {
                var leaverequests =
                db.LeaveRequests.Include(l => l.AspNetUser)
                    .Include(l => l.LeaveType)
                    .Include(l => l.Staff)
                    .Where(a => a.firmId == firm && a.Staff.isActive == true).OrderByDescending(s => s.createdDate);
                return View(leaverequests.ToList());
            }
        }
        #endregion

        #region ------ LeavesMasterManager ---------
        public ActionResult LeavesMasterManager(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            var loginId = LoginEmployeeId();
            var firm = LoginUserFirmId();

              ViewBag.staffid =
              db.Reportings.Include(s => s.Staff).Where(s => s.Staff.isActive == true && s.firmid == firm).Where(a => a.ReportingManagerId == loginId).Select(a => new SelectListItem()
              {
                  Text = a.Staff.schoolCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                  Value = a.Staff.staffId.ToString()
              });

            if (datepicker == null && datepicker2 == null)
            {
                var data = db.LeaverequestListForMgr(loginId, firm, 0, null, null);
                var lrm = data.OrderByDescending(s => s.createdDate).Select(a => new LeaveRequestModel()
                {
                    staffId = a.staffId,
                    Staffcode = a.schoolCode,
                    Fullname = a.fullname,
                    dateFrom = a.dateFrom,
                    sesionDateFrom = a.sesionDateFrom,
                    dateTo = a.dateTo,
                    sesionDateTo = a.sesionDateTo,
                    LeaveTyp = a.LeaveType,
                    createdDate = a.createdDate,
                    totalCounts = a.totalCounts,
                    status = a.status,
                    tranId = a.tranId
                }).ToList();
                return View(lrm);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;

                var data = db.LeaverequestListForMgr(loginId, firm, empId, start, end);
                var lrm = data.OrderByDescending(s => s.createdDate).Select(a => new LeaveRequestModel()
                {
                    staffId = a.staffId,
                    Staffcode = a.schoolCode,
                    Fullname = a.fullname,
                    dateFrom = a.dateFrom,
                    sesionDateFrom = a.sesionDateFrom,
                    dateTo = a.dateTo,
                    sesionDateTo = a.sesionDateTo,
                    LeaveTyp = a.LeaveType,
                    createdDate = a.createdDate,
                    totalCounts = a.totalCounts,
                    status = a.status,
                    tranId = a.tranId
                }).ToList();
                return View(lrm);
            }
        }
       #endregion

        #region --------- LeaveMasterEdit ----------
        public ActionResult LeaveMasterEdit(int? id, string actionName)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }

            LeaveRequestModel model = new LeaveRequestModel();
            model.tranId = leaverequest.tranId;
            model.ActionName = actionName;
            model.approvalDate = leaverequest.approvalDate;
            model.approvedBY = leaverequest.approvedBY;
            model.createdDate = leaverequest.createdDate;
            model.staffId = leaverequest.staffId;
            model.dateFrom = leaverequest.dateFrom;
            model.sesionDateFrom = leaverequest.sesionDateFrom;
            model.sesionDateTo = leaverequest.sesionDateTo;
            model.dateTo = leaverequest.dateTo;
            model.rejoing = leaverequest.rejoing;
            model.status = leaverequest.status;
            model.Staff = leaverequest.Staff;
            model.firmId = leaverequest.firmId;
            model.totalCounts = leaverequest.totalCounts;
            model.lTypeId = leaverequest.lTypeId;

            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
            ViewBag.Leavetype = new SelectList(db.LeaveTypes.Where(a => a.firmId == leaverequest.firmId), "leaveTypeId", "LeaveType1", leaverequest.lTypeId);

            return View(model);
        }

        [HttpPost]
        public ActionResult LeaveMasterEdit(LeaveRequest request, string actionName)
        {
            if (ModelState.IsValid)
            {
                var leaveBL = db.GetLeaveBalanceFromNewTbl(request.staffId, request.lTypeId).FirstOrDefault();
                var finalBalance = leaveBL.TotalCount;

                var ltypeIdbalId =
                          db.LeaveTypes.FirstOrDefault(
                              s =>
                                  s.LeaveType1 == "Informed Absent" && s.firmId == request.firmId &&
                                  s.leaveTypeId == request.lTypeId);

                if (ltypeIdbalId != null)
                {
                    var trecordBalCount =
                        db.LeaveRecordNews.FirstOrDefault(
                            s => s.LevetypeIds == ltypeIdbalId.leaveTypeId && s.staffids == request.staffId)
                            .totalLeaves;
                    if (trecordBalCount <= 0)
                    {
                        LeaveRequest lv = db.LeaveRequests.Find(request.tranId);
                        {
                            lv.tranId = request.tranId;
                            lv.staffId = request.staffId;
                            lv.lTypeId = request.lTypeId;
                            lv.dateFrom = request.dateFrom;
                            lv.sesionDateFrom = request.sesionDateFrom;
                            lv.dateTo = request.dateTo;
                            lv.sesionDateTo = request.sesionDateTo;
                            lv.totalCounts = request.totalCounts;
                            lv.rejoing = request.rejoing;
                            lv.status = request.status;
                            lv.firmId = request.firmId;
                            lv.DeductStatus = true;
                            lv.createdDate = DateTime.UtcNow;
                        }
                        db.LeaveRequests.AddOrUpdate(lv);

                        var firstOrDefault =
                            db.LeavePassbooks.Where(s => s.LplrequestId == request.tranId).FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            var lpappbookid = firstOrDefault.LptranId;

                            LeavePassbook lp = db.LeavePassbooks.Find(lpappbookid);
                            {
                                lp.LpstaffId = request.staffId;
                                lp.LpdateFrom = request.dateFrom;
                                lp.LpsesionDateFrom = request.sesionDateFrom;
                                lp.LpdateTo = request.dateTo;
                                lp.LpsesionDateTo = request.sesionDateTo;
                                lp.totalLeavess = request.totalCounts;
                                lp.LpcreatedDate = DateTime.UtcNow;
                                lp.Lpstatus = request.status;
                                lp.LpfirmId = request.firmId;
                                lp.LplTypeId = request.lTypeId;
                                lp.LplrequestId = request.tranId;
                                lp.LpDeductStatus = true;
                                lp.LpfirmId = request.firmId;
                            }
                            db.LeavePassbooks.AddOrUpdate(lp);
                        }

                        db.SaveChanges();
                        TempData["notice"] = "update";
                        return RedirectToAction(actionName);
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (request.totalCounts > finalBalance)
                        {
                            TempData["notice"] = "notenough";
                            return RedirectToAction("Create");
                        }

                        try
                        {
                            LeaveRequest lv = db.LeaveRequests.Find(request.tranId);
                            {
                                lv.tranId = request.tranId;
                                lv.staffId = request.staffId;
                                lv.lTypeId = request.lTypeId;
                                lv.dateFrom = request.dateFrom;
                                lv.sesionDateFrom = request.sesionDateFrom;
                                lv.dateTo = request.dateTo;
                                lv.sesionDateTo = request.sesionDateTo;
                                lv.totalCounts = request.totalCounts;
                                lv.rejoing = request.rejoing;
                                lv.status = request.status;
                                lv.firmId = request.firmId;
                                lv.DeductStatus = true;
                                lv.createdDate = DateTime.UtcNow;
                            }
                            db.LeaveRequests.AddOrUpdate(lv);

                            var firstOrDefault =
                                db.LeavePassbooks.Where(s => s.LplrequestId == request.tranId).FirstOrDefault();
                            if (firstOrDefault != null)
                            {
                                var lpappbookid = firstOrDefault.LptranId;

                                LeavePassbook lp = db.LeavePassbooks.Find(lpappbookid);
                                {
                                    lp.LpstaffId = request.staffId;
                                    lp.LpdateFrom = request.dateFrom;
                                    lp.LpsesionDateFrom = request.sesionDateFrom;
                                    lp.LpdateTo = request.dateTo;
                                    lp.LpsesionDateTo = request.sesionDateTo;
                                    lp.totalLeavess = request.totalCounts;
                                    lp.LpcreatedDate = DateTime.UtcNow;
                                    lp.Lpstatus = request.status;
                                    lp.LpfirmId = request.firmId;
                                    lp.LplTypeId = request.lTypeId;
                                    lp.LplrequestId = request.tranId;
                                    lp.LpDeductStatus = true;
                                    lp.LpfirmId = request.firmId;
                                }
                                db.LeavePassbooks.AddOrUpdate(lp);
                            }

                            db.SaveChanges();
                            TempData["notice"] = "update";
                            return RedirectToAction(actionName);
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
                            throw;
                        }                     
                    }
                }
            }
            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", request.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", request.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", request.staffId);
            return View(request);
        }

        #endregion

        #region ------------- Details --------------

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        #endregion

        #region -------------- Create --------------
        [HttpGet]
        public ActionResult Create()
        {
            string userName = User.Identity.GetUserName();
            var userExists = db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            var firm = LoginUserFirmId();
            int staffId = 0;


            Staff firstOrDefault = db.Staffs.FirstOrDefault(a => a.userId == userExists.Id);
            if (firstOrDefault != null)
            {
                staffId = firstOrDefault.staffId;
            }
            db.Staffs.Find(staffId);

            LeaveRequestModel model = new LeaveRequestModel();
            model.staffId = staffId;
            ViewBag.lTypeId = new SelectList(db.LeaveTypes.Where(a => a.firmId == firm), "leaveTypeId", "LeaveType1");
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(LeaveRequestModel leaverequest)
        {
            var firmstmember = LoginUserFirmId();
            var leavesame =
                db.LeaveRequests.FirstOrDefault(
                    s =>
                        s.staffId == leaverequest.staffId && s.lTypeId == leaverequest.lTypeId &&
                        s.dateFrom == leaverequest.dateFrom && s.dateTo == leaverequest.dateTo &&
                        s.totalCounts == leaverequest.totalCounts && s.firmId == firmstmember);
            var LeaveTtype =
                db.LeaveTypes.FirstOrDefault(s => s.leaveTypeId == leaverequest.lTypeId && s.firmId == firmstmember)
                    .LeaveType1;

            if (LeaveTtype != "Informed Absent")
            {
                var leaverequestBal =
                    db.LeaveRequests.Where(
                        s =>
                            s.staffId == leaverequest.staffId && s.lTypeId == leaverequest.lTypeId &&
                            s.firmId == firmstmember && (s.status == "Planned" || s.status == "Requested"))
                        .Sum(a => a.totalCounts);
                var leaveBLApp =
                    db.GetLeaveBalanceFromNewTbl(leaverequest.staffId, leaverequest.lTypeId).FirstOrDefault();
                var TotalLeaves = leaveBLApp.TotalCount - leaverequestBal;

                if (TotalLeaves <= 0)
                {
                    TotalLeaves = 0;
                }
                if (TotalLeaves < leaverequest.totalCounts)
                {
                    TempData["notice"] = "alreadyleavesend";
                    ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
                    ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", leaverequest.lTypeId);
                    ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
                    return View(leaverequest);
                }
            }

            if (leavesame == null)
            {

                var Leavetp = db.LeaveTypes.FirstOrDefault(s => s.leaveTypeId == leaverequest.lTypeId && s.firmId == firmstmember).LeaveType1;
                var dtdate = DateTime.Today;
                int staffIds = LoginEmployeeId();
                var dtst = db.Staffs.Where(a => a.staffId == staffIds).FirstOrDefault().probation;

                var edp = db.EditPermissions.Where(a => a.StaffId == staffIds).FirstOrDefault();
                if (edp != null)
                {
                    if (((edp.IsProbationLeaveApp == false && dtst != null) ||
                         (edp.IsProbationLeaveApp == null && dtst != null)))
                    {
                        if (dtdate > dtst)
                        {
                            var firm = LoginUserFirmId();
                            var leaveBL =
                                db.GetLeaveBalanceFromNewTbl(leaverequest.staffId, leaverequest.lTypeId)
                                    .FirstOrDefault();
                            var finalBalance = leaveBL.TotalCount;

                            var ltypeIdbalId =
                                db.LeaveTypes.FirstOrDefault(
                                    s =>
                                        s.LeaveType1 == "Informed Absent" && s.firmId == firm &&
                                        s.leaveTypeId == leaverequest.lTypeId);

                            if (ltypeIdbalId != null)
                            {
                                var trecordBalCount =
                                    db.LeaveRecordNews.FirstOrDefault(
                                        s =>
                                            s.LevetypeIds == ltypeIdbalId.leaveTypeId &&
                                            s.staffids == leaverequest.staffId).totalLeaves;
                                if (trecordBalCount <= 0)
                                {
                                    int staffId = LoginEmployeeId();
                                    var lr = new LeaveRequest
                                    {
                                        staffId = staffId,
                                        dateFrom = leaverequest.dateFrom,
                                        sesionDateFrom = leaverequest.sesionDateFrom,
                                        dateTo = leaverequest.dateTo,
                                        sesionDateTo = leaverequest.sesionDateTo,
                                        totalCounts = leaverequest.totalCounts,
                                        createdDate = DateTime.UtcNow,
                                        status = leaverequest.status,
                                        firmId = firm,
                                        lTypeId = leaverequest.lTypeId,
                                        DeductStatus = true
                                    };

                                    db.LeaveRequests.Add(lr);
                                    db.SaveChanges();

                                    var lrp = new LeavePassbook
                                    {
                                        LpstaffId = staffId,
                                        LpdateFrom = leaverequest.dateFrom,
                                        LpsesionDateFrom = leaverequest.sesionDateFrom,
                                        LpdateTo = leaverequest.dateTo,
                                        LpsesionDateTo = leaverequest.sesionDateTo,
                                        totalLeavess = leaverequest.totalCounts,
                                        LpcreatedDate = DateTime.UtcNow,
                                        Lpstatus = leaverequest.status,
                                        LpfirmId = firm,
                                        LplTypeId = leaverequest.lTypeId,
                                        LplrequestId = lr.tranId,
                                        LpDeductStatus = true
                                    };

                                    db.LeavePassbooks.Add(lrp);
                                    db.SaveChanges();
                                    TempData["notice"] = "success";

                                    var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                                    var leaveReq = db.LeaveRequests.FirstOrDefault(a => a.staffId == staffId);

                                    var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                                    var reporting = db.Staffs.FirstOrDefault(a => a.staffId == staffId);

                                    var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);

                                    string emailabc = "";
                                    foreach (var ex in reportingmails)
                                    {
                                        var mgeremail =
                                            db.Staffs.Where(s => s.staffId == ex.ReportingManagerId)
                                                .FirstOrDefault()
                                                .email;

                                        emailabc = emailabc + mgeremail + ",";
                                    }

                                    string s2 = emailabc;
                                    string[] words = s2.Split(',');
                                    foreach (string word in words)
                                    {
                                        var dbmail = word;
                                        string mailSendTo = reporting.emailReportingTo;
                                        var leaveReqID = lr.tranId;
                                        var lnkHref1 =
                                            "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                            Url.Action("ReplyApproval", "LeaveRequest",
                                                new
                                                {
                                                    pagId = 1,
                                                    empId = staffId,
                                                    tranId = leaveReqID,
                                                    status = true,
                                                    uReportingMail = mailSendTo,
                                                    countleave = leaverequest.totalCounts,
                                                    firmid = firm,
                                                }, "http") + "' > Approve </a> ";
                                        var rejectHref =
                                            "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                            Url.Action("ReplyApproval", "LeaveRequest",
                                                new
                                                {
                                                    pagId = 1,
                                                    empId = staffId,
                                                    tranId = leaveReqID,
                                                    status = false,
                                                    uReportingMail = mailSendTo,
                                                    countleave = leaverequest.totalCounts,
                                                    firmid = firm,
                                                }, "http") + "' > Reject </a> ";
                                        string subject1 = "Leave Request -" + userdetail.firstName + " " +
                                                          userdetail.lastName;
                                        string body1 =
                                            "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request For Leave - </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                                            userdetail.firstName + " " + userdetail.lastName + "<br> Leave Type :" + Leavetp + " <br>From Date: " +
                                            Convert.ToDateTime(leaverequest.dateFrom).ToString("dd-MMM-yyyy") +
                                            "<br>  To Date: " +
                                            Convert.ToDateTime(leaverequest.dateTo).ToString("dd-MMM-yyyy") +
                                            "<br> Total Days: " +
                                            leaverequest.totalCounts +
                                            "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" +
                                            lnkHref1 +
                                            "</td><td style='padding: 10px;'>" + rejectHref + "</td></tr></table>";
                                        var services = new MailServiceConfig();
                                        var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress,
                                            dbs.Password, dbs.HostName, dbs.SMTPPortNo);

                                    }
                                    return RedirectToAction("IndexLeavePassbook");
                                }
                            }
                            else
                            {
                                if (ModelState.IsValid)
                                {
                                    if (leaverequest.totalCounts > finalBalance)
                                    {
                                        TempData["notice"] = "notenough";
                                        return RedirectToAction("Create");
                                    }

                                    try
                                    {
                                        int staffId = LoginEmployeeId();
                                        var lr = new LeaveRequest
                                        {
                                            staffId = staffId,
                                            dateFrom = leaverequest.dateFrom,
                                            sesionDateFrom = leaverequest.sesionDateFrom,
                                            dateTo = leaverequest.dateTo,
                                            sesionDateTo = leaverequest.sesionDateTo,
                                            totalCounts = leaverequest.totalCounts,
                                            createdDate = DateTime.UtcNow,
                                            status = leaverequest.status,
                                            firmId = firm,
                                            lTypeId = leaverequest.lTypeId,
                                            DeductStatus = true
                                        };

                                        db.LeaveRequests.Add(lr);
                                        db.SaveChanges();

                                        var lrp = new LeavePassbook
                                        {
                                            LpstaffId = staffId,
                                            LpdateFrom = leaverequest.dateFrom,
                                            LpsesionDateFrom = leaverequest.sesionDateFrom,
                                            LpdateTo = leaverequest.dateTo,
                                            LpsesionDateTo = leaverequest.sesionDateTo,
                                            totalLeavess = leaverequest.totalCounts,
                                            LpcreatedDate = DateTime.UtcNow,
                                            Lpstatus = leaverequest.status,
                                            LpfirmId = firm,
                                            LplTypeId = leaverequest.lTypeId,
                                            LplrequestId = lr.tranId,
                                            LpDeductStatus = true
                                        };

                                        db.LeavePassbooks.Add(lrp);
                                        db.SaveChanges();

                                        TempData["notice"] = "success";


                                        var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                                        var leaveReq = db.LeaveRequests.FirstOrDefault(a => a.staffId == staffId);

                                        var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                                        var reporting = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                                        string mailSendTo = reporting.emailReportingTo;

                                        var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);

                                        string emailabc = "";
                                        foreach (var ex in reportingmails)
                                        {
                                            var mgeremail =
                                                db.Staffs.Where(s => s.staffId == ex.ReportingManagerId)
                                                    .FirstOrDefault()
                                                    .email;

                                            emailabc = emailabc + mgeremail + ",";
                                        }

                                        string s2 = emailabc;
                                        string[] words = s2.Split(',');
                                        foreach (string word in words)
                                        {
                                            var dbmail = word;

                                            var leaveReqID = lr.tranId;
                                            var lnkHref1 =
                                                "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                                Url.Action("ReplyApproval", "LeaveRequest",
                                                    new
                                                    {
                                                        pagId = 1,
                                                        empId = staffId,
                                                        tranId = leaveReqID,
                                                        status = true,
                                                        uReportingMail = mailSendTo,
                                                        countleave = leaverequest.totalCounts,
                                                        firmid = firm,
                                                    }, "http") + "' > Approve </a> ";
                                            var rejectHref =
                                                "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                                Url.Action("ReplyApproval", "LeaveRequest",
                                                    new
                                                    {
                                                        pagId = 1,
                                                        empId = staffId,
                                                        tranId = leaveReqID,
                                                        status = false,
                                                        uReportingMail = mailSendTo,
                                                        countleave = leaverequest.totalCounts,
                                                        firmid = firm,
                                                    }, "http") + "' > Reject </a> ";
                                            string subject1 = "Leave Request -" + userdetail.firstName + " " +
                                                              userdetail.lastName;
                                            string body1 =
                                                "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request For Leave - </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                                                userdetail.firstName + " " + userdetail.lastName + "<br> Leave Type :" + Leavetp +
                                                "<br> From Date: " +
                                                Convert.ToDateTime(leaverequest.dateFrom).ToString("dd-MMM-yyyy") +
                                                "<br>  To Date: " +
                                                Convert.ToDateTime(leaverequest.dateTo).ToString("dd-MMM-yyyy") +
                                                "<br> Total Days: " +
                                                leaverequest.totalCounts +
                                                "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" +
                                                lnkHref1 +
                                                "</td><td style='padding: 10px;'>" + rejectHref +
                                                "</td></tr></table>";
                                            var services = new MailServiceConfig();
                                            var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress,
                                                dbs.Password, dbs.HostName, dbs.SMTPPortNo);
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
                                        throw;
                                    }
                                    return RedirectToAction("IndexLeavePassbook");
                                }
                            }
                        }
                        else
                        {
                            TempData["notice"] = "probation";
                            return RedirectToAction("Create");
                        }
                    }
                    else
                    {
                        var firm = LoginUserFirmId();
                        var leaveBL =
                            db.GetLeaveBalanceFromNewTbl(leaverequest.staffId, leaverequest.lTypeId)
                                .FirstOrDefault();
                        var finalBalance = leaveBL.TotalCount;

                        var ltypeIdbalId =
                            db.LeaveTypes.FirstOrDefault(
                                s =>
                                    s.LeaveType1 == "Informed Absent" && s.firmId == firm &&
                                    s.leaveTypeId == leaverequest.lTypeId);

                        if (ltypeIdbalId != null)
                        {
                            var trecordBalCount =
                                db.LeaveRecordNews.FirstOrDefault(
                                    s =>
                                        s.LevetypeIds == ltypeIdbalId.leaveTypeId &&
                                        s.staffids == leaverequest.staffId)
                                    .totalLeaves;
                            if (trecordBalCount <= 0)
                            {
                                int staffId = LoginEmployeeId();
                                var lr = new LeaveRequest
                                {
                                    staffId = staffId,
                                    dateFrom = leaverequest.dateFrom,
                                    sesionDateFrom = leaverequest.sesionDateFrom,
                                    dateTo = leaverequest.dateTo,
                                    sesionDateTo = leaverequest.sesionDateTo,
                                    totalCounts = leaverequest.totalCounts,
                                    createdDate = DateTime.UtcNow,
                                    status = leaverequest.status,
                                    firmId = firm,
                                    lTypeId = leaverequest.lTypeId,
                                    DeductStatus = true
                                };

                                db.LeaveRequests.Add(lr);
                                db.SaveChanges();

                                var lrp = new LeavePassbook
                                {
                                    LpstaffId = staffId,
                                    LpdateFrom = leaverequest.dateFrom,
                                    LpsesionDateFrom = leaverequest.sesionDateFrom,
                                    LpdateTo = leaverequest.dateTo,
                                    LpsesionDateTo = leaverequest.sesionDateTo,
                                    totalLeavess = leaverequest.totalCounts,
                                    LpcreatedDate = DateTime.UtcNow,
                                    Lpstatus = leaverequest.status,
                                    LpfirmId = firm,
                                    LplTypeId = leaverequest.lTypeId,
                                    LplrequestId = lr.tranId,
                                    LpDeductStatus = true
                                };
                                db.LeavePassbooks.Add(lrp);
                                db.SaveChanges();
                                TempData["notice"] = "success";

                                var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                                var leaveReq = db.LeaveRequests.FirstOrDefault(a => a.staffId == staffId);

                                var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                                var reporting = db.Staffs.FirstOrDefault(a => a.staffId == staffId);

                                var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);

                                string emailabc = "";
                                foreach (var ex in reportingmails)
                                {
                                    var mgeremail =
                                        db.Staffs.Where(s => s.staffId == ex.ReportingManagerId)
                                            .FirstOrDefault()
                                            .email;

                                    emailabc = emailabc + mgeremail + ",";
                                }

                                string s2 = emailabc;
                                string[] words = s2.Split(',');
                                foreach (string word in words)
                                {
                                    var dbmail = word;

                                    string mailSendTo = reporting.emailReportingTo;
                                    var leaveReqID = lr.tranId;
                                    var lnkHref1 =
                                        "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                        Url.Action("ReplyApproval", "LeaveRequest",
                                            new
                                            {
                                                pagId = 1,
                                                empId = staffId,
                                                tranId = leaveReqID,
                                                status = true,
                                                uReportingMail = mailSendTo,
                                                countleave = leaverequest.totalCounts,
                                                firmid = firm,
                                            }, "http") + "' > Approve </a> ";
                                    var rejectHref =
                                        "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                        Url.Action("ReplyApproval", "LeaveRequest",
                                            new
                                            {
                                                pagId = 1,
                                                empId = staffId,
                                                tranId = leaveReqID,
                                                status = false,
                                                uReportingMail = mailSendTo,
                                                countleave = leaverequest.totalCounts,
                                                firmid = firm,
                                            }, "http") + "' > Reject </a> ";
                                    string subject1 = "Leave Request -" + userdetail.firstName + " " +
                                                      userdetail.lastName;
                                    string body1 =
                                        "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request For Leave - </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                                        userdetail.firstName + " " + userdetail.lastName + "<br> Leave Type :" + Leavetp + "<br> From Date: " +
                                        Convert.ToDateTime(leaverequest.dateFrom).ToString("dd-MMM-yyyy") +
                                        "<br>  To Date: " +
                                        Convert.ToDateTime(leaverequest.dateTo).ToString("dd-MMM-yyyy") +
                                        "<br> Total Days: " +
                                        leaverequest.totalCounts +
                                        "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" +
                                        lnkHref1 +
                                        "</td><td style='padding: 10px;'>" + rejectHref + "</td></tr></table>";
                                    var services = new MailServiceConfig();
                                    var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress,
                                        dbs.Password,
                                        dbs.HostName, dbs.SMTPPortNo);
                                }
                                return RedirectToAction("IndexLeavePassbook");
                            }
                        }
                        else
                        {
                            if (ModelState.IsValid)
                            {
                                if (leaverequest.totalCounts > finalBalance)
                                {
                                    TempData["notice"] = "notenough";
                                    return RedirectToAction("Create");
                                }

                                try
                                {
                                    int staffId = LoginEmployeeId();
                                    var lr = new LeaveRequest
                                    {
                                        staffId = staffId,
                                        dateFrom = leaverequest.dateFrom,
                                        sesionDateFrom = leaverequest.sesionDateFrom,
                                        dateTo = leaverequest.dateTo,
                                        sesionDateTo = leaverequest.sesionDateTo,
                                        totalCounts = leaverequest.totalCounts,
                                        createdDate = DateTime.UtcNow,
                                        status = leaverequest.status,
                                        firmId = firm,
                                        lTypeId = leaverequest.lTypeId,
                                        DeductStatus = true
                                    };
                                    db.LeaveRequests.Add(lr);
                                    db.SaveChanges();

                                    var lrp = new LeavePassbook
                                    {
                                        LpstaffId = staffId,
                                        LpdateFrom = leaverequest.dateFrom,
                                        LpsesionDateFrom = leaverequest.sesionDateFrom,
                                        LpdateTo = leaverequest.dateTo,
                                        LpsesionDateTo = leaverequest.sesionDateTo,
                                        totalLeavess = leaverequest.totalCounts,
                                        LpcreatedDate = DateTime.UtcNow,
                                        Lpstatus = leaverequest.status,
                                        LpfirmId = firm,
                                        LplTypeId = leaverequest.lTypeId,
                                        LplrequestId = lr.tranId,
                                        LpDeductStatus = true
                                    };

                                    db.LeavePassbooks.Add(lrp);
                                    db.SaveChanges();

                                    TempData["notice"] = "success";


                                    var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                                    var leaveReq = db.LeaveRequests.FirstOrDefault(a => a.staffId == staffId);

                                    var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                                    var reporting = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                                    string mailSendTo = reporting.emailReportingTo;

                                    var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);

                                    string emailabc = "";
                                    foreach (var ex in reportingmails)
                                    {
                                        var mgeremail =
                                            db.Staffs.Where(s => s.staffId == ex.ReportingManagerId)
                                                .FirstOrDefault()
                                                .email;

                                        emailabc = emailabc + mgeremail + ",";
                                    }

                                    string s2 = emailabc;
                                    string[] words = s2.Split(',');
                                    foreach (string word in words)
                                    {
                                        var dbmail = word;

                                        var leaveReqID = lr.tranId;
                                        var lnkHref1 =
                                            "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                            Url.Action("ReplyApproval", "LeaveRequest",
                                                new
                                                {
                                                    pagId = 1,
                                                    empId = staffId,
                                                    tranId = leaveReqID,
                                                    status = true,
                                                    uReportingMail = mailSendTo,
                                                    countleave = leaverequest.totalCounts,
                                                    firmid = firm,
                                                }, "http") + "' > Approve </a> ";
                                        var rejectHref =
                                            "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                            Url.Action("ReplyApproval", "LeaveRequest",
                                                new
                                                {
                                                    pagId = 1,
                                                    empId = staffId,
                                                    tranId = leaveReqID,
                                                    status = false,
                                                    uReportingMail = mailSendTo,
                                                    countleave = leaverequest.totalCounts,
                                                    firmid = firm,
                                                }, "http") + "' > Reject </a> ";
                                        string subject1 = "Leave Request -" + userdetail.firstName + " " +
                                                          userdetail.lastName;
                                        string body1 =
                                            "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request For Leave - </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                                            userdetail.firstName + " " + userdetail.lastName + "<br> Leave Type :" + Leavetp + "<br> From Date: " +
                                            Convert.ToDateTime(leaverequest.dateFrom).ToString("dd-MMM-yyyy") +
                                            "<br>  To Date: " +
                                            Convert.ToDateTime(leaverequest.dateTo).ToString("dd-MMM-yyyy") +
                                            "<br> Total Days: " +
                                            leaverequest.totalCounts +
                                            "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" +
                                            lnkHref1 +
                                            "</td><td style='padding: 10px;'>" + rejectHref + "</td></tr></table>";
                                        var services = new MailServiceConfig();
                                        var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress,
                                            dbs.Password, dbs.HostName, dbs.SMTPPortNo);
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
                                    throw;
                                }
                                return RedirectToAction("IndexLeavePassbook");
                            }
                        }
                    }
                }
                else
                {
                    var firm = LoginUserFirmId();
                    var leaveBL =
                        db.GetLeaveBalanceFromNewTbl(leaverequest.staffId, leaverequest.lTypeId).FirstOrDefault();
                    var finalBalance = leaveBL.TotalCount;

                    var ltypeIdbalId =
                        db.LeaveTypes.FirstOrDefault(
                            s =>
                                s.LeaveType1 == "Informed Absent" && s.firmId == firm &&
                                s.leaveTypeId == leaverequest.lTypeId);

                    if (ltypeIdbalId != null)
                    {
                        var trecordBalCount =
                            db.LeaveRecordNews.FirstOrDefault(
                                s => s.LevetypeIds == ltypeIdbalId.leaveTypeId && s.staffids == leaverequest.staffId)
                                .totalLeaves;
                        if (trecordBalCount <= 0)
                        {
                            int staffId = LoginEmployeeId();
                            var lr = new LeaveRequest
                            {
                                staffId = staffId,
                                dateFrom = leaverequest.dateFrom,
                                sesionDateFrom = leaverequest.sesionDateFrom,
                                dateTo = leaverequest.dateTo,
                                sesionDateTo = leaverequest.sesionDateTo,
                                totalCounts = leaverequest.totalCounts,
                                createdDate = DateTime.UtcNow,
                                status = leaverequest.status,
                                firmId = firm,
                                lTypeId = leaverequest.lTypeId,
                                DeductStatus = true
                            };

                            db.LeaveRequests.Add(lr);
                            db.SaveChanges();

                            var lrp = new LeavePassbook
                            {
                                LpstaffId = staffId,
                                LpdateFrom = leaverequest.dateFrom,
                                LpsesionDateFrom = leaverequest.sesionDateFrom,
                                LpdateTo = leaverequest.dateTo,
                                LpsesionDateTo = leaverequest.sesionDateTo,
                                totalLeavess = leaverequest.totalCounts,
                                LpcreatedDate = DateTime.UtcNow,
                                Lpstatus = leaverequest.status,
                                LpfirmId = firm,
                                LplTypeId = leaverequest.lTypeId,
                                LplrequestId = lr.tranId,
                                LpDeductStatus = true
                            };

                            db.LeavePassbooks.Add(lrp);
                            db.SaveChanges();

                            var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                            var leaveReq = db.LeaveRequests.FirstOrDefault(a => a.staffId == staffId);
                            var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                            var reporting = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                            string mailSendTo = reporting.emailReportingTo;

                            var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);
                            string emailabc = "";
                            foreach (var ex in reportingmails)
                            {
                                var mgeremail =
                                    db.Staffs.Where(s => s.staffId == ex.ReportingManagerId).FirstOrDefault().email;

                                emailabc = emailabc + mgeremail + ",";
                            }


                            string s2 = emailabc;
                            string[] words = s2.Split(',');
                            foreach (string word in words)
                            {
                                var dbmail = word;

                                var leaveReqID = lr.tranId;
                                var lnkHref1 =
                                    "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                    Url.Action("ReplyApproval", "LeaveRequest",
                                        new
                                        {
                                            pagId = 1,
                                            empId = staffId,
                                            tranId = leaveReqID,
                                            status = true,
                                            uReportingMail = mailSendTo,
                                            countleave = leaverequest.totalCounts,
                                            firmid = firm,
                                        }, "http") + "' > Approve </a> ";
                                var rejectHref =
                                    "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                    Url.Action("ReplyApproval", "LeaveRequest",
                                        new
                                        {
                                            pagId = 1,
                                            empId = staffId,
                                            tranId = leaveReqID,
                                            status = false,
                                            uReportingMail = mailSendTo,
                                            countleave = leaverequest.totalCounts,
                                            firmid = firm,
                                        }, "http") + "' > Reject </a> ";
                                string subject1 = "Leave Request -" + userdetail.firstName + " " +
                                                  userdetail.lastName;
                                string body1 =
                                    "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request For Leave - </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                                    userdetail.firstName + " " + userdetail.lastName + "<br> Leave Type :" + Leavetp + "<br> From Date: " +
                                    Convert.ToDateTime(leaverequest.dateFrom).ToString("dd-MMM-yyyy") +
                                    "<br>  To Date: " +
                                    Convert.ToDateTime(leaverequest.dateTo).ToString("dd-MMM-yyyy") +
                                    "<br> Total Days: " +
                                    leaverequest.totalCounts +
                                    "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" + lnkHref1 +
                                    "</td><td style='padding: 10px;'>" + rejectHref + "</td></tr></table>";

                                var services = new MailServiceConfig();
                                var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress, dbs.Password,
                                    dbs.HostName, dbs.SMTPPortNo);
                            }
                            TempData["notice"] = "success";
                            return RedirectToAction("IndexLeavePassbook");

                        }
                    }

                    if (ModelState.IsValid)
                    {
                        if (leaverequest.totalCounts > finalBalance)
                        {
                            TempData["notice"] = "notenough";
                            return RedirectToAction("Create");
                        }
                        try
                        {
                            int staffId = LoginEmployeeId();
                            var lr = new LeaveRequest
                            {
                                staffId = staffId,
                                dateFrom = leaverequest.dateFrom,
                                sesionDateFrom = leaverequest.sesionDateFrom,
                                dateTo = leaverequest.dateTo,
                                sesionDateTo = leaverequest.sesionDateTo,
                                totalCounts = leaverequest.totalCounts,
                                createdDate = DateTime.UtcNow,
                                status = leaverequest.status,
                                firmId = firm,
                                lTypeId = leaverequest.lTypeId,
                                DeductStatus = true
                            };

                            db.LeaveRequests.Add(lr);
                            db.SaveChanges();

                            var lrp = new LeavePassbook
                            {
                                LpstaffId = staffId,
                                LpdateFrom = leaverequest.dateFrom,
                                LpsesionDateFrom = leaverequest.sesionDateFrom,
                                LpdateTo = leaverequest.dateTo,
                                LpsesionDateTo = leaverequest.sesionDateTo,
                                totalLeavess = leaverequest.totalCounts,
                                LpcreatedDate = DateTime.UtcNow,
                                Lpstatus = leaverequest.status,
                                LpfirmId = firm,
                                LplTypeId = leaverequest.lTypeId,
                                LplrequestId = lr.tranId,
                                LpDeductStatus = true
                            };

                            db.LeavePassbooks.Add(lrp);
                            db.SaveChanges();
                            TempData["notice"] = "success";

                            var userdetail = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                            var leaveReq = db.LeaveRequests.FirstOrDefault(a => a.staffId == staffId);
                            var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);
                            var reporting = db.Staffs.FirstOrDefault(a => a.staffId == staffId);
                            string mailSendTo = reporting.emailReportingTo;

                            var reportingmails = db.Reportings.Where(a => a.StaffId == staffId);
                            string emailabc = "";
                            foreach (var ex in reportingmails)
                            {
                                var mgeremail =
                                    db.Staffs.Where(s => s.staffId == ex.ReportingManagerId).FirstOrDefault().email;
                                emailabc = emailabc + mgeremail + ",";
                            }
                            string s2 = emailabc;
                            string[] words = s2.Split(',');
                            foreach (string word in words)
                            {
                                var dbmail = word;

                                var leaveReqID = lr.tranId;
                                var lnkHref1 =
                                    "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                    Url.Action("ReplyApproval", "LeaveRequest",
                                        new
                                        {
                                            pagId = 1,
                                            empId = staffId,
                                            tranId = leaveReqID,
                                            status = true,
                                            uReportingMail = mailSendTo,
                                            countleave = leaverequest.totalCounts,
                                            firmid = firm,
                                        }, "http") + "' > Approve </a> ";
                                var rejectHref =
                                    "<a style='color: #333; background-color: #3C8DBC; border-color: #ccc; padding: 6px 12px; display: inline-block; cursor: pointer;' href='" +
                                    Url.Action("ReplyApproval", "LeaveRequest",
                                        new
                                        {
                                            pagId = 1,
                                            empId = staffId,
                                            tranId = leaveReqID,
                                            status = false,
                                            uReportingMail = mailSendTo,
                                            countleave = leaverequest.totalCounts,
                                            firmid = firm,
                                        }, "http") + "' > Reject </a> ";
                                string subject1 = "Leave Request -" + userdetail.firstName + " " +
                                                  userdetail.lastName;
                                string body1 =
                                    "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Request For Leave - </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'><br> Employee Name: " +
                                    userdetail.firstName + " " + userdetail.lastName + "<br> Leave Type :" + Leavetp + "<br> From Date: " +
                                    Convert.ToDateTime(leaverequest.dateFrom).ToString("dd-MMM-yyyy") +
                                    "<br>  To Date: " +
                                    Convert.ToDateTime(leaverequest.dateTo).ToString("dd-MMM-yyyy") +
                                    "<br> Total Days: " +
                                    leaverequest.totalCounts +
                                    "</td></tr><tr style='border: 1px solid gray; padding: 10px;'><td>" + lnkHref1 +
                                    "</td><td style='padding: 10px;'>" + rejectHref + "</td></tr></table>";

                                var services = new MailServiceConfig();
                                var flag = services.SendMail(dbmail, body1, subject1, dbs.EmailAddress, dbs.Password,
                                    dbs.HostName, dbs.SMTPPortNo);
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
                            throw;
                        }
                        return RedirectToAction("IndexLeavePassbook");
                    }

                    ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
                    ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1",
                        leaverequest.lTypeId);
                    ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
                    return View(leaverequest);
                }
            }
            else
            {
                ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
                ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", leaverequest.lTypeId);
                ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
                return RedirectToAction("IndexLeavePassbook");
            }
            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", leaverequest.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
            return View(leaverequest);
        }

        #endregion

        #region ---- ReplyApprovalForMail ----------
        public ActionResult ReplyApproval(int? pagId, int? empId, int? tranId, Boolean? status, string uReportingMail, decimal? countleave, int? firmid)
        {
            var IdnotNull = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId);
            if (IdnotNull != null)
            {
                if (status == true)
                {
                    //var itypeid = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId).lTypeId;
                    //var leaveBL = db.GetLeaveEmployeeLeaveBalance(empId, itypeid).FirstOrDefault();
                    //var finalBalance = leaveBL.TotalCount;

                    //if (countleave > finalBalance)
                    //{
                    //    TempData["flag"] = "Employee have not enough leave balance.";
                    //    return View();
                    //}
                    //else
                    //{
                    var alreadyapproved = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId);
                    if (alreadyapproved.status == "Approved")
                    {
                        TempData["flag"] = "Leave request already approved.";
                        return View();
                    }
                    else
                    {
                        var mguid = db.Reportings.FirstOrDefault(s => s.StaffId == empId).ReportingManagerId;
                        var mgrUId = db.Staffs.Where(s => s.staffId == mguid).FirstOrDefault().userId;

                        var leRe = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId);
                        leRe.status = status == true ? "Approved" : "Rejected";
                        leRe.approvedBY = mgrUId;
                        leRe.approvalDate = DateTime.Now;
                        db.LeaveRequests.AddOrUpdate(leRe);

                        var whleave =
                            db.LeaveTypes.Where(d => d.leaveTypeId == IdnotNull.lTypeId).FirstOrDefault().LeaveType1;

                        var recordid =
                           db.LeaveRecordNews.Where(s => s.LevetypeIds == IdnotNull.lTypeId && s.staffids == empId).FirstOrDefault();

                        var ttleave = recordid.totalLeaves - countleave;
                        LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                        {
                            tbll.staffids = empId;
                            tbll.totalLeaves = ttleave;
                            tbll.CreatedDate = DateTime.UtcNow;
                            tbll.LevetypeIds = IdnotNull.lTypeId;
                            tbll.firmsId = firmid;
                            tbll.IsActiveLeave = true;
                        }
                        db.LeaveRecordNews.AddOrUpdate(tbll);

                        //var lr = new TblLeaveRecord
                        //{
                        //    staffids = empId,
                        //    totalLeaves = -countleave,
                        //    CreatedDate = DateTime.UtcNow,
                        //    LevetypeIds = leRe.lTypeId,
                        //    firmsId = firmid,
                        //    IsActiveLeave = false
                        //};
                        //db.TblLeaveRecords.Add(lr);
                        db.SaveChanges();

                        var lb = db.LeavePassbooks.Where(a => a.LplrequestId == tranId).FirstOrDefault();

                        if (lb != null)
                        {
                            var lprequestid = lb.LptranId;
                            LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestid);
                            {
                                lb1.LplTypeId = leRe.lTypeId;
                                lb1.LptranId = lprequestid;
                                lb1.LpstaffId = empId;
                                lb1.LpdateFrom = leRe.dateFrom;
                                lb1.LpsesionDateFrom = leRe.sesionDateFrom;
                                lb1.LpdateTo = leRe.dateTo;
                                lb1.LpsesionDateTo = leRe.sesionDateTo;
                                lb1.LptotalCounts = countleave;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.Lpstatus = "Approved";
                                lb1.LpapprovedBY = mgrUId;
                                lb1.LpapprovalDate = DateTime.UtcNow;
                                lb1.LpfirmId = firmid;
                                lb1.LeaveRecordIds = recordid.LeaveRecordId;
                                lb1.LpDeductStatus = true;
                            }
                            ;
                            db.LeavePassbooks.AddOrUpdate(lb1);
                            db.SaveChanges();
                        }

                        db.SaveChanges();

                        string flag = status == true ? "Approved Successfully.!" : "Rejected.!";
                        TempData["flag"] = "Leave request - " + flag;

                        var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firmid);
                        var reporting = db.Staffs.FirstOrDefault(a => a.staffId == empId);
                        string mailSendTo = reporting.email;
                        string subject1 = "Leave Request status";
                        string body1 =
                            "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Leave Request Status- </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'>" +
                            "<br> Your " + whleave + " Request " + flag +
                            "</td></tr></table>";

                        var services = new MailServiceConfig();
                        services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName,
                            dbs.SMTPPortNo);
                    }
                }
                //}
                else
                {

                    var whleave1 =
                        db.LeaveTypes.FirstOrDefault(d => d.leaveTypeId == IdnotNull.lTypeId).LeaveType1;

                    var alreadyapproved = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId);
                    if (alreadyapproved.status == "Rejected")
                    {
                        TempData["flag"] = "Leave request already rejected.";
                        return View();
                    }
                    else if (alreadyapproved.status == "Approved")
                    {
                        var mguid = db.Reportings.FirstOrDefault(s => s.StaffId == empId).ReportingManagerId;
                        var mgrUId = db.Staffs.Where(s => s.staffId == mguid).FirstOrDefault().userId;

                        var leRe = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId);
                        leRe.status = "Rejected";
                        leRe.approvedBY = mgrUId;
                        leRe.approvalDate = DateTime.Now;
                        db.LeaveRequests.AddOrUpdate(leRe);
                        db.SaveChanges();

                        var lb2 = db.LeavePassbooks.FirstOrDefault(a => a.LplrequestId == tranId);
                        if (lb2 != null)
                        {
                            var lprequestidNew = lb2.LptranId;
                            LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestidNew);
                            {
                                lb1.LplTypeId = leRe.lTypeId;
                                lb1.LptranId = lprequestidNew;
                                lb1.LpstaffId = empId;
                                lb1.LpdateFrom = leRe.dateFrom;
                                lb1.LpsesionDateFrom = leRe.sesionDateFrom;
                                lb1.LpdateTo = leRe.dateTo;
                                lb1.LpsesionDateTo = leRe.sesionDateTo;
                                lb1.LptotalCounts = countleave;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.Lpstatus = "Rejected";
                                lb1.LpapprovedBY = mgrUId;
                                lb1.LpapprovalDate = DateTime.UtcNow;
                                lb1.LpfirmId = firmid;
                                lb1.LpDeductStatus = true;
                            }
                            db.LeavePassbooks.AddOrUpdate(lb1);
                        }
                        var recordid = db.LeaveRecordNews.FirstOrDefault(s => s.LevetypeIds == IdnotNull.lTypeId && s.staffids == empId);

                        var ttleave = recordid.totalLeaves + countleave;
                        LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                        {
                            tbll.staffids = empId;
                            tbll.totalLeaves = ttleave;
                            tbll.CreatedDate = DateTime.UtcNow;
                            tbll.LevetypeIds = IdnotNull.lTypeId;
                            tbll.firmsId = firmid;
                            tbll.IsActiveLeave = true;
                        }
                        db.LeaveRecordNews.AddOrUpdate(tbll);

                        //var lr = new TblLeaveRecord
                        //{
                        //    staffids = empId,
                        //    totalLeaves = countleave,
                        //    CreatedDate = DateTime.UtcNow,
                        //    LevetypeIds = leRe.lTypeId,
                        //    firmsId = leRe.firmId,
                        //    IsActiveLeave = false
                        //};
                        //db.TblLeaveRecords.Add(lr);
                        db.SaveChanges();

                        string flag = status == true ? "Approved Successfully.!" : "Rejected.!";
                        TempData["flag"] = "Leave request - " + flag;

                        var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firmid);
                        var reporting = db.Staffs.FirstOrDefault(a => a.staffId == empId);
                        string mailSendTo = reporting.email;
                        string subject1 = "Leave Request status";
                        string body1 =
                            "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Leave Request Status- </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'>" +
                            "<br> Your " + whleave1 + " Request " + flag +
                            "</td></tr></table>";
                        var services = new MailServiceConfig();
                        services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName,
                            dbs.SMTPPortNo);
                    }
                    else
                    {

                        var whleave2 =
                            db.LeaveTypes.FirstOrDefault(d => d.leaveTypeId == IdnotNull.lTypeId).LeaveType1;

                        var mguid = db.Reportings.FirstOrDefault(s => s.StaffId == empId).ReportingManagerId;
                        var mgrUId = db.Staffs.Where(s => s.staffId == mguid).FirstOrDefault().userId;
                        var leRe = db.LeaveRequests.FirstOrDefault(a => a.tranId == tranId);
                        leRe.status = status == true ? "Approved" : "Rejected";
                        leRe.approvedBY = mgrUId;
                        leRe.approvalDate = DateTime.Now;
                        db.LeaveRequests.AddOrUpdate(leRe);
                        db.SaveChanges();

                        var lb = db.LeavePassbooks.Where(a => a.LplrequestId == tranId).FirstOrDefault();
                        if (lb != null)
                        {
                            var lprequestid = lb.LptranId;
                            LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestid);
                            {
                                lb1.LplTypeId = leRe.lTypeId;
                                lb1.LptranId = lprequestid;
                                lb1.LpstaffId = empId;
                                lb1.LpdateFrom = leRe.dateFrom;
                                lb1.LpsesionDateFrom = leRe.sesionDateFrom;
                                lb1.LpdateTo = leRe.dateTo;
                                lb1.LpsesionDateTo = leRe.sesionDateTo;
                                lb1.LptotalCounts = countleave;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.Lpstatus = "Rejected";
                                lb1.LpapprovedBY = mgrUId;
                                lb1.LpapprovalDate = DateTime.UtcNow;
                                lb1.LpfirmId = firmid;
                                lb1.LpDeductStatus = true;
                            }
                            ;
                            db.LeavePassbooks.AddOrUpdate(lb1);
                            db.SaveChanges();
                        }

                        db.SaveChanges();

                        string flag = status == true ? "Approved Successfully.!" : "Rejected.!";
                        TempData["flag"] = "Leave request - " + flag;

                        var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firmid);
                        var reporting = db.Staffs.FirstOrDefault(a => a.staffId == empId);
                        string mailSendTo = reporting.email;
                        string subject1 = "Leave Request status";
                        string body1 =
                            "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Leave Request Status- </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'>" +
                            "<br> Your " + whleave2 + " Request " + flag +
                            "</td></tr></table>";

                        var services = new MailServiceConfig();
                        services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName,
                            dbs.SMTPPortNo);
                    }
                }
                return View();
            }
            TempData["flag"] = "Invalid leave request";
            return View();
        }
        #endregion

        #region ------------- Edit -----------------

        public ActionResult Edit(int? id, string status)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            if (status == "True")
            {
                TempData["status"] = " approved";
            }
            else
            {
                TempData["status"] = " rejected";
            }

            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes.Where(a => a.firmId == firm), "leaveTypeId", "LeaveType1", leaverequest.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
            return View(leaverequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveRequest request, bool? status, string ActionName)
        {
            if (ModelState.IsValid)
            {
                string loginuserid1 = GetUserId();
                if (status == true)
                {
                    string loginuserid = GetUserId();
                    var LeaveTtype =
                    db.LeaveTypes.FirstOrDefault(s => s.leaveTypeId == request.lTypeId && s.firmId == request.firmId).LeaveType1;

                    if (LeaveTtype == "Informed Absent")
                    {
                        LeaveRequest lv = db.LeaveRequests.Find(request.tranId);
                        {
                            lv.tranId = request.tranId;
                            lv.lTypeId = request.lTypeId;
                            lv.staffId = request.staffId;
                            lv.dateFrom = request.dateFrom;
                            lv.sesionDateFrom = request.sesionDateFrom;
                            lv.dateTo = request.dateTo;
                            lv.sesionDateTo = request.sesionDateTo;
                            lv.totalCounts = request.totalCounts;
                            lv.createdDate = DateTime.UtcNow;
                            lv.status = "Approved";
                            lv.approvedBY = loginuserid;
                            lv.approvalDate = DateTime.UtcNow;
                            lv.firmId = request.firmId;
                        }
                        db.LeaveRequests.AddOrUpdate(lv);

                        var recordid = db.LeaveRecordNews.FirstOrDefault(s => s.LevetypeIds == request.lTypeId && s.staffids == request.staffId);

                        var ttleave = recordid.totalLeaves - request.totalCounts;
                        LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                        {
                            tbll.staffids = request.staffId;
                            tbll.totalLeaves = ttleave;
                            tbll.CreatedDate = DateTime.UtcNow;
                            tbll.LevetypeIds = request.lTypeId;
                            tbll.firmsId = request.firmId;
                            tbll.IsActiveLeave = true;
                        }
                        db.LeaveRecordNews.AddOrUpdate(tbll);
                        db.SaveChanges();

                        //var lr = new TblLeaveRecord
                        //{
                        //    staffids = request.staffId,
                        //    totalLeaves = -request.totalCounts,
                        //    CreatedDate = DateTime.UtcNow,
                        //    LevetypeIds = request.lTypeId,
                        //    firmsId = request.firmId,
                        //    IsActiveLeave = false
                        //};
                        //db.TblLeaveRecords.Add(lr);
                        //db.SaveChanges();

                        var lb = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();

                        if (lb != null)
                        {
                            var lprequestid = lb.LptranId;
                            LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestid);
                            {
                                lb1.LplTypeId = request.lTypeId;
                                lb1.LptranId = lprequestid;
                                lb1.LpstaffId = request.staffId;
                                lb1.LpdateFrom = request.dateFrom;
                                lb1.LpsesionDateFrom = request.sesionDateFrom;
                                lb1.LpdateTo = request.dateTo;
                                lb1.LpsesionDateTo = request.sesionDateTo;
                                lb1.LptotalCounts = request.totalCounts;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.Lpstatus = "Approved";
                                lb1.LpapprovedBY = loginuserid;
                                lb1.LpapprovalDate = DateTime.UtcNow;
                                lb1.LpfirmId = request.firmId;
                                lb1.LeaveRecordIds = recordid.LeaveRecordId;
                                lb1.LpDeductStatus = true;
                            }
                            db.LeavePassbooks.AddOrUpdate(lb1);
                            db.SaveChanges();
                        }
                        TempData["notice"] = "leaverequest";
                        return RedirectToAction(ActionName);
                    }


                    var leaveBL = db.GetLeaveBalanceFromNewTbl(request.staffId, request.lTypeId).FirstOrDefault();
                    var finalBalance = leaveBL.TotalCount;

                    if (request.totalCounts > finalBalance)
                    {
                        TempData["notice"] = "notenoughApprove";
                        return RedirectToAction(ActionName);
                    }
                    else
                    {
                        LeaveRequest lv = db.LeaveRequests.Find(request.tranId);
                        {
                            lv.tranId = request.tranId;
                            lv.lTypeId = request.lTypeId;
                            lv.staffId = request.staffId;
                            lv.dateFrom = request.dateFrom;
                            lv.sesionDateFrom = request.sesionDateFrom;
                            lv.dateTo = request.dateTo;
                            lv.sesionDateTo = request.sesionDateTo;
                            lv.totalCounts = request.totalCounts;
                            lv.createdDate = DateTime.UtcNow;
                            lv.status = "Approved";
                            lv.approvedBY = loginuserid1;
                            lv.approvalDate = DateTime.UtcNow;
                            lv.firmId = request.firmId;
                        }
                        db.LeaveRequests.AddOrUpdate(lv);

                        var recordid =
                        db.LeaveRecordNews.Where(s => s.LevetypeIds == request.lTypeId && s.staffids == request.staffId).FirstOrDefault();

                        var ttleave = recordid.totalLeaves - request.totalCounts;
                        LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                        {
                            tbll.staffids = request.staffId;
                            tbll.totalLeaves = ttleave;
                            tbll.CreatedDate = DateTime.UtcNow;
                            tbll.LevetypeIds = request.lTypeId;
                            tbll.firmsId = request.firmId;
                            tbll.IsActiveLeave = true;
                        }
                        db.LeaveRecordNews.AddOrUpdate(tbll);
                        db.SaveChanges();

                        //var lr = new TblLeaveRecord
                        //{
                        //    staffids = request.staffId,
                        //    totalLeaves = -request.totalCounts,
                        //    CreatedDate = DateTime.UtcNow,
                        //    LevetypeIds = request.lTypeId,
                        //    firmsId = request.firmId,
                        //    IsActiveLeave = false
                        //};
                        //db.TblLeaveRecords.Add(lr);
                        //db.SaveChanges();

                        var lb = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();

                        if (lb != null)
                        {
                            var lprequestid = lb.LptranId;
                            LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestid);
                            {
                                lb1.LplTypeId = request.lTypeId;
                                lb1.LptranId = lprequestid;
                                lb1.LpstaffId = request.staffId;
                                lb1.LpdateFrom = request.dateFrom;
                                lb1.LpsesionDateFrom = request.sesionDateFrom;
                                lb1.LpdateTo = request.dateTo;
                                lb1.LpsesionDateTo = request.sesionDateTo;
                                lb1.LptotalCounts = request.totalCounts;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.Lpstatus = "Approved";
                                lb1.LpapprovedBY = loginuserid1;
                                lb1.LpapprovalDate = DateTime.UtcNow;
                                lb1.LpfirmId = request.firmId;
                                lb1.LeaveRecordIds = recordid.LeaveRecordId;
                                lb1.LpDeductStatus = true;
                            }
                            ;
                            db.LeavePassbooks.AddOrUpdate(lb1);
                            db.SaveChanges();

                            var leaveT = db.LeaveTypes.FirstOrDefault(d => d.leaveTypeId == request.lTypeId).LeaveType1;

                            var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == request.firmId);
                            var reporting = db.Staffs.FirstOrDefault(a => a.staffId == request.staffId);
                            string mailSendTo = reporting.email;
                            string subject1 = "Leave Request status";
                            string body1 =
                                "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Leave Request Status- </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'>" +
                                "<br> Your " + leaveT + " Request : Approved Successfully" +
                                "</td></tr></table>";

                            var services = new MailServiceConfig();
                            services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName,
                                dbs.SMTPPortNo);
                        }
                    }

                    TempData["notice"] = "leaverequest";
                    return RedirectToAction(ActionName);
                }
                else
                {
                    string loginuserid2 = GetUserId();
                    var design = new LeaveRequest()
                    {
                        tranId = request.tranId,
                        lTypeId = request.lTypeId,
                        staffId = request.staffId,
                        dateFrom = request.dateFrom,
                        sesionDateFrom = request.sesionDateFrom,
                        dateTo = request.dateTo,
                        sesionDateTo = request.sesionDateTo,
                        totalCounts = request.totalCounts,
                        createdDate = DateTime.UtcNow,
                        status = "Rejected",
                        approvedBY = loginuserid2,
                        approvalDate = DateTime.UtcNow,
                        firmId = request.firmId
                    };
                    db.LeaveRequests.AddOrUpdate(design);

                    var lb2 = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();
                    if (lb2 != null)
                    {
                        var lprequestidNew = lb2.LptranId;
                        LeavePassbook lb11 = db.LeavePassbooks.Find(lprequestidNew);
                        {
                            lb11.LplTypeId = request.lTypeId;
                            lb11.LptranId = lprequestidNew;
                            lb11.LpstaffId = request.staffId;
                            lb11.LpdateFrom = request.dateFrom;
                            lb11.LpsesionDateFrom = request.sesionDateFrom;
                            lb11.LpdateTo = request.dateTo;
                            lb11.LpsesionDateTo = request.sesionDateTo;
                            lb11.LptotalCounts = request.totalCounts;
                            lb11.LpcreatedDate = DateTime.UtcNow;
                            lb11.Lpstatus = "Rejected";
                            lb11.LpapprovedBY = loginuserid2;
                            lb11.LpapprovalDate = DateTime.UtcNow;
                            lb11.LpfirmId = request.firmId;
                            lb11.LpDeductStatus = true;
                        }
                        db.LeavePassbooks.AddOrUpdate(lb11);
                    }
                    db.SaveChanges();

                    var leaveT2 = db.LeaveTypes.FirstOrDefault(d => d.leaveTypeId == request.lTypeId).LeaveType1;

                    var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == request.firmId);
                    var reporting = db.Staffs.FirstOrDefault(a => a.staffId == request.staffId);
                    string mailSendTo = reporting.email;
                    string subject1 = "Leave Request status";
                    string body1 =
                        "<table style='border:  1px solid gray; '><tr style='background-color: #3C8DBC; border: 1px solid gray;'><td colspan='2' style='padding: 10px;'><b>Leave Request Status- </b> </td></tr><tr style='border:  1px solid gray; '><td colspan='2' style='border: 1px solid gray; padding: 10px;'>" +
                        "<br> Your " + leaveT2 + " Request : Rejected" +
                        "</td></tr></table>";

                    var services = new MailServiceConfig();
                    services.SendMail(mailSendTo, body1, subject1, dbs.EmailAddress, dbs.Password, dbs.HostName,
                        dbs.SMTPPortNo);

                    TempData["notice"] = "reject";
                    return RedirectToAction(ActionName);
                }
            }
            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", request.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", request.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", request.staffId);
            return View(request);
        }
        #endregion

        #region ----- Delete and Delete Confirm ----

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(
        //    [Bind(
        //        Include =
        //            "tranId,lTypeId,staffId,dateFrom,sesionDateFrom,dateTo,sesionDateTo,totalCounts,createdDate,status,approvedBY,approvalDate"
        //        )] LeaveRequest leaverequest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(leaverequest).State = EntityState.Modified;
        //        db.SaveChanges();
        //        TempData["notice"] = "Successfully Updated";
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
        //    ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", leaverequest.lTypeId);
        //    ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
        //    return View(leaverequest);
        //}

        // GET: /LeaveRequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        // POST: /LeaveRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaverequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        #endregion

        #region ----------- Des-pose ---------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region -------- LeaveBalanceDetails -------

        public ActionResult LeavesBalanceDetails()
        {
            int staffId = LoginEmployeeId();
            var mode = new LeaveRequestModel();
            var data = db.GetLeaveBalanceLeaveTypeWise(staffId);
            var rtnData = data.Select(a => new LeaveRequestModel()
            {
                LeaveTyp = a.LeaveType,
                LeaveBalance = a.AddedBalance,
                UsedBalance = a.ReduceBalance,
                RemainingBalance = a.AvailableBalance
            }).ToList();

            return View(rtnData);
        }

        #endregion

        #region ------- LeaveBalanceDashboard ------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult LeavesBalanceDashboard(int? staffId)
        {
            var mode = new LeaveRequestModel();
            var data = db.GetLeaveBalanceLeaveTypeWise(staffId);
            var rtnData = data.Select(a => new LeaveRequestModel()
            {
                LeaveTyp = a.LeaveType,
                LeaveBalance = a.AddedBalance,
                UsedBalance = a.ReduceBalance,
                RemainingBalance = a.AvailableBalance
            }).ToList();

            return View(rtnData);
        }

        #endregion

        #region -------- AddLeaveBalance -----------

        public ActionResult AddLeaveBalance(int? staffId)
        {
            var firm = LoginUserFirmId();
            LeaveRequestModel model = new LeaveRequestModel();
            model.staffId = staffId;
            ViewBag.lTypeId = new SelectList(db.LeaveTypes.Where(a => a.firmId == firm), "leaveTypeId", "LeaveType1");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddLeaveBalance(LeaveRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var leavebalance = new LeaveBalance
                {
                    leaveTypeId = model.lTypeId,
                    staffId = model.staffId,
                    noOfLeaves = model.NoOfLeaves,
                    Narration = model.Narration
                };
                db.LeaveBalances.Add(leavebalance);
                db.SaveChanges();

                return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + model.staffId) });
            }
            TempData["notice"] = "Validation Problem!";
            return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + model.staffId) });
        }

        #endregion

        #region ----------- Encrypt ----------------

        public string Encrypt(string plainText)
        {
            //string key = "jdsg432387#";
            string key = "chan221988#";
            byte[] EncryptKey = { };
            //byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] IV = { 27, 98, 45, 23, 65, 173, 17, 88 };
            EncryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        #endregion

        #region -------------- Search --------------

        public ActionResult Search()
        {
            var firm = LoginUserFirmId();
            Holiday hs = new Holiday();
            var getfatherInformation = db.Holidays.Where(s => s.firmId == firm).Select(a => a.date);
            return Json(getfatherInformation, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region -------- Leave-Count ---------------

        public ActionResult lcount(int leave)
        {
            Holiday hs = new Holiday();
            //var g = db.Holidays;
            //int a = 0;
            //DateTime? c = null;
            //foreach (var b in db.Holidays)
            //{
            //    a = b.holidayId;
            //     c = b.date;
            //}
            //var item = new Holiday
            //{
            //    holidayId = a,
            //    date = c
            //};

            //var g = db.Holidays.Select(a => a.date);
            //return Json(g);

            var leavecount = db.LeaveTypes.Find(leave);
            var lcount = leavecount.counts;
            int staffId = LoginEmployeeId();


            var remainingleaves = lcount -
                                  (db.LeaveRequests.Where(
                                      a => a.lTypeId == leave && a.status == "Approved" && a.staffId == staffId)
                                      .Sum(a => a.totalCounts));
            int p = 0;
            if (remainingleaves == null)
            {
                p = 0;
            }

            return Json(p);

        }

        #endregion

        #region ------- LeaveBalanceCount ----------

        //public string LeaveBanaceCount(int empId, int ltypeId)
        //{
        //    var leaveBalane =
        //        db.GetLeaveBalanceLeaveTypeWise(empId).FirstOrDefault(a => a.LeaveTypeId == ltypeId);
        //    return leaveBalane.AvailableBalance.ToString();
        //}

        public string EmployeeLeaveBalanceSelected(int empId, int ltypeId)
        {
            var leaveBalance =
                db.GetLeaveBalanceFromNewTbl(empId, ltypeId).FirstOrDefault();

            return leaveBalance.TotalCount.ToString();
        }

        #endregion

        #region ------------ Weekends --------------

        public ActionResult Weekends()
        {
            var firm = LoginUserFirmId();
            var deptModel = new WeekendsModel()
            {
                DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName"),
            };
            return View(deptModel);
        }


        [HttpPost]
        [WebMethod]
        public ActionResult Weekends(string Dates1, WeekendsModel model)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                var datestrings = !String.IsNullOrEmpty(Dates1) ? Dates1.Split(',') : new string[] { };

                for (int i = 0; i < datestrings.Count(); i++)
                {
                    var daysame = Convert.ToDateTime(datestrings[i]);

                    var condition = db.Weekends.FirstOrDefault(a => a.dates == daysame && a.deptId == model.DeptId);

                    if (condition == null)
                    {
                        var weekendObj = new Weekend()
                        {
                            dates = Convert.ToDateTime(datestrings[i]),
                            day = Convert.ToDateTime(datestrings[i]).DayOfWeek.ToString(),
                            deptId = model.DeptId,
                            firmId = firm
                        };
                        db.Weekends.Add(weekendObj);
                    }

                }
                db.SaveChanges();
                TempData["notice"] = "success";
                var deptModel = new WeekendsModel()
                {
                    DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName"),
                };
                return RedirectToAction("Weekends");
            }

            model.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName");
            return View(model);
        }

        #endregion

        #region ------------ Week-report -----------

        public ActionResult WeekendReport()
        {
            var firm = LoginUserFirmId();
            var weekendsmodel = db.Weekends.Where(a => a.firmId == firm)
                .Include(s => s.Department)
                .OrderBy(a => a.dates);
            return View(weekendsmodel.ToList());

            //var week = db.Weekends.Include(m=>m.Department).OrderBy(a => a.dates).Select(a => new WeekendsModel()
            //{
            //    Dates = a.dates,
            //    Day = a.day,
            //    DeptId = (int) a.deptId
            //}).ToList();
            //return View(week);            
        }

        #endregion

        #region ---------- Update Setting ----------

        [HttpGet]
        public ActionResult UpdateSetting()
        {
            var firm = LoginUserFirmId();
            var count = db.PayrollConfigs.Count(a => a.firmId == firm);
            if (count == 0)
            {
                ViewBag.visible = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateSetting(PayrollConfigModel payrollConfig)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var lr = new PayrollConfig()
                {
                    MonthlyLeaves = payrollConfig.MonthlyLeaves,
                    WorkingHoursDay = payrollConfig.WorkingHoursDay,
                    MinWorkingHoursForFullDay = payrollConfig.WorkingHoursDay,
                    HalfDayMinWorkHours = payrollConfig.HalfDayMinWorkHours,
                    WorkingdaysPermonth = payrollConfig.WorkingdaysPermonth,
                    isActive = true,
                    firmId = firm,
                };
                db.PayrollConfigs.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("UpdateSetting");
            }
            return View();
        }
        #endregion

        #region ------------- Index ----------------
        public ActionResult PayrollSettingIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.PayrollConfigs.Where(a => a.firmId == firm).ToList());
        }
        #endregion

        #region -------- DeletePayroll -------------
        public ActionResult DeletePayroll(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayrollConfig bts = db.PayrollConfigs.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeletePayroll(int id)
        {
            try
            {
                PayrollConfig bts = db.PayrollConfigs.Find(id);
                db.PayrollConfigs.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("UpdateSetting");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("UpdateSetting");
            }
        }
       #endregion

        #region ------------ DeleteWeekend ---------

        public ActionResult DeleteWeekend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Weekend week = db.Weekends.Find(id);
            if (week == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(week);
        }

        // POST: /Permissions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWeekend(int id)
        {
            Weekend week = db.Weekends.Find(id);
            db.Weekends.Remove(week);
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("Weekends");
        }


        #endregion

        #region -------- CompansationTran ----------

        public ActionResult CompansationTran()
        {
            return View();
        }

        #endregion

        #region ------- OptionalHolidayIndex -------

        public ActionResult OptionalHolidayIndex()
        {
            return View(db.OptionalHolidays.ToList().OrderBy(s => s.date));
        }

        #endregion

        #region ------ CompansationTranList --------

        public ActionResult CompansationTranList()
        {
            var leaverequests = db.CompansationTrans.Include(l => l.Staff);
            return View(leaverequests.ToList());
        }

        #endregion

        #region ------------ LeavesMaster ----------

        public ActionResult LeavesMaster(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            int firm = LoginUserFirmId();
            if (empId == 0)
                empId = null;
            var monthFirstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthLastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            if (empId == null && datepicker != null && datepicker2 != null)
            {
                var leaverequestsd =
                    db.LeaveRequests.Include(l => l.AspNetUser)
                        .Include(l => l.LeaveType)
                        .Include(l => l.Staff)
                        .Where(a => (a.dateFrom >= datepicker && a.dateFrom <= datepicker2) || (a.dateTo >= datepicker && a.dateFrom <= datepicker2 && a.firmId == firm)).OrderByDescending(s => s.createdDate);
                return PartialView(leaverequestsd.ToList());
            }
            else if (empId != null && datepicker != null && datepicker2 != null)
            {
                var leaverequestsd =
                    db.LeaveRequests.Include(l => l.AspNetUser)
                        .Include(l => l.LeaveType)
                        .Include(l => l.Staff)
                        .Where(
                            a =>
                                a.staffId == empId &&
                                ((a.dateFrom >= datepicker && a.dateFrom <= datepicker2) ||
                                 (a.dateTo >= datepicker && a.dateFrom <= datepicker2 && a.firmId == firm))).OrderByDescending(s => s.createdDate);
                return PartialView(leaverequestsd.ToList());
            }
            else if (empId != null && datepicker == null && datepicker2 == null)
            {
                var leaverequestsd =
                    db.LeaveRequests.Where(
                        a =>
                            a.staffId == empId &&
                            ((a.dateFrom >= monthFirstDate && a.dateFrom <= monthLastDate) ||
                             (a.dateTo >= monthFirstDate && a.dateFrom <= monthLastDate && a.firmId == firm))).OrderByDescending(s => s.createdDate);
                var count = leaverequestsd.Count();
                return PartialView(leaverequestsd.ToList());
            }
            else
            {
                var leaverequests =
                    db.LeaveRequests.Include(l => l.AspNetUser)
                        .Include(l => l.LeaveType)
                        .Include(l => l.Staff)
                        .Where(a => a.firmId == firm && (a.dateFrom >= monthFirstDate && a.dateFrom <= monthLastDate) || (a.dateTo >= monthFirstDate && a.dateFrom <= monthLastDate)).OrderByDescending(s => s.createdDate);
                return PartialView(leaverequests.ToList());
            }
        }

        #endregion

        #region ----- OptionalHolidayRequestList ---

        public ActionResult OptionalHolidayRequestList()
        {
            var optional = db.OptionalHolidayTrans.Include(l => l.OptionalHoliday).Include(l => l.Staff);
            return View(optional.ToList());
        }

        #endregion

        #region -------- Edit-weekend --------------

        [HttpGet]
        public ActionResult Editweekend(int? weekendId)
        {
            var firm = LoginUserFirmId();
            if (weekendId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var weekend = db.Weekends.Find(weekendId);
            if (weekend == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            ViewBag.departmentlist = new SelectList(db.Departments.Where(s => s.firmId == firm), "deptId", "deptName", weekend.deptId);

            return View(weekend);
        }

        #endregion

        #region ---------- Edit-weekend ------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editweekend(Weekend objWeekend)
        {
            var weekend = db.Weekends.Find(objWeekend.weekendId);
            if (weekend != null)
            {
                if (ModelState.IsValid)
                {
                    // Weekend weekend = new Weekend();
                    weekend.dates = objWeekend.dates;
                    weekend.deptId = objWeekend.deptId;
                    if (objWeekend.dates != null)
                    {
                        DateTime weekdate = (DateTime)objWeekend.dates;
                        string weekday = Convert.ToString(weekdate.DayOfWeek);
                        weekend.day = weekday;
                    }
                    db.Weekends.AddOrUpdate(weekend);
                    db.SaveChanges();
                    TempData["notice"] = "update";
                }
                return RedirectToAction("Weekends");
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ------- OptionalHolidayCreate ------

        public ActionResult OptionalHolidayCreate()
        {
            return View();
        }

      
        [HttpPost]
        public ActionResult OptionalHolidayCreate(OptionalHolidayModel mmodel)
        {
            if (ModelState.IsValid)
            {
                var lr = new OptionalHoliday()
                {
                    optionalHolidayName = mmodel.optionalHolidayName,
                    date = mmodel.date,
                };

                db.OptionalHolidays.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("OptionalHolidayCreate");
            }
            return View(mmodel);
        }


        #endregion

        #region ------- OptionalHolidayEdit --------

        public ActionResult OptionalHolidayEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionalHoliday holiday = db.OptionalHolidays.Find(id);
            if (holiday == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(holiday);
        }

        [HttpPost]
        public ActionResult OptionalHolidayEdit(
            [Bind(Include = "optionalId,optionalHolidayName,date")] OptionalHoliday holiday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holiday).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("OptionalHolidayCreate");
            }
            return View(holiday);
        }

        #endregion

        #region -------- OptionalHolidayDelete -----

        public ActionResult OptionalHolidayDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionalHoliday holiday = db.OptionalHolidays.Find(id);
            if (holiday == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(holiday);
        }

        [HttpPost, ActionName("OptionalHolidayDelete")]
        public ActionResult OptionalHolidayDeleteConfirmed(int id)
        {
            OptionalHoliday holiday = db.OptionalHolidays.Find(id);
            db.OptionalHolidays.Remove(holiday);
            db.SaveChanges();
            return RedirectToAction("OptionalHolidayCreate");
        }

        #endregion

        #region ---------- CreateLeaveMaster -------
        public ActionResult CreateLeaveMaster()
        {
            var leaveMaster = new LeaveMasterModel();
            var firm = LoginUserFirmId();
            ViewBag.lTypeId = new SelectList(db.LeaveTypes.Where(a => a.firmId == firm), "leaveTypeId", "LeaveType1");
            leaveMaster = new LeaveMasterModel
            {
                StaffList =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.staffCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        })
            };
            return View(leaveMaster);
        }         

        [HttpPost]
        public ActionResult CreateLeaveMaster(LeaveMasterModel model, string add, string deduct)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                if (add != null)
                {
                    foreach (var staffid in model.StaffId)
                    {
                        var newstafffid = Convert.ToInt32(staffid);
                        var recordid =
                            db.LeaveRecordNews.FirstOrDefault(s => s.LevetypeIds == model.LevetypeIds && s.staffids == newstafffid);
                        if (recordid != null)
                        {                          
                            LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                            {
                                tbll.staffids = newstafffid;
                                tbll.totalLeaves = model.totalLeaves;
                                tbll.CreatedDate = DateTime.UtcNow;
                                tbll.LevetypeIds = model.LevetypeIds;
                                tbll.firmsId = firm;
                                tbll.IsActiveLeave = true;
                            }
                            db.LeaveRecordNews.AddOrUpdate(tbll);
                            db.SaveChanges();

                            int lstid3 = recordid.LeaveRecordId;
                            var passbookid = db.LeavePassbooks.FirstOrDefault(s => s.LeaveRecordIds == recordid.LeaveRecordId && s.LpstaffId == newstafffid);
                            LeavePassbook lb1 = db.LeavePassbooks.Find(passbookid.LptranId);
                            {
                                lb1.LpstaffId = newstafffid;
                                lb1.LptotalCounts = model.totalLeaves;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.LplTypeId = model.LevetypeIds;
                                lb1.LpfirmId = firm;                                
                                lb1.LeaveRecordIds = lstid3;
                            }
                            db.LeavePassbooks.AddOrUpdate(lb1);
                            db.SaveChanges();                            
                        }
                        else
                        {
                            var lrNew = new LeaveRecordNew
                            {
                                staffids = newstafffid,
                                totalLeaves = model.totalLeaves,
                                CreatedDate = DateTime.UtcNow,
                                LevetypeIds = model.LevetypeIds,
                                firmsId = firm,
                                IsActiveLeave = true
                            };
                            db.LeaveRecordNews.Add(lrNew);
                            db.SaveChanges();

                            int lstid5 = lrNew.LeaveRecordId;
                            var lrp = new LeavePassbook
                            {
                                LpstaffId = newstafffid,
                                LptotalCounts = model.totalLeaves,
                                LpcreatedDate = DateTime.UtcNow,
                                LplTypeId = model.LevetypeIds,
                                LpfirmId = firm,                               
                                LeaveRecordIds = lstid5,
                            };
                            db.LeavePassbooks.Add(lrp);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    foreach (var staffid in model.StaffId)
                    {
                        var newstafffid = Convert.ToInt32(staffid);
                        var recordid =
                            db.LeaveRecordNews.FirstOrDefault(s => s.LevetypeIds == model.LevetypeIds && s.staffids == newstafffid);

                        var ttleave = recordid.totalLeaves - model.totalLeaves;
                        LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                        {
                            tbll.staffids = newstafffid;
                            tbll.totalLeaves = ttleave;
                            tbll.CreatedDate = DateTime.UtcNow;
                            tbll.LevetypeIds = model.LevetypeIds;
                            tbll.firmsId = firm;
                            tbll.IsActiveLeave = true;
                        }
                        db.LeaveRecordNews.AddOrUpdate(tbll);
                        db.SaveChanges();

                        var lrp = new LeavePassbook
                        {
                            LpstaffId = newstafffid,
                            LptotalCounts = model.totalLeaves,
                            LpcreatedDate = DateTime.UtcNow,
                            LplTypeId = model.LevetypeIds,
                            LpfirmId = firm,
                            LeaveRecordIds = recordid.LeaveRecordId,
                            LpDeductStatus = true
                        };
                        db.LeavePassbooks.Add(lrp);
                        db.SaveChanges();
                    }
                }
                TempData["notice"] = "success";
                return RedirectToAction("CreateLeaveMaster", "LeaveRequest");
            }
            else
            {
                ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", model.LevetypeIds);
                return View(model);
            }
        }
        #endregion

        #region -------- IndexEmpLeaveSheet --------
        public ActionResult IndexEmpLeaveSheet()
        {
            var firm = LoginUserFirmId();
            int staffId = LoginEmployeeId();
            var leaverequests =
                db.LeaveRecordNews.Include(l => l.LeaveType).Include(l => l.Staff);
            return View(leaverequests.Where(a => a.staffids == staffId).ToList());
        }
        #endregion

        #region ----- EmployeeLeaveBucket ----------
        public ActionResult EmployeeLeaveBucket()
        {
            int staffId = LoginEmployeeId();
            var getList = db.EmployeeLeaveBucket(staffId);
            var atsheetModel = getList.Select(a => new EmployeebucketModel()
            {
                LeavesTypeAndTotalLeaves = a.LeaveType + ": " + a.totalLeaves,
            }).ToList();
            return View(atsheetModel);
        }

        #endregion 

        #region --------- IndexLeavePassbook -------
        [Authorize]
        public ActionResult IndexLeavePassbook()
        {
            var firm = LoginUserFirmId();
            int staffId = LoginEmployeeId();
            var leavePassbook =
                db.LeavePassbooks.Include(l => l.LeaveType).Include(l => l.TblLeaveRecord).Include(s => s.Staff).OrderByDescending(s => s.LpcreatedDate);
            return View(leavePassbook.Where(a => a.LpstaffId == staffId && a.LpfirmId == firm).ToList());
        }
        #endregion


        #region ------- AllEmpLeavePassbook --------
        public ActionResult AllEmpLeavePassbook(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            var firmId = LoginUserFirmId();
            ViewBag.staffid =
              db.Staffs.ToList().Where(s => s.firmId == firmId && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });
            if (datepicker == null && datepicker2 == null)
            {
                var loginId = LoginEmployeeId();
                var firm = LoginUserFirmId();
                var leavePassbook =
                    db.LeavePassbooks.Include(l => l.LeaveType)
                        .Include(l => l.TblLeaveRecord)
                        .Include(s => s.Staff)
                        .OrderByDescending(s => s.LpcreatedDate);
                return View(leavePassbook.Where(a => a.LpfirmId == firm && a.Staff.isActive == true).ToList());
            }
            else if (datepicker != null && datepicker2 != null && empId == null)
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var firm = LoginUserFirmId();
                var loginId = LoginEmployeeId();
                var leavePassbook =
                db.LeavePassbooks.Where(a => a.LpcreatedDate >= start && a.LpcreatedDate <= end).Include(l => l.LeaveType).Include(l => l.TblLeaveRecord).Include(s => s.Staff).OrderByDescending(s => s.LpcreatedDate);
                return View(leavePassbook.Where(a => a.LpfirmId == firm && a.Staff.isActive == true).ToList());
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;
                var firm = LoginUserFirmId();
                var leavePassbook =
                db.LeavePassbooks.Where(a => a.LpcreatedDate >= start && a.LpcreatedDate <= end).Include(l => l.LeaveType).Include(l => l.TblLeaveRecord).Include(s => s.Staff).OrderByDescending(s => s.LpcreatedDate).Where(s => s.LpstaffId == empId);
                return View(leavePassbook.Where(a => a.LpfirmId == firm && a.Staff.isActive == true).ToList());
            }
        }

#endregion


        #region ---- EmployeeLeaveBucketUpdated ----
        public ActionResult EmployeeLeaveBucketUpdated()
        {
            int staffId = LoginEmployeeId();
            int firmid = LoginUserFirmId();

            var getList = db.EmployeeLeaveBucketFromNewTable(staffId, firmid);
            var atsheetModel = getList.Select(a => new EmployeebucketModel()
            {
                LeavesTypeAndTotalLeaves = (a.LeaveType == "Informed Absent" ? a.TotalCount : (a.TotalCount < 0 ? 0 : a.TotalCount)).ToString(),
                EmployeeCode = a.fullname,
                LeavesType = a.LeaveType,
                TotalLeaves = a.TotalCount < 0 ? 0 : a.TotalCount,
            }).ToList();
            return View(atsheetModel);
        }
        #endregion


        #region -------- AllEmpLeaveSheet ----------
        public ActionResult AllEmpLeaveSheet(int? staffid)
        {
            int firmid = LoginUserFirmId();

            ViewBag.staffid =
             db.Staffs.ToList().Where(s => s.firmId == firmid && s.isActive == true).Select(a => new SelectListItem()
             {
                 Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                 Value = a.staffId.ToString()
             });

            if (staffid == null)
            {
                var agencyContracts = db.LeavesheetAllEmp(firmid).GroupBy(
                    x => new
                    {
                        x.fullname,
                        x.LeaveType
                    }).Where(s => s.FirstOrDefault().firmsId == firmid)
                    .Select(
                        g => new TblLeaveRecordModel()
                        {
                            leavetypes = g.FirstOrDefault().LeaveType,
                            totalLeaves =
                                (g.FirstOrDefault().LeaveType == "Compensation Leaves"
                                    ? g.Sum(s => s.totalLeaves) < 0 ? 0 : (g.Sum(s => s.totalLeaves))
                                    : (g.Sum(s => s.totalLeaves))),
                            staffname = g.FirstOrDefault().fullname,
                        }).ToList();
                return View(agencyContracts);
            }
            else
            {
                var agencyContracts = db.LeavesheetAllEmp(firmid).GroupBy(
                    x => new
                    {
                        x.staffids,
                        x.fullname,
                        x.LeaveType
                    }).Where(s => s.FirstOrDefault().firmsId == firmid && s.FirstOrDefault().staffids == staffid)
                    .Select(
                        g => new TblLeaveRecordModel()
                        {
                            leavetypes = g.FirstOrDefault().LeaveType,
                            totalLeaves =
                                (g.FirstOrDefault().LeaveType == "Compensation Leaves"
                                    ? g.Sum(s => s.totalLeaves) < 0 ? 0 : (g.Sum(s => s.totalLeaves))
                                    : (g.Sum(s => s.totalLeaves))),
                            staffname = g.FirstOrDefault().fullname,
                        }).ToList();
                return View(agencyContracts);
            }
        }
#endregion

        #region ---- AllEmployeeLeaveBalanceSheet --
        public ActionResult AllEmployeeLeaveBalanceSheet()
        {
            int firmid = LoginUserFirmId();
            var getList = db.AllEmployeeLeaveBalanceSheet(firmid);
            var atsheetModel = getList.Select(a => new EmployeebucketModel()
            {
                EmployeeCode = a.schoolCode + " (" + a.fullname + ")",
                EmployeeName = a.fullname,
                LeavesTypeAndTotalLeaves = a.LeaveType + ":" + a.TotalCount,
                TotalLeaves = a.TotalCount,
            }).ToList();
            return View(atsheetModel);
        }

#endregion

        #region ------- DeleteLeaveRequest ---------
        public ActionResult DeleteLeaveRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeavePassbook leaverequest = db.LeavePassbooks.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }
#endregion

        #region ------- DeleteLeaveRequest ---------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequest(int id)
        {
            LeavePassbook leaverequest = db.LeavePassbooks.Find(id);
            db.LeavePassbooks.Remove(leaverequest);

            var lb = db.LeavePassbooks.Where(a => a.LptranId == id).FirstOrDefault();
            if (lb != null)
            {
                var lprequestid = lb.LplrequestId;
                LeaveRequest lr = db.LeaveRequests.Find(lprequestid);
                db.LeaveRequests.Remove(lr);
            }
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("IndexLeavePassbook");
        }
#endregion

        #region --- DeleteLeaveRequestSeniour ------
        public ActionResult DeleteLeaveRequestSeniour(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestSeniour(int id)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaverequest);

            var lb = db.LeavePassbooks.Where(a => a.LplrequestId == id).FirstOrDefault();
            if (lb != null)
            {
                var lprequestid = lb.LptranId;
                LeavePassbook lr = db.LeavePassbooks.Find(lprequestid);
                db.LeavePassbooks.Remove(lr);
            }
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("LeaveRequestListForSeniours");
        }
        #endregion

        #region ------ DeleteLeaveRequestMgr -------
        public ActionResult DeleteLeaveRequestMgr(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestMgr(int id)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaverequest);

            var lb = db.LeavePassbooks.Where(a => a.LplrequestId == id).FirstOrDefault();
            if (lb != null)
            {
                var lprequestid = lb.LptranId;
                LeavePassbook lr = db.LeavePassbooks.Find(lprequestid);
                db.LeavePassbooks.Remove(lr);
            }
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("LeavesMasterManager");
        }
#endregion

        #region ----- DeleteLeaveRequestAfterApp ---
        public ActionResult DeleteLeaveRequestAfterApp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestAfterApp(int id)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaverequest);

            var lb = db.LeavePassbooks.Where(a => a.LplrequestId == id).FirstOrDefault();
            if (lb != null)
            {
                // var lprequestLr = lb.LeaveRecordIds;
                // TblLeaveRecord lrRecord = db.TblLeaveRecords.Find(lprequestLr);
                var lprequestid = lb.LptranId;
                LeavePassbook lr = db.LeavePassbooks.Find(lprequestid);

                //   db.TblLeaveRecords.Remove(lrRecord);
                db.LeavePassbooks.Remove(lr);
            }
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("LeaveRequestListForSeniours");
        }
#endregion

        #region -- DeleteLeaveRequestAfterAppMgr ---
        public ActionResult DeleteLeaveRequestAfterAppMgr(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestAfterAppMgr(int id)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaverequest);

            var lb = db.LeavePassbooks.Where(a => a.LplrequestId == id).FirstOrDefault();
            if (lb != null)
            {
                //  var lprequestLr = lb.LeaveRecordIds;
                // TblLeaveRecord lrRecord = db.TblLeaveRecords.Find(lprequestLr);

                var lprequestid = lb.LptranId;
                LeavePassbook lr = db.LeavePassbooks.Find(lprequestid);

                // db.TblLeaveRecords.Remove(lrRecord);
                db.LeavePassbooks.Remove(lr);

            }

            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("LeavesMasterManager");
        }
#endregion

        #region ---- DeleteLeaveRequestAfterReject -
        public ActionResult DeleteLeaveRequestAfterReject(int? id, string actionName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestAfterReject(int id, string actionName)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaverequest);

            var lb = db.LeavePassbooks.Where(a => a.LplrequestId == id).FirstOrDefault();
            if (lb != null)
            {
                LeavePassbook lr = db.LeavePassbooks.Find(lb.LptranId);
                db.LeavePassbooks.Remove(lr);
            }

            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction(actionName);
        }
#endregion

        #region --- DeleteLeaveReqAfterRejectEmp ---
        public ActionResult DeleteLeaveRequestAfterRejectEmp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LeavePassbook leaverequest = db.LeavePassbooks.Find(id);

            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestAfterRejectEmp(int id)
        {
            var lb = db.LeavePassbooks.Where(a => a.LptranId == id).FirstOrDefault();
            if (lb != null)
            {

                var lprequestid = lb.LplrequestId;
                LeaveRequest lr = db.LeaveRequests.Find(lprequestid);
                LeavePassbook leaverequest = db.LeavePassbooks.Find(id);
                db.LeaveRequests.Remove(lr);
                db.LeavePassbooks.Remove(leaverequest);
            }
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("IndexLeavePassbook");
        }
#endregion

        #region ---- DeleteLeaveReqAfterAppEmp -----
        public ActionResult DeleteLeaveRequestAfterAppEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LeavePassbook leaverequest = db.LeavePassbooks.Find(id);

            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leaverequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeaveRequestAfterAppEmployee(int id)
        {
            var lb = db.LeavePassbooks.Where(a => a.LptranId == id).FirstOrDefault();
            if (lb != null)
            {

                var lprequestid = lb.LplrequestId;
                LeaveRequest lr = db.LeaveRequests.Find(lprequestid);

                //  var lprequestLr = lb.LeaveRecordIds;
                //   TblLeaveRecord lrRecord = db.TblLeaveRecords.Find(lprequestLr);

                LeavePassbook leaverequest = db.LeavePassbooks.Find(id);

                //  db.TblLeaveRecords.Remove(lrRecord);
                db.LeaveRequests.Remove(lr);
                db.LeavePassbooks.Remove(leaverequest);
            }
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("IndexLeavePassbook");
        }
#endregion

        #region -------- LeaveMasterEdit -----------

        public ActionResult EditLeavesAfterApproval(int? id, string actionName)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }

            LeaveRequestModel model = new LeaveRequestModel();
            model.tranId = leaverequest.tranId;
            model.ActionName = actionName;
            model.approvalDate = leaverequest.approvalDate;
            model.approvedBY = leaverequest.approvedBY;
            model.createdDate = leaverequest.createdDate;
            model.staffId = leaverequest.staffId;
            model.dateFrom = leaverequest.dateFrom;
            model.sesionDateFrom = leaverequest.sesionDateFrom;
            model.sesionDateTo = leaverequest.sesionDateTo;
            model.dateTo = leaverequest.dateTo;
            model.rejoing = leaverequest.rejoing;
            model.status = leaverequest.status;
            model.Staff = leaverequest.Staff;
            model.firmId = leaverequest.firmId;
            model.totalCounts = leaverequest.totalCounts;
            model.lTypeId = leaverequest.lTypeId;
            model.LtypeName = leaverequest.LeaveType.LeaveType1;

            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
            ViewBag.Leavetype = new SelectList(db.LeaveTypes.Where(a => a.firmId == leaverequest.firmId), "leaveTypeId", "LeaveType1", leaverequest.lTypeId);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditLeavesAfterApproval(LeaveRequest request, string actionName, string status)
        {
            string loginuserid = GetUserId();
            if (ModelState.IsValid)
            {
                var alredyleave = db.LeaveRequests.FirstOrDefault(a => a.tranId == request.tranId && a.status == "Approved").totalCounts;

                var leaveBL = db.GetLeaveBalanceFromNewTbl(request.staffId, request.lTypeId).FirstOrDefault();
                var finalBalance = leaveBL.TotalCount;

                var remainLeave = request.totalCounts - alredyleave;

                var ltype = db.LeaveTypes.FirstOrDefault(s => s.firmId == request.firmId && s.leaveTypeId == request.lTypeId);

                if ((remainLeave > finalBalance) || (finalBalance == 0) && ltype.LeaveType1 != "Informed Absent")
                {
                    TempData["notice"] = "notenough";
                    return RedirectToAction(actionName);
                }
                else
                {
                    if (status == "Approved")
                    {
                        LeaveRequest lv = db.LeaveRequests.Find(request.tranId);
                        {
                            lv.tranId = request.tranId;
                            lv.lTypeId = request.lTypeId;
                            lv.staffId = request.staffId;
                            lv.dateFrom = request.dateFrom;
                            lv.sesionDateFrom = request.sesionDateFrom;
                            lv.dateTo = request.dateTo;
                            lv.sesionDateTo = request.sesionDateTo;
                            lv.totalCounts = request.totalCounts;
                            lv.createdDate = DateTime.UtcNow;
                            lv.status = "Approved";
                            lv.approvedBY = loginuserid;
                            lv.approvalDate = DateTime.UtcNow;
                            lv.firmId = request.firmId;
                        }
                        db.LeaveRequests.AddOrUpdate(lv);
                        var lb = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();

                        if (lb != null)
                        {
                            var lprequestid = lb.LptranId;
                            LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestid);
                            {
                                lb1.LplTypeId = request.lTypeId;
                                lb1.LptranId = lprequestid;
                                lb1.LpstaffId = request.staffId;
                                lb1.LpdateFrom = request.dateFrom;
                                lb1.LpsesionDateFrom = request.sesionDateFrom;
                                lb1.LpdateTo = request.dateTo;
                                lb1.LpsesionDateTo = request.sesionDateTo;
                                lb1.LptotalCounts = request.totalCounts;
                                lb1.LpcreatedDate = DateTime.UtcNow;
                                lb1.Lpstatus = "Approved";
                                lb1.LpapprovedBY = loginuserid;
                                lb1.LpapprovalDate = DateTime.UtcNow;
                                lb1.LpfirmId = request.firmId;
                                lb1.LeaveRecordIds = lb.LeaveRecordIds;
                                lb1.LpDeductStatus = true;
                                lb1.totalLeavess = request.totalCounts;
                            }

                            db.LeavePassbooks.AddOrUpdate(lb1);

                            var recordid =
                            db.LeaveRecordNews.Where(s => s.LevetypeIds == request.lTypeId && s.staffids == request.staffId).FirstOrDefault();

                            var ttleave = recordid.totalLeaves - remainLeave;
                            LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                            {
                                tbll.staffids = request.staffId;
                                tbll.totalLeaves = ttleave;
                                tbll.CreatedDate = DateTime.UtcNow;
                                tbll.LevetypeIds = request.lTypeId;
                                tbll.firmsId = request.firmId;
                                tbll.IsActiveLeave = true;
                            }
                            db.LeaveRecordNews.AddOrUpdate(tbll);

                            //TblLeaveRecord tbll = db.TblLeaveRecords.Find(lb.LeaveRecordIds);
                            //{
                            //    tbll.staffids = request.staffId;
                            //    tbll.totalLeaves = -request.totalCounts;
                            //    tbll.CreatedDate = DateTime.UtcNow;
                            //    tbll.LevetypeIds = request.lTypeId;
                            //    tbll.firmsId = request.firmId;
                            //    tbll.IsActiveLeave = false;
                            //}
                            //db.TblLeaveRecords.AddOrUpdate(tbll);

                            db.SaveChanges();
                            TempData["notice"] = "update";
                            return RedirectToAction(actionName);
                        }
                    }
                }
            }
            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", request.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", request.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", request.staffId);
            return View(request);

        }




        #region ---- Get Staff Under Manager ----
        [HttpPost]
        public ActionResult GetStaff()
        {
            List<int> subcatId = new List<int>();
            List<string> subcat = new List<string>();
            var loginid = LoginEmployeeId();
            var id = GetUserId();
            Boolean flag = false;
            var firm = LoginUserFirmId();

            foreach (var VARIABLE in db.Staffs.Where(a => a.firmId == firm && a.reportingId == loginid))
            {
                subcat.Add(VARIABLE.staffCode + ": " + VARIABLE.firstName + " " + VARIABLE.middleName + " " +
                           VARIABLE.lastName);
                subcatId.Add(VARIABLE.staffId);
            }
            return Json(new { Subcat = subcat, SubcatId = subcatId });
        }
        #endregion


        public ActionResult RejectAfterApproval(int? id, string status)
        {

            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaverequest = db.LeaveRequests.Find(id);
            if (leaverequest == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            if (status == "True")
            {
                TempData["status"] = " approved";
            }
            else
            {
                TempData["status"] = " rejected";
            }

            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", leaverequest.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes.Where(a => a.firmId == firm), "leaveTypeId", "LeaveType1", leaverequest.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", leaverequest.staffId);
            return View(leaverequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectAfterApproval(LeaveRequest request, bool? status, string ActionName)
        {
            if (ModelState.IsValid)
            {
                string loginuserid = GetUserId();

                var design = new LeaveRequest()
                {
                    tranId = request.tranId,
                    lTypeId = request.lTypeId,
                    staffId = request.staffId,
                    dateFrom = request.dateFrom,
                    sesionDateFrom = request.sesionDateFrom,
                    dateTo = request.dateTo,
                    sesionDateTo = request.sesionDateTo,
                    totalCounts = request.totalCounts,
                    createdDate = DateTime.UtcNow,
                    status = "Rejected",
                    approvedBY = loginuserid,
                    approvalDate = DateTime.UtcNow,
                    firmId = request.firmId
                };
                db.LeaveRequests.AddOrUpdate(design);

                var lb2 = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();
                if (lb2 != null)
                {
                    var lprequestidNew = lb2.LptranId;
                    LeavePassbook lb11 = db.LeavePassbooks.Find(lprequestidNew);
                    {
                        lb11.LplTypeId = request.lTypeId;
                        lb11.LptranId = lprequestidNew;
                        lb11.LpstaffId = request.staffId;
                        lb11.LpdateFrom = request.dateFrom;
                        lb11.LpsesionDateFrom = request.sesionDateFrom;
                        lb11.LpdateTo = request.dateTo;
                        lb11.LpsesionDateTo = request.sesionDateTo;
                        lb11.LptotalCounts = request.totalCounts;
                        lb11.LpcreatedDate = DateTime.UtcNow;
                        lb11.Lpstatus = "Rejected";
                        lb11.LpapprovedBY = loginuserid;
                        lb11.LpapprovalDate = DateTime.UtcNow;
                        lb11.LpfirmId = request.firmId;
                        lb11.LpDeductStatus = true;
                    }

                    db.LeavePassbooks.AddOrUpdate(lb11);
                }

                var recordid =
                             db.LeaveRecordNews.Where(s => s.LevetypeIds == request.lTypeId && s.staffids == request.staffId).FirstOrDefault();

                var ttleave = recordid.totalLeaves + request.totalCounts;
                LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                {
                    tbll.staffids = request.staffId;
                    tbll.totalLeaves = ttleave;
                    tbll.CreatedDate = DateTime.UtcNow;
                    tbll.LevetypeIds = request.lTypeId;
                    tbll.firmsId = request.firmId;
                    tbll.IsActiveLeave = true;
                }
                db.LeaveRecordNews.AddOrUpdate(tbll);
                db.SaveChanges();

                //var lr = new TblLeaveRecord
                //{
                //    staffids = request.staffId,
                //    totalLeaves = request.totalCounts,
                //    CreatedDate = DateTime.UtcNow,
                //    LevetypeIds = request.lTypeId,
                //    firmsId = request.firmId,
                //    IsActiveLeave = false
                //};
                //db.TblLeaveRecords.Add(lr);


                TempData["notice"] = "reject";
                return RedirectToAction(ActionName);


            }
            ViewBag.approvedBY = new SelectList(db.AspNetUsers, "Id", "UserName", request.approvedBY);
            ViewBag.lTypeId = new SelectList(db.LeaveTypes, "leaveTypeId", "LeaveType1", request.lTypeId);
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", request.staffId);
            return View(request);
        }


        [Authorize]
        public ActionResult ApptMultipleLeaveRequest(string[] customerIDs)
        {
            if (customerIDs == null)
            {
                return Json("Please select at-least one checkbox.");
            }
            else
            {
                int count = 0;
                string loginuserid = GetUserId();
                foreach (string customerID in customerIDs)
                {
                    int traid = Int32.Parse(customerID);
                    var request = db.LeaveRequests.Where(s => s.tranId == traid).FirstOrDefault();
                    string enrollno = null;
                    LeaveRequest lv = db.LeaveRequests.Find(request.tranId);
                    {
                        lv.tranId = request.tranId;
                        lv.lTypeId = request.lTypeId;
                        lv.staffId = request.staffId;
                        lv.dateFrom = request.dateFrom;
                        lv.sesionDateFrom = request.sesionDateFrom;
                        lv.dateTo = request.dateTo;
                        lv.sesionDateTo = request.sesionDateTo;
                        lv.totalCounts = request.totalCounts;
                        lv.createdDate = DateTime.UtcNow;
                        lv.status = "Approved";
                        lv.approvedBY = loginuserid;
                        lv.approvalDate = DateTime.UtcNow;
                        lv.firmId = request.firmId;
                    }
                    db.LeaveRequests.AddOrUpdate(lv);

                    var recordid =
                           db.LeaveRecordNews.Where(s => s.LevetypeIds == request.lTypeId && s.staffids == request.staffId).FirstOrDefault();

                    var ttleave = recordid.totalLeaves - request.totalCounts;
                    LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                    {
                        tbll.staffids = request.staffId;
                        tbll.totalLeaves = ttleave;
                        tbll.CreatedDate = DateTime.UtcNow;
                        tbll.LevetypeIds = request.lTypeId;
                        tbll.firmsId = request.firmId;
                        tbll.IsActiveLeave = true;
                    }
                    db.LeaveRecordNews.AddOrUpdate(tbll);
                    db.SaveChanges();

                    //var lr = new TblLeaveRecord
                    //{
                    //    staffids = request.staffId,
                    //    totalLeaves = -request.totalCounts,
                    //    CreatedDate = DateTime.UtcNow,
                    //    LevetypeIds = request.lTypeId,
                    //    firmsId = request.firmId,
                    //    IsActiveLeave = false
                    //};
                    //db.TblLeaveRecords.Add(lr);


                    var lb = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();

                    if (lb != null)
                    {
                        var lprequestid = lb.LptranId;
                        LeavePassbook lb1 = db.LeavePassbooks.Find(lprequestid);
                        {
                            lb1.LplTypeId = request.lTypeId;
                            lb1.LptranId = lprequestid;
                            lb1.LpstaffId = request.staffId;
                            lb1.LpdateFrom = request.dateFrom;
                            lb1.LpsesionDateFrom = request.sesionDateFrom;
                            lb1.LpdateTo = request.dateTo;
                            lb1.LpsesionDateTo = request.sesionDateTo;
                            lb1.LptotalCounts = request.totalCounts;
                            lb1.LpcreatedDate = DateTime.UtcNow;
                            lb1.Lpstatus = "Approved";
                            lb1.LpapprovedBY = loginuserid;
                            lb1.LpapprovalDate = DateTime.UtcNow;
                            lb1.LpfirmId = request.firmId;
                            lb1.LeaveRecordIds = recordid.LeaveRecordId;
                            lb1.LpDeductStatus = true;
                        };
                        db.LeavePassbooks.AddOrUpdate(lb1);
                        db.SaveChanges();
                    }
                    TempData["notice"] = "Leave request approved successfully";
                    count++;
                    //var staffEmail = db.Staffs.Where(s => s.staffId == request.staffId).FirstOrDefault().email;
                    //string mailSendTo = staffEmail;
                    //string subject1 = "Your Leave request status";
                    //string body1 =
                    //    "Your Leave request approved successfully";
                    //var services = new MailService();
                    //services.SendMail(mailSendTo, body1, subject1, "innovativefusiontest@gmail.com");
                }

                return Json("Total :" + count + "Leave request approved successfully!");
            }
        }


        [Authorize]
        public ActionResult RejectMultipleLeaveRequest(string[] customerIDs)
        {
            if (customerIDs == null)
            {
                return Json("Please select at least one checkbox.");
            }
            else
            {
                int count = 0;
                string loginuserid = GetUserId();
                foreach (string customerID in customerIDs)
                {
                    int traid = Int32.Parse(customerID);
                    var request = db.LeaveRequests.Where(s => s.tranId == traid).FirstOrDefault();

                    var design = new LeaveRequest()
                    {
                        tranId = request.tranId,
                        lTypeId = request.lTypeId,
                        staffId = request.staffId,
                        dateFrom = request.dateFrom,
                        sesionDateFrom = request.sesionDateFrom,
                        dateTo = request.dateTo,
                        sesionDateTo = request.sesionDateTo,
                        totalCounts = request.totalCounts,
                        createdDate = DateTime.UtcNow,
                        status = "Rejected",
                        approvedBY = loginuserid,
                        approvalDate = DateTime.UtcNow,
                        firmId = request.firmId
                    };
                    db.LeaveRequests.AddOrUpdate(design);

                    var lb2 = db.LeavePassbooks.Where(a => a.LplrequestId == request.tranId).FirstOrDefault();
                    if (lb2 != null)
                    {
                        var lprequestidNew = lb2.LptranId;
                        LeavePassbook lb11 = db.LeavePassbooks.Find(lprequestidNew);
                        {
                            lb11.LplTypeId = request.lTypeId;
                            lb11.LptranId = lprequestidNew;
                            lb11.LpstaffId = request.staffId;
                            lb11.LpdateFrom = request.dateFrom;
                            lb11.LpsesionDateFrom = request.sesionDateFrom;
                            lb11.LpdateTo = request.dateTo;
                            lb11.LpsesionDateTo = request.sesionDateTo;
                            lb11.LptotalCounts = request.totalCounts;
                            lb11.LpcreatedDate = DateTime.UtcNow;
                            lb11.Lpstatus = "Rejected";
                            lb11.LpapprovedBY = loginuserid;
                            lb11.LpapprovalDate = DateTime.UtcNow;
                            lb11.LpfirmId = request.firmId;
                            lb11.LpDeductStatus = true;
                        }
                        ;
                        db.LeavePassbooks.AddOrUpdate(lb11);
                    }
                    db.SaveChanges();
                    TempData["notice"] = "reject";
                    count++;
                    //var staffEmail = db.Staffs.Where(s => s.staffId == request.staffId).FirstOrDefault().email;
                    //string mailSendTo = staffEmail;
                    //string subject1 = "Your Leave request status";
                    //string body1 = "Your Leave request rejected !!!";
                    //var services = new MailService();
                    //services.SendMail(mailSendTo, body1, subject1, "innovativefusiontest@gmail.com");
                }

                return Json("Total :" + count + " Leave request rejected successfully!");
            }
        }


        [HttpGet]
        public ActionResult LeaveSandwichSetting()
        {
            var firm = LoginUserFirmId();
            var levesetting = db.LeaveMasters.FirstOrDefault(s => s.FirmId == firm);
            if (levesetting != null)
            {
                LeaveMaster lMasters = db.LeaveMasters.Find(levesetting.LeaveMasterId);
                {
                    lMasters.LeaveMasterId = levesetting.LeaveMasterId;
                    lMasters.SandwichApply = levesetting.SandwichApply;
                    lMasters.FirmId = levesetting.FirmId;
                }
                return View(lMasters);
            }
            ViewData["none"] = "none";
            return View();
        }
        #region ManuallyCheckin
        public ActionResult LeaveSandwichSetting(int? Attendid, bool? Status)
        {
            var firmId = LoginUserFirmId();
            if (Attendid != null && Status != null)
            {
                var data = db.LeaveMasters.FirstOrDefault(x => x.LeaveMasterId == Attendid);
                if (data != null)
                {
                    LeaveMaster lm = db.LeaveMasters.Find(Attendid);
                    {
                        lm.SandwichApply = Status;
                        db.LeaveMasters.AddOrUpdate(lm);
                    }
                    db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LeaveMaster lmaster = new LeaveMaster();
                    {
                        lmaster.SandwichApply = Status;
                        lmaster.FirmId = firmId;
                        db.LeaveMasters.Add(lmaster);
                    }
                    db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion

        #region ------- EmployeeAverageTime --------
        [HttpGet]
        public ActionResult EmployeeAverageTime()
        {
            var firm = LoginUserFirmId();
            TblAverageTimeModel fam = new TblAverageTimeModel
            {
                StaffListTime =
                    db.StaffDropdownList().Where(s => s.firmId == firm).ToList().Select(a => new SelectListItem()
                    {
                        Text = a.fullName,
                        Value = a.staffid.ToString()
                    }),
            };
            return View(fam);
        }

        [HttpPost]
        public ActionResult EmployeeAverageTime(TblAverageTimeModel assignShiftModel)
        {
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var staffid in assignShiftModel.empid)
                    {
                        var staffidnew = Convert.ToInt32(staffid);

                        var data = db.TblAverageTimes.FirstOrDefault(a => a.staffid == staffidnew);
                        if (data == null)
                        {
                            var assignShift = new TblAverageTime
                            {
                                staffid = staffidnew,
                                hourtime = assignShiftModel.hourtime,
                                firmid = firm
                            };
                            db.TblAverageTimes.Add(assignShift);
                            db.SaveChanges();
                            TempData["notice"] = "success";
                        }
                        else
                        {
                            TempData["notice"] = "exist";
                        }
                    }
                }
                return RedirectToAction("UpdateSetting");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult AverageTimeIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.TblAverageTimes.Where(a => a.firmid == firm).Include(s => s.Staff).ToList());
        }

        [HttpGet]
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EditAverageTime(int? id)
        {
            var firm = LoginUserFirmId();
            TblAverageTimeModel model = new TblAverageTimeModel();
            var data = db.TblAverageTimes.FirstOrDefault(a => a.AverageId == id);
            if (data != null)
            {
                model.AverageId = data.AverageId;
                model.hourtime = data.hourtime;
                model.firmid = (int)data.firmid;
                model.staffid = (int)data.staffid;
                model.StaffListTime = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditAverageTime(TblAverageTimeModel mmodel)
        {
            if (ModelState.IsValid)
            {
                TblAverageTime bts = db.TblAverageTimes.Find(mmodel.AverageId);
                {
                    bts.AverageId = (int)mmodel.AverageId;
                    bts.staffid = mmodel.staffid;
                    bts.firmid = mmodel.firmid;
                    bts.hourtime = mmodel.hourtime;
                }
                db.TblAverageTimes.AddOrUpdate(bts);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("UpdateSetting");
            }
            return View(mmodel);
        }

        public ActionResult DeleteAverageTime(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblAverageTime bts = db.TblAverageTimes.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteAverageTime(int id)
        {
            try
            {
                TblAverageTime bts = db.TblAverageTimes.Find(id);
                db.TblAverageTimes.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("UpdateSetting");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("UpdateSetting");
            }
        }

#endregion

        #region -- AverageTimeForAttendanceSheet ---

        [HttpGet]
        public ActionResult EmployeeTimeForAttendanceSheet()
        {
            var firm = LoginUserFirmId();
            TblAverageTimeModel fam = new TblAverageTimeModel
            {
                StaffListTime =
                    db.StaffDropdownList().Where(s => s.firmId == firm).ToList().Select(a => new SelectListItem()
                    {
                        Text = a.fullName,
                        Value = a.staffid.ToString()
                    }),
            };
            return View(fam);
        }

        [HttpPost]
        public ActionResult EmployeeTimeForAttendanceSheet(TblAverageTimeModel assignShiftModel)
        {
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var staffid in assignShiftModel.empid)
                    {
                        var staffidnew = Convert.ToInt32(staffid);

                        var data = db.TblAverageTimeForAttendanceSheets.FirstOrDefault(a => a.staffid == staffidnew);
                        if (data == null)
                        {
                            var assignShift = new TblAverageTimeForAttendanceSheet
                            {
                                staffid = staffidnew,
                                hourtime = assignShiftModel.hourtime,
                                firmid = firm
                            };
                            db.TblAverageTimeForAttendanceSheets.Add(assignShift);
                            db.SaveChanges();
                            TempData["notice"] = "success";
                        }
                        else
                        {
                            TempData["notice"] = "exist";
                        }
                    }
                }
                return RedirectToAction("UpdateSetting");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult AverageTimeForAttendanceSheetIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.TblAverageTimeForAttendanceSheets.Where(a => a.firmid == firm).Include(s => s.Staff).ToList());
        }

        [HttpGet]
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EditAverageTimeForAttendanceSheet(int? id)
        {
            var firm = LoginUserFirmId();
            TblAverageTimeModel model = new TblAverageTimeModel();
            var data = db.TblAverageTimeForAttendanceSheets.FirstOrDefault(a => a.AverageId == id);
            if (data != null)
            {
                model.AverageId = data.AverageId;
                model.hourtime = data.hourtime;
                model.firmid = (int)data.firmid;
                model.staffid = (int)data.staffid;
                model.StaffListTime = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditAverageTimeForAttendanceSheet(TblAverageTimeModel mmodel)
        {
            if (ModelState.IsValid)
            {
                TblAverageTimeForAttendanceSheet bts = db.TblAverageTimeForAttendanceSheets.Find(mmodel.AverageId);
                {
                    bts.AverageId = (int)mmodel.AverageId;
                    bts.staffid = mmodel.staffid;
                    bts.firmid = mmodel.firmid;
                    bts.hourtime = mmodel.hourtime;
                }
                db.TblAverageTimeForAttendanceSheets.AddOrUpdate(bts);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("UpdateSetting");
            }
            return View(mmodel);
        }

        public ActionResult DeleteAverageTimeForAttendanceSheet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblAverageTimeForAttendanceSheet bts = db.TblAverageTimeForAttendanceSheets.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteAverageTimeForAttendanceSheet(int id)
        {
            try
            {
                TblAverageTimeForAttendanceSheet bts = db.TblAverageTimeForAttendanceSheets.Find(id);
                db.TblAverageTimeForAttendanceSheets.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("UpdateSetting");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("UpdateSetting");
            }
        }


        #endregion

    }
}


public class DemoTest
{
    public int StaffIdTest { get; set; }
}
