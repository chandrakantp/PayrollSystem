using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Masters
{
    [Authorize]
    public class DesignationController : BaseController
    {
        #region ------- Dbcontext  --------
        private dbContainer db = new dbContainer();
        #endregion

        #region -------- Index ------------

        // GET: /Designation/
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            return View(db.Designations.Where(a => a.isActive == true && a.firmId == firm).ToList());
        }

        #endregion
 
        #region -------- Details ----------

        // GET: /Designation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(designation);
        }

        #endregion 

        #region --------- Create ----------

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DesignationModel designation)
        {
            if (ModelState.IsValid)
            {
                var firmId = LoginUserFirmId();
                if (!db.Designations.Any(m => m.designation1 == designation.designation1 && m.firmId == firmId && m.isActive==true ))
                {
                    var lr = new Designation()
                    {
                        designation1 = designation.designation1,
                        firmId = LoginUserFirmId(),
                        isActive = designation.isActive = true,
                    };
                    db.Designations.Add(lr);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Create");

                }
                TempData["notice"] = "exist";
                return View();
                //designation.isActive =true;
                //db.Designations.Add(designation);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(designation);
        }

        #endregion 

        #region ---------- Edit -----------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationModel designation = new DesignationModel();
            var model = db.Designations.Find(id);

            designation.designationId = model.designationId;
            designation.designation1 = model.designation1;
            designation.FirmId = model.firmId;


            return View(designation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DesignationModel designation)
        {
            if (ModelState.IsValid)
            {
                var xyz = db.Designations.Find(designation.designationId);
                xyz.designation1 = designation.designation1;
                xyz.firmId = designation.FirmId;
                xyz.isActive = true;
                db.Designations.AddOrUpdate(xyz);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Create");

            }
            return View(designation);
        }

        #endregion        

        #region --------- Delete ----------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(designation);
        }

        // POST: /Designation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Designation designation = db.Designations.Find(id);
                designation.isActive = false;
                db.Designations.AddOrUpdate(designation);
                db.SaveChanges();
                TempData["notice1"] = "Successfully Deleted.!";
                return RedirectToAction("Create");
            }
            catch (Exception)
            {
                TempData["notice1"] = "Cant't delete, this designation used somewhere.!";
                return RedirectToAction("Create");
            }

        }

        #endregion 

        #region -------- Despose ----------
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
