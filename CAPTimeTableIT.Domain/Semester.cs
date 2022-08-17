using CAPTimeTableIT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Domain
{
    public class Semester : BaseEntity
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string ListWeek { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }

        public virtual List<CapWeek> CapWeeks { get; set; }
        public virtual List<TimeTable> TimeTables { get; set; }
        public virtual List<Class> Classes { get; set; }

        public Semester()
        {
            StartTime = DateTime.UtcNow;
        }
    }
}
