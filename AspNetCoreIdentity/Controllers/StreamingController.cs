using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreIdentity.Infrastructure;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers {
    [Route ("api/[controller]")]
    public class StreamingController : Controller {

        [HttpGet]
        [Route ("videos")]
        [Authorize (Policy = "TrialOnly")]
        public IActionResult Videos () {
            var videos = new List<VideoVM> ();
            videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/1S3rNPtrDYc",
                    Title = "3 Doors Down - Father's Son",
                    Description = "Live from Houston"
            });

            videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/Bsl4bOj8vMU",
                    Title = "3 Doors Down - Kryptonite",
                    Description = "Live from Houston"
            });

            return Ok (videos);
        }

        [HttpGet]
        [Route ("videos/action/comedies")]
        [StreamingCategoryAuthorize (StreamingCategory.ACTION_COMEDIES)]
        public IActionResult ActionComedies () {
            var videos = new List<VideoVM> ();
            videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/1S3rNPtrDYc",
                    Title = "3 Doors Down - Father's Son",
                    Description = "Live from Houston"
            });

            videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/Bsl4bOj8vMU",
                    Title = "3 Doors Down - Kryptonite",
                    Description = "Live from Houston"
            });

            return Ok (videos);
        }

        [HttpGet]
        [Route ("videos/register")]
        [Authorize]
        public IActionResult UserRegistrations () {

            var registeredStreamingCategories = User.Claims
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
    }

}