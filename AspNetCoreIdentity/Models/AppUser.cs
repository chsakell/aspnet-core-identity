using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Models
{
    public class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string NormalizeUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
