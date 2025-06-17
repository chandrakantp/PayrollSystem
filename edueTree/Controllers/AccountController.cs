using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
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
    public class AccountController : BaseController
    {
        #region ------------------ DbContext ------------------
        private dbContainer db = new dbContainer();
        #endregion

        #region -------------- AccountController --------------
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;

        }

        #endregion

        #region --------------- UserManager -------------------
        public UserManager<ApplicationUser> UserManager { get; private set; }
        #endregion

        #region ----------- GetAuthenticationManager ----------
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        #endregion

        #region --------------------- Login -------------------
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //string mailTo = "patilchandrakant98@gmail.com", userId, password, smtpPort, host, firmName = db.FirmInfoes.FirstOrDefault().firmName;
            //string subject = "Welcome to " + firmName;
            //string body = "Welcome " + "Ch" + ",<br/> <p>Thank you for joining up our organization. Here is your login details,</p></br>";
            //body = body + " <p><b> Username:</b> " + "patilchandrakant98@gmail.com" + ",</p><p><b> Password:</b> " + "123456" + "</p><br/><br/> <p><b> Thank you,</b></p><p>The HR Team.</p>";
            //var services = new MailService();
            //var flag = services.SendMail(mailTo, body, subject, "innovativefusiontest@gmail.com");

            if (User.Identity.IsAuthenticated)
            {
                return returnUrl == null ? RedirectToAction("StaffProfile", "Staffs") : RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        #endregion

        #region ------------------- GetPublicIP ---------------
        [AllowAnonymous]
        public string GetPublicIp()
        {
            String direction;
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                direction = stream.ReadToEnd();
            }
            int first = direction.IndexOf("Address: ", StringComparison.Ordinal) + 9;
            int last = direction.LastIndexOf("</body>", StringComparison.Ordinal);
            direction = direction.Substring(first, last - first);

            return direction;
        }
        #endregion       

        #region ------------- CityStateCountByIp --------------
        //public static string CityStateCountByIp(string IP)
        //{
        //    var URL = "http://freegeoip.net/json/" + IP;
        //    var request = System.Net.WebRequest.Create(URL);

        //    using (WebResponse wrs = request.GetResponse())
        //    using (Stream stream = wrs.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        string json = reader.ReadToEnd();
        //        var obj = JObject.Parse(json);
        //        var City = (string)obj["city"];
        //        var State = (string)obj["region_name"];
        //        var CountryName = (string)obj["country_name"];

        //        return (City);
        //    }
        //    return "";
        //}

        #endregion

        #region ----------------- Login Post ------------------

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
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

                                //var localip ="";
                                //var host = Dns.GetHostEntry(Dns.GetHostName());
                                //foreach (var ip in host.AddressList)
                                //{
                                //    if (ip.AddressFamily == AddressFamily.InterNetwork)
                                //    {
                                //         localip =  ip.ToString();
                                //    }
                                //}


                                
                                 IPHostGenerator gp = new IPHostGenerator();
                                 var localip =  gp.GetVisitorDetails();

                                //var getipaddress = GetPublicIp();
                                // var cityip = CityStateCountByIp(getipaddress);


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
                                //var login90 =
                                //    db.TblLoginRecords.Where(s => s.firmid == stsid.firmId).Count();

                                //if (login90 >= 47)
                                //{
                                //    var loginLast = db.TblLoginRecords.Where(s => s.firmid == stsid.firmId).Last();
                                //    TblLoginRecord master = db.TblLoginRecords.Find(loginLast);
                                //    db.TblLoginRecords.Remove(master);
                                //    db.SaveChanges();
                                //}
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

        #endregion 

        #region ---------------- Register ---------------------
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        #endregion
      
        #region ----------------- RegisterPost ----------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region ----------------- Disassociate ----------------
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            ManageMessageId? message = result.Succeeded ? ManageMessageId.RemoveLoginSuccess : ManageMessageId.Error;
            return RedirectToAction("Manage", new { Message = message });
        }
        #endregion

        #region ------------------- Manage --------------------
        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }


        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["notice"] = "change";
                        return RedirectToAction("Manage");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });

                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["notice"] = "change";
                        return RedirectToAction("Manage");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region -------------- ExternalLogin ------------------
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        #endregion

        #region ----------- ExternalLoginCallback -------------
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        #endregion

        #region ----------------- LinkLogin -------------------
        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        #endregion

        #region ------------ LinkLoginCallback ----------------
        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }
        #endregion
             
        #region -------------- ExternalLoginConfirmation ------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        #endregion

        #region ------------------ LogOff ---------------------
        //
        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {            
            AuthenticationManager.SignOut();
            return RedirectToAction("login", "hrms");
        }
        #endregion

        #region ------------ ExternalLoginFailure -------------
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #endregion

        #region -------------- RemoveAccountList --------------
        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return PartialView("_RemoveAccountPartial", linkedAccounts);
        }
        #endregion

        #region ------------- Des-pose ------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region --------------- Helpers -----------------------
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
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

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            private string LoginProvider { get; set; }
            private string RedirectUri { get; set; }
            private string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion 

        #region ------------ ResetPassword --------------------
        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResetPasswordViewModel model = new ResetPasswordViewModel() { Id = id };
            return View(model);
        }

        #endregion

        #region ------------- Post ResetPassword --------------
        //
        // POST: /AccountAdmin/ResetPassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var removePassword = UserManager.RemovePassword(model.Id);
            if (removePassword.Succeeded)
            {
                //Removed Password Success
                var addPassword = UserManager.AddPassword(model.Id, model.Password);
                if (addPassword.Succeeded)
                {
                    TempData["ResetPasswordSuccess"] = "Your password reset successfully !!!";
                    return RedirectToAction("Login", "hrms");
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region ------------- Forgot password -----------------
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword()
        {
            return View();
        }

        #endregion 

        #region --------------- ForgetPasswordPost ------------
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
                    var body = "<b> Please find the Password Reset Link. </b><br/>" + lnkHref;
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
            catch (Exception )
            {
                ViewData["success"] = "Your Email-Id are not valid";
            }
            return View("ForgotPassword");
        }

        #endregion

        #region --------------- Confirm mail ------------------

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            IdentityResult result;
            try
            {
                result = await UserManager.ConfirmEmailAsync(userId, code);
            }
            catch (InvalidOperationException ioe)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                ViewBag.errorMessage = ioe.Message;
                return View("Error");
            }

            if (result.Succeeded)
            {
                return View();
            }

            // If we got this far, something failed.
            AddErrors(result);
            ViewBag.errorMessage = "ConfirmEmail failed";
            return View("Error");
        }

        #endregion

        #region ------------- ResetPasswordComment ------------
        //[AllowAnonymous]
        //public ActionResult ResetPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult ResetPassword(ResetPasswordModel model, string code ,string emailaddress)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //AspNetUser user = _repoAspDotNetUser.GetAspNetUser(email);
        //        //UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //        UserManager.GetEmailAsync()
        //        var getmail = db.Staffs.Where(a => a.email == emailaddress);

        //        if (getmail != null)
        //        {
        //            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.Password);
        //            bool result = UserManager.GeneratePasswordResetToken(emailaddress, code, hashedNewPassword);

        //            //if (result)
        //            //{
        //                ModelState.AddModelError("", "Please return to the login page and enjoy with new password.");
        //            //}
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "It's not a valid, this attempt is already processed.");
        //        }
        //    }
        //    return View();
        //}
        #endregion

        #region -------------- UserRegister -------------------
        [AllowAnonymous]
        public ActionResult UserRegister()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRegister(UserRegistreModel model)
        {
            if (ModelState.IsValid)
            {
                // var errors = ModelState.Values.SelectMany(v => v.Errors);
                try
                {
                    var orDefault = db.TblUniversals.FirstOrDefault(a => a.UniversalId == 1);
                    if (orDefault != null)
                    {
                        var sameFirmname = db.FirmInfoes.FirstOrDefault(a => a.firmName == model.FirmName);
                        if (sameFirmname == null)
                        {
                            var prifix = Convert.ToDecimal(orDefault.UniIdFirm) + 1;
                            var uni = db.TblUniversals.FirstOrDefault(a => a.UniIdIsActive == true);
                            if (uni != null)
                                uni.UniIdFirm = Convert.ToString(prifix, CultureInfo.InvariantCulture);
                            db.TblUniversals.AddOrUpdate(uni);

                            const string prifixStudent = "00001";
                            var uniEmployee = db.TblUniversals.FirstOrDefault(a => a.UniIdIsActive == true);
                            if (uniEmployee != null)
                                if (uni != null) uni.UniIdEmployee = prifixStudent;
                            db.TblUniversals.AddOrUpdate(uniEmployee);
                            var user = new ApplicationUser() { UserName = model.Email };
                            var result = await UserManager.CreateAsync(user, model.Password);

                            if (result.Succeeded)
                            {
                                var firstOrDefault = db.AspNetUsers.FirstOrDefault(a => a.UserName == model.Email);
                                if (firstOrDefault != null)
                                {
                                    var userids = firstOrDefault.Id;
                                    if (uni != null)
                                    {
                                        var firmIn = new FirmInfo
                                        {
                                            firmName = model.FirmName,
                                            renewalDate = DateTime.Now,
                                            validDate = DateTime.Now.AddDays(30),
                                            email = model.Email,
                                            contact = model.Contact,
                                            ipFiltering = false,
                                            UniversalId = uni.UniIdFirm,
                                            CompanyId = model.CompanyId,
                                            isActive = true,
                                            Noofemp = Convert.ToString(model.NoOfEmployees),
                                            TotalAmount = model.Amount
                                        };
                                        var asprole = new AspNetRole()
                                        {
                                            Name = "Admin of " + model.FirmName,
                                            FirmInfo = firmIn,
                                        };
                                        if (uniEmployee != null)
                                        {
                                            var staffs = new Staff
                                            {
                                                userId = userids,
                                                FirmInfo = firmIn,
                                                firstName = model.FirstName,
                                                middleName = model.MiddleName,
                                                lastName = model.LastName,
                                                contact = model.Contact,
                                                email = model.Email,
                                                isActive = true,
                                                joingDate = DateTime.Now,
                                                schoolCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                staffCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                CompanyId = model.CompanyId
                                            };
                                            var userrole = new UserRole()
                                            {
                                                userId = userids,
                                                AspNetRole = asprole,
                                            };
                                            var menuitms = db.MenuItems.Where(a => a.isActive == true);
                                            foreach (var menu in menuitms)
                                            {
                                                var per = new Permission()
                                                {
                                                    menuItemId = menu.menuItemId,
                                                    AspNetRole = asprole,
                                                    IsActive = true
                                                };
                                                db.Permissions.Add(per);
                                            }

                                            var lpcounter = new TblLoginCounter()
                                            {
                                                FirmInfo = firmIn,
                                                Lcounter = 0,
                                                updateDate = DateTime.Now,
                                            };

                                            db.FirmInfoes.Add(firmIn);
                                            db.Staffs.Add(staffs);
                                            db.AspNetRoles.Add(asprole);
                                            db.UserRoles.Add(userrole);
                                            db.TblLoginCounters.Add(lpcounter);

                                            db.SaveChanges();

                                            var ltype = new LeaveType()
                                            {
                                                LeaveType1 = "Paid Leaves",
                                                firmId = firmIn.firmId,
                                            };
                                            db.LeaveTypes.Add(ltype);

                                            var ltype1 = new LeaveType()
                                            {
                                                LeaveType1 = "Compensation Leaves",
                                                firmId = firmIn.firmId,
                                            };
                                            db.LeaveTypes.Add(ltype1);

                                            var ltype2 = new LeaveType()
                                            {
                                                LeaveType1 = "Informed Absent",
                                                firmId = firmIn.firmId,
                                            };
                                            db.LeaveTypes.Add(ltype2);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                                string mailTo = model.Email, userId, password, smtpPort, host, firmName = db.FirmInfoes.FirstOrDefault().firmName;
                                string subject = "Welcome to " + firmName;
                                string body = "Welcome " + model.FirstName + ",<br/> <p>Thank you for joining up our organization. Here is your login details,</p></br>";
                                body = body + " <p><b> Username:</b> " + model.Email + ",</p><p><b> Password:</b> " + model.Password + "</p><br/><br/> <p><b> Thank you,</b></p><p>The HR Team.</p>";
                                var services = new MailService();
                                var flag = services.SendMail(mailTo, body, subject, "innovativefusiontest@gmail.com");

                                await SignInAsync(user, isPersistent: false);
                                return RedirectToAction("StaffProfile", "Staffs");
                            }
                            else
                            {
                                ModelState.AddModelError("CustomError", "Email address already exists !!!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Firm name already exists !!!");
                        }
                    }
                }
                catch (Exception)
                {
                    var userids = db.AspNetUsers.FirstOrDefault(a => a.UserName == model.Email);
                    db.AspNetUsers.Remove(userids);
                    db.SaveChanges();
                    return View(model);
                }
                return View(model);
            }
            return View(model);
        }
        #endregion

        #region ---------------- UserLogin --------------------
        public async Task<JsonResult> UserLogin(string email, string password)
        {
            int flag = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    flag = 1;
                    using (dbContainer entities = new dbContainer())
                    {
                        flag = 2;
                        bool userValid = entities.Staffs.Any(user => user.email == email);
                        if (userValid)
                        {
                            flag = 3;
                            var dbUser = entities.Staffs.FirstOrDefault(u => u.email == email);
                            if (dbUser != null)
                            {
                                Dictionary<string, string> userDict = new Dictionary<string, string>();
                                userDict.Add("UserId", dbUser.userId.ToString());
                                userDict.Add("Name", dbUser.firstName + " " + dbUser.lastName);
                                userDict.Add("Email", dbUser.email);
                                userDict.Add("MobileNo", dbUser.contact);
                                return Json(userDict, JsonRequestBehavior.AllowGet);
                            }
                            return Json(flag, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(flag, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(flag + " " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ------- Change Password For Superadmin --------
        public ActionResult ChangePasswordForSuperadmin(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("ChangePasswordForSuperadmin");
            return View();
        }


        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordForSuperadmin(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("ChangePasswordForSuperadmin");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["notice"] = "change";
                        return RedirectToAction("ChangePasswordForSuperadmin");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });

                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["notice"] = "change";
                        return RedirectToAction("ChangePasswordForSuperadmin");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

    }
}

public class IPHostGenerator
{
    internal string GetCurrentPageUrl()
    {
        return HttpContext.Current.Request.Url.AbsoluteUri;
    }
    internal string GetVisitorDetails()
    {
        string varIPAddress = string.Empty;
        string varVisitorCountry = string.Empty;
        string varIpAddress = string.Empty;
        varIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(varIpAddress))
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                varIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }

        //varIPAddress = (System.Web.UI.Page)Request.ServerVariables["HTTP_X_FORWARDED_FOR"];    
        if (varIPAddress == "" || varIPAddress == null)
        {
            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                varIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        //varIPAddress = Request.ServerVariables["REMOTE_ADDR"];    
        return varIpAddress;
    }      
}