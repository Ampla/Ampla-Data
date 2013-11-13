using System.Web;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    ///     Interface that wraps the HttpResponse object to simplify testing
    /// </summary>
    public interface IHttpResponseWrapper
    {
        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        HttpCookieCollection Cookies { get; }

        /// <summary>
        /// Redirects the specified automatic string.
        /// </summary>
        /// <param name="url">Url to redirect to.</param>
        void Redirect(string url);
    }
}