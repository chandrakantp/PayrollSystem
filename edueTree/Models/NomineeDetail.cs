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
    
    public partial class NomineeDetail
    {
        public int nomineeid { get; set; }
        public string nomineename { get; set; }
        public string nomineeaddress { get; set; }
        public string nomineeralation { get; set; }
        public string nomineestatus { get; set; }
        public Nullable<bool> isdeleted { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Staffid { get; set; }
        public Nullable<int> firmid { get; set; }
        public string age { get; set; }
        public string share { get; set; }
    }
}
