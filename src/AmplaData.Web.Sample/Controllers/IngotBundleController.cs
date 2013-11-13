using System.Linq;
using System.Web.Mvc;
using AmplaData.Data;

using AmplaData.Web.Controllers;
using AmplaData.Web.Sample.Models;

namespace AmplaData.Web.Sample.Controllers
{
    public class IngotBundleController : RepositoryController<IngotBundleModel>
    {
        public IngotBundleController(IRepository<IngotBundleModel> repository) : base(repository)
        {
            
        }

        public ActionResult EditCast(IngotCastModel cast)
        {
            if (cast == null)
            {
                return HttpNotFound();
            }
            IngotCastWithBundlesModel model = new IngotCastWithBundlesModel(cast);
            FilterValue filter = new FilterValue("CastNo", cast.CastNo);
            model.Bundles = Repository.FindByFilter(filter).ToList();
            return View("Index",model.Bundles);
        }

    }
}