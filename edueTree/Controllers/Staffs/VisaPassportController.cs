using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Staffs
{
    public class VisaPassportController : BaseController
    {
        #region ---- Dbcontext ----
        private dbContainer db = new dbContainer();
        #endregion

        #region ------ Index ------

        //
        // GET: /VisaPassport/
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Index(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;
                }

            }
            else
            {
                ViewBag.message = true;
            }
            var data = db.VisaPassports.Where(a => a.StaffId == staffId && a.IsDeleted==false).ToList();
            TempData["staffId"] = staffId;

            return View(data);
        }

        #endregion

        #region ------- Details ---
        public ActionResult Details(int id)
        {
            var data = db.VisaPassports.Where(a => a.StaffId == id).ToList();

            return View(data);
        }

        #endregion 

        #region ----- Create ------
         [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Create(int? staffId)
        {
            var visaPassportModel = new VisaPassportModel {StaffId = staffId};
            return View(visaPassportModel);
        }

        [HttpPost]
        public ActionResult Createpost(VisaPassportModel visaPassportModel)
        {
            try
            {
                var visaPassport=new VisaPassport();
                if (ModelState.IsValid)
                {
                    visaPassport.Country = visaPassportModel.Country;
                    visaPassport.CreatedDate = DateTime.Now;
                    visaPassport.ExpiryDate = visaPassportModel.ExpiryDate;
                    visaPassport.Num = visaPassportModel.Num;
                    visaPassport.IsDeleted = false;
                    visaPassport.StaffId = visaPassportModel.StaffId;
                    visaPassport.Type = visaPassportModel.Type;
                    db.VisaPassports.Add(visaPassport);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                }
                return RedirectToAction("StaffDashboard", "Staffs", new { staffid = visaPassportModel.StaffId });
            }
            catch
            {
                return View();
            }
        }

        #endregion 

        #region ------ Edit -------
        public ActionResult Edit(int id)
        {
            VisaPassportModel visaPassportModel = new VisaPassportModel();
           
            var data = db.VisaPassports.FirstOrDefault(a=>a.PassVisaId==id);
            if (data != null)
            {
                visaPassportModel = new VisaPassportModel
                {
                    Country = data.Country,
                    ExpiryDate = data.ExpiryDate,
                    Num = data.Num,
                    PassVisaId = data.PassVisaId,
                    StaffId = data.StaffId,
                    Type = data.Type
                };


            }
            return View(visaPassportModel);
        }
        
        [HttpPost]
        public ActionResult Editpost(VisaPassportModel visaPassportModel)
        {
            try
            {
                var data = db.VisaPassports.FirstOrDefault(a => a.PassVisaId == visaPassportModel.PassVisaId);
                if (data != null)
                {
                    data.Country = visaPassportModel.Country;
                    data.ExpiryDate = visaPassportModel.ExpiryDate;
                    data.Num = visaPassportModel.Num;
                    data.Type = visaPassportModel.Type;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                }

                return RedirectToAction("StaffDashboard", "Staffs", new { staffid = visaPassportModel.StaffId });
            }
            catch
            {
                return View();
            }
        }

        #endregion 

        #region ------ Delete -----
        
        public ActionResult Delete(int id)
        {
            var visaPassportModel = new VisaPassportModel {PassVisaId = id};
            return View(visaPassportModel);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(VisaPassportModel visaPassportModel)
        {
            var data = db.VisaPassports.FirstOrDefault(a => a.PassVisaId == visaPassportModel.PassVisaId);
            try
            {
              
                  
                if (data != null)
                {
                    //data.IsDeleted = true;
                    //db.Entry(data).State = EntityState.Modified;
                    db.VisaPassports.Remove(data);
                    db.SaveChanges();
                    TempData["notice"] = "delete";
                    return RedirectToAction("StaffDashboard", "Staffs", new { staffid = data.StaffId });
                }
            }
            catch
            {
                return View();
            }
            return RedirectToAction("StaffDashboard", "Staffs", new { staffid = data.StaffId });
        } 
        #endregion 
    }
}
