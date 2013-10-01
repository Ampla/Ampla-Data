using AmplaWeb.Security.Membership;

namespace AmplaWeb.Security.Authentication
{
    /// <summary>
    ///     Interface to allow access to Ampla Users
    /// </summary>
    public interface IAmplaUserService
    {
        /// <summary>
        ///     Login an Ampla users using username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        AmplaUser Login(string userName, string password, out string message);
        
        /// <summary>
        ///     Renew the session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        AmplaUser RenewSession(string session);
        
        /// <summary>
        ///     Logout the user
        /// </summary>
        /// <param name="userName"></param>
        void Logout(string userName);

        /// <summary>
        ///     Gets an array of currently logged in users
        /// </summary>
        /// <returns></returns>
        string[] GetUsers();

        /// <summary>
        ///     Gets the already logged in user specified by the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AmplaUser GetLoggedInUser(string name);
    }
}