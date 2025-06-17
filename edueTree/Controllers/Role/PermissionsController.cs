using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Role
{
    public class PermissionsController : BaseController
    {
        #region ------- DbContext -----------
        private dbContainer db = new dbContainer();
        #endregion 

        #region ------- Index ---------------

        // GET: /Permissions/
        public ActionResult Index()
        {
            TimeZone zone = TimeZone.CurrentTimeZone;
            // Demonstrate ToLocalTime and ToUniversalTime.
            DateTime local = zone.ToLocalTime(DateTime.Now);
            DateTime universal = zone.ToUniversalTime(DateTime.Now);
            Console.WriteLine(local);
            Console.WriteLine(universal);
            var firm = LoginUserFirmId();
            var permissions = db.Permissions.Where(s => s.AspNetRole.firmId == firm).Include(p => p.AspNetRole).Include(p => p.MenuItem).OrderBy(a => a.AspNetRole.Name);
            return View(permissions.ToList());
        }

        #endregion 

        #region -------- Details ------------
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(permission);
        }

        #endregion 

        #region ----------- Create ----------

        public ActionResult Create()
        {
            var firm = LoginUserFirmId();
            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm).OrderBy(a => a.Name), "Id", "Name");
            ViewBag.MId = new SelectList(db.MenuItems.OrderBy(a => a.itemName), "menuItemId", "itemName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PermissionModel permission)
        {
            if (ModelState.IsValid)
            {
                foreach (var prid in permission.MId)
                {
                    var mitemID = Convert.ToInt32(prid);
                    var data = db.Permissions.FirstOrDefault(m => m.RoleId == permission.RoleId && m.menuItemId == mitemID);
                    //if (!db.Permissions.Any(m => m.RoleId == permission.RoleId && m.menuItemId == permission.menuItemId))
                    if (data == null)
                    {
                        var lr = new Permission()
                        {
                            RoleId = permission.RoleId,
                            menuItemId = mitemID,
                            IsActive = true
                        };

                        db.Permissions.Add(lr);
                        db.SaveChanges();
                        TempData["notice"] = "success";
                        //return RedirectToAction("Create");
                    }
                    else
                    {
                        TempData["notice"] = "exist";
                        //return RedirectToAction("Create");
                    }
                }
                return RedirectToAction("Create");
            }
            var firm = LoginUserFirmId();
            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name", permission.RoleId);
            ViewBag.MId = new SelectList(db.MenuItems, "menuItemId", "itemName", permission.menuItemId);
            return View(permission);
        }
        #endregion 

        #region ---------- Edit -------------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            var firm = LoginUserFirmId();
            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name", permission.RoleId);
            ViewBag.menuItemId = new SelectList(db.MenuItems, "menuItemId", "itemName", permission.menuItemId);
            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "permissionId,menuItemId,RoleId")] Permission permission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permission).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Create");
            }
            var firm = LoginUserFirmId();
            ViewBag.RoleId = new SelectList(db.AspNetRoles.Where(a => a.firmId == firm), "Id", "Name", permission.RoleId);
            ViewBag.menuItemId = new SelectList(db.MenuItems, "menuItemId", "itemName", permission.menuItemId);
            return View(permission);
        }

        #endregion  

        #region --- Delete ------------------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(permission);
        }

        // POST: /Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Permission permission = db.Permissions.Find(id);
            db.Permissions.Remove(permission);
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("Create");
        }

        #endregion  

        #region ------- Des-pose ------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion  

        #region ----- Get-Role Permission ---
        [Authorize]
        public ActionResult _GetMenuAssign(int? id)
        {
            var permissionView = new permissionViewModel();
            var firmid = LoginUserFirmId();
            if (id == null)
            {
                return PartialView("_GetMenuAssign", permissionView);
            }
            else
            {
                    var query =
                    from c in db.MenuItems.Where(s => s.isActive == true)
                    where !(from o in db.Permissions.Where(d => d.RoleId == id)
                            select o.menuItemId)
                           .Contains(c.menuItemId)
                    select c;

                    var query1 =
                    from c in db.MenuItems.Where(s=>s.isActive == true)
                    where (from o in db.Permissions.Where(d => d.RoleId == id)
                           select o.menuItemId)
                          .Contains(c.menuItemId)
                    select c;

                permissionView.Rolelist = db.AspNetRoles.Where(a => a.firmId == firmid).ToList();
                permissionView.MenuItemslist = db.MenuItems.ToList();
                permissionView.roleId = (int)id;
                
                permissionView.PermissionsList1 = query1.ToList();
                permissionView.PermissionsList = query.ToList();
                permissionView.PermissionsList1.AddRange(permissionView.PermissionsList);

                return PartialView("_GetMenuAssign", permissionView);
            }
        }

        #endregion 

        #region ---- _Changepermissions -----
        [Authorize]
        public ActionResult _Changepermissions(string Attendid, bool Status, int? Roleid)
        {
            var perid = Convert.ToInt32(Attendid);
            var update = db.Permissions.FirstOrDefault(a => a.menuItemId == perid && a.RoleId == Roleid);

            if (update != null)
            {
                update.IsActive = Status;
                db.Permissions.AddOrUpdate(update);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var lr = new Permission
                {
                    menuItemId = Convert.ToInt32(Attendid),
                    RoleId = Roleid,
                    IsActive = true
                };
                db.Permissions.Add(lr);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region - RoleBaseCheckboxAssign --- 

        public ActionResult RolebasedResult()
        {
            var permissionView = new permissionViewModel();
            var firmid = LoginUserFirmId();
            permissionView.Rolelist = db.AspNetRoles.Where(a => a.firmId == firmid).ToList();

            return View(permissionView);
        }

        #endregion

        #region --- Role based CheckAssign --

        public ActionResult RoleAssignment()
        {
            permissionViewModel permissionView = new permissionViewModel();
            var firmid = LoginUserFirmId();
            permissionView.Rolelist = db.AspNetRoles.Where(a => a.firmId == firmid).ToList();
            permissionView.MenuItemslist = db.MenuItems.ToList();
            return View(permissionView);
        }

        #endregion

    }
}
