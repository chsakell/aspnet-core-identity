using System;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentity.Infrastructure {
    public class StreamingCategoryAuthorizeAttribute : AuthorizeAttribute {
        const string POLICY_PREFIX = "StreamingCategory_";

        public StreamingCategoryAuthorizeAttribute (StreamingCategory category) => Category = category;

        // Get or set the Category property by manipulating the underlying Policy property
        public StreamingCategory Category {
            get {
                var category = (StreamingCategory) Enum.Parse (typeof (StreamingCategory),
                    Policy.Substring (POLICY_PREFIX.Length));

                return (StreamingCategory) category;
            }
            set {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }

    public enum StreamingCategory {
        ACTION_AND_ADVENTURE = 1,
        ACTION_COMEDIES = 2,
        ACTION_THRILLERS = 3,
        SCI_FI = 4,
        ANIMATION = 5,
        MUSIC_VIDEOS = 6,
        BOXING_MOVIES = 7,
        FAMILY_MOVIES = 8
    }
}