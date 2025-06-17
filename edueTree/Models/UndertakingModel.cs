using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Asn1.Crmf;

namespace edueTree.Models
{
    public class UndertakingModel
    {
        public int Underid { get; set; }
        public bool Undertakingcheck { get; set; }
        public bool? IsDeleted { get; set; }
        public int? StaffId { get; set; }
        public int? Firmid { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime? JoinDate { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}