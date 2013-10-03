using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using AmplaWeb.Security.Membership;
using AmplaWeb.Security.Web.Interfaces;

namespace AmplaWeb.Security.Authentication
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IHttpRequestWrapper request;
        private readonly IHttpResponseWrapper response;

        public FormsAuthenticationService(IHttpRequestWrapper request, IHttpResponseWrapper response)
        {
            this.request = request;
            this.response = response;
        }

        public void SignIn(AmplaUser amplaUser, bool createPersistentCookie)
        {
            if (amplaUser == null) throw new ArgumentNullException("amplaUser");
            FormsAuthentication.SetAuthCookie(amplaUser.UserName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void StoreUserTicket(AmplaUser amplaUser, bool createPersistentCookie)
        {
            string session = amplaUser.Session;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, amplaUser.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), createPersistentCookie, session);

            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
        }

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

        public void SetCurrentUser(IPrincipal principal)
        {
            HttpContext.Current.User = principal;
        }
    }
}