using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers {
    [Route ("api/[controller]/[action]")]
    public class AccountController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ResultVM> Register ([FromBody] RegisterVM model) {
            if (ModelState.IsValid) {
                IdentityResult result = null;
                var user = await _userManager.FindByNameAsync (model.UserName);

                if (user != null) {
                    return new ResultVM {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>User already exists</li>"
                    };
                }

                user = new IdentityUser {
                    Id = Guid.NewGuid ().ToString (),
                    UserName = model.UserName,
                    Email = model.Email
                };

                result = await _userManager.CreateAsync (user, model.Password);

                if (result.Succeeded) {
                    if(model.StartFreeTrial) {
                    Claim trialClaim = new Claim("Trial", DateTime.Now.ToString());
                    await _userManager.AddClaimAsync(user, trialClaim);
                }

                    return new ResultVM {
                        Status = Status.Success,
                            Message = "User Created",
                            Data = user
                    };
                } else {
                    var resultErrors = result.Errors.Select (e => "<li>" + e.Description + "</li>");
                    return new ResultVM {
                        Status = Status.Error,
                            Message = "Invalid data",
                            Data = string.Join ("", resultErrors)
                    };
                }
            }

            var errors = ModelState.Keys.Select (e => "<li>" + e + "</li>");
            return new ResultVM {
                Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join ("", errors)
            };
        }

        [HttpPost]
        public async Task<ResultVM> Login ([FromBody] LoginVM model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByNameAsync (model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync (user, model.Password)) {

                    await _signInManager.PasswordSignInAsync (model.UserName, model.Password, true, lockoutOnFailure : false);

                    return new ResultVM {
                        Status = Status.Success,
                            Message = "Succesfull login",
                            Data = model
                    };
                }

                return new ResultVM {
                    Status = Status.Error,
                        Message = "Invalid data",
                        Data = "<li>Invalid Username or Password</li>"
                };
            }

            var errors = ModelState.Keys.Select (e => "<li>" + e + "</li>");
            return new ResultVM {
                Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join ("", errors)
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<UserClaims> Claims () {
            var claims = User.Claims.Select (c => new ClaimVM {
                Type = c.Type,
                    Value = c.Value
            });

            return new UserClaims {
                UserName = User.Identity.Name,
                    Claims = claims
            };
        }

        [HttpGet]
        public async Task<UserStateVM> Authenticated () {
            return new UserStateVM {
                IsAuthenticated = User.Identity.IsAuthenticated,
                    Username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty
            };
        }

        [HttpPost]
        public async Task SignOut () {
            await _signInManager.SignOutAsync ();
        }
    }
}