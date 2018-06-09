using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Infrastructure
{
    public static class VideoRepository
    {
        public static List<VideoVM> Videos;

        static VideoRepository()
        {
            Videos = new List<VideoVM>();
            
            // Music Videos
            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/1S3rNPtrDYc",
                    Title = "3 Doors Down - Father's Son",
                    Description = "Live from Houston",
                    Category = StreamingCategory.MUSIC_VIDEOS
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/Bsl4bOj8vMU",
                    Title = "3 Doors Down - Kryptonite",
                    Description = "Live from Houston",
                    Category = StreamingCategory.MUSIC_VIDEOS
            });
        }
    }
}