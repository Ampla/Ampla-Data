using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class DowntimeController : RepositoryController<DowntimeModel>
    {
        public DowntimeController(IRepository<DowntimeModel> repository) : base(repository)
        {
        }
    }
}