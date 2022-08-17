using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    [Authorize(Roles = "BCN Khoa")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        public ActionResult Index()
        {
            var subjects = _subjectService.GetAll();
            return View(subjects);
        }
        // GET: Subject
        public ActionResult EditSubject(int id)
        {
            var subject = _subjectService.GetById(id);
            var model = new SubjectPostModel
            {
                Id = id,
                Name = subject.Name,
                Description = subject.Description,
                SubjectCode = subject.SubjectCode
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditSubject(SubjectPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var subject = _subjectService.GetById(model.Id);
            if(subject == null)
            {
                ModelState.AddModelError("SubjectId", "Không tìm thấy môn học");
            }
            subject.Name = model.Name;
            subject.Description = model.Description;
            subject.LastModifiedDateTime = DateTime.UtcNow;
            await _subjectService.UpdateAsync();
            return RedirectToAction("Index");
        }
    }
}