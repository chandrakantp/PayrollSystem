using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;
using edueTree.Services;

namespace edueTree.Controllers
{
    public class MasterSettingController : BaseController
    {
        #region ----------  DBContext -----------
        private dbContainer db = new dbContainer(); 
        #endregion

        #region --------- AddBufferTime ---------

        [HttpGet]
        public ActionResult AddBufferTime()
        {
            var firm = LoginUserFirmId();
            var count = db.BufferTimeSettings.Count(a=>a.firmid==firm);
            if (count == 0)
            {
                ViewBag.visible = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddBufferTime(BufferTimeSettingModel btsModel)
        {
            var firmIdnew = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                var lr = new BufferTimeSetting()
                {
                    EarlyIn = btsModel.EarlyIn,
                    firmid = firmIdnew,
                    Isactive = true
                };
                db.BufferTimeSettings.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AddBufferTime");
            }
            return View();
        } 
        #endregion 

        #region ------------ Index --------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            return View(db.BufferTimeSettings.Where(a => a.firmid == firm).ToList());
        } 
        #endregion 

        #region ----------- Details -------------
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 

        #region -------------- Edit -------------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bts = db.BufferTimeSettings.Find(id);
            var dept = new BufferTimeSettingModel {EarlyIn = bts.EarlyIn, firmid = bts.firmid, BufferId = bts.BufferId};

            return View(bts);
        }            
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BufferId,EarlyIn,firmid,Isactive")] BufferTimeSetting bts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bts).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("AddBufferTime");
            }
            return View(bts);
        }
        #endregion 

        #region --------- Delete Record ---------

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BufferTimeSetting bts = db.BufferTimeSettings.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BufferTimeSetting bts = db.BufferTimeSettings.Find(id);
                db.BufferTimeSettings.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("AddBufferTime");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("AddBufferTime");
            }
        }

        #endregion  
         
        #region ---- PayrollSystemSettings ------

        [HttpGet]
        public ActionResult PayrollSystemSetting()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PayrollSystemSetting(PayrollConfigModel paysystem)
        {
            var firmIdnew = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                var lr = new PayrollConfig()
                {
                    MonthlyLeaves = paysystem.MonthlyLeaves,
                    WorkingHoursDay = paysystem.WorkingHoursDay,
                    HalfDayMinWorkHours = paysystem.HalfDayMinWorkHours,
                    WorkingdaysPermonth = paysystem.WorkingdaysPermonth,
                    firmId = firmIdnew,
                    isActive = true,
                };
                db.PayrollConfigs.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("PayrollSystemSetting");
            }
            return View();
        }

        public ActionResult PayrollSystemIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.PayrollConfigs.Where(a => a.firmId == firm && a.isActive == true).ToList());
        }

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EditPayrollSystem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayrollConfig bts = db.PayrollConfigs.Find(id);
            PayrollConfigModel dept = new PayrollConfigModel();
            dept.Id = bts.Id;
            dept.FirmId = bts.firmId;
            dept.MonthlyLeaves = bts.MonthlyLeaves;
            dept.WorkingHoursDay = bts.WorkingHoursDay;
            dept.HalfDayMinWorkHours = bts.HalfDayMinWorkHours;
            dept.WorkingdaysPermonth = bts.WorkingdaysPermonth;
            return View(dept);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayrollSystem(PayrollConfigModel bts)
        {
            if (ModelState.IsValid)
            {
                PayrollConfig pc = db.PayrollConfigs.Find(bts.Id);
                {
                    pc.MonthlyLeaves = bts.MonthlyLeaves;
                    pc.WorkingHoursDay = bts.WorkingHoursDay;
                    pc.HalfDayMinWorkHours = bts.HalfDayMinWorkHours;
                    pc.WorkingdaysPermonth = bts.WorkingdaysPermonth;
                    pc.firmId = bts.FirmId;
                    pc.isActive = true;
                }
                db.PayrollConfigs.AddOrUpdate(pc);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("UpdateSetting", "LeaveRequest");
            }
            return View(bts);
        }

        public ActionResult DeletePayrollSetting(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PayrollConfig bts = db.PayrollConfigs.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeletePayrollSetting(int id)
        {
            try
            {
                PayrollConfig bts = db.PayrollConfigs.Find(id);
                db.PayrollConfigs.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("PayrollSystemSetting");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("PayrollSystemSetting");
            }
        }

        #endregion

        #region --------- CreateEmailConfig -----
        [HttpGet]
        public ActionResult CreateEmailConfig()
        {
            var firm = LoginUserFirmId();
            var count = db.TblEmailConfigs.Where(s => s.firmid == firm).Count();
            if (count == 0)
            {
                ViewBag.visible = true;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmailConfig(TblEmailConfigModel ecm)
        {
            var firmIdnew = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                var lr = new TblEmailConfig()
                {
                    EmailAddress = ecm.EmailAddress,
                    Password = ecm.Password,
                    SMTPPortNo = ecm.SMTPPortNo,
                    HostName = ecm.HostName,
                    CreatedDate = DateTime.Now,
                    firmid = firmIdnew,
                    isActive = false
                };
                db.TblEmailConfigs.Add(lr);
                db.SaveChanges();

                string To = ecm.EmailAddress;
                var lnkHref = "<a href='http://people.innovative-fusion.com/MasterSetting/mailAuthentication?mail=" +
                    ecm.EmailAddress + "'> Configure Your Email </a>";
                string subject = "Email Configuration Link";
                string body = "<b> Please click the link below. </b><br/>" + lnkHref;
                var services = new MailServiceConfig();
                var flag = services.SendMail(To, body, subject, ecm.EmailAddress, ecm.Password, ecm.HostName, ecm.SMTPPortNo);
                TempData["success"] = flag;
                TempData["notice"] = "success";
                return RedirectToAction("CreateEmailConfig");
            }
            return View();
        }

        #endregion

        #region ----------- EmailConfigIndex ----
        public ActionResult EmailConfigIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.TblEmailConfigs.Where(a => a.firmid == firm).ToList());
        }

        #endregion 

        #region --------- DeleteEmailConfig -----
        public ActionResult DeleteEmailConfig(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblEmailConfig bts = db.TblEmailConfigs.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteEmailConfig(int id)
        {
            try
            {
                TblEmailConfig bts = db.TblEmailConfigs.Find(id);
                db.TblEmailConfigs.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("CreateEmailConfig");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("CreateEmailConfig");
            }
        }
        #endregion 

        #region ------- EditEmailConfig ---------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult EditEmailConfig(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblEmailConfig bts = db.TblEmailConfigs.Find(id);
            TblEmailConfigModel dept = new TblEmailConfigModel();
            dept.EmailConfigId = bts.EmailConfigId;
            dept.EmailAddress = bts.EmailAddress;
            dept.Password = bts.Password;
            dept.SMTPPortNo = bts.SMTPPortNo;
            dept.HostName = bts.HostName;
            dept.isActive = bts.isActive;

            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmailConfig(TblEmailConfig bts)
        {
            if (ModelState.IsValid)
            {
                TblEmailConfig ec = db.TblEmailConfigs.Find(bts.EmailConfigId);
                {
                    ec.EmailConfigId = bts.EmailConfigId;
                    ec.EmailAddress = bts.EmailAddress;
                    ec.Password = bts.Password;
                    ec.SMTPPortNo = bts.SMTPPortNo;
                    ec.HostName = bts.HostName;
                    ec.isActive = false;
                }
                db.TblEmailConfigs.AddOrUpdate(ec);
                db.SaveChanges();

                string To = bts.EmailAddress;
                var lnkHref = "<a href='http://people.innovative-fusion.com/MasterSetting/mailAuthentication?mail=" +
                    bts.EmailAddress + "'> Configure Your Email </a>";
                string subject = "Email Configuration Link";
                string body = "<b> Please click the link below. </b><br/>" + lnkHref;
                var services = new MailServiceConfig();
                var flag = services.SendMail(To, body, subject, bts.EmailAddress, bts.Password, bts.HostName, bts.SMTPPortNo);
                TempData["success"] = flag;
                TempData["notice"] = "success";

                return RedirectToAction("CreateEmailConfig");
            }
            return View(bts);
        } 
        #endregion 

        #region ------ mailAuthentication -------
        public ActionResult mailAuthentication(string mail)
        {
            var obj = db.TblEmailConfigs.FirstOrDefault(a => a.EmailAddress == mail);
            obj.isActive = true;
            db.TblEmailConfigs.AddOrUpdate(obj);
            db.SaveChanges();
            TempData["Success"] = "Email configure Successfully !!!";
            return RedirectToAction("Index", "Home");
        } 
        #endregion 

    }
}
