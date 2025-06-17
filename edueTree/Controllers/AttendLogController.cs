using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using edueTree.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace edueTree.Controllers
{

    public class AttendLogController : BaseController
    {
        #region ----------- DBContext -----------
        private dbContainer db = new dbContainer();
        #endregion

        #region --------- UserManager -----------
        public AttendLogController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AttendLogController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;

        }


        public UserManager<ApplicationUser> UserManager { get; private set; }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        #endregion

        #region ------------ MobileLog ----------
        [AllowAnonymous]
        public async Task<JsonResult> MobileLog(string userid, string password)
        {
            try
            {
                var ststate = db.Staffs.FirstOrDefault(s => s.schoolCode == userid);
                var ep = db.EditPermissions.Where(s => s.StaffId == ststate.staffId).FirstOrDefault().AppAttendance;
                if (ep == true)
                {
                    var user = await UserManager.FindAsync(userid, password);
                    if (user != null)
                    {
                        var fs = db.AspNetUsers.FirstOrDefault(a => a.UserName == userid);
                        var userId = fs.Id;

                        if (db.Staffs.Any(u => u.userId == userId && u.isActive == true && u.FirmInfo.isActive == true))
                        {
                            var stname = db.Staffs.Where(s => s.userId == userId).FirstOrDefault();
                            return Json(JResponseNestedComon("100", userid, stname.firstName + " " + stname.lastName), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(JResponseNestedComon("104", "user not active", null), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(JResponseNestedComon("101", "Check UserName Or Password.", "Unsuccessful"),
                               JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(JResponseNestedComon("106", "You don't have Permission to Apply Attendance.", "Unsuccessful"),
                           JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception )
            {
                return Json(JResponseNestedComon("103", "Error", null), JsonRequestBehavior.AllowGet);
            }           
        }
#endregion

        #region ---------- AppDataCheckStatus ---
        public JsonResult AppDataCheckStatus(string userId, string checkStatus, string datetime, string latitude, string longitude)
        {
            try
            {
                using (dbContainer db = new dbContainer())
                {

                    var ststate = db.Staffs.FirstOrDefault(s => s.schoolCode == userId);
                    var ep = db.EditPermissions.FirstOrDefault(s => s.StaffId == ststate.staffId).AppAttendance;
                    if (ep == true)
                    {
                        var attetend = new AttendanceStaff();
                        attetend.enrollNumber = "";
                        attetend.attendDate = Convert.ToDateTime(datetime);
                        attetend.verifyMode = 5;
                        attetend.inOutMode = Convert.ToInt32(checkStatus);
                        attetend.worCode = 0;
                        attetend.attendTime = null;
                        attetend.isManuallyCheckOut = true;
                        attetend.firmId = ststate.firmId;
                        attetend.AttendMode = "AppData";
                        attetend.StaffId = ststate.staffId;
                        attetend.latitude = latitude;
                        attetend.longitude = longitude;

                        db.AttendanceStaffs.Add(attetend);
                        TempData["notice"] = "success";
                        db.SaveChanges();

                        return Json(JResponseNestedComon("100", "Successfully", "DataInserted"),
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(JResponseNestedComon("106", "You don't have Permission to Apply Attendance.", "Unsuccessful"),
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(JResponseNestedComon("103", "Error", null), JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResponseNestedComon JResponseNestedComon(string code, string discription, string inJson)
        {
            JsonResponseNestedComon jr = new JsonResponseNestedComon
            {
                StatusCode = code,
                StatusDescription = discription,
                ResponseResult = inJson
            };
            return jr;
        }

        #endregion
    }
}
public class JsonResponseNestedComon
{
    public string StatusCode { get; set; }
    public string StatusDescription { get; set; }
    public string ResponseResult { get; set; }
}