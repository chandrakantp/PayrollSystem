using System.Web.Mvc;

namespace edueTree.Controllers
{
   
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SuperadminDashboard()
        {
            return View();
        }

        public ActionResult pagenotfound()
        {
          //  ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      

        public ActionResult UnderConstruction()
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

        public ActionResult Pricingcomparison()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {

            return View();
        }
    }

 
}