//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace edueTree.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkExperiance
    {
        public int previousId { get; set; }
        public string organizationName { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> releaseDate { get; set; }
        public string designation { get; set; }
        public string attachedCertificate { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual Staff Staff { get; set; }
    }
}
