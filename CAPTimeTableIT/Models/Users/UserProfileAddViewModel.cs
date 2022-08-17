using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Models.Users
{
    public class UserProfileAddViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string StaffCode { get; set; }

    }
}