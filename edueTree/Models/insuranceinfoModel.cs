using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public partial class InsuranceinfoModel
    {
        public int Insid { get; set; }
        [Required(ErrorMessage = "Policy No is required")]
        public string Policyno { get; set; }
        [Required(ErrorMessage = "Expiry date is required")]
        public DateTime? Expirydate { get; set; }

        public bool? IsDeleted { get; set; }
        public int? staffid { get; set; }
        public int Firmid { get; set; }
        [Required(ErrorMessage = "Policydate is required")]
        public DateTime? Policydate { get; set; }
        public string Name { get; set; }
        public string Staffcode { get; set; }


        public string Nomineename { get; set; }
        public string Nomineeaddress { get; set; }
        public string Nomineeralation { get; set; }
        public string Nomineestatus { get; set; }
      
        public DateTime? Dob { get; set; }
        public DateTime? CreatedDate { get; set; }
      
        public string Age { get; set; }
        public string Share { get; set; }


    }
}