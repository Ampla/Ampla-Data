using System.Web.Security;
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Security.Authentication
{
    public class AmplaCredentialsProvider : ICredentialsProvider
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AmplaCredentialsProvider(IFormsAuthenticationService formsAuthenticationService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
        }

        public Credentials GetCredentials()
        {
            FormsAuthenticationTicket ticket = formsAuthenticationService.GetUserTicket();
            if (ticket != null)
            {
                return CredentialsProvider.ForSession(ticket.UserData).GetCredentials();
            }
            return null;
        }
    }
}