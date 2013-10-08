using System;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    /// <summary>
    /// Wrapper class for the HttpRequest
    /// </summary>
    public class AmplaHttpRequestWrapper : IHttpRequestWrapper
    {
        private readonly HttpRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaHttpRequestWrapper"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public AmplaHttpRequestWrapper(HttpRequest request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the query string for the request
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public NameValueCollection QueryString
        {
            get { return request.QueryString; }
        }

        /// <summary>
        /// Gets the URL for the request
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public Uri Url
        {
            get { return request.Url; }
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        public HttpCookieCollection Cookies
        {
            get { return request.Cookies; }
        }
    }
}