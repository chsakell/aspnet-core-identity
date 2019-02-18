using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = CreateWebHostBuilder(args).Build();

           using (var scope = host.Services.CreateScope())
           {
               var services = scope.ServiceProvider;
               var config = services.GetService<IConfiguration>(); // the key/fix!
               var useInMemoryIdentityServer = config.GetValue<bool>("InMemoryIdentityServer");
               if (useInMemoryIdentityServer)
               {
                    DatabaseInitializer.Init(services);
               }
           }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
