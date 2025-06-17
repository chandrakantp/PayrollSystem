using System;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edueTree.Models;
using OfficeOpenXml;
using System.IO;
using System.Web.UI.WebControls;

namespace edueTree.Controllers
{
    public class LeaveUploadController : BaseController
    {
        #region ----------- DBContext -------------
        private dbContainer db = new dbContainer();
        #endregion 

        #region ------------- Upload --------------
        [HttpGet]
        public ActionResult Upload()
        {
            int firmid = LoginUserFirmId();
            var agencyContracts = db.LeavesheetAllEmpFromNewTbl(firmid).GroupBy(
                   x => new
                   {
                       x.fullname,
                       x.LeaveType
                   }).Where(s => s.FirstOrDefault().firmsId == firmid)
                   .Select(
                       g => new TblLeaveRecordModel()
                       {
                           leavetypes = g.FirstOrDefault().LeaveType,
                           totalLeaves =
                               (g.FirstOrDefault().LeaveType == "Compensation Leaves"
                                   ? g.Sum(s => s.totalLeaves) < 0 ? 0 : (g.Sum(s => s.totalLeaves))
                                   : (g.Sum(s => s.totalLeaves))),
                           staffname = g.FirstOrDefault().fullname,
                       }).ToList();
            return View(agencyContracts);
        }

        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            var firm = LoginUserFirmId();
            if (Request != null)
            {

                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        int rowcount = 2;
                        for (int rowIterator = 1; rowIterator <= noOfRow; rowIterator++)
                        {
                            var user = new UsersLeaveSheetModel();
                            user.Leavetype = workSheet.Cells[1, rowcount].Value == null ? null : workSheet.Cells[1, rowcount].Value.ToString();

                            if (user.Leavetype != null)
                            {
                                var data1 =
                                    db.LeaveTypes.FirstOrDefault(s => s.LeaveType1 == user.Leavetype && s.firmId == firm);
                                var ltypeid = data1.leaveTypeId;
                                if (data1 != null)
                                {
                                    int rowcount2 = 1;
                                    for (int rowIterator2 = 2; rowIterator2 <= noOfRow; rowIterator2++)
                                    {
                                        var username = workSheet.Cells[rowIterator2, 1].Value.ToString() ?? string.Empty;
                                        var lb = workSheet.Cells[rowIterator2, rowcount].Value == null
                                            ? Convert.ToString(0)
                                            : workSheet.Cells[rowIterator2, rowcount].Value.ToString();

                                        string[] words = username.Split(' ');
                                        var uniid = words[0];

                                        bool userValid = db.Staffs.Any(t => t.schoolCode == uniid && t.firmId == firm);
                                        if (userValid)
                                        {
                                            var staffdetail = db.Staffs.FirstOrDefault(x => x.schoolCode == uniid && x.firmId == firm).staffId;
                                            var recordid = db.LeaveRecordNews.FirstOrDefault(s => s.LevetypeIds == ltypeid && s.staffids == staffdetail);
                                            decimal valueTwoDecimal = 0;
                                            decimal lbvalue = 0;
                                            decimal value;
                                            if (Decimal.TryParse(lb, out value))
                                            {
                                                lbvalue = Convert.ToDecimal(lb);
                                                valueTwoDecimal = Math.Round(lbvalue, 2);
                                            }
                                            else
                                            {
                                                valueTwoDecimal = 0;
                                            }
                                            LeaveRecordNew tbll = db.LeaveRecordNews.Find(recordid.LeaveRecordId);
                                            {
                                                tbll.staffids = staffdetail;
                                                tbll.totalLeaves = valueTwoDecimal;
                                                tbll.CreatedDate = DateTime.UtcNow;
                                                tbll.LevetypeIds = ltypeid;
                                                tbll.firmsId = firm;
                                                tbll.IsActiveLeave = true;
                                            }
                                            db.LeaveRecordNews.AddOrUpdate(tbll);
                                            db.SaveChanges();
                                            rowcount2++;
                                        }
                                    }
                                }
                                rowcount++;
                            }
                        }
                    }
                    TempData["notice"] = "success";
                    return RedirectToAction("Upload", "LeaveUpload");
                }
            }
            return RedirectToAction("Upload", "LeaveUpload");
        }
        #endregion

        #region ---- AttendanceDataUpload ---------
        [HttpGet]
        public ActionResult AttendanceDataUpload()
        {
            return View();
        }
        public string ourPath;
        [HttpPost]
        public ActionResult AttendanceDataUpload(FormCollection formCollection)
        {
            HttpPostedFileBase fileee = Request.Files["UploadedFile"];
            var streamfile = new StreamReader(fileee.InputStream);

            int counter = 0;
            var firm = LoginUserFirmId();
            while ((line = (streamfile.ReadLine())) != null)
            {
                string[] fields = line.Trim().Split('\t');
                string enrollno = fields[0];
                string attendDate = fields[1];
                string veryMode = fields[2];
                string inoutmode = fields[3];

                if (!string.IsNullOrEmpty(enrollno) && !string.IsNullOrEmpty(attendDate) && !string.IsNullOrEmpty(veryMode) && !string.IsNullOrEmpty(inoutmode))
                {
                    var memberFingure =
                        db.MemberattendaceConfigs.AsEnumerable().FirstOrDefault(s => s.FingureId == int.Parse(enrollno) && s.FirmId == firm);
                    if (memberFingure != null)
                    {
                        var checkalreadyexists =

                            db.AttendanceStaffs.AsEnumerable().Count(
                                s =>
                                    s.StaffId == memberFingure.StaffId && s.enrollNumber == enrollno && s.firmId == firm &&
                                    s.attendDate == Convert.ToDateTime(attendDate) &&
                                    s.inOutMode == int.Parse(inoutmode) && s.verifyMode == int.Parse(veryMode));
                        if (checkalreadyexists == 0)
                        {
                            var atdata = new AttendanceStaff
                            {
                                enrollNumber = enrollno,
                                attendDate = Convert.ToDateTime(attendDate),
                                verifyMode = Convert.ToInt32(veryMode),
                                inOutMode = Convert.ToInt32(inoutmode),
                                worCode = 0,
                                firmId = firm,
                                AttendMode = "MannualyUpload",
                                StaffId = memberFingure.StaffId,
                            };
                            db.AttendanceStaffs.Add(atdata);
                            db.SaveChanges();
                        }
                    }
                }
            }
            TempData["notice"] = "success";
            return RedirectToAction("AttendanceDataUpload");
        } 

        public string line { get; set; }
        #endregion
    }
}


public class UsersLeaveSheetModel
{
    public string Leavetype { get; set; }
}

