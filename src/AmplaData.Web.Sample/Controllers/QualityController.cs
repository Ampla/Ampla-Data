using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class QualityController : RepositoryController<QualityModel>
    {
        public QualityController(IRepository<QualityModel> repository)
            : base(repository)
        {
        }
    }
}