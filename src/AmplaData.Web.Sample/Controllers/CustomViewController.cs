using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class CustomViewController : ReadOnlyRepositoryController<CustomViewModel>
    {
        public CustomViewController(IRepository<CustomViewModel> repository)
            : base(repository)
        {
        }
    }
}