using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NETCore.Encrypt;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetCoreIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TwoFactorAuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UrlEncoder _urlEncoder;

        public TwoFactorAuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _urlEncoder = urlEncoder;
        }

        [HttpGet]
        [Authorize]
        public async Task<AccountDetailsVM> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            var logins = await _userManager.GetLoginsAsync(user);

            return new AccountDetailsVM
            {
                Username = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                ExternalLogins = logins.Select(login => login.ProviderDisplayName).ToList(),
                TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                TwoFactorClientRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user)
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<AuthenticatorDetailsVM> SetupAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            var authenticatorDetails = await GetAuthenticatorDetailsAsync(user);

            return authenticatorDetails;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<int>> ValidAutheticatorCodes()
        {
            List<int> validCodes = new List<int>();

            var user = await _userManager.GetUserAsync(User);

            var key = await _userManager.GetAuthenticatorKeyAsync(user);

            var hash = new HMACSHA1(Infrastructure.Identity.Internals.Base32.FromBase32(key));
            var unixTimestamp = Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
            var timestep = Convert.ToInt64(unixTimestamp / 30);
            // Allow codes from 90s in each direction (we could make this configurable?)
            for (int i = -2; i <= 2; i++)
            {
                var expectedCode = Infrastructure.Identity.Internals.Rfc6238AuthenticationService.ComputeTotp(hash, (ulong)(timestep + i), modifier: null);
                validCodes.Add(expectedCode);
            }

            return validCodes;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultVM> VerifyAuthenticator([FromBody] VefiryAuthenticatorVM verifyAuthenticator)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                var errors = GetErrors(ModelState).Select(e => "<li>" + e + "</li>");
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join("", errors)
                };
            }

            var verificationCode = verifyAuthenticator.VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2FaTokenValid)
            {
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>Verification code is invalid.</li>"
                };
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            var result = new ResultVM
            {
                Status = Status.Success,
                Message = "Your authenticator app has been verified",
            };

            if (await _userManager.CountRecoveryCodesAsync(user) != 0) return result;

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            result.Data = new { recoveryCodes };
            return result;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultVM> ResetAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);

            await _signInManager.RefreshSignInAsync(user);

            var result = new ResultVM
            {
                Status = Status.Success,
                Message = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key."
            };

            return result;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultVM> Disable2FA()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Cannot disable 2FA as it's not currently enabled"
                };
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);

            return new ResultVM
            {
                Status = result.Succeeded ? Status.Success : Status.Error,
                Message = result.Succeeded ? "2FA has been successfully disabled" : $"Failed to disable 2FA {result.Errors.FirstOrDefault()?.Description}"
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultVM> GenerateRecoveryCodes()
        {
            var user = await _userManager.GetUserAsync(User);

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            if (!isTwoFactorEnabled)
            {
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Cannot generate recovery codes as you do not have 2FA enabled"
                };
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            return new ResultVM
            {
                Status = Status.Success,
                Message = "You have generated new recovery codes",
                Data = new { recoveryCodes }
            };
        }

        [HttpPost]
        public async Task<ResultVM> Login([FromBody] TwoFactorLoginVM model)
        {
            if (ModelState.IsValid)
            {
                return await TwoFaLogin(model.TwoFactorCode, isRecoveryCode: false, model.RememberMachine);
            }

            var errors = GetErrors(ModelState).Select(e => "<li>" + e + "</li>");
            return new ResultVM
            {
                Status = Status.Error,
                Message = "Invalid data",
                Data = string.Join("", errors)
            };
        }

        [HttpPost]
        public async Task<ResultVM> LoginWithRecovery([FromBody] TwoFactorRecoveryCodeLoginVM model)
        {
            if (ModelState.IsValid)
            {
                return await TwoFaLogin(model.RecoveryCode, isRecoveryCode: true);
            }

            var errors = GetErrors(ModelState).Select(e => "<li>" + e + "</li>");
            return new ResultVM
            {
                Status = Status.Error,
                Message = "Invalid data",
                Data = string.Join("", errors)
            };
        }

        [HttpGet]
        public IActionResult AesKey()
        {
            return Ok(EncryptProvider.CreateAesKey().Key);
        }

        #region Private methods

        private async Task<ResultVM> TwoFaLogin(string code, bool isRecoveryCode, bool rememberMachine = false)
        {
            SignInResult result = null;

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>Unable to load two-factor authentication user</li>"
                };
            }

            var authenticatorCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

            if (!isRecoveryCode)
            {
                result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, true,
                    rememberMachine);
            }
            else
            {
                result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(authenticatorCode);
            }

            if (result.Succeeded)
            {
                return new ResultVM
                {
                    Status = Status.Success,
                    Message = $"Welcome {user.UserName}"
                };
            }
            else if (result.IsLockedOut)
            {
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>Account locked out</li>"
                };
            }
            else
            {
                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = $"<li>Invalid {(isRecoveryCode ? "recovery" : "authenticator")} code</li>"
                };
            }
        }

        private List<string> GetErrors(ModelStateDictionary modelState)
        {
            var errors = new List<string>();

            foreach (var state in modelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }

        private async Task<AuthenticatorDetailsVM> GetAuthenticatorDetailsAsync(IdentityUser user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var email = await _userManager.GetEmailAsync(user);

            return new AuthenticatorDetailsVM
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey)
            };
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("ASP.NET Core Identity"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        #endregion
    }
}
