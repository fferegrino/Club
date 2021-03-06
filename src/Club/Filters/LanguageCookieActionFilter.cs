﻿using Club.Helpers;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Filters
{
    //CUSTOM COOKIE LOCALIZATION

    public class LanguageCookieActionFilter: ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public const string CultureCookieName = Startup.CultureCookieName;

        public LanguageCookieActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("LanguageActionFilter");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //string cultureName;
            //var request = context.HttpContext.Request;
            //var cultureCookie = request.Cookies[CultureCookieName];

            //if (cultureCookie.Any())
            //{
            //    cultureName = cultureCookie[0];
            //}
            //else
            //{
            //    cultureName = request.Headers["Accept-Language"];
            //    var cultures = CultureHelper.ParserHeaderAcceptedLanguage(cultureName);

            //    var cultureFirst = cultures?.FirstOrDefault();
            //    if (cultureFirst != null)
            //        cultureName = cultureFirst.Value;

            //    cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            //    context.HttpContext.Response.Cookies.Append(CultureCookieName, cultureName);

            //}

            //_logger.LogInformation($"Setting the culture from the URL: {cultureName}");

            // Override thread culture
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");

            base.OnActionExecuting(context);
        }
    }
}
