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
            AmplaUser user = amplaUserService.Login("User", "password", out message);
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
            AmplaUser user = amplaUserService.Login("Invalid", "password", out message);
            Assert.That(user, Is.Null);

            Assert.That(webServiceClient.Sessions, Is.Empty);
            Assert.That(message, Is.Not.Empty);
            Assert.That(message, Is.StringContaining("password"));
        }
    }
}