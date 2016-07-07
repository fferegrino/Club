using Microsoft.AspNet.Builder;
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
                name: "Users short routes",
                template: "u/{*username}",
                defaults: new { controller = "users", action = "details" }
                );

            routes.MapRoute(
                name: "Calendar route",
                template: "calendar/{year:int?}/{month:int?}",
                defaults: new { controller = "calendar", action = "index" }
                );

            routes.MapRoute(
                name: "Attend event route",
                template: "events/attend/{*eventCode}",
                defaults: new { controller = "events", action = "attend" }
                );

            routes.MapRoute(
                name: "Submissions route",
                template: "submissions",
                defaults: new { action = "index", controller = "submit" }
                );

            routes.MapRoute(
                name: "Submit route",
                template: "submit/{problemId:int}",
                defaults: new { action = "create", controller = "submit" }
                );

            routes.MapRoute(
                name: "View submission",
                template: "submission/{problemId:int}/{user?}",
                defaults: new { action = "details", controller = "submit" }
                );

            routes.MapRoute(
                name: "Detail routes",
                template: "{controller}/{id:int}",
                defaults: new { action = "detail" }
                );

            routes.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "home", action = "index" }
                );
        }
    }
}
