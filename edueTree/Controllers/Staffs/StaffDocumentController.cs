using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using edueTree.Models;
using Ionic.Zip;

namespace edueTree.Controllers.Staffs
{
    [Authorize]
    public class StaffDocumentController : BaseController
    {
        #region --------- DbContext -------------
        private dbContainer db = new dbContainer();
        #endregion
        
        #region ------------ Index --------------

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
            var staffdocuments = db.StaffDocuments.Include(s => s.Staff).Where(a => a.staffId == staffId);
            return View(staffdocuments.ToList());
        }

        #endregion  
        
        #region ------- StaffDucumentDetails ----
        public ActionResult StaffDucumentDetails()
        {
            int staffId = LoginEmployeeId();
            ViewData["staffId"] = staffId;
            var staffdocuments = db.StaffDocuments.Include(s => s.Staff).Where(a => a.staffId == staffId);
            return View(staffdocuments.ToList());
        }
        #endregion 

        #region --------- Details  --------------
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffDocument staffdocument = db.StaffDocuments.Find(id);
            if (staffdocument == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffdocument);
        }

        #endregion 

        #region ------------ Create -------------
            
        public ActionResult Create(int? stafId)
        {
            var stadocModel = new StaffDocumentModel
            {
                StaffName = SettingStaffName(stafId),
                StaffId = Convert.ToInt32(stafId)
            };
            return View(stadocModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffDocumentModel staffdocmodel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var staffdoc = db.Staffs.FirstOrDefault(s => s.staffId == staffdocmodel.StaffId).schoolCode;
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        var fileName = Path.GetFileName(file.FileName);
                        var rondom = staffdoc + Guid.NewGuid() + fileName;
                        var path = Path.Combine(Server.MapPath("~/StaffDocument/"), rondom);
                        file.SaveAs(path);
                        staffdocmodel.AttachedDoc = rondom;
                        ViewBag.Path = String.Format("~/StaffDocument/", fileName);
                    }


                    var stafDoc = new StaffDocument()
                    {
                        staffId = staffdocmodel.StaffId,
                        title = staffdocmodel.Title,
                        attachedDoc = staffdocmodel.AttachedDoc,
                        firmId = firm
                    };

                    db.StaffDocuments.Add(stafDoc);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffdocmodel.StaffId) });
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return View(staffdocmodel);
        }

        #endregion 

        #region ------ AllStaffDocument ---------

        public ActionResult AllStaffDocument()
        {
            var firm = LoginUserFirmId();
            return View(db.StaffDocuments.Where(s => s.firmId == firm).Include(s=>s.Staff).Where(s=>s.Staff.isActive==true).ToList());           
        }
        #endregion

        #region ------ DowmloadStaffDoc ---------
        public ActionResult DowmloadStaffDoc(string StaffName)
        {            
            string[] words = StaffName.Split(':');
            string stname = words[0];

            var stid = db.Staffs.FirstOrDefault(s => s.schoolCode == stname).staffId;
            var docment = db.StaffDocuments.Where(d => d.staffId == stid);

            int cnt = 1;
            var outputStream = new MemoryStream();
            var zip = new ZipFile();
            foreach (var ex in docment)
            {               
                   // zip.AddEntry(ex.attachedDoc, "content1"+cnt);
                    zip.AddFile(Server.MapPath("/StaffDocument/" + ex.attachedDoc));                   
                    cnt ++;
            }
            zip.Save(outputStream);
            outputStream.Position = 0;            
            return File(outputStream, "application/zip", stname+".zip");         
        }
#endregion

        #region ------------ Edit ---------------
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffDocument staffdocument = db.StaffDocuments.Find(id);
            if (staffdocument == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", staffdocument.staffId);
            return View(staffdocument);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "docId,staffId,title,attachedDoc")] StaffDocument staffdocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffdocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", staffdocument.staffId);
            return View(staffdocument);
        }

        #endregion 

        #region ------------ Delete -------------
       
        public ActionResult Delete(int? id, int? staffid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StaffDocument staffdocument = db.StaffDocuments.Find(id);
            if (staffdocument == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffdocument);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, int? staffid)
        {
            StaffDocument staffdocument = db.StaffDocuments.Find(id);
            db.StaffDocuments.Remove(staffdocument);
            db.SaveChanges();
            TempData["notice"] = "delete";
            return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffid) });
        }

        #endregion

        #region --------------- Dispose ---------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region ------------ Encrypt ------------

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
