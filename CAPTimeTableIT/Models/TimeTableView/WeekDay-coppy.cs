using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.TimeTableView
{
    public class WeekDay
    {
        public int Days { get; set; }
        public List<Tiet> Tiets { get; set; }

        public WeekDay()
        {
            int[] days = { 2, 3, 4, 5, 6, 7};

            for (int i = 0; i < days.Length; i++)
            {
                Days += days[i] * Convert.ToInt32(Math.Pow(10, days.Length - i - 1));
            }
        }
    }
}