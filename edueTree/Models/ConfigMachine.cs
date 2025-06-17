using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class ConfigMachine
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Ip Address is required")]
        public string IPAddress { get; set; }
        [Required(ErrorMessage = "Port is Required")]
        public string Port { get; set; }

       
        [Required(ErrorMessage = "Serial Key is Required.")]
        public string SerialKey { get; set; }

        public int? Firmid { get; set; }
        public bool? Isdeleted { get; set; }
        public virtual FirmInfo FirmInfo { get; set; }
    }
}