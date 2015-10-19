using System.Web.Mvc;

namespace AmplaData.Web.Controllers
{
    /// <summary>
    ///     Interface for Repository operations for the model 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IReadOnlyRepositoryController<in TModel>
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

   }
}