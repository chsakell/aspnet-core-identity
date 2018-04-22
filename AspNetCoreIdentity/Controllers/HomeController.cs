using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(UserManager<AppUser> userManager)
        {
            userManager.CreateAsync(new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                EmailAddress = "chsakell@gmail.com",
                UserName = "chsakell"
            }, "%Kupimarko10");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
