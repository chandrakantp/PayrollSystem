using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Services;
using edueTree.Models;

namespace edueTree.Controllers.PayRoll
{
    [Authorize]
    public class VariableSettingsController : BaseController
    {
        #region ------------ Db-context -------------
        private dbContainer db = new dbContainer();
        #endregion

        #region -------------- Index ----------------
        public ActionResult Index()
        {
            var firm = LoginUserFirmId();
            var variablesettings = db.GetPayheadList(firm).Select(a => new VariableSettingModel()
            {
                VariableId = a.variableId,
                VariableName = a.VariableName,
                Status = a.Status,
                Percentage = a.percentage.TrimEnd('+'),
                IsCalculateFromCtc = (bool)a.isCalculateFromCTC,
                IsEarnings = (bool)a.isEarnings,
                IsDuductions = (bool)a.isDuductions,
                payheadtype = a.payheadtype,
                Under = a.Under,
                payslipname = a.payslipname,
                calculationtype = a.Calculationtype,
                Structureid = a.SalaryStructId,
                Structname = a.StuctureName,
                affnetsalary = a.affectnetsal,
                Calculationperiod = a.calculationperiod,
                Value = a.Value,
                IsearningDeduction = a.EarnDeduct,
            }).ToList();
            return View(variablesettings);
        }
        #endregion

        #region -------------- Create ---------------
        [HttpGet]
        public ActionResult Create()
        {
            var firmId = LoginUserFirmId();
            var model = new VariableSettingModel
            {
                CalculateList1 = new SelectList(db.VariableSettings.Where(a => a.firmId == firmId), "variableId", "VariableName"),
                CalculateList2 = new SelectList(db.VariableSettings.Where(a => a.firmId == firmId), "variableId", "VariableName"),
                //CalculateList3 = new SelectList(db.VariableSettings.Where(a => a.firmId == firmId), "variableId", "VariableName"),
                DepartmentList = new SelectList(db.Departments.Where(a => a.firmId == firmId), "deptId", "deptName"),
                StructureList =
                   db.SalaryStructures.ToList()
                       .Where(s => s.FirmId == firmId && s.IsActive == true)
                       .Select(a => new SelectListItem()
                       {
                           Text = a.StuctureName,
                           Value = a.Id.ToString()
                       })
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VariableSettingModel varsetting)
        {
            try
            {
                var firmId = LoginUserFirmId();
                if (db.VariableSettings.Any(m => m.VariableName == varsetting.VariableName && m.firmId == firmId && m.SalaryStructId == varsetting.Structureid))
                {
                    TempData["notice"] = "exist";
                    return RedirectToAction("Create");
                }
                var count = 1;

                var obj = new VariableSetting
                {
                    VariableName = varsetting.VariableName,
                    Status = varsetting.Status,
                    isCalculateFromCTC = varsetting.IsCalculateFromCtc,
                    calculatemulti = ConvertStringArrayToString(varsetting.CalculateFrom1),
                    //calculateFrom1 = varsetting.CalculateFrom1 == 0 ? (int?)null : varsetting.CalculateFrom1,
                    calculateFrom2 = varsetting.CalculateFrom2 == 0 ? (int?)null : varsetting.CalculateFrom2,
                    //calculateFrom3 = varsetting.CalculateFrom3 == 0 ? (int?)null : varsetting.CalculateFrom3,
                    graterThanLimit = varsetting.GraterThanLimit,
                    graterLimitAmt = varsetting.GraterLimitAmt,
                    lessThanLimit = varsetting.LessThanLimit,
                    lessLimitAmt = varsetting.LessLimitAmt,
                    persentage = varsetting.Persentage,
                    isActive = true,
                    isEarnings = varsetting.IsEarnings,
                    isDuductions = varsetting.IsDuductions,
                    createdDate = DateTime.UtcNow,
                    createdBy = GetUserId(),
                    payslipname = varsetting.payslipname,
                    payheadtype = varsetting.payheadtype,
                    affectnetsal = varsetting.affnetsalary,
                    gratuity = varsetting.Graduity,
                    Under = varsetting.Under,
                    firmId = firmId,
                    attendaceleave = varsetting.attendaceleave,
                    statuspay = varsetting.Status,
                    SalaryStructId = varsetting.Structureid,
                    calculationperiod = varsetting.Calculationperiod,
                    Calculationtype = varsetting.calculationtype,
                    incometaxdetails = varsetting.Incometaxdetails,
                    Roundingmethod = varsetting.Roundingmethod,
                    roundinglimit = varsetting.roundinglimit,
                    EarnDeduct = varsetting.IsearningDeduction,
                    PerValue = varsetting.PerValue,
                    Value = varsetting.Value,
                    ComputeOn = varsetting.compute,
                    productiontype = varsetting.productiontype,
                    statutoryPayType = varsetting.statutorypaytype,
                    statutoryRegiNo = varsetting.RegistrationNo
                };
                db.VariableSettings.Add(obj);
                db.SaveChanges();

                if (varsetting.calculationtype == "As Computed Value")
                {
                    string[] effectivefrom = Request.Form.GetValues("effectivefrom");
                    string[] fromamount = Request.Form.GetValues("fromamount");
                    string[] amountupto = Request.Form.GetValues("amountupto");
                    string[] Slabtype = Request.Form.GetValues("Slabtype");
                    string[] Valuebasis = Request.Form.GetValues("valuebasis");

                    for (int i = 0; i <= effectivefrom.Length - 1; i++)
                    {

                        var spacifiedFormula = new TblSpecifiedFormula();
                        if (!string.IsNullOrEmpty(effectivefrom[i]))
                        {
                            spacifiedFormula.EffectiveFrom = Convert.ToDateTime(effectivefrom[i]);
                        }
                        if (!string.IsNullOrEmpty(fromamount[i]))
                        {
                            spacifiedFormula.FromAmount = Convert.ToDecimal(fromamount[i]);
                        }
                        if (!string.IsNullOrEmpty(amountupto[i]))
                        {
                            spacifiedFormula.AmountUpto = Convert.ToDecimal(amountupto[i]);
                        }
                        if (!string.IsNullOrEmpty(Slabtype[i]))
                        {
                            spacifiedFormula.SlabType = Convert.ToBoolean(Slabtype[i]);
                        }
                        if (!string.IsNullOrEmpty(Valuebasis[i]))
                        {
                            spacifiedFormula.ValueBasis = Convert.ToDecimal(Valuebasis[i]);
                        }

                        spacifiedFormula.FirmId = firmId;
                        spacifiedFormula.VariableSetting = obj;

                        db.TblSpecifiedFormulas.Add(spacifiedFormula);
                        db.SaveChanges();
                    }
                }

                if (varsetting.payheadtype == "Gratuity")
                {
                    string[] from = Request.Form.GetValues("from");
                    string[] to = Request.Form.GetValues("to");
                    string[] amount = Request.Form.GetValues("amount");

                    for (int i = 0; i <= from.Length - 1; i++)
                    {
                        var GratuityCalculation = new TblGratuityCalculation
                        {
                            Firmid = firmId,
                            FromMonth = Convert.ToInt32(from[i]),
                            ToMonth = Convert.ToInt32(to[i]),
                            EligibleDays = Convert.ToInt32(amount[i]),
                            VariableSetting = obj
                        };
                        db.TblGratuityCalculations.Add(GratuityCalculation);
                        db.SaveChanges();
                    }
                }
                TempData["notice"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["notice"] = "error";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region ----------- DeleteConfirm -----------

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariableSetting variablesetting = db.VariableSettings.Find(id);
            if (variablesetting == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(variablesetting);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VariableSetting variablesetting = db.VariableSettings.Find(id);
            db.VariableSettings.Remove(variablesetting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion        

        #region ---------- GetVariableInfo ----------

        [HttpPost]
        [WebMethod]
        public ActionResult GetVariableInfo(int deptId)
        {
            var firmId = LoginUserFirmId();
            var variableId = new List<string>();
            var variableName = new List<string>();
            var variableObj = db.VariableSettings.Where(a => a.deptId == deptId && a.firmId == firmId);
            foreach (var o in variableObj)
            {
                variableId.Add(o.variableId.ToString());
                variableName.Add(o.VariableName);
            }
            return Json(new { text = variableName, value = variableId });
        }

        #endregion

        #region ----------- MachineConfig -----------

        public ActionResult MachineConfig(int? id = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var config = db.MachineConfigures.Find(id);
            if (config == null)
            {

                return RedirectToAction("pagenotfound", "Home");
            }
            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MachineConfig(MachineConfigure config)
        {
            if (ModelState.IsValid)
            {
                db.Entry(config).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "update";
                return RedirectToAction("MachineConfig");
            }
            return View(config);
        }
        #endregion

        #region ----------- DeleteVariable ----------
        public ActionResult DeleteVariable(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariableSetting bts = db.VariableSettings.Find(id);
            if (bts == null)
            {
                return RedirectToAction("pagenotfound", "Home");
            }
            return View(bts);
        }
        [HttpPost]
        public ActionResult DeleteVariable(int id)
        {
            try
            {
                var tblSpecified = db.TblSpecifiedFormulas.Where(a => a.FormulaVariableId == id).ToList();
                foreach (var VARIABLE in tblSpecified)
                {
                    TblSpecifiedFormula bts1 = db.TblSpecifiedFormulas.Find(VARIABLE.FormulaId);
                    db.TblSpecifiedFormulas.Remove(bts1);
                    db.SaveChanges();
                }

                var tblgratuity = db.TblGratuityCalculations.Where(a => a.VariableId == id).ToList();
                foreach (var ex in tblgratuity)
                {
                    TblGratuityCalculation btsgratuity = db.TblGratuityCalculations.Find(ex.GratuityId);
                    db.TblGratuityCalculations.Remove(btsgratuity);
                    db.SaveChanges();
                }

                var firmid = LoginUserFirmId();
                VariableSetting bts = db.VariableSettings.Find(id);
                var Data = db.VariableSettings.Where(a => a.firmId == firmid && a.calculatemulti != "").ToList();
                foreach (var VARIABLE in Data)
                {
                    string[] words = VARIABLE.calculatemulti.Split(',');
                    foreach (string varid in words)
                    {
                        if (varid == Convert.ToString(id))
                        {
                            var Result = db.TblSpecifiedFormulas.Where(a => a.FormulaVariableId == VARIABLE.variableId).ToList();
                            if (Result != null)
                            {
                                foreach (var Re in Result)
                                {
                                    db.TblSpecifiedFormulas.Remove(Re);
                                    db.SaveChanges();
                                }
                            }
                            db.VariableSettings.Remove(VARIABLE);
                            db.SaveChanges();
                        }
                    }
                }

                db.VariableSettings.Remove(bts);
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
        
        #region ------------ EditVariable -----------

        public ActionResult EditVariable(int? id)
        {
            var firm = LoginUserFirmId();
            VariableSettingModel model = new VariableSettingModel();
            var data = db.VariableSettings.FirstOrDefault(a => a.variableId == id);
            if (data != null)
            {
                model.Structureid = data.SalaryStructId;
                model.VariableId = data.variableId;
                model.VariableName = data.VariableName;
                model.payslipname = data.payslipname;
                model.payheadtype = data.payheadtype;
                model.Under = data.Under;
                model.calculationtype = data.Calculationtype;
                model.affnetsalary = data.affectnetsal;
                model.Calculationperiod = data.calculationperiod;
                model.EarnDeduct = data.EarnDeduct;
                model.PerValue = data.PerValue;
                model.Value = data.Value;
                model.compute = data.ComputeOn;
                model.Status = data.Status;
                model.IsCalculateFromCtc = (bool)data.isCalculateFromCTC;
                model.Persentage = data.persentage;
                model.IsEarnings = (bool)data.isEarnings;
                model.IsDuductions = (bool)data.isDuductions;
                model.GraterThanLimit = data.graterThanLimit;
                model.GraterLimitAmt = data.graterLimitAmt;
                model.LessThanLimit = data.lessThanLimit;
                model.LessLimitAmt = data.lessLimitAmt;
                // model.CalculateFrom1 = data.calculateFrom1;
                model.CalculateFrom2 = data.calculateFrom2;
                model.Firmid = (int)data.firmId;
                model.CreatedBy = data.createdBy;
                model.IsearningDeduction = data.EarnDeduct;
                model.Roundingmethod = data.Roundingmethod;
                model.productiontype = data.productiontype;
                model.roundinglimit = (int)data.roundinglimit;
                model.Graduity = data.gratuity;
                model.Incometaxdetails = data.incometaxdetails;
                model.attendaceleave = data.attendaceleave;
                model.statutorypaytype = data.statutoryPayType;
                model.RegistrationNo = data.statutoryRegiNo;
                //model.statutorypaytype=data.

                model.StructureList =
                    db.SalaryStructures.ToList()
                        .Where(s => s.FirmId == firm && s.IsActive == true)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.StuctureName,
                            Value = a.Id.ToString()
                        });
                ViewBag.DeptList = new SelectList(db.Departments.Where(a => a.firmId == firm), "deptId", "deptName", data.deptId);
                ViewBag.CalculateList1 = new SelectList(db.VariableSettings.Where(a => a.firmId == firm), "variableId",
                    "VariableName", data.calculateFrom1);
                ViewBag.CalculateList2 = new SelectList(db.VariableSettings.Where(a => a.firmId == firm), "variableId",
                    "VariableName", data.calculateFrom1);
            }
            model.TblSpecifiedList = db.TblSpecifiedFormulas.Where(a => a.FormulaVariableId == data.variableId).ToList();
            model.TblgratuityList = db.TblGratuityCalculations.Where(a => a.VariableId == data.variableId).ToList();

            var FirmId = LoginUserFirmId();
            string csv = db.VariableSettings.FirstOrDefault(s => s.variableId == model.VariableId).calculatemulti;
            int mos = 0;
            var intList = csv.Split(',')
                                .Select(m => { int.TryParse(m, out mos); return mos; })
                                .Where(m => m != 0)
                                .ToList();
            var varlist = intList;
            var selected = varlist;
            var categories = db.VariableSettings.Where(s => s.firmId == FirmId && s.isActive == true && s.SalaryStructId == model.Structureid)
                .AsEnumerable()
                .Select(c => new SelectListItem
                {
                    Value = c.variableId.ToString(),
                    Text = c.VariableName,
                    Selected = selected.Contains(c.variableId)
                }).ToList();
            ViewBag.Categories = categories;
            return View(model);
        } 

        [HttpPost]
        public ActionResult EditVariable(VariableSettingModel varsetting)
        {
            try
            {
                var firmId = LoginUserFirmId();
                var data = db.VariableSettings.FirstOrDefault(a => a.variableId == varsetting.VariableId);
                if (data != null)
                {
                    data.VariableName = varsetting.VariableName;
                    data.Status = varsetting.Status;
                    data.isCalculateFromCTC = varsetting.IsCalculateFromCtc;
                    data.calculatemulti = ConvertStringArrayToString(varsetting.CalculateFrom1);
                    data.calculateFrom2 = varsetting.CalculateFrom2 == 0 ? (int?)null : varsetting.CalculateFrom2;
                    data.graterThanLimit = varsetting.GraterThanLimit;
                    data.graterLimitAmt = varsetting.GraterLimitAmt;
                    data.lessThanLimit = varsetting.LessThanLimit;
                    data.lessLimitAmt = varsetting.LessLimitAmt;
                    data.persentage = varsetting.Persentage;
                    data.isActive = true;
                    data.isEarnings = varsetting.IsEarnings;
                    data.isDuductions = varsetting.IsDuductions;
                    data.createdDate = DateTime.UtcNow;
                    data.createdBy = GetUserId();
                    data.payslipname = varsetting.payslipname;
                    data.payheadtype = varsetting.payheadtype;
                    data.affectnetsal = varsetting.affnetsalary;
                    data.gratuity = varsetting.Graduity;
                    data.Under = varsetting.Under;
                    data.deptId = varsetting.deptId;
                    data.firmId = firmId;
                    data.attendaceleave = varsetting.attendaceleave;
                    data.statuspay = varsetting.Status;
                    data.SalaryStructId = varsetting.Structureid;
                    data.calculationperiod = varsetting.Calculationperiod;
                    data.Calculationtype = varsetting.calculationtype;
                    data.incometaxdetails = varsetting.Incometaxdetails;
                    data.Roundingmethod = varsetting.Roundingmethod;
                    data.roundinglimit = varsetting.roundinglimit;
                    data.EarnDeduct = varsetting.IsearningDeduction;
                    data.PerValue = varsetting.PerValue;
                    data.Value = varsetting.Value;
                    data.ComputeOn = varsetting.compute;
                    data.productiontype = varsetting.productiontype;
                    data.statutoryPayType = varsetting.statutorypaytype;
                    data.statutoryRegiNo = varsetting.RegistrationNo;

                    db.VariableSettings.AddOrUpdate(data);
                    db.SaveChanges();
                }
                if (varsetting.calculationtype == "As Computed Value")
                {
                    string[] formulaId = Request.Form.GetValues("FormulaId");
                    string[] effectivefrom = Request.Form.GetValues("effectivefrom");
                    string[] fromamount = Request.Form.GetValues("fromamount");
                    string[] amountupto = Request.Form.GetValues("amountupto");
                    string[] Slabtype = Request.Form.GetValues("Formulaslabtype");
                    string[] Valuebasis = Request.Form.GetValues("valuebasis");

                    for (int i = 0; i <= effectivefrom.Length - 1; i++)
                     {
                        var frid = Convert.ToString(formulaId[i]);

                        if (!string.IsNullOrEmpty(frid))
                        {
                            TblSpecifiedFormula spformula = db.TblSpecifiedFormulas.Find(Convert.ToInt32(frid));
                            if (!string.IsNullOrEmpty(effectivefrom[i]))
                            {
                                spformula.EffectiveFrom = Convert.ToDateTime(effectivefrom[i]);
                            }
                            if (!string.IsNullOrEmpty(fromamount[i]))
                            {
                                spformula.FromAmount = Convert.ToDecimal(fromamount[i]);
                            }
                            if (!string.IsNullOrEmpty(amountupto[i]))
                            {
                                spformula.AmountUpto = Convert.ToDecimal(amountupto[i]);
                            }
                            if (!string.IsNullOrEmpty(Slabtype[i]))
                            {
                                spformula.SlabType = Convert.ToBoolean(Slabtype[i]);
                            }
                            if (!string.IsNullOrEmpty(Valuebasis[i]))
                            {
                                spformula.ValueBasis = Convert.ToDecimal(Valuebasis[i]);
                            }
                            db.TblSpecifiedFormulas.AddOrUpdate(spformula);
                            db.SaveChanges();
                        }
                        else
                        {
                            var spacifiedFormula = new TblSpecifiedFormula();
                            if (!string.IsNullOrEmpty(effectivefrom[i]))
                            {
                                spacifiedFormula.EffectiveFrom = Convert.ToDateTime(effectivefrom[i]);
                            }
                            if (!string.IsNullOrEmpty(fromamount[i]))
                            {
                                spacifiedFormula.FromAmount = Convert.ToDecimal(fromamount[i]);
                            }
                            if (!string.IsNullOrEmpty(amountupto[i]))
                            {
                                spacifiedFormula.AmountUpto = Convert.ToDecimal(amountupto[i]);
                            }
                            if (!string.IsNullOrEmpty(Slabtype[i]))
                            {
                                spacifiedFormula.SlabType = Convert.ToBoolean(Slabtype[i]);
                            }
                            if (!string.IsNullOrEmpty(Valuebasis[i]))
                            {
                                spacifiedFormula.ValueBasis = Convert.ToDecimal(Valuebasis[i]);
                            }
                            spacifiedFormula.FirmId = firmId;
                            spacifiedFormula.FormulaVariableId = varsetting.VariableId;
                            db.TblSpecifiedFormulas.Add(spacifiedFormula);
                            db.SaveChanges();
                        }
                    }          
                }

                if (varsetting.payheadtype == "Gratuity")
                {
                    string[] gratuityid = Request.Form.GetValues("gratuityid");
                    string[] from = Request.Form.GetValues("fromMonthGratuity");
                    string[] to = Request.Form.GetValues("toMonthGratuity");
                    string[] amount = Request.Form.GetValues("eligibleDaysGratuity");

                    for (int i = 0; i <= from.Length - 1; i++)
                    {
                        var grid = Convert.ToString(gratuityid[i]);

                        if (!string.IsNullOrEmpty(grid))
                        {
                            TblGratuityCalculation spGratuityformula =
                            db.TblGratuityCalculations.Find(Convert.ToInt32(grid));
                            spGratuityformula.Firmid = firmId;
                            spGratuityformula.FromMonth = Convert.ToInt32(from[i]);
                            spGratuityformula.ToMonth = Convert.ToInt32(to[i]);
                            spGratuityformula.EligibleDays = Convert.ToInt32(amount[i]);
                            db.TblGratuityCalculations.AddOrUpdate(spGratuityformula);
                            db.SaveChanges();
                        }
                        else
                        {
                            var GratuityCalculation = new TblGratuityCalculation
                            {
                                Firmid = firmId,
                                FromMonth = Convert.ToInt32(from[i]),
                                ToMonth = Convert.ToInt32(to[i]),
                                EligibleDays = Convert.ToInt32(amount[i]),
                                VariableId = varsetting.VariableId
                            };
                            db.TblGratuityCalculations.Add(GratuityCalculation);
                            db.SaveChanges();
                        }
                    }
                }
                TempData["notice"] = "update";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["notice"] = "error";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region ------------- GetPayhead ------------

        public ActionResult GetPayhead(int? id)
        {
            var FirmId = LoginUserFirmId();
            var variableList = db.VariableSettings.ToList().Where(s => s.firmId == FirmId && s.isActive == true && s.SalaryStructId == id).Select(a => new SelectListItem()
            {
                Text = a.VariableName,
                Value = a.variableId.ToString()
            });
            return Json(new SelectList(variableList, "Value", "Text"));
        }

        #endregion

        #region ----- ConvertStringArrayToString ----
        public string ConvertStringArrayToString(string[] array)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();

            if (array != null)
            {
                foreach (string value in array)
                {
                    builder.Append(value);
                    builder.Append(',');
                }
            }

            return builder.ToString();
        }
        #endregion

        #region -- ConvertStringArrayToStringJoin ---
        static string ConvertStringArrayToStringJoin(string[] array)
        {
            // Use string Join to concatenate the string elements.
            string result = string.Join(",", array);
            return result;
        }
        #endregion

        #region --------- Payhead Replicate ---------
        public ActionResult CopyPayheads(int? id)
        {
            var firm = LoginUserFirmId();
            var getvariableIds = db.VariableSettings.Where(a => a.SalaryStructId == id && a.firmId == firm).ToList();

            foreach (var ex in getvariableIds)
            {
                if (db.VariableSettings.Any(a => a.variableId == ex.variableId))
                {
                    db.RepliacatePayheads(ex.variableId, ex.SalaryStructId, ex.firmId);
                }
            }

            TempData["notice"] = "CopyPayheads";
            return RedirectToAction("Index");
        }
        #endregion

        #region ----- Delete Gratuity-Specified -----

        public ActionResult DeleteGratuityHead(int? grtid)
        {
            TblGratuityCalculation grt = db.TblGratuityCalculations.Find(grtid);
            db.TblGratuityCalculations.Remove(grt);
            db.SaveChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSpeciFormulaHead(int? grtspid)
        {
            TblSpecifiedFormula grt = db.TblSpecifiedFormulas.Find(grtspid);
            db.TblSpecifiedFormulas.Remove(grt);
            db.SaveChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region -------------  Dispose --------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
