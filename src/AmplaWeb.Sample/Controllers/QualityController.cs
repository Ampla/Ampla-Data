using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class QualityController : RepositoryController<QualityModel>
    {
        public QualityController(IRepository<QualityModel> repository)
            : base(repository)
        {
        }
    }
}