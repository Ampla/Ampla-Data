using System;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    public class SimpleHttpContext : IHttpRequestWrapper, IHttpResponseWrapper
    {
        public SimpleHttpContext(string requestUrl)
        {
            request = new SimpleHttpRequest(requestUrl, null);
            response = new SimpleHttpResponse();
        }

        private SimpleHttpRequest request;
        private SimpleHttpResponse response;

        public NameValueCollection QueryString
        {
            get { return request.QueryString; }
        }

        public Uri Url
        {
            get { return request.Url; }
        }

        public HttpCookieCollection RequestCookies
        {
            get { return request.Cookies; }
        }

        public HttpCookieCollection ResponseCookies
        {
            get { return response.Cookies; }
        }

        HttpCookieCollection IHttpRequestWrapper.Cookies
        {
            get { return request.Cookies; }
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
            request = new SimpleHttpRequest(url, response.Cookies);
            response = new SimpleHttpResponse();
        }
    }
}