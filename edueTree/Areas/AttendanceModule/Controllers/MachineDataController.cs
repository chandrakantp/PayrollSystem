using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edueTree.Controllers;
using edueTree.Models;

namespace edueTree.Areas.AttendanceModule.Controllers
{
    public class MachineDataController : BaseController
    {
        #region DBContext
        private dbContainer db = new dbContainer();
        #endregion

        #region Index

        //
        // GET: /AttendanceModule/MachineData/
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region Details
        //
        // GET: /AttendanceModule/MachineData/Details/5
        public ActionResult Details(int id)
        {
            return View();
        } 
        #endregion

        #region Create
        //
        // GET: /AttendanceModule/MachineData/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AttendanceModule/MachineData/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        } 
        #endregion

        #region Edit
        //
        // GET: /AttendanceModule/MachineData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AttendanceModule/MachineData/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        } 
        #endregion

        #region Delete
        //
        // GET: /AttendanceModule/MachineData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AttendanceModule/MachineData/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        } 
        #endregion

        #region MachineData

        public ActionResult MachineData(DateTime? datepicker, DateTime? datepicker2, int? empId, string AttendmodeList)
        {
            var firmId = LoginUserFirmId();
            if (datepicker == null && datepicker2 == null)
            {
                DateTime date = DateTime.UtcNow;
                var start = new DateTime(date.Year, date.Month, 1);
                var end = start.AddMonths(1).AddDays(-1);
                var staffId = LoginEmployeeId();

                var data = db.MachineData(start, end, staffId, firmId, AttendmodeList);
                //AttendanceStaffs.Where(a => a.enrollNumber == "33");

                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + ": " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                }).ToList();

                return View(atsheetModel);
            }
            else
            {
                DateTime start = (DateTime)datepicker;
                DateTime end = (DateTime)datepicker2;

                var data = db.MachineData(start, end, empId, firmId, AttendmodeList);
                //AttendanceStaffs.Where(a => a.enrollNumber == "33");

                var atsheetModel = data.Select(a => new AttendanceSheetModel()
                {
                    EmployeeName = a.staffCode + " " + a.firstName + " " + a.middleName + " " + a.lastName,
                    TranDate = a.attendDate,
                }).ToList();

                return View(atsheetModel);
            }
        }

        #endregion
    }
}
