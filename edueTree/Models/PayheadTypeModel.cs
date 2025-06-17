using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class PayheadTypeModel
    {
     
            public int Id { get; set; }
            [Required(ErrorMessage = "Field Can not be empty.") ]
            public string Name { get; set; }
            public DateTime? CreatedOn { get; set; }
            public bool? IsActive { get; set; }
            public int? Firmid { get; set; }

    }
}