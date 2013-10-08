
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    /// <summary>
    ///     HttpContext that will support the HttpRequest, HttpResponse, HttpSession wrappers.
    ///     Note that the Request.Redirect will create a new Context for the redirect.
    /// </summary>
    public class SimpleHttpContext 
    {
        public class SimpleHttpRequest : IHttpRequestWrapper
        {
            public SimpleHttpRequest(string requestUrl, HttpCookieCollection cookies)
            {
                Url = new Uri(requestUrl);
                QueryString = HttpUtility.ParseQueryString(Url.Query);
                Cookies = cookies;
            }

            public NameValueCollection QueryString { get; private set; }
            public Uri Url { get; private set; }
            public HttpCookieCollection Cookies { get; private set; }
        }

        public class SimpleHttpResponse : IHttpResponseWrapper
        {
            private readonly Action<string> redirectFunc;

            public SimpleHttpResponse(Action<string> redirectFunc)
            {
                this.redirectFunc = redirectFunc;
                Cookies = new HttpCookieCollection();
            }

            public HttpCookieCollection Cookies { get; private set; }
            public void Redirect(string url)
            {
                redirectFunc(url);
            }
        }

        public class SimpleHttpSession : IHttpSessionWrapper
        {
            private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();
            /// <summary>
            /// Sets the value.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public void SetValue(string key, object value)
            {
                dictionary[key] = value;
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public object GetValue(string key)
            {
                object value;
                dictionary.TryGetValue(key, out value);
                return value;
            }
        }

        public SimpleHttpContext(string requestUrl)
        {
            request = new SimpleHttpRequest(requestUrl,new HttpCookieCollection());
            response = new SimpleHttpResponse(Redirect);
            session = new SimpleHttpSession();
        }

        public IHttpRequestWrapper Request
        {
            get { return request; }
        }

        public IHttpResponseWrapper Response
        {
            get { return response; }
        }

        public IHttpSessionWrapper Session
        {
            get { return session; }
        }

        private SimpleHttpRequest request;
        private SimpleHttpResponse response;
        private readonly SimpleHttpSession session;

        /// <summary>
        /// Redirects the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void Redirect(string url)
        {
            request = new SimpleHttpRequest(url, response.Cookies);
            response = new SimpleHttpResponse(Redirect);
        }
    }
}