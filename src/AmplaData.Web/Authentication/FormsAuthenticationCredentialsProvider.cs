using System.Web.Security;
using AmplaData.Data.AmplaData2008;
using AmplaData.Web.Authentication.Forms;

namespace AmplaData.Web.Authentication
{
    public class FormsAuthenticationCredentialsProvider : ICredentialsProvider
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public FormsAuthenticationCredentialsProvider(IFormsAuthenticationService formsAuthenticationService)
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