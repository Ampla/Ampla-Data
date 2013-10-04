using System.Web.Security;

namespace AmplaWeb.Security.Authentication
{
    public interface IFormsAuthenticationService
    {
        void SignIn(AmplaUser amplaUser, bool createPersistentCookie);
        void SignOut();

        void SessionExpired();

        void StoreUserTicket(AmplaUser amplaUser, bool createPersistentCookie);
        FormsAuthenticationTicket GetUserTicket();
    }
}