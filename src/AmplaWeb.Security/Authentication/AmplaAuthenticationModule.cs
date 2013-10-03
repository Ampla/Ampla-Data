using System;
using System.Web;
using System.Web.Mvc;
using AmplaWeb.Security.Sessions;


namespace AmplaWeb.Security.Authentication
{
    public class AmplaAuthenticationModule
    {
        public void Initialize(HttpApplication httpApplication)
        {
            httpApplication.AuthenticateRequest += AuthenticateRequest;
        }

        private void AuthenticateRequest(object sender, EventArgs eventArgs)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                var sessionMapper = DependencyResolver.Current.GetService<ISessionMapper>();

                if (sessionMapper != null)
                {
                    sessionMapper.Login();
                }
            }
        }
    }
}