using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using edueTree.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edueTree.Controllers
{
    [Authorize]
    public class TrainingController : BaseController
    {

        #region ------------- Db-context --------------

        private dbContainer db = new dbContainer();

        #endregion

        #region ----- Training Controller Con. --------

        public TrainingController()

            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public TrainingController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;
        }

        #endregion

        #region ------------ UserManager --------------

        public UserManager<ApplicationUser> UserManager { get; private set; }

        #endregion

        #region --------------- Index -----------------
        
        public ActionResult Index(DateTime? datepicker, DateTime? datepicker2, int? deptid)
        {

            var firmid = LoginUserFirmId();
            ViewBag.deptid =
              db.Departments.ToList().Where(s => s.firmId == firmid).Select(a => new SelectListItem()
              {
                  Text = a.deptName,
                  Value = a.deptId.ToString()
              });

            var id = GetUserId();
            var rroles = db.UserRoles.Where(q => q.userId == id).FirstOrDefault().RoleId;

            var asprole = db.AspNetRoles.FirstOrDefault(a => a.Id == rroles);
            var loginuser = LoginEmployeeId();
            string[] words = asprole.Name.Split(' ');
            if (words[0] == "Admin" || words[0] == "HR")
            {
                if (datepicker == null && datepicker2 == null)
                {
                    var date = DateTime.UtcNow;
                    var start = new DateTime(date.Year, date.Month, 1);
                    var end = start.AddMonths(1).AddDays(-1);
                    var firm1 = LoginUserFirmId();
                    // var training = db.Trainings.Where(a => a.Isdeleted == false && a.FirmId == firm).ToList();
                    var training1 = db.GetAllTraining(firm1);
                    var trainingModel1 =
                        training1.Where(a => a.TrainingDateStart >= start && a.TrainingDateStart <= end)
                            .Select(a => new TrainingModel()
                            {
                                DepartmentName = a.deptName,
                                Topic = a.Topic,
                                TrainigId = a.TrainigId,
                                TrainerName = a.TrainerName,
                                TrainerDept = a.TrainerDept,
                                EgibilityCriteria = a.EgibilityCriteria,
                                Duration = a.Duration,
                                Category = a.Category,
                                TrainingDateStart = a.TrainingDateStart,
                                TrainingEndDate = a.TrainingEndDate,
                                NoofAttendant = a.NoofAttendant,
                                TrainingType = a.TrainingType,
                                TrainingRepetition = a.TrainingRepetition,
                                Narration = a.Narration,
                                TrainingContent = a.TrainingContent,
                                TrainingLocation = a.TrainingLocation,
                                Isdeleted = a.Isdeleted,
                                Createddate = a.Createddate,
                                FirmId = a.FirmId,
                                StaffId = a.StaffId,
                                TrainingEndTime = a.TrainingEndTime,

                                //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                            }).ToList();
                    return View(trainingModel1);

                }

                var firm = LoginUserFirmId();
                // var training = db.Trainings.Where(a => a.Isdeleted == false && a.FirmId == firm).ToList();
                var training = db.GetAllTraining(firm);
                if (deptid == 0)
                {
                    var trainingModel2 =
                        training.Where(a => a.TrainingDateStart >= datepicker && a.TrainingDateStart <= datepicker2)
                            .Select(a => new TrainingModel()
                            {

                                DepartmentName = a.deptName,
                                Topic = a.Topic,
                                TrainigId = a.TrainigId,
                                TrainerName = a.TrainerName,
                                TrainerDept = a.TrainerDept,
                                EgibilityCriteria = a.EgibilityCriteria,
                                Duration = a.Duration,
                                Category = a.Category,
                                TrainingDateStart = a.TrainingDateStart,
                                TrainingEndDate = a.TrainingEndDate,
                                NoofAttendant = a.NoofAttendant,
                                TrainingType = a.TrainingType,
                                TrainingRepetition = a.TrainingRepetition,
                                Narration = a.Narration,
                                TrainingContent = a.TrainingContent,
                                TrainingLocation = a.TrainingLocation,
                                Isdeleted = a.Isdeleted,
                                Createddate = a.Createddate,
                                FirmId = a.FirmId,
                                StaffId = a.StaffId,
                                TrainingEndTime = a.TrainingEndTime,

                                //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                            }).ToList();
                    return View(trainingModel2);
                }
                var trainingModel =
                    training.Where(
                        a =>
                            a.TrainingDateStart >= datepicker && a.TrainingDateStart <= datepicker2 &&
                            a.deptId == deptid).Select(a => new TrainingModel()
                            {

                                DepartmentName = a.deptName,
                                Topic = a.Topic,
                                TrainigId = a.TrainigId,
                                TrainerName = a.TrainerName,
                                TrainerDept = a.TrainerDept,
                                EgibilityCriteria = a.EgibilityCriteria,
                                Duration = a.Duration,
                                Category = a.Category,
                                TrainingDateStart = a.TrainingDateStart,
                                TrainingEndDate = a.TrainingEndDate,
                                NoofAttendant = a.NoofAttendant,
                                TrainingType = a.TrainingType,
                                TrainingRepetition = a.TrainingRepetition,
                                Narration = a.Narration,
                                TrainingContent = a.TrainingContent,
                                TrainingLocation = a.TrainingLocation,
                                Isdeleted = a.Isdeleted,
                                Createddate = a.Createddate,
                                FirmId = a.FirmId,
                                StaffId = a.StaffId,
                                TrainingEndTime = a.TrainingEndTime,

                                //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                            }).ToList();
                return View(trainingModel);
            }
            else
            {

                if (datepicker == null && datepicker2 == null)
                {

                    var date = DateTime.UtcNow;

                    var start = new DateTime(date.Year, date.Month, 1);
                    var end = start.AddMonths(1).AddDays(-1);
                    var firm1 = LoginUserFirmId();
                    // var training = db.Trainings.Where(a => a.Isdeleted == false && a.FirmId == firm).ToList();
                    var training1 = db.GetAllTraining(firm1);
                    var trainingModel1 =
                        training1.Where(
                            a => a.TrainingDateStart >= start && a.TrainingDateStart <= end && a.StaffId == loginuser)
                            .Select(a => new TrainingModel()
                            {
                                DepartmentName = a.deptName,
                                Topic = a.Topic,
                                TrainigId = a.TrainigId,
                                TrainerName = a.TrainerName,
                                TrainerDept = a.TrainerDept,
                                EgibilityCriteria = a.EgibilityCriteria,
                                Duration = a.Duration,
                                Category = a.Category,
                                TrainingDateStart = a.TrainingDateStart,
                                TrainingEndDate = a.TrainingEndDate,
                                NoofAttendant = a.NoofAttendant,
                                TrainingType = a.TrainingType,
                                TrainingRepetition = a.TrainingRepetition,
                                Narration = a.Narration,
                                TrainingContent = a.TrainingContent,
                                TrainingLocation = a.TrainingLocation,
                                Isdeleted = a.Isdeleted,
                                Createddate = a.Createddate,
                                FirmId = a.FirmId,
                                StaffId = a.StaffId,
                                TrainingEndTime = a.TrainingEndTime,

                                //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                            }).ToList();
                    return View(trainingModel1);

                }

                var firm = LoginUserFirmId();
                // var training = db.Trainings.Where(a => a.Isdeleted == false && a.FirmId == firm).ToList();
                var training = db.GetAllTraining(firm);

                if (deptid == 0)
                {
                    var trainingModel2 =
                        training.Where(
                            a =>
                                a.TrainingDateStart >= datepicker && a.TrainingDateStart <= datepicker2 &&
                                a.StaffId == loginuser)
                            .Select(a => new TrainingModel()
                            {

                                DepartmentName = a.deptName,
                                Topic = a.Topic,
                                TrainigId = a.TrainigId,
                                TrainerName = a.TrainerName,
                                TrainerDept = a.TrainerDept,
                                EgibilityCriteria = a.EgibilityCriteria,
                                Duration = a.Duration,
                                Category = a.Category,
                                TrainingDateStart = a.TrainingDateStart,
                                TrainingEndDate = a.TrainingEndDate,
                                NoofAttendant = a.NoofAttendant,
                                TrainingType = a.TrainingType,
                                TrainingRepetition = a.TrainingRepetition,
                                Narration = a.Narration,
                                TrainingContent = a.TrainingContent,
                                TrainingLocation = a.TrainingLocation,
                                Isdeleted = a.Isdeleted,
                                Createddate = a.Createddate,
                                FirmId = a.FirmId,
                                StaffId = a.StaffId,
                                TrainingEndTime = a.TrainingEndTime,

                                //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                            }).ToList();
                    return View(trainingModel2);
                }
                var trainingModel =
                    training.Where(
                        a =>
                            a.TrainingDateStart >= datepicker && a.TrainingDateStart <= datepicker2 &&
                            a.deptId == deptid && a.StaffId == loginuser).Select(a => new TrainingModel()
                            {

                                DepartmentName = a.deptName,
                                Topic = a.Topic,
                                TrainigId = a.TrainigId,
                                TrainerName = a.TrainerName,
                                TrainerDept = a.TrainerDept,
                                EgibilityCriteria = a.EgibilityCriteria,
                                Duration = a.Duration,
                                Category = a.Category,
                                TrainingDateStart = a.TrainingDateStart,
                                TrainingEndDate = a.TrainingEndDate,
                                NoofAttendant = a.NoofAttendant,
                                TrainingType = a.TrainingType,
                                TrainingRepetition = a.TrainingRepetition,
                                Narration = a.Narration,
                                TrainingContent = a.TrainingContent,
                                TrainingLocation = a.TrainingLocation,
                                Isdeleted = a.Isdeleted,
                                Createddate = a.Createddate,
                                FirmId = a.FirmId,
                                StaffId = a.StaffId,
                                TrainingEndTime = a.TrainingEndTime,

                                //EmployeeName = a.employeeCode + ": " + a.EmployeeName,
                            }).ToList();
                return View(trainingModel);
            }
         
        }

        #endregion 

        #region ---------------- Details --------------
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion 

        #region -------------- Create -----------------
        public ActionResult Create()
        {
            var firm = LoginUserFirmId();
            var trainingModel = new TrainingModel
            {
                DeptListItems = db.Departments.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
                {
                    Text = a.deptName,
                    Value = a.deptId.ToString()
                })
            };

            return View(trainingModel);
        }        
        [HttpPost]
        public ActionResult Create(TrainingModel trainingModel)
        {

            var staffid = LoginEmployeeId();
            var firm = LoginUserFirmId();
            try
            {

                if (ModelState.IsValid)
                {
                    var training = new Training
                    {
                        Category = trainingModel.Category,
                        Createddate = DateTime.Now,
                        Duration = trainingModel.Duration,
                        EgibilityCriteria = trainingModel.EgibilityCriteria,
                        FirmId = firm,
                        Isdeleted = false,
                        Narration = trainingModel.Narration,
                        NoofAttendant = trainingModel.NoofAttendant,
                        StaffId = staffid,
                        Topic = trainingModel.Topic,
                        TrainerDept = trainingModel.TrainerDept,
                        TrainerName = trainingModel.TrainerName,
                        TrainingContent = trainingModel.TrainingContent,
                        TrainingDateStart = trainingModel.TrainingDateStart,
                        TrainingEndDate = trainingModel.TrainingEndDate,
                        TrainingEndTime = trainingModel.TrainingEndTime,
                        TrainingLocation = trainingModel.TrainingLocation,
                        TrainingRepetition = trainingModel.TrainingRepetition,
                        TrainingStartTime = trainingModel.TrainingStartTime,
                        TrainingType = trainingModel.TrainingType


                    };
                    db.Trainings.Add(training);
                    db.SaveChanges();
                    TempData["notice"] = "success";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "error";
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View();
            }
        }

        #endregion 

        #region -------------- Edit -------------------
        
        public ActionResult Edit(int id)
        {
            var firm = LoginUserFirmId();
            TrainingModel trainingModel = new TrainingModel();
            var data = db.Trainings.FirstOrDefault(a => a.TrainigId == id);
            if (data != null)
            {
                trainingModel = new TrainingModel
                {
                    Category = data.Category,
                    Createddate = data.Createddate,
                    Duration = data.Duration,
                    EgibilityCriteria = data.EgibilityCriteria,
                    Narration = data.Narration,
                    NoofAttendant = data.NoofAttendant,
                    Topic = data.Topic,
                    TrainerDept = data.TrainerDept,
                    TrainerName = data.TrainerName,
                    TrainigId = data.TrainigId,
                    StaffId = data.StaffId,
                    TrainingContent = data.TrainingContent,
                    TrainingDateStart = data.TrainingDateStart,
                    TrainingEndDate = data.TrainingEndDate,
                    TrainingEndTime = data.TrainingEndTime,
                    TrainingLocation = data.TrainingLocation,
                    TrainingRepetition = data.TrainingRepetition,
                    TrainingStartTime = data.TrainingStartTime,
                    TrainingType = data.TrainingType,
                    Isdeleted = data.Isdeleted,
                    DeptListItems =
                        db.Departments.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
                        {
                            Text = a.deptName,
                            Value = a.deptId.ToString()
                        })
                };

            }
            return View(trainingModel);
        }
        
        [HttpPost]
        public ActionResult Editpost(TrainingModel trainingModel)
        {
            try
            {
                var data = db.Trainings.FirstOrDefault(a => a.TrainigId == trainingModel.TrainigId);
                if (data != null)
                {
                    data.Category = trainingModel.Category;
                    data.Duration = trainingModel.Duration;
                    data.EgibilityCriteria = trainingModel.EgibilityCriteria;
                    data.Narration = trainingModel.Narration;
                    data.NoofAttendant = trainingModel.NoofAttendant;
                    data.StaffId = trainingModel.StaffId;
                    data.Topic = trainingModel.Topic;
                    data.TrainerDept = trainingModel.TrainerDept;
                    data.TrainerName = trainingModel.TrainerName;
                    data.TrainingContent = trainingModel.TrainingContent;
                    data.TrainingDateStart = trainingModel.TrainingDateStart;
                    data.TrainingEndDate = trainingModel.TrainingEndDate;
                    data.TrainingEndTime = trainingModel.TrainingEndTime;
                    data.TrainingLocation = trainingModel.TrainingLocation;
                    data.TrainingRepetition = trainingModel.TrainingRepetition;
                    data.TrainingStartTime = trainingModel.TrainingStartTime;
                    data.TrainingEndTime = trainingModel.TrainingEndTime;
                    data.TrainingType = trainingModel.TrainingType;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion 

        #region --------------- Delete ----------------
        public ActionResult Delete(int id)
        {

            var training = db.Trainings.FirstOrDefault(a => a.TrainigId == id);



            return View(training);

        }
        
        [HttpPost]
        public ActionResult DeleteConfirm(Training training)
        {
            try
            {
                var data = db.Trainings.FirstOrDefault(a => a.TrainigId == training.TrainigId);
                if (data != null)
                {
                    data.Isdeleted = true;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "delete";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["notice"] = "error";
                return View("Index");
            }
        }

        #endregion 

        #region --------------- Invitation ------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Invitation(int id)
        {
            var firm = LoginUserFirmId();

            var data = db.Trainings.FirstOrDefault(a => a.TrainigId == id);
            var deptname = db.Departments.FirstOrDefault(a => a.deptId == data.TrainerDept);
            var trainingFeedback = new TrainingFeedbackModel();
            if (data != null)
            {
                if (deptname != null)
                    trainingFeedback = new TrainingFeedbackModel
                    {
                        TrainingId = data.TrainigId,
                        Topic = data.Topic,
                        TrainerName = data.TrainerName,
                        Deptname = deptname.deptName,
                        Duration = data.Duration,
                        StaffList =
                            db.Staffs.ToList()
                                .Where(s => s.firmId == firm && s.isActive == true)
                                .Select(a => new SelectListItem()
                                {
                                    Text = a.staffCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                                    Value = a.staffId.ToString()
                                })
                    };
            }

            return View(trainingFeedback);

        }

        public ActionResult Invitationpost(TrainingFeedbackModel trainingFeedbackModel)
        {
            var firm = LoginUserFirmId();
            if (ModelState.IsValid)
            {

                foreach (var staffid in trainingFeedbackModel.StaffId)
                {
                    var newstafffid = Convert.ToInt32(staffid);
                    var data =
                        db.TrainingFeedbacks.FirstOrDefault(
                            a => a.StaffId == newstafffid && a.TrainingId == trainingFeedbackModel.TrainingId);
                    if (data == null)
                    {
                        var training = new TrainingFeedback
                        {
                            StaffId = Convert.ToInt32(staffid),
                            TrainingId = trainingFeedbackModel.TrainingId,
                            FirmId = firm,
                            Status = "Invited"
                        };
                        db.TrainingFeedbacks.Add(training);
                        db.SaveChanges();
                    }

                }

                TempData["notice"] = "success";
                return RedirectToAction("Invitation", "Training", new {id = trainingFeedbackModel.TrainingId});
            }
            else
            {
                TempData["notice"] = "error";
                return RedirectToAction("Invitation", "Training", new {id = trainingFeedbackModel.TrainingId});
            }


        }

        #endregion 

        #region ------------- InvitationIndex ---------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult InvitationIndex(int trainingId)
        {
            var firm = LoginUserFirmId();

            var data = (from a in db.Trainings
                join b in db.TrainingFeedbacks on a.TrainigId equals b.TrainingId
                join c in db.Staffs on b.StaffId   equals c.staffId
                where b.FirmId == firm && b.TrainingId == trainingId && c.isActive == true
                select
                    new
                    {
                        a.TrainerName,
                        a.Topic,
                        a.Duration,
                        b.Id,
                        b.StaffId,
                        b.TrainingId,
                        b.Status,
                        c.firstName,
                        c.middleName,
                        c.lastName
                    }).ToList();
            var traingfeedback =
                data.Select(
                    a =>
                        new TrainingFeedbackModel()
                        {
                            TrainerName = a.TrainerName,
                            Duration = a.Duration,
                            Topic = a.Topic,
                            Status = a.Status,
                            StaffNamea = a.firstName + " " + a.middleName + " " + a.lastName,
                            TrainingId = a.TrainingId,
                            Id = a.Id
                        }).ToList();
            return View(traingfeedback.ToList());

        }

        #endregion 

        #region ------- InvitationListEmployee --------

        public ActionResult InvitationListEmployee()
        {
            var staffid = LoginEmployeeId();
            var firm = LoginUserFirmId();

            var data = (from a in db.Trainings
                join b in db.TrainingFeedbacks on a.TrainigId equals b.TrainingId
                join c in db.Staffs on b.StaffId equals c.staffId
                where b.FirmId == firm && b.StaffId == staffid && a.Isdeleted == false
                select
                    new
                    {
                        a.TrainerName,
                        a.TrainingDateStart,
                        a.TrainingEndDate,
                        a.Topic,
                        a.Duration,
                        b.Id,
                        b.StaffId,
                        b.Status,
                        b.Feedback,
                        c.firstName,
                        c.middleName,
                        c.lastName
                    }).ToList();
            var traingfeedback =
                data.Select(
                    a =>
                        new TrainingFeedbackModel()
                        {
                            TrainerName = a.TrainerName,
                            Duration = a.Duration,
                            Topic = a.Topic,
                            Feedback = a.Feedback,
                            TrainingDateStart = a.TrainingDateStart,
                            TrainingEndDate = a.TrainingEndDate,
                            Status = a.Status,
                            StaffNamea = a.firstName + " " + a.middleName + " " + a.lastName,
                            Id = a.Id
                        }).ToList();
            return View(traingfeedback.ToList());
        }

        #endregion 

        #region ------------ Delete Invitation --------

        public ActionResult DeleteInvitation(int id)
        {
            TrainingFeedbackModel trainingFeedbackModel = new TrainingFeedbackModel();
            var traingFeedback = db.TrainingFeedbacks.FirstOrDefault(a => a.TrainingId == id);
            if (traingFeedback != null)
            {
                trainingFeedbackModel = new TrainingFeedbackModel
                {
                    TrainingId = traingFeedback.TrainingId,
                    Id = traingFeedback.Id,

                };


            }
            return View(trainingFeedbackModel);
        }

        public ActionResult DeleteInvitationpost(TrainingFeedbackModel trainingFeedback)
        {
            var trainingfeedbackdelete = db.TrainingFeedbacks.Find(trainingFeedback.Id);

            TempData["notice"] = "delete";
            db.TrainingFeedbacks.Remove(trainingfeedbackdelete);
            db.SaveChanges();

            return RedirectToAction("Invitation", "Training", new {id = trainingfeedbackdelete.TrainingId});
        }

        #endregion 

        #region ----------- Edit Invitation -----------

        public ActionResult EditInvitation(int id)
        {


            var trainingModel = new TrainingFeedbackModel();
            var data = db.TrainingFeedbacks.FirstOrDefault(a => a.Id == id);
            if (data != null)
            {
                trainingModel = new TrainingFeedbackModel
                {
                    Feedback = data.Feedback,
                    Status = data.Status,
                    Id = data.Id,
                    TrainingId = data.TrainingId

                };
            }
            return View(trainingModel);
        }

        public ActionResult EditInvitationpost(TrainingFeedbackModel trainingFeedbackModel)
        {
            try
            {
                var data = db.TrainingFeedbacks.FirstOrDefault(a => a.Id == trainingFeedbackModel.Id);
                if (data != null)
                {
                    data.Status = trainingFeedbackModel.Status;
                    data.Feedback = trainingFeedbackModel.Feedback;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                    return RedirectToAction("Invitation", "Training", new {id = trainingFeedbackModel.TrainingId});
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Invitation", "Training", new {id = trainingFeedbackModel.TrainingId});
                // ignored
            }
            return RedirectToAction("Invitation", "Training", new {id = trainingFeedbackModel.TrainingId});
        }

        #endregion 

        #region ---------- Present Or Absent ----------

        [Authorize]
        public ActionResult AppOrRejectMultiple(string[] customerIDs)
        {
            if (customerIDs == null)
            {
                return Json("Please select at least one checkbox.");
            }
            var count = 0;
            foreach (
                var request in
                    customerIDs.Select(Int32.Parse)
                        .Select(invitationId => db.TrainingFeedbacks.FirstOrDefault(s => s.Id == invitationId)))
            {
                if (request != null)
                {
                    request.Status = "Absent";
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();
                }
                count++;
            }
            db.SaveChanges();
            return Json("Total :" + count + "Successfully!");
        }

        [Authorize]
        public ActionResult AppOrPresentMultiple(string[] customerIDs)
        {
            if (customerIDs == null)
            {
                return Json("Please select at least one checkbox.");
            }
            var count = 0;
            foreach (
                var request in
                    customerIDs.Select(Int32.Parse)
                        .Select(invitationId => db.TrainingFeedbacks.FirstOrDefault(s => s.Id == invitationId)))
            {
                if (request != null)
                {
                    request.Status = "Present";
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();
                }
                count++;
            }
            db.SaveChanges();
            return Json("Total :" + count + "Successfully!");
        }

        #endregion 

        #region ----------- Employee feedback ---------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Employeefeedback(int id)
        {
            TrainingFeedbackModel trainingFeedbackModel = new TrainingFeedbackModel();
            var data = (from a in db.Trainings

                join b in db.TrainingFeedbacks on a.TrainigId equals b.TrainingId
                join c in db.Departments on a.TrainerDept equals c.deptId
                where b.Id == id
                select new {a.TrainerName, a.Topic, a.TrainerDept, a.Duration, b.Status, b.Feedback, b.Id, c.deptName})
                .FirstOrDefault();
            if (data != null)
            {
                trainingFeedbackModel.Deptname = data.deptName;
                trainingFeedbackModel.TrainerDept = data.TrainerDept;
                trainingFeedbackModel.Duration = data.Duration;
                trainingFeedbackModel.Status = data.Status;
                trainingFeedbackModel.Feedback = data.Feedback;
                trainingFeedbackModel.Topic = data.Topic;
                trainingFeedbackModel.TrainerName = data.TrainerName;
                trainingFeedbackModel.Id = data.Id;

            }
            return View(trainingFeedbackModel);
        }

        public ActionResult Employeefeedbackpost(TrainingFeedbackModel trainingFeedbackModel)
        {

            var data = db.TrainingFeedbacks.FirstOrDefault(a => a.Id == trainingFeedbackModel.Id);
            if (data != null)
            {
                data.Feedback = trainingFeedbackModel.Feedback;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "success";
            }
            return RedirectToAction("InvitationListEmployee", "Training");
        }

        #endregion 

        #region ------------- Happy Sheet -------------

        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Happysheet(int id)
        {
            var firm = LoginUserFirmId();
            var data = (from a in db.Trainings
                join d in db.Departments on a.TrainerDept equals d.deptId
                join b in db.TrainingFeedbacks on a.TrainigId equals b.TrainingId
                join c in db.Staffs on b.StaffId equals c.staffId
                join f in db.FirmInfoes on c.firmId equals f.firmId
                where b.FirmId == firm && b.TrainingId == id
                select
                    new
                    {
                        a.TrainerName,
                        a.Topic,
                        d.deptName,
                        a.Duration,
                        b.Id,
                        b.StaffId,
                        b.TrainingId,
                        b.Status,
                        c.firstName,
                        c.middleName,
                        c.lastName,
                        c.staffCode,
                        b.Feedback,
                        f.logo,
                        a.Category,
                    }).ToList();
            var traingfeedback =
                data.Select(
                    a =>
                        new TrainingFeedbackModel()
                        {
                            TrainerName = a.TrainerName,
                            Duration = a.Duration,
                            Topic = a.Topic,
                            Status = a.Status,
                            Deptname = a.deptName,
                            StaffNamea = a.firstName + " " + a.middleName + " " + a.lastName,
                            TrainingId = a.TrainingId,
                            Id = a.Id,
                            Feedback = a.Feedback,
                            StaffId1 = a.staffCode,
                            FirmLogo = a.logo,
                            category=a.Category
                        }).ToList();
            return View(traingfeedback);
        }
        #endregion       

        #region ---- Training FromDate to date Count --

        public ActionResult TrainingCount(DateTime? datepicker, DateTime? datepicker2)
        {
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                datepicker = new DateTime(date.Year, date.Month, 1);
                datepicker2 = start.AddMonths(1).AddDays(-1);

            }
            var firm = LoginUserFirmId();
            var training = db.GetAllDepartmentwiseCount(datepicker, datepicker2, firm);
            var trainingModel = training.Select(a => new TrainingModel()
            {
                DepartmentName = a.deptName,

                manpower = a.Total_Manpower,

                totaltraning = a.Totaltrainng
            }).ToList();
            return View(trainingModel);
        }

        #endregion 

        #region -------- TrainingMonthlyReport --------

        public ActionResult TrainingMonthlyReport(int? month, int? year)
        {

            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            DateTime firstDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            DateTime lastDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)));
            var firmid = LoginUserFirmId();
            var data = db.GetMonthlyTrainingReport(firstDate, lastDate, firmid);
            var trainingModel = data.Select(a => new TrainingModel()
            {
                DepartmentName = a.deptName,
                trgValue = a.trgValue,
                TotalManpower = a.Total_Manpower,
                week1 = a.week1,
                week2 = a.week2,
                week3 = a.week3,
                week4 = a.week4,
                YTD = a.TYD,
                Varience = a.Varience,
                MonthOfHrs = a.MonthOfHrs,
                Monthno = a.Monthno,
                premonth = a.premonth,
                YearNo = a.YearNo,
                short_excess = a.short_excess,
            }).ToList();

            return View(trainingModel);
        }

        #endregion 

        #region ------------ GetCount -----------------

        public ActionResult GetCount(int? deptId)
        {
            var result = db.Staffs.Count(a => a.deptId == deptId && a.isActive == true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion 

        #region ---------- GetManHrTarget_Create ------

        public ActionResult GetManHrTarget()
        {
            var firm = LoginUserFirmId();
            TrainingModel trainingModel = new TrainingModel
            {
                deptList = db.Departments.Where(s => s.firmId == firm).ToList()
            };

            return View(trainingModel);
        }

        public ActionResult GetManHrTargetpost(string srnos, string deptids, string deptnames, string manhourtargets,
            string month, string year)
        {
            var srnosOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(srnos);
            var srnosEnum = srnosOj as object[] ?? srnosOj.Cast<object>().ToArray();

            var deptidOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(deptids);
            var deptidEnum = deptidOj as object[] ?? deptidOj.Cast<object>().ToArray();


            var deptnamesOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(deptnames);
            var deptnamesEnum = deptnamesOj as object[] ?? deptnamesOj.Cast<object>().ToArray();

            var manhourtargetsOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(manhourtargets);
            var manhourtargetsEnum = manhourtargetsOj as object[] ?? manhourtargetsOj.Cast<object>().ToArray();


            int alredy = 0;
            int inserted = 0;
            for (int i = 0; i < srnosEnum.Count(); i++)
            {
                var firmid = LoginUserFirmId();
                string deptname = Convert.ToString(deptnamesEnum[i]);
                int deptidnew = Convert.ToInt32(deptidEnum[i]);
                int manhourtarget = Convert.ToInt32(manhourtargetsEnum[i]);
                int monthno = Convert.ToInt32(month);
                int yearno = Convert.ToInt32(year);

                var checkAlredyExists =
                    db.MonthlyTTargets.Where(
                        a =>
                            a.firmid == firmid && a.trgMonth == monthno && a.trgYear == yearno && a.deptName == deptname &&
                            a.deptid == deptidnew);
                if (!checkAlredyExists.Any())
                {
                    var tblobj = new MonthlyTTarget
                    {

                        trgValue = Convert.ToInt32(manhourtargetsEnum[i]),
                        deptName = Convert.ToString(deptnamesEnum[i]),
                        trgMonth = Convert.ToInt32(monthno),
                        trgYear = Convert.ToInt32(yearno),
                        deptid = Convert.ToInt32(deptidnew),
                        firmid = firmid
                    };
                    db.MonthlyTTargets.Add(tblobj);
                    db.SaveChanges();
                    inserted++;
                }
                else
                {
                    alredy++;
                }
            }
            var result =
                new
                {
                    Success = "true",
                    Message =
                        "Successfully inserted rows:" + inserted.ToString() + ", Already exist rows:" +
                        alredy.ToString()
                };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion 

        #region --------- GetManHrTarget_Index --------

        public ActionResult GetManHrIndex(int? month, int? year)
        {

            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var firmid = LoginUserFirmId();
            var data = db.GetAllManHrsTarget(month, year, firmid);
            var monthlyttarget = data.Select(a => new MonthlyTTarget()
            {
                firmid = a.firmid,
                trgValue = a.trgValue,
                trgMonth = a.trgMonth,
                deptName = a.deptName,
                trgYear = a.trgYear,
                trgId = a.trgId


            }).ToList();

            return View(monthlyttarget);
        }

        #endregion 

        #region --------- GetManHr_Edit ---------------

        public ActionResult GetMontHrEdit(int id)
        {
            var monthlytargetmodel = new MonthlyTTargetModel();
            var data = db.MonthlyTTargets.FirstOrDefault(a => a.trgId == id);
            if (data != null)
            {
                monthlytargetmodel.trgId = data.trgId;
                monthlytargetmodel.trgMonth = data.trgMonth;
                monthlytargetmodel.trgValue = data.trgValue;
                monthlytargetmodel.trgYear = data.trgYear;
                monthlytargetmodel.deptName = data.deptName;
            }
            return View(monthlytargetmodel);
        }

        public ActionResult GetMontHrEditPost(MonthlyTTargetModel monthlyTTargetModel)
        {
            var data = db.MonthlyTTargets.FirstOrDefault(a => a.trgId == monthlyTTargetModel.trgId);
            if (data != null)
            {
                data.trgValue = monthlyTTargetModel.trgValue;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
            }
            else
            {
                TempData["notice"] = "error";
            }
            return RedirectToAction("GetManHrIndex");
        }


        #endregion 

        #region --------- GetManHr_Delete -------------

        public ActionResult GetManDelete(int id)
        {
            MonthlyTTarget monthlyTTarget = new MonthlyTTarget {trgId = id};
            return View(monthlyTTarget);
        }

        public ActionResult DeletemonthlytConfirm(MonthlyTTarget monthlyTTarget)
        {
            var monthlytarget = db.MonthlyTTargets.Find(monthlyTTarget.trgId);

            TempData["notice"] = "delete";
            db.MonthlyTTargets.Remove(monthlytarget);
            db.SaveChanges();
            return RedirectToAction("GetManHrIndex");
        }

        #endregion 

        #region ----------- GetAllDept ----------------

        [HttpPost]
        public ActionResult GetAllDept()
        {
            var Deptid = new List<int>();
            var Deptname = new List<string>();

            Deptname.Add("All Department");
            var firm = LoginUserFirmId();
            Deptid.Add(0);
            foreach (var variable in db.Departments.Where(a => a.firmId == firm))
            {
                Deptname.Add(variable.deptName);
                Deptid.Add(variable.deptId);
            }

            return Json(new {deptid = Deptid, deptname = Deptname});
        }

        #endregion 

        #region Store Generated Training Monthly Report


        public ActionResult StoreTrainingMonthlyReport(string departmentNames, string totalManpower, string trgValue,
            string shortExcesses, string ytDs, string week1S, string week2S, string week3S, string week4S,
            string monthOfHrs, string variences, string monthno, string yearNo)
        {
            var departmentNamesOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(departmentNames);
            var departmentNamesEnum = departmentNamesOj as object[] ?? departmentNamesOj.Cast<object>().ToArray();

            var totalManpowerOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(totalManpower);
            var totalManpowerEnum = totalManpowerOj as object[] ?? totalManpowerOj.Cast<object>().ToArray();

            var trgValueOj = (IEnumerable) new JavaScriptSerializer().DeserializeObject(trgValue);
            var trgValueEnu = trgValueOj as object[] ?? trgValueOj.Cast<object>().ToArray();

            var shortExcessesOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(shortExcesses);
            var shortExcessesEnumerable = shortExcessesOjbect as object[] ??
                                          shortExcessesOjbect.Cast<object>().ToArray();


            var ytDsOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(ytDs);
            var ytDsEnumerable = ytDsOjbect as object[] ?? ytDsOjbect.Cast<object>().ToArray();

            var week1SOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(week1S);
            var week1SEnumerable = week1SOjbect as object[] ?? week1SOjbect.Cast<object>().ToArray();

            var week2SOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(week2S);
            var week2SEnumerable = week2SOjbect as object[] ?? week2SOjbect.Cast<object>().ToArray();

            var week3SOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(week3S);
            var week3SEnumerable = week3SOjbect as object[] ?? week3SOjbect.Cast<object>().ToArray();

            var week4SOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(week4S);
            var week4SEnumerable = week4SOjbect as object[] ?? week4SOjbect.Cast<object>().ToArray();

            var monthOfHrsOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(monthOfHrs);
            var monthOfHrsEnumerable = monthOfHrsOjbect as object[] ?? monthOfHrsOjbect.Cast<object>().ToArray();

            var variencesOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(variences);
            var variencesEnumerable = variencesOjbect as object[] ?? variencesOjbect.Cast<object>().ToArray();

            var monthnoOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(monthno);
            var monthnoEnumerable = monthnoOjbect as object[] ?? monthnoOjbect.Cast<object>().ToArray();

            var yearNoOjbect = (IEnumerable) new JavaScriptSerializer().DeserializeObject(yearNo);
            var yearNoEnumerable = yearNoOjbect as object[] ?? yearNoOjbect.Cast<object>().ToArray();

            int firmId = LoginUserFirmId();

            int alredy = 0;
            int inserted = 0;
            for (int i = 0; i < departmentNamesEnum.Count(); i++)
            {

                string deptname = Convert.ToString(departmentNamesEnum[i]).Trim();
                int year = Convert.ToInt32(yearNoEnumerable[i]);
                int month = Convert.ToInt32(monthnoEnumerable[i]);
                int totalmanpower = Convert.ToInt32(totalManpowerEnum[i]);
                decimal trgvalue = Convert.ToDecimal(trgValueEnu[i]);
                decimal shortexcess = Convert.ToDecimal(shortExcessesEnumerable[i]);
                decimal monthtarget = Convert.ToDecimal(ytDsEnumerable[i]);
                decimal week1 = Convert.ToDecimal(week1SEnumerable[i]);
                decimal week2 = Convert.ToDecimal(week2SEnumerable[i]);
                decimal week3 = Convert.ToDecimal(week3SEnumerable[i]);
                decimal week4 = Convert.ToDecimal(week4SEnumerable[i]);
                decimal totalAchiveHrs = Convert.ToDecimal(monthOfHrsEnumerable[i]);
                decimal varience = Convert.ToDecimal(variencesEnumerable[i]);
                var checkAlredyExists =
                    db.VarienceReports.FirstOrDefault(
                        a => a.Daptname == deptname && a.YearNo == year && a.MonthNo == month && a.firmid == firmId);
                if (checkAlredyExists == null)
                {
                    var tblobj = new VarienceReport
                    {
                        Daptname = deptname,
                        TotalManpower = totalmanpower,
                        trgManHrs = trgvalue,
                        short_excess = shortexcess,
                        targetMonth = monthtarget,
                        week1 = week1,
                        week2 = week2,
                        week3 = week3,
                        week4 = week4,
                        totalAchiveHrs = totalAchiveHrs,
                        varience = varience,
                        MonthNo = month,
                        YearNo = year,
                        firmid = firmId
                    };

                    db.VarienceReports.Add(tblobj);

                    db.SaveChanges();
                    inserted++;
                }
                else
                {
                    checkAlredyExists.Daptname = deptname;
                    checkAlredyExists.TotalManpower = totalmanpower;
                    checkAlredyExists.trgManHrs = trgvalue;
                    checkAlredyExists.short_excess = shortexcess;
                    checkAlredyExists.targetMonth = monthtarget;
                    checkAlredyExists.week1 = week1;
                    checkAlredyExists.week2 = week2;
                    checkAlredyExists.week3 = week3;
                    checkAlredyExists.week4 = week4;
                    checkAlredyExists.totalAchiveHrs = totalAchiveHrs;
                    checkAlredyExists.varience = varience;
                    db.Entry(checkAlredyExists).State = EntityState.Modified;
                    db.SaveChanges();
                    alredy++;
                }
            }
            var result =
                new
                {
                    Success = "true",
                    Message = "Successfully inserted rows:" + inserted + ", Successfully Updated  rows:" + alredy
                };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region --------- AllTrainingByEmployee -------
        public ActionResult AllTrainingByEmployee(DateTime? datepicker, DateTime? datepicker2)
        {           
            var firm = LoginUserFirmId();
            if (datepicker != null && datepicker2 == null || datepicker == null && datepicker2 != null)
            {
                TempData["notice"] = "date";
                return RedirectToAction("AllTrainingByEmployee");
            }
            if (datepicker == null && datepicker2 == null)
            {
                var date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);

                var data = db.GetAllTrainingByEmp(firm, start, end);
                var atsheetModel = data.Select(a => new AllTrainingModel()
                {
                    fullname = a.fullname,
                    status = a.Status,
                    trainername = a.TrainerName,
                    topic = a.Topic,
                    duration = a.Duration,
                    trainingtype = a.TrainingType,
                    trainingrepition = a.TrainingRepetition,
                    trainingstarttime = a.TrainingStartTime,
                    trainingdate = a.TrainingDate,
                }).ToList();
                return View(atsheetModel);
            }
            else
            {
                var data = db.GetAllTrainingByEmp(firm, datepicker, datepicker2);
                var atsheetModel = data.Select(a => new AllTrainingModel()
                {
                    fullname = a.fullname,
                    status = a.Status,
                    trainername = a.TrainerName,
                    topic = a.Topic,
                    duration = a.Duration,
                    trainingtype = a.TrainingType,
                    trainingrepition = a.TrainingRepetition,
                    trainingstarttime = a.TrainingStartTime,
                    trainingdate = a.TrainingDate,
                }).ToList();
                return View(atsheetModel);
            }
        }
#endregion
    }
}
