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
    public class StaffEducationController : BaseController
    {
        #region ---------- Dbcontext ------------
        private dbContainer db = new dbContainer();
        #endregion
        
        #region ----------- EduIndex ------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public async Task<ActionResult> EduIndex(int? staffId)
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
            var staffeducations = db.StaffEducations.Include(s => s.Degree).Include(s => s.DegreeSubject).Include(s => s.Staff).Where(a => a.staffId == staffId);
            return View(staffeducations.ToList());
        }

        #endregion

        #region --------- StaffEduDetails -------
        public async Task<ActionResult> StaffEduDetails()
        {
            int staffId = LoginEmployeeId();
            ViewData["staffId"] = staffId;
            var staffeducations = db.StaffEducations.Include(s => s.Degree).Include(s => s.DegreeSubject).Include(s => s.Staff).Where(a => a.staffId == staffId);
            return View(staffeducations.ToList());
        }
        #endregion

        #region --------- EduDetails ------------
        public async Task<ActionResult> EduDetails(int? id)
        {
            var data = db.EditPermissions.FirstOrDefault(a => a.StaffId == id);
            if (data != null)
            {
                ViewBag.message = data.GeneralInfo;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            if (staffeducation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffeducation);
        }
        #endregion

        #region ---------- EduCreate ------------
        public ActionResult EduCreate(int? staffId)
        {
            ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a=>a.degreeName), "degreeId", "degreeName");
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a=>a.subjectName), "degreeSubId", "subjectName");
            var edu = new StaffEducation
            {
                //DegreeList = new SelectList(db.Degrees, "degreeId", "degreeName"),
                //DegreeSubjectList = new SelectList(db.DegreeSubjects, "degreeSubId", "subjectName"),
                staffId = staffId
            };
            return View(edu);
        }
        #endregion

        #region ---------- EduCreate ------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EduCreate([Bind(Include = "eduId,degreeId,degreeSubId,addmissionYear,passingYear,persentage,staffId,university")] StaffEducation staffeducation)
        {
            if (ModelState.IsValid)
            {
                db.StaffEducations.Add(staffeducation);
                db.SaveChanges();
                //return RedirectToAction("EduIndex");
                TempData["notice"] = "success";
                return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffeducation.staffId) });
                //return RedirectToAction("StaffDashboard", "Staffs", new { staffId = staffeducation.staffId });
            }

            ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
            return View(staffeducation);
        }

        #endregion
        
        #region -------- Education edit ---------

        [FilterConfig.EncryptedActionParameterAttribute]
        public async Task<ActionResult> EduEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            if (staffeducation == null)
            {
               
                return RedirectToAction("pagenotfound", "Home");
            }
           ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
          //  staffeducation.DegreeList = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            //staffeducation.DegreeSubjectList = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
            return View(staffeducation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EduEdit([Bind(Include = "eduId,degreeId,degreeSubId,addmissionYear,passingYear,persentage,staffId,university")] StaffEducation staffeducation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffeducation).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffeducation.staffId) });
                //return RedirectToAction("StaffDashboard", "Staffs", new { staffId = staffeducation.staffId });
            }
            ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
            return View(staffeducation);
        }

        #endregion

        #region --------- SelfEduDelete ---------
        public async Task<ActionResult> SelfEduDelete(int? id, int? staffid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            if (staffeducation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffeducation);
        }
        #endregion

        #region ----- SelfEduDeleteConfirmed ----
        // POST: /StaffEducation/Delete/5
        [HttpPost, ActionName("SelfEduDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SelfEduDeleteConfirmed(int id, int? staffid)
        {
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            db.StaffEducations.Remove(staffeducation);
            await db.SaveChangesAsync();
            return RedirectToAction("StaffProfile", "Staffs", new { staffid = staffid });
        }

        #endregion

        #region ---------- SelfEduEdit ----------
        [FilterConfig.EncryptedActionParameterAttribute]
        public async Task<ActionResult> SelfEduEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            if (staffeducation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            staffeducation.DegreeList = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            staffeducation.DegreeSubjectList = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
            return View(staffeducation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelfEduEdit([Bind(Include = "eduId,degreeId,degreeSubId,addmissionYear,passingYear,persentage,staffId,university")] StaffEducation staffeducation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffeducation).State = EntityState.Modified;
                db.SaveChanges();
                // return RedirectToAction("EduIndex");
                return RedirectToAction("StaffProfile", "Staffs");
            }
            ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
            return View(staffeducation);
        }

        #endregion

        #region ----------- EduDelete -----------
        // GET: /StaffEducation/Delete/5
        public async Task<ActionResult> EduDelete(int? id, int? staffid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            if (staffeducation == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(staffeducation);
        } 
        #endregion

        #region ------- EduDeleteConfirmed ------
        // POST: /StaffEducation/Delete/5
        [HttpPost, ActionName("EduDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EduDeleteConfirmed(int id, int? staffid)
        {
            StaffEducation staffeducation = await db.StaffEducations.FindAsync(id);
            db.StaffEducations.Remove(staffeducation);
            await db.SaveChangesAsync();
            return RedirectToAction("StaffDashboard", "Staffs", new { q = Encrypt("Staffid=" + staffid) });
        }

        #endregion

        #region ----------- Despose -------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #endregion

        #region ---------- SelfEduCreate --------
        public ActionResult SelfEduCreate(int? staffId)
        {
            var staffIdnew = LoginEmployeeId();
            ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName");
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName");
            var edu = new StaffEducation
            {             
                staffId = staffIdnew
            };
            return View(edu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelfEduCreate([Bind(Include = "eduId,degreeId,degreeSubId,addmissionYear,passingYear,persentage,staffId,university")] StaffEducation staffeducation)
        {
            if (ModelState.IsValid)
            {
                db.StaffEducations.Add(staffeducation);
                db.SaveChanges();
                //return RedirectToAction("EduIndex");
                TempData["notice"] = "success";
                return RedirectToAction("StaffProfile", "Staffs", new { staffId = staffeducation.staffId });
            }

            ViewBag.degreeId = new SelectList(db.Degrees.OrderBy(a => a.degreeName), "degreeId", "degreeName", staffeducation.degreeId);
            ViewBag.degreeSubId = new SelectList(db.DegreeSubjects.OrderBy(a => a.subjectName), "degreeSubId", "subjectName", staffeducation.degreeSubId);
            return View(staffeducation);
        }

        #endregion

        #region ---------- Encrypt --------------
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
