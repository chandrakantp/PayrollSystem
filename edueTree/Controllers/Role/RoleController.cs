using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Role
{
    [Authorize]
    public class RoleController : BaseController
    {
        #region --------- Dbcontext -----
        private dbContainer db = new dbContainer();
        #endregion

        #region ------- Index -----------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            return View(db.AspNetRoles.Where(a => a.firmId == firm).ToList());
        }
        #endregion

        #region ---------- Detail -------

        // GET: /Role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(aspnetrole);
        }

        #endregion

        #region -------- Create ---------
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AspNetRoleModel aspnetrole)
        {
            if (ModelState.IsValid)
            {
                var lr = new AspNetRole
                {
                    Name = aspnetrole.Name,
                    firmId = LoginUserFirmId()
                };

                db.AspNetRoles.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("Create");
            }

            return View(aspnetrole);
        }
        #endregion

        #region ---------- Edit ---------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            AspNetRoleModel netrole = new AspNetRoleModel();
            netrole.Name = aspnetrole.Name;
            netrole.Id=aspnetrole.Id;
            netrole.firmId = aspnetrole.firmId;
            if (aspnetrole == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(netrole);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,firmId")] AspNetRole aspnetrole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aspnetrole.firmId = LoginUserFirmId();
                    db.Entry(aspnetrole).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                    return RedirectToAction("Create");
                }
                return View(aspnetrole);
            }
            catch (Exception)
            {
                TempData["notice"] = "warn";
                return View(aspnetrole);
            }
        }

        #endregion

        #region --------- Delete --------
        // GET: /Role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(aspnetrole);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                  AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspnetrole);
            TempData["notice"] = "delete";
            db.SaveChanges();
            return RedirectToAction("Create");
            }
            catch (Exception)
            {
               TempData["notice"] = "cantdelete";
                return RedirectToAction("Create");
            }
        }

        #endregion

        #region ------- Access Denied ---
        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region ------- Secret ----------
        public string Secret()
        {
            return "You do not have permission access this page";
        }
        #endregion

        #region -------- Dispose --------
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
