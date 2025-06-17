using edueTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace edueTree.Controllers.PayRoll
{
    public class ComputedValueController : BaseController
    {

        #region ----- DbContext -----
        dbContainer db = new dbContainer();
        #endregion

        #region ----- Index ---------
        public ActionResult Index()
        {
            var data = db.VariableSettings.ToList().Where(a => a.isActive == true);

            return View();
        } 
        #endregion

        #region ------ Details ------
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion

        #region ------- Create ------
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

        #region ------- Edit --------
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        #region ------- Delete ------
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
    }
}
