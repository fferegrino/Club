using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Club.Models.Repositories;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Club.Common;
using Club.Common.Security;
using Club.Models.Context;
using Club.Models.Entities;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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


            services.Configure<MvcOptions>(options =>
            {
                var formatter = options.OutputFormatters.First(f => f is JsonOutputFormatter) as JsonOutputFormatter;

                formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                formatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            

            services.AddIdentity<ClubUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                
                config.Password.RequiredLength = 6;
                config.Password.RequireNonLetterOrDigit = false;
                config.Cookies.ApplicationCookie.LoginPath = "/account/login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int) HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                };
                

            })
            .AddEntityFrameworkStores<ClubContext>();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ClubContext>();

#if DEBUG
            services.AddScoped<IMailService, DebugMailService>();
#endif
            services.AddSingleton<IEventCodeGenerator, DefaultEventCodeGenerator>();
            services.AddSingleton<IQrCodeApi, GoQrApi>();

            services.AddScoped<IUserSession, UserSession>();
            services.AddScoped<IWebUserSession, UserSession>();

            MappingConfig.Configure(services);

            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IClubUsersRepository, ClubUsersRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();

            services.AddScoped<IDateTime, DateTimeAdapter>();

            services.AddSingleton<IPagedDataRequestFactory,PagedDataRequestFactory>();

            services.AddTransient<ClubContextSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, ClubContextSeedData seeder)
        {
            //app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseIdentity();


            app.UseStatusCodePages();

            app.UseMvc(RouteConfig.Configure);


            app.UseFacebookAuthentication(new FacebookOptions
            {
                AppId = "X",
                AppSecret = "X"
            });

            await seeder.EnsureSeedData();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
