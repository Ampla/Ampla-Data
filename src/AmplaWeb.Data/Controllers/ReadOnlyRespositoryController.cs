using System.Web.Mvc;
using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;

namespace AmplaWeb.Sample.Controllers
{
    public abstract class ReadOnlyRepositoryController<TModel> : BootstrapBaseController where TModel : class, new()
    {
        protected ReadOnlyRepositoryController(IRepositorySet repositorySet)
        {
            Repository = repositorySet.GetReadOnlyRepository<TModel>();
        }

        protected IReadOnlyRepository<TModel> Repository { get; private set; }

        /// <summary>
        ///     GET /{Model}/
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(Repository.GetAll());
        }

        /// <summary>
        ///     GET /{Model}/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            TModel model = Repository.FindById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
    }

}