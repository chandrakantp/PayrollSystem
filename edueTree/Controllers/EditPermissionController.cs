using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class EditPermissionController : BaseController
    {
        #region ----------- DbContext -----------
        dbContainer db = new dbContainer();
        #endregion

        #region ------------ Index --------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var data = (from a in db.Staffs
                        join b in db.EditPermissions on a.staffId equals b.StaffId
                        where a.firmId == firm
                        select new {a.schoolCode, a.firstName, a.middleName,a.isActive, a.lastName, b.GeneralInfo, b.AppAttendance, b.LeaveApproval, b.ManualAttendance, b.PermissionId, b.StaffId,b.IsProbationLeaveApp }).ToList();
            var editPermissionModel = data.Select(a => new EditPermissionModel() { AppAttendance = (bool)a.AppAttendance, StaffName = a.schoolCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName, GeneralInfo = (bool)a.GeneralInfo, IsActive = (bool)a.isActive, LeaveApproval = a.LeaveApproval, ManualAttendance = (bool)a.ManualAttendance, PermissionId = a.PermissionId, IsProbationLeaveApp = (bool) a.IsProbationLeaveApp }).ToList();

            return View(editPermissionModel);
        }

        #endregion 
        
        #region ----------- Details -------------
        //
        // GET: /EditPermission/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 
       
        #region  --------------- Create ----------
        public ActionResult Create(int? staffId)
        {
            var firm = LoginUserFirmId();          
            var editPermissionModel = new EditPermissionModel()
            {
                StaffList = db.Staffs.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                })
            };
            return View(editPermissionModel);
        }

      
        [HttpPost]
        public ActionResult Create(EditPermissionModel editPermissionModel)
        {
            try
            {
                var userid = LoginEmployeeId();
                var  editPermission=new EditPermission();
                var data = db.EditPermissions.FirstOrDefault(a => a.StaffId == editPermissionModel.StaffId);
                var staffinfo = db.Staffs.FirstOrDefault(a=>a.staffId==editPermissionModel.StaffId);
                if (staffinfo != null)
                {
                    staffinfo.isActive = editPermissionModel.IsActive;
                    db.Entry(staffinfo).State = EntityState.Modified;                                
                }
                if (data != null)
                {
                    data.LeaveApproval = editPermissionModel.LeaveApproval;
                    data.GeneralInfo = editPermissionModel.GeneralInfo;
                    data.ManualAttendance = editPermissionModel.ManualAttendance;
                    data.IsProbationLeaveApp = editPermissionModel.IsProbationLeaveApp;
                    data.AppAttendance = editPermissionModel.AppAttendance;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Create");
                }
                if (ModelState.IsValid)
                {
                    editPermission.AppAttendance = editPermissionModel.AppAttendance;
                    editPermission.CreatedBy = userid;
                    editPermission.GeneralInfo = editPermissionModel.GeneralInfo;
                    editPermission.IsProbationLeaveApp = editPermissionModel.IsProbationLeaveApp;
                    editPermission.LeaveApproval = editPermission.LeaveApproval;
                    editPermission.ManualAttendance = editPermissionModel.ManualAttendance;
                    editPermission.StaffId = editPermissionModel.StaffId;
                    db.EditPermissions.Add(editPermission);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        } 
        #endregion 

        #region ----------- Edit ----------------
        public ActionResult Edit(int id)
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion  
      
        #region ----------- Delete --------------

        //
        // GET: /EditPermission/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /EditPermission/Delete/5
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

        #region ----- Get-user-permission -------

        public ActionResult Getuserpermission(int? id)
        {
            EditPermissionModel editPermission = new EditPermissionModel();
            var staffinfo = db.Staffs.FirstOrDefault(a => a.staffId == id);
            if (staffinfo != null)
            {
                
                    if (staffinfo.isActive != null) editPermission.IsActive = (bool)staffinfo.isActive;
            }
            var data = db.EditPermissions.FirstOrDefault(a => a.StaffId == id);
            if (data != null)
            {
                editPermission.LeaveApproval = data.LeaveApproval;
                if (data.GeneralInfo != null) editPermission.GeneralInfo = (bool)data.GeneralInfo;
                if (data.ManualAttendance != null) editPermission.ManualAttendance = (bool)data.ManualAttendance;
                if (data.IsProbationLeaveApp != null) editPermission.IsProbationLeaveApp = (bool)data.IsProbationLeaveApp;
                //if (data.IsActive != null) editPermission.IsActive = (bool)data.IsActive;
                if (data.AppAttendance != null) editPermission.AppAttendance = (bool)data.AppAttendance;
                return PartialView("_Getuserpermision", editPermission);
            }
            editPermission.LeaveApproval = false;
            editPermission.AppAttendance = false;
            editPermission.GeneralInfo = false;
            editPermission.IsProbationLeaveApp = false;
            //editPermission.IsActive = false;
            editPermission.ManualAttendance = false;        
            return PartialView("_Getuserpermision", editPermission);

        } 
        #endregion

        #region ------ Manage Permission --------

        public ActionResult ManagePermission()
        {
            var firmid = LoginUserFirmId();
            var data = db.ManagePermission(firmid);
            var editpermission = data.Select(a => new EditPermissionModel() { StaffId = a.staffId, StaffName = a.staffCode + " : " + a.StaffName, GeneralInfo = a.GeneralInfo != null && (bool)a.GeneralInfo, AppAttendance = a.AppAttendance != null && (bool)a.AppAttendance, ManualAttendance = a.ManualAttendance != null && (bool)a.ManualAttendance, IsActive = a.IsActive != null && (bool)a.IsActive, IsProbationLeaveApp = a.IsProbationLeaveApp != null && (bool)a.IsProbationLeaveApp });
            return View(editpermission.ToList());
        }



        public ActionResult ManagePermissionpost(string Attendid, bool Status)
        {
            string[] words = Attendid.Split(' ');
            string type = words[0];
            int staffid = Convert.ToInt32(words[1]);
            bool status = Status;
            if (type != "IsActive")
            {
                var data = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffid);
                if (data != null)
                {
                    if (type == "generalinfo")
                    {
                        data.GeneralInfo = status;
                    }
                    if (type == "AppAttendance")
                    {
                        data.AppAttendance = status;
                    }
                    if (type == "ManualAttendance")
                    {
                        data.ManualAttendance = status;
                    }

                    if (type == "IsProbationLeaveApp")
                    {
                        data.IsProbationLeaveApp = status;
                    }
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    EditPermission objedit = new EditPermission();
                    objedit.StaffId = staffid;
                    if (type == "generalinfo")
                    {
                        objedit.GeneralInfo = status;
                    }
                    if (type == "AppAttendance")
                    {
                        objedit.AppAttendance = status;
                    }
                    if (type == "ManualAttendance")
                    {
                        objedit.ManualAttendance = status;
                    }

                    if (type == "IsProbationLeaveApp")
                    {
                        objedit.IsProbationLeaveApp = status;
                    }
                    db.EditPermissions.Add(objedit);
                    db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var data = db.Staffs.FirstOrDefault(a => a.staffId == staffid);
                if (data != null)
                {
                    if (status == false)
                    {
                        var orDefault = db.Reportings.Where(s => s.StaffId == staffid).ToList();
                        foreach (var ex in orDefault)
                        {
                            Reporting bts = db.Reportings.Find(ex.ReportingId);
                            db.Reportings.Remove(bts);
                            db.SaveChanges();
                        }

                        var firstOrDefault = db.Reportings.Where(s => s.ReportingManagerId == staffid).ToList();
                        foreach (var ex in firstOrDefault)
                        {
                            Reporting bts = db.Reportings.Find(ex.ReportingId);
                            db.Reportings.Remove(bts);
                            db.SaveChanges();
                        }
                    }
                    data.isActive = status;                    
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
