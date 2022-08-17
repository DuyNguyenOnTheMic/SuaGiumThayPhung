using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CAPTimeTableIT.Infrastructure;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CAPTimeTableIT.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AspNetUserRoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUserRoleService _userroleService;
        private readonly IRoleService _roleService;
        /*  private ApplicationSignInManager _signInManager;
          private ApplicationUserManager UserManager;*/

        public AspNetUserRoleController(IUserRoleService userroleService, IRoleService roleService)
        {
            _userroleService = userroleService;
            _roleService = roleService;
        }
        [Authorize(Roles = "Admin")]
        // GET: AspNetRoles/Create
        public ActionResult Create(string id)
        {
            ViewBag.Users = new SelectList(db.Users, "Id", "UserName");
            ViewBag.Role = db.Roles.Find(id);
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRoleAddViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            try
            {
                var user = userManager.FindById(model.UserId);
                var role = _roleService.GetById(model.Id);
                if(user != null)
                {
                    userManager.AddToRole(user.Id, role.Name);
                    //db.SaveChanges();
                }                
            }
            catch
            {
                throw;
            }
            return RedirectToAction("Index", "AspNetUsers");
        }
        public ActionResult Delete(string roleId, string userId)
        {
            if (roleId == null && userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetRole = db.Roles.Find(roleId);
            var aspNetUser = db.Users.Find(userId);
            if (aspNetRole == null && aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string roleId, string userId)
        {
            var aspNetRole = db.Roles.Find(roleId);
            var aspNetUser = db.Users.Find(userId);
            db.Users.Remove(aspNetUser);
            db.Roles.Remove(aspNetRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
