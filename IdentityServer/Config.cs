using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "chsakell",
                    Password = "password",

                    Claims = new []
                    {
                        new Claim("name", "chsakell"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "admin",
                    Password = "password",

                    Claims = new []
                    {
                        new Claim("name", "admin"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // JavaScript Client
                new Client
                {
                    ClientId = "AspNetCoreIdentity",
                    ClientName = "AspNetCoreIdentity Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5000/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5000/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5000" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "SocialAPI"
                    }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("SocialAPI", "Social Network API")
            };
        }
    }
}
