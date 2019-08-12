using System.Threading.Tasks;
using AspNetCoreIdentity.Infrastructure;
using AspNetCoreIdentity.Models;
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


            services.AddIdentity<IdentityUser, IdentityRole>(config => { config.SignIn.RequireConfirmedEmail = true; })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddUserManager<AppUserManager>()
                .AddDefaultTokenProviders();

            // Configure External Providers authentication
            services.ConfigureExternalProviders(Configuration);

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