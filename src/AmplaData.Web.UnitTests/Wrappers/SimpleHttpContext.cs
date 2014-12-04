using System;
using System.Web;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    ///     HttpContext that will support the HttpRequest, HttpResponse, HttpSession wrappers.
    ///     Note that the Request.Redirect will create a new Context for the redirect.
    /// </summary>
    /// <remarks>
    /// After a redirect, the previous request/response wrappers are disposed to ensure the unit tests are easy 
    /// </remarks>
    public class SimpleHttpContext
    {
        public static SimpleHttpContext Create(string requestUrl)
        {
            return new SimpleHttpContext(requestUrl);
        }

        private SimpleHttpContext(string requestUrl) 
        {
            request = SimpleHttpRequest.Create(requestUrl, new HttpCookieCollection(), true);
            response = SimpleHttpResponse.Create(Redirect);
            session = SimpleHttpSession.Create(true);
        }

        public SimpleHttpContext WithSessionsDisabled()
        {
            SimpleHttpContext newContext = new SimpleHttpContext(request.Url.ToString())
                {
                    Session = SimpleHttpSession.Create(false)
                };
            request.Dispose();
            response.Dispose();
            return newContext;
        }

        public SimpleHttpContext WithRequestsNotAuthenticated()
        {
            SimpleHttpContext newContext = new SimpleHttpContext(request.Url.ToString())
            {
                Session = SimpleHttpSession.Create(false),
                Request = SimpleHttpRequest.Create(request.Url.ToString(), request.Cookies, false)
            };
            request.Dispose();
            response.Dispose();
            return newContext;
        }

        public IHttpRequestWrapper Request
        {
            get
            {
                return request;
            }
            private set
            {
                request.Dispose();
                request = (SimpleHttpRequest) value;
            }
        }

        public IHttpResponseWrapper Response
        {
            get { return response; }
        }

        public IHttpSessionWrapper Session
        {
            get { return session; }
            private set { session = (SimpleHttpSession) value; }
        }

        private SimpleHttpRequest request;
        private SimpleHttpResponse response;
        private SimpleHttpSession session;

        /// <summary>
        /// Redirects the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void Redirect(string url)
        {
            IDisposable oldRequest = request;
            IDisposable oldResponse = response;

            request = SimpleHttpRequest.Create(url, response.Cookies, request.IsAuthenticated);
            response = SimpleHttpResponse.Create(Redirect);

            oldRequest.Dispose();
            oldResponse.Dispose();

        }
    }
}