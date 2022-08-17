using CAPTimeTableIT.Common;
using CAPTimeTableIT.Infrastructure;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using CAPTimeTableIT.Models.Home;
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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ISemesterService _semesterService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly IUserService _userService;
        private ApplicationUserManager _userManager;
        public HomeController(ISemesterService semesterService, ApplicationUserManager userManager, IClassService classService, ISubjectService subjectService, IUserService userService)
        {
            _semesterService = semesterService;
            _userManager = userManager;
            _classService = classService;
            _subjectService = subjectService;
            _userService = userService;
        }
        public async Task<ActionResult> Index()
        {
            var timeTableViewModel = new TimeTableViewModel();

            //var classModel = new TimeTableViewModel();
            timeTableViewModel.ClassViewModels = await _subjectService.GetAllClassInWeekAsync(35);
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
        [Authorize(Roles = "BCN Khoa,Giảng viên,Bộ môn")]
       /* public async Task<ActionResult> Calendar()
        {
            //todo user id
            var currentUserId = User.Identity.GetUserId();
            var createdAssignerId = User.Identity.GetUserId();

            var roles = _userManager.GetRoles(User.Identity.GetUserId());
            var hasPermission = roles.Any(t => t == CapstoneSystem.SubjectRole || t == CapstoneSystem.BCNKhoaRole);
            var isBcnKhoa = roles[0] == CapstoneSystem.BCNKhoaRole;

            //todo get all users()
            var allUsers = await _userService.GetAllAsync();
            var timeTableViewModel = new AssignTimeTableViewModel
            {
                IsBcnKhoa = isBcnKhoa,
                ApplicationUsers = allUsers, //dropdown list
                CurrentUserId = currentUserId,
            };
            if (string.IsNullOrEmpty(createdAssignerId))
            {
                createdAssignerId = currentUserId;
            }

            timeTableViewModel.ClassViewModels = await _subjectService.GetAllClassInWeekAsync(35);
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

        }*/
       

        public ActionResult ImportHistory()
        {

            return View();
        }

       /* public async Task<ActionResult> PersonalCalendar(int id = 0)
        {
            //user id = teacher id
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if (id == 0)
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
            timeTableViewModel.ClassViewModels = timeTableViewModel.ClassViewModels.OrderBy(t => t.TietS).ThenByDescending(t=>t.ThuS).ToList();
            timeTableViewModel.Sotiet = timeTableViewModel.ClassViewModels.Select(t=>t.TietS.Value).Distinct().OrderBy(t=>t).ToList();
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
        }*/
    }
}