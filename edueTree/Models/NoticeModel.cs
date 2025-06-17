using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class NoticeModel
    {
        public int noticeId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Narration is required")]
        public string Narration { get; set; }
        [Required(ErrorMessage = "Notice Display Date is required")]
        public Nullable<System.DateTime> NoticeDisplayUpto { get; set; }

        public Nullable<System.DateTime> SystemDatetime { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<int> FirmId { get; set; }

        public virtual Staff Staff { get; set; }
    }
}