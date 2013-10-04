using System.Web.SessionState;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    public class AmplaHttpSessionWrapper : IHttpSessionWrapper
    {
        private readonly HttpSessionState httpSessionState;

        public AmplaHttpSessionWrapper(HttpSessionState httpSessionState)
        {
            this.httpSessionState = httpSessionState;
        }

        public void SetValue(string key, object value)
        {
            httpSessionState[key] = value;
        }

        public object GetValue(string key)
        {
            return httpSessionState[key];
        }
    }
}