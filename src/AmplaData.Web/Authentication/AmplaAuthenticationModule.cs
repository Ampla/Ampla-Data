using System;
using System.Web;
using System.Web.Mvc;
using AmplaData.Web.Sessions;

namespace AmplaData.Web.Authentication
{
    /// <summary>
    /// Ampla Authentication Module to hookup events on the HttpApplication
    /// </summary>
    public class AmplaAuthenticationModule
    {
        /// <summary>
        /// Initializes the specified HTTP application.
        /// </summary>
        /// <param name="httpApplication">The HTTP application.</param>
        public void Initialize(HttpApplication httpApplication)
        {
            httpApplication.AuthenticateRequest += AuthenticateRequest;
            httpApplication.PostAcquireRequestState += PostAcquireRequestState;
        }

        /// <summary>
        ///     Ensure the Session is aligned with FormsAuthentication
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PostAcquireRequestState(object sender, EventArgs e)
        {
            AlignSessionWithFormsAuthentication ensureAlignedSession = DependencyResolver.Current.GetService<AlignSessionWithFormsAuthentication>();
            ensureAlignedSession.Execute();
        }

        /// <summary>
        ///     When the Authenticate Request is complete check for AmplaSession in the Query string
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
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