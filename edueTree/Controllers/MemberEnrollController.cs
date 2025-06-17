using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class MemberEnrollController : BaseController
    {
        #region ------ DbContext ---------
        dbContainer db = new dbContainer(); 
        #endregion 

        #region ------- Index ------------
     
        public ActionResult Index()
        {
          
         var dt=   DateTime.UtcNow;
            var firm = LoginUserFirmId();
            var memberlist = db.GetAllmemberEnroll();
            var membermodel = memberlist.Where(a => a.firmId == firm).Select(a => new MemberattendanceConfigModel() { StaffName = a.firstName + " " + a.middleName + " " + a.lastName, SerialKey = a.SerialKey, ConfigId = a.ConfigId, FingureId = a.FingureId }).ToList();
            return View(membermodel);
        }

        #endregion 

        #region -------- Details ---------
         [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Details(int? staffId)
        {
            var firm = LoginUserFirmId();
            var machineenrollbyEmplist = db.GetEnrollmemberById(staffId);
            var membermodel = machineenrollbyEmplist.Where(a => a.firmId == firm).Select(a => new MemberattendanceConfigModel() { StaffName = a.firstName + " " + a.middleName + " " + a.lastName, SerialKey = a.SerialKey, ConfigId = a.ConfigId, FingureId = a.FingureId }).ToList();

            return View(membermodel);
        }

        #endregion 

        #region ------- Create -----------
        
        public ActionResult Create()
        {
            var firm = LoginUserFirmId();
            var config = new MemberattendanceConfigModel
            {
                StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive==true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                }),
                MachineList = db.MachineConfigures.ToList().Where(s => s.firmId == firm && s.Isdeleted==false).Select(a => new SelectListItem()
               {
                   Text = a.SerialKey.ToString(),
                   Value = a.id.ToString()
               })
            };
            return View(config);
        }

        
        [HttpPost]
        public ActionResult Create(MemberattendanceConfigModel memberattendanceConfigModel)
        {
            try
            {
                var data1 =
                db.MemberattendaceConfigs.FirstOrDefault(
                    a =>

                        a.StaffId == memberattendanceConfigModel.StaffId &&
                        a.Machineid == memberattendanceConfigModel.Machineid && a.IsDeleted == false);
                if (data1 != null)
                {
                    TempData["notice"] = "exist";
                    return RedirectToAction("Create");
                }
                var data =
                    db.MemberattendaceConfigs.FirstOrDefault(
                        a =>
                          
                            a.FingureId == memberattendanceConfigModel.FingureId &&
                            a.Machineid == memberattendanceConfigModel.Machineid && a.IsDeleted==false);
                

                if (data != null)
                {
                    TempData["notice"] = "exist";
                    return RedirectToAction("Create");
                }
               
                var firmId = LoginUserFirmId();
                var login = LoginEmployeeId();
                if (ModelState.IsValid)
                {
                    var memberattendace = new MemberattendaceConfig
                    {
                        CreatedBy = login,
                        FingureId = memberattendanceConfigModel.FingureId,
                        IsDeleted = false,
                        Machineid = memberattendanceConfigModel.Machineid,
                        StaffId = memberattendanceConfigModel.StaffId,
                        FirmId = firmId

                    };
                    db.MemberattendaceConfigs.Add(memberattendace);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Create");
                }
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
        }

        #endregion 

        #region  --------- Edit -----------
        
        public ActionResult Edit(int id)
        {
            var firm = LoginUserFirmId();
            var memberattendance = new MemberattendanceConfigModel();
            var data = db.MemberattendaceConfigs.FirstOrDefault(a => a.ConfigId == id);
            if (data != null)
            {
                memberattendance = new MemberattendanceConfigModel
                {
                    StaffList = db.Staffs.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
                    {
                        Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                        Value = a.staffId.ToString()
                    }),
                    MachineList = db.MachineConfigures.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
                    {
                        Text = a.SerialKey.ToString(),
                        Value = a.id.ToString()
                    }),
                    Machineid = data.Machineid,
                    ConfigId = data.ConfigId,
                    FingureId = data.FingureId,
                    StaffId = data.StaffId,

                };
            }

            return View(memberattendance);
        }
        
        [HttpPost]
        public ActionResult Edit(MemberattendanceConfigModel memberattendance)
        {
            try
            {
                var data = db.MemberattendaceConfigs.FirstOrDefault(a => a.ConfigId == memberattendance.ConfigId);
                if (data != null)
                {
                    var abc =
                        db.MemberattendaceConfigs.FirstOrDefault(
                            a => a.FingureId == memberattendance.FingureId && a.Machineid == memberattendance.Machineid && a.IsDeleted==false);
                    if (abc == null)
                    {
                        data.FingureId = memberattendance.FingureId;
                        data.Machineid = memberattendance.Machineid;
                        data.StaffId = memberattendance.StaffId;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["notice"] = "update";
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        TempData["notice"] = "exist";
                        return RedirectToAction("Create");
                    }
                  
                }

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        #endregion 

        #region -------- Delete ----------
        public ActionResult Delete(int id)
        {
            MemberattendanceConfigModel memberattendance = new MemberattendanceConfigModel();
            memberattendance.ConfigId = id;
            return View(memberattendance);
        }
        
        [HttpPost]
        public ActionResult DeleteConfirm(MemberattendanceConfigModel memberattendance)
        {
            try
            {
                var data = db.MemberattendaceConfigs.FirstOrDefault(a => a.ConfigId == memberattendance.ConfigId);
                if (data != null)
                {
                    //data.IsDeleted = true;
                    //db.Entry(data).State = EntityState.Modified;
                    db.MemberattendaceConfigs.Remove(data);
                    db.SaveChanges();
                    TempData["notice"] = "delete";
                    return RedirectToAction("Create");
                }
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        #endregion 
      
    }
}
