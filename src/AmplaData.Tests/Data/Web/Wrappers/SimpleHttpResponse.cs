using System;
using System.Web;
using AmplaData.Web.Wrappers;

namespace AmplaData.Data.Web.Wrappers
{
    public class SimpleHttpResponse : IHttpResponseWrapper, IDisposable
    {
        public static SimpleHttpResponse Create(Action<string> redirectFunc)
        {
            return new SimpleHttpResponse(redirectFunc);
        }

        private readonly Action<string> redirectFunc;
        private readonly HttpCookieCollection cookies;
        private bool disposed;

        private SimpleHttpResponse(Action<string> redirectFunc)
        {
            this.redirectFunc = redirectFunc;
            cookies = new HttpCookieCollection();
            disposed = false;
        }

        public HttpCookieCollection Cookies
        {
            get
            {
                CheckDisposed();
                return cookies;
            }
        }

        public void Redirect(string url)
        {
            CheckDisposed();
            redirectFunc(url);
        }

        private void CheckDisposed()
        {
            if (disposed) throw new ObjectDisposedException("HttpRequest");
        }

        public void Dispose()
        {
            disposed = true;
        }
    }
}