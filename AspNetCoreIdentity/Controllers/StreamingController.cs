using System.Collections.Generic;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route ("api/[controller]/[action]")]
public class StreamingController : Controller {

    [HttpGet]
    [Authorize(Policy = "TrialOnly")]
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
}