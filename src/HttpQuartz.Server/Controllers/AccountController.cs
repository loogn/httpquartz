using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Autowired.Core;
using HttpQuartz.Server.Models;
using HttpQuartz.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpQuartz.Server.Controllers
{
    public class AccountController : MvcController
    {
        [Autowired] private SystemUserService systemUserBusiness;

        public AccountController(AutowiredService autowiredService)
        {
            autowiredService.Autowired(this);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> DoLogin(LoginRequest request)
        {
            var ro = systemUserBusiness.Login(request);
            if (ro.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim("userid", ro.Result.Id.ToString()),
                    new Claim("nickname", ro.Result.Nickname)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                ro.Result = null;
                return Json(ro);
            }
            else
            {
                return Json(ro);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public string AccessDenied()
        {
            return "无权访问";
        }

        public IActionResult UpdatePwd()
        {
            return View();
        }

        public IActionResult DoUpdatePwd(UpdatePasswordRequest request)
        {
            var ro = systemUserBusiness.UpdatePassword(request);
            return Json(ro);
        }
    }
}