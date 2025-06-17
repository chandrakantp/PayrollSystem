using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class BankInfoModel
    {
        public int BankinfoId { get; set; }
        [Required(ErrorMessage = "Bank Name is Required")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Bank Ifsc is Required")]
        public string BankIfsc { get; set; }
        [Required(ErrorMessage = "Bank Acount No is Required")]
        public string AccountNo { get; set; }
        [Required(ErrorMessage = "Bank Account Holder is Required")]
        public string HolderName { get; set; }
        [Required(ErrorMessage = "Bank Branch is Required")]
        public string Branch { get; set; }
        public string StaffName { get; set; }
        public string StaffCode { get; set; }
        public bool? IsDeleted { get; set; }
        public int? StaffId { get; set; }
    }
}