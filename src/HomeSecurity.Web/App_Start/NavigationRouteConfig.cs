using HomeSecurity.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using NavigationRoutes;

namespace HomeSecurity.Web.App_Start
{
    public class NavigationRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<HomeController>("Home", c => c.Index());

            routes.MapNavigationRoute<HomeSecurityController>("Dashboard", c => c.Index());

            routes.MapNavigationRoute<DocsController>("Documentation",c => c.Dummy())
                  .AddChildRoute<DocsController>("Home", c => c.Index())
                  .AddChildRoute<DocsController>("Getting Ready", c => c.GettingReadyForTheMeeting())
                  .AddChildRoute<DocsController>("Lab - External Door", c => c.ExternalDoorEntry())
                  .AddChildRoute<DocsController>("Lab - Doorbell", c => c.DoorBell())
                  .AddChildRoute<DocsController>("Lab - Alarm", c => c.Alarm())
                  .AddChildRoute<DocsController>("Lab - Alarm Control Panel", c => c.AlarmControlPanel())
                  .AddChildRoute<DocsController>("Master Control Panel", c => c.MasterControlPanel())
                ;

            routes.MapNavigationRoute<SponsorsController>("Sponsors", c => c.Index());
        }
    }
}