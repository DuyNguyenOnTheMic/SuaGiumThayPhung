using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.TimeTableView
{
    public class TimeTableViewModel
    {
        /*public List<Tiet> Tiets { get; set;}*/
        public List<int> sotiet = new List<int> { 1, 4, 7, 10, 13 };
        public List<string> khungGio = new List<string> { "1-3","4-6","7-9","10-12","13-15" };
        public List<int> day = new List<int> { 2, 3, 4, 5, 6, 7 };
        /*public List<WeekDay> WeekDays { get; set; }*/
        public List<ClassViewModel> ClassViewModels { get; set; }
        public List<SubjectSummaryModel> SubjectViewModels { get; set; }
    }
    /*public class Tiet
    {
        public int SoTiet { get; set; }*/


    /*    public Tiet()
        {
            int[] sotiet = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            for (int i = 0; i < sotiet.Length; i++)
            {
                SoTiet += sotiet[i] * Convert.ToInt32(Math.Pow(10, sotiet.Length - i - 1));
            }
        }
    }*/

    /*public class WeekDay
    {
        public int Days { get; set; }
        public List<Tiet> Tiets { get; set; }

        public WeekDay()
        {
            int[] day = { 2, 3, 4, 5, 6, 7};
            for (int i = 0; i < day.Length; i++)
            {
                Days += day[i] * Convert.ToInt32(Math.Pow(10, day.Length - i - 1));
            }
        }
    }*/
}