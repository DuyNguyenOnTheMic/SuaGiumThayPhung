using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.Subject
{
    public class SubjectPostModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Môn học không được bỏ trống")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubjectCode { get; set; }
    }
}