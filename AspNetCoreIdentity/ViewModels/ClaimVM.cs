using System.Collections.Generic;

namespace AspNetCoreIdentity.ViewModels
{
    public class ClaimVM
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class UserClaims
    {
        public IEnumerable<ClaimVM> Claims { get; set; }
        public string UserName { get; set; }
    }
}
