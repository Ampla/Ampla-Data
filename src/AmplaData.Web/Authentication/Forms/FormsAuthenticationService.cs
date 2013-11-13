using System;
using System.Web;
using System.Web.Security;
using AmplaData.Web.Wrappers;

namespace AmplaData.Web.Authentication.Forms
{
    /// <summary>
    /// Service that wraps the FormsAuthenicationService
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IHttpRequestWrapper request;
        private readonly IHttpResponseWrapper response;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsAuthenticationService"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        public FormsAuthenticationService(IHttpRequestWrapper request, IHttpResponseWrapper response)
        {
            this.request = request;
            this.response = response;
        }

        /// <summary>
        /// Sign out of FormsAuthentication
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Expire the Session (and Ticket the ticket)
        /// </summary>
        public void SessionExpired()
        {
            FormsAuthentication.SignOut();
            string url = request.Url.ToString();
            response.Redirect(url);
        }

        /// <summary>
        /// Stores the user ticket.
        /// </summary>
        /// <param name="amplaUser">The ampla user.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        public void StoreUserTicket(AmplaUser amplaUser, bool createPersistentCookie)
        {
            string session = amplaUser.Session;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, amplaUser.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), createPersistentCookie, session);
            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
        }

        /// <summary>
        /// Gets the user ticket.
        /// </summary>
        /// <returns></returns>
        public FormsAuthenticationTicket GetUserTicket()
        {
            var authCookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                string encTicket = authCookie.Value;
                if (!String.IsNullOrEmpty(encTicket))
                {
                    return FormsAuthentication.Decrypt(encTicket);
                }
            }
            return null;
        }
    }
}