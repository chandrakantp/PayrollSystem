using System;
using System.ComponentModel.DataAnnotations;

namespace edueTree.Models
{
    public class EPFModel
    {
        public int EpFid { get; set; }

        public string Name { get; set; }

        public string perAddress { get; set; }

        public DateTime? dob { get; set; }

        public Nullable<int> gender { get; set; }
        public bool? marriedstatus { get; set; }
        public string perpincode { get; set; }
        public string perarea { get; set; }
        public string tempAddress { get; set; }
        public string tempArea { get; set; }
        public string temppin { get; set; }
        public DateTime? doj { get; set; }
        [Required(ErrorMessage = "EPF Account no is Required")]
        [StringLength(40, ErrorMessage = "EPFAccountNo cannot be longer than 40 characters.")]
        public string EPFAccountNo { get; set; }
        public string fathername { get; set; }
        public string UANNo { get; set; }
        public int staffid { get; set; }
        public string staffcode { get; set; }
        public int FirmId { get; set; }

        public DateTime? JOD { get; set; }
        public string Nomineename { get; set; }
        public string Nomineeaddress { get; set; }
        public string Nomineeralation { get; set; }
        public string Nomineestatus { get; set; }
        public bool? Isdeleted { get; set; }
       
        public DateTime? CreatedDate { get; set; }
       
        public string Age { get; set; }
        public string Share { get; set; }
    }
}