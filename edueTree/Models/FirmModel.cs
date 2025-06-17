using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class FirmModel
    {
        public int FirmId { get; set; }
        [Required(ErrorMessage = "Firm name is required")]
        public string FirmName { get; set; }

        public string Logo { get; set; }

        [Required(ErrorMessage = "Flat no is required")]
        public string FlatNo { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string Area { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pincode must be numeric")]
        public string Pincode { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        
        public string Contact { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        public string Fax { get; set; }
              
        public string Website { get; set; }
    }

    public partial class FirmInfo
    {
        public string ipAddress { get; set; }
    }

}