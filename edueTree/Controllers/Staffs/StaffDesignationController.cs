using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Staffs
{
    [Authorize]
    public class StaffDesignationController : BaseController
    {
        #region -------- Dbcontext ------

        public readonly dbContainer Db = new dbContainer();
        #endregion

        #region --------- Index ---------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Index(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = Db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
                if (details != null)
                {
                    ViewBag.message = details.GeneralInfo;
                }
                else
                {
                    ViewBag.message = false;    
                }
                
            }
            else
            {
                ViewBag.message = true;
            }
            ViewData["staffId"] = staffId;
            var model = Db.StaffDesignations.Where(a => a.staffId == staffId).Select(a => new StaffDesignModel()
            {
                StaffId = (int)a.staffId,
                DateFrom = (DateTime)a.fromDate,
                DateEnd = (DateTime)a.endDate,
                Designation = a.Designation.designation1,
                StaffCode = a.Staff.staffCode,
                DesignId = (int)a.designationId,
                transId = a.tranId,
            }).ToList();

            return View(model);
        }

        #endregion 

        #region --- StaffDesigDetails ---
        public ActionResult StaffDesignationDetails()
        {

            int staffId = LoginEmployeeId();

            ViewData["staffId"] = staffId;
            var model = Db.StaffDesignations.Where(a => a.staffId == staffId).Select(a => new StaffDesignModel()
            {
                StaffId = (int)a.staffId,
                DateFrom = (DateTime)a.fromDate,
                DateEnd = (DateTime)a.endDate,
                Designation = a.Designation.designation1,
                StaffCode = a.Staff.staffCode
            }).ToList();

            return View(model);
        }
        #endregion  

        #region -------- Details --------
        // GET: /StaffDesignation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffDesignation staffdesignation = Db.StaffDesignations.Find(id);
            if (staffdesignation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffdesignation);
        }

        #endregion 

        #region -------- Create ---------
        
        public ActionResult Create(int? staId)
        {
            var firm = LoginUserFirmId();
            ViewBag.designationId = new SelectList(Db.Designations.Where(a => a.firmId == firm && a.isActive==true).OrderBy(a=>a.designation1), "designationId", "designation1");
            var staModel = new StaffDesignModel
            {
                StaffName = SettingStaffName(staId),
                StaffId = Convert.ToInt32(staId)
            };
            return View(staModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffDesignModel staffdesignation)
        {
            if (ModelState.IsValid)
            {
                var sdegObj = new StaffDesignation
                {
                    staffId = staffdesignation.StaffId,
                    isActive = true,
                    designationId = staffdesignation.DesignId,
                    fromDate = staffdesignation.DateFrom
                };

                Db.StaffDesignations.Add(sdegObj);

                var desgnHistory = Db.StaffDesignations.Where(a => a.staffId == staffdesignation.StaffId && a.isActive == true);

                foreach (var coll in desgnHistory)
                {
                    coll.isActive = false;
                    coll.endDate = staffdesignation.DateFrom;
                    Db.StaffDesignations.AddOrUpdate(coll);
                }

                var staffdb = Db.Staffs.FirstOrDefault(a => a.staffId == staffdesignation.StaffId);
                if (staffdb != null) staffdb.designationId = staffdesignation.DesignId;

                Db.Staffs.AddOrUpdate(staffdb);
                Db.SaveChanges();
                TempData["notice"] = "success";

                return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffdesignation.StaffId) });
                //return RedirectToAction("StaffDashboard", "Staffs", new { staffId = staffdesignation.StaffId });
            }

            staffdesignation.StaffName = SettingStaffName(staffdesignation.StaffId);

            ViewBag.designationId = new SelectList(Db.Designations, "designationId", "designation1", staffdesignation.DesignId);
            ViewBag.staffId = new SelectList(Db.Staffs, "staffId", "staffCode", staffdesignation.StaffId);
            return View(staffdesignation);
        }

        #endregion 

        #region ---------- Edit ---------
        [HttpGet]
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffDesignation staffdesignation = Db.StaffDesignations.Find(id);
            if (staffdesignation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            staffdesignation.Design1List = new SelectList(Db.Designations.Where(a=>a.firmId==firm && a.isActive==true).OrderBy(a=>a.designation1), "designationId", "designation1", staffdesignation.designationId);

            return View(staffdesignation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tranId,staffId,designationId,isActive,fromDate,endDate")] StaffDesignation staffdesignation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Db.Entry(staffdesignation).State = EntityState.Modified;
                    Db.SaveChanges();

                    return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffdesignation.staffId) });
                    //return RedirectToAction("StaffDashboard", "Staffs", new { staffId = staffdesignation.staffId });
                }
                ViewBag.designationId = new SelectList(Db.Designations, "designationId", "designation1", staffdesignation.designationId);
                ViewBag.staffId = new SelectList(Db.Staffs, "staffId", "staffCode", staffdesignation.staffId);
                return View(staffdesignation);
            }
            catch (Exception)
            {

                return null;
            }
        }

        #endregion 

        #region ------ Delete Confirm ---
        // GET: /StaffDesignation/Delete/5
        public ActionResult Delete(int? id, int? staffid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffDesignation staffdesignation = Db.StaffDesignations.Find(id);
            if (staffdesignation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffdesignation);
        }

        // POST: /StaffDesignation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? staffid)
        {
            StaffDesignation staffdesignation = Db.StaffDesignations.Find(id);
            Db.StaffDesignations.Remove(staffdesignation);
            Db.SaveChanges();
            return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffid) });
        }

        #endregion 

        #region ------- Despose ---------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion 

        #region --------- Encrypt -------
        public string Encrypt(string plainText)
        {
            //string key = "jdsg432387#";
            const string key = "chan221988#";
            //byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] iv = { 27, 98, 45, 23, 65, 173, 17, 88 };
            var encryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, des.CreateEncryptor(encryptKey, iv), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        #endregion 
    }
}
