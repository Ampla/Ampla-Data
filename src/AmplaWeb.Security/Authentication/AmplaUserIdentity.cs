using System.Security.Principal;
using System.Web.Security;

namespace AmplaWeb.Security.Authentication
{
    public class AmplaUserIdentity : IIdentity, IPrincipal
    {
        public AmplaUserIdentity(FormsAuthenticationTicket authenticationTicket)
        {
            Name = authenticationTicket.Name;
            Session = authenticationTicket.UserData;
        }

        public string Name { get; private set; }

        public string Session { get; private set; }

        public string AuthenticationType { get { return "User"; } }

        public bool IsAuthenticated { get { return true; } }
        
        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }

        public IIdentity Identity { get { return this; }}
    }
}