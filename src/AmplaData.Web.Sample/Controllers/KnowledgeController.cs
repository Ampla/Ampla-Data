using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class KnowledgeController : RepositoryController<KnowledgeModel>
    {
        public KnowledgeController(IRepository<KnowledgeModel> repository)
            : base(repository)
        {
        }
    }
}