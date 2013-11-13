using System;
using System.Collections.Specialized;
using System.Web;

namespace AmplaData.Web.Wrappers
{
    public class SimpleHttpRequest : IHttpRequestWrapper, IDisposable
    {
        private bool disposed;
        private readonly bool isAuthenticated;
        private readonly HttpCookieCollection cookies;
        private readonly Uri url;
        private readonly NameValueCollection queryString;

        public static SimpleHttpRequest Create(string requestUrl, HttpCookieCollection cookies, bool isAuthenticated)
        {
            return new SimpleHttpRequest(requestUrl, cookies, isAuthenticated);
        }

        private SimpleHttpRequest(string requestUrl, HttpCookieCollection cookies, bool isAuthenticated)
        {
            this.isAuthenticated = isAuthenticated;
            url = new Uri(requestUrl);
            queryString = HttpUtility.ParseQueryString(Url.Query);
            this.cookies = cookies;
            disposed = false;
        }

        public NameValueCollection QueryString
        {
            get
            {
                CheckDisposed();
                return queryString;
            }
        }

        public Uri Url
        {
            get
            {
                CheckDisposed();
                return url;
            }

        }

        public HttpCookieCollection Cookies
        {
            get
            {
                CheckDisposed();
                return cookies;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                CheckDisposed();
                return isAuthenticated;
            }
        }

        public void Dispose()
        {
            disposed = true;
        }
        private void CheckDisposed()
        {
            if (disposed) throw new ObjectDisposedException("HttpRequest");
        }
    }
}