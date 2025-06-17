using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class AssignShiftController : BaseController
    {

        #region ------------- DbContext --------------
        dbContainer db = new dbContainer();
        #endregion

        #region --------------- Index ----------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var data = db.GetAllAssignShiftByEmployee(firm);

            var assignshiftmodel = data.Select(a => new AssignShiftModel()
            {
                StaffName = a.firstName + " " + a.middleName + " " + a.lastName,
                ShiftName = a.shiftName + " [ " + a.startTime + " " + a.endTime + " ]",
                Firmid = a.firmId,
                Fromdate = a.Fromdate,
                Todate = a.Todate,
                ShiftassignId = a.ShiftassignId

            }).ToList();
            return View(assignshiftmodel);
        }

        #endregion

        #region ----------- Details ------------------
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion

        #region --------------- Create ---------------
        public ActionResult Create()
        {         
            var firm = LoginUserFirmId();
            AssignShiftModel assignShiftModel = new AssignShiftModel();
            assignShiftModel.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
               {
                   Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                   Value = a.staffId.ToString()
               });
            assignShiftModel.ShiftList = db.ShiftMasters.Where(a => a.firmId == firm).Select(a => new SelectListItem()
            {
                Text = a.shiftName + " [" + a.startTime + " " + a.endTime + " ]",
                Value = a.shiftId.ToString()
            });
            return View(assignShiftModel);
        }

        [HttpPost]
        public ActionResult Createpost(AssignShiftModel assignShiftModel)
        {
            var login = LoginEmployeeId();
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var staffid in assignShiftModel.StaffId)
                    {
                        foreach (var shiftid in assignShiftModel.ShiftId)
                        {
                            var staffidnew = Convert.ToInt32(staffid);
                            var shiftidnew = Convert.ToInt32(shiftid);

                            var data = db.AssignShifts.FirstOrDefault(a => a.StaffId == staffidnew && a.ShiftId == shiftidnew && a.Fromdate == assignShiftModel.Fromdate && a.Todate == assignShiftModel.Todate && a.Isdeleted == false);
                            if (data == null)
                            {
                                var assignShift = new AssignShift
                                {
                                    CreatedBy = login,
                                    Fromdate = assignShiftModel.Fromdate,
                                    Todate = assignShiftModel.Todate,
                                    ShiftId = shiftidnew,
                                    StaffId = staffidnew,
                                    Isdeleted = false
                                };
                                db.AssignShifts.Add(assignShift);
                                db.SaveChanges();
                                TempData["notice"] = "success";
                            }
                            else
                            {
                                TempData["notice"] = "exist";
                            }
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region ---------------- Edit ----------------
        public ActionResult Edit(int id)
        {
            var firm = LoginUserFirmId();
            AssignShiftModel assignShiftModel = new AssignShiftModel();
            var data = db.AssignShifts.FirstOrDefault(a => a.ShiftassignId == id);
            if (data != null)
            {
                assignShiftModel.ShiftId1 = data.ShiftId;
                assignShiftModel.StaffId1 = data.StaffId;
                assignShiftModel.ShiftassignId = data.ShiftassignId;
                assignShiftModel.Fromdate = data.Fromdate;
                assignShiftModel.Todate = data.Todate;
                assignShiftModel.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
                assignShiftModel.ShiftList = db.ShiftMasters.Where(a => a.firmId == firm).Select(a => new SelectListItem()
                {
                    Text = a.shiftName + " [" + a.startTime + " " + a.endTime + " ]",
                    Value = a.shiftId.ToString()
                });

            }
            return View(assignShiftModel);
        }
        
        [HttpPost]
        public ActionResult Edit(AssignShiftModel assignShiftModel)
        {
            try
            {
                var data = db.AssignShifts.FirstOrDefault(a => a.ShiftassignId == assignShiftModel.ShiftassignId);
                if (data != null)
                {
                    data.Fromdate = assignShiftModel.Fromdate;
                    data.Todate = assignShiftModel.Todate;
                    data.ShiftId = assignShiftModel.ShiftId1;
                    data.StaffId = assignShiftModel.StaffId1;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "error";
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region -------------- Delete ----------------
        public ActionResult Delete(int id)
        {
            var assignShiftModel = new AssignShiftModel {ShiftassignId = id};
            return View(assignShiftModel);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(AssignShiftModel assignShiftModel)
        {
            try
            {
                var data = db.AssignShifts.FirstOrDefault(a => a.ShiftassignId == assignShiftModel.ShiftassignId);
                if (data != null)
                {
                    var bts = db.AssignShifts.Find(assignShiftModel.ShiftassignId);
                    db.AssignShifts.Remove(bts);
                    db.SaveChanges();
                    TempData["notice"] = "delete";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "error";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region ----- GetAssignShift to Employee -----
        public ActionResult GetAssignShiftByEmployeeId(int staffId)
        {
            var data = db.AssignShifts.Where(a => a.StaffId == staffId).ToList();

            return PartialView(data);
        }

        #endregion

        #region ------ AllassignShiftResult ----------
        public ActionResult AllassignShiftResult(int? staffid)
        {
            var firm = LoginUserFirmId();
            ViewBag.staffid =
             db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
             {
                 Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                 Value = a.staffId.ToString()
             });
            var data = db.GetAllAssignShiftByEmployee(firm);
            if (staffid == null)
            {
                var assignshiftmodel = data.Where(a => a.Isdeleted == false).Select(a => new AssignShiftModel()
                {
                    StaffName = a.firstName + " " + a.middleName + " " + a.lastName,
                    ShiftName = a.shiftName + " [ " + a.startTime + " " + a.endTime + " ]",
                    Firmid = a.firmId,
                    Fromdate = a.Fromdate,
                    Todate = a.Todate,
                    ShiftassignId = a.ShiftassignId

                }).ToList();
                return View(assignshiftmodel);
            }
            else
            {
                var assignshiftmodel = data.Where(a => a.Isdeleted == false && a.StaffId == staffid).Select(a => new AssignShiftModel()
                {
                    StaffName = a.firstName + " " + a.middleName + " " + a.lastName,
                    ShiftName = a.shiftName + " [ " + a.startTime + " " + a.endTime + " ]",
                    Firmid = a.firmId,
                    Fromdate = a.Fromdate,
                    Todate = a.Todate,
                    ShiftassignId = a.ShiftassignId

                }).ToList();
                return View(assignshiftmodel);
            }

        }
        #endregion
    }
}
