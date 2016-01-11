using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Club.AppConfig;
using Club.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Club.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Club
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(appEnv.ApplicationBasePath)
                    .AddJsonFile("config.json")
                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentity<ClubUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/account/login";
            })
            .AddEntityFrameworkStores<ClubContext>();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ClubContext>();

#if DEBUG
            services.AddScoped<IMailService, DebugMailService>();
#endif

            MappingConfig.Configure(services);

            services.AddScoped<IClubRepository, ClubRepository>();

            services.AddTransient<ClubContextSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app,  ClubContextSeedData seeder)
        {
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(RouteConfig.Configure);

            await seeder.EnsureSeedData();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
