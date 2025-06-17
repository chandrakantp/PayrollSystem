using edueTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace edueTree
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        #region DBContext

        private dbContainer _db = new dbContainer();

        #endregion
        public void DoWork()
        {
        }


        public void Add_attendance(int mid, string idwEnrollNumber, int idwVerifyMode, int idwInOutMode, int idwYear, int idwMonth, int idwDay, int idwHour, int idwMinute, int idwSecond, int idwWorkcode)
        {
            int firmid;
            var enrollNos = idwEnrollNumber;
            var workcode = idwWorkcode;
            var mode = idwVerifyMode;
            var outMode = idwInOutMode;
            var dates = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
            var check = _db.AttendanceStaffs.Count(a =>
                                           a.enrollNumber == enrollNos && a.inOutMode == outMode && a.verifyMode == mode &&
                                           a.worCode == workcode && a.attendDate == dates && a.machineId == mid);
            var attend = new AttendanceStaff
              {

                  enrollNumber = enrollNos,
                  inOutMode = outMode,
                  verifyMode = mode,
                  worCode = workcode,
                  attendDate = dates,
                  firmId = firmid,
                  machineId = mid
              };
            _db.AddToAttendanceStaffs(attend);
            _db.SaveChanges();
        }
    }
}
