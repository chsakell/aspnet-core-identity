using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetCoreIdentity.Infrastructure;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetCoreIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<ResultVM> Register([FromBody] RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = null;
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    return new ResultVM
                    {
                        Status = Status.Error,
                        Message = "Invalid data",
                        Data = "<li>User already exists</li>"
                    };
                }

                user = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email
                };

                result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.StartFreeTrial)
                    {
                        Claim trialClaim = new Claim("Trial", DateTime.Now.ToString());
                        await _userManager.AddClaimAsync(user, trialClaim);
                    }
                    else if (model.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return new ResultVM
                    {
                        Status = Status.Success,
                        Message = "Email confirmation is pending",
                        Data = user
                    };
                }
                else
                {
                    var resultErrors = result.Errors.Select(e => "<li>" + e.Description + "</li>");
                    return new ResultVM
                    {
                        Status = Status.Error,
                        Message = "Invalid data",
                        Data = string.Join("", resultErrors)
                    };
                }
            }

            var errors = ModelState.Keys.Select(e => "<li>" + e + "</li>");
            return new ResultVM
            {
                Status = Status.Error,
                Message = "Invalid data",
                Data = string.Join("", errors)
            };
        }

        [HttpPost]
        public async Task<ResultVM> Login([FromBody] LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var result = new ResultVM();

                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        // Rule #1
                        if (!await _signInManager.CanSignInAsync(user))
                        {
                            result.Status = Status.Error;
                            result.Data = "<li>Email confirmation required</li>";

                            return result;
                        }

                        var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true,
                            lockoutOnFailure: false);

                        result.Status = signInResult == SignInResult.Success ? Status.Success : Status.Error;
                        result.Message = signInResult == SignInResult.Success ? $"Welcome {user.UserName}" : "Invalid login";
                        result.Data = signInResult == SignInResult.Success ? (object)model : $"<li>Invalid login attempt - {signInResult}</li>";

                        return result;
                    }

                    result.Status = Status.Error;
                    result.Data = $"<li>Invalid Username or Password</li>";

                    return result;
                }

                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>Invalid Username or Password</li>"
                };
            }

            var errors = ModelState.Keys.Select(e => "<li>" + e + "</li>");
            return new ResultVM
            {
                Status = Status.Error,
                Message = "Invalid data",
                Data = string.Join("", errors)
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<UserClaims> Claims()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var userClaims = await _userManager.GetClaimsAsync(loggedInUser);
            var claims = userClaims.Union(User.Claims)
                .GroupBy(c => c.Type)
                .Select(c => new ClaimVM
                {
                    Type = c.First().Type,
                    Value = c.First().Value
                });

            return new UserClaims
            {
                UserName = User.Identity.Name,
                Claims = claims
            };
        }

        [HttpGet]
        public async Task<IActionResult> Authenticated()
        {
            return Ok(new
            {
                User.Identity.IsAuthenticated,
                Username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty,
                AuthenticationMethod = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod)?.Value,
                DisplaySetPassword = User.Identity.IsAuthenticated 
                                     && !(await _userManager.HasPasswordAsync(
                                         (await _userManager.GetUserAsync(User))
                                         ))
            });
        }

        [HttpGet]
        [Route("/account/confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }

            var message = !result.Succeeded ? $"Error confirming email for user with ID '{userId}':" : "Confirmation succeeded";

            return new LocalRedirectResult($"/?message={message}&type={(result.Succeeded ? "success" : "danger")}");
        }


        [HttpGet]
        [Route("/account/ConfirmExternalProvider")]
        public async Task<IActionResult> ConfirmExternalProvider(string userId, string code,
            string loginProvider, string providerDisplayName, string providerKey)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // This comes from an external provider so we can confirm the email as well
            var confirmationResult = await _userManager.ConfirmEmailAsync(user, code);
            if (!confirmationResult.Succeeded)
                return new LocalRedirectResult($"/?message={providerDisplayName} failed to associate&type=danger");

            var newLoginResult = await _userManager.AddLoginAsync(user,
                new ExternalLoginInfo(null, loginProvider, providerKey,
                    providerDisplayName));

            if (!newLoginResult.Succeeded)
                return new LocalRedirectResult($"/?message={providerDisplayName} failed to associate&type=danger");

            var result = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey,
                isPersistent: false, bypassTwoFactor: true);
            return new LocalRedirectResult($"/?message={providerDisplayName} has been added successfully");
        }

        [HttpPost]
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}