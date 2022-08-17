using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.Asssign
{
    public class AssignTeacherModel
    {
        public int ClassId { get; set; }
        public string TeacherId { get; set; }
        public string TeacherEmail { get; set; }
    }
}