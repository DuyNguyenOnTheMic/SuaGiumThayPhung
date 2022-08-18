using CAPTimeTableIT.Common;
using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using CAPTimeTableIT.Models.Asssign;
using CAPTimeTableIT.Models.Home;
using CAPTimeTableIT.Models.TimeTableView;
using CAPTimeTableIT.Models.Users;
using CAPTimeTableIT.Infrastructure.Models;
using ExcelDataReader;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    [Authorize]
    public class ImportController : BaseController
    {
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly IUserService _userService;
        private readonly ISemesterService _semesterService;
        private readonly IExcelService _excelService;
        private readonly ICapstoneEmailService _capstoneEmailService;
        private readonly IUserProfileService _userProfileService;
        private ApplicationUserManager _userManager;
        //private readonly DbSet<Class> _classes;
        //private readonly DbSet<Subject> _subjects;
        public ImportController(ApplicationUserManager userManager, ISemesterService semesterService, IClassService classService, ISubjectService subjectService, IUserService userService,
            IExcelService excelService, ICapstoneEmailService capstoneEmailService, IUserProfileService userProfileService)
        {
            _semesterService = semesterService;
            _userManager = userManager;
            _classService = classService;
            _subjectService = subjectService;
            _userService = userService;
            _excelService = excelService;
            _capstoneEmailService = capstoneEmailService;
            _userProfileService = userProfileService;
        }
        [HandleError]
        public ActionResult Import()
        {
            ViewBag.Message = "Your contact page.";
            /*get semester list=return model*/
            ViewBag.Semesters = new SelectList(_semesterService.GetAll(), "Id", "Name");
            ViewBag.ErrorMessages = TempData["ErrorMessages"];
            return View(new ImportViewModel());
            //return RedirectToAction("AssignLecture", "Import");
        }
        [HandleError]
        public ActionResult ReplaceTimeTable()
        {
            ViewBag.Message = "Your contact page.";
            /*get semester list=return model*/
            ViewBag.Semesters = new SelectList(_semesterService.GetAll(), "Id", "Name");
            ViewBag.ErrorMessages = TempData["ErrorMessages"];
            return View(new ImportViewModel());
            //return RedirectToAction("AssignLecture", "Import");
        }
        [HandleError]
        [Authorize(Roles = "BCN Khoa")]
        public async Task<ActionResult> Index()
        {
            return View(_classService.GetAll().ToList());
        }

        [HandleError]
        [HttpPost]
        [Authorize(Roles = "BCN Khoa")]
        /// <summary>
        /// 1 Ma LHP - Thu : 1 lan
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ImportDataFromExcel(int semesterId)
        {            
            var result = await _excelService.ImportTimeTable(Request.Files[0].InputStream, semesterId);            
            if(result.Count > 0)
            {
                TempData["ErrorMessages"] = result;
                return RedirectToAction("Import", "Import");
            }
            return RedirectToAction("AssignLecture", "Import");
        }

        [HandleError]
        [HttpPost]
        [Authorize(Roles = "BCN Khoa")]
        public async Task<ActionResult> ImportReplaceTimeTable(int semesterId)
        {
            await _classService.DeleteAsync(semesterId);

            var result = await _excelService.ImportTimeTable(Request.Files[0].InputStream, semesterId);
            if (result.Count > 0)
            {
                TempData["ErrorMessages"] = result;
                return RedirectToAction("ReplaceTimeTable", "Import");
            }
            return RedirectToAction("AssignLecture", "Import");
        }

        [HandleError]
        [Authorize(Roles = "BCN Khoa, Bộ môn")]
        public async Task<ActionResult> AssignLecture(int id = 0)
        {
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if(id == 0 && semesters.Count > 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            ViewBag.Semesters = new SelectList(_semesterService.GetAll(), "Id", "Name");
            ViewBag.LatestSemesterId = theLatestSemesterId;
            //validation
            return await GetTimeTableAsync(theLatestSemesterId);
        }

        [HandleError]
        private async Task<ActionResult> GetTimeTableAsync(int semesterId)
        {
            //user id
            var email = User.Identity.Name;
            var currentUser = _userManager.FindByEmail(email);
            var createdAssignerId = currentUser.Id;

            var roles = _userManager.GetRoles(currentUser.Id);
            var hasPermission = roles.Any(t => t == CapstoneSystem.SubjectRole || t == CapstoneSystem.BCNKhoaRole);
            //if (!hasPermission) return RedirectToAction("Index", "Home");
            //not Truong Bo Mon && not Ban Chu nhiem khoa => redirect
            //1 user - role
            var isBcnKhoa = roles[0] == CapstoneSystem.BCNKhoaRole;

            //get all users()
            var allUsers = await _userService.GetAllAsync();
            var timeTableViewModel = new AssignTimeTableViewModel
            {
                IsBcnKhoa = isBcnKhoa,
                ApplicationUsers = allUsers, //dropdown list
                CurrentUserId = currentUser.Id,
            };
            if (string.IsNullOrEmpty(createdAssignerId))
            {
                createdAssignerId = currentUser.Id;
            }

            timeTableViewModel.ClassViewModels = await _subjectService.GetAllClassInWeekAsync(semesterId);
            timeTableViewModel.TotalAssignedClass = timeTableViewModel.ClassViewModels.Where(t => !string.IsNullOrEmpty(t.TeacherId)).Count();
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
            var userRoleModel = new UserRoleViewModel
            {
                UserId = currentUser.Id,
                RoleName = roles[0]
            };
            ViewBag.UserRole = userRoleModel;
            return View(timeTableViewModel);
        }

        /// <summary>
        /// /Import/AssignTeacher
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HandleError]
        [Authorize(Roles = "BCN Khoa, Bộ môn")]
        [HttpPost]
        public async Task<ActionResult> AssignTeacher(AssignTeacherModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            var capstoneClass = await _classService.GetByIdAsync(model.ClassId);
            //get assigner
            var assigner = _userProfileService.GetById(currentUserId);
            var subject = _subjectService.GetById(capstoneClass.SubjectId.Value);
            //get class Id
            
            if (capstoneClass == null)
            {
                return Json(false);//error message
            }
            //get teacher Id
            //var teacher = await _userService.GetByIdAsync(model.TeacherId);
            var isBcnKhoa = _userManager.IsInRole(currentUserId, "BCN Khoa");
            if (string.IsNullOrEmpty(model.TeacherId))
            {
                capstoneClass.CreatedAssignerId = null;
            }
            else
            {
                if (string.IsNullOrEmpty(capstoneClass.TeacherId) || capstoneClass.CreatedAssignerId == currentUserId || isBcnKhoa)
                {
                    if (string.IsNullOrEmpty(capstoneClass.TeacherId))
                    {
                        capstoneClass.CreatedAssignerId = currentUserId;
                    }
                }
            }
            var oldTeacherId = capstoneClass.TeacherId;
            var oldEmail = capstoneClass.TeacherEmail;
            capstoneClass.TeacherId = model.TeacherId;
            capstoneClass.LastModifiedAssignerId = currentUserId;
            capstoneClass.TeacherEmail = model.TeacherEmail;
            await _classService.UpdateAsync(capstoneClass);
            //send email
            //old teacher
            if (!string.IsNullOrEmpty(oldTeacherId))
            {
                var userProfile = _userProfileService.GetById(oldTeacherId);
                var assignedEmail = new AssignTeacherEmailTemplate
                {
                    MailTo = oldEmail,
                    MailSubject = $"Hủy phân công lịch giảng dạy môn {subject.Name}",
                    MaLhp = capstoneClass.MaLHP,
                    Subject = subject.Name,
                    FullName = userProfile.FullName,
                    Thu = capstoneClass.Thu,
                    Tiet = $"{capstoneClass.TietS}-{capstoneClass.TietS + 3}",
                    Assigner = assigner.FullName
                };
                SendUnassignedMail(assignedEmail);
            }
            if (!string.IsNullOrEmpty(model.TeacherId))
            {
                var userProfile = _userProfileService.GetById(model.TeacherId);
                var assignedEmail = new AssignTeacherEmailTemplate
                {
                    MailTo = model.TeacherEmail,
                    MailSubject = $"Phân công lịch giảng dạy môn {subject.Name}",
                    MaLhp = capstoneClass.MaLHP,
                    Subject = subject.Name,
                    FullName = userProfile.FullName,
                    Thu = capstoneClass.Thu,
                    Tiet = $"{capstoneClass.TietS}-{capstoneClass.TietS + 3}",
                    Assigner = assigner.FullName
                };
                SendAssignedMail(assignedEmail);
            }
            

            //get total assigned classes
            var countClasses = _semesterService.CountClass(capstoneClass.SemesterId.Value);
            var result = new
            {
                status = true,
                message = "Update thanh cong",
                data = new
                {
                    totalClass = countClasses.TotalClass,
                    totalAssignedClass = countClasses.TotalAssignedClass
                }
            };
            return Json(result);
        }

        [HandleError]
        [HttpPost]
        public async Task<JsonResult> ValidateTeacher(string newTeacherId, int classId)
        {
            var validClass = await _classService.ValidateTeacherInAClassAsync(newTeacherId, classId);
            if(validClass.Success == false)
            {
                if (validClass.HasConfirm == false)
                {                    
                    return Json(new
                    {
                        status = false,
                        message = validClass.Message,
                        hasConfirm = false
                    });
                }
                else
                {                   
                    return Json(new
                    {
                        status = false,
                        message = validClass.Message,
                        hasConfirm = true                      
                    });
                }
            }
            //dk 2
            var a1 = new
            {
                status = validClass.Success               
            };
            return Json(a1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ClassId</param>
        /// <returns></returns>
        [HandleError]
        [HttpPost]
        public async Task<JsonResult> GetTeacherIdByClassId(int id)
        {
            //get classId => Class => teacherId
            var captoneClass = _classService.GetById(id);
            if(captoneClass == null)
            {
                return Json(new
                {
                    status = false,
                    teacherId = 0
                });
            }
            return Json(new
            {
                status = true,
                teacherId = captoneClass.TeacherId
            });
        }
        [HandleError]
        [Authorize(Roles = "BCN Khoa")]
        [HttpPost]
        public async Task<JsonResult> ExportTimeTable(int id)
        {
            var fileByteArray = await _classService.ExportTimeTableAsync(id);
            string fileName = $"TimeTable_{DateTime.Now.ToShortDateString()}.xlsx";
            return Json(new { filename = fileName, message = Convert.ToBase64String(fileByteArray) });
        }

        #region validate
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">class id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ValidateDeleteClass(int id)
        {
            var findClass = await _classService.GetByIdAsync(id);
            if(findClass == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Không tìm thấy lớp",
                });
            }
            if (!string.IsNullOrEmpty(findClass.TeacherId))
            {
                return Json(new
                {
                    status = false,
                    message = "Không thể xóa lớp đang có giáo viên dạy",
                });
            }
            return Json(new
            {
                status = true,
                message = "",
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Class Id</param>
        /// <returns></returns>
        [Authorize(Roles = "BCN Khoa")]
        [HttpPost]
        public async Task<JsonResult> DeleteClass(int id)
        {
            var capstoneClass = await _classService.GetByIdAsync(id);
            if(capstoneClass == null)
            {
                return Json(new { status = false });
            }
            var semesterId = capstoneClass.SemesterId.Value;
            await _classService.DeleteAsync(capstoneClass);
            //get total assigned classes
            var countClasses = _semesterService.CountClass(semesterId);
            return Json(new { 
                status = true,
                message = "Update thanh cong",
                data = new
                {
                    totalClass = countClasses.TotalClass,
                    totalAssignedClass = countClasses.TotalAssignedClass
                }
            });
        }
        #endregion

        #region send emails
        private void SendAssignedMail(AssignTeacherEmailTemplate model)
        {
            var emailTemplate = RenderRazorViewToString("~/Views/Shared/AssignTeacher.cshtml", model);
            _capstoneEmailService.SendHtmlMail(model.MailTo, model.MailSubject, emailTemplate);
        }
        private void SendUnassignedMail(AssignTeacherEmailTemplate model)
        {
            var emailTemplate = RenderRazorViewToString("~/Views/Shared/UnAssignTeacher.cshtml", model);
            _capstoneEmailService.SendHtmlMail(model.MailTo, model.MailSubject, emailTemplate);
        }
        #endregion
    }
}