﻿using System.Web.Routing;
using AmplaData.Web.Sample.Controllers;
using AmplaData.Web.Sample.NavigationRoutes;

namespace AmplaData.Web.Sample.App_Start
{
    public static class NavigationRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<HomeController>("Modules", c => c.Index())
                  .AddChildRoute<ProductionController>("Production", c => c.Index())
                  .AddChildRoute<DowntimeController>("Downtime", c => c.Index())
                  .AddChildRoute<MetricsController>("Metrics", c => c.Index())
                  .AddChildRoute<QualityController>("Quality", c => c.Index())
                  .AddChildRoute<PlanningController>("Planning", c => c.Index())
                  .AddChildRoute<KnowledgeController>("Knowledge", c => c.Index())
                  .AddChildRoute<MaintenanceController>("Maintenance", c => c.Index())
                  .AddChildRoute<EnergyController>("Energy", c => c.Index());
                  
            routes.MapNavigationRoute<ShiftLogController>("Shift Log", c => c.Index());

            routes.MapNavigationRoute<IngotCastController>("Ingot Casts", c => c.Default())
                  .AddChildRoute<IngotCastController>("Select", c => c.Select())
                  .AddChildRoute<IngotCastController>("List", c => c.Index())
                  ;

            routes.MapNavigationRoute<IngotBundleController>("Ingot Bundles", c => c.Index());
        }
    }
}
