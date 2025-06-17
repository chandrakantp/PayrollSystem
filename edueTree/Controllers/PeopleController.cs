using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class PeopleController : BaseController
    {            
        #region -------- User-Register ----------
        [AllowAnonymous]
        public ActionResult UserRegisterViaPayuMoney(string plan, string employee, string package, string price)
        {
            var data = new UserRegistreModel()
            {
                Amount = Convert.ToDecimal(plan),
                NoOfEmployees = Convert.ToString(employee),
                package = package,
                price = price
            };

            return View(data);
        }
        #endregion 

        #region ------- Plans Get Method --------
        public ActionResult Plans(string plan)
        {
               decimal? planamt = 0;

                switch (plan)
                {
                    case "startup540":
                        planamt = 540;
                        break;
                    case "startup1140":
                        planamt = 1140;
                        break;
                    case "startup2940":
                        planamt = 2940;
                        break;
                    case "startup5940":
                        planamt = 5940;
                        break;
                    case "startup11940":
                        planamt = 11940;
                        break;
                    case "yearly5400":
                        planamt = 5400;
                        break;
                    case "yearly11400":
                        planamt = 11400;
                        break;
                    case "yearly29400":
                        planamt = 29400;
                        break;
                    case "yearly59400":
                        planamt = 59400;
                        break;
                    case "yearly119400":
                        planamt = 119400;
                        break;                   
                }

                var price = new PricingModel()
                {
                    Amount = Convert.ToString(planamt),
                };

                return View(price);
        }
        #endregion

        #region ------- Plans Post Method -------
        [HttpPost]
        public void Plans(UserRegistreModel model)
        {
            string payment = model.Payment;
            if (payment == "paypal")
            {
                string ramount = Convert.ToString(model.Amount);
                TempData["model3"] = ramount;
                TempData["model"] = model;
                TempData["message"] = payment;
                string redirecturl = "";
                string firstName = model.FirstName;
                //string amount = Convert.ToString(model.Amount);
                //string productInfo = "HRMS";
                string itemInfo = "HRMS";
                string email = model.Email;
                string phone = model.Contact;
                string middleName = model.MiddleName;
                string lastName = model.LastName;
                string Noofemp = model.NoOfEmployees;
                string FirmName = model.FirmName;
                string price = model.price;
                string package = model.package;
                //var PAmount = new List<UserRegistreModel>();
                //TempData["UserRegistreModel"] = model;
                //if (model.Amount == 540 || model.Amount == 1140 || model.Amount == 2940 || model.Amount == 5940 || model.Amount == 11940)
                if (price == "rupees")
                {
                    //string paypal = "model.Amount";
                    model.Amount = model.Amount / 70;

                } string amount = Convert.ToString(model.Amount);
                //string currency = "INR";
                //Mention URL to redirect content to paypal site
                redirecturl += "https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=" +
                    //redirecturl += "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business=" +
                ConfigurationManager.AppSettings["paypalemail"].ToString();
                //ConfigurationManager.AppSettings["currency_code"].ToString();
                TempData["message"] = model.Payment;
                //First name i assign static based on login details assign this value
                redirecturl += "&first_name=" + firstName;

                //City i assign static based on login user detail you change this value
                //redirecturl += "&city=Pune";

                ////State i assign static based on login user detail you change this value
                //redirecturl += "&state=Maharastra";

                ////Product Name
                //redirecturl += "&item_name=" + productInfo;

                //Product Name
                redirecturl += "&amount=" + amount;

                //Phone No
                redirecturl += "&night_phone_a=" + phone;

                //Product Name
                redirecturl += "&item_name=" + itemInfo;

                //Address
                redirecturl += "&email=" + email;

              
                TempData["model2"] = model;
                //Success return page url
                redirecturl += "&return=" + ConfigurationManager.AppSettings["SuccessURL"].ToString();

                //Failed return page url
                redirecturl += "&cancel_return=" + ConfigurationManager.AppSettings["FailedURL"].ToString();

                Response.Redirect(redirecturl);
            }
            else
            {
                string firstName = model.FirstName;
                string middleName = model.MiddleName;
                string lastName = model.LastName;
                //string amount = Convert.ToString(model.Amount);
                string Noofemp = model.NoOfEmployees;
                //string currency = "INR";
                string productInfo = "HRMS";
                string email = model.Email;
                string phone = model.Contact;
                string FirmName = model.FirmName;
                string ramount = Convert.ToString(model.Amount);
                TempData["model3"] = ramount;
                TempData["message"] = model.Payment;
                string price = model.price;
                //string surl = form["txtsurl"].ToString();
                //string furl = form["txtfurl"].ToString();
                //TempData["student"] = model.Amount;
                //TempData["model2"] = model.Amount;
                RemotePost myremotepost = new RemotePost();
                string key = "cOyLul";
                string salt = "sOpcIt7y";
                if (price == "yer")
                {
                    //string paypal = "model.Amount";
                    model.Amount = model.Amount * 70;

                } string amount = Convert.ToString(model.Amount);
                //posting all the parameters required for integration.
                TempData["model"] = model;
                //myremotepost.Url = "https://test.payu.in/_payment";
                myremotepost.Url = "https://secure.payu.in/_payment";
                myremotepost.Add("key", "cOyLul");
                string txnid = Generatetxnid();
                myremotepost.Add("txnid", txnid);
                myremotepost.Add("amount", amount);
                //myremotepost.Add("currency", currency);
                myremotepost.Add("productinfo", productInfo);
                myremotepost.Add("firstname", firstName);
                myremotepost.Add("phone", phone);
                myremotepost.Add("email", email);
                //UserRegistreModel register = new UserRegistreModel();
                TempData["model7"] = model;
                myremotepost.Add("surl", "http://people.innovative-fusion.com/Return/Return");//Change the success url here depending upon the port number of your local system.
                myremotepost.Add("furl", "http://people.innovative-fusion.com/Return/Return");//Change the failure url here depending upon the port number of your local system.
                myremotepost.Add("service_provider", "payu_paisa");
                string hashString = key + "|" + txnid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|||||||||||" + salt;
                //string hashString = "3Q5c3q|2590640|3053.00|OnlineBooking|vimallad|ladvimal@gmail.com|||||||||||mE2RxRwx";
                string hash = Generatehash512(hashString);
                myremotepost.Add("hash", hash);
                myremotepost.Post();
            }
        }
        #endregion

        #region -------- Plans Old Code  --------
        //[HttpPost]
        //public void Plans(PricingModel model)
        //{

        //    string firstName = model.FirstName;
        //    string amount = Convert.ToString(model.Amount);
        //    string productInfo = model.ProductInfo;
        //    string email = model.Email;
        //    string phone = model.Phone;
        //    //string surl = form["txtsurl"].ToString();
        //    //string furl = form["txtfurl"].ToString();

        //    RemotePost myremotepost = new RemotePost();
        //    string key = "cOyLul";
        //    string salt = "sOpcIt7y";

        //    //posting all the parameters required for integration.

        //    //myremotepost.Url = "https://test.payu.in/_payment";
        //    myremotepost.Url = "https://secure.payu.in/_payment";
        //    myremotepost.Add("key", "cOyLul");
        //    string txnid = Generatetxnid();
        //    myremotepost.Add("txnid", txnid);
        //    myremotepost.Add("amount", amount);
        //    myremotepost.Add("productinfo", productInfo);
        //    myremotepost.Add("firstname", firstName);
        //    myremotepost.Add("phone", phone);
        //    myremotepost.Add("email", email);
        //    myremotepost.Add("surl", "http://people.innovative-fusion.com/Return/Return");//Change the success url here depending upon the port number of your local system.
        //    myremotepost.Add("furl", "http://people.innovative-fusion.com/Return/Return");//Change the failure url here depending upon the port number of your local system.
        //    myremotepost.Add("service_provider", "payu_paisa");
        //    string hashString = key + "|" + txnid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|||||||||||" + salt;
        //    //string hashString = "3Q5c3q|2590640|3053.00|OnlineBooking|vimallad|ladvimal@gmail.com|||||||||||mE2RxRwx";
        //    string hash = Generatehash512(hashString);
        //    myremotepost.Add("hash", hash);
        //    myremotepost.Post();
        //}
        #endregion

        #region -------- RemotePost -------------
        public class RemotePost
        {
            private NameValueCollection Inputs = new NameValueCollection();

            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Write("<html><head>");
                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");
                System.Web.HttpContext.Current.Response.End();
            }
        }
      
        #endregion

        #region -------- Generate hash Value ----
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


        public string Generatetxnid()
        {
            Random rnd = new Random();
            string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            string txnid1 = strHash.ToString().Substring(0, 20);
            return txnid1;
        }

        #endregion
    }
}
