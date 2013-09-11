using System.Web.Routing;

namespace AmplaWeb.Sample.NavigationRoutes
{
    public interface INavigationRouteFilter
    {
        bool  ShouldRemove(Route navigationRoutes);
    }
}
