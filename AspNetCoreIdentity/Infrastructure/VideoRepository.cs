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

            // ACTION_AND_ADVENTURE Videos
            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/2w5VZmos5I4",
                    Title = "Tomb Raider",
                    Description = "Trailer #1",
                    Category = StreamingCategory.ACTION_AND_ADVENTURE
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/sIMChzE_aCo",
                    Title = "SICARIO: DAY OF THE SOLDADO",
                    Description = "Official Trailer (HD)",
                    Category = StreamingCategory.ACTION_AND_ADVENTURE
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/eJU6S5KOsNI",
                    Title = "Mile 22",
                    Description = "Trailer #1",
                    Category = StreamingCategory.ACTION_AND_ADVENTURE
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/wb49-oV0F78",
                    Title = "Mission: Impossible - Fallout (2018)",
                    Description = "Official Trailer",
                    Category = StreamingCategory.ACTION_AND_ADVENTURE
            });

            // ACTION_COMEDIES Videos
            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/qmxMAdV6s4U",
                    Title = "GAME NIGHT",
                    Description = "Trailer #1",
                    Category = StreamingCategory.ACTION_COMEDIES
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/kjaHhduqS5o",
                    Title = "The Spy Who Dumped Me",
                    Description = "Trailer #1",
                    Category = StreamingCategory.ACTION_COMEDIES
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/-Qv6p6pTz5I",
                    Title = "Johnny English Strikes Again",
                    Description = "Official Trailer (HD)",
                    Category = StreamingCategory.ACTION_COMEDIES
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/MFWF9dU5Zc0",
                    Title = "OCEAN'S 8",
                    Description = "Official 1st Trailer",
                    Category = StreamingCategory.ACTION_COMEDIES
            });

            // ACTION_THRILLERS Videos
            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/acQyrwQyCOk",
                    Title = "Insidious: The Last Key",
                    Description = "Official Trailer",
                    Category = StreamingCategory.ACTION_THRILLERS
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/u9Mv98Gr5pY",
                    Title = "VENOM",
                    Description = "Official Trailer (HD)",
                    Category = StreamingCategory.ACTION_THRILLERS
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/EVGaOGNzyx8",
                    Title = "MUTE",
                    Description = "Official Trailer (HD)",
                    Category = StreamingCategory.ACTION_THRILLERS
            });

            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/aDshY43Ol2U",
                    Title = "The Commuter",
                    Description = "Official Teaser Trailer ",
                    Category = StreamingCategory.ACTION_THRILLERS
            });

            // SCI_FI Videos
            Videos.Add (new VideoVM {
                Url = "https://www.youtube.com/embed/1S3rNPtrDYc",
                    Title = "Deadpool 2",
                    Description = "Official Trailer",
                    Category = StreamingCategory.SCI_FI
            });
            
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