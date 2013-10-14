using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class MetricsController : RepositoryController<MetricsModel>
    {
        public MetricsController(IRepository<MetricsModel> repository)
            : base(repository)
        {
        }
    }
}