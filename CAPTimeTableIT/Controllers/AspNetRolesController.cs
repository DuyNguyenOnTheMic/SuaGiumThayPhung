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
using Microsoft.AspNet.Identity.EntityFramework;

namespace CAPTimeTableIT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AspNetRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IRoleService _roleService;
        public AspNetRolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        // GET: AspNetRoles
        public ActionResult Index()
        {
            List<ApplicationRole> result = _roleService.GetAll();
            return View(result);
        }

        // GET: AspNetRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetRole = db.Roles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            return View(new RoleCreateModel());
        }

        // POST: AspNetRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleCreateModel aspNetRole)
        {
            if (ModelState.IsValid)
            {
                var newRole = new ApplicationRole
                {
                    Name = aspNetRole.RoleName
                };
                await _roleService.AddAsync(newRole);
                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetRole = db.Roles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] IdentityRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetRole = db.Roles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var aspNetRole = db.Roles.Find(id);
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
