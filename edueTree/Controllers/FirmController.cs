using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using edueTree.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;

namespace edueTree.Controllers
{
    [Authorize]
    public class FirmController : BaseController
    {
        #region ---------- DbContext --------------
        private dbContainer db = new dbContainer();
        #endregion

        #region ---------- FirmController ---------
        public FirmController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public FirmController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;

        }

        #endregion

        #region --------- UserManager -------------
        public UserManager<ApplicationUser> UserManager { get; private set; }
        #endregion

        #region ------ GetAuthenticationManager ---
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        #endregion

        #region -------------- Index --------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            return View(db.FirmInfoes.Where(s => s.firmId == firm).ToList());
        }

        public ActionResult FirmsForSuperadmin()
        {
            var firms =
                  db.FirmInfoes.Select(a => new FirmInfoModel
                      {
                          firmId = a.firmId,
                          firmName = a.firmName,
                          logo = a.logo,
                          flatNo = a.flatNo,
                          street = a.street,
                          area = a.area,
                          city = a.city,
                          state = a.state,
                          pincode = a.pincode,
                          email = a.email,
                          contact = a.contact,
                          fax = a.fax,
                          isActivefirm = (bool)a.isActive
                      });
            return View(firms.ToList());
        }
        #endregion 

        #region --------- FirmSubmitStatus --------
        [HttpPost]
        public ActionResult FirmSubmitStatus(string Attendid, bool Status)
        {
            var frmid = Convert.ToInt64(Attendid);
            bool status = Status;

            var firmupdate = db.FirmInfoes.Find(frmid);
            {
                firmupdate.isActive = status;
                db.FirmInfoes.AddOrUpdate(firmupdate);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }            
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public  async Task<ActionResult> SuperAdminFirmLogin(int? id)
        {
            var frmid = Convert.ToInt64(id);
            var aspuserid = db.AspNetRoles.FirstOrDefault(s => s.firmId == frmid);
            if (aspuserid != null)
            {
                var pkidasp = aspuserid.Id;
                var roleid = db.UserRoles.FirstOrDefault(s => s.RoleId == pkidasp);
                if (roleid != null)
                {
                    var roleUserid = roleid.userId;
                    var user = await UserManager.FindByIdAsync(roleUserid);
                    if (user != null)
                    {
                        var identity =
                            await
                                UserManager.CreateIdentityAsync(user,
                                    DefaultAuthenticationTypes.ApplicationCookie);
                        GetAuthenticationManager()
                            .SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
                        return RedirectToAction("StaffProfile", "Staffs");
                    }
                    else
                    {
                        return RedirectToAction("FirmsForSuperadmin", "Firm");
                    }
                }
            }
            return View();
        }
        #endregion

        #region ------------ Details --------------
    
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FirmInfo firminfo = db.FirmInfoes.Find(id);
            if (firminfo == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(firminfo);
        }

        #endregion

        #region ------------- Create --------------

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FirmModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                    else if (file.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                        string[] allowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".bmp" };
                        if (!allowedFileExtensions.Contains
                            (file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ModelState.AddModelError("File",
                                "Please file of type: " + string.Join(", ", allowedFileExtensions));
                        }
                        else if (file.ContentLength > MaxContentLength)
                        {
                            ModelState.AddModelError("File",
                                "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                        }
                        else
                        {
                            if (file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var rondom = Guid.NewGuid() + fileName;
                                var path = Path.Combine(Server.MapPath("~/FirmLogo/"), rondom);
                                file.SaveAs(path);
                                model.Logo = rondom;
                                ViewBag.Path = String.Format("~/FirmLogo/", fileName);
                            }
                        }
                        var firmInfo = new FirmInfo()
                        {
                            firmName = model.FirmName,
                            logo = model.Logo,
                            flatNo = model.FlatNo,
                            street = model.Street,
                            area = model.Area,
                            city = model.City,
                            state = model.State,
                            pincode = model.Pincode,
                            contact = model.Contact,
                            email = model.Email,
                            fax = model.Fax,
                            website = model.Website,
                        };
                        db.FirmInfoes.Add(firmInfo);
                    }
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return View(model);
        }

        #endregion 

        #region ------------ FirmInfo -------------
        public ActionResult FirmInfo()
        {
            var firm = LoginUserFirmId();
            FirmInfo firminfo = db.FirmInfoes.Find(firm);
            if (firminfo == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(firminfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FirmInfo(FirmInfo model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var rondom = Guid.NewGuid() + fileName;
                        var path = Path.Combine(Server.MapPath("~/FirmLogo/"), rondom);
                        file.SaveAs(path);
                        model.logo = rondom;
                        ViewBag.Path = String.Format("~/FirmLogo/", fileName);
                    }
                    var firmInfo = new FirmInfo();
                    firmInfo.firmId = model.firmId;
                    firmInfo.firmName = model.firmName;
                    firmInfo.logo = model.logo;
                    firmInfo.flatNo = model.flatNo;
                    firmInfo.street = model.street;
                    firmInfo.area = model.area;
                    firmInfo.city = model.city;
                    firmInfo.state = model.state;
                    firmInfo.pincode = model.pincode;
                    firmInfo.contact = model.contact;
                    firmInfo.email = model.email;
                    firmInfo.fax = model.fax;
                    firmInfo.website = model.website;
                    firmInfo.isActive = model.isActive;
                    firmInfo.ipFiltering = model.ipFiltering;
                    db.Entry(firmInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "success";

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return View(model);
        }

        #endregion
         
        #region ---------- Static IP Address ------
        public ActionResult StaticIpAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StaticIpAdd(FirmInfo model)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var ip = new StaticIP()
                {
                    firmId = firm,
                    staticIPaddress = model.ipAddress,
                    isActive = true
                };

                FirmInfo info = db.FirmInfoes.Find(firm);
                info.ipFiltering = true;
                db.FirmInfoes.AddOrUpdate(info);
                db.SaveChanges();

                db.StaticIPs.Add(ip);
                db.SaveChanges();
                TempData["notice"] = "IP Address Added Successfully";
                return RedirectToAction("FirmInfo");

            }
            return View(model);
        }

        #endregion 

        #region ----------- IpIndex ---------------
        public ActionResult IPIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.StaticIPs.Where(s => s.firmId == firm).ToList());
        }

        #endregion 

        #region ------------- Edit ----------------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FirmInfo firminfo = db.FirmInfoes.Find(id);
            if (firminfo == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(firminfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FirmInfo model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var rondom = Guid.NewGuid() + fileName;
                        var path = Path.Combine(Server.MapPath("~/FirmLogo/"), rondom);
                        file.SaveAs(path);
                        model.logo = rondom;
                        ViewBag.Path = String.Format("~/FirmLogo/", fileName);
                    }
                    var firmInfo = new FirmInfo()
                    {
                        firmId = model.firmId,
                        firmName = model.firmName,
                        logo = model.logo,
                        flatNo = model.flatNo,
                        street = model.street,
                        area = model.area,
                        city = model.city,
                        state = model.state,
                        pincode = model.pincode,
                        contact = model.contact,
                        email = model.email,
                        fax = model.fax,
                        website = model.website,
                        isActive = model.isActive
                    };
                    db.Entry(firmInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";

                    return RedirectToAction("Index", new { q = Encrypt("id=" + model.firmId) });
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return View(model);
        }

        #endregion 

        #region -------------- Encrypt ------------
        public string Encrypt(string plainText)
        {
            //string key = "jdsg432387#";
            string key = "chan221988#";
            byte[] EncryptKey = { };
            //  byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] IV = { 27, 98, 45, 23, 65, 173, 17, 88 };
            EncryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        #endregion 

        #region --------------- Delete ------------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FirmInfo firminfo = db.FirmInfoes.Find(id);
            if (firminfo == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(firminfo);
        }        

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FirmInfo firminfo = db.FirmInfoes.Find(id);
            db.FirmInfoes.Remove(firminfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion 

        #region ------------ Des-pose -------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion 

        #region ----------- FirmInfo --------------
        public ActionResult FirmInfoUpdate()
        {
            var firm = LoginUserFirmId();
            FirmInfo firminfo = db.FirmInfoes.Find(firm);

            FirmInfoModel frm = new FirmInfoModel();
            frm.firmId = firminfo.firmId;
            frm.firmName = firminfo.firmName;
            frm.flatNo = firminfo.flatNo;
            frm.street = firminfo.street;
            frm.logo = firminfo.logo;
            frm.area = firminfo.area;
            frm.city = firminfo.city;
            frm.state = firminfo.state;
            frm.pincode = firminfo.pincode;
            frm.contact = firminfo.contact;
            frm.email = firminfo.email;
            frm.fax = firminfo.fax;
            frm.firmId = firminfo.firmId;
            frm.isActive = firminfo.isActive;

            if (firminfo == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(frm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FirmInfoUpdate(FirmInfoModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var rondom = Guid.NewGuid() + fileName;
                        var path = Path.Combine(Server.MapPath("~/FirmLogo/"), rondom);
                        file.SaveAs(path);
                        model.logo = rondom;
                        ViewBag.Path = String.Format("~/FirmLogo/", fileName);
                    }
                    var firmInfo = new FirmInfo();
                    firmInfo.firmId = model.firmId;
                    firmInfo.firmName = model.firmName;
                    firmInfo.logo = model.logo;
                    firmInfo.flatNo = model.flatNo;
                    firmInfo.street = model.street;
                    firmInfo.area = model.area;
                    firmInfo.city = model.city;
                    firmInfo.state = model.state;
                    firmInfo.pincode = model.pincode;
                    firmInfo.contact = model.contact;
                    firmInfo.email = model.email;
                    firmInfo.fax = model.fax;
                    firmInfo.website = model.website;
                    firmInfo.isActive = model.isActive;
                    firmInfo.ipFiltering = model.ipFiltering;
                    db.Entry(firmInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "success";

                    return RedirectToAction("FirmInfoUpdate");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return View(model);
        }
        #endregion 

        #region ------- LoginFromSuperadmin --------
        //public async Task<ActionResult> LoginFromSuperadmin(string userId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(userId);
        //        if (user != null)
        //        {
        //            var identity =
        //                await
        //                    UserManager.CreateIdentityAsync(user,
        //                        DefaultAuthenticationTypes.ApplicationCookie);
        //            GetAuthenticationManager()
        //                .SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
        //            return RedirectToAction("StaffProfile", "Staffs");
        //        }
        //        else
        //        {
        //            return RedirectToAction("FirmsForSuperadmin", "Firm");
        //        }
        //    }
        //    return View();
        //}
        #endregion

        public ActionResult FirmDashboard()
        {
            return View();
        }
    }
}
