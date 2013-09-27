using System.Web.Mvc;

namespace AmplaWeb.Data.Controllers
{
    public class RepositoryController<TModel> : BootstrapBaseController, IRepositoryController<TModel> where TModel : class, new()
    {
        protected RepositoryController(IRepositorySet repositorySet)
        {
            Repository = repositorySet.GetRepository<TModel>();
        }

        protected IRepository<TModel> Repository { get; private set; }

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

        /// <summary>
        ///     GET /{Model}/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            TModel model = new TModel();
            return View(model);
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
            return View(model);
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
            return View(model);
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
            return View(model);
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
            return View(model);
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