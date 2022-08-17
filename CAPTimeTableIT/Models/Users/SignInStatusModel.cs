using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.Models
{
    public class SignInStatusModel
    {
        public SignInStatus SignInStatus { get; set; }
        public string RoleName { get; set; }
    }
}