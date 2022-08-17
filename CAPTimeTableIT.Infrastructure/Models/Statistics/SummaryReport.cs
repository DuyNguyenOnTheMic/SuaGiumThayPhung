using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Models.Statistics
{
    public class SummaryReport
    {
        public int SemesterId { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int TotalTeacher { get; set; }
        public int TotalHour { get; set; }
        public int TotalTietHoc { get; set; }
        public List<string> Semesters { get; set; }
    }
}
