using System.Linq;
using System.Web.Mvc;
using AmplaWeb.Data;
using AmplaWeb.Data.AmplaRespository;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class IngotBundleController : RepositoryController<IngotBundleModel>
    {
        public IngotBundleController(IRepositorySet repositorySet)
            : base(repositorySet)
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