using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    [Route("[controller]/[action]")]
    public class ExternalAccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ExternalAccountController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View("../Home/Index");
        }

        [HttpGet]
        public IActionResult Login(string provider, string returnUrl = null)
        {
            var redirectUrl = "/ExternalAccount/Callback";// Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                //ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                //_logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }


            // If the user does not have an account, then ask the user to create an account.

            var loginProvider = info.LoginProvider;

            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = new IdentityUser { Id = Guid.NewGuid().ToString(), UserName = userEmail, Email = userEmail };

            var createUserResult = await _userManager.CreateAsync(user);
            if (createUserResult.Succeeded)
            {
                // Add the Trial claim..
                Claim trialClaim = new Claim("Trial", DateTime.Now.ToString());
                await _userManager.AddClaimAsync(user, trialClaim);

                createUserResult = await _userManager.AddLoginAsync(user, info);
                if (createUserResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                return LocalRedirect(returnUrl);
            }

            foreach (var error in createUserResult.Errors)
            {
                return LocalRedirect(returnUrl);
            }

            return LocalRedirect(returnUrl);
        }
    }
}
