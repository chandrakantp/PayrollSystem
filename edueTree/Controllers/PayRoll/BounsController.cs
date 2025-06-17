using System;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.PayRoll
{
    public class BounsController: BaseController
    {
        #region ------- DbContext -------
        private dbContainer db = new dbContainer(); 
        #endregion

        #region ------- AddBonus --------
        public ActionResult AddBonus()
        {
            var firm = LoginUserFirmId();
            BonusModel model = new BonusModel();
            model.StaffList = db.Staffs.Where(a => a.isActive == true && a.firmId == firm).ToList().Select(a => new SelectListItem()
            {
                Text = a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });

            model.MonthList = new SelectList(new[]
            {
                new SelectListItem {Text = "----  Select ----", Value = null, Selected = true},
                new SelectListItem {Text = "January", Value = "1"},
                new SelectListItem {Text = "February", Value = "2"},
                new SelectListItem {Text = "March", Value = "3"},
                new SelectListItem {Text = "April", Value = "4"},
                new SelectListItem {Text = "May", Value = "5"},
                new SelectListItem {Text = "June", Value = "6"},
                new SelectListItem {Text = "July", Value = "7"},
                new SelectListItem {Text = "August", Value = "8"},
                new SelectListItem {Text = "September", Value = "9"},
                new SelectListItem {Text = "October", Value = "10"},
                new SelectListItem {Text = "November", Value = "11"},
                new SelectListItem {Text = "December", Value = "12"},
              }, "Value", "Text");

            model.YearList = new SelectList(new[]
              {
                new SelectListItem {Text = "----  Select ----", Value = null, Selected = true},
                new SelectListItem {Text = "2016", Value = "2016"},
                new SelectListItem {Text = "2017", Value = "2017"},
                new SelectListItem {Text = "2018", Value = "2018"},
                new SelectListItem {Text = "2019", Value = "2019"},
                new SelectListItem {Text = "2020", Value = "2020"},
                new SelectListItem {Text = "2016", Value = "2021"},
                new SelectListItem {Text = "2017", Value = "2022"},
                new SelectListItem {Text = "2018", Value = "2023"},
                new SelectListItem {Text = "2019", Value = "2024"},
                new SelectListItem {Text = "2020", Value = "2025"},
            }, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult AddBonus(BonusModel model)
        {
            if (ModelState.IsValid)
            {
                var firm = LoginUserFirmId();
                var addbonu = new Bonu();
                addbonu.amount = model.Amount;
                addbonu.narration = model.Narration;
                addbonu.bonusMonth = model.BonusMonth;
                addbonu.bouusYear = model.BonusYear;
                addbonu.staffId = model.StaffId;
                addbonu.tranDate = DateTime.Now;
                addbonu.firmId = firm;
                db.Bonus.Add(addbonu);
                db.SaveChanges();
                TempData["notice"] = "success";
                return RedirectToAction("AddBonus");
            }


            model.StaffList = db.Staffs.Where(a => a.isActive == true).ToList().Select(a => new SelectListItem()
            {
                Text = a.firstName + " " + a.middleName + " " + a.lastName,
                Value = a.staffId.ToString()
            });

            model.MonthList = new SelectList(new[]
            {
                new SelectListItem {Text = "----  Select ----", Value = null, Selected = true},
                new SelectListItem {Text = "January", Value = "1"},
                new SelectListItem {Text = "February", Value = "2"},
                new SelectListItem {Text = "March", Value = "3"},
                new SelectListItem {Text = "April", Value = "4"},
                new SelectListItem {Text = "May", Value = "5"},
                new SelectListItem {Text = "June", Value = "6"},
                new SelectListItem {Text = "July", Value = "7"},
                new SelectListItem {Text = "August", Value = "8"},
                new SelectListItem {Text = "September", Value = "9"},
                new SelectListItem {Text = "October", Value = "10"},
                new SelectListItem {Text = "November", Value = "11"},
                new SelectListItem {Text = "December", Value = "12"},
              }, "Value", "Text");

            model.YearList = new SelectList(new[]
              {
                new SelectListItem {Text = "----  Select ----", Value = null, Selected = true},
                new SelectListItem {Text = "2016", Value = "2016"},
                new SelectListItem {Text = "2017", Value = "2017"},
                new SelectListItem {Text = "2018", Value = "2018"},
                new SelectListItem {Text = "2019", Value = "2019"},
                new SelectListItem {Text = "2020", Value = "2020"},
                new SelectListItem {Text = "2016", Value = "2021"},
                new SelectListItem {Text = "2017", Value = "2022"},
                new SelectListItem {Text = "2018", Value = "2023"},
                new SelectListItem {Text = "2019", Value = "2024"},
                new SelectListItem {Text = "2020", Value = "2025"},
            }, "Value", "Text");

            return View(model);
        }

        #endregion
        
    }
}