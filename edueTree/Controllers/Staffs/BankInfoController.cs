using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Staffs
{
    public class BankInfoController : BaseController
    {
        #region ------ Dbcontext ------
        private dbContainer db =new dbContainer();
        #endregion 

        #region -------- Index --------
        //
        // GET: /BankInfo/

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
            var staffModel = new StaffModel {BankInfolist = db.BankInfoes.Where(a => a.StaffId == staffId && a.IsDeleted==false).ToList()};
            Session["staffid"] = staffId;
            return View(staffModel);
        }
        
        #endregion 

        #region --------- Details -----
        //
        // GET: /BankInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 

        #region --------- Create ------
        //
        // GET: /BankInfo/Create
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Create()
        {
            var staffId = Convert.ToInt32(Session["staffid"].ToString());
            var bankInfoModel = new BankInfoModel { StaffId = staffId };

            return View(bankInfoModel);
        }

        //
        // POST: /BankInfo/Create
        [HttpPost]
        public ActionResult Createpost(BankInfoModel bankInfoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var bankInfo = new BankInfo
                    {
                        AccountNo = bankInfoModel.AccountNo,
                        BankIfsc = bankInfoModel.BankIfsc,
                        BankName = bankInfoModel.BankName,
                        Branch = bankInfoModel.Branch,
                        HolderName = bankInfoModel.HolderName,
                        IsDeleted = false,
                        StaffId = bankInfoModel.StaffId
                    };
                    db.BankInfoes.Add(bankInfo);
                    db.SaveChanges();
                    TempData["notice"] = "success";
                    return RedirectToAction("StaffDashboard", "Staffs", new { staffId = bankInfoModel.StaffId });
                }
                TempData["notice"] = "error";
                return RedirectToAction("StaffDashboard", "Staffs", new { staffId = bankInfoModel.StaffId });
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("StaffDashboard", "Staffs", new { staffId = bankInfoModel.StaffId });
            }
        }

        #endregion 

        #region ---------- Edit -------
        //
        // GET: /BankInfo/Edit/5
        [FilterConfig.EncryptedActionParameterAttribute]
        public ActionResult Edit(int id)
        {
            var staffmodel = new StaffModel();
            var bankDetail = db.BankInfoes.FirstOrDefault(a => a.BankinfoId == id);
            if (bankDetail == null) return View(staffmodel);
            staffmodel.BankInfo = new BankInfoModel
            {
                AccountNo = bankDetail.AccountNo,
                BankIfsc = bankDetail.BankIfsc,
                BankName = bankDetail.BankName,
                BankinfoId = bankDetail.BankinfoId,
                Branch = bankDetail.Branch,
                HolderName = bankDetail.HolderName,
               StaffId = bankDetail.StaffId
            };

            return View(staffmodel);
        }

        //
        // POST: /BankInfo/Edit/5
        [HttpPost]
        public ActionResult Editpost(StaffModel staffModel)
        {
            try
            {
            
              var data = db.BankInfoes.FirstOrDefault(a => a.BankinfoId == staffModel.BankInfo.BankinfoId);
                if (data != null)
                {
                    data.AccountNo = staffModel.BankInfo.AccountNo;
                    data.BankIfsc = staffModel.BankInfo.BankIfsc;
                    data.BankName = staffModel.BankInfo.BankName;
                    data.Branch = staffModel.BankInfo.Branch;
                    data.HolderName = staffModel.BankInfo.HolderName;
               
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["notice"] = "update";
                    return RedirectToAction("StaffDashboard", "Staffs", new { staffId = data.StaffId });
                }

             
            }
            catch
            {
                return View();
            }
            return RedirectToAction("StaffDashboard", "Staffs", new { staffId = staffModel.BankInfo.StaffId });
        }

        #endregion 

        #region --------- Delete ------
        //
        // GET: /BankInfo/Delete/5
        public ActionResult Delete(int id)
        {
            BankInfo bankinfo = db.BankInfoes.FirstOrDefault(a => a.BankinfoId == id);
            if (bankinfo != null)
            {
                bankinfo.BankinfoId = bankinfo.BankinfoId;
                bankinfo.StaffId = bankinfo.StaffId;
            }
            return View(bankinfo);
        }

        //
        // POST: /BankInfo/Delete/5
        [HttpPost]
        public ActionResult Deletepost(BankInfo bankInfoModel)
        {
            try
            {
               //BankInfo bankInfo=new BankInfo();
                var bankinfo = db.BankInfoes.FirstOrDefault(a => a.BankinfoId == bankInfoModel.BankinfoId);
                if (bankinfo != null)
                {
                   // bankinfo.IsDeleted = true;
                   // db.Entry(bankinfo).State = EntityState.Modified;
                    db.BankInfoes.Remove(bankinfo);
                    db.SaveChanges();
                    TempData["notice"] = "delete";
                }
                return RedirectToAction("StaffDashboard", "Staffs", new { staffId = bankInfoModel.StaffId });
            }
            catch
            {
                return RedirectToAction("StaffDashboard", "Staffs", new { staffId = bankInfoModel.StaffId });
            }

        } 
        #endregion 
    }
}
