using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class MonthlyTTargetModel
    {
        public int trgId { get; set; }
        public int? trgValue { get; set; }
        public int? trgMonth { get; set; }
        public int? trgYear { get; set; }
        public int firmid { get; set; }
        public string deptName { get; set; }

        public virtual FirmInfo FirmInfo { get; set; }
    }
}