using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models.TimeTableView;
using CAPTimeTableIT.Models.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    [Authorize]
    public class PersonalCalendarController : Controller
    {
        private readonly ISemesterService _semesterService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly IUserService _userService;
        private ApplicationUserManager _userManager;
        // GET: PersonalCalendar
        public PersonalCalendarController(ISemesterService semesterService, ApplicationUserManager userManager, IClassService classService, ISubjectService subjectService, IUserService userService)
        {
            _semesterService = semesterService;
            _userManager = userManager;
            _classService = classService;
            _subjectService = subjectService;
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> PersonalCalendar(int id = 0)
        {
            //user id = teacher id
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if (id == 0 && semesters.Count > 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            ViewBag.Semesters = new SelectList(_semesterService.GetAll(), "Id", "Name");
            ViewBag.LatestSemesterId = theLatestSemesterId;
            //validation
            return await GetTimeTableAsync(theLatestSemesterId);
        }

        private async Task<ActionResult> GetTimeTableAsync(int semesterId)
        {
            //todo user id
            var teacherId = User.Identity.GetUserId();
            var timeTableViewModel = new TeacherTimeTableViewModel();

            timeTableViewModel.ClassViewModels = await _subjectService.GetAllClassInWeekByClassIdAndTeacherIdAsync(semesterId, teacherId);
            timeTableViewModel.ClassViewModels = timeTableViewModel.ClassViewModels.OrderBy(t => t.TietS).ThenByDescending(t => t.ThuS).ToList();
            timeTableViewModel.Sotiet = timeTableViewModel.ClassViewModels.Select(t => t.TietS.Value).Distinct().OrderBy(t => t).ToList();
            var subjects = new List<SubjectSummaryModel>();
            //get subject from class
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
            var userRoleModel = new UserRoleViewModel
            {
                UserId = teacherId,
                RoleName = ""
            };
            ViewBag.UserRole = userRoleModel;
            return View(timeTableViewModel);
        }
    }
}