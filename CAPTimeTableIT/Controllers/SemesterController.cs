using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    [Authorize(Roles = "BCN Khoa")]
    public class SemesterController : Controller
    {
        private const int SubstractYear = 2;
        private const int AddYear = 30;
        private readonly ISemesterService _semesterService;
        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }
        // GET: Subject
        public async Task<ActionResult> Create()
        {
            var semesters = await _semesterService.GetAllAsync();
            ViewBag.Semesters = new SelectList(semesters, "Id", "Name");
            ViewBag.StartYear = DateTime.Now.Year - SubstractYear;
            ViewBag.EndYear = DateTime.Now.Year + AddYear;
            return View("Create", new SemesterAddViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SemesterAddViewModel semestermodel)
        {
            if (ModelState.IsValid)
            {
                var newSemester = new Semester
                {
                    Name = semestermodel.Name/* + " (" + semestermodel.StartYear + " - " + semestermodel.EndYear + ")"*/,
                    StartYear = semestermodel.StartYear,
                    EndYear = semestermodel.EndYear,
                    StartTime = semestermodel.StartTime,
                    ListWeek = semestermodel.ListWeek,
                };
                await _semesterService.AddAsync(newSemester);
                return RedirectToAction("ManageSemester", "Semester");
            }
            return View(semestermodel);
        }
        public async Task<ActionResult> ManageSemester()
        {
            var semesters = await _semesterService.GetSemesterSummaries() ;//_semesterService.GetAllAsync();
            return View(semesters);
        }
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return Redirect("ManageSemester");
            }
            var removeSemester = _semesterService.GetById(id);
            if (removeSemester == null)
            {
                return HttpNotFound();
            }
            return View(removeSemester);
        }

        [HttpPost]
        public async Task<JsonResult> ValidateSemesterThatNeedToBeDeleted(int id)
        {
            var validSemester = await _semesterService.ValidateDeleteSemesterIdAsync(id);
            if (validSemester.Success == false)
            {
                if (validSemester.HasConfirm == false)
                {
                    return Json(new
                    {
                        status = validSemester.Success,
                        message = validSemester.Message,
                        hasConfirm = false
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = validSemester.Success,
                        message = validSemester.Message,
                        hasConfirm = true
                    });
                }
            }
            var a1 = new
            {
                status = validSemester.Success
            };
            return Json(a1);
        }

        public JsonResult ValidateNewSemester(string name)
        {
            var findSemester = _semesterService.FindSemester(name);
            if (findSemester == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // POST: Semester/Delete
        [HttpPost, ActionName("DeleteConfirmed")]
        public JsonResult DeleteConfirmed(int id)
        {
            _semesterService.Delete(id);
            return Json(true);
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var editSemester = await _semesterService.GetByIdAsync(id.Value);
            if (editSemester == null)
            {
                return HttpNotFound();
            }
            var model = new SemesterEditViewModel
            {
                Id = editSemester.Id,
                Name = editSemester.Name,
                StartYear = editSemester.StartYear,
                EndYear = editSemester.EndYear,
                ListWeek = editSemester.ListWeek,
                StartTime = editSemester.StartTime,
            };
            var startYear = DateTime.Now.Year - SubstractYear;
            if(startYear > model.StartYear)
            {
                startYear = model.StartYear;
            }
            ViewBag.StartYear = startYear;
            ViewBag.EndYear = DateTime.Now.Year + AddYear;
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(SemesterEditViewModel semester)
        {
            if (ModelState.IsValid)
            {
                var findSemester = await _semesterService.GetByIdAsync(semester.Id);
                if (findSemester == null)
                {
                    ModelState.AddModelError("404", "Không tìm thấy học kỳ");
                    return View(semester);
                }
                //disable Name in Edit semester
                //findSemester.Name = semester.Name;
                findSemester.StartYear = semester.StartYear;
                findSemester.EndYear = semester.EndYear;
                findSemester.StartTime = semester.StartTime;
                findSemester.ListWeek = semester.ListWeek;

                var result = await _semesterService.UpdateAsync();
                return RedirectToAction("ManageSemester", "Semester");
            }
            return View(semester);
        }
    }
}