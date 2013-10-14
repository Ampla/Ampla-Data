using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class EnergyController : RepositoryController<EnergyModel>
    {
        public EnergyController(IRepository<EnergyModel> repository)
            : base(repository)
        {
        }
    }
}