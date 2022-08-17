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
    public class AssignTimeTableViewModel
    {
        public List<int> sotiet = new List<int> { 1, 4, 7, 10, 13 };
        public List<string> khungGio = new List<string> { "1-3", "4-6", "7-9", "10-12", "13-15" };
        public List<int> day = new List<int> { 2, 3, 4, 5, 6, 7 };
        /*public List<WeekDay> WeekDays { get; set; }*/
        public List<ClassViewModel> ClassViewModels { get; set; }
        public List<SubjectSummaryModel> SubjectViewModels { get; set; }
        /// <summary>
        /// 1 user - 1 role
        /// </summary>
        public bool IsBcnKhoa { get; set; }
        public string CurrentUserId { get; set; }
        public List<CapstoneUser> ApplicationUsers { get; set; }
        public int TotalAssignedClass { get; set; }
        public int TotalClass
        {
            get { return ClassViewModels.Count; }
        }
    }
}