using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace AmplaWeb.Security.Authentication
{
    public class AmplaAuthorizationModule
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AmplaAuthorizationModule() : this(new FormsAuthenticationService())
        {
        }

        public AmplaAuthorizationModule(IFormsAuthenticationService formsAuthenticationService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
        }

        public void Initialize(HttpApplication httpApplication)
        {
            httpApplication.PostAuthorizeRequest += PostAuthoriseRequest;
        }

        private void PostAuthoriseRequest(object sender, EventArgs e)
        {
            FormsAuthenticationTicket ticket = formsAuthenticationService.GetUserTicket();
            if (ticket != null)
            {
                AmplaUserIdentity amplaIdentity = new AmplaUserIdentity(ticket);
                string[] roles = Roles.GetRolesForUser(amplaIdentity.Name);
                formsAuthenticationService.SetCurrentUser(new GenericPrincipal(amplaIdentity, roles));
            }
        }
    }
}