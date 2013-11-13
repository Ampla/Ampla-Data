using System;
using System.Web;
using System.Web.Mvc;
using AmplaData.Security.Sessions;

namespace AmplaData.Security.Authentication
{
    public class AmplaAuthenticationModule
    {
        public void Initialize(HttpApplication httpApplication)
        {
            httpApplication.AuthenticateRequest += AuthenticateRequest;
            httpApplication.PostAcquireRequestState += PostAcquireRequestState;
        }

        private void PostAcquireRequestState(object sender, EventArgs e)
        {
            AlignSessionWithFormsAuthentication ensureAlignedSession = DependencyResolver.Current.GetService<AlignSessionWithFormsAuthentication>();
            ensureAlignedSession.Execute();
        }

        private void AuthenticateRequest(object sender, EventArgs eventArgs)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                var loginAmplaSession = DependencyResolver.Current.GetService<LoginAmplaSessionUsingQueryString>();

                if (loginAmplaSession != null)
                {
                    loginAmplaSession.Execute();
                }
            }
        }
    }
}