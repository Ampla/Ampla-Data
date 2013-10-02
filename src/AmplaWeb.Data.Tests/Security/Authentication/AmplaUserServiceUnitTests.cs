using AmplaWeb.Security.AmplaSecurity2007;
using AmplaWeb.Security.Membership;
using NUnit.Framework;

namespace AmplaWeb.Security.Authentication
{
    [TestFixture]
    public class AmplaUserServiceUnitTests : TestFixture
    {
        [Test]
        public void Login()
        {
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            AmplaUserService amplaUserService = new AmplaUserService(webServiceClient);

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
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            AmplaUserService amplaUserService = new AmplaUserService(webServiceClient);

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
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            AmplaUserService amplaUserService = new AmplaUserService(webServiceClient);

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
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            webServiceClient.AddExistingSession("Admin");

            AmplaUserService amplaUserService = new AmplaUserService(webServiceClient);

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
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            webServiceClient.AddExistingSession("Admin");

            AmplaUserService amplaUserService = new AmplaUserService(webServiceClient);

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
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            AmplaUserService amplaUserService = new AmplaUserService(webServiceClient);

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
    }
}