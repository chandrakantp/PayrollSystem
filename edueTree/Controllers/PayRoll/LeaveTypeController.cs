using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.PayRoll
{
    [Authorize]
    public class LeaveTypeController : BaseController
    {
        #region ----- DbContext -----
        private readonly dbContainer db = new dbContainer();
        #endregion

        #region ------- Index -------

        // GET: /LeaveType/
        public ActionResult Index()
        {
            ViewBag.DepartmentList = new SelectList(db.Departments.ToList(), "deptId", "deptName");

            var firm = LoginUserFirmId();
            var leavetypes = db.LeaveTypes;
            return View(leavetypes.Where(a => a.firmId == firm).ToList());
        }

        #endregion

        #region ------ Details ------
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leavetype = db.LeaveTypes.Find(id);
            if (leavetype == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leavetype);
        }

        #endregion

        #region ------ Create -------

        // GET: /LeaveType/Create
        public ActionResult Create()
        {
            var firmId = LoginUserFirmId();
            //var model = new LeaveTypeModel()
            //{
            //    DepartmentList = new SelectList(db.Departments.Where(x => x.firmId == firmId), "deptId", "deptName", firmId)
            //};
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeModel leavetype)
        {
            var firmId = LoginUserFirmId();

            var data =
                db.LeaveTypes.Where(
                    s => s.firmId == firmId && s.LeaveType1 == leavetype.LeaveType1);
            if (data.Any() != false)
            {
                TempData["notice"] = "exist";
                return RedirectToAction("Create");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var lr = new LeaveType()
                    {
                        LeaveType1 = leavetype.LeaveType1,
                        counts = leavetype.counts,
                        firmId = LoginUserFirmId(),
                        deptId = leavetype.deptId
                    };
                    db.LeaveTypes.Add(lr);
                    db.SaveChanges();

                    var model1 = db.Staffs.Where(f => f.firmId == firmId);

                    foreach (var staff in model1.ToList())
                    {
                        var lr1 = new TblLeaveRecord
                        {
                            staffids = staff.staffId,
                            totalLeaves = 0,
                            CreatedDate = DateTime.UtcNow,
                            LevetypeIds = lr.leaveTypeId,
                            firmsId = firmId,
                            IsActiveLeave = true
                        };
                        db.TblLeaveRecords.Add(lr1);
                        db.SaveChanges();

                        var lrNew = new LeaveRecordNew
                        {
                            staffids = staff.staffId,
                            totalLeaves = 0,
                            CreatedDate = DateTime.UtcNow,
                            LevetypeIds = lr.leaveTypeId,
                            firmsId = firmId,
                            IsActiveLeave = true
                        };
                        db.LeaveRecordNews.Add(lrNew);
                        db.SaveChanges();

                        var lrp = new LeavePassbook
                        {
                            LpstaffId = staff.staffId,
                            LptotalCounts = 0,
                            LpcreatedDate = DateTime.UtcNow,
                            LplTypeId = lr.leaveTypeId,
                            LpfirmId = firmId,
                            LeaveRecordIds = lr1.LeaveRecordId
                        };
                        db.LeavePassbooks.Add(lrp);
                    }
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Create");
                }
            }
            // ViewBag.DepartmentList = new SelectList(db.Departments.ToList(), "deptId", "deptName");
            //leavetype.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firmId), "deptId",
            //             "deptName");

            return View(leavetype);
        }

        #endregion

        #region ------- Edit --------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveType leavetype = db.LeaveTypes.Find(id);
            if (leavetype == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            var model = new LeaveTypeModel
            {
                deptId = leavetype.deptId,
                firmId = leavetype.firmId,
                leaveTypeId = leavetype.leaveTypeId,
                LeaveType1 = leavetype.LeaveType1,
                counts = leavetype.counts,
                DepartmentList =
                    new SelectList(db.Departments.Where(a => a.firmId == leavetype.firmId), "deptId", "deptName")
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeModel model)
        {
            if (ModelState.IsValid)
            {
                LeaveType leavetype = db.LeaveTypes.Find(model.leaveTypeId);
                leavetype.deptId = model.deptId;
                leavetype.firmId = model.firmId;
                leavetype.LeaveType1 = model.LeaveType1;
                leavetype.counts = model.counts;

                db.LeaveTypes.AddOrUpdate(leavetype);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Create");
            }
            model.DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == model.firmId), "deptId", "deptName");
            return View(model);
        }

        #endregion

        #region ------- Delete ------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveType leavetype = db.LeaveTypes.Find(id);
            if (leavetype == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(leavetype);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                LeaveType leavetype = db.LeaveTypes.Find(id);
                db.LeaveTypes.Remove(leavetype);


                var lrecord = db.TblLeaveRecords.Where(a => a.LevetypeIds == id);
                foreach (var ex in lrecord)
                {                                  
                    if (ex.totalLeaves == 0)
                    {
                        var lprequestLr = ex.LeaveRecordId;
                        TblLeaveRecord lrRecord = db.TblLeaveRecords.Find(lprequestLr);
                        db.TblLeaveRecords.Remove(lrRecord);
                    }
                }

                var lb = db.LeavePassbooks.Where(a => a.LplTypeId == id);
                foreach (var ex1 in lb)
                {
                    if (ex1.LptotalCounts == 0)
                    {
                        var lprequestid = ex1.LptranId;
                        LeavePassbook lr = db.LeavePassbooks.Find(lprequestid);
                        db.LeavePassbooks.Remove(lr);
                    }
                }

                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("Create");
            }
            catch (Exception)
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("Create");
            }
        }

        #endregion

        #region ------ Dispose ------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
