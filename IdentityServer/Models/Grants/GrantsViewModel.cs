using System.Collections.Generic;

namespace IdentityServer.Models.Grants
{
    public class GrantsViewModel
    {
        public IEnumerable<GrantViewModel> Grants { get; set; }
    }
}
