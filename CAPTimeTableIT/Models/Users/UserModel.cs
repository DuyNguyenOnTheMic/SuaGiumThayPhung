using CAPTimeTableIT.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Models.Users
{
    public class UserModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage="Tên giảng viên không được bỏ trống")]
        public string FullName { get; set; }
        [Remote("ValidateNewUserProfile", "UserProfile", AdditionalFields = "Id", ErrorMessage = "Mã CBGD bị trùng")]
        [RegularExpression("[0-9a-zA-Z]+", ErrorMessage="Mã CBGD không hợp lệ")]
        public string StaffCode { get; set; }
    }
}