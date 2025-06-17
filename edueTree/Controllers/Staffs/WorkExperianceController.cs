using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Staffs
{
    [Authorize]
    public class WorkExperianceController : BaseController
    {
        #region ----------- DbContext ------
        private dbContainer db = new dbContainer();
        #endregion 

        #region ---------- Index -----------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Index(int? staffId)
        {
            var loginuser = LoginEmployeeId();
            if (loginuser == staffId)
            {
                var details = db.EditPermissions.FirstOrDefault(a => a.StaffId == staffId);
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
            var workexperiances = db.WorkExperiances.Include(w => w.Staff).Where(a => a.staffId == staffId);
            return View(workexperiances.ToList());
        }

        #endregion 

        #region ------- WorkExpeDetail -----
        public ActionResult StaffWorkExperienceDetails()
        {
            int staffId = LoginEmployeeId();
            ViewData["staffId"] = staffId;
            var workexperiances = db.WorkExperiances.Include(w => w.Staff).Where(a => a.staffId == staffId);
            return View(workexperiances.ToList());
        }
        #endregion

        #region --------- Detail -----------
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkExperiance workexperiance = await db.WorkExperiances.FindAsync(id);
            if (workexperiance == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(workexperiance);
        }
        #endregion 

        #region ---------- Create ----------

        public ActionResult Create(int? staffId)
        {
            var wkex = new WorkExperiance { staffId = staffId };
            return View(wkex);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "previousId,organizationName,JoiningDate,releaseDate,designation,attachedCertificate,staffId,CreatedDate")] WorkExperiance workexperiance)
        {
            if (ModelState.IsValid)
            {
                db.WorkExperiances.Add(workexperiance);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + workexperiance.staffId) });

            }
            return View(workexperiance);
        }

        #endregion 

        #region --------- Edit -------------
        [FilterConfig.EncryptedActionParameterAttribute]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkExperiance workexperiance = await db.WorkExperiances.FindAsync(id);
            if (workexperiance == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(workexperiance);
        }

        // POST: /WorkExperiance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "previousId,organizationName,JoiningDate,releaseDate,designation,attachedCertificate,staffId,CreatedDate")] WorkExperiance workexperiance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workexperiance).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + workexperiance.staffId) });

                //return RedirectToAction("StaffDashboard", "Staffs", new { staffId = workexperiance.staffId });
            }
            return View(workexperiance);
        }

        #endregion

        #region ------- Delete -------------
        // GET: /WorkExperiance/Delete/5
        public async Task<ActionResult> Delete(int? id, int? staffid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkExperiance workexperiance = await db.WorkExperiances.FindAsync(id);
            if (workexperiance == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(workexperiance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, int? staffid)
        {
            WorkExperiance workexperiance = db.WorkExperiances.Find(id);
            db.WorkExperiances.Remove(workexperiance);
            db.SaveChanges();
            return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffid) });
        }

        #endregion

        #region ---------- Despose ---------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion 

        #region ---------- Encrypt ---------
        public string Encrypt(string plainText)
        {
            //string key = "jdsg432387#";
            string key = "chan221988#";
            byte[] EncryptKey = { };
            //byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] IV = { 27, 98, 45, 23, 65, 173, 17, 88 };
            EncryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        } 
        #endregion 
    }
}
