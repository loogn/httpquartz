using System;
using Autowired.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpQuartz.Server.Controllers
{
    public class HomeController : MvcController
    {
        public HomeController(AutowiredService autowiredService)
        {
            autowiredService.Autowired(this);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult welcome()
        {
            return View();
        }

        [AllowAnonymous]
        public string Time()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}