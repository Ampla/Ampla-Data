using System.Web.SessionState;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    /// <summary>
    /// Wrapper class for the HttpSession
    /// </summary>
    public class AmplaHttpSessionWrapper : IHttpSessionWrapper
    {
        private readonly HttpSessionState httpSessionState;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaHttpSessionWrapper"/> class.
        /// </summary>
        /// <param name="httpSessionState">State of the HTTP session.</param>
        public AmplaHttpSessionWrapper(HttpSessionState httpSessionState)
        {
            this.httpSessionState = httpSessionState;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, object value)
        {
            httpSessionState[key] = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            return httpSessionState[key];
        }
    }
}