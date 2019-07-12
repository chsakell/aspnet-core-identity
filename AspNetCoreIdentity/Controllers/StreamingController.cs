using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Infrastructure;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers {
    [Route ("api/[controller]")]
    public class StreamingController : Controller {

        private readonly UserManager<IdentityUser> _userManager;
        public IAuthorizationService _authorizationService { get; }

        public StreamingController (UserManager<IdentityUser> manager,
            IAuthorizationService authorizationService) {
            _authorizationService = authorizationService;
            _userManager = manager;
        }

        [HttpGet]
        [Route ("videos")]
        [Authorize (Policy = "TrialOnly")]
        public IActionResult Videos () {
            var videos = VideoRepository.Videos.Take (4);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("videos/register")]
        [Authorize]
        public async Task<IActionResult> UserRegistrations () {
            var loggedInUser = await _userManager.GetUserAsync (User);

            // Remove any categories registered     
            var userClaims = await _userManager.GetClaimsAsync (loggedInUser);

            var registeredStreamingCategories = userClaims
                .Where (c => Enum.IsDefined (typeof (StreamingCategory), c.Type))
                .Select (c => c.Type);

            List<object> categories = new List<object> ();

            foreach (StreamingCategory category in Enum.GetValues (typeof (StreamingCategory))) {
                categories.Add (new {
                    Category = category.ToString (),
                        Value = (int) category,
                        Registered = registeredStreamingCategories.Any (c => c == category.ToString ())
                });
            }

            return Ok (categories);
        }

        [HttpPost]
        [Route ("videos/add")]
        [Authorize]
        public async Task<IActionResult> AddVideo ([FromBody] VideoVM video) {

            var authorizationResult = await _authorizationService
                .AuthorizeAsync (User, video, "AddVideoPolicy");

            if (authorizationResult.Succeeded) {
                VideoRepository.Videos.Add (video);
                return Ok ();
            } else {
                return new ForbidResult ();
            }
        }

        [HttpPost]
        [Route ("videos/register")]
        [Authorize]
        public async Task<IActionResult> RegisterCategoryAsync ([FromBody] List<string> categories) {
            var loggedInUser = await _userManager.GetUserAsync (User);
            var userClaims = await _userManager.GetClaimsAsync (loggedInUser);
            // Do not use User.Claims

            var registeredStreamingCategoriesClaims = userClaims
                .Where (c => Enum.IsDefined (typeof (StreamingCategory), c.Type));

            // Remove any categories registered  
            var removeClaimsResult = await _userManager.RemoveClaimsAsync (loggedInUser,
                registeredStreamingCategoriesClaims.Where (claim => !categories.Any (c => c == claim.Type)));

            // Add new categories
            var newClaims = categories.Where (c => !registeredStreamingCategoriesClaims.Any (claim => claim.Type == c))
                .Select (type => new Claim (type, DateTime.Now.ToString ()));

            if (newClaims.Count () > 0) {
                var addClaimsResult = await _userManager.AddClaimsAsync (loggedInUser, newClaims);
            }

            return Ok ();
        }

        #region Categories

        [HttpGet]
        [Route ("ACTION_COMEDIES")]
        [StreamingCategoryAuthorize (StreamingCategory.ACTION_COMEDIES)]
        public IActionResult ActionComedies () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.ACTION_COMEDIES);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("ACTION_AND_ADVENTURE")]
        [StreamingCategoryAuthorize (StreamingCategory.ACTION_AND_ADVENTURE)]
        public IActionResult ActionAdventure () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.ACTION_AND_ADVENTURE);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("ACTION_THRILLERS")]
        [StreamingCategoryAuthorize (StreamingCategory.ACTION_THRILLERS)]
        public IActionResult ActionThrillers () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.ACTION_THRILLERS);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("SCI_FI")]
        [StreamingCategoryAuthorize (StreamingCategory.SCI_FI)]
        public IActionResult SCI_FI () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.SCI_FI);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("ANIMATION")]
        [StreamingCategoryAuthorize (StreamingCategory.ANIMATION)]
        public IActionResult ANIMATION () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.ANIMATION);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("MUSIC_VIDEOS")]
        [StreamingCategoryAuthorize (StreamingCategory.MUSIC_VIDEOS)]
        public IActionResult MUSIC_VIDEOS () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.MUSIC_VIDEOS);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("BOXING_MOVIES")]
        [StreamingCategoryAuthorize (StreamingCategory.BOXING_MOVIES)]
        public IActionResult BOXING_MOVIES () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.BOXING_MOVIES);

            return Ok (videos);
        }

        [HttpGet]
        [Route ("FAMILY_MOVIES")]
        [StreamingCategoryAuthorize (StreamingCategory.FAMILY_MOVIES)]
        public IActionResult FAMILY_MOVIES () {

            var videos = VideoRepository.Videos
                .Where (v => v.Category == StreamingCategory.FAMILY_MOVIES);

            return Ok (videos);
        }

        #endregion

    }

    public class RegisterCategoryVM {
        public string Category { get; set; }
    }

}