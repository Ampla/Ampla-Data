using System.Web.Routing;

namespace AmplaData.Web.Sample.NavigationRoutes
{
    public interface INavigationRouteFilter
    {
        bool  ShouldRemove(Route navigationRoutes);
    }
}
