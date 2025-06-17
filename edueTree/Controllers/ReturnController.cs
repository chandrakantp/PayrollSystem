using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using edueTree.Models;
using edueTree.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace edueTree.Controllers
{
    public class ReturnController : Controller
    {
        #region ---------- DBContext ------------
        private dbContainer db = new dbContainer();
#endregion

        #region ------ Return Constructor -------
        public ReturnController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public ReturnController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;

        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        #endregion

        #region ---------- UserManager ----------
        public UserManager<ApplicationUser> UserManager { get; private set; }
        #endregion 

        #region ------- Paypal Get Method -------
        [HttpGet]
        public async Task<ActionResult> paypal(FormCollection form, UserRegistreModel adding1)
        {
            UserRegistreModel adding = (UserRegistreModel)TempData["model2"];
            string package = adding.package;
            //UserRegistreModel model = (UserRegistreModel) TempData["student"];
            //if (ModelState.IsValid)
            //{
            // var errors = ModelState.Values.SelectMany(v => v.Errors);
            try
            {
                //UserRegistreModel adding = (UserRegistreModel)TempData["model2"];
                var model3 = TempData["model3"] as string;
                var orDefault = db.TblUniversals.FirstOrDefault(a => a.UniversalId == 1);
                if (orDefault != null)
                {
                    var sameFirmname = db.FirmInfoes.FirstOrDefault(a => a.firmName == adding.FirmName);
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
                        var user = new ApplicationUser() { UserName = adding.Email };
                        var result = await UserManager.CreateAsync(user, adding.Password);

                        if (result.Succeeded)
                        {
                            var firstOrDefault = db.AspNetUsers.FirstOrDefault(a => a.UserName == adding.Email);
                            if (firstOrDefault != null)
                            {
                                var userids = firstOrDefault.Id;
                                if (uni != null)
                                {
                                    if (package == "yer")
                                    {
                                        var firmIn = new FirmInfo
                                        {
                                            firmName = adding.FirmName,
                                            renewalDate = DateTime.Now,
                                            validDate = DateTime.Now.AddYears(1),
                                            email = adding.Email,
                                            contact = adding.Contact,
                                            ipFiltering = false,
                                            UniversalId = uni.UniIdFirm,
                                            CompanyId = adding.CompanyId,
                                            isActive = true,
                                            Noofemp = Convert.ToString(adding.NoOfEmployees),
                                            TotalAmount = Convert.ToDecimal(model3),
                                            paymethod = adding.Payment,
                                            currency = adding.price,
                                            package = adding.package
                                        };
                                        var asprole = new AspNetRole()
                                        {
                                            Name = "Admin of " + adding.FirmName,
                                            FirmInfo = firmIn,
                                        };
                                        if (uniEmployee != null)
                                        {
                                            var staffs = new Staff
                                            {
                                                userId = userids,
                                                FirmInfo = firmIn,
                                                firstName = adding.FirstName,
                                                middleName = adding.MiddleName,
                                                lastName = adding.LastName,
                                                contact = adding.Contact,
                                                email = adding.Email,
                                                isActive = true,
                                                joingDate = DateTime.Now,
                                                schoolCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                staffCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                CompanyId = adding.CompanyId
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
                                    else
                                    {
                                        var firmIn = new FirmInfo
                                        {
                                            firmName = adding.FirmName,
                                            renewalDate = DateTime.Now,
                                            validDate = DateTime.Now.AddDays(30),
                                            email = adding.Email,
                                            contact = adding.Contact,
                                            ipFiltering = false,
                                            UniversalId = uni.UniIdFirm,
                                            CompanyId = adding.CompanyId,
                                            isActive = true,
                                            Noofemp = Convert.ToString(adding.NoOfEmployees),
                                            TotalAmount = Convert.ToDecimal(model3),
                                            paymethod = adding.Payment,
                                            currency = adding.price,
                                            package = adding.package
                                        };
                                        var asprole = new AspNetRole()
                                        {
                                            Name = "Admin of " + adding.FirmName,
                                            FirmInfo = firmIn,
                                        };
                                        if (uniEmployee != null)
                                        {
                                            var staffs = new Staff
                                            {
                                                userId = userids,
                                                FirmInfo = firmIn,
                                                firstName = adding.FirstName,
                                                middleName = adding.MiddleName,
                                                lastName = adding.LastName,
                                                contact = adding.Contact,
                                                email = adding.Email,
                                                isActive = true,
                                                joingDate = DateTime.Now,
                                                schoolCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                staffCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                CompanyId = adding.CompanyId
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
                            }
                            string mailTo = adding.Email, userId, password, smtpPort, host, firmName = db.FirmInfoes.FirstOrDefault().firmName;
                            string subject = "Welcome to " + firmName;
                            string body = "Welcome " + adding.FirstName + ",<br/> <p>Thank you for joining up our organization. Here is your login details,</p></br>";
                            body = body + " <p><b> Username:</b> " + adding.Email + ",</p><p><b> Password:</b> " + adding.Password + "</p><br/><br/> <p><b> Thank you,</b></p><p>The HR Team.</p>";
                            var services = new MailService();
                            var flag = services.SendMail(mailTo, body, subject, "info@innovative-fusion.com");

                            //await SignInAsync(user, isPersistent: false);
                            var identity =
                                await
                                    UserManager.CreateIdentityAsync(user,
                                        DefaultAuthenticationTypes.ApplicationCookie);
                            GetAuthenticationManager()
                                .SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
                            //TempData["Message1"] = " !!! Your Order ID is :" + order_id;
                            return RedirectToAction("StaffProfile", "Staffs");
                        }
                        else
                        {
                            UserRegistreModel addingg = (UserRegistreModel)TempData["model2"];
                            ModelState.AddModelError("CustomError", "Email address already exists !!!");
                            TempData["Message1"] = "Email address already exists !!!";
                            return RedirectToAction("Pricing", "hrms", addingg);
                        }

                    }
                    else
                    {
                        UserRegistreModel addingg = (UserRegistreModel)TempData["model2"];
                        ModelState.AddModelError("CustomError", "Firm name already exists !!!");
                        TempData["Message1"] = "Firm name already exists !!!";
                        return RedirectToAction("Pricing", "hrms", addingg);
                    }
                }
            }
            catch (Exception)
            {
                UserRegistreModel addingg = (UserRegistreModel)TempData["model2"];
                var userids = db.AspNetUsers.FirstOrDefault(a => a.UserName == addingg.Email);
                db.AspNetUsers.Remove(userids);
                db.SaveChanges();
                //return View(addingg);
                return RedirectToAction("Pricing", "hrms", addingg);
            }
            //return View(adding1);
            //}
            return RedirectToAction("Pricing", "hrms");
        }
#endregion

        #region  ------- Return post Method -----
        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form, UserRegistreModel model)
        {
            UserRegistreModel adding = (UserRegistreModel)TempData["model"];
            var message = TempData["message"] as string;
            string package = adding.package;
            var model3 = TempData["model3"] as string;
            if (message == "payumoney")
            {
                try
                {
                    //UserRegistreModel adding = (UserRegistreModel)TempData["model"];
                    string[] merc_hash_vars_seq;
                    string merc_hash_string = string.Empty;
                    string merc_hash = string.Empty;
                    string order_id = string.Empty;
                    string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                    if (form["status"].ToString() == "success")
                    {
                        merc_hash_vars_seq = hash_seq.Split('|');
                        Array.Reverse(merc_hash_vars_seq);
                        merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + form["status"].ToString();


                        foreach (string merc_hash_var in merc_hash_vars_seq)
                        {
                            merc_hash_string += "|";
                            merc_hash_string = merc_hash_string + (form[merc_hash_var] != null ? form[merc_hash_var] : "");

                        }
                        Response.Write(merc_hash_string);
                        merc_hash = Generatehash512(merc_hash_string).ToLower();

                        if (merc_hash != form["hash"])
                        {
                            Response.Write("Hash value did not matched");
                        }
                        else
                        {
                            order_id = Request.Form["txnid"];
                            // TempData["Message"] = "Your Payment is successful. Hash value is matched. Your Order ID is:" +order_id;
                            // Response.Write("<br/>Hash value matched"); --current code
                            TempData["Message"] = "Your Payment is successful. Your Order ID is :" + order_id;

                            //if (ModelState.IsValid)
                            //{
                            // var errors = ModelState.Values.SelectMany(v => v.Errors);
                            try
                            {
                                //UserRegistreModel adding = (UserRegistreModel)TempData["model2"];
                                //var model3 = TempData["model3"] as string;
                                var orDefault = db.TblUniversals.FirstOrDefault(a => a.UniversalId == 1);
                                if (orDefault != null)
                                {
                                    var sameFirmname = db.FirmInfoes.FirstOrDefault(a => a.firmName == adding.FirmName);
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
                                        var user = new ApplicationUser() { UserName = adding.Email };
                                        var result = await UserManager.CreateAsync(user, adding.Password);

                                        if (result.Succeeded)
                                        {
                                            var firstOrDefault = db.AspNetUsers.FirstOrDefault(a => a.UserName == adding.Email);
                                            if (firstOrDefault != null)
                                            {
                                                var userids = firstOrDefault.Id;
                                                if (uni != null)
                                                {
                                                    if (package == "yer")
                                                    {
                                                        var firmIn = new FirmInfo
                                                        {
                                                            firmName = adding.FirmName,
                                                            renewalDate = DateTime.Now,
                                                            validDate = DateTime.Now.AddYears(1),
                                                            email = adding.Email,
                                                            contact = adding.Contact,
                                                            ipFiltering = false,
                                                            UniversalId = uni.UniIdFirm,
                                                            CompanyId = adding.CompanyId,
                                                            isActive = true,
                                                            Noofemp = Convert.ToString(adding.NoOfEmployees),
                                                            TotalAmount = Convert.ToDecimal(model3),
                                                            paymethod = adding.Payment,
                                                            currency = adding.price,
                                                            package = adding.package
                                                        };
                                                        var asprole = new AspNetRole()
                                                        {
                                                            Name = "Admin of " + adding.FirmName,
                                                            FirmInfo = firmIn,
                                                        };
                                                        if (uniEmployee != null)
                                                        {
                                                            var staffs = new Staff
                                                            {
                                                                userId = userids,
                                                                FirmInfo = firmIn,
                                                                firstName = adding.FirstName,
                                                                middleName = adding.MiddleName,
                                                                lastName = adding.LastName,
                                                                contact = adding.Contact,
                                                                email = adding.Email,
                                                                isActive = true,
                                                                joingDate = DateTime.Now,
                                                                schoolCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                                staffCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                                CompanyId = adding.CompanyId
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
                                                    else
                                                    {
                                                        var firmIn = new FirmInfo
                                                        {
                                                            firmName = adding.FirmName,
                                                            renewalDate = DateTime.Now,
                                                            validDate = DateTime.Now.AddDays(30),
                                                            email = adding.Email,
                                                            contact = adding.Contact,
                                                            ipFiltering = false,
                                                            UniversalId = uni.UniIdFirm,
                                                            CompanyId = adding.CompanyId,
                                                            isActive = true,
                                                            Noofemp = Convert.ToString(adding.NoOfEmployees),
                                                            TotalAmount = Convert.ToDecimal(model3),
                                                            paymethod = adding.Payment,
                                                            currency = adding.price,
                                                            package = adding.package
                                                        };
                                                        var asprole = new AspNetRole()
                                                        {
                                                            Name = "Admin of " + adding.FirmName,
                                                            FirmInfo = firmIn,
                                                        };
                                                        if (uniEmployee != null)
                                                        {
                                                            var staffs = new Staff
                                                            {
                                                                userId = userids,
                                                                FirmInfo = firmIn,
                                                                firstName = adding.FirstName,
                                                                middleName = adding.MiddleName,
                                                                lastName = adding.LastName,
                                                                contact = adding.Contact,
                                                                email = adding.Email,
                                                                isActive = true,
                                                                joingDate = DateTime.Now,
                                                                schoolCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                                staffCode = uni.UniIdFirm + "" + uniEmployee.UniIdEmployee,
                                                                CompanyId = adding.CompanyId
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
                                            }
                                            string mailTo = adding.Email, userId, password, smtpPort, host, firmName = db.FirmInfoes.FirstOrDefault().firmName;
                                            string subject = "Welcome to " + firmName;
                                            string body = "Welcome " + adding.FirstName + ",<br/> <p>Thank you for joining up our organization. Here is your login details,</p></br>";
                                            body = body + " <p><b> Username:</b> " + adding.Email + ",</p><p><b> Password:</b> " + adding.Password + "</p><br/><br/> <p><b> Thank you,</b></p><p>The HR Team.</p>";
                                            var services = new MailService();
                                            var flag = services.SendMail(mailTo, body, subject, "info@innovative-fusion.com");

                                            //await SignInAsync(user, isPersistent: false);
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
                                            UserRegistreModel addingg = (UserRegistreModel)TempData["model2"];
                                            ModelState.AddModelError("CustomError", "Email address already exists !!!");
                                            TempData["Message1"] = "Email address already exists !!!";
                                            return RedirectToAction("Pricing", "hrms", addingg);
                                        }

                                    }
                                    else
                                    {
                                        UserRegistreModel addingg = (UserRegistreModel)TempData["model2"];
                                        ModelState.AddModelError("CustomError", "Firm name already exists !!!");
                                        TempData["Message1"] = "Firm name already exists !!!";
                                        return RedirectToAction("Pricing", "hrms", addingg);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                UserRegistreModel addingg = (UserRegistreModel)TempData["model2"];
                                var userids = db.AspNetUsers.FirstOrDefault(a => a.UserName == addingg.Email);
                                db.AspNetUsers.Remove(userids);
                                db.SaveChanges();
                                //return View(addingg);
                                return RedirectToAction("Pricing", "hrms", addingg);
                            }
                            //return View(adding1);
                            //}
                            return RedirectToAction("Pricing", "hrms");
                        }
                    }
                    else
                    {
                        UserRegistreModel add = (UserRegistreModel)TempData["model"];
                        var check = new FailureSignupFirm()
                        {
                            UniversalId = add.FirstName,
                            middleName = add.MiddleName,
                            lastName = add.LastName,
                            TotalAmount = Convert.ToDecimal(model3),
                            DateTime = DateTime.Now,
                            Noofemp = add.NoOfEmployees,
                            email = add.Email,
                            contact = add.Email,
                            firmName = add.FirmName,
                            paymethod = add.Payment,
                            currency = add.price,
                            package = add.package
                        };
                        db.FailureSignupFirms.Add(check);
                        db.SaveChanges();
                        TempData["Message1"] = "Your order was cancelled.";
                        return RedirectToAction("Pricing", "hrms");
                    }
                }
                catch (Exception ex)
                {
                    //Response.Write("<span style='color:red'>" + ex.Message + "</span>");
                    TempData["Message"] = ex.Message;
                }
            }
            //}
            return View();
        }
        #endregion

        #region ------- Return Failure ----------
        public ActionResult Return1(FormCollection form, UserRegistreModel model)
        {
            //UserRegistreModel modeljh = (UserRegistreModel) TempData["UserRegistreModel"];
            var model3 = TempData["model3"] as string;

            UserRegistreModel add = (UserRegistreModel)TempData["model"];
            var check = new FailureSignupFirm()
             {
                 TotalAmount = Convert.ToDecimal(model3),
                 DateTime = DateTime.Now,
                 UniversalId = add.FirstName,
                 middleName = add.MiddleName,
                 lastName = add.LastName,
                 email = add.Email,
                 contact = add.Contact,
                 firmName = add.FirmName,
                 paymethod = add.Payment,
                 currency = add.price,
                 package = add.package
             };
            db.FailureSignupFirms.Add(check);
            db.SaveChanges();
            TempData["Message1"] = "Your order was cancelled.";
            return RedirectToAction("Pricing", "hrms");
            //return View();
        }
#endregion

        #region ---- Old Comment Code -----------
        //   [HttpGet]
        // public ActionResult Return()
        // {
        //     return View();
        // }

        //[HttpPost]
        //public ActionResult Return(FormCollection form)
        //{
        //    try
        //    {
        //        string[] merc_hash_vars_seq;
        //        string merc_hash_string = string.Empty;
        //        string merc_hash = string.Empty;
        //        string order_id = string.Empty;
        //        string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

        //        if (form["status"].ToString() == "success")
        //        {
        //            merc_hash_vars_seq = hash_seq.Split('|');
        //            Array.Reverse(merc_hash_vars_seq);
        //            merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + form["status"].ToString();


        //            foreach (string merc_hash_var in merc_hash_vars_seq)
        //            {
        //                merc_hash_string += "|";
        //                merc_hash_string = merc_hash_string + (form[merc_hash_var] != null ? form[merc_hash_var] : "");

        //            }
        //            Response.Write(merc_hash_string);
        //            merc_hash = Generatehash512(merc_hash_string).ToLower();

        //            if (merc_hash != form["hash"])
        //            {
        //                Response.Write("Hash value did not matched");
        //            }
        //            else
        //            {
        //                order_id = Request.Form["txnid"];                        
        //                TempData["Message"] = "Your Payment is successful. Hash value is matched. Your Order ID is:" +order_id;
        //                 Response.Write("<br/>Hash value matched"); --current code
        //                TempData["Message"] = "Your Payment is successful. Your Order ID is :" + order_id;

        //                Insert firm here if payment successfully transfer.
        //                 var firm = new FirmInfo()
        //                {                        
        //                };
        //                 db.FirmInfoes.Add(firm);
        //                db.SaveChanges();



        //                var add = new PaymentInfo()
        //                {
        //                    orderid = order_id,
        //                    IsPayumoney = true,
        //                    PaymentDate = DateTime.Now,
        //                };
        //                db.PaymentInfoes.Add(add);
        //                db.SaveChanges();

        //                return RedirectToAction("Return", "Return");                        
        //            }
        //        }
        //        else
        //        {
        //            TempData["Message"] = "Hash value did not matched";                   
        //            return RedirectToAction("Return", "Return");
        //            Response.Write("Hash value did not matched"); --current code
        //            osc_redirect(osc_href_link(FILENAME_CHECKOUT, 'payment' , 'SSL', null, null,true));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<span style='color:red'>" + ex.Message + "</span>");
        //        TempData["Message"] = ex.Message;
        //    }
        //    return View();
        //}
        #endregion

        #region ------ GeneratehashValue --------
        public string Generatehash512(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        #endregion
    }
}
