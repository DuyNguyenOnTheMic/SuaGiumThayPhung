using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.TimeTableView
{
    public class TimeTableByTeacherViewModel
    {
        public List<int> Sotiet = new List<int> { 1, 4, 7, 10, 13 };
        public List<string> KhungGio = new List<string> { "1-3", "4-6", "7-9", "10-12", "13-15" };
        public List<int> Day = new List<int> { 2, 3, 4, 5, 6, 7 };
        public List<ClassViewModel> ClassViewModels { get; set; }
        public List<CapstoneUser> ApplicationUsers { get; set; }
        public int TotalAssignedClass { get; set; }
        public int TotalClass
        {
            get { return ClassViewModels.Count; }
        }
    }
}