using CAPTimeTableIT.Infrastructure.Models.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.TimeTableView
{
    public class TeacherTimeTableViewModel
    {
        public List<int> Sotiet = new List<int>();
        public List<string> KhungGio = new List<string> { "1-3", "4-6", "7-9", "10-12", "13-15" };
        public List<int> DayOfWeek = new List<int> { 2, 3, 4, 5, 6, 7 };
        public List<ClassViewModel> ClassViewModels { get; set; }
        public List<SubjectSummaryModel> SubjectViewModels { get; set; }        
        public List<string> Semesters { get; set; }
    }
}