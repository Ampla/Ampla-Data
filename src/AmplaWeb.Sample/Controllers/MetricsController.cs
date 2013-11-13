using AmplaData.Data;
using AmplaData.Data.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class MetricsController : ReadOnlyRepositoryController<MetricsModel>
    {
        public MetricsController(IRepository<MetricsModel> repository)
            : base(repository)
        {
        }
    }
}