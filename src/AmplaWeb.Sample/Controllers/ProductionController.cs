using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class ProductionController : RepositoryController<ProductionModel>
    {
        public ProductionController(IRepository<ProductionModel> repository) : base(repository)
        {
        }
    }
}