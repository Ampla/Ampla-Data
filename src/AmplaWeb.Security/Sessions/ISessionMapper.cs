namespace AmplaWeb.Security.Sessions
{
    /// <summary>
    ///     Interface for automatically logging in sessions from the query string
    /// </summary>
    public interface ISessionMapper
    {
        /// <summary>
        /// Login the session if possible
        /// </summary>
        void Login();
    }
}