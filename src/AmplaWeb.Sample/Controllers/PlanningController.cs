using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class PlanningController : RepositoryController<PlanningModel>
    {
        public PlanningController(IRepository<PlanningModel> repository) : base(repository)
        {
        }
    }
}