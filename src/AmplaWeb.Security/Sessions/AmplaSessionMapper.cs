using System;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Data.Web.Interfaces;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Authentication.Forms;

namespace AmplaWeb.Security.Sessions
{
    /// <summary>
    ///     Session Mapper class for automatic logging in sessions from the query string using http://localhost/?amplaSession={session}
    /// </summary>
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

        /// <summary>
        /// Login the session if possible
        /// </summary>
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