using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Domain
{
    public class CapWeek : BaseEntity
    {
        public DateTime Weekday;
        public virtual Semester Semester { get; set; }
        public virtual List<CapDay> CapDays { get; set; }
    }
}
