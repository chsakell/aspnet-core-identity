using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.ViewModels
{
    public class AssociateViewModel
    {
        public string Username { get; set; }
        public string OriginalEmail { get; set; }
        public string AssociateEmail { get; set; }
        public bool associateExistingAccount { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
    }
}
