using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.Asssign
{
    public class AssignTeacherEmailTemplate: MailInfo
    {
        public string MaLhp { get; set; }
        public string Subject { get; set; }
        public string FullName { get; set; }
        public string Tiet { get; set; }
        public string Thu { get; set; }
        public string Assigner { get; set; }
    }
}