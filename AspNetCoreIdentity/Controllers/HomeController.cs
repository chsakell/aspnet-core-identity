using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Infrastructure;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbInitializer dbInitializer;

        public HomeController(IDbInitializer dbInitializer)
        {
            this.dbInitializer = dbInitializer;

            var task = dbInitializer.Initialize();
            task.Wait();
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
