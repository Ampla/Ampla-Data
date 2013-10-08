
namespace AmplaWeb.Data.Sessions
{
    /// <summary>
    /// Interface for storing the session
    /// </summary>
    public interface IAmplaSessionStorage
    {
        /// <summary>
        /// Gets a value indicating whether the session storage is [enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; }

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