namespace AmplaWeb.Data.Web.Interfaces
{
    /// <summary>
    /// Interface that wraps the HttpSession object to simplify testing
    /// </summary>
    public interface IHttpSessionWrapper
    {
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