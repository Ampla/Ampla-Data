using AmplaWeb.Data.Sessions;
using AmplaWeb.Data.Web.Wrappers;
using AmplaWeb.Security.AmplaSecurity2007;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Authentication.Forms;
using NUnit.Framework;

namespace AmplaWeb.Security.Sessions
{
    [TestFixture]
    public class AlignSessionWithFormsAuthenticationUnitTests : TestFixture
    {
        private SimpleHttpContext context;

        private IFormsAuthenticationService FormsAuthenticationService
        {
            get
            {
                return new FormsAuthenticationService(context.Request, context.Response);
            }
        }

        private AmplaSessionStorage AmplaSessionStorage
        {
            get
            {
                return new AmplaSessionStorage(context.Session);
            }
        }

        private IAmplaUserService amplaUserService;
        private SimpleSecurityWebServiceClient securityWebService;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            context = SimpleHttpContext.Create("http://localhost");
            
            securityWebService = new SimpleSecurityWebServiceClient("User");
            amplaUserService = new AmplaUserService(securityWebService, new AmplaUserStore());
        }

        [Test]
        public void SessionStorageIsSetFromForms()
        {
            securityWebService.AddExistingSession("User");
            string session = securityWebService.Sessions[0].SessionId;
            string message;
            AmplaUser user = amplaUserService.SessionLogin(session, out message);

            Assert.That(user, Is.Not.Null);
            FormsAuthenticationService.StoreUserTicket(user, false);

            context.Response.Redirect("http://localhost/Production");

            Assert.That(AmplaSessionStorage.GetAmplaSession(), Is.Empty);
            new AlignSessionWithFormsAuthentication(context.Request, AmplaSessionStorage, FormsAuthenticationService).Execute();

            Assert.That(AmplaSessionStorage.GetAmplaSession(), Is.EqualTo(session));
        }

        [Test]
        public void DisabledSessionStorageDoesntThrow()
        {
            context = context.WithSessionsDisabled();
            Assert.That(context.Session.Enabled, Is.False);

            securityWebService.AddExistingSession("User");
            string session = securityWebService.Sessions[0].SessionId;
            string message;
            AmplaUser user = amplaUserService.SessionLogin(session, out message);

            Assert.That(user, Is.Not.Null);
            FormsAuthenticationService.StoreUserTicket(user, false);

            context.Response.Redirect("http://localhost/Production");

            new AlignSessionWithFormsAuthentication(context.Request, AmplaSessionStorage, FormsAuthenticationService).Execute();

            Assert.That(AmplaSessionStorage.GetAmplaSession(), Is.Empty);
        }

        [Test]
        public void WontExecuteForUnauthenticatedSessions()
        {
            context = context.WithRequestsNotAuthenticated();
            Assert.That(context.Request.IsAuthenticated, Is.False);

            securityWebService.AddExistingSession("User");
            string session = securityWebService.Sessions[0].SessionId;
            string message;
            AmplaUser user = amplaUserService.SessionLogin(session, out message);

            Assert.That(user, Is.Not.Null);
            FormsAuthenticationService.StoreUserTicket(user, false);

            context.Response.Redirect("http://localhost/Production");

            Assert.That(AmplaSessionStorage.GetAmplaSession(), Is.Empty);
            new AlignSessionWithFormsAuthentication(context.Request, AmplaSessionStorage, FormsAuthenticationService).Execute();

            Assert.That(AmplaSessionStorage.GetAmplaSession(), Is.Empty);
        }
    }
}