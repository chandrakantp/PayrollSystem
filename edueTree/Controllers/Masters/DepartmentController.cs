using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;
using PagedList;

namespace edueTree.Controllers.Masters
{
    [Authorize]
    public class DepartmentController : BaseController
    {
        #region ------- DBContext -------

        private dbContainer db = new dbContainer();

        #endregion

        #region ----- PagedList Index....
        
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {

            var firm = LoginUserFirmId();
            return View(db.Departments.Where(a => a.firmId == firm).ToList());

            //int pageSize = 10;
            //int pageIndex = 1;
            //pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            //ViewBag.CurrentSort = sortOrder;

            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "deptName" : sortOrder;

            //IPagedList<Department> dept = null;
            //var firm = LoginUserFirmId();
            //switch (sortOrder)
            //{
            //    case "deptName":
            //        if (sortOrder.Equals(CurrentSort))
            //            dept = db.Departments.Where(x => x.firmId == firm).OrderByDescending(m => m.deptName).ToPagedList(pageIndex, pageSize);
            //        else
            //            dept = db.Departments.OrderBy(m => m.deptName).Where(x => x.firmId == firm).ToPagedList(pageIndex, pageSize);
            //        break;

            //}
            //return View(dept);
        } 
        #endregion 
        
        #region ------- Details ---------

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(department);
        }

        #endregion 

        #region ----- Dept Create -------
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentModel department)
        {
            var firmIdnew = LoginUserFirmId();
            var data = db.Departments.FirstOrDefault(x => x.deptName == department.deptName && x.firmId==firmIdnew);
            if (data == null)
            {
                if (ModelState.IsValid)
                {
                    var lr = new Department()
                    {
                        deptName = department.deptName,
                        firmId=firmIdnew
                    };
                    db.Departments.Add(lr);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("create");
                }
            }
            else
            {
                TempData["notice"] = "error";
                return RedirectToAction("create");
            }

            return View(department);
        }

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            DepartmentModel dept = new DepartmentModel();
            dept.deptName = department.deptName;
            dept.deptId = department.deptId;
            dept.firmId = department.firmId;


            if (dept == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(dept);
        }

        #endregion 

        #region ---- Edit Post Method ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "deptId,deptName,firmId")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Create");
            }
            return View(department);
        }

        #endregion  

        #region ---- Delete Record ------

        // GET: /Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(department);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Department department = db.Departments.Find(id);
                db.Departments.Remove(department);
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

        #region ------- Despose ---------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion 
              
        #region ---- Assign Computer ----
        public ActionResult AssignedComputer()
        {
            var compModel = new ComputerAssingedModel
            {
                StaffList = db.Staffs.ToList().Select(a => new SelectListItem()
                {
                    Text = a.staffCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                })
            };
            return View(compModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignedComputer(ComputerAssingedModel computer)
        {
            if (ModelState.IsValid)
            {
                var lr = new ComputerAssinged()
                {
                    staffId = computer.staffId,
                    IPAddress = computer.IPAddress,
                    MachineName = computer.MachineName,
                    Username = computer.Username
                };

                db.ComputerAssingeds.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AssignedComputer");
            }

            return View(computer);
        }
        
        #endregion 

        #region -- AsignComputerReport --

        public ActionResult AssignedComputerReport()
        {
            return View(db.ComputerAssingeds.ToList());
        }
        
        #endregion 

        #region --- UserLogTimeReport ---
        public ActionResult UserLogTimeReport(DateTime? datepicker)
        {
            int staffId = LoginEmployeeId();
            if (datepicker == null)
            {
                DateTime start = DateTime.Now;
                var getList = db.GetNetDurationTodaysHistory(start, staffId);
                var logModel = getList.Select(a => a.unLockTime != null ? (a.totalDuration != null ? (a.lockTime != null ? (a.staffId != null ? new LogTimeModel()
                {
                    staffId = (int)a.staffId,
                    MachineName = a.machineName,
                    lockTime = (DateTime)a.lockTime,
                    unLockTime = (DateTime)a.unLockTime,
                    totalDuration = (TimeSpan)a.totalDuration
                } : null) : null) : null) : null).ToList();

                return View(logModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                var getList = db.GetNetDurationTodaysHistory(start, staffId);
                var logModel = getList.Select(a => a.totalDuration != null ? (a.unLockTime != null ? (a.lockTime != null ? (a.staffId != null ? new LogTimeModel()
                {
                    staffId = (int)a.staffId,
                    MachineName = a.machineName,
                    lockTime = (DateTime)a.lockTime,
                    unLockTime = (DateTime)a.unLockTime,
                    totalDuration = (TimeSpan)a.totalDuration
                } : null) : null) : null) : null).ToList();

                return View(logModel);
            }
        } 
        #endregion 
    }
}
