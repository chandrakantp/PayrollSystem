using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web.Mvc;


namespace edueTree.Models
{
    public class StudentModel
    {
        
    
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Roll no is required")]
        public string RollNo { get; set; }

        public string SchoolCode { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

       
        public string Gender { get; set; }
         //[Required(ErrorMessage = "Gender is required")]
        //public string GenderSet
        //{
        //    get
        //    {
        //        if (Gender.Equals(1))
        //            return "Male";
        //        else if (Gender.Equals(2))
        //            return "Female";
        //        else
        //            return "Not defined";
        //    }
        //}

        //[Required(ErrorMessage = "Birth date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]      
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Mother name is required")]
        public string MotherName { get; set; }

        [Required(ErrorMessage = "Father first name is required")]
        public string FatherFirstName { get; set; }
        [Required(ErrorMessage = "Middle name is required")]
        public string FatherMiddleName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string FatherLastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact is required")]
        public string Contact1 { get; set; }
        [Required(ErrorMessage = "Contact is required")]
        public string Contact2 { get; set; }
        [Required(ErrorMessage = "Father occupation is required")]
        public string FatherOccupation { get; set; }
        [Required(ErrorMessage = "Pancard number is required")]
        public string PancardNo { get; set; }
        public string PassportNo { get; set; }
        public string StudentPhoto { get; set; }
        [Required(ErrorMessage = "Emergency contact is required")]
        public string EmergencyContact1 { get; set; }
        [Required(ErrorMessage = "Emergency contact relation is required")]
        public string EmergencyContact1Relation { get; set; }

        public string EmergencyContact2 { get; set; }
        public string EmergencyContact2Relation { get; set; }

        [Required(ErrorMessage = "Admission year is required")]
        public string AddmissionYear { get; set; }

        [Required(ErrorMessage = "Temperary address is required")]
        public string TempAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int TempCityId { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        public string TempPincode { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string TempArea { get; set; }

        [Required(ErrorMessage = "Temperary address is required")]
        public string PerAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int PerCityId { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        public string PerPincode { get; set; }
        [Required(ErrorMessage = "Area is required")]
        public string PerArea { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Blood group is required")]
        public string BloodGroup { get; set; }
        public string BodyMarks { get; set; }

        [Required(ErrorMessage = "Mother tongue is required")]
        public string MotherTongue { get; set; }

        [Required(ErrorMessage = "Cast is required")]
        public int CastId { get; set; }

        [Required(ErrorMessage = "Cast is required")]
        public int SubCastId { get; set; }

        [Required(ErrorMessage = "class and division is required")]
        public int DivisionId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]      
        public DateTime? LeavingDate { get; set; } 
                       
        public string LeavingReason { get; set; }

        public string SiblingBrothers { get; set; }

        public string SiblingSisters { get; set; }
         
        public DateTime CreatedDate { get; set; }                        
        
        public bool MedicalCondition { get; set; }

        public string MedicalDetails { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
     
        public virtual City City { get; set; }
        public virtual City City1 { get; set; }
        
        public IEnumerable<SelectListItem> CastList { get; set; }
        public IEnumerable<SelectListItem> SubCastList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }

        public IEnumerable<SelectListItem> DivisionList { get; set; }

        public virtual ClassDivision  ClassDivision { get; set; }

        public string SibBrotherName { get; set; }
        public string SibSisterName { get; set; }


        public int ClassId { get; set; }
        public int InfoId { get; set; }       
        public string NameOfSchool { get; set; }
        public string PreresonOfLeaving { get; set; }
        public string Precontact { get; set; }
        public string Preschooladdress { get; set; }
        public string Preschoolcity { get; set; }
        public string Prestate { get; set; }
        public string PassingYear { get; set; }
        public string Prepincode { get; set; }
        public IEnumerable<SelectListItem> ClassList { get; set; }

        public string ClassDivName { get; set; }       
    }
}