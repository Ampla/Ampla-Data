using System.Web.Mvc;

namespace AmplaData.Data.Controllers
{
    /// <summary>
    ///     Interface for Repository operations for the model 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IRepositoryController<in TModel>
    {
        /// <summary>
        ///     GET /{Model}/
        /// </summary>
        /// <returns></returns>
        ActionResult Index();

        /// <summary>
        ///     GET /{Model}/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Details(int id = 0);

        /// <summary>
        ///     GET /{Model}/Record/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Record(int id = 0);

        /// <summary>
        ///     GET /{Model}/Create
        /// </summary>
        /// <returns></returns>
        ActionResult Create();

        /// <summary>
        ///     POST /{Model}/Create
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        ActionResult Create(TModel model);

        /// <summary>
        ///     Get /{Model}/Edit/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Edit(int id = 0);

        /// <summary>
        ///     POST /{Model}/Edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        ActionResult Edit(TModel model);

        /// <summary>
        ///     GET /{Model}/Delete/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Delete(int id = 0);

        /// <summary>
        ///     POST /{Model}/Delete/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        ActionResult DeleteConfirmed(int id);
    }
}