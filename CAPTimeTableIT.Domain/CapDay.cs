using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Domain
{
    public class CapDay : BaseEntity
    {
        public DateTime DayOfWeek { get; set; }

        public virtual CapWeek CapWeek { get; set; }
    }
}
