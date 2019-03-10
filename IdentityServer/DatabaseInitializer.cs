using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public class DatabaseInitializer
    {
        public static void Init(IServiceProvider provider, bool useInMemoryStores)
        {
            if (!useInMemoryStores)
            {
                provider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                provider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

                //InitializeIdentityServer(provider);
            }
            InitializeIdentityServer(provider);

            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            var chsakell = userManager.FindByNameAsync("chsakell").Result;
            if (chsakell == null)
            {
                chsakell = new IdentityUser
                {
                    UserName = "chsakell"
                };
                var result = userManager.CreateAsync(chsakell, "$AspNetIdentity10$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                chsakell = userManager.FindByNameAsync("chsakell").Result;

                result = userManager.AddClaimsAsync(chsakell, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Chris Sakellarios"),
                    new Claim(JwtClaimTypes.GivenName, "Christos"),
                    new Claim(JwtClaimTypes.FamilyName, "Sakellarios"),
                    new Claim(JwtClaimTypes.Email, "chsakellsblog@blog.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "https://chsakell.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Localhost 10', 'postal_code': 69118, 'country': 'Greece' }", 
                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }).Result;

                if (useInMemoryStores)
                {
                    //userManager.AddClaimAsync(chsakell, new Claim(JwtClaimTypes.Subject, chsakell.Id));
                }

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Console.WriteLine("chsakell created");
            }
            else
            {
                Console.WriteLine("chsakell already exists");
            }
        }

        private static void InitializeIdentityServer(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<ConfigurationDbContext>();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApis())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
