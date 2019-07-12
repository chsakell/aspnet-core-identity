using System.Diagnostics;
using AspNetCoreIdentity.Infrastructure;
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
