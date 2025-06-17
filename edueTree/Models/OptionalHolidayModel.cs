using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class OptionalHolidayModel
    {
        public OptionalHolidayModel()
        {
            this.OptionalHolidayTrans = new HashSet<OptionalHolidayTran>();
        }
    
        public int optionalId { get; set; }
        [Required(ErrorMessage = "Holiday Name is required")]
        public string optionalHolidayName { get; set; }

        [Required(ErrorMessage = "Holiday date is required")]
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual ICollection<OptionalHolidayTran> OptionalHolidayTrans { get; set; }
    }

    public class LeaveBalanceModel
    {
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public Nullable<decimal> Column1 { get; set; }    
    }
}