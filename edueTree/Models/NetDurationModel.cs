using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class NetDurationModel
    {
        public DateTime? Trandate { get; set; }
        public string UserNames { get; set; }
        public string TotalDuration { get; set; }
        public string LoginType { get; set; }
        public int? Staffid { get; set; }
        public int? FirmId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TimeSpan? TransactionTime { get; set; }
        public string Staffname { get; set; }
        public TimeSpan? TotalnetDuration { get; set; }
    }
}