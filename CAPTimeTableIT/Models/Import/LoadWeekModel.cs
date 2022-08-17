using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.TimeTable
{
    public class LoadWeekModel
    {
        public string Thu { get; set; }
        public int? TietBD { get; set; }
        public int? SoTiet { get; set; }
        public int? colspan { get; set; }

        public LoadWeekModel()
        {
            colspan = (TietBD) / (SoTiet);
        }
        
    }
}