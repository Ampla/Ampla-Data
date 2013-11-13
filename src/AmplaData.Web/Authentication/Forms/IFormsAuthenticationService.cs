using System.Web.Security;

namespace AmplaData.Web.Authentication.Forms
{
    /// <summary>
    ///     Service that wraps the FormsAuthenicationService
    /// </summary>
    public interface IFormsAuthenticationService
    {
        /// <summary>
        ///     Sign out of FormsAuthentication
        /// </summary>
        void SignOut();

        /// <summary>
        ///     Expire the Session (and Ticket the ticket)
        /// </summary>
        void SessionExpired();

        /// <summary>
        /// Stores the user ticket.
        /// </summary>
        /// <param name="amplaUser">The ampla user.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        void StoreUserTicket(AmplaUser amplaUser, bool createPersistentCookie);

        /// <summary>
        /// Gets the user ticket.
        /// </summary>
        /// <returns></returns>
        FormsAuthenticationTicket GetUserTicket();
    }
}