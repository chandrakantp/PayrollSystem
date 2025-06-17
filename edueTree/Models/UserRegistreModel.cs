using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    //public class UserRegistreModel
    //{
    //    [Required]
    //    [Display(Name = "Full Name")]
    //    public string FirstName { get; set; }

    //    [Required]
    //    //[Display(Name = "Middle name")]
    //    public string MiddleName { get; set; }

    //    [Required]
    //   // [Display(Name = "Last name")]
    //    public string LastName { get; set; }

    //    [Required(ErrorMessage = "Email address is required.")]
    //    [EmailAddress(ErrorMessage = "Please enter valid email id.")]
    //    public string Email { get; set; }

    //    [Required]
    //    [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric.")]
    //    [Display(Name = "Contact")]
    //    public string Contact { get; set; }

    //    [Required]
    //    [Display(Name = "Firm Name ")]
    //    public string FirmName { get; set; }

    //     [Required]
    //    [Display(Name = "No Of Employee ")]
    //    public int? NoOfEmployees { get; set; }


    //     [Required]
    //    [Display(Name = "Amount ")]
    //    public decimal? Amount { get; set; }


    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm Password")]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
      
       
    //    public string CompanyId { get; set; }
    //}

    public class UserRegistreModel
    {
        //public string currency { get; set; }
        public string package { get; set; }
        public string price { get; set; }
        public string ProductInfo { get; set; }
        public string Payment { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FirstName { get; set; }

        //[Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        // [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter valid email id.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric.")]
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        [Required]
        [Display(Name = "Firm Name ")]
        public string FirmName { get; set; }

        [Required(ErrorMessage = "No of Employees is required.")]
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No of Employees must be numeric.")]
        [Display(Name = "No of Employees")]
        public string NoOfEmployees { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Amount must be numeric.")]
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public string CompanyId { get; set; }
    }
}