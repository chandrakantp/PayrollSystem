using System.Web.Mvc;
using edueTree.Models;
using Microsoft.AspNet.Identity;

namespace edueTree.Controllers.Staffs
{
    public class GratuityController : Controller
    {
        #region ----------- Dbcontext -----------
        private dbContainer db = new dbContainer();
        #endregion
        
        #region --------- UserManager -----------
        public UserManager<ApplicationUser> UserManager { get; set; }
        #endregion

        #region --------- Gratuity List ---------
        //
        // GET: /Gratuity/
        /// <summary>
        /// Gratuity List
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
             //var gratuity= db.g
            return View();
        }

        #endregion 

        #region -------- Gratuity Details -------
        //
        // GET: /Gratuity/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 

        #region ---------- Gratuity Create ------
        //
        // GET: /Gratuity/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gratuity/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion 

        #region ---------- Gratuity Edit --------
        //
        // GET: /Gratuity/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Gratuity/Edit/5
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

        #region --------- Gratuity Delete -------
        //
        // GET: /Gratuity/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Gratuity/Delete/5
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
