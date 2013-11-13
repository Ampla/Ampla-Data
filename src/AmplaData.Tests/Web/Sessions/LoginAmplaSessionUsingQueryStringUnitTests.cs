using System;
using AmplaData.Data.AmplaSecurity2007;
using AmplaData.Web.Authentication;
using AmplaData.Web.Authentication.Forms;
using AmplaData.Web.Wrappers;
using NUnit.Framework;

namespace AmplaData.Web.Sessions
{
    [TestFixture]
    public class LoginAmplaSessionUsingQueryStringUnitTests : TestFixture
    {
        private SimpleHttpContext context;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            context = SimpleHttpContext.Create("http://localhost");
        }

        /// <summary>
        /// SessionStorage is created each time it is access to ensure it gets the current context.
        /// </summary>
        private IAmplaSessionStorage SessionStorage
        {
            get
            {
                return new AmplaSessionStorage(context.Session);
            }
        }

        /// <summary>
        /// FormsAuthentication Service is created each time to ensure it gets the the current context
        /// </summary>
        private IFormsAuthenticationService FormsAuthenticationService
        {
            get
            {
                return new FormsAuthenticationService(context.Request, context.Response);
            }
        }

         [Test]
         public void AutomaticLoginFromSession()
         {
             SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User"); 
             webServiceClient.AddExistingSession("User");

             string session = webServiceClient.Sessions[0].SessionId;
             string url = "http://localhost/?amplaSession=" + session;

             context = SimpleHttpContext.Create(url);

             IAmplaUserService amplaUserService = new AmplaUserService(webServiceClient, new AmplaUserStore());
             Assert.That(SessionStorage.GetAmplaSession(), Is.Empty);

             LoginAmplaSessionUsingQueryString loginAmplaSession = new LoginAmplaSessionUsingQueryString(context.Request, context.Response, amplaUserService, FormsAuthenticationService, SessionStorage);
             loginAmplaSession.Execute();

             Assert.That(context.Request.Cookies, Is.Not.Empty);
             Assert.That(context.Request.Url, Is.EqualTo(new Uri("http://localhost/")));

             Assert.That(SessionStorage.GetAmplaSession(), Is.EqualTo(session));

             var ticket = FormsAuthenticationService.GetUserTicket();

             Assert.That(ticket, Is.Not.Null);
             Assert.That(ticket.UserData, Is.EqualTo(session));
         }

         [Test]
         public void NoSession()
         {
             SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");

             const string session = "invalid";

             const string url = "http://localhost/?amplaSession=" + session;

             context = SimpleHttpContext.Create(url);

             IAmplaUserService amplaUserService = new AmplaUserService(webServiceClient, new AmplaUserStore());

             LoginAmplaSessionUsingQueryString loginAmplaSession = new LoginAmplaSessionUsingQueryString(context.Request, context.Response, amplaUserService, FormsAuthenticationService, SessionStorage);

             loginAmplaSession.Execute();

             Assert.That(context.Request.Cookies, Is.Empty);
             Assert.That(context.Request.Url, Is.EqualTo(new Uri(url)));

             var ticket = FormsAuthenticationService.GetUserTicket();

             Assert.That(ticket, Is.Null);
             Assert.That(SessionStorage.GetAmplaSession(), Is.Empty);
         }

         [Test]
         public void WrongSessionKey()
         {
             SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
             webServiceClient.AddExistingSession("User");

             string session = webServiceClient.Sessions[0].SessionId;
             string url = "http://localhost/?session=" + session;
             context = SimpleHttpContext.Create(url);
             
             IAmplaUserService amplaUserService = new AmplaUserService(webServiceClient, new AmplaUserStore());

             LoginAmplaSessionUsingQueryString loginAmplaSession = new LoginAmplaSessionUsingQueryString(context.Request, context.Response, amplaUserService, FormsAuthenticationService, SessionStorage);

             loginAmplaSession.Execute();

             Assert.That(context.Request.Cookies, Is.Empty);
             Assert.That(context.Request.Url, Is.EqualTo(new Uri(url)));

             var ticket = FormsAuthenticationService.GetUserTicket();

             Assert.That(ticket, Is.Null);
             Assert.That(SessionStorage.GetAmplaSession(), Is.Empty);
         }
    }
}