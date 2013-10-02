using System.Web.Mvc;

namespace AmplaWeb.Data.Controllers
{
    [Authorize]
    public abstract class ReadOnlyRepositoryController<TModel> : BootstrapBaseController where TModel : class, new()
    {
        protected ReadOnlyRepositoryController(IReadOnlyRepository<TModel> repository)
        {
            Repository = repository;
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