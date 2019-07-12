using AspNetCoreIdentity.Models;
using System.Collections.Generic;

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
