using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IUserService _userService;
        private readonly IUserProfileService _userProfileService;

        //public AccountController() { }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IUserProfileService userProfileService,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userProfileService = userProfileService;
            _userService = userService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = "/";
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public void ExternalLogin()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback") }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public async Task<SignInStatusModel> ExternalSignInAsync2<TUser, TKey>(SignInManager<TUser, TKey> manager, ExternalLoginInfo loginInfo, UserManager<TUser, TKey> UserManager) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
        {

            SignInStatus result = await manager.ExternalSignInAsync(loginInfo, isPersistent: false);
            if ((uint)result <= 2u)
            {
                var currentUser = await _userService.GetUserInfoByEmailAsync(loginInfo.Email);
                if (currentUser != null)
                {
                    //get userProfile
                    var userProfile = _userProfileService.GetById(currentUser.UserId);
                    if (userProfile == null)
                    {
                        var newUserProfile = new UserProfile
                        {
                            Id = currentUser.UserId,
                            FullName = loginInfo.Email
                        };
                        await _userProfileService.AddAsync(newUserProfile);
                    }
                    var signinResult = new SignInStatusModel
                    {
                        RoleName = currentUser.RoleName,
                        SignInStatus = result
                    };
                    return signinResult;
                }
            }

            var user = Activator.CreateInstance<TUser>();
            user.GetType().GetProperty("UserName").SetValue(user, loginInfo.Email);
            user.GetType().GetProperty("Email").SetValue(user, loginInfo.Email);
            if ((await UserManager.CreateAsync(user)).Succeeded)
            {
                var id = (TKey)user.GetType().GetProperty("Id").GetValue(user);
                //get userProfile
                var userProfile = _userProfileService.GetById(id.ToString());
                if (userProfile == null)
                {
                    var newUserProfile = new UserProfile
                    {
                        Id = id.ToString(),
                        FullName = loginInfo.Email
                    };
                    await _userProfileService.AddAsync(newUserProfile);
                }
                if ((await UserManager.AddLoginAsync(id, loginInfo.Login)).Succeeded)
                {
                    await manager.UserManager.AddToRoleAsync(user.Id, "Chưa Phân Quyền");
                    await manager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    var signinResult = new SignInStatusModel
                    {
                        RoleName = "Chưa Phân Quyền",
                        SignInStatus = SignInStatus.Success
                    };
                    return signinResult;
                    //return SignInStatus.Success;
                }
            }

            return new SignInStatusModel
            {
                RoleName = "",
                SignInStatus = SignInStatus.Failure
            }; ;
        }


        //
        // GET: /Account/ExternalLoginCallback
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            // Get user information
            var user = new ApplicationUser
            {
                Email = User.Identity.Name,
                UserName = User.Identity.Name,
            };

            // Check if user exists
            var currentUser = await UserManager.FindByEmailAsync(user.Email);
            if (currentUser != null)
            {
                if (currentUser.Roles.Count != 0)
                {
                    // Add role claim to user
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

                    var currentRole = await UserManager.GetRolesAsync(currentUser.Id);
                    identity.AddClaim(new Claim(ClaimTypes.Role, currentRole[0]));
                    IOwinContext context = HttpContext.GetOwinContext();

                    context.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    context.Authentication.SignIn(identity);
                }
            }
            else
            {
                // Create new user
                await UserManager.CreateAsync(user);
                await UserManager.AddToRoleAsync(user.Id, "Chưa Phân Quyền");
            }

            if (currentUser.Roles.FirstOrDefault().ToString() == "BCN Khoa")
            {
                return RedirectToAction("Calendar", "Calendar");
            }
            if (currentUser.Roles.FirstOrDefault().ToString() == "Bộ môn")
            {
                return RedirectToAction("Calendar", "Calendar");
            }
            if (currentUser.Roles.FirstOrDefault().ToString() == "Giảng viên")
            {
                return RedirectToAction("PersonalCalendar", "PersonalCalendar");
            }
            if (currentUser.Roles.FirstOrDefault().ToString() == "Admin")
            {
                return RedirectToAction("Index", "AspNetUsers");
            }
            return RedirectToLocal(returnUrl);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            /// Send an OpenID Connect sign-out request.
            HttpContext.GetOwinContext()
                        .Authentication
                        .SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}