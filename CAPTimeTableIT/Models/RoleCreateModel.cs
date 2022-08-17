using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models
{
    public class RoleCreateModel
    {
        [Required]
        [StringLength(256)]
        public string RoleName { get; set; }
    }
}