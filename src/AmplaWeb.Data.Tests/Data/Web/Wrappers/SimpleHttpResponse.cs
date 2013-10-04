using System.Web;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    public class SimpleHttpResponse : IHttpResponseWrapper
    {
        public SimpleHttpResponse()
        {
            Cookies = new HttpCookieCollection();
        }

        public string Url { get; private set; }

        public HttpCookieCollection Cookies { get; private set; }
        public void Redirect(string url)
        {
            Url = url;
        }
    }
}