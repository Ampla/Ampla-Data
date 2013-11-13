using AmplaData.Data;
using AmplaData.Data.Controllers;
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