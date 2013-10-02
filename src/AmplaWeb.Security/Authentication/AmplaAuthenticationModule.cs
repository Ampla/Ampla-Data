using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AmplaWeb.Security.Membership;

namespace AmplaWeb.Security.Authentication
{
    public class AmplaAuthenticationModule
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AmplaAuthenticationModule() : this(new FormsAuthenticationService())
        {
        }

        public AmplaAuthenticationModule(IFormsAuthenticationService formsAuthenticationService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
        }

        public void Initialize(HttpApplication httpApplication)
        {
            httpApplication.AuthenticateRequest += AuthenticateRequest;
            httpApplication.PostAuthorizeRequest += PostAuthoriseRequest;
        }

        private void AuthenticateRequest(object sender, EventArgs eventArgs)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                string amplaSession = queryString["amplaSession"];
                if (!string.IsNullOrEmpty(amplaSession))
                {
                    IAmplaUserService amplaUserService = DependencyResolver.Current.GetService<IAmplaUserService>();
                    if (amplaUserService != null)
                    {
                        string message;
                        AmplaUser amplaUser = amplaUserService.SessionLogin(amplaSession, out message);
                        if (amplaUser != null)
                        {
                            formsAuthenticationService.StoreUserTicket(HttpContext.Current.Response.Cookies, amplaUser, false);
                            
                            UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url);
                            var query = HttpUtility.ParseQueryString(builder.Query);
                            query.Remove("amplaSession");
                            builder.Query = query.ToString();
                            HttpContext.Current.Response.Redirect(builder.ToString());
                        }
                    }
                    
                }
            }
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