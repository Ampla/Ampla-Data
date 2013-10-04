using System;
using System.Web;
using System.Web.Security;
using AmplaWeb.Data.Session;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Security.Authentication.Forms
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IHttpRequestWrapper request;
        private readonly IHttpResponseWrapper response;
        private readonly IAmplaSessionStorage amplaSessionStorage;

        public FormsAuthenticationService(IHttpRequestWrapper request, IHttpResponseWrapper response, IAmplaSessionStorage amplaSessionStorage)
        {
            this.request = request;
            this.response = response;
            this.amplaSessionStorage = amplaSessionStorage;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            amplaSessionStorage.SetAmplaSession(null);
        }

        public void SessionExpired()
        {
            FormsAuthentication.SignOut();
            amplaSessionStorage.SetAmplaSession(null);
            string url = request.Url.ToString();
            response.Redirect(url);
        }

        public void StoreUserTicket(AmplaUser amplaUser, bool createPersistentCookie)
        {
            string session = amplaUser.Session;
            amplaSessionStorage.SetAmplaSession(session);

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