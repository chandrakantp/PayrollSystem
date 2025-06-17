using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services;
using edueTree.Models;
using System.Text;

namespace edueTree.Controllers.PayRoll
{
    [Authorize]
    public class CTCController : BaseController
    {
        #region ----------- Dbcontext -------------
        private dbContainer db = new dbContainer();
        #endregion

        #region  ------------ Index ----------------
        public ActionResult Index(int? staffId)
        {
            var firm = LoginUserFirmId();
            var staffctcs = db.StaffCTCs.Where(a => a.firmId == firm).Include(d=>d.SalaryStructure).Include(s => s.Staff).Where(a => a.Staff.isActive == true).OrderBy(s => s.Staff.staffCode);
            return View(staffctcs.ToList());
        }
        #endregion

        #region --------- IncrementReport ---------
        public ActionResult IncrementReport(int? empId)
         {
            var firm = LoginUserFirmId();         
            ViewBag.staffid =
            db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });
            if (empId == null)
            {
                var staffctcs =
                    db.StaffCTCs.Include(s => s.Staff).Where(a => a.firmId == firm && a.Staff.isActive == true);
                return View(staffctcs.ToList());
            }
            else
            {
                var staffctcs =
                  db.StaffCTCs.Include(s => s.Staff).Where(a => a.firmId == firm && a.staffId == empId);
                return View(staffctcs.ToList());
            }
        }
        #endregion

        #region  --------- MyIncrementReport -------
        public ActionResult MyIncrementReport()
        {
            var firm = LoginUserFirmId();
            var staffId = LoginEmployeeId();
            var staffctcs = db.StaffCTCs.Include(s => s.Staff).Where(a => a.staffId == staffId & a.firmId == firm);
            return View(staffctcs.ToList());
        }

        #endregion

        #region  ----------- Ledger ----------------
        public ActionResult Ladger(int? empId)
        {
            var result = db.EmployeeLedger(empId, null, null, null, null);
            return View(result.ToList());
        }

        #endregion

        #region --------- Staff-info --------------
        [HttpPost]
        [WebMethod]
        public ActionResult GetStaffInfo(int deptId)
        {
            var staffId = new List<string>();
            var staffName = new List<string>();
            var staffObj = db.Staffs.Where(a => a.deptId == deptId);
            foreach (var o in staffObj)
            {
                staffId.Add(o.staffId.ToString());
                staffName.Add(o.firstName + " " + o.middleName + " " + o.lastName);
            }
            return Json(new { text = staffName, value = staffId });
        }

        #endregion

        #region ------------ StoreRecord ----------
        [WebMethod]
        public ActionResult StoreRecords(string variableIds, string amtList, string perList, int? staffId, decimal ctcTxt, string status, string narration, DateTime incrimentDate)
        {
            try
            {
                var variableIdsOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(variableIds);
                var variableIdsEnumerable = variableIdsOjbect as object[] ?? variableIdsOjbect.Cast<object>().ToArray();

                var amtListOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(amtList);
                var amtListEnumerable = amtListOjbect as object[] ?? amtListOjbect.Cast<object>().ToArray();

                var perListOjbect = (IEnumerable)new JavaScriptSerializer().DeserializeObject(perList);
                var perListEnumerable = perListOjbect as object[] ?? perListOjbect.Cast<object>().ToArray();
                var fdg = db.StaffCTCs.FirstOrDefault(m => m.staffId == staffId && m.status == "Current");
                if (fdg != null)
                {
                    fdg.status = "Previous";
                    fdg.isActive = false;
                    db.StaffCTCs.AddOrUpdate(fdg);
                }

                var ctcobj = new StaffCTC
                {
                    status = status,
                    staffId = staffId,
                    ctc = ctcTxt,
                    incrementDate = incrimentDate,
                    createdDate = DateTime.UtcNow,
                    createdBy = GetUserId(),
                    narration = narration,
                    isActive = status == "Current",
                    firmId = LoginUserFirmId()
                };
                
                for (int i = 0; i < variableIdsEnumerable.Count(); i++)
                {
                    var salvarObj = new SalaryVariable
                    {
                        StaffCTC = ctcobj,
                        amount = Convert.ToDecimal(amtListEnumerable[i]),
                        variableId = Convert.ToInt32(variableIdsEnumerable[i]),
                        persentage = Convert.ToDecimal(perListEnumerable[i]),
                        firmId = LoginUserFirmId()
                    };
                    db.SalaryVariables.Add(salvarObj);
                }

                db.StaffCTCs.Add(ctcobj);
                db.SaveChanges();

                TempData["message"] = "record successfully inserted";
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("Create", "CTC", new { /* params */ });
                return Json(new { success = true, message = TempData["message"], url = redirectUrl });

                //return RedirectToAction("Index", "CTC");
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

        #endregion

        #region ----------- GetCalculate ----------
        [HttpPost]
        public ActionResult GetCalculate(decimal amt, int deptId)
        {
            var result = db.HeadCalculateWithCTC(deptId, amt);
            //return Json(new { response = result });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ---------- Create -----------------
        public ActionResult Create()
        {
            var firm = LoginUserFirmId();

            StaffCtcModel staffCtcModel = new StaffCtcModel
            {
                StaffList = db.Staffs.Where(a => a.isActive == true).ToList().Where(a => a.firmId == firm).Select(a => new SelectListItem
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                }),
               // StructureList = db.SalaryStructures.ToList().Where(a => a.FirmId == firm).Select(a => new SelectListItem() { Text = a.StuctureName, Value = a.Id.ToString() }),
            };
            return View(staffCtcModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffCtcModel staffctc)
        {
            try
            {
                var firmid = LoginUserFirmId();
                var getuser = GetUserId();

                var getstructid =
                    db.Structurepermissions.FirstOrDefault(s => s.StaffId == staffctc.staffId && s.FirmId == firmid && s.IsDeleted == false);

                if (getstructid != null)
                {
                    if (ModelState.IsValid)
                    {
                        var ctcnew = new StaffCTC
                        {
                            ctc = staffctc.ctc,
                            staffId = staffctc.staffId,
                            incrementDate = staffctc.incrementDate,
                            status = staffctc.status,
                            createdBy = getuser,
                            createdDate = DateTime.Now,
                            isActive = true,
                            narration = staffctc.narration,
                            structId = getstructid.StructId,
                            firmId = firmid,
                        };
                        db.StaffCTCs.Add(ctcnew);
                        db.SaveChanges();
                        TempData["notice"] = "success";
                    }
                    else
                    {
                        TempData["notice"] = "error";
                    }
                }
                else
                {
                    TempData["notice"] = "assignStructerror";
                }
                return RedirectToAction("Create");
            }
            catch (Exception)
            {
                TempData["notice"] = "error";
                return RedirectToAction("Index");
            }
        }
        #endregion
        
        #region -------------- Edit ---------------
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int? id)
        {
            var firm = LoginUserFirmId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffCTC staffctc = db.StaffCTCs.FirstOrDefault(a => a.ctcId == id);
            StaffCtcModel ctcModel = new StaffCtcModel();
            if (staffctc != null)
            {
                ctcModel.ctcId = staffctc.ctcId;
              //  ctcModel.structId = staffctc.structId;
                ctcModel.createdBy = staffctc.createdBy;
                ctcModel.ctc = staffctc.ctc;
                ctcModel.isActive = staffctc.isActive;
                ctcModel.narration = staffctc.narration;
                ctcModel.incrementDate = staffctc.incrementDate;
                ctcModel.status = staffctc.status;
                ctcModel.firmId = staffctc.firmId;
                ctcModel.staffId = staffctc.staffId;
            }
            //ctcModel.StructureList =
            //    db.SalaryStructures.ToList()
            //        .Where(a => a.FirmId == firm)
            //        .Select(a => new SelectListItem() { Text = a.StuctureName, Value = a.Id.ToString() });
            //ViewBag.StructureListNEW = new SelectList(db.SalaryStructures.Where(s => s.FirmId == firm), "Id", "StuctureName", staffctc.structId);

            ctcModel.StaffList = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
            {
                Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });          
           //ViewBag.staffId = new SelectList(db.Staffs, "staffId", "staffCode", staffctc.staffId);
            return View(ctcModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffCtcModel staffCtcModel)
        {
            if (ModelState.IsValid)
            {
                var data = db.StaffCTCs.FirstOrDefault(a => a.ctcId == staffCtcModel.ctcId);
                if (data != null)
                {
                    var stid = db.Structurepermissions.Where(s => s.StaffId == staffCtcModel.staffId).ToList().FirstOrDefault();

                    if (stid != null)
                    {
                        data.structId = stid.StructId;
                    }
                    else
                    {
                        data.structId = null;
                    }
                        data.createdBy = staffCtcModel.createdBy;
                        data.ctc = staffCtcModel.ctc;
                        data.isActive = staffCtcModel.isActive;
                        data.narration = staffCtcModel.narration;
                        data.incrementDate = staffCtcModel.incrementDate;
                        data.status = staffCtcModel.status;
                        data.firmId = staffCtcModel.firmId;
                        data.staffId = staffCtcModel.staffId;
                }
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        } 
        #endregion 

        #region --- Delete and delete Confirm -----
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffCTC department = db.StaffCTCs.Find(id);
            if (department == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(department);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var sv = db.SalaryVariables.Where(a => a.ctcId == id);

                StaffCTC department = db.StaffCTCs.Find(id);
                db.StaffCTCs.Remove(department);
                foreach (var sss in sv)
                {
                    db.SalaryVariables.Remove(sss);
                }

                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region ----------- Despose ---------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region --------- Salary slip -------------
        public ActionResult EmployeeSalarySlip()
        {
            return View();
        }

        #endregion

        #region --------- AttendanceRequest -------
        public ActionResult AdvanceRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdvanceRequest(AdvanceRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var ar = new AdvanceRequest
                {
                    Amount = model.Amount,
                    willPayEMI = model.willPayEMI,
                    staffId = LoginEmployeeId(),
                    requestedDate = DateTime.UtcNow,
                    firmId = LoginUserFirmId(),
                };
                db.AdvanceRequests.Add(ar);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AdvanceRequest");
            }
            return View(model);
        }
        #endregion

        #region -------- AdvanceRequestIndex ------
        public ActionResult AdvanceRequestIndex()
        {
            var firm = LoginUserFirmId();
            int staffId = LoginEmployeeId();
            var index = db.AdvanceRequests.Where(a => a.staffId == staffId & a.firmId == firm);
            return View(index.ToList());
        }
        #endregion 

        #region --- AdvanceRequestPendingReport ---
        public ActionResult AdvanceRequestPendingReport()
        {
            var firm = LoginUserFirmId();
            var index = db.AdvanceRequests.Where(a => a.firmId == firm);
            return View(index.ToList());
        }
        #endregion 

        #region ------- SalarySheetHorizontal -----
        public ActionResult SalarySheetHorizontal(int? month, int? year)
        {
            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            var firmid = LoginUserFirmId();
            var getList = db.SalarySheetHorizontal(month, year, firmid);
            var atsheetModel = getList.Select(a => new PaySlipModel()
            {
                EmployeeName = a.staffCode + ": " + a.firstName + " " + a.lastName,
                HeadName = a.VariableName,
                HeadAmount = a.amount,
                IsEarnings = Convert.ToBoolean(a.isEarnings),
                IsDuductions = Convert.ToBoolean(a.isDuductions),
                tranid = a.tranid,
                variablesId = a.variableid,
                Ctcid = a.ctcid,
                tranmonth = a.tranMonth
            }).ToList();
            //var allTransactionsViewModel = atsheetModel
            //        .GroupBy(t => new { t.EmployeeName, t.HeadName })
            //        .GroupBy(g => g.Key.EmployeeName, g => new { g.Key.HeadName, Total = g.Sum(t => t.HeadAmount) })
            //        ;
            return View(atsheetModel);
        }
        #endregion 

        #region ------- Loan Advance Edit ---------

        public ActionResult AdvanceRequestPendingEdit(int? requestId)
        {
            var editdata = db.AdvanceRequests.FirstOrDefault(a => a.requestId == requestId);
            var model = new AdvancePendingRequestModel();
            {
                model.requestId = editdata.requestId;
                model.fullName = editdata.Staff.staffCode + " : " + editdata.Staff.firstName + " " + editdata.Staff.middleName + " " + editdata.Staff.lastName;
                model.Amount = editdata.Amount;
                model.willPayEMI = editdata.willPayEMI;
                model.FirmId = (int)editdata.firmId;
                model.paidYear = editdata.paidYear;
                model.paidMonth = editdata.paidMonth;
                model.staffId = editdata.staffId;
            }
            return View(model);
        }
        #endregion 

        #region ---- AdvanceRequestPendingEdit ----
        [HttpPost]
        public ActionResult AdvanceRequestPendingEdit(AdvancePendingRequestModel model, string approve, string rejected)
        {
            if (ModelState.IsValid)
            {
                var advdata = db.AdvanceRequests.FirstOrDefault(a => a.requestId == model.requestId);
                if (approve != null)
                {
                    advdata.Amount = model.Amount;
                    advdata.paidMonth = model.paidMonth;
                    advdata.paidYear = model.paidYear;
                    advdata.isApprove = true;
                    advdata.isPaid = true;
                    advdata.firmId = model.FirmId;
                    advdata.willPayEMI = model.willPayEMI;

                    var paidmonth = (int)model.paidMonth;
                    var paidyear = (int)model.paidYear;

                    for (int i = 0; i < model.willPayEMI; i++)
                    {
                        var emi = new AdvanceEMI
                        {
                            month = new DateTime(paidyear, paidmonth, 1).AddMonths(1).Month,
                            year = new DateTime(paidyear, paidmonth, 1).AddMonths(1).Year,
                            Amount = (decimal?)Math.Round((double)(model.Amount / model.willPayEMI), 2),
                            requestId = model.requestId,
                            firmId = model.FirmId,
                            staffid = model.staffId
                        };
                        if (paidmonth == 12)
                            paidyear = new DateTime(paidyear, paidmonth, 1).AddMonths(1).Year;
                        paidmonth = new DateTime(paidyear, paidmonth, 1).AddMonths(1).Month;
                        db.AdvanceEMIs.Add(emi);
                    }
                    db.AdvanceRequests.AddOrUpdate(advdata);
                    db.SaveChanges();
                }

                if (rejected != null)
                {
                    advdata.Amount = model.Amount;
                    advdata.paidMonth = model.paidMonth;
                    advdata.paidYear = model.paidYear;
                    advdata.isApprove = false;
                    advdata.isPaid = false;
                    advdata.firmId = model.FirmId;
                }
                db.AdvanceRequests.AddOrUpdate(advdata);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("AdvanceRequestPendingReport");
            }
            return View(model);
        }
        #endregion 

        #region -------- Bonus Report -------------
        public ActionResult BonusReport(int? empId)
        {
            if (empId != null)
            {
                var firm = LoginUserFirmId();
                var bonus = db.BounusReport(empId);
                //model = new CtcModel();
                var model = bonus.Select(a => new MonthllyAttendanceModel()
                {
                    EmployeeName = a.firstName + " " + a.middleName + " " + a.lastName,
                    TotalAmount = a.amount,
                    Month = (int)a.tranMonth,
                    Year = (int)a.tranYearyear,
                    HeadName = a.HeadName,
                    Narration = a.Narration,
                });
                return View(model);
            }
            else
            {
                var loginempId = LoginEmployeeId();
                var bonus = db.BounusReport(loginempId);

                var model = bonus.Select(a => new MonthllyAttendanceModel()
                {
                    EmployeeName = a.firstName + " " + a.middleName + " " + a.lastName,
                    TotalAmount = a.amount,
                    Month = (int)a.tranMonth,
                    Year = (int)a.tranYearyear,
                    HeadName = a.HeadName,
                    Narration = a.Narration,
                });
                return View(model);
            }
        }
        #endregion 

        #region -------- GetStructureEmp ----------

        public ActionResult GetStructureEmp(int? id)
        {
            var FirmId = LoginUserFirmId();
            var EmployeeList = db.Structurepermissions.ToList().Where(s => s.FirmId == FirmId && s.IsDeleted == false && s.StructId == id).Select(a => new SelectListItem()
            {
                Text = a.Staff.firstName + " " + a.Staff.middleName + " " + a.Staff.lastName,
                Value = a.StaffId.ToString(),
                Selected = true
            });
            return Json(new SelectList(EmployeeList, "Value", "Text"));
        }

        #endregion  

        #region ------ CreateAllowances -----------
        [HttpGet]
        public ActionResult CreateAllowances()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAllowances(TblAllowanceModel smodel)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var qus = new TblAllowance
                {
                    AllowanceName = smodel.AllowanceName,
                    CreatedDate = DateTime.Now,
                    firmId = firm
                };

                db.TblAllowances.Add(qus);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("CreateAllowances");
            }
            return View(smodel);
        }

        #endregion  

        #region ------ AllowancesIndex ------------
        public ActionResult AllowancesIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.TblAllowances.Where(s => s.firmId == firm).ToList());
        }
        #endregion 

        #region -------- DeleteAllowances ---------

        public ActionResult DeleteAllowances(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bts = db.TblAllowances.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteAllowances(int id)
        {
            try
            {
                var bts = db.TblAllowances.Find(id);
                db.TblAllowances.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("CreateAllowances");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("CreateAllowances");
            }
        }
        #endregion 

        #region ---------- EditAllowances ---------
        [HttpGet]
        public ActionResult EditAllowances(int? id)
        {
            TblAllowance bts = db.TblAllowances.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            var model = new TblAllowanceModel();
            model.AllowanceId = bts.AllowanceId;
            model.AllowanceName = bts.AllowanceName;
            model.firmId = (int)bts.firmId;
            return View(model);
        }

        public ActionResult EditAllowances(TblAllowanceModel mmodel)
        {
            var bts = db.TblAllowances.Find(mmodel.AllowanceId);
            {
                bts.AllowanceId = mmodel.AllowanceId;
                bts.AllowanceName = mmodel.AllowanceName;
                bts.firmId = mmodel.firmId;
            }
            db.TblAllowances.AddOrUpdate(bts);
            db.SaveChanges();
            TempData["notice"] = "update";
            return RedirectToAction("CreateAllowances");
        }
        #endregion 

        #region -------- CreateAllowancesRequest --
        public ActionResult CreateAllowancesRequest()
        {
            var firm = LoginUserFirmId();
            var pmodel = new AllowRequestModel
            {
                AllowancesList = new SelectList(db.TblAllowances.Where(s => s.firmId == firm), "AllowanceId", "AllowanceName"),
            };
            return View(pmodel);
        }

        [HttpPost]
        public ActionResult CreateAllowancesRequest(AllowRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var staffid = LoginEmployeeId();

                var qus = new TblAllowTransaction
                {
                    AllowId = model.AllowId,
                    staffid = staffid,
                    StaffRemark = model.StaffRemark,
                    status = "Pending",
                    Amount = model.Amount.Trim(),
                    transactionType = model.transactionType,
                    Expensedate = model.Expensedate,
                    firmid = firm,
                    createdDate = DateTime.UtcNow,
                };
                db.TblAllowTransactions.Add(qus);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AllowancesRequestIndex");
            }
            model.AllowancesList = new SelectList(db.TblAllowances, "AllowanceId", "AllowanceName");
            return View(model);
        }

        #endregion 

        #region ------ AllowancesRequestIndex -----
        public ActionResult AllowancesRequestIndex(DateTime? datepicker, DateTime? datepicker2)
        {
            var empId = LoginEmployeeId();
            var firm = LoginUserFirmId();
            if (datepicker != null && datepicker2 != null)
            {
                var alloRequest =
                db.TblAllowTransactions.Include(l => l.TblAllowance).Include(s => s.Staff)
               .Where(a => a.firmid == firm && a.staffid == empId && (a.createdDate >= datepicker && a.createdDate <= datepicker2)).OrderByDescending(s => s.createdDate);
                return View(alloRequest.ToList());
            }
            else
            {
                var alloRequest =
                db.TblAllowTransactions.Include(l => l.TblAllowance).Include(s => s.Staff)
                    .Where(a => a.firmid == firm && a.staffid == empId)
                    .OrderByDescending(s => s.createdDate);
                return View(alloRequest.ToList());
            }
        }
        #endregion 

        #region -- AllowancesRequestApprovalList --
        public ActionResult AllowancesRequestApprovalList(int? empId, DateTime? datepicker, DateTime? datepicker2)
        {
            int firm = LoginUserFirmId();
            ViewBag.staffid =
              db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
              {
                  Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                  Value = a.staffId.ToString()
              });

            if (empId == 0)
                empId = null;

            if (empId == null && datepicker != null && datepicker2 != null)
            {
                var alloRequest =
                db.TblAllowTransactions.Include(l => l.TblAllowance).Include(s => s.Staff)
               .Where(a => a.firmid == firm && a.Staff.isActive == true && (a.createdDate >= datepicker && a.createdDate <= datepicker2)).OrderByDescending(s => s.createdDate);
                return View(alloRequest.ToList());
            }
            else if (empId != null && datepicker != null && datepicker2 != null)
            {
                var alloRequest =
                db.TblAllowTransactions.Include(l => l.TblAllowance).Include(s => s.Staff)
                .Where(a => a.firmid == firm && a.staffid == empId && a.Staff.isActive == true && (a.createdDate >= datepicker && a.createdDate <= datepicker2)).OrderByDescending(s => s.createdDate);
                return View(alloRequest.ToList());
            }
            else if (empId != null && datepicker == null && datepicker2 == null)
            {
                var alloRequest =
                   db.TblAllowTransactions.Include(l => l.TblAllowance).Include(s => s.Staff)
                       .Where(a => a.firmid == firm && a.staffid == empId)
                       .OrderByDescending(s => s.createdDate);
                return View(alloRequest.ToList());
            }
            else
            {
                var alloRequest =
                    db.TblAllowTransactions.Where(s => s.firmid == firm && s.Staff.isActive == true).Include(l => l.TblAllowance).Include(s => s.Staff)                       
                        .OrderByDescending(s => s.createdDate);
                return View(alloRequest.ToList());
            }

        }
        #endregion 

        #region ------- EditAllowRequest ----------
        public ActionResult EditAllowRequest(int? id, string status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var firmid = LoginUserFirmId();
            var payheadModel = new AllowRequestModel();
            var data = db.TblAllowTransactions.Find(id);

            if (data == null) return View(payheadModel);
            payheadModel.AllowId = data.AllowId;
            payheadModel.AllowTranId = data.AllowTranId;
            payheadModel.staffid = data.staffid;
            payheadModel.Amount = data.Amount;
            payheadModel.StaffRemark = data.StaffRemark;
            payheadModel.AdminRemark = data.AdminRemark;
            payheadModel.firmid = firmid;
            payheadModel.Expensedate = data.Expensedate;
            payheadModel.transactionType = data.transactionType;
            payheadModel.Allowancename = data.TblAllowance.AllowanceName;
            return View(payheadModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAllowRequest(AllowRequestModel request, string ActionName)
        {
            if (ModelState.IsValid)
            {
                string loginuserid = GetUserId();
                TblAllowTransaction lv = db.TblAllowTransactions.Find(request.AllowTranId);
                {
                    lv.AllowTranId = request.AllowTranId;
                    lv.AllowId = request.AllowId;
                    lv.staffid = request.staffid;
                    lv.Expensedate = request.Expensedate;
                    lv.AdminRemark = request.AdminRemark;
                    lv.StaffRemark = request.StaffRemark;
                    lv.Amount = request.Amount.Trim();
                    lv.createdDate = DateTime.UtcNow;
                    lv.status = request.status;
                    lv.transactionType = request.transactionType;
                    lv.ApprovedBy = loginuserid;
                    lv.firmid = request.firmid;
                }
                db.TblAllowTransactions.AddOrUpdate(lv);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction(ActionName);
            }
            return View(request);
        }
        #endregion 

        #region ------ DeleteAllowancesRequest ----
        public ActionResult DeleteAllowancesRequest(int? id, string ActionName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblAllowTransaction bts = db.TblAllowTransactions.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeleteAllowancesRequest(int id, string ActionName)
        {
            try
            {
                TblAllowTransaction bts = db.TblAllowTransactions.Find(id);
                db.TblAllowTransactions.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction(ActionName);
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction(ActionName);
            }
        }
        #endregion 

        #region ----- EditAllowRequestMember ------
        public ActionResult EditAllowRequestMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var firmid = LoginUserFirmId();
            var payheadModel = new AllowRequestModel();
            var data = db.TblAllowTransactions.Find(id);

            if (data == null) return View(payheadModel);
            payheadModel.AllowId = data.AllowId;
            payheadModel.AllowTranId = data.AllowTranId;
            payheadModel.staffid = data.staffid;
            payheadModel.Amount = data.Amount;
            payheadModel.StaffRemark = data.StaffRemark;
            payheadModel.firmid = firmid;
            payheadModel.Expensedate = data.Expensedate;
            payheadModel.transactionType = data.transactionType;
            ViewBag.AllowIdType = new SelectList(db.TblAllowances.Where(s => s.firmId == firmid), "AllowanceId", "AllowanceName", data.AllowId);
            return View(payheadModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAllowRequestMember(AllowRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var lv = db.TblAllowTransactions.Find(request.AllowTranId);
                {
                    lv.AllowTranId = request.AllowTranId;
                    lv.AllowId = request.AllowId;
                    lv.staffid = request.staffid;
                    lv.Expensedate = request.Expensedate;

                    lv.StaffRemark = request.StaffRemark;
                    lv.Amount = request.Amount.Trim();
                    lv.createdDate = DateTime.UtcNow;
                    lv.status = "Pending";
                    lv.transactionType = request.transactionType;
                    lv.firmid = request.firmid;
                }
                db.TblAllowTransactions.AddOrUpdate(lv);
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("AllowancesRequestIndex");
            }
            return View(request);
        }
        #endregion

        #region ----- DeleteLoadAndAdvances -------

        public ActionResult DeleteLoadAndAdvances(int? id, string Actionname)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvanceRequest bts = db.AdvanceRequests.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeleteLoadAndAdvances(int id, string Actionname)
        {
            try
            {
                var qus = db.AdvanceRequests.FirstOrDefault(s => s.requestId == id);
                var rat = db.AdvanceEMIs.Where(s => s.requestId == qus.requestId);
                if (rat != null)
                {
                    foreach (var ex in rat)
                    {
                        AdvanceEMI aemi = db.AdvanceEMIs.Find(ex.emiId);
                        db.AdvanceEMIs.Remove(aemi);
                    }
                    db.SaveChanges();
                }
                AdvanceRequest bts = db.AdvanceRequests.Find(id);
                db.AdvanceRequests.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction(Actionname);
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction(Actionname);
            }
        }
        #endregion

        #region ---AllowancesRequestByAdmin--------
        public ActionResult AllowancesRequestByAdmin()
        {
            var firm = LoginUserFirmId();
            var pmodel = new AllowRequestModel
            {
                AllowancesList = new SelectList(db.TblAllowances.Where(s => s.firmId == firm), "AllowanceId", "AllowanceName"),

                EmployeeAllowList = db.Staffs.Where(a => a.isActive == true).ToList().Where(a => a.firmId == firm).Select(a => new SelectListItem
                {
                    Text = a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                })
            };
            return View(pmodel);
        }

        [HttpPost]
        public ActionResult AllowancesRequestByAdmin(AllowRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var getuid = GetUserId();
                var qus = new TblAllowTransaction
                {
                    AllowId = model.AllowId,
                    staffid = model.staffid,
                    AdminRemark = model.AdminRemark,
                    status = "Approved",
                    Amount = model.Amount.Trim(),
                    transactionType = model.transactionType,
                    Expensedate = model.Expensedate,
                    firmid = firm,
                    createdDate = DateTime.UtcNow,
                    ApprovedBy = getuid
                };
                db.TblAllowTransactions.Add(qus);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AllowancesRequestApprovalList");
            }
            model.AllowancesList = new SelectList(db.TblAllowances, "AllowanceId", "AllowanceName");
            return View(model);
        }

        #endregion

        #region ---- Calculate Month For Salary----

        public ActionResult CalculateMonthForSalary()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateMonthForSalary(CalMonthModel model)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var dt = db.TblCalculateMonths.FirstOrDefault(s => s.firmid == firm);
                if (dt == null)
                {                   
                        var qus = new TblCalculateMonth
                        {
                            CalculateBy = model.Title,
                            firmid = firm,
                            SalaryRule = model.SalaryRule
                        };
                        db.TblCalculateMonths.Add(qus);
                        db.SaveChanges();
                        TempData["notice"] = "success";
                        return RedirectToAction("CalculateMonthForSalary");                    
                }
                else
                {
                    TempData["notice"] = "exist";
                    return RedirectToAction("CalculateMonthForSalary");
                }
            }
            return View(model);
        }

        public ActionResult CalculateMonthIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.TblCalculateMonths.Where(s => s.firmid == firm).ToList());
        }


        public ActionResult DeleteCalculateMonth(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bts = db.TblCalculateMonths.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteCalculateMonth(int id)
        {
            try
            {
                TblCalculateMonth bts = db.TblCalculateMonths.Find(id);
                db.TblCalculateMonths.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("CalculateMonthForSalary");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("CalculateMonthForSalary");
            }
        }
        #endregion

        #region --- Overtime Cal Employee Wise ----

        public ActionResult OvertimecalculateCreate()
        {
            var firm = LoginUserFirmId();
            var config = new OvetimeCalculateModel
            {
                StaffListForOvertime = db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                }),
            };
            return View(config);
        }

        [HttpPost]
        public ActionResult OvertimecalculateCreate(OvetimeCalculateModel model)
        {

            if (model.Overtime == false && model.Undertime == false)
            {
                TempData["notice"] = "falsestatus";
                return RedirectToAction("OvertimecalculateCreate");
            }
            var firm = LoginUserFirmId();
            var data2= db.TblCalculateMonths.FirstOrDefault(s => s.firmid == firm);
            if (data2 != null)
            {
                var data = db.TblOvertimeCalculates.FirstOrDefault(s => s.FirmId == firm && s.StaffId == model.StaffId);
                if (data == null)
                {
                    var qus = new TblOvertimeCalculate
                    {
                        StaffId = model.StaffId,
                        FirmId = firm,
                        PerHour = model.PerHour,
                        Overtime = model.Overtime,
                        Undertime = model.Undertime,
                        calMonthId = data2.CalMonthId,
                    };
                    db.TblOvertimeCalculates.Add(qus);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("OvertimecalculateCreate");
                }
                else
                {
                    TempData["notice"] = "alreadyexist";
                    return RedirectToAction("OvertimecalculateCreate");
                }
            }
            TempData["notice"] = "exist";

                model.StaffListForOvertime =
                db.Staffs.ToList().Where(s => s.firmId == firm && s.isActive == true).Select(a => new SelectListItem()
                {
                    Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                    Value = a.staffId.ToString()
                });

            return View(model);
        }

        //public ActionResult OvertimecalculateIndex()
        //{
        //    var firm = LoginUserFirmId();
        //    return View(db.TblOvertimeCalculates.Include(s => s.Staff).Include(d => d.TblCalculateMonth).Where(s => s.FirmId == firm).ToList());
        //} 

        public ActionResult OvertimecalculateIndex()
        {
            var firm = LoginUserFirmId();
            var data = db.OvertimeSetting(firm);
            var atsheetModel = data.Select(a => new OvetimeCalculateModel()
            {
                StaffId = a.StaffId,
                EmployeeName = a.EmployeeName,
                calculationtype = a.Calculationtype,
                Overtime = a.Overtime != null && (bool)a.Overtime,
                Undertime = a.Undertime != null && (bool)a.Undertime,
                PerHour = a.PerHour,
                FirmId = a.FirmId,
                OvertimeCalId = a.OvertimeCalId
            }).ToList();
            return View(atsheetModel);
        }

        public ActionResult DeleteOvertimeCalculate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bts = db.TblOvertimeCalculates.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeleteOvertimeCalculate(int id)
        {
            try
            {
                var bts = db.TblOvertimeCalculates.Find(id);
                db.TblOvertimeCalculates.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("OvertimecalculateCreate");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("OvertimecalculateCreate");
            }
        }
        public ActionResult EditOvertimeCalculate(int id)
        {
            var firm = LoginUserFirmId();
            var oct = new OvetimeCalculateModel();
            var data = db.TblOvertimeCalculates.FirstOrDefault(a => a.OvertimeCalId == id);
            if (data != null)
            {                 
                oct = new OvetimeCalculateModel
                {
                    StaffListForOvertime = db.Staffs.ToList().Where(s => s.firmId == firm).Select(a => new SelectListItem()
                    {
                        Text = a.schoolCode + ":" + a.firstName + " " + a.middleName + " " + a.lastName,
                        Value = a.staffId.ToString()
                    }),

                    OvertimeCalId = data.OvertimeCalId,
                    FirmId = data.FirmId,
                    Overtime = (bool)data.Overtime,
                    Undertime = (bool)data.Undertime,
                    StaffId = data.StaffId,
                    PerHour = data.PerHour,
                    CalMonthId = data.calMonthId
                };
            }
            return View(oct);
        }
      
        [HttpPost]
        public ActionResult EditOvertimeCalculate(OvetimeCalculateModel mmodel)
        {
            try
            {
                if (mmodel.Overtime == false && mmodel.Undertime == false)
                {
                    TempData["notice"] = "falsestatus";
                    return RedirectToAction("EditOvertimeCalculate",new{ mmodel.OvertimeCalId});
                }
                var data = db.TblOvertimeCalculates.FirstOrDefault(a => a.OvertimeCalId == mmodel.OvertimeCalId);
                if (data != null)
                {
                  
                        data.OvertimeCalId = mmodel.OvertimeCalId;
                        data.FirmId = mmodel.FirmId;
                        data.Overtime = (bool) mmodel.Overtime;
                        data.Undertime = (bool) mmodel.Undertime;
                        data.StaffId = mmodel.StaffId;
                        data.PerHour = mmodel.PerHour;
                        data.calMonthId = mmodel.CalMonthId;

                        db.TblOvertimeCalculates.AddOrUpdate(data);
                        db.SaveChanges();
                        TempData["notice"] = "update";
                        return RedirectToAction("OvertimecalculateCreate");
                    }                                                 
                return RedirectToAction("OvertimecalculateCreate");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region ------ Calculate Salary Rule ------

        public ActionResult CalculateSalaryRule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateSalaryRule(CalMonthModel model)
        {
            if (model.Title != null)
            {
                var firm = LoginUserFirmId();
                var dt = db.tblSalaryCalculateRules.FirstOrDefault(s => s.firmid == firm);
                if (dt == null)
                {
                    var qus = new tblSalaryCalculateRule
                    {
                        CalculateBy = model.Title,
                        firmid = firm,
                    };
                    db.tblSalaryCalculateRules.Add(qus);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("CalculateSalaryRule");
                }
                else
                {
                    TempData["notice"] = "exist";
                    return RedirectToAction("CalculateSalaryRule");
                }
            }
            else
            {
                TempData["notice"] = "plzselect";
                return RedirectToAction("CalculateSalaryRule");
            }
            return View(model);
        }

        public ActionResult CalculateSalaryRuleIndex()
        {
            var firm = LoginUserFirmId();
            return View(db.tblSalaryCalculateRules.Where(s => s.firmid == firm).ToList());
        }

        public ActionResult DeleteCalculateSalaryRule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bts = db.tblSalaryCalculateRules.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeleteCalculateSalaryRule(int id)
        {
            try
            {
                var bts = db.tblSalaryCalculateRules.Find(id);
                db.tblSalaryCalculateRules.Remove(bts);
                db.SaveChanges();
                TempData["notice"] = "delete";
                return RedirectToAction("CalculateSalaryRule");
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("CalculateSalaryRule");
            }
        }
        #endregion

        #region ----- DeleteSalaryVariables -------

        public ActionResult DeleteSalaryVariables(int? id, int? month, int? tranid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SalaryVariable bts = db.SalaryVariables.Find(tranid);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }

        [HttpPost]
        public ActionResult DeleteSalaryVariables(int id, int? month)
        {
            try
            {               
                var rat = db.SalaryVariables.Where(s => s.ctcId == id && s.tranMonth == month);
                if (rat != null)
                {
                    foreach (var ex in rat)
                    {
                        SalaryVariable aemi = db.SalaryVariables.Find(ex.tranId);
                        db.SalaryVariables.Remove(aemi);
                    }
                    db.SaveChanges();
                }            
                TempData["notice"] = "delete";
                return RedirectToAction("SalarySheetHorizontal", new { month = month, year = 2017 });
            }
            catch
            {
                TempData["notice"] = "cantdelete";
                return RedirectToAction("SalarySheetHorizontal");
            }
        }
        #endregion

    }
}

public class Demo
{
    public string Text { get; set; }
}


