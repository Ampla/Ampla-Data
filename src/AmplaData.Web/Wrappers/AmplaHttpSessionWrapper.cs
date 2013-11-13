using System.Web.SessionState;

namespace AmplaData.Web.Wrappers
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
        /// Gets a value indicating whether the session is [enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled
        {
            get { return httpSessionState != null; }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, object value)
        {
            if (httpSessionState != null)
            {
                httpSessionState[key] = value;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            if (httpSessionState != null)
            {
                return httpSessionState[key];
            }
            return null;
        }
    }
}