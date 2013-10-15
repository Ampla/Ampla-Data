using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class KnowledgeController : RepositoryController<KnowledgeModel>
    {
        public KnowledgeController(IRepository<KnowledgeModel> repository)
            : base(repository)
        {
        }
    }
}