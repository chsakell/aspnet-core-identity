using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.ViewModels
{
    public class AuthenticatorDetailsVM
    {
        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }
    }
}
