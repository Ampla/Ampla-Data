using System;
using System.Web;
using System.Web.Security;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Security.Authentication.Forms
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

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void SessionExpired()
        {
            FormsAuthentication.SignOut();
            string url = request.Url.ToString();
            response.Redirect(url);
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
    }
}