using System.Web.Routing;
using AmplaWeb.Sample.Controllers;
using AmplaWeb.Sample.NavigationRoutes;

namespace AmplaWeb.Sample.App_Start
{
    public class NavigationRouteConfig
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
