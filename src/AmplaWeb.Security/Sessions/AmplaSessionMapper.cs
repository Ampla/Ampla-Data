using System;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Membership;
using AmplaWeb.Security.Web.Interfaces;

namespace AmplaWeb.Security.Sessions
{
    public class AmplaSessionMapper : ISessionMapper
    {
        private readonly IHttpRequestWrapper requestWrapper;
        private readonly IHttpResponseWrapper responseWrapper;
        private readonly IAmplaUserService amplaUserService;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AmplaSessionMapper(IHttpRequestWrapper requestWrapper, IHttpResponseWrapper responseWrapper, IAmplaUserService amplaUserService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.requestWrapper = requestWrapper;
            this.responseWrapper = responseWrapper;
            this.amplaUserService = amplaUserService;
            this.formsAuthenticationService = formsAuthenticationService;
        }
        
        public void Login()
        {
            NameValueCollection queryString = requestWrapper.QueryString;

            string amplaSession = queryString["amplaSession"];
            if (!string.IsNullOrEmpty(amplaSession))
            {
                if (amplaUserService != null)
                {
                    string message;
                    AmplaUser amplaUser = amplaUserService.SessionLogin(amplaSession, out message);
                    if (amplaUser != null)
                    {
                        formsAuthenticationService.StoreUserTicket(amplaUser, false);

                        UriBuilder builder = new UriBuilder(requestWrapper.Url);
                        var query = HttpUtility.ParseQueryString(builder.Query);
                        query.Remove("amplaSession");
                        builder.Query = query.ToString();
                        responseWrapper.Redirect(builder.ToString());
                    }
                }

            }
        }
    }
}