using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Models.Subjects
{
    public class ClassViewModel
    {
        public int SemesterId { get; set; }
        public int ClassId { get; set; }
        public string Thu { get; set; }
        public string MaLHP { get; set; }
        public string Phong { get; set; }
        public string LoaiHP { get; set; }
        public int? TietBD { get; set; }
        public int? SoTiet { get; set; }
        public int? ThuS { get; set; }
        public int? TietS { get; set; }
        public int? TuanBD { get; set; }
        public int? TuanKT { get; set; }
        public string SubjectCode { get; set; }
        public string TeacherId { get; set; }
        public string TeacherEmail { get; set; }
        public string CreatedAssignerId { get; set; }
        public string LastModifiedAssignerId { get; set; }
        public SubjectSummaryModel SubjectInfo { get; set; }
    }
}
