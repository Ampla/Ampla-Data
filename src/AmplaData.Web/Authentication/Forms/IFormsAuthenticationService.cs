using System.Web.Security;

namespace AmplaData.Web.Authentication.Forms
{
    public interface IFormsAuthenticationService
    {
        void SignOut();

        void SessionExpired();

        void StoreUserTicket(AmplaUser amplaUser, bool createPersistentCookie);
        FormsAuthenticationTicket GetUserTicket();
    }
}