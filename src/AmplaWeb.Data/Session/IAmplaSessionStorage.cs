
namespace AmplaWeb.Data.Session
{
    /// <summary>
    /// Interface for storing the session
    /// </summary>
    public interface IAmplaSessionStorage
    {
        /// <summary>
        /// Sets the ampla session.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetAmplaSession(string value);

        /// <summary>
        /// Gets the ampla session.
        /// </summary>
        /// <returns></returns>
        string GetAmplaSession();
    }
}