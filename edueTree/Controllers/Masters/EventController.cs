using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Masters
{
    [Authorize]
    public class EventController : BaseController
    {
        #region ---- Dbcontext  ---

        public readonly dbContainer Db = new dbContainer(); 
        #endregion

        #region ---- Event list ---

        // GET: /Event/
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            return View(Db.Events.Where(a => a.firmId == firm).ToList());
        }
        
        #endregion 

        #region ---- Details ------
        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = Db.Events.Find(id);
            if (@event == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(@event);
        } 
        #endregion 

        #region ---- Create -------

        // GET: /Event/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(EventModel newEvent)
        {
            var Firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {
                if (!Db.Events.Any(m => m.event1 == newEvent.event1 && m.firmId==Firm ))
                {
                    var lr = new Event()
                    {
                        event1 = newEvent.event1,
                        dateFrom = newEvent.dateFrom,
                        dateTo = newEvent.dateTo,
                        totalDays = newEvent.totalDays,
                        firmId = LoginUserFirmId()
                    };
                    Db.Events.Add(lr);
                    Db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("Index");
                }
                TempData["notice"] = "exist";
                return RedirectToAction("Create");
            }

            //if (ModelState.IsValid)
            //{
            //    db.Events.Add(newEvent);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            return View(newEvent);
        }
        
        #endregion 

        #region ----- Edit --------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = Db.Events.Find(id);
            EventModel model = new EventModel();

            if (@event == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            model.dateFrom = @event.dateFrom;
            model.dateTo = @event.dateTo;
            model.event1 = @event.event1;
            model.eventId = @event.eventId;
            model.totalDays = @event.totalDays;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventModel @event)
        {
            if (ModelState.IsValid)
            {
                Event data = Db.Events.Find(@event.eventId);
                data.dateFrom = @event.dateFrom;
                data.dateTo = @event.dateTo;
                data.event1 = @event.event1;
                data.totalDays = @event.totalDays;

                Db.Events.AddOrUpdate(data);
                Db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Index");
            }
            return View(@event);
        }
        
        #endregion

        #region ------ Delete -----
        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = Db.Events.Find(id);
            if (@event == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(@event);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = Db.Events.Find(id);
            Db.Events.Remove(@event);
            TempData["notice"] = "delete";
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        #endregion 

        #region ---- Add Notice ---
        public ActionResult AddNotice()
        {
            var staffid = LoginEmployeeId();
            var model = new NoticeModel {StaffId = staffid};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotice(NoticeModel newNotice)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();

                var noticeinfo = new notice()
                {
                    staffId = newNotice.StaffId,
                    Title = newNotice.Title,
                    narration = newNotice.Narration,
                    noticeDisplayUpto = newNotice.NoticeDisplayUpto,
                    systemDatetime = DateTime.UtcNow,
                    isActive = true,
                    firmId = firm
                };

                Db.notices.Add(noticeinfo);
                Db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AddNotice");
            }
            return View();
        }
        
        #endregion 

        #region -- Manage Notice --
        public ActionResult ManageNotice()
        {
            var firm = LoginUserFirmId();
            return View(Db.notices.Where(a => a.firmId == firm).ToList().OrderByDescending(m => m.noticeDisplayUpto));
        }
        
        #endregion 

        #region ---- Notice -------
        public ActionResult Notice()
        {
            var firm = LoginUserFirmId();
            return View(Db.notices.Where(a => a.firmId == firm).ToList().OrderByDescending(m => m.noticeDisplayUpto));
        } 
        #endregion 

        #region --- Edit Notice ---
        public ActionResult EditNotice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var @event = Db.notices.Find(id);
            if (@event == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }

            var model = new NoticeModel
            {
                IsActive = @event.isActive,
                StaffId = @event.staffId,
                Title = @event.Title,
                SystemDatetime = @event.systemDatetime,
                FirmId = @event.firmId,
                NoticeDisplayUpto = @event.noticeDisplayUpto,
                noticeId = @event.noticeId,
                Narration = @event.narration
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNotice(NoticeModel @event)
        {
            if (ModelState.IsValid)
            {
                var single = Db.notices.Find(@event.noticeId);
                single.narration = @event.Narration;
                single.noticeDisplayUpto = @event.NoticeDisplayUpto;
                single.Title = @event.Title;
                single.isActive = @event.IsActive;

                Db.notices.AddOrUpdate(single);
                Db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("AddNotice");
            }
            return View(@event);
        }
        
        #endregion 

        #region -- Delete Notice --

        public ActionResult DeleteNotice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            notice department = Db.notices.Find(id);
            if (department == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(department);
        }

        [HttpPost, ActionName("DeleteNotice")]
        public ActionResult DeleteNotice(int id)
        {
            try
            {
                notice department = Db.notices.Find(id);
                Db.notices.Remove(department);
                Db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("AddNotice");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("AddNotice");
            }
        }
        
        #endregion 

        #region ---- Board --------

        public ActionResult Board()
        {
            var firm = LoginUserFirmId();
            return View(Db.notices.Where(s=>s.firmId==firm).ToList().OrderByDescending(m => m.noticeDisplayUpto));
        }

        public ActionResult BoardForHr()
        {
            return View(Db.notices.ToList().OrderByDescending(m => m.noticeDisplayUpto));
        }

     

        public ActionResult EmployeeProbationPeriod()
        {
            var firm = LoginUserFirmId();
            var birtdayinfo = Db.UpComingProbation(firm).OrderBy(a => a.probation);
            var atsheetModel = birtdayinfo.Select(a => new StaffModel()
            {
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                StaffCode = a.staffCode,
                Dob = a.dob,
                Probation = a.probation,
                JoiningDate = a.joingDate,
              

            }).ToList();
            return View(atsheetModel);

        }

        public ActionResult EmployeeAnniversaryTenure()
        {
            var firm = LoginUserFirmId();
            var birtdayinfo = Db.UpComingAnnivarsoryTenvue(firm).OrderBy(a => a.joingDate);
            var atsheetModel = birtdayinfo.Select(a => new StaffModel()
            {
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                StaffCode = a.staffCode,
                Dob = a.dob,
                Probation = a.probation,
                JoiningDate = a.joingDate
            }).ToList();
            return View(atsheetModel);

        }
        
        
     #endregion 

        #region -- EmployeeEvent --
        public ActionResult EmployeeEvent()
        {
            var firm = LoginUserFirmId();
            var birtdayinfo = Db.UpComingBirthday(firm);
            var atsheetModel = birtdayinfo.Select(a => new StaffModel()
            {
                FirstName = a.firstName,
                MiddleName = a.middleName,
                LastName = a.lastName,
                StaffCode = a.staffCode,
                Dob = a.dob,

            }).ToList();
            return View(atsheetModel);

        } 
        #endregion 

        #region --- Shortcut ------
        public ActionResult Shotcuts()
        {
            return View();
        } 
        #endregion 

        #region --- Despose -------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion 
    }
}
