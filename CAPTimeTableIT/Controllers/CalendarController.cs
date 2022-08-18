using CAPTimeTableIT.Common;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models.TimeTableView;
using CAPTimeTableIT.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ISemesterService _semesterService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly IUserService _userService;
        private ApplicationUserManager _userManager;

        public CalendarController(ApplicationUserManager userManager, ISemesterService semesterService, IClassService classService, ISubjectService subjectService, IUserService userService)
        {
            _semesterService = semesterService;
            _userManager = userManager;
            _classService = classService;
            _subjectService = subjectService;
            _userService = userService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Calendar(int id = 0)
        {
            //user id = teacher id
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if (id == 0 && semesters.Count > 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            ViewBag.Semesters = new SelectList(semesters, "Id", "Name");
            ViewBag.LatestSemesterId = theLatestSemesterId;
            //validation
            return await CalendarBySemester(theLatestSemesterId);
        }

        public async Task<ActionResult> Teacher(int id = 0)
        {
            //user id = teacher id
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if (id == 0 && semesters.Count > 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            ViewBag.Semesters = new SelectList(semesters, "Id", "Name");
            ViewBag.LatestSemesterId = theLatestSemesterId;
            return await GetTeacherCalendarBySemester(theLatestSemesterId);
        }

        private async Task<ActionResult> CalendarBySemester(int semesterId)
        {
            //user id
            var email = User.Identity.Name;
            var currentUser = _userManager.FindByEmail(email);
            var role = _userManager.GetRoles(currentUser.Id);

            var isBcnKhoa = role[0] == CapstoneSystem.BCNKhoaRole;

            //get all users()
            var allUsers = await _userService.GetAllAsync();
            var timeTableViewModel = new AssignTimeTableViewModel
            {
                IsBcnKhoa = isBcnKhoa,
                ApplicationUsers = allUsers, //dropdown list
                CurrentUserId = currentUser.Id,
            };

            timeTableViewModel.ClassViewModels = await _subjectService.GetAllClassInWeekAsync(semesterId);
            var subjects = new List<SubjectSummaryModel>();
            foreach (var item in timeTableViewModel.ClassViewModels)
            {
                if (!subjects.Any(t => t.Id == item.SubjectInfo.Id))
                {
                    subjects.Add(item.SubjectInfo);
                }
            }
            //var subjects = timeTableViewModel.ClassViewModels.Select(t => t.SubjectInfo).Distinct().OrderBy(t=>t.Name).ToList();
            subjects = subjects.OrderBy(t => t.Name).ToList();
            timeTableViewModel.SubjectViewModels = subjects;// await _subjectService.GetAllSubjectSummaryAsync();
            return View(timeTableViewModel);

        }

        private async Task<ActionResult> GetTeacherCalendarBySemester(int semesterId)
        {
            //get users that has class in the semester
            var allUsers = await _userService.GetAllAsync();//(semesterId);
            allUsers = allUsers.OrderBy(t=>t.UserName).ToList();
            var timeTableViewModel = new TimeTableByTeacherViewModel
            {
                ApplicationUsers = allUsers
            };

            timeTableViewModel.ClassViewModels = await _subjectService.GetAllClassInWeekAsync(semesterId);
            var subjects = new List<SubjectSummaryModel>();
            foreach (var item in timeTableViewModel.ClassViewModels)
            {
                if (!subjects.Any(t => t.Id == item.SubjectInfo.Id))
                {
                    subjects.Add(item.SubjectInfo);
                }
            }
            return View(timeTableViewModel);

        }
    }
}