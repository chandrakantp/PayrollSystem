using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class SalaryStuctureController : BaseController
    {

        #region ------ DBContext ------
        dbContainer db = new dbContainer();

        #endregion 

        #region -------- Index --------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var data = db.SalaryStructures.Where(s => s.FirmId == firm).ToList();
            return View(data.ToList());
        }
        #endregion 

        #region ------- Details -------
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion 

        #region -------- Create -------
        public ActionResult Create()
        {
            return View();
        }
                
        [HttpPost]
        public ActionResult Create(StructureSalaryModel structureSalaryModel)
        {
            var firmid = LoginUserFirmId();
            try
            {
                var data = db.SalaryStructures.FirstOrDefault(a => a.StuctureName == structureSalaryModel.StuctureName && a.FirmId == firmid);
                if (data != null)
                {
                    TempData["notice"] = "exist";
                    return RedirectToAction("Create");
                }
                else
                {
                    var salaryStructure = new SalaryStructure();
                    salaryStructure.StuctureName = structureSalaryModel.StuctureName.Trim();
                    salaryStructure.CreatedOn = DateTime.Now;
                    salaryStructure.IsActive = true;
                    salaryStructure.FirmId = firmid;
                    db.SalaryStructures.Add(salaryStructure);
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
        }
        #endregion 

        #region -------- Edit ---------       
        [FilterConfig.EncryptedActionParameterAttribute]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var firmid = LoginUserFirmId();
            var data = db.SalaryStructures.FirstOrDefault(a => a.Id == id);
            var Result = new StructureSalaryModel();
            if (data != null)
            {
                Result.Id = data.Id;
                Result.CreatedOn = DateTime.Now;
                Result.StuctureName = data.StuctureName;
                Result.IsActive = true; 
                Result.FirmId = firmid;
                return View(Result);
            }
            return View(Result);
        }
       
        [FilterConfig.EncryptedActionParameterAttribute]
        [HttpPost]
        public ActionResult Edit(StructureSalaryModel salary)
         {
            try
            {
                var firmid = LoginUserFirmId();
                   var data = db.SalaryStructures.FirstOrDefault(a => a.StuctureName == salary.StuctureName && a.FirmId==firmid);
                if (data == null)
                {
                    SalaryStructure sr = db.SalaryStructures.Find(salary.Id);
                    {
                        sr.StuctureName = salary.StuctureName;
                        sr.IsActive = true;
                        sr.CreatedOn = DateTime.Now;
                        sr.FirmId = firmid;
                    }
                    db.SalaryStructures.AddOrUpdate(sr);
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
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
        }
        #endregion 

        #region ------- Delete --------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaryStructure salaryStructure = db.SalaryStructures.Find(id);
            if (salaryStructure == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(salaryStructure);
        }
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SalaryStructure salaryStructure)
        {
            try
            {
                SalaryStructure salary = db.SalaryStructures.Find(salaryStructure.Id);

                db.SalaryStructures.Remove(salary);
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

    }
}
