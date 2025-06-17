using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Role
{
    public class RoleAssignController : BaseController
    {
        #region ------ DbContext --------
        private dbContainer db = new dbContainer(); 
        #endregion

        #region ------- Index -----------

        // GET: /RoleAssign/
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var data = db.AsignRoleEmpReport(firm);
            var userroles = data.Select(a => new RoleAssignInfdexModel()
            {
                RoleName = a.Name,
                FullName = a.schoolCode +":"+ a.firstName +" "+ a.middleName + " " + a.lastName,               
                Firmid = a.firmId,
                TranId = a.TransId,
                
            }).ToList();
            return View(userroles);

            //var userroles = db.UserRoles.Where(a => a.AspNetRole.firmId == firm).Include(u => u.AspNetRole).Include(u => u.AspNetUser).OrderBy(a => a.AspNetRole.Name);
            //return View(userroles.ToList());
        }

        #endregion
        
        #region ------- Details ---------
        // GET: /RoleAssign/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userrole = db.UserRoles.Find(id);
            if (userrole == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(userrole);
        }

        #endregion
        
        #region -------- Create ---------

        public ActionResult Create()
        {
            var firm = LoginUserFirmId();

            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name");
            //ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.userId = db.Staffs.Where(a => a.firmId == firm && a.isActive==true).ToList().Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.userId.ToString()
            });
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRoleModel userrole)
        {

            if (ModelState.IsValid)
            {
                var lr = new UserRole()
                {
                    userId = userrole.userId,
                    RoleId = userrole.RoleId,
                };

                db.UserRoles.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("Create");
            }

            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", userrole.RoleId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "UserName", userrole.userId);
            return View(userrole);
        }

        #endregion

        #region --- Edit Get and Post ---
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userrole = db.UserRoles.Find(id);
            if (userrole == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            var firm = LoginUserFirmId();
            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name", userrole.RoleId);
            var userId = userrole.userId;
            var userdetail = db.Staffs.FirstOrDefault(a => a.userId == userId);
            userrole.UserName = userdetail.firstName + " " + userdetail.lastName;
            return View(userrole);
        }

        // POST: /RoleAssign/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,RoleId,TransId")] UserRole userrole)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                db.Entry(userrole).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Create");
            }
            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name", userrole.RoleId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "UserName", userrole.userId);
            return View(userrole);
        }

        #endregion

        #region --------- Delete --------
        // GET: /RoleAssign/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userrole = db.UserRoles.Find(id);
            if (userrole == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(userrole);
        }

        // POST: /RoleAssign/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRole userrole = db.UserRoles.Find(id);
            db.UserRoles.Remove(userrole);
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("Create");
        }

        #endregion

        #region -------- Despose --------
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
