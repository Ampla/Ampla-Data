using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class MetricsController : ReadOnlyRepositoryController<MetricsModel>
    {
        public MetricsController(IReadOnlyRepository<MetricsModel> repository)
            : base(repository)
        {
        }
    }
}