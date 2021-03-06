﻿using System;
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
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.Localization;
using System.Globalization;
using Microsoft.AspNet.Mvc.Razor;
using Club.Filters;

namespace Club
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public static IDataProtectionProvider DataProtectionProvider { get; set; }

        public static String HtmlFooter = "© 2016 ESCOM - IPN ACM Student Chapter";
        public static String CallToActionText = "Blog";
        public static String CallToActionLink = "http://escom-ipn.acm.org/blog/";
        public static String Theme = "cosmo";

        public const string CultureCookieName = "_cultureLocalizationClub";

        public static Club.ViewModels.SettingsViewModel Settings = new ViewModels.SettingsViewModel
        {
            HtmlFooter = HtmlFooter,
            Theme = Theme,
            CallToActionLink = CallToActionLink,
            CallToActionText = CallToActionText
        };

        public Startup(IApplicationEnvironment appEnv, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile("mailConfig.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCaching();
            services.AddSession();
            services.AddLocalization(options => options.ResourcesPath = "Resources");


            services
                .AddMvc(opy =>
                {
                    opy.Filters.Add(typeof(LanguageCookieActionFilter));
                })
                .AddViewLocalization(options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization();


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
                config.Password.RequiredLength = 3;
                config.Password.RequireNonLetterOrDigit = false;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonLetterOrDigit = false;
                config.Cookies.ApplicationCookie.LoginPath = "/account/login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                };


            })
            .AddEntityFrameworkStores<ClubContext>()
            .AddDefaultTokenProviders();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ClubContext>();

            services.AddScoped<IMailService, SendGridMailService>();

            services.AddSingleton<IEventCodeGenerator, DefaultEventCodeGenerator>();
            services.AddSingleton<IQrCodeApi, GoQrApi>();

            services.AddScoped<IUserSession, UserSession>();
            services.AddScoped<IWebUserSession, UserSession>();

            MappingConfig.Configure(services);

            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IClubUsersRepository, ClubUsersRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IProblemsRepository, ProblemsRepository>();
            services.AddScoped<IUserLevelsRepository, UserLevelsRepository>();
            services.AddScoped<ITopicsRepository, TopicsRepository>();
            services.AddScoped<ISubmissionsRepository, SubmissionsRepository>();
            services.AddScoped<ITermsRepository, TermsRepository>();
            services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();




            services.AddScoped<IDateTime, DateTimeAdapter>();

            services.AddSingleton<IPagedDataRequestFactory, PagedDataRequestFactory>();

            services.AddTransient<ClubContextSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, ClubContextSeedData seeder)
        {
            //app.UseDefaultFiles();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseIdentity();


            app.UseStatusCodePages();

            app.UseSession();
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            app.UseMvc(RouteConfig.Configure);

            var mexCulture = new CultureInfo("es-ES");
            mexCulture.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy HH:mm";
            mexCulture.DateTimeFormat.LongDatePattern = "dd/MM/yyyy HH:mm";
            mexCulture.DateTimeFormat.DateSeparator = "/";
            mexCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";


            var supportedCultures = new[]
            {
                new CultureInfo("es-MX"),
                new CultureInfo("en-US")
            };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                // Set options here to change middleware behavior
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider
                    {
                        CookieName = CultureCookieName
                    },
                    new AcceptLanguageHeaderRequestCultureProvider
                    {

                    }

                }
            };

            app.UseRequestLocalization(requestLocalizationOptions, defaultRequestCulture: new RequestCulture("es-MX"));
            await seeder.EnsureSeedData();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
