using System;
using System.ComponentModel.DataAnnotations;

namespace edueTree.Models
{
    public class VisaPassportModel
    {
        public int PassVisaId { get; set; }
        [Required(ErrorMessage = "Passport Number is Required")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "This Field must be numeric")]
        public string Num { get; set; }
        [Required(ErrorMessage = "Country is Required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Passport Expiry  Date is Required")]
        public DateTime? ExpiryDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? StaffId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string Type { get; set; }

        public string Nomineename { get; set; }
        public string Nomineeaddress { get; set; }
        public string Nomineeralation { get; set; }
        public string Nomineestatus { get; set; }
      
        public DateTime? Dob { get; set; }

       
        public string Age { get; set; }
        public string Share { get; set; }
        public string StaffCode { get; set; }
        public string Staffname { get; set; }
    }
}