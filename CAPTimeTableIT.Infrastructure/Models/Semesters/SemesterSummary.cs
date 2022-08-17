using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Models.Semesters
{
    public class SemesterSummary
    {
        public int SemesterId { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string ListWeek { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalSubject { get; set; }
        /// <summary>
        /// means record in classes table, not real classes
        /// </summary>
        public int TotalAssignedClass { get; set; }
    }
}
