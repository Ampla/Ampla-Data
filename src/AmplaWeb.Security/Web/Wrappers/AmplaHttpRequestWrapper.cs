using System;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Security.Web.Interfaces;

namespace AmplaWeb.Security.Web.Wrappers
{
    public class AmplaHttpRequestWrapper : IHttpRequestWrapper
    {
        private readonly HttpRequest request;

        public AmplaHttpRequestWrapper(HttpRequest request)
        {
            this.request = request;
        }

        public NameValueCollection QueryString
        {
            get { return request.QueryString; }
        }

        public Uri Url
        {
            get { return request.Url; }
        }

        public HttpCookieCollection Cookies
        {
            get { return request.Cookies; }
        }
    }
}