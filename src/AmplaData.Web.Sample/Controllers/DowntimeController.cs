using AmplaData.Data;
using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class DowntimeController : RepositoryController<DowntimeModel>
    {
        public DowntimeController(IRepository<DowntimeModel> repository) : base(repository)
        {
        }
    }
}