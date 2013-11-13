using System.Web.Mvc;
using AmplaData.Data;
using AmplaData.Data.Records;

namespace AmplaData.Web.Controllers
{
    [Authorize]
    public class RepositoryController<TModel> : BootstrapBaseController, IRepositoryController<TModel> where TModel : class, new()
    {
        protected RepositoryController(IRepository<TModel> repository)
        {
            Repository = repository;
        }

        protected IRepository<TModel> Repository { get; private set; }

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

        /// <summary>
        ///     GET /{Model}/Versions/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Versions(int id = 0)
        {
            ModelVersions versions = Repository.GetVersions(id);
            if (versions == null )
            {
                return HttpNotFound();
            }
            return View("Versions", versions);
        }

        /// <summary>
        ///     GET /{Model}/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            TModel model = new TModel();
            return View("Create", model);
        }

        /// <summary>
        ///     POST /{Model}/Create
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(TModel model)
        {
            if (ModelState.IsValid)
            {
                Repository.Add(model);
                Success("Your information was saved!");
                return RedirectToAction("Index");
            }

            Error("There were some errors in your form.");
            return View("Create", model);
        }

        /// <summary>
        ///     Get /{Model}/Edit/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            TModel model = Repository.FindById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Edit", model);
        }

        /// <summary>
        ///     POST /{Model}/Edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(TModel model)
        {
            if (ModelState.IsValid)
            {
                Repository.Update(model);

                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }

        /// <summary>
        ///     GET /{Model}/Delete/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            TModel model = Repository.FindById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Delete", model);
        }

        /// <summary>
        ///     POST /{Model}/Delete/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TModel model = Repository.FindById(id);
            Repository.Delete(model);
            
            return RedirectToAction("Index");
        }
    }

}