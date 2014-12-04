using AmplaData.AmplaSecurity2007;
using NUnit.Framework;

namespace AmplaData.Web.Authentication
{
    [TestFixture]
    public class AmplaUserServiceUnitTests : TestFixture
    {
        private SimpleSecurityWebServiceClient webServiceClient;
        private AmplaUserStore amplaUserStore;
        private AmplaUserService amplaUserService;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            webServiceClient = new SimpleSecurityWebServiceClient("User");
            amplaUserStore = new AmplaUserStore();
            amplaUserService = new AmplaUserService(webServiceClient, amplaUserStore);
        }

        protected override void OnTearDown()
        {
            base.OnTearDown();
            webServiceClient = null;
            amplaUserStore = null;
            amplaUserService = null;
        }

        [Test]
        public void Login()
        {
            string message;
            AmplaUser user = amplaUserService.SimpleLogin("User", "password", out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);

            Assert.That(user.UserName, Is.EqualTo("User"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(webServiceClient.Sessions[0].SessionId));
            Assert.That(message, Is.Null.Or.Empty);
        }

        [Test]
        public void LoginInvalid()
        {
            string message;
            AmplaUser user = amplaUserService.SimpleLogin("Invalid", "password", out message);
            Assert.That(user, Is.Null);

            Assert.That(webServiceClient.Sessions, Is.Empty);
            Assert.That(message, Is.Not.Empty);
            Assert.That(message, Is.StringContaining("password"));
        }

        [Test]
        public void LoginAndLogout()
        {
            string message;
            AmplaUser user = amplaUserService.SimpleLogin("User", "password", out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);

            Assert.That(user.UserName, Is.EqualTo("User"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(webServiceClient.Sessions[0].SessionId));
            Assert.That(message, Is.Null.Or.Empty);

            amplaUserService.Logout("User");

            Assert.That(webServiceClient.Sessions, Is.Empty);
        }

        [Test]
        public void LoginUsingSession()
        {
            webServiceClient.AddExistingSession("Admin");
            Assert.That(webServiceClient.Sessions, Is.Not.Empty);

            string session = webServiceClient.Sessions[0].SessionId;

            string message;
            AmplaUser user = amplaUserService.SessionLogin(session, out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(user.UserName, Is.EqualTo("Admin"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(session));
            Assert.That(message, Is.Null.Or.Empty);
        }

        [Test]
        public void LoginUsingSessionAndLogout()
        {
            webServiceClient.AddExistingSession("Admin");
            Assert.That(webServiceClient.Sessions, Is.Not.Empty);

            string session = webServiceClient.Sessions[0].SessionId;

            string message;
            AmplaUser user = amplaUserService.SessionLogin(session, out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(user.UserName, Is.EqualTo("Admin"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(session));
            Assert.That(message, Is.Null.Or.Empty);

            amplaUserService.Logout("Admin");

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);
        }

        [Test]
        public void LoginTwice()
        {
            string message;
            AmplaUser user = amplaUserService.SimpleLogin("User", "password", out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);

            Assert.That(user.UserName, Is.EqualTo("User"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(webServiceClient.Sessions[0].SessionId));
            Assert.That(message, Is.Null.Or.Empty);

            // login again
            user = amplaUserService.SimpleLogin("User", "password", out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);
            Assert.That(webServiceClient.Sessions.Count, Is.EqualTo(1));

            Assert.That(user.UserName, Is.EqualTo("User"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(webServiceClient.Sessions[0].SessionId));
            Assert.That(message, Is.Null.Or.Empty);
            
        }

        [Test]
        public void MultipleSessionsForSameUser()
        {
            webServiceClient.AddExistingSession("Admin");
            webServiceClient.AddExistingSession("Admin");

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);

            string session1 = webServiceClient.Sessions[0].SessionId;
            string session2 = webServiceClient.Sessions[1].SessionId;

            string message;
            AmplaUser user = amplaUserService.SessionLogin(session1, out message);
            Assert.That(user, Is.Not.Null);

            Assert.That(user.UserName, Is.EqualTo("Admin"));
            Assert.That(user.Session, Is.Not.Empty);
            Assert.That(user.Session, Is.EqualTo(session1));
            Assert.That(user.Session, Is.Not.EqualTo(session2));
            Assert.That(message, Is.Null.Or.Empty);
        }

    }
}