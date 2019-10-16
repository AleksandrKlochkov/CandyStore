using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Store.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace Store.Controllers
{
    public class AccountController : Controller  
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    PostalCode = model.PostalCode,
                    Address = model.Address
                };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "user");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                EditModel model = new EditModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    PostalCode = user.PostalCode,
                    Address = user.Address
                };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        public static string useridstatic = null;
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> EditAdmin(string userid)
        {
            ViewBag.UserId = userid;
            ApplicationUser user = await UserManager.FindByIdAsync(userid);
            var role = UserManager.GetRoles(userid);
            foreach (var p in role)
            {
                ViewBag.Role = p;
            }
            if (user != null)
            {
                useridstatic = userid;
                EditModel model = new EditModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    PostalCode = user.PostalCode,
                    Address = user.Address
                };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Edit(EditModel model)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.PostalCode = model.PostalCode;
                user.Address = model.Address;
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditAdmin(EditModel model)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(useridstatic);
            if (user != null)
            {
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.PostalCode = model.PostalCode;
                user.Address = model.Address;
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    ModelState.AddModelError("", "Успешно изменено");
                    return RedirectToAction("EditList", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult EditList()
        {
            var db = new ApplicationContext();
            var users = db.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> AddRole(string userid, string role)
        {
            ApplicationUser user = UserManager.FindById(userid);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (role.Equals("admin"))
                {
                    IdentityResult rt = await UserManager.RemoveFromRoleAsync(userid, "user");
                    IdentityResult rt1 = await UserManager.AddToRoleAsync(userid, "admin");
                    return RedirectToRoute(new { controller = "Account", action = "EditAdmin", userid = userid });
                }
                else
                {
                    IdentityResult rt = await UserManager.RemoveFromRoleAsync(userid, "admin");
                    IdentityResult rt1 = await UserManager.AddToRoleAsync(userid, "user");
                    return RedirectToRoute(new { controller = "Account", action = "EditAdmin", userid = userid });
                }
            }

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string userid)
        {
            ApplicationUser user = UserManager.FindById(userid);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToRoute(new { controller = "Account", action = "EditList" });
                }
            }
            return RedirectToRoute(new { controller = "Account", action = "EditList" });
        }


    }
}