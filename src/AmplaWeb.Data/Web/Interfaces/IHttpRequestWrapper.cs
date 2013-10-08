using System;
using System.Collections.Specialized;
using System.Web;

namespace AmplaWeb.Data.Web.Interfaces
{
    /// <summary>
    /// Interface that wraps the HttpRequest object to simplify testing
    /// </summary>
    public interface IHttpRequestWrapper
    {
        /// <summary>
        /// Gets the query string for the request
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        NameValueCollection QueryString { get; }

        /// <summary>
        /// Gets the URL for the request
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        Uri Url { get; }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        HttpCookieCollection Cookies { get; }

        /// <summary>
        /// Gets a value indicating whether the request has been [authenticated].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is authenticated]; otherwise, <c>false</c>.
        /// </value>
        bool IsAuthenticated { get; }
    }
}