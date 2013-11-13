using System;
using System.Collections.Specialized;
using System.Web;
using AmplaData.Data.Sessions;
using AmplaData.Data.Web.Interfaces;
using AmplaData.Security.Authentication;
using AmplaData.Security.Authentication.Forms;

namespace AmplaData.Security.Sessions
{
    /// <summary>
    ///     Session Mapper class for automatic logging in sessions from the query string using http://localhost/?amplaSession={session}
    /// </summary>
    public class LoginAmplaSessionUsingQueryString : ISessionCommand
    {
        private readonly IHttpRequestWrapper requestWrapper;
        private readonly IHttpResponseWrapper responseWrapper;
        private readonly IAmplaUserService amplaUserService;
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IAmplaSessionStorage amplaSessionStorage;

        public LoginAmplaSessionUsingQueryString(IHttpRequestWrapper requestWrapper, IHttpResponseWrapper responseWrapper, IAmplaUserService amplaUserService, IFormsAuthenticationService formsAuthenticationService, IAmplaSessionStorage amplaSessionStorage)
        {
            this.requestWrapper = requestWrapper;
            this.responseWrapper = responseWrapper;
            this.amplaUserService = amplaUserService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.amplaSessionStorage = amplaSessionStorage;
        }

        /// <summary>
        /// Login the session if possible
        /// </summary>
        public void Execute()
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
                        amplaSessionStorage.SetAmplaSession(amplaUser.Session);

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