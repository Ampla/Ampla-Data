using System;
using System.Collections.Specialized;
using System.Web;

namespace AmplaWeb.Data.Web.Interfaces
{
    public interface IHttpRequestWrapper
    {
        NameValueCollection QueryString { get; }

        Uri Url { get; }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        HttpCookieCollection Cookies { get; }
    }
}