using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using edueTree.Models;

namespace edueTree.Controllers
{
    [Authorize]
    public class PerformanceController : BaseController
    {
        #region ---------- DbContext ------------
        private dbContainer db = new dbContainer();
        #endregion

        #region -------- Create Form ------------
        public ActionResult CreateForm()
        {
            var firm = LoginUserFirmId();
            var pmodel = new PerformModel
            {
                SectionList = new SelectList(db.TblSections.Where(s=>s.firmid==firm), "SectionId", "SectionName"),
            };
            return View(pmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForm(PerformModel pmodel)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var qus = new QuestionMaster
                {
                    QuestionName = pmodel.QuestionName,
                    SectionsId = pmodel.SectionsId,
                    weightage = pmodel.weightage,
                    CreatedDate = DateTime.Now,
                    firmid = firm
                };
                db.QuestionMasters.Add(qus);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("CreateForm");
            }
            pmodel.SectionList = new SelectList(db.TblSections, "SectionId", "SectionName");
            return View(pmodel);
        }
        #endregion

        #region -------- QuestionEdit -----------
        [HttpGet]
        public ActionResult QuestionEdit(int? id)
        {
            var firm = LoginUserFirmId();
            QuestionMaster bts = db.QuestionMasters.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            PerformModel model = new PerformModel();
            model.QuestionId = bts.QuestionId;
            model.QuestionName = bts.QuestionName;
            model.SectionsId = (int) bts.SectionsId;
            model.weightage = bts.weightage;
            model.firmid =Convert.ToString(bts.firmid);
            ViewBag.SectionsList = new SelectList(db.TblSections.Where(s=>s.firmid==firm), "SectionId", "SectionName", bts.SectionsId);
            return View(model);
        }

        public ActionResult QuestionEdit(PerformModel mmodel)
        {
            if (ModelState.IsValid)
            {
                QuestionMaster bts = db.QuestionMasters.Find(mmodel.QuestionId);
                {
                    bts.QuestionId = mmodel.QuestionId;
                    bts.SectionsId = mmodel.SectionsId;
                    bts.QuestionName = mmodel.QuestionName;
                    bts.weightage = mmodel.weightage;
                    bts.firmid = Convert.ToInt32(mmodel.firmid);
                }
                db.QuestionMasters.AddOrUpdate(bts);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("CreateForm");
            }
            ViewBag.SectionsList = new SelectList(db.TblSections, "SectionId", "SectionName", mmodel.SectionsId);
            return View(mmodel);
        }
        #endregion

        #region ------- Section Create ----------
        [HttpGet]
        public ActionResult SectionCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SectionCreate(SectionModel smodel)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var qus = new TblSection
                {
                    SectionName = smodel.SectionName,
                    CreatedDate = DateTime.Now,
                    firmid = firm
                };

                db.TblSections.Add(qus);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("SectionCreate");
            }
            return View(smodel);
        }
        #endregion

        #region ----------- Section Index -------
        public ActionResult SectionIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.TblSections.Where(s=>s.firmid==firm).ToList());
        }
        #endregion

        #region ------ Delete Section -----------
        public ActionResult DeleteSection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblSection bts = db.TblSections.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteSection(int id)
        {
            try
            {
                TblSection bts = db.TblSections.Find(id);
                db.TblSections.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("SectionCreate");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("SectionCreate");
            }
        } 
        #endregion

        #region ------- section Edit ------------
        [HttpGet]
        public ActionResult SectionEdit(int? id)
        {
            TblSection bts = db.TblSections.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            SectionModel model = new SectionModel();
            model.SectionId = bts.SectionId;
            model.SectionName = bts.SectionName;
            model.firmid = (int) bts.firmid;
            return View(model);
        }

        public ActionResult SectionEdit(SectionModel mmodel)
        {
            TblSection bts = db.TblSections.Find(mmodel.SectionId);
            {
                bts.SectionId = mmodel.SectionId;
                bts.SectionName = mmodel.SectionName;
                bts.firmid = mmodel.firmid;
            }
            db.TblSections.AddOrUpdate(bts);
            db.SaveChanges();
            TempData["notice"] = "update";
            return RedirectToAction("SectionCreate");
        }
        #endregion

        #region -------- QuestionIndex ----------
        public ActionResult QuestionIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.QuestionMasters.Include(s => s.TblSection).Where(a => a.firmid == firm).ToList());
        }
        #endregion

        #region ---- Feedback Form Methods ------
        public ActionResult FeedbackFormCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedbackFormCreate(FeedbackFormModel fmodel)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var qus = new FeedbackFormMaster
                {
                    Title = fmodel.Title,
                    Duration = Convert.ToString(fmodel.Duration).Trim(),
                    createdDate = DateTime.Now,
                    firmid = firm
                };

                db.FeedbackFormMasters.Add(qus);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("FeedbackFormCreate");
            }
            return View(fmodel);
        }

           [HttpGet]
        public ActionResult FeedFormEdit(int? id)
        {
            FeedbackFormMaster bts = db.FeedbackFormMasters.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            FeedbackFormModel model = new FeedbackFormModel();
            model.FeedbackId = bts.FeedbackId;
            model.Title = bts.Title;
            model.Duration = bts.Duration;
            model.firmid = bts.firmid;

            return View(model);
        }

        public ActionResult FeedFormEdit(FeedbackFormModel mmodel)
            {
            var firm = LoginUserFirmId();
            var dt = db.FeedbackFormMasters.FirstOrDefault(s => s.Title == mmodel.Title && s.firmid == firm && s.FeedbackId != mmodel.FeedbackId);
            if (dt == null)
            {
                FeedbackFormMaster bts = db.FeedbackFormMasters.Find(mmodel.FeedbackId);
                {
                    bts.FeedbackId = mmodel.FeedbackId;
                    bts.Title = mmodel.Title;
                    bts.Duration = mmodel.Duration;
                    bts.firmid = mmodel.firmid;
                }
                db.FeedbackFormMasters.AddOrUpdate(bts);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("FeedbackFormCreate");
            }
            TempData["notice"] = "exist";
            return RedirectToAction("FeedbackFormCreate");
        }


        public ActionResult FeedbackFormIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.FeedbackFormMasters.Where(a => a.firmid == firm).ToList());
        }
        #endregion

        #region --- QuestionAssignment Methods ---
        public ActionResult QuestionAssignment()
        {
            var firm = LoginUserFirmId();
            var pmodel = new QuestionAssigmentModel();
            pmodel.FormTitleList = new SelectList(db.FeedbackFormMasters.Where(s=>s.firmid==firm), "FeedbackId", "Title");
            pmodel.QueList = db.QuestionMasters.Where(d=>d.firmid==firm).ToList();            
            return View(pmodel);
        }

        [HttpPost]
        public ActionResult QuestionAssignmentNew(string[] customerIDs, string[] feedid)
        {
            if (customerIDs == null)
            {
                return Json("Please select at least one checkbox.");
            }
            else
            {
                string sp = string.Join(" ", feedid);
                var feedtext = db.FeedbackFormMasters.FirstOrDefault(s => s.Title == sp).FeedbackId;
                int count = 0;
                foreach (string ex in customerIDs)
                {
                    int traid = Int32.Parse(ex);
                    var firm = LoginUserFirmId();
                    var data =
                        db.QuestionAssigments.FirstOrDefault(
                            a => a.qustId == traid && a.feedbackId == feedtext && a.firmId == firm);
                    if (data == null)
                    {
                        var qus = new QuestionAssigment
                        {
                            feedbackId = feedtext,
                            qustId = traid,
                            firmId = firm
                        };

                        db.QuestionAssigments.Add(qus);
                        count++;
                        db.SaveChanges();                       
                    }
                    else
                    {
                        var result = new { Success = "false", Message = "Record Already Exist !" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                var resultnew = new { Success = "true", Message = "Question assign successfully" };
                return Json(resultnew, JsonRequestBehavior.AllowGet);
               // return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult QuestionAssignmentIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.QuestionAssigments.Include(s => s.QuestionMaster).Include(d => d.FeedbackFormMaster).Where(a => a.firmId == firm).ToList());

        }
        #endregion

        #region ------- FormAssign Methods ------
        public ActionResult FormAssignmentResult()
        {
            var firm = LoginUserFirmId();
            var data = db.TblFormAssignments.Where(s=>s.firmid==firm && s.Staff.isActive==true).Include(s => s.FeedbackFormMaster).Include(s => s.Staff);
            return View(data.ToList());
        }

        [HttpGet]
        public ActionResult FormAssignmentToEmp()
        {
            var firm = LoginUserFirmId();
            FormAssignModel fam = new FormAssignModel();

            //fam.FormNameList = db.FeedbackFormMasters.Where(a => a.firmid == firm).Select(a => new SelectListItem()
            //{
            //    Text = a.Title,
            //    Value = a.FeedbackId.ToString()
            //});

            fam.FormNameList = db.QuestionAssignFormDropdownList(firm).Select(a => new SelectListItem()
            {
                Text = a.Title,
                Value = a.FeedbackId.ToString()
            });

            fam.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });

            return View(fam);
        }

        [HttpPost]
        public ActionResult FormAssignmentToEmp(FormAssignModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var firm = LoginUserFirmId();
                    foreach (var staffid in model.Empid)
                    {
                        var staffidnew = Convert.ToInt32(staffid);
                        var data = db.TblFormAssignments.FirstOrDefault(a => a.Empid == staffidnew);
                        if (data == null)
                        {
                            var fassign = new TblFormAssignment
                            {
                                feedformId = model.feedformId,
                                Empid = staffidnew,
                                firmid = firm
                            };
                            db.TblFormAssignments.Add(fassign);
                            db.SaveChanges();
                            TempData["notice"] = "success";
                        }
                        else
                        {
                            TempData["notice"] = "exist";
                        }
                    }
                }
                return RedirectToAction("FormAssignmentToEmp");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult FormAssignEdit(int? id)
        {
            var firm = LoginUserFirmId();
            FormAssignModel model = new FormAssignModel();
            var data = db.TblFormAssignments.FirstOrDefault(a => a.FormAssignmentId == id);
            if (data != null)
            {
                model.feedformId = data.feedformId;
                model.FormAssignmentId = data.FormAssignmentId;
                model.EmployeeassignId = (int)data.Empid;
                model.firmId = (int)data.firmid;
                ViewBag.FormList = new SelectList(db.FeedbackFormMasters.Where(s => s.firmid == firm), "FeedbackId", "Title", data.feedformId);
                model.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
                //ViewBag.StaffList = new SelectList(db.Staffs.Where(s=>s.firmId==firm), "staffId", "Fullname", data.Empid);   
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult FormAssignEdit(FormAssignModel mmodel)
        {
            var data = db.TblFormAssignments.FirstOrDefault(a => a.Empid == mmodel.EmployeeassignId);
            if (data == null)
            {
                TblFormAssignment bts = db.TblFormAssignments.Find(mmodel.FormAssignmentId);
                {
                    bts.FormAssignmentId = mmodel.FormAssignmentId;
                    bts.feedformId = mmodel.feedformId;
                    bts.Empid = mmodel.EmployeeassignId;
                    bts.firmid = mmodel.firmId;
                }
                db.TblFormAssignments.AddOrUpdate(bts);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("FormAssignmentToEmp");
            }
            else
            {
                TempData["notice"] = "exist";
                return RedirectToAction("FormAssignmentToEmp");
            }

        }

        #endregion

        #region ---- EvaluatorAssign Method -----

        [HttpGet]
        public ActionResult EvaluatorAssignForm()
        {
            var firm = LoginUserFirmId();
            FormEvaluatorModel fam = new FormEvaluatorModel();
            fam.StaffListTwo = db.StaffDropdownConcate().Where(s => s.firmid == firm).ToList().Select(a => new SelectListItem()
            {
                Text = a.fullName,
                Value = a.Empid.ToString()
            });


            fam.StaffListOne = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });
            return View(fam);
        }

        [HttpPost]
        public ActionResult EvaluatorAssignForm(FormEvaluatorModel assignShiftModel)
        {
            var login = LoginEmployeeId();
            var firm = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {                    
                    foreach (var staffid in assignShiftModel.empid)
                    {
                        foreach (var evaluid in assignShiftModel.evaluatorEmpid)
                        {
                            var staffidnew = Convert.ToInt32(staffid);
                            var evaidnew = Convert.ToInt32(evaluid);

                            if (staffidnew != evaidnew)
                            {
                                var data =
                                    db.TblFormEvaluators.FirstOrDefault(
                                        a => a.empid == staffidnew && a.evaluatorEmpid == evaidnew);
                                if (data == null)
                                {
                                    var assignShift = new TblFormEvaluator
                                    {
                                        empid = staffidnew,
                                        evaluatorEmpid = evaidnew,
                                        firmid = firm, 
                                    };
                                    db.TblFormEvaluators.Add(assignShift);
                                    db.SaveChanges();
                                    TempData["notice"] = "success";
                                }
                                else
                                {
                                    TempData["notice"] = "exist";
                                }
                            }
                            else
                            {
                                TempData["notice"] = "SameEntry";
                                return RedirectToAction("EvaluatorAssignForm");
                            }                            
                        }
                    }
                }
                return RedirectToAction("EvaluatorAssignForm");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EvaluteAssignEdit(int? id)
        {
            var firm = LoginUserFirmId();
            FormEvaluatorModel model = new FormEvaluatorModel();
            var data = db.TblFormEvaluators.FirstOrDefault(a => a.EvaluatorId == id);
            if (data != null)
            {
                model.Empname =data.Staff.schoolCode +" "+ data.Staff.firstName +" "+ data.Staff.lastName;
                model.EvaluatorId = data.EvaluatorId;
                model.Formempid = (int)data.empid;
                model.FormevaluatorEmpid= (int)data.evaluatorEmpid;
                
                model.firmId=(int)data.firmid;

                model.StaffListOne = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });
                //ViewBag.StaffList = new SelectList(db.Staffs.Where(s=>s.firmId==firm), "staffId", "FullnameNew", data.evaluatorEmpid); 
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EvaluteAssignEdit(FormEvaluatorModel mmodel)
        {
            TblFormEvaluator bts = db.TblFormEvaluators.Find(mmodel.EvaluatorId);
            {
                bts.EvaluatorId = mmodel.EvaluatorId;
                bts.empid = mmodel.Formempid;
                bts.evaluatorEmpid = mmodel.FormevaluatorEmpid;
                bts.firmid = mmodel.firmId;
            }
            db.TblFormEvaluators.AddOrUpdate(bts);
            db.SaveChanges();
            TempData["notice"] = "update";
            return RedirectToAction("EvaluatorAssignForm");
        }

        public ActionResult FormEvaluatorResult(int? staffid)
        {
            var firm = LoginUserFirmId();
            ViewBag.staffid =
              db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });


            if (staffid == null)
            {
                var data = db.TblFormEvaluators.Where(d => d.firmid == firm)
                    .Include(s => s.Staff)
                    .Include(s => s.Staff1);
                return View(data.ToList().Where(d => d.Staff.isActive == true));
            }
            else
            {
                var data = db.TblFormEvaluators.Where(d => d.firmid == firm)
                   .Include(s => s.Staff)
                   .Include(s => s.Staff1).Where(d=>d.evaluatorEmpid==staffid);
                return View(data.ToList().Where(d => d.Staff.isActive == true));
            }
        }

        public ActionResult EmployeeEvaluatorAssignResult()
        {
            var firm = LoginUserFirmId();
            var emp = LoginEmployeeId();
            var data = db.TblFormEvaluators.Where(s => s.empid == emp && s.firmid==firm).Include(s => s.Staff).Include(s => s.Staff1);
            return View(data.ToList().Where(d => d.Staff.isActive == true));
        }
        #endregion

        #region ----- All  Question Methods -----
        public ActionResult EmpQuestionCreateRating()
        {
            var stid = LoginEmployeeId();
            var dt = db.TblRatings.FirstOrDefault(s => s.EmployeId == stid && s.evaluId == null);
            if (dt != null)
            {
                var data = db.GetUpdatedEmpQuestionList(stid);
                var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                {
                    Questions = a.QuestionName,
                    Title = a.Title,
                    Wightage = a.weightage,
                    QuestionId = a.QuestionId,
                    FeedbacFId = a.feedformId,
                    firmid = a.firmid,
                    staffid = a.Empid,
                    Rating = a.ratingper,
                }).ToList();
                return View(assignshiftmodel);
            }
            else
            {
                var data = db.GetEmpQuestionList(stid);
                var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                {
                    Questions = a.QuestionName,
                    Title = a.Title,
                    Wightage = a.weightage,
                    QuestionId = a.QuestionId,
                    FeedbacFId = a.feedformId,
                    firmid = a.firmid,
                    staffid = a.Empid,            
                    Rating = 0,
                }).ToList();
                return View(assignshiftmodel);
            }
            
        }

        public ActionResult SubmitQuestionFromEmp(string staffids, string questionId, string feedbackIds, string rating)
        {
            var perListOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(staffids);
            var perListEnum = perListOj as object[] ?? perListOj.Cast<object>().ToArray();

            var questionIdOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(questionId);
            var monthDaysEnum = questionIdOj as object[] ?? questionIdOj.Cast<object>().ToArray();

            var feedbackIdsOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(feedbackIds);
            var feedbackIdsEnum = feedbackIdsOj as object[] ?? feedbackIdsOj.Cast<object>().ToArray();

            var ratingsOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(rating);
            var ratingsOjEnum = ratingsOj as object[] ?? ratingsOj.Cast<object>().ToArray();

            int alredy = 0;
            int inserted = 0;
            for (int i = 0; i < perListEnum.Count(); i++)
            {
                var firmid = LoginUserFirmId();
                int staffid = Convert.ToInt32(perListEnum[i]);
                int qid = Convert.ToInt32(monthDaysEnum[i]);
                int feedid = Convert.ToInt32(feedbackIdsEnum[i]);
                var rti =
                    db.TblRatings.Where(
                        a => a.formid == feedid && a.qusmasteId == qid && a.EmployeId == staffid && a.evaluId==null).FirstOrDefault();
                if (rti == null)
                {
                    var tblobj = new TblRating()
                    {
                        EmployeId = staffid,
                        qusmasteId = Convert.ToInt32(monthDaysEnum[i]),
                        formid = Convert.ToInt32(feedbackIdsEnum[i]),
                        ratingper = Convert.ToDecimal(ratingsOjEnum[i]),
                        CreatedDate = DateTime.Now,
                        firmId = firmid
                    };
                    db.TblRatings.Add(tblobj);
                    db.SaveChanges();
                    inserted++;
                }
                else
                {
                    TblRating lp = db.TblRatings.Find(rti.RatingId);
                    {
                        lp.EmployeId = staffid;
                        lp.qusmasteId = Convert.ToInt32(monthDaysEnum[i]);
                        lp.formid = Convert.ToInt32(feedbackIdsEnum[i]);
                        lp.ratingper = Convert.ToDecimal(ratingsOjEnum[i]);
                        lp.CreatedDate = DateTime.Now;
                        lp.firmId = firmid;                       
                    }
                    db.TblRatings.AddOrUpdate(lp);
                    db.SaveChanges();
                    alredy++;
                }
            }
            var result = new { Success = "true", Message = "Form submitted Successfully"};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EvaluatorQuestionRating(int? empid)
        {

            var firm = LoginUserFirmId();
            var loginEmployee = LoginEmployeeId();
            ViewBag.staffid =
             db.TblFormEvaluators.Include(s => s.Staff).Where(s => s.evaluatorEmpid == loginEmployee && s.firmid == firm && s.Staff.isActive==true).Select(a => new SelectListItem()
             {
                 Text = a.Staff.staffCode + ":" + a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                 Value = a.Staff.staffId.ToString()
             });
            if (empid != 0)
            {
                var evaid = LoginEmployeeId();
                var fa = db.TblFormAssignments.FirstOrDefault(s => s.Empid == empid);
                if (fa != null)
                {                
                var dt = db.TblRatings.FirstOrDefault(s => s.EmployeId == empid && s.evaluId ==evaid && s.formid==fa.feedformId);
                    if (dt != null)
                    {
                        var data = db.GetQuestionRatingEvaluator(empid, evaid);
                        var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                        {
                            Questions = a.QuestionName,
                            Title = a.Title,
                            Wightage = a.weightage,
                            QuestionId = a.QuestionId,
                            FeedbacFId = a.feedformId,
                            firmid = a.firmid,
                            staffid = a.Empid,
                            Rating = a.ratingper
                        }).ToList();
                        return View(assignshiftmodel);
                    }
                    else
                    {
                        var data = db.GetEmpQuestionList(empid);
                        var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                        {
                            Questions = a.QuestionName,
                            Title = a.Title,
                            Wightage = a.weightage,
                            QuestionId = a.QuestionId,
                            FeedbacFId = a.feedformId,
                            firmid = a.firmid,
                            staffid = a.Empid
                        }).ToList();
                        return View(assignshiftmodel);
                    }
                }
            }
            TempData["none"] = "norecord";
            return View();
        }

        public ActionResult GetEmpList()
        {
            List<int> subcatId = new List<int>();
            List<string> subcat = new List<string>();
            var loginEmployee = LoginEmployeeId();
            Boolean flag = false;
             subcat.Add("Select Employee");
            var firm = LoginUserFirmId();
             subcatId.Add(0);
            foreach (var ex in db.TblFormEvaluators.Include(s => s.Staff).Where(s => s.evaluatorEmpid == loginEmployee))
            {
                subcat.Add(ex.Staff.staffCode + " : " + ex.Staff.firstName + " " + ex.Staff.middleName + " " +
                           ex.Staff.lastName);
                subcatId.Add(ex.Staff.staffId);
            }
            return Json(new { Subcat = subcat, SubcatId = subcatId });
        }

        public ActionResult GetAllEmpList()
        {
            var subcatId = new List<int>();
            var subcat = new List<string>();
          
            var flag = false;
            subcat.Add("All Employee");
            var firm = LoginUserFirmId();
            subcatId.Add(0);
            foreach (var ex in db.FormEvaluatorEmpList().Where(s=>s.firmId==firm))
            {
                subcat.Add(ex.fullname);
                subcatId.Add(ex.staffId);
            }
            return Json(new { Subcat = subcat, SubcatId = subcatId });
        }

        public ActionResult SubmitQuestionFromEvaluator(string staffids, string questionId, string feedbackIds, string rating)
        {
            var lei = LoginEmployeeId();
            var perListOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(staffids);
            var perListEnum = perListOj as object[] ?? perListOj.Cast<object>().ToArray();

            var questionIdOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(questionId);
            var monthDaysEnum = questionIdOj as object[] ?? questionIdOj.Cast<object>().ToArray();

            var feedbackIdsOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(feedbackIds);
            var feedbackIdsEnum = feedbackIdsOj as object[] ?? feedbackIdsOj.Cast<object>().ToArray();

            var ratingsOj = (IEnumerable)new JavaScriptSerializer().DeserializeObject(rating);
            var ratingsOjEnum = ratingsOj as object[] ?? ratingsOj.Cast<object>().ToArray();

            int alredy = 0;
            int inserted = 0;
            int updates = 0;
            for (int i = 0; i < perListEnum.Count(); i++)
            {
                var firmid = LoginUserFirmId();
                int staffid = Convert.ToInt32(perListEnum[i]);
                int qid = Convert.ToInt32(monthDaysEnum[i]);
                int feedid = Convert.ToInt32(feedbackIdsEnum[i]);
                var rtid = db.TblRatings.FirstOrDefault(a => a.formid == feedid && a.qusmasteId == qid && a.EmployeId == staffid && a.evaluId==lei);

                if (rtid == null)
                {
                    var tblobj = new TblRating()
                    {
                        EmployeId = staffid,
                        qusmasteId = Convert.ToInt32(monthDaysEnum[i]),
                        formid = Convert.ToInt32(feedbackIdsEnum[i]),
                        ratingper = Convert.ToDecimal(ratingsOjEnum[i]),
                        CreatedDate = DateTime.Now,
                        firmId = firmid,
                        evaluId = lei
                    };
                    db.TblRatings.Add(tblobj);
                    db.SaveChanges();
                    inserted++;
                }
                else
                {
                    TblRating lp = db.TblRatings.Find(rtid.RatingId);
                    {
                        lp.EmployeId = staffid;
                        lp.qusmasteId = Convert.ToInt32(monthDaysEnum[i]);
                        lp.formid = Convert.ToInt32(feedbackIdsEnum[i]);
                        lp.ratingper = Convert.ToDecimal(ratingsOjEnum[i]);
                        lp.CreatedDate = DateTime.Now;
                        lp.firmId = firmid;
                        lp.evaluId = lei;
                    }
                    db.TblRatings.AddOrUpdate(lp);
                    db.SaveChanges();
                    updates++;
                }
            }
            var result = new { Success = "true", Message = "Form submitted Successfully" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ------ All Rating Reports -------
        public ActionResult EvaluatorRatingReport(int? empid)
        {
            if (empid != 0)
            {
                var evaid = LoginEmployeeId();
                var data = db.GetQuestionRatingEvaluator(empid, evaid);
                var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                {
                    Questions = a.QuestionName,
                    Title = a.Title,
                    Wightage = a.weightage,
                    QuestionId = a.QuestionId,
                    FeedbacFId = a.feedformId,
                    firmid = a.firmid,
                    staffid = a.Empid,
                    Rating = a.ratingper
                }).ToList();
                return View(assignshiftmodel);
            }
            else
            {
                TempData["none"] = "norecord";
                return View();
            }
        }

        public ActionResult EmployeeRatingReport()
        {
                var empId = LoginEmployeeId();              
                var data = db.GetEmpoyeeRatingReport(empId);
                var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                {
                    Questions = a.QuestionName,
                    Title = a.Title,
                    Wightage = a.weightage,
                    QuestionId = a.QuestionId,
                    FeedbacFId = a.feedformId,
                    firmid = a.firmid,
                    staffid = a.Empid,
                    Rating = a.ratingper
                }).ToList();
                return View(assignshiftmodel);            
        }

        public ActionResult AllEmployeeRatingReport(int? empid)
        {

            var firm = LoginUserFirmId();

            ViewBag.staffid =
             db.FormEvaluatorEmpList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
             {
                 Text = a.fullname,
                 Value = a.staffId.ToString()
             });


            if (empid == 0 || empid == null)
            {
                var evaluatorid = LoginEmployeeId();
                var firmid = LoginUserFirmId();
                var data = db.GetRatingAllEmployee(firmid, 0, evaluatorid);
                var assignshiftmodel = data.Select(a => new EvaluatorratingModel()
                {
                    Title = a.title,
                    EmpFName = a.firstname + ' ' + a.middlename + ' ' + a.lastname,
                    EvaluatorFName = a.firstname1 + ' ' + a.middlename1 + ' ' + a.lastname1,
                    Staffcode = a.staffcode,
                    Staffid = a.EmployeeId,
                    Rating = a.ratings,
                    Evalutid=a.evaluid
                }).ToList();
                return View(assignshiftmodel);
            }
            else
            {
                var evaluatorid = LoginEmployeeId();
                var firmid = LoginUserFirmId();
                var data = db.GetRatingAllEmployee(firmid, empid, evaluatorid);
                var assignshiftmodel = data.Select(a => new EvaluatorratingModel()
                {
                    Title = a.title,
                    EmpFName = a.firstname + ' ' + a.middlename + ' ' + a.lastname,
                    EvaluatorFName = a.firstname1 + ' ' + a.middlename1 + ' ' + a.lastname1,
                    Staffcode = a.staffcode,
                    Staffid = a.EmployeeId,
                    Rating = a.ratings,
                    Evalutid = a.evaluid
                }).ToList();
                return View(assignshiftmodel);
            }
        }

        public ActionResult EmployeeRatingDetailsForHr(int? empid , int? evaluteid)
        {
            if (evaluteid == null)
            {
                var data = db.GetEmpoyeeRatingReportForHr(empid,0);
                var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                {
                    Questions = a.QuestionName,
                    Title = a.Title,
                    Wightage = a.weightage,
                    QuestionId = a.QuestionId,
                    FeedbacFId = a.feedformId,
                    firmid = a.firmid,
                    staffid = a.Empid,
                    Rating = a.ratingper
                }).ToList();
                return View(assignshiftmodel);
            }
            else
            {
                var data = db.GetEmpoyeeRatingReportForHr(empid, evaluteid);
                var assignshiftmodel = data.Select(a => new EmpQuestionlistModel()
                {
                    Questions = a.QuestionName,
                    Title = a.Title,
                    Wightage = a.weightage,
                    QuestionId = a.QuestionId,
                    FeedbacFId = a.feedformId,
                    firmid = a.firmid,
                    staffid = a.Empid,
                    Rating = a.ratingper
                }).ToList();
                return View(assignshiftmodel);
            }
        }
        #endregion

        #region ------ All Delete Methods -------
        public ActionResult DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionMaster bts = db.QuestionMasters.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteQuestion(int id)
        {
            try
            {
                QuestionMaster bts = db.QuestionMasters.Find(id);
                db.QuestionMasters.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("CreateForm");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("CreateForm");
            }
        }


        public ActionResult DeleteForm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedbackFormMaster bts = db.FeedbackFormMasters.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteForm(int id)
        {
            try
            {
                FeedbackFormMaster bts = db.FeedbackFormMasters.Find(id);
                db.FeedbackFormMasters.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("FeedbackFormCreate");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("FeedbackFormCreate");
            }
        }


        public ActionResult DeleteAssignQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAssigment bts = db.QuestionAssigments.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteAssignQuestion(int id)
        {
            try
            {
                var qus = db.QuestionAssigments.FirstOrDefault(s => s.QuestionAssId == id);
                var rat = db.TblRatings.Where(s => s.qusmasteId == qus.qustId && s.formid == qus.feedbackId);
                if (rat != null)
                {
                    foreach (var ex in rat)
                    {
                        TblRating tblrat = db.TblRatings.Find(ex.RatingId);
                        db.TblRatings.Remove(tblrat);                       
                    }
                    db.SaveChanges();
                }
                QuestionAssigment bts = db.QuestionAssigments.Find(id);
                db.QuestionAssigments.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("QuestionAssignmentIndex");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("QuestionAssignmentIndex");
            }
        }



        public ActionResult DeleteEvaluteAssign(int? id, int? empid, int? evaluteid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblFormEvaluator bts = db.TblFormEvaluators.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteEvaluteAssign(int id, int? empid, int? evaluteid)
        {
            try
            {
                TblFormEvaluator bts = db.TblFormEvaluators.Find(id);
                db.TblFormEvaluators.Remove(bts);

                var rat = db.TblRatings.Where(s => s.EmployeId == empid && s.evaluId == evaluteid);
                if (rat != null)
                {
                    foreach (var ex in rat)
                    {
                        TblRating tblrat = db.TblRatings.Find(ex.RatingId);
                        db.TblRatings.Remove(tblrat);
                    }
                    db.SaveChanges();
                }

                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("EvaluatorAssignForm");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("EvaluatorAssignForm");
            }
        }


        public ActionResult DeleteFormAssign(int? id, int? formid,int? stffid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblFormAssignment bts = db.TblFormAssignments.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteFormAssign(int id,int? formid, int? stffid)
        {
            try
            {
                TblFormAssignment bts = db.TblFormAssignments.Find(id);
                db.TblFormAssignments.Remove(bts);

                var rat = db.TblRatings.Where(s => s.formid == formid && s.EmployeId == stffid);
                if (rat != null)
                {
                    foreach (var ex in rat)
                    {
                        TblRating tblrat = db.TblRatings.Find(ex.RatingId);
                        db.TblRatings.Remove(tblrat);
                    }
                    db.SaveChanges();
                }

                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("FormAssignmentToEmp");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("FormAssignmentToEmp");
            }
        }

        #endregion
      

    }
}
