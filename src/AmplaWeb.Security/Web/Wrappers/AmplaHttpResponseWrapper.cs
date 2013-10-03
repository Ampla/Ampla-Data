using System.Web;
using AmplaWeb.Security.Web.Interfaces;

namespace AmplaWeb.Security.Web.Wrappers
{
    /// <summary>
    /// Wrapper class for the HttpResponse object
    /// </summary>
    public class AmplaHttpResponseWrapper : IHttpResponseWrapper
    {
        private readonly HttpResponse response;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaHttpResponseWrapper"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        public AmplaHttpResponseWrapper(HttpResponse response)
        {
            this.response = response;
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        public HttpCookieCollection Cookies 
        {
            get
            {
                return response.Cookies;
            }
        }

        /// <summary>
        /// Redirects the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void Redirect(string url)
        {
            response.Redirect(url, true);
        }
    }
}