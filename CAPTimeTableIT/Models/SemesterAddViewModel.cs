using CAPTimeTableIT.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Models
{
    public class SemesterAddViewModel
    {        
        public int Id { get; set; }
        [Required]
        public int StartYear { get; set; }

        [Required]
        public int EndYear { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn tuần bắt đầu")]
        //[StringLength(2)]
        [Range(1, 52, ErrorMessage = "Nhập tuần từ 1 đến 52")]
        public string ListWeek { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập học kỳ")]
        [Range(100, 999, ErrorMessage = "Nhập số từ 100 đến 999")]
        [Remote("ValidateNewSemester", "Semester", ErrorMessage = "Học kỳ bị trùng tên")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Bắt buộc nhập!"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        public DateTime StartTime { get; set; }

        //[DataType(DataType.Date)] [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public SemesterAddViewModel()
        {
            StartTime = DateTime.UtcNow;
        }

    }
}