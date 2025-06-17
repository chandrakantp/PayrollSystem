using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using edueTree.Models;
using Microsoft.AspNet.Identity;

namespace edueTree.Controllers
{
    public class BaseController : Controller
    {
        #region ----------- DBContext ------------
        private readonly dbContainer _db = new dbContainer(); 
        #endregion

        #region ---------- Initialize ------------
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName();
                var userExists = _db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
                if (userExists != null)
                {
                    var permissions = _db.GetMenuUserWise(userExists.Id);
                    ViewBag.Menu = permissions.Select(a => new MenuAggregate()
                    {
                        Action = a.action,
                        Control = a.controller,
                        Id = a.menuItemId,
                        Title = a.itemName,
                        MainMenu = a.MainMenu
                    }).ToList();
                }
            }


        } 
        #endregion

        #region ------- Setting Staff Name -------
        public string SettingStaffName(int? staffId)
        {
            var name = "";
            var staffName = _db.Staffs.FirstOrDefault(a => a.staffId == staffId);
            if (staffName != null)
            {
                name = staffName.firstName + " " + staffName.middleName + " " + staffName.lastName;
            }
            return name;
        } 
        #endregion

        #region ------ Staff Reporting Name ------

        public string StaffReportingName(int reportingId)
        {
            var name = "";
            var staffReportingName = _db.Staffs.FirstOrDefault(a => a.reportingId == reportingId);
            if (staffReportingName != null)
            {
                name = staffReportingName.firstName + " " + staffReportingName.middleName + " " + staffReportingName.lastName;
            }
            return name;
        }

        #endregion

        #region ----------- Get UserId -----------
        public string GetUserId()
        {
            string userName = User.Identity.GetUserName();
            var userExists = _db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            return userExists != null ? userExists.Id : null;
        } 
        #endregion

        #region ------- User profile photo -------

        public string UserProfilePhoto(string userId)
        {
            //var userExists = db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            var photo = "";
            var firstOrDefault = _db.Staffs.FirstOrDefault(a => a.userId == userId);
            if (firstOrDefault != null)
            {
                if (firstOrDefault.passportPhoto == null)
                {
                    photo = "../../fonts/download.jpg";
                }
                else
                {
                    photo = "../../StaffDocument/" + firstOrDefault.passportPhoto;
                }
            }
            return photo;

        }

        #endregion
        
        #region -------- Login Employee Id -------
        public int LoginEmployeeId()
        {
            var userName = User.Identity.GetUserName();
            var userExists = _db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            var staffId = 0;
            var firstOrDefault = _db.Staffs.FirstOrDefault(a => a.userId == userExists.Id);
            if (firstOrDefault != null)
            {
                staffId = firstOrDefault.staffId;
            }
            return staffId;
        }
        #endregion

        #region -------- LoginUserFirmId ---------
        public int LoginUserFirmId()
        {
            try
            {
                string userName = User.Identity.GetUserName();
                var userExists = _db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
                int firmId = 0;

                Staff firstOrDefault = _db.Staffs.FirstOrDefault(a => a.userId == userExists.Id);
                if (firstOrDefault != null)
                {
                    if (firstOrDefault.firmId != null) firmId = (int)firstOrDefault.firmId;
                }
                return firmId;
            }
            catch (Exception)
            {

                return 0;
            }
        } 
        #endregion
        
        #region ------ LoginUserCompanyId --------
        public string LoginUserCompanyId()
        {
            try
            {
                var userName = User.Identity.GetUserName();
                var userExists = _db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
                var companyId = "";
                var firstOrDefault = _db.Staffs.FirstOrDefault(a => a.userId == userExists.Id);
                if (firstOrDefault != null)
                {
                    if (firstOrDefault.CompanyId != null) companyId = firstOrDefault.CompanyId;
                }
                return companyId;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion      
    }
}