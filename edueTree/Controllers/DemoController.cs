using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace edueTree.Controllers
{
    public class DemoController : Controller
    {       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Demo()
        {
            return View();
        }
        [HttpPost]
        public void Demo(FormCollection form)
        {

            string firstName = form["txtfirstname"].ToString();
            string amount = form["txtamount"].ToString();
            string productInfo = form["txtprodinfo"].ToString();
            string email = form["txtemail"].ToString();
            string phone = form["txtphone"].ToString();
            //string surl = form["txtsurl"].ToString();
            //string furl = form["txtfurl"].ToString();

            RemotePost myremotepost = new RemotePost();
            string key = "uHma4WcR";
            string salt = "0RuDmBr8Of";

            //posting all the parameters required for integration.

            myremotepost.Url = "https://test.payu.in/_payment";
            myremotepost.Add("key", "uHma4WcR");
            string txnid = Generatetxnid();
            myremotepost.Add("txnid", txnid);
            myremotepost.Add("amount", amount);
            myremotepost.Add("productinfo", productInfo);
            myremotepost.Add("firstname", firstName);
            myremotepost.Add("phone", phone);
            myremotepost.Add("email", email);
            myremotepost.Add("surl", "http://people.innovative-fusion.com/Return/Return");//Change the success url here depending upon the port number of your local system.
            myremotepost.Add("furl", "http://people.innovative-fusion.com/Return/Return");//Change the failure url here depending upon the port number of your local system.
            myremotepost.Add("service_provider", "payu_paisa");
            string hashString = key + "|" + txnid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|||||||||||" + salt;            
            //string hashString = "3Q5c3q|2590640|3053.00|OnlineBooking|vimallad|ladvimal@gmail.com|||||||||||mE2RxRwx";
            string hash = Generatehash512(hashString);
            myremotepost.Add("hash", hash);
            myremotepost.Post();
        }

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

        //Hash generation Algorithm

        public string Generatehash512(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);
            new UnicodeEncoding();
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
            string txnid1 = strHash.Substring(0, 20);
            return txnid1;
        }


    }
}
