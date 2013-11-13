using System.Collections.Generic;
using AmplaData.Data.Web.Interfaces;

namespace AmplaData.Data.Web.Wrappers
{
    public class SimpleHttpSession : IHttpSessionWrapper
    {
        private readonly bool enabled;
        private readonly Dictionary<string, object> dictionary;

        public static SimpleHttpSession Create(bool enabled)
        {
            return new SimpleHttpSession(enabled);
        }

        private SimpleHttpSession(bool enabled)
        {
            this.enabled = enabled;
            dictionary = enabled ? new Dictionary<string, object>() : null;
        }

        /// <summary>
        /// Gets a value indicating whether the session is [enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get { return enabled; } }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, object value)
        {
            if (dictionary != null)
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            object value = null;
            if (dictionary != null)
            {
                dictionary.TryGetValue(key, out value);
            }
            return value;
        }
    }
}