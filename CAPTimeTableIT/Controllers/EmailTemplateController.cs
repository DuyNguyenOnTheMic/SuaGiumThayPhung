using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models.Asssign;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    public class EmailTemplateController : Controller
    {
        private readonly ICapstoneEmailService _capstoneEmailService;
        public EmailTemplateController(ICapstoneEmailService capstoneEmailService)
        {
            _capstoneEmailService = capstoneEmailService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AssignTeacher(AssignTeacherEmailTemplate model)
        {
            var emailTemplate = RenderViewToString("AssignTeacher", model);
           /* _capstoneEmailService.SendMail(model.MailTo, model.MailSubject, emailTemplate);*/
            return RedirectToAction("RequestSuccessfully");
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}