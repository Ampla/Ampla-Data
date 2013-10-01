using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using AmplaWeb.Security.Membership;

namespace AmplaWeb.Security.Authentication
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(AmplaUser amplaUser, bool createPersistentCookie)
        {
            if (amplaUser == null) throw new ArgumentNullException("amplaUser");
            FormsAuthentication.SetAuthCookie(amplaUser.UserName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void StoreUserTicket(HttpResponse response, AmplaUser amplaUser)
        {
            string session = amplaUser.Session;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, amplaUser.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, session);
            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
        }

        public FormsAuthenticationTicket GetUserTicket()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
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

        public void SetCurrentUser(IPrincipal principal)
        {
            HttpContext.Current.User = principal;
        }
    }
}