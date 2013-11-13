using AmplaData.Data;
using AmplaData.Data.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class ProductionController : RepositoryController<ProductionModel>
    {
        public ProductionController(IRepository<ProductionModel> repository) : base(repository)
        {
        }
    }
}