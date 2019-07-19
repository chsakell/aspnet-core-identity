using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IAuthorizationPolicyProvider, StreamingCategoryPolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddTransient<IAuthorizationHandler, StreamingCategoryAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, UserCategoryAuthorizationHandler>();

            // Send Grid

            // dotnet user-secrets set SendGridUser ""
            // dotnet user-secrets set SendGridKey ""

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc();

            bool useInMemoryProvider = bool.Parse(Configuration["InMemoryProvider"]);
            services.AddDbContext<IdentityDbContext>(options =>
            {
                if (!useInMemoryProvider)
                {
                    options.UseSqlServer(Configuration.GetConnectionString("AspNetCoreIdentityDb"),
                        optionsBuilder =>
                        optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
                }
                else
                {
                    options.UseInMemoryDatabase("AspNetCoreIdentityDb");
                }
            });


            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            // Google

            //dotnet user-secrets set "Authentication:Google:ClientId" ""
            //dotnet user-secrets set "Authentication:Google:ClientSecret" ""

            if (Configuration["Authentication:Google:ClientId"] != null)
            {
                services.AddAuthentication().AddGoogle(o =>
                {
                    // Configure your auth keys, usually stored in Config or User Secrets
                    o.ClientId = Configuration["Authentication:Google:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    o.Scope.Add("https://www.googleapis.com/auth/plus.me");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
                    o.SaveTokens = true;
                    o.Events.OnCreatingTicket = ctx =>
                    {
                        List<AuthenticationToken> tokens = ctx.Properties.GetTokens() as List<AuthenticationToken>;
                        tokens.Add(new AuthenticationToken()
                            {Name = "TicketCreated", Value = DateTime.UtcNow.ToString()});
                        ctx.Properties.StoreTokens(tokens);
                        return Task.CompletedTask;
                    };
                });
            }

            // Facebook

            // dotnet user-secrets set Authentication:Facebook:AppId ""
            // dotnet user-secrets set Authentication:Facebook:AppSecret ""

            if (Configuration["Authentication:Facebook:AppId"] != null)
            {
                services.AddAuthentication().AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                });
            }

            // Twitter

            // dotnet user-secrets set Authentication:Twitter:ConsumerAPIKey ""
            // dotnet user-secrets set Authentication:Twitter:ConsumerAPISecret ""

            if (Configuration["Authentication:Twitter:ConsumerAPIKey"] != null)
            {
                services.AddAuthentication().AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerAPIKey"];
                    twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerAPISecret"];
                    twitterOptions.RetrieveUserDetails = true;
                });
            }

            // Microsoft

            // dotnet user-secrets set Authentication:Microsoft:ClientId ""
            // dotnet user-secrets set Authentication:Microsoft:ClientSecret ""

            if (Configuration["Authentication:Microsoft:ClientId"] != null)
            {
                services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                });
            }

            // GitHub

            // dotnet user-secrets set Authentication:GitHub:ClientId ""
            // dotnet user-secrets set Authentication:GitHub:ClientSecret ""

            if (Configuration["Authentication:GitHub:ClientId"] != null)
            {
                services.AddAuthentication().AddGitHub(gitHubOptions =>
                {
                    gitHubOptions.ClientId = Configuration["Authentication:GitHub:ClientId"];
                    gitHubOptions.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                });
            }

            // LinkedIn

            // dotnet user-secrets set Authentication:LinkedIn:ClientId ""
            // dotnet user-secrets set Authentication:LinkedIn:ClientSecret ""

            if (Configuration["Authentication:LinkedIn:ClientId"] != null)
            {
                services.AddAuthentication().AddLinkedIn(linkedInOptions =>
                {
                    linkedInOptions.ClientId = Configuration["Authentication:LinkedIn:ClientId"];
                    linkedInOptions.ClientSecret = Configuration["Authentication:LinkedIn:ClientSecret"];
                    linkedInOptions.CallbackPath = "/signin-linkedin";
                });
            }

            // DropBox

            // dotnet user-secrets set Authentication:DropBox:ClientId ""
            // dotnet user-secrets set Authentication:DropBox:ClientSecret ""

            if (Configuration["Authentication:DropBox:ClientId"] != null)
            {
                services.AddAuthentication().AddDropbox(dropBoxOptions =>
                {
                    dropBoxOptions.ClientId = Configuration["Authentication:DropBox:ClientId"];
                    dropBoxOptions.ClientSecret = Configuration["Authentication:DropBox:ClientSecret"];
                    dropBoxOptions.CallbackPath = "/signin-dropbox";
                });
            }

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            services.AddScoped<IDbInitializer, DbInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}