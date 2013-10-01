using System.Security.Principal;
using System.Web;
using System.Web.Security;
using AmplaWeb.Security.Membership;

namespace AmplaWeb.Security.Authentication
{
    public interface IFormsAuthenticationService
    {
        void SignIn(AmplaUser amplaUser, bool createPersistentCookie);
        void SignOut();
        void StoreUserTicket(HttpResponse response, AmplaUser user);
        FormsAuthenticationTicket GetUserTicket();
        void SetCurrentUser(IPrincipal principal);
    }
}