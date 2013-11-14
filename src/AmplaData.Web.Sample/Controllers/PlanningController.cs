using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class PlanningController : RepositoryController<PlanningModel>
    {
        public PlanningController(IRepository<PlanningModel> repository) : base(repository)
        {
        }
    }
}