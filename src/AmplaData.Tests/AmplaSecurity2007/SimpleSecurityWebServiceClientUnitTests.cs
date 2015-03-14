using System;
using NUnit.Framework;

namespace AmplaData.AmplaSecurity2007
{
    [TestFixture]
    public class SimpleSecurityWebServiceClientUnitTests : TestFixture
    {
        [Test]
        public void CreateSession()
        {
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
            
            CreateSessionRequest request = new CreateSessionRequest {Username = "User", Password = "password"};

            CreateSessionResponse response = webServiceClient.CreateSession(request);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Session, Is.Not.Null);

            Assert.That(response.Session.User, Is.EqualTo("User"));
            Assert.That(response.Session.SessionID, Is.Not.Empty);

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);
            Assert.That(webServiceClient.Sessions.Count, Is.EqualTo(1));

            SimpleSession session = webServiceClient.Sessions[0];
            Assert.That(session.IsValid(), Is.True);
            Assert.That(session.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreateSessionDuplicateLogin()
        {
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");

            CreateSessionRequest request = new CreateSessionRequest { Username = "User", Password = "password" };
            webServiceClient.CreateSession(request);

            webServiceClient.CreateSession(request);

            Assert.That(webServiceClient.Sessions, Is.Not.Empty);
            Assert.That(webServiceClient.Sessions.Count, Is.EqualTo(1));

            SimpleSession session = webServiceClient.Sessions[0];
            Assert.That(session.IsValid(), Is.True);
            Assert.That(session.Count, Is.EqualTo(2));
        }

        [Test]
        public void CreateSessionInvalid()
        {
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");

            CreateSessionRequest request = new CreateSessionRequest { Username = "Invalid", Password = "password" };
            Assert.Throws<InvalidOperationException>(()=>webServiceClient.CreateSession(request));
        }

        [Test]
        public void CreateSessionBySession()
        {
            SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");

            string sessionId = Guid.NewGuid().ToString();
            Assert.That(webServiceClient.FindBySession(sessionId), Is.Null);

            webServiceClient.AddExistingSession("User");
            
            Assert.That(webServiceClient.Sessions, Is.Not.Empty);
            Assert.That(webServiceClient.Sessions.Count, Is.EqualTo(1));

            SimpleSession session = webServiceClient.Sessions[0];
            Assert.That(session, Is.Not.Null);

            SimpleSession found = webServiceClient.FindBySession(session.SessionId);
            Assert.That(found, Is.Not.Null);
        }
    }
}