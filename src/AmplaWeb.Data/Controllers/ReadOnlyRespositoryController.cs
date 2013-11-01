using System.Web.Mvc;
using AmplaWeb.Data.Records;

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
            return View("Index", Repository.GetAll());
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
            return View("Details", model);
        }

        /// <summary>
        ///     GET /{Model}/Record/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Record(int id = 0)
        {
            AmplaRecord model = Repository.FindRecord(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Record", model);
        }

        /// <summary>
        ///     GET /{Model}/History/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult History(int id = 0)
        {
            AmplaAuditRecord record = Repository.GetHistory(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View("History", record);
        }
    }
}