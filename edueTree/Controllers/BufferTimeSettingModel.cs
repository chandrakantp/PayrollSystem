using System;
using System.ComponentModel.DataAnnotations;

namespace edueTree.Models
{
    public partial class BufferTimeSettingModel
    {
        public int BufferId { get; set; }
        [Required(ErrorMessage = "Set Buffer Time field is required.")]
        public Nullable<int> EarlyIn { get; set; }
        public Nullable<int> EarlyOut { get; set; }
        public Nullable<int> LateIn { get; set; }
        public Nullable<int> LateOut { get; set; }
        public Nullable<int> firmid { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public Nullable<int> CountNotnull { get; set; }
    }
}
