
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
        AmplaUser SimpleLogin(string userName, string password, out string message);

        /// <summary>
        ///     Login an Ampla user using session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        AmplaUser SessionLogin(string session, out string message);

        /// <summary>
        ///     Login using Integrated Security
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        AmplaUser IntegratedLogin(out string message);
        
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
    }
}