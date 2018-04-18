using AspNetCoreIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Infrastructure
{
    public static class UserRepository
    {
        public static List<AppUser> Users;

        static UserRepository()
        {
            Users = new List<AppUser>();
        }
    }
}
