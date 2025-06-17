using System.Linq;
using System.Web.Mvc;
using edueTree.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edueTree.Controllers.Staffs
{
    public class UndertakingController : Controller
    {
        #region -------- Dbcontext ---------
        private readonly dbContainer _db = new dbContainer();
        #endregion

        #region ----- StaffConstructor -----

        public UndertakingController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context: new ApplicationDbContext())))
        {
        }
        public UndertakingController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        #endregion

        #region ------- UserManager --------
        public UserManager<ApplicationUser> UserManager { get; private set; }
        #endregion
        
        #region --------- Index ------------
        public ActionResult Index()
        {           
            var undertakingList = _db.Undertakings.ToList();
            return View(undertakingList);
        }
    
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Details(int? staffid)
        {
             UndertakingModel undertakingModel = new UndertakingModel();
             var undertakingDetails = (from a in _db.Staffs
                 join b in _db.Undertakings on a.staffId equals b.staffid
                                       where b.staffid == staffid
                 select
                     new
                     {
                         a.staffId,
                         a.staffCode,
                         a.firstName,
                         a.middleName,
                         a.lastName,
                         a.gender,
                         a.isActive,
                         a.isMarried,
                         a.joingDate,
                         a.dob,
                         b.IsDeleted,
                         b.underid,
                         b.undertakingcheck
                     }).FirstOrDefault();
             if (undertakingDetails != null)
             {
                 
                 undertakingModel.Underid = undertakingDetails.underid;
                 undertakingModel.Name = undertakingDetails.firstName + " " + undertakingDetails.middleName;
                 undertakingModel.Undertakingcheck = (bool) undertakingDetails.undertakingcheck;
             }

             return View(undertakingModel);
        }
        #endregion

        #region ------- Create -------------
        public ActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {              
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
#endregion

        #region -------- Edit --------------
        public ActionResult Edit(int id)
        {
            return View();
        }
     
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region --------- Delete -----------
        public ActionResult Delete(int id)
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
