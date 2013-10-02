using System.Web.Mvc;
using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class IngotCastController : RepositoryController<IngotCastModel>
    {
        public IngotCastController(IRepository<IngotCastModel> repository) : base(repository)
        {
        }

        /// <summary>
        ///     GET /IngotCast/Select
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            return View(Repository.GetAll());
        }

        public ActionResult Bundles(int id)
        {
            IngotCastModel cast = Repository.FindById(id);
            if (cast == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("EditCast", "IngotBundle", new {cast.CastNo});

        }

        public ActionResult Default()
        {
            return Index();
        }
    }
}