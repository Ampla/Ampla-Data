using AmplaWeb.Data.Sessions;
using AmplaWeb.Data.Web.Interfaces;
using AmplaWeb.Security.Authentication.Forms;

namespace AmplaWeb.Security.Sessions
{
    /// <summary>
    /// Align the Ampla session stored in the HttpSession with the Forms Authentication Ticket
    /// </summary>
    public class AlignSessionWithFormsAuthentication : ISessionCommand
    {
        private readonly IHttpRequestWrapper httpRequest;
        private readonly IAmplaSessionStorage amplaSessionStorage;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignSessionWithFormsAuthentication"/> class.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="amplaSessionStorage">The ampla session storage.</param>
        /// <param name="formsAuthenticationService">The forms authentication service.</param>
        public AlignSessionWithFormsAuthentication(IHttpRequestWrapper httpRequest, IAmplaSessionStorage amplaSessionStorage, IFormsAuthenticationService formsAuthenticationService)
        {
            this.httpRequest = httpRequest;
            this.amplaSessionStorage = amplaSessionStorage;
            this.formsAuthenticationService = formsAuthenticationService;
        }

        /// <summary>
        /// Make sure the Session is aligned.
        /// </summary>
        public void Execute()
        {
            if (httpRequest.IsAuthenticated)
            {
                if (amplaSessionStorage.Enabled)
                {
                    string session = amplaSessionStorage.GetAmplaSession();
                    if (string.IsNullOrEmpty(session))
                    {
                        var ticket = formsAuthenticationService.GetUserTicket();
                        if (ticket != null)
                        {
                            string formsSession = ticket.UserData;
                            if (!string.IsNullOrEmpty(formsSession))
                            {
                                amplaSessionStorage.SetAmplaSession(formsSession);
                            }
                        }
                    }
                }
            }
        }
    }
}