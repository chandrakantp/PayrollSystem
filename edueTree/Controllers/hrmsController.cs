using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using edueTree.Models;
using edueTree.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;

namespace edueTree.Controllers
{
    public class hrmsController : BaseController
    {        
        private dbContainer db = new dbContainer();

        #region -------- UserManager_Hrms --------
        public hrmsController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public hrmsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
    
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        #endregion

        #region ------------ Login ---------------

        [AllowAnonymous]
        public ActionResult login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return returnUrl == null ? RedirectToAction("StaffProfile", "Staffs") : RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    var firstOrDefault = db.AspNetUsers.FirstOrDefault(a => a.UserName == model.UserName);
                    if (firstOrDefault == null) return View(model);
                    var userId = firstOrDefault.Id;
                    if (model.UserName == "superadmin")
                    {
                        var identity =
                            await
                                UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                        GetAuthenticationManager()
                            .SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                        return returnUrl == null
                            ? RedirectToAction("FirmDashboard", "Firm")
                            : RedirectToLocal(returnUrl);
                    }
                    if (db.Staffs.Any(u => u.userId == userId && u.isActive == true && u.FirmInfo.isActive == true))
                    {
                        {
                            {
                                var identity =
                                    await
                                        UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                                GetAuthenticationManager()
                                    .SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                                var stsid = db.Staffs.FirstOrDefault(s => s.userId == userId);


                                var LtypeIdbal =
                                    db.LeaveTypes.FirstOrDefault(
                                        s => s.LeaveType1 == "Compensation Leaves" && s.firmId == stsid.firmId);
                                if (LtypeIdbal != null)
                                {
                                    var stdate =
                                        db.LeaveRecordNews.FirstOrDefault(
                                            s =>
                                                s.staffids == stsid.staffId && s.firmsId == stsid.firmId &&
                                                s.LevetypeIds == LtypeIdbal.leaveTypeId);
                                    if (stdate != null)
                                    {
                                        db.GetTotalCompansationnew(stsid.staffId, stdate.CreatedDate);
                                    }
                                }

                                IPHostGenerator gp = new IPHostGenerator();
                                var localip = gp.GetVisitorDetails();

                                DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
                                DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer

                                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local

                                var url = "http://freegeoip.net/json/" + localip;
                                var request = WebRequest.Create(url);

                                using (WebResponse wrs = request.GetResponse())
                                using (Stream stream = wrs.GetResponseStream())
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string json = reader.ReadToEnd();
                                    var obj = JObject.Parse(json);
                                    var City = (string)obj["city"];
                                    var State = (string)obj["region_name"];
                                    var CountryName = (string)obj["country_name"];
                                    var ZipCode = (string)obj["zip_code"];
                                    var TimeZone = (string)obj["time_zone"];
                                    var Latitude = (string)obj["latitude"];
                                    var Longitude = (string)obj["longitude"];

                                    var lrNew = new TblLoginRecord
                                    {
                                        Staffid = stsid.staffId,
                                        createdDate = localTime,
                                        firmid = stsid.firmId,
                                        City = City,
                                        State = State,
                                        CountryName = CountryName,
                                        ZipCode = ZipCode,
                                        TimeZone = TimeZone,
                                        Latitude = Latitude,
                                        Longitude = Longitude,
                                        IpAddress = localip
                                    };
                                    db.TblLoginRecords.Add(lrNew);
                                    db.SaveChanges();
                                }

                                var lcfirm =
                                    db.TblLoginCounters.FirstOrDefault(s => s.Lfirmid == stsid.firmId).LcounterId;

                                TblLoginCounter lc = db.TblLoginCounters.Find(lcfirm);
                                {
                                    var totalcounter = lc.Lcounter + 1;
                                    lc.Lcounter = totalcounter;
                                    lc.updateDate = localTime;

                                    db.TblLoginCounters.AddOrUpdate(lc);
                                    db.SaveChanges();
                                }

                                return returnUrl == null ? RedirectToAction("StaffProfile", "Staffs") : RedirectToLocal(returnUrl);
                            }
                        }
                    }
                    ModelState.AddModelError("CustomError", "You are not a valid Employee or your firm is not valid.");
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Invalid user-name or password.");
                }
                return View(model);
            }
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region -------- Forgot password ---------
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(LostPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string staffuserid = db.Staffs.Where(s => s.email == model.Email).FirstOrDefault().userId;
                    var user = UserManager.FindById(staffuserid);
                    if (user == null)
                    {
                        ViewData["error"] = "Your Email-Id are not valid";
                        return View("ForgotPassword");
                    }
                    var to = model.Email;
                    var code = Guid.NewGuid();

                    var lnkHref = "<a href='" + Url.Action("ResetPassword", "Account", new { id = user.Id, code }, "http") + "'>Reset Password</a>";

                    //HTML Template for Send email
                    new EmailManager();
                    const string subject = "HRMS - Reset Your Password";
                    var body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;
                    //Get and set the AppSettings using configuration manager.

                    var firm = db.Staffs.Where(s => s.email == model.Email).FirstOrDefault().firmId;
                    var dbs = db.TblEmailConfigs.FirstOrDefault(s => s.firmid == firm);

                    if (db == null)
                    {
                        var services = new MailService();
                        var flag = services.SendMail(to, body, subject, "noreplay@hrms.innovative-fusion.com");
                        ViewData["success"] = flag;
                    }
                    else
                    {
                        var services = new MailServiceConfig();
                        var flag = services.SendMail(to, body, subject, dbs.EmailAddress, dbs.Password, dbs.HostName, dbs.SMTPPortNo);
                        ViewData["success"] = flag;
                    }
                    //var callbackUrl = Url.Action("ResetPassword", "Account",
                    //new { id = user.Id, code = code }, protocol: Request.Url.Scheme);

                    //await UserManager.SendEmailAsync(user.Id, "Reset Password",
                    //"Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                    //return View("ForgotPassword");
                    //if (flag)
                    //{
                    //    ViewData["success"] = "Reset password link send on this email address. please Check -" +
                    //                          model.Email;
                    //}
                    //else
                    //{
                    //    ViewData["success"] = "Not";
                    //}
                }

                // If we got this far, something failed, redisplay form

            }
            catch (Exception)
            {
                ViewData["success"] = "Your Email-Id are not valid";
            }
            return View("ForgotPassword");
        }
        #endregion


        public ActionResult leavemanagement()
        {
            return View();
        }
      
        public ActionResult trainingmanagement()
        {
            return View();
        }
     
        public ActionResult performancemanagement()
        {
            return View();
        }      

        public ActionResult attendancemanagement()
        {
            return View();
        }

        public ActionResult elearning()
        {
            return View();
        }

        public ActionResult payrollmanagement()
        {
            return View();
        }

        public ActionResult noticemanagement()
        {
            return View();
        }

        public ActionResult trackerapp()
        {
            return View();
        }

        public ActionResult shiftmanagement()
        {
            return View();
        } 
            
        public ActionResult Features()
        {
            return View();
        }
        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult Resources()
        {
            return View();
        }
        
    }
}
