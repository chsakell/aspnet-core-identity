using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers {
    [Route ("api/[controller]/[action]")]
    [Authorize(Policy = "AdminOnly")]
    public class ManageController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityDbContext _context;

        public ManageController (UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager, IdentityDbContext context) {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Users () {
            return Ok(_context.Users);
        }
    }
}