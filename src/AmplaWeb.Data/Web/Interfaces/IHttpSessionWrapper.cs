namespace AmplaData.Data.Web.Interfaces
{
    /// <summary>
    /// Interface that wraps the HttpSession object to simplify testing
    /// </summary>
    public interface IHttpSessionWrapper
    {
        /// <summary>
        /// Gets a value indicating whether the session is [enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetValue(string key, object value);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object GetValue(string key);
    }
}