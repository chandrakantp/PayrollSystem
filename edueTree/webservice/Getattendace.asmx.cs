using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Services;
using edueTree.Models;

namespace edueTree.webservice
{
    /// <summary>
    /// Summary description for Getattendace
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Getattendace : WebService
    {

        #region Dbcontext
        private dbContainer _db = new dbContainer();
        #endregion

        #region GetAttendace

        /// <summary>
        /// This webserice is responsible for Fill Attendace using attendace machine
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="idwEnrollNumber"></param>
        /// <param name="idwVerifyMode"></param>
        /// <param name="idwInOutMode"></param>
        /// <param name="idwYear"></param>
        /// <param name="idwMonth"></param>
        /// <param name="idwDay"></param>
        /// <param name="idwHour"></param>
        /// <param name="idwMinute"></param>
        /// <param name="idwSecond"></param>
        /// <param name="idwWorkcode"></param>
        /// <param name="firmId"></param>
        /// <returns></returns>
        [WebMethod]
        public string AddAttendanceusingmachindata(int mid, string idwEnrollNumber, int idwVerifyMode, int idwInOutMode,int idwYear, int idwMonth, int idwDay, int idwHour, int idwMinute, int idwSecond, int idwWorkcode,int firmId)
        {
            string c = "0";

            if (mid == null)
            {
                return c;
            }
            else
            {                
                try
                {
                    var dates = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);               
                    var check = _db.AttendanceStaffs.Count(a =>
                        a.enrollNumber == idwEnrollNumber && a.inOutMode == idwInOutMode &&
                        a.verifyMode == idwVerifyMode &&
                        a.worCode == idwWorkcode && a.attendDate == dates && a.firmId == firmId);
                    if (check == 0)
                    {
                        var enroll = Convert.ToInt32(idwEnrollNumber);
                        var data =
                            _db.MemberattendaceConfigs.FirstOrDefault(
                                a => a.FingureId == enroll && a.FirmId == firmId &&
                                    a.IsDeleted == false);
                        if (data != null)
                        {
                            var attend = new AttendanceStaff
                            {
                                enrollNumber = idwEnrollNumber,
                                inOutMode = idwInOutMode,
                                verifyMode = idwVerifyMode,
                                worCode = idwWorkcode,
                                attendDate = dates,
                                firmId = firmId,
                                StaffId = data.StaffId,
                                AttendMode = "Machine",                                
                            };
                            _db.AttendanceStaffs.Add(attend);
                            _db.SaveChanges();
                            c = "1";
                        }

                        return c;
                    }
                }
                catch (Exception)
                {
                    return c;

                }
                return c;
            }
        }

        #endregion

        #region Get Record if serial-key is exist
     
        [WebMethod]
        public string GetSerialKeyisExistlist(string serialkey)
        {            
            try
            {
                var machineconfigList = (_db.MachineConfigures.Where(a => a.SerialKey == serialkey)).First();
                string abc = machineconfigList.ToString();
                return abc;

            }
            catch (Exception)
            {

                return null;
            }
       
        }
        #endregion

    }
}
