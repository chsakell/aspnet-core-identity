using System;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentity.Infrastructure {
    public class StreaminCategoryAuthorizeAttribute : AuthorizeAttribute {
        const string POLICY_PREFIX = "StreamingCategory_";

        public StreaminCategoryAuthorizeAttribute (StreamingCategory category) => Category = category;

        // Get or set the Age property by manipulating the underlying Policy property
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
        ACTION_AND_ADVENTURE,
        ACTION_COMEDIES,
        ACTION_THRILLERS,
        ADVENTURES,
        SCI_FI,
        ANIME,
        ANIME_ACTION,
        BASKETBALL_MOVIES,
        BOXIES_MOVIES,
        FAMILY_MOVIES,
        CLASSIC_COMEDIES,
        CLASSIC_DRAMAS,
        MUSICALS
    }
}