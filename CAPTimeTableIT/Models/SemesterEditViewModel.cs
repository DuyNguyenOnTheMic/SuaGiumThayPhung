using CAPTimeTableIT.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Models
{
    public class SemesterEditViewModel
    {
        public int Id { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập Tuần bắt đầu")]
        [Range(1, 52, ErrorMessage = "Nhập tuần từ 1 đến 52")]
        public string ListWeek { get; set; }
        public string Name { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        public DateTime StartTime { get; set; }
    }
}