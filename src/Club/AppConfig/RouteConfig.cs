﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.AppConfig
{
    public static class RouteConfig
    {
        public static void Configure(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "home", action = "index" }
                );
        }
    }
}
