using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using CAPTimeTableIT.Models.Models;
using CAPTimeTableIT.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CAPTimeTableIT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AspNetUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserProfileService _userProfileService;
        public AspNetUsersController(ApplicationDbContext dbContext, ApplicationUserManager userManager, IUserService userService, IRoleService roleService,
            IUserProfileService userProfileService)
        {
            _db = dbContext;
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
            _userProfileService = userProfileService;
        }
        // GET: AspNetUsers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var result = _userService.GetUserRoles();
            return View(result);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UserName()
        {
            var result = _userService.GetUserRoles();
            return View(result);
        }
        
        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id, string roleId)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roleId))
            {
                return RedirectToAction("Index");
            }
            var roles = _db.Roles.Select(t=>new RoleModel
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
            //at least 1 role
            var role = roles.FirstOrDefault(t=>t.Id == roleId);
            ViewBag.Roles = new SelectList(roles, "Id", "Name", roleId);
            ViewBag.UserId = id;
            //ViewBag.Role = _db.Roles.Find(id);
            return View(role);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string roleId,string userId, string oldRoleId)
        {
            var test = new UserStore<ApplicationUser>(_db);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            /*var roles = userManager.FindById(model.RoleId);*/
            var role = roleManager.FindById(roleId);
            var oldRole = roleManager.FindById(oldRoleId);
            var user = await userManager.FindByIdAsync(userId);
            var roles = _roleService.GetAll().Select(t => new RoleModel
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
            if (oldRoleId != roleId)
            {
                userManager.RemoveFromRole(userId, oldRole.Name);
                userManager.AddToRole(userId, role.Name);
            }
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AspNetUsers/Edit/5
        public async Task<ActionResult> EditUserName(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var findUser = _userManager.FindById(id);
            if(findUser == null)
            {
                return RedirectToAction("Index");
            }
            var userProfile = _userProfileService.GetById(id);
            if (userProfile == null)
            {
                userProfile = new UserProfile
                {
                    Id = id,
                    FullName = findUser.UserName//User.Identity.Name
                };
                await _userProfileService.AddAsync(userProfile);
                //return RedirectToAction("Index");
            }
            var userModel = new UserModel
            {
                Id = userProfile.Id,
                FullName = userProfile.FullName,
                StaffCode = userProfile.StaffCode
            };
            return View(userModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserName(UserModel model)
        {
            var userProfile = _userProfileService.GetById(model.Id);
            if(userProfile != null)
            {
                userProfile.FullName = model.FullName;
                userProfile.StaffCode = model.StaffCode;
                await _userProfileService.UpdateAsync(userProfile);
            }            
            return RedirectToAction("UserName");
        }
        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetUser = _db.Users.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> ValidateUserThatNeedToBeDeleted(DeletedUserModel user)
        {
            //users cannot deleted theirselvies
            var currentUserId = User.Identity.GetUserId();
            if(currentUserId == user.UserId)
            {
                return Json(new
                {
                    status = false,
                    message = "Bạn không thể tự xóa chính bạn",
                    hasConfirm = false
                });
            }
            var validUser = await _userService.ValidateDeleteUserAsync(user.UserId);
            if (validUser.Success == false)
            {
                if (validUser.HasConfirm == false)
                {
                    return Json(new
                    {
                        status = validUser.Success,
                        message = validUser.Message,
                        hasConfirm = false
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = validUser.Success,
                        message = validUser.Message,
                        hasConfirm = true
                    });
                }
            }
            return Json(new
            {
                status = validUser.Success
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<JsonResult> DeleteConfirmed(DeletedUserModel model)
        {
            var message = new
            {
                status = false,
                messaage = "User is not found"
            };
            if (model == null || string.IsNullOrEmpty(model.UserId))
            {
                return Json(message);
            }
            var user = _userService.GetById(model.UserId);
            if(user == null)
            {
                return Json(message);
            }
            var result = await _userService.DeleteAsync(user);
            return Json(new
            {
                result = result > 0
            });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
