using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers
{
    public class NoAssignController : BaseController
    {
        #region  ------------ DBContext -----------------
        private dbContainer db = new dbContainer();
        #endregion 

        #region ------ MachineConfigeNotAssignEmpList --
        public ActionResult MachineConfigeNotAssignEmpList()
        {
            var firm = LoginUserFirmId();

            var data = db.NotAssignMemberMachineConfige(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);            
        }
        #endregion

        #region -------- MemberNotAssignShiftList ------
        public ActionResult MemberNotAssignShiftList()
        {
            var firm = LoginUserFirmId();

            var data = db.NotAssignMemberShift(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion

        #region --- MemberNotAssignNetUsernameList -----
        public ActionResult MemberNotAssignNetUsernameList()
        {
            var firm = LoginUserFirmId();

            var data = db.NotAssignUserName(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion

        #region ------ MemberNotSubmitDocumentList -----
        public ActionResult MemberNotSubmitDocumentList()
        {
            var firm = LoginUserFirmId();

            var data = db.NotStaffSubmitDocument(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion

        #region ----- MemberNotAssignEmailReporting ----
        public ActionResult MemberNotAssignEmailReporting()
        {
            var firm = LoginUserFirmId();

            var data = db.NotAssignEmailReporting(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion

        #region ----  NotAssignFormToEmployee ----------
        public ActionResult NotAssignFormToEmployee()
        {
            var firm = LoginUserFirmId();

            var data = db.NotAssignFormToEmp(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                FormName = a.Title
            }).ToList();
            return View(assignshiftmodel);
        }
#endregion

        #region --- NotAssignEvaluatorToEmployee -------
        public ActionResult NotAssignEvaluatorToEmployee()
        {
            var firm = LoginUserFirmId();

            var data = db.NotAssignEvaluatorToEmp(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion

        #region ---- NotAssignEsiInfoToEmployee --------
        public ActionResult NotAssignEsiInfoToEmployee()
        {
            var firm = LoginUserFirmId();

            var data = db.NotFillUpEsiInfoToStaff(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.staffCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
#endregion

        #region ----- NotAssignInsuranceInfoToEmp ------
        public ActionResult NotAssignInsuranceInfoToEmployee()
        {
            var firm = LoginUserFirmId();

            var data = db.NotFillUpInsuRanceInfoToStaff(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.schoolCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
#endregion

        #region ---- NotAssignEpfInfoToEmployee --------
        public ActionResult NotAssignEpfInfoToEmployee()
        {
            var firm = LoginUserFirmId();

            var data = db.NotFillEPFDetailsToStaff(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.schoolCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
#endregion

        #region -- NotFillVisaPassportDetailToStaff ----
        public ActionResult NotFillVisaPassportDetailToStaff()
        {
            var firm = LoginUserFirmId();

            var data = db.NotFillVisaPassportToStaff(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.schoolCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion  

        #region ------ NotFillBankInfoToStaff ----------
        public ActionResult NotFillBankInfoToStaff()
        {
            var firm = LoginUserFirmId();

            var data = db.NotFillBankInfoToStaff(firm);
            var assignshiftmodel = data.Select(a => new NotAssignMemberMachineModel()
            {
                Fullname = a.schoolCode + " : " + a.firstName + " " + a.middleName + " " + a.lastName
            }).ToList();
            return View(assignshiftmodel);
        }
        #endregion

    }
}
