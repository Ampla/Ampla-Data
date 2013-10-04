using System.Web.SessionState;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    public class AmplaHttpSessionWrapper : IHttpSessionWrapper
    {
        private readonly IHttpSessionState httpSessionState;

        public AmplaHttpSessionWrapper(IHttpSessionState httpSessionState)
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