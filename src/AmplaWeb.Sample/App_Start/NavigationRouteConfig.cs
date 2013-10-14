using System.Web.Routing;
using AmplaWeb.Sample.Controllers;
using AmplaWeb.Sample.NavigationRoutes;

namespace AmplaWeb.Sample.App_Start
{
    public class NavigationRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<ProductionController>("Production", c => c.Index());
            routes.MapNavigationRoute<DowntimeController>("Downtime", c => c.Index());
            routes.MapNavigationRoute<MetricsController>("Metrics", c => c.Index());
            routes.MapNavigationRoute<QualityController>("Quality", c => c.Index());
            routes.MapNavigationRoute<PlanningController>("Planning", c => c.Index());
            routes.MapNavigationRoute<MaintenanceController>("Maintenance", c => c.Index()); 
            routes.MapNavigationRoute<ShiftLogController>("Shift Log", c => c.Index());

            routes.MapNavigationRoute<IngotCastController>("Ingot Casts", c => c.Default())
                  .AddChildRoute<IngotCastController>("Select", c => c.Select())
                  .AddChildRoute<IngotCastController>("List", c => c.Index())
                  ;

            routes.MapNavigationRoute<IngotBundleController>("Ingot Bundles", c => c.Index());

            /*
            routes.MapNavigationRoute<HomeController>("Automatic Scaffolding", c => c.Index());

            routes.MapNavigationRoute<ExampleLayoutsController>("Example Layouts", c => c.Starter())
                  .AddChildRoute<ExampleLayoutsController>("Marketing", c => c.Marketing())
                  .AddChildRoute<ExampleLayoutsController>("Fluid", c => c.Fluid())
                  .AddChildRoute<ExampleLayoutsController>("Sign In", c => c.SignIn())
                ;
             */
        }
    }
}
