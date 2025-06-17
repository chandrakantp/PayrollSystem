using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class StructureassignmentController : BaseController
    {
        #region ------ DBContext ------

        public readonly dbContainer Db = new dbContainer();
        #endregion

        #region --------- Index -------
        public ActionResult Index()
        {
            var firmid = LoginUserFirmId();
            var structurepermission = Db.Structurepermissions.Where(a => a.FirmId == firmid && a.Staff.isActive == true).ToList();
            return View(structurepermission);
        }
        #endregion 

        #region -------- Details ------
        
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 

        #region ------- Create --------
        
        [HttpGet]
        public ActionResult Create()
        {
            var firmid = LoginUserFirmId();
            ViewBag.structlist =Db.SalaryStructures.Where(a => a.FirmId == firmid).OrderBy(a => a.StuctureName).ToList().Select(a => new SelectListItem() { Text = a.StuctureName, Value = Convert.ToString(a.Id) });
            ViewBag.stafflist = Db.Staffs.Where(a => a.firmId == firmid && a.isActive == true).ToList().Select(a => new SelectListItem(){Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,Value = a.staffId.ToString()});
            return View();
        }
      
        [HttpPost]
        public ActionResult Create(StructurepermissionModel structurepermissionModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var staff in structurepermissionModel.staffid)
                    {
                        var firmid = LoginUserFirmId();
                        var staffid = Convert.ToInt32(staff);
                        var data = Db.Structurepermissions.FirstOrDefault(m=>m.StaffId == staffid && m.IsDeleted==false);
                        //if (!db.Permissions.Any(m => m.RoleId == permission.RoleId && m.menuItemId == permission.menuItemId))
                        if (data == null)
                        {
                            var lr = new Structurepermission()
                            {
                                StructId = structurepermissionModel.StructId,
                                StaffId = staffid,
                                CreatedOn = DateTime.Now,
                                FirmId = firmid,
                                IsDeleted = false
                            };

                            Db.Structurepermissions.Add(lr);
                            Db.SaveChanges();


                            var stctcpk = Db.StaffCTCs.Where(s => s.staffId == staffid).ToList();
                            foreach (var ex in stctcpk)
                            {
                                StaffCTC st = Db.StaffCTCs.Find(ex.ctcId);
                                {
                                    st.structId = structurepermissionModel.StructId;
                                    Db.StaffCTCs.AddOrUpdate(st);
                                    Db.SaveChanges();
                                }
                            }

                            TempData["notice"] = "success";
                            //return RedirectToAction("Create");
                        }
                        else
                        {
                            TempData["notice"] = "exist";
                        }
                    }
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }
        #endregion 

        #region ------- Edit ----------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int id)
        {
            var structurepermissionModel = new StructurepermissionModel();
            var data = Db.Structurepermissions.FirstOrDefault(a => a.Id == id);
            if (data != null)
            {
                structurepermissionModel.StructId = data.StructId;
                structurepermissionModel.Staffidnew = data.StaffId;
            }
            structurepermissionModel.Id = id;
            var firmid = LoginUserFirmId();
            ViewBag.Structlist = new SelectList(Db.SalaryStructures.Where(a => a.FirmId == firmid), "Id", "StuctureName", structurepermissionModel.Id);
            //ViewBag.Structlist = db.SalaryStructures.Where(a => a.FirmId == firmid).ToList().Select(a=> new SelectListItem() { Text=a.StuctureName , Value=Convert.ToString(a.Id)});

            structurepermissionModel.Staffidnewlist = Db.Staffs.Where(a => a.firmId == firmid && a.isActive == true).ToList().Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });
            return View(structurepermissionModel);
        }
       
        [HttpPost]
        public ActionResult Edit(StructurepermissionModel structurepermissionModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var structurepermission = new Structurepermission();
                    var result = Db.Structurepermissions.FirstOrDefault(a => a.StructId == structurepermissionModel.StructId && a.StaffId == structurepermission.StaffId);

                    if (result != null)
                    {
                        TempData["notice"] = "exist";
                        return RedirectToAction("Create");
                    }

                    var data = Db.Structurepermissions.FirstOrDefault(a => a.Id == structurepermissionModel.Id);
                    if (data != null)
                    {                        
                        data.StructId = structurepermissionModel.StructId;
                        data.StaffId = structurepermissionModel.Staffidnew;
                        Db.Entry(data).State = EntityState.Modified;
                        Db.SaveChanges();                       
                    }

                    var stctcpk = Db.StaffCTCs.Where(s => s.staffId == structurepermissionModel.Staffidnew).ToList();
                    foreach (var ex in stctcpk)
                    {
                        StaffCTC st = Db.StaffCTCs.Find(ex.ctcId);
                        {
                            st.structId = structurepermissionModel.StructId;
                            Db.StaffCTCs.AddOrUpdate(st);
                            Db.SaveChanges();
                        }
                    }

                    TempData["notice"] = "update";
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

        #region --------- Delete ------
        public ActionResult Delete(int? id, int? stid, int? structureid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Structurepermission bts = Db.Structurepermissions.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        
        [HttpPost]
        public ActionResult Delete(int id, int? stid, int? structureid)
        {
            try
            {
                //&& s.structId == structureid
                var stctcpk = Db.StaffCTCs.Where(s => s.staffId == stid).ToList();
                foreach (var ex in stctcpk)
                {
                    StaffCTC st = Db.StaffCTCs.Find(ex.ctcId);
                    {
                        st.structId = null;
                        Db.StaffCTCs.AddOrUpdate(st);
                        Db.SaveChanges();
                    }
                }

                Structurepermission structurepermission = Db.Structurepermissions.Find(id);
                Db.Structurepermissions.Remove(structurepermission);
                Db.SaveChanges();
             
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
    }
}
