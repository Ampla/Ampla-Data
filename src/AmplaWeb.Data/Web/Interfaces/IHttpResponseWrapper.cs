using System.Web;

namespace AmplaWeb.Data.Web.Interfaces
{
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