using AmplaData.Data;
using AmplaData.Data.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class ShiftLogController : RepositoryController<ShiftLogModel>
    {
        public ShiftLogController(IRepository<ShiftLogModel> repository) : base(repository)
        {
        }
    }
}