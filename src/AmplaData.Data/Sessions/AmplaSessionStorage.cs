using System;
using AmplaData.Data.Web.Interfaces;

namespace AmplaData.Data.Sessions
{
    /// <summary>
    ///     AmplaSessionStorage in the HttpSession
    /// </summary>
    public class AmplaSessionStorage : IAmplaSessionStorage
    {
        private const string amplaSessionKey = "__AmplaSession";
        private readonly IHttpSessionWrapper httpSessionWrapper;

        public AmplaSessionStorage(IHttpSessionWrapper httpSessionWrapper)
        {
            this.httpSessionWrapper = httpSessionWrapper;
        }

        /// <summary>
        /// Gets a value indicating whether the session storage is [enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get { return httpSessionWrapper.Enabled; }}

        /// <summary>
        /// Sets the ampla session.
        /// </summary>
        /// <param name="value">
        ///     The ampla session.
        /// </param>
        public void SetAmplaSession(string value)
        {
            httpSessionWrapper.SetValue(amplaSessionKey, value);
        }

        /// <summary>
        /// Gets the ampla session.
        /// </summary>
        /// <returns>
        /// The ampla session.
        /// </returns>
        public string GetAmplaSession()
        {
            object value = httpSessionWrapper.GetValue(amplaSessionKey);
            return Convert.ToString(value);
        }
    }
}