using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class ReportingController : BaseController
    {
        #region -------------- DbContext ------------------
        dbContainer db = new dbContainer();
        #endregion 

        #region ---------------- Index --------------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var data = db.GetReporting(firm);
            var reportmodel = data.Select(a => new ReportingModel() {ReportingId = a.reportingid,StaffName =  a.fname+" "+a.mname+" "+a.lname ,EmailReportingTo = a.emailReportingto,RptMgrName = a.firstName+" "+a.middleName+" "+a.lastName, RoleName = a.EmployeeRole});
          
          
            return View(reportmodel);
        }

        #endregion 

        #region --------------- Details -------------------
        
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 

        #region ---------------- Create -------------------
        public ActionResult Create()
        {
            var firm = LoginUserFirmId();
            ReportingModel rm = new ReportingModel();

            rm.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });

            rm.StaffListNew = db.Staffs.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });

            return View(rm);
        }
        
        [HttpPost]
        public ActionResult Create(ReportingModel reportingModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = db.Staffs.FirstOrDefault(a => a.staffId == reportingModel.StaffId);
                    if (data != null)
                    {
                        data.emailReportingTo = reportingModel.EmailReportingTo;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    var userrole = db.UserRoles.FirstOrDefault(a => a.userId == data.userId);
                    if (userrole != null)
                    {
                        userrole.RoleId = reportingModel.RoleId;
                        db.Entry(userrole).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    foreach (var i in reportingModel.ReportingManagerId)
                    {
                        if (i != null)
                        {
                            int reportingid =Convert.ToInt32( i);
                            var allreadyExist =
                                db.Reportings.FirstOrDefault(
                                    a => a.StaffId == reportingModel.StaffId && a.ReportingManagerId == reportingid);
                            if (allreadyExist == null)
                            {
                                var reporting = new Reporting
                                {
                                    ReportingManagerId = Convert.ToInt32(reportingid), 
                                    StaffId = reportingModel.StaffId, 
                                    Isdeleted = false, 
                                    CreatedBy = DateTime.Now
                                };
                                db.Reportings.Add(reporting);
                                db.SaveChanges();
                              
                            }
                        }
                        TempData["notice"] = "success";
                    }
                }
                return RedirectToAction("Create", "Reporting");
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create", "Reporting");
            }
        }

        #endregion 

        #region ---------------- Delete -------------------
        //
        // GET: /Reporting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Reporting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region ---------- GetRoleRoporting ---------------
        public ActionResult GetRoleReporting(int? id)
        {
            var firm = LoginUserFirmId();
            var staff = db.Staffs.FirstOrDefault(a => a.staffId == id);
            var userrole = db.UserRoles.FirstOrDefault(a => a.userId == staff.userId);
            var staffReportingModel = new ReportingModel();
            if (userrole != null) staffReportingModel.RoleId = userrole.RoleId;
            if (staff != null) staffReportingModel.EmailReportingTo = staff.emailReportingTo;
            staffReportingModel.RoleListnew = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
            staffReportingModel.Isdeleted = false;
            if (userrole != null) staffReportingModel.UserId = userrole.userId;
            staffReportingModel.StaffListNew =
                db.Staffs.ToList()
                    .Where(s => s.firmId == firm && s.isActive == true)
                    .Select(a => new SelectListItem()
                    {
                        Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                        Value = a.staffId.ToString()
                    });
            //var reporting = db.Reportings.Where(a => a.StaffId == id).ToList();

            //var myList = new string[reporting.Count];
            //var i = 0;
            //foreach (var staffid in reporting)
            //{
            //    myList[i++] = staffid.ReportingManagerId.ToString();
            //}
            
            //staffReportingModel.ReportingManagerId = myList;
            return PartialView("_GetRole", staffReportingModel);
        }
        #endregion

        #region ------- EmailReportingForm Get Post--------

        [HttpGet]
        public ActionResult EmailReportingForm()
        {
            var firm = LoginUserFirmId();
            FormEvaluatorModel fam = new FormEvaluatorModel
            {
                StaffListTwo =
                    db.StaffDropdownList().Where(s => s.firmId == firm).ToList().Select(a => new SelectListItem()
                    {
                        Text = a.fullName,
                        Value = a.staffid.ToString()
                    }),
                StaffListOne =
                    db.Staffs.ToList()
                        .Where(s => s.firmId == firm && s.isActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                            Value = a.staffId.ToString()
                        })
            };

            return View(fam);
        }

        [HttpPost]
        public ActionResult EmailReportingForm(FormEvaluatorModel assignShiftModel)
        {
            var login = LoginEmployeeId();
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var staffid in assignShiftModel.empid)
                    {
                        foreach (var evaluid in assignShiftModel.evaluatorEmpid)
                        {
                            var staffidnew = Convert.ToInt32(staffid);
                            var evaidnew = Convert.ToInt32(evaluid);

                            if (staffidnew != evaidnew)
                            {
                                 var data = db.Reportings.FirstOrDefault(a => a.StaffId == staffidnew && a.ReportingManagerId == evaidnew);
                            if (data == null)
                            {
                                var assignShift = new Reporting
                                {
                                    StaffId = staffidnew,
                                    ReportingManagerId = evaidnew,
                                    firmid = firm
                                };
                                db.Reportings.Add(assignShift);
                                db.SaveChanges();
                                TempData["notice"] = "success";
                            }
                            else
                            {
                                TempData["notice"] = "exist";
                            }
                            }
                            else
                            {
                                TempData["notice"] = "notassign";
                            }
                        }
                    }
                }
                return RedirectToAction("EmailReportingForm");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region ------------- EmailReportingResult --------
        public ActionResult EmailReportingResult(int? staffid, int? staffidEmp)
        {
            var firm = LoginUserFirmId();            
            ViewBag.staffid =
               db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });

            if (staffid != null && staffidEmp ==null)
            {
                var data = db.Reportings.Include(s => s.Staff).Include(s => s.Staff1).Where(s => s.firmid == firm).Where(s => s.ReportingManagerId == staffid).Where(s => s.Staff.isActive == true && s.Staff.isActive == true);
                return View(data.ToList());
            }
            else if (staffidEmp != null && staffid==null)
            {
                var data = db.Reportings.Include(s => s.Staff).Include(s => s.Staff1).Where(s => s.firmid == firm).Where(s => s.StaffId == staffidEmp).Where(s => s.Staff.isActive == true && s.Staff.isActive == true);
                return View(data.ToList());
            }
            else
            {
                var data = db.Reportings.Include(s => s.Staff).Include(s => s.Staff1).Where(s => s.firmid == firm).Where(s => s.Staff.isActive == true && s.Staff.isActive == true); ;
                return View(data.ToList());
            }
        }
        #endregion

        #region ------------- EmailReportingEdit ----------
        [HttpGet]
        public ActionResult EmailReportingEdit(int? id)
        {
            var firm = LoginUserFirmId();
            EmailReportingModel model = new EmailReportingModel();
            var data = db.Reportings.FirstOrDefault(a => a.ReportingId == id);
            if (data != null)
            {
                model.Empname = data.Staff.schoolCode + " " + data.Staff.firstName + " " + data.Staff.lastName;
                model.ReportingId = data.ReportingId;
                model.StaffId = (int)data.StaffId;
                model.ReportingManagerId = (int)data.ReportingManagerId;
                model.firmid = firm;
                model.Reportinglist =
                    db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                    {
                        Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                         Value = a.staffId.ToString()
                    });

                //    StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                //{
                //    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                //    Value = a.staffId.ToString()
                //})
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EmailReportingEdit(EmailReportingModel mmodel)
        {
            Reporting bts = db.Reportings.Find(mmodel.ReportingId);
            {
                bts.ReportingId = mmodel.ReportingId;
                bts.StaffId = mmodel.StaffId;
                bts.ReportingManagerId = mmodel.ReportingManagerId;
                bts.firmid = mmodel.firmid;
            }
            db.Reportings.AddOrUpdate(bts);
            db.SaveChanges();
            TempData["notice"] = "update";
            return RedirectToAction("EmailReportingForm");
        }
        #endregion

        #region --------- DeleteEmailReporting ------------
        public ActionResult DeleteEmailReporting(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reporting bts = db.Reportings.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteEmailReporting(int id)
        {
            try
            {
                Reporting bts = db.Reportings.Find(id);
                db.Reportings.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("EmailReportingForm");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("EmailReportingForm");
            }
        }

        #endregion
        
    }
}
