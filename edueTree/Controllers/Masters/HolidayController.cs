using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Masters
{
    [Authorize]
    public class HolidayController : BaseController
    {
        #region -------- Dbcontext -------
        private dbContainer db = new dbContainer();
        #endregion

        #region --------- Index ----------

        // GET: /Holiday/
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var holidays = db.Holidays;
            return View(holidays.Where(a => a.firmId == firm).ToList());
        }
        #endregion

        #region -------- Holiday ---------
        public ActionResult HolidaysCalender()
        {
            return View();
        }

        #endregion 

        #region ----- Emp-holiday-index --

        public ActionResult EmpHolidayIndex()
        {
            var holidays = db.Holidays;
            return View(holidays.ToList());
        }

        #endregion 

        #region -------- Details ---------
        // GET: /Holiday/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(holiday);
        }

        #endregion 

        #region --------- Create ---------

        public ActionResult Create()
        {
            HolidayModel holiday = new HolidayModel();
            //holiday.YearidList = new SelectList(db.Years, "yearId", "years");
            return View(holiday);

        }

        #endregion 

        #region ------- CreatePost -------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "holidayId,holidayName,date,resion,yearId")] HolidayModel holiday)
        {
            if (ModelState.IsValid)
            {
                var lr = new Holiday()
                {
                    holidayName = holiday.holidayName,
                    date = holiday.date,
                    firmId = LoginUserFirmId()
                };

                db.Holidays.Add(lr);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("Create");
            }

            //holiday.YearidList = new SelectList(db.Years, "yearId", "years", holiday.yearId);
            return View(holiday);

            // ViewBag.yearId = new SelectList(db.Years, "yearId", "years", holiday.yearId);
            // return View(holiday);
        }
        
        #endregion 
        
        #region ---------- Edit ----------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            // ViewBag.yearId = new SelectList(db.Years, "yearId", "years", holiday.yearId);
            return View(holiday);
        } 
        #endregion 

        #region --------- Weekends -------
        [HttpPost]
        public JsonResult Holidayweekend1()
        {
            var firmId = LoginUserFirmId();
            int staffid = LoginEmployeeId();
            var result = db.CalendarList(staffid, firmId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Holidayweekend()
        {
            return View();
        }

        #endregion 

        #region ---------- Edit ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "holidayId,holidayName,date,resion,yearId,firmId")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                holiday.firmId = LoginUserFirmId();
                db.Entry(holiday).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Create");
            }
            // ViewBag.yearId = new SelectList(db.Years, "yearId", "years", holiday.yearId);
            return View(holiday);
        }

        #endregion 

        #region -------- Delete ----------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(holiday);
        }

        // POST: /Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Holiday holiday = db.Holidays.Find(id);
            db.Holidays.Remove(holiday);
            db.SaveChanges();
            return RedirectToAction("create");
        }

        #endregion 

        #region -------- Despose ---------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion 

        #region -------- Weekends --------
        [HttpPost]
        public JsonResult NoticeCalendar1()
        {
            var firmId = LoginUserFirmId();           
            var result = db.NoticeCalendar(firmId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NoticeCalendar()
        {
            return View();
        }

        #endregion 

        #region ---- Event Schedule ------

        public ActionResult StaffBirthdayInYear(string status)
        {
            var firm = LoginUserFirmId();
            if (string.IsNullOrEmpty(status))
            {               
                var data = db.TotalBirthdayInYear(firm);
                var datainModel = data.Select(a => new NoticeBoardModel()
                {
                   StaffIdsp = a.staffid,
                   EmpFullnameSp = a.fullname,
                   DateSp = a.DateOfBirth,
                   StatusSp = a.statusdob
                }).ToList();
                return View(datainModel);
            }
            else
            {
                var data = db.TotalBirthdayInYear(firm);
                var datainModel = data.Where(a => a.statusdob == status).Select(a => new NoticeBoardModel()
                {
                    StaffIdsp = a.staffid,
                    EmpFullnameSp = a.fullname,
                    DateSp = a.DateOfBirth,
                    StatusSp = a.statusdob
                }).ToList();
                return View(datainModel);              
            }
        }


        public ActionResult StaffProhibitionPeriodInYear(string status)
        {
            var firm = LoginUserFirmId();
            if (string.IsNullOrEmpty(status))
            {
                var data = db.TotalProhibitionInYear(firm);
                var datainModel = data.Select(a => new NoticeBoardModel()
                {
                    StaffIdsp = a.staffid,
                    EmpFullnameSp = a.fullname,
                    DateSp = a.Prohibition,
                    StatusSp = a.statusdob
                }).ToList();
                return View(datainModel);
            }
            else
            {
                var data = db.TotalProhibitionInYear(firm);
                var datainModel = data.Where(a => a.statusdob == status).Select(a => new NoticeBoardModel()
                {
                    StaffIdsp = a.staffid,
                    EmpFullnameSp = a.fullname,
                    DateSp = a.Prohibition,
                    StatusSp = a.statusdob
                }).ToList();
                return View(datainModel);
            }
        }


        public ActionResult StaffAnniversaryTenureInYear(string status)
        {
            var firm = LoginUserFirmId();
            if (string.IsNullOrEmpty(status))
            {
                var data = db.TotalAnniversaryTenureInYear(firm);
                var datainModel = data.Select(a => new NoticeBoardModel()
                {
                    StaffIdsp = a.staffid,
                    EmpFullnameSp = a.fullname,
                    DateSp = a.joingDate,
                    StatusSp = a.statusdob
                }).ToList();
                return View(datainModel);
            }
            else
            {
                var data = db.TotalAnniversaryTenureInYear(firm);
                var datainModel = data.Where(a => a.statusdob == status).Select(a => new NoticeBoardModel()
                {
                    StaffIdsp = a.staffid,
                    EmpFullnameSp = a.fullname,
                    DateSp = a.joingDate,
                    StatusSp = a.statusdob
                }).ToList();
                return View(datainModel);
            }
        }
#endregion 
    }
}
