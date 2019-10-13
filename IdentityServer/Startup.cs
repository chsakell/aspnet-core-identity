using System;
using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            bool useInMemoryStores = bool.Parse(Configuration["UseInMemoryStores"]);
            var connectionString = Configuration.GetConnectionString("IdentityServerConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (useInMemoryStores)
                {
                    options.UseInMemoryDatabase("IdentityServerDb");
                }
                else
                {
                    options.UseSqlServer(connectionString);
                }
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2); ;

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

                var builder = services.AddIdentityServer(options =>
                    {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;
                    })
                    // this adds the config data from DB (clients, resources)
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = opt =>
                        {
                            if (useInMemoryStores)
                            {
                                opt.UseInMemoryDatabase("IdentityServerDb");
                            }
                            else
                            {
                                opt.UseSqlServer(connectionString,
                                    optionsBuilder =>
                                        optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
                            }
                        };
                    })
                    // this adds the operational data from DB (codes, tokens, consents)
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = opt =>
                        {
                            if (useInMemoryStores)
                            {
                                opt.UseInMemoryDatabase("IdentityServerDb");
                            }
                            else
                            {
                                opt.UseSqlServer(connectionString,
                                    optionsBuilder =>
                                        optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
                            }
                        };

                        // this enables automatic token cleanup. this is optional.
                        options.EnableTokenCleanup = true;
                    })
                    .AddAspNetIdentity<IdentityUser>();
            
            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }
    }
}
