using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class MaintenanceController : RepositoryController<MaintenanceModel>
    {
        public MaintenanceController(IRepository<MaintenanceModel> repository) : base(repository)
        {
        }
    }
}