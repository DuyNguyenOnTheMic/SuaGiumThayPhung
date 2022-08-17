using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models.Models;
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
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserService userService, IRoleService roleService,
            IUserProfileService userProfileService)
        {
            _userService = userService;
            _roleService = roleService;
            _userProfileService = userProfileService;
        }
        // GET: UserProfile
        //[Authorize(Roles = "Giảng viên")]
        //public ActionResult UserProfile()
        //{
        //    var result = _userService.GetUserRoles();
        //    return View(result);
        //}
        public async Task<ActionResult> EditUserProfile()
        {
            var currentUserId = User.Identity.GetUserId();
            var userProfile = _userProfileService.GetById(currentUserId);
            if (userProfile == null)
            {
                userProfile = new UserProfile
                {
                    Id = currentUserId,
                    FullName = User.Identity.Name
                };
                await _userProfileService.AddAsync(userProfile);
                //return RedirectToAction("Index", "Home");
            }

            //map UserProfile => UserModel
            var userModel = new UserModel
            {
                Id = currentUserId,
                FullName = userProfile.FullName,
                StaffCode = userProfile.StaffCode
            };
            ViewBag.ShowSuccesfulMessage = false;
            return View(userModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserProfile(UserModel model)
        {
            var userProfile = _userProfileService.GetById(model.Id);
            if (userProfile != null)
            {
                userProfile.FullName = model.FullName;
                userProfile.StaffCode = model.StaffCode;
                await _userProfileService.UpdateAsync(userProfile);
            }
            ViewBag.ShowSuccesfulMessage = true;
            return View(model);
        }
        #region Validation
        public JsonResult ValidateNewUserProfile(string staffCode, string id)
        {
            if(string.IsNullOrEmpty(staffCode) || string.IsNullOrEmpty(id)) return Json(true, JsonRequestBehavior.AllowGet);
            var findStaffCode = _userProfileService.FindUserProfile(staffCode);
            if (findStaffCode == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if(findStaffCode.Id == id) return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}