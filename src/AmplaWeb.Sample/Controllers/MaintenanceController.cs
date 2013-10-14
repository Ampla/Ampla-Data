using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class MaintenanceController : RepositoryController<MaintenanceModel>
    {
        public MaintenanceController(IRepository<MaintenanceModel> repository) : base(repository)
        {
        }
    }
}