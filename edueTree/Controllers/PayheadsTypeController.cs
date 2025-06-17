using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class PayheadsTypeController : BaseController
    {
        #region ----- DBContext -------

        private dbContainer db = new dbContainer();

        #endregion 

        #region ------- Index ---------

        //public ActionResult Index()
        //{
        //    var payheadsList = db.TblPayheadTypes.Where(a => a.IsActive == true).ToList();

        //    return View(payheadsList);
        //}

       

        //#region Create      
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}
       
        //[HttpPost]
        //public ActionResult Create(PayheadTypeModel payhead)
        //{
        //    var firmid = LoginUserFirmId();
        //    try
        //    {              
        //        if (ModelState.IsValid)
        //        {
        //            var pay = new TblPayheadType
        //            {
        //                Name = payhead.Name,
        //                IsActive = true,
        //                CreatedOn = DateTime.Now,
        //                Firmid = firmid
        //            };
        //            db.TblPayheadTypes.Add(pay);
        //            db.SaveChanges();
        //            TempData["notice"] = "success";
        //            return RedirectToAction("Create");
        //        }
        //        TempData["notice"] = "error";
        //        return RedirectToAction("Create");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //#endregion

        //#region Edit    
        //[FilterConfig.EncryptedActionParameterAttribute]
        //public ActionResult Edit(int id)
        //{
        //    var firmid = LoginUserFirmId();
        //    var payheadModel = new PayheadTypeModel();
        //    var data = db.TblPayheadTypes.FirstOrDefault(a => a.Id == id);

        //      if (data == null) return View(payheadModel);
        //      payheadModel.Id = data.Id;
        //      payheadModel.Name = data.Name;
        //      payheadModel.IsActive = true;
        //      payheadModel.Firmid = firmid;
        //      payheadModel.CreatedOn = data.CreatedOn;

        //      return View(payheadModel);
        //}
     
        //[HttpPost]
        //public ActionResult Edit(PayheadTypeModel payheadModel)
        //{
        //    try
        //    {           
        //        if (!ModelState.IsValid) return RedirectToAction("Index");

        //        var data = db.TblPayheadTypes.FirstOrDefault(a => a.Id == payheadModel.Id);
        //        if (data != null)
        //        {
        //            data.Name = payheadModel.Name;
        //            data.IsActive = true;
        //            data.Firmid = LoginUserFirmId();
        //            data.CreatedOn = DateTime.Now;
        //            db.TblPayheadTypes.AddOrUpdate(data);

        //            db.SaveChanges();
        //            TempData["notice"] = "success";
        //            return RedirectToAction("Create");
        //        }
        //        TempData["notice"] = "error";
        //        return RedirectToAction("Create");
        //    }
        //    catch
        //    {
        //        TempData["notice"] = "error";
        //        return RedirectToAction("Create");
        //    }
        //}

        //#endregion

        //public ActionResult DeletePayheadType(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TblPayheadType bts = db.TblPayheadTypes.Find(id);
        //    if (bts == null)
        //    {
        //        return RedirectToAction("pagenotfound", "Home");
        //    }
        //    return View(bts);
        //}

        //[HttpPost]
        //public ActionResult DeletePayheadType(int id)
        //{
        //    try
        //    {
        //        TblPayheadType bts = db.TblPayheadTypes.Find(id);
        //        db.TblPayheadTypes.Remove(bts);
        //        db.SaveChanges();
        //        TempData["notice"] = "delete";
        //        return RedirectToAction("Create");
        //    }
        //    catch
        //    {
        //        TempData["notice"] = "cantdelete";
        //        return RedirectToAction("Create");
        //    }
        //} 

        #endregion
    }
}
