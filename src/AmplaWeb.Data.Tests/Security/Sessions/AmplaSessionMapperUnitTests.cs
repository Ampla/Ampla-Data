using System;
using AmplaWeb.Security.AmplaSecurity2007;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Web.Wrappers;
using NUnit.Framework;

namespace AmplaWeb.Security.Sessions
{
    [TestFixture]
    public class AmplaSessionMapperUnitTests : TestFixture
    {
         [Test]
         public void AutomaticLoginFromSession()
         {
             SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User"); 
             webServiceClient.AddExistingSession("User");

             string session = webServiceClient.Sessions[0].SessionId;
             string url = "http://localhost/?amplaSession=" + session;
             SimpleHttpContext context = new SimpleHttpContext(url);

             IAmplaUserService amplaUserService = new AmplaUserService(webServiceClient, new AmplaUserStore());
             IFormsAuthenticationService formsAuthenticationService = new FormsAuthenticationService(context, context);

             AmplaSessionMapper amplaSessionMapper = new AmplaSessionMapper(context, context, amplaUserService, formsAuthenticationService);

             amplaSessionMapper.Login();

             Assert.That(context.RequestCookies, Is.Not.Empty);
             Assert.That(context.Url, Is.EqualTo(new Uri("http://localhost/")));

             var ticket = formsAuthenticationService.GetUserTicket();

             Assert.That(ticket, Is.Not.Null);
             Assert.That(ticket.UserData, Is.EqualTo(session));
         }

         [Test]
         public void NoSession()
         {
             SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");

             const string session = "invalid";

             const string url = "http://localhost/?amplaSession=" + session;
             SimpleHttpContext context = new SimpleHttpContext(url);

             IAmplaUserService amplaUserService = new AmplaUserService(webServiceClient, new AmplaUserStore());
             IFormsAuthenticationService formsAuthenticationService = new FormsAuthenticationService(context, context);

             AmplaSessionMapper amplaSessionMapper = new AmplaSessionMapper(context, context, amplaUserService, formsAuthenticationService);

             amplaSessionMapper.Login();

             Assert.That(context.RequestCookies, Is.Empty);
             Assert.That(context.Url, Is.EqualTo(new Uri(url)));

             var ticket = formsAuthenticationService.GetUserTicket();

             Assert.That(ticket, Is.Null);
         }

         [Test]
         public void WrongSessionKey()
         {
             SimpleSecurityWebServiceClient webServiceClient = new SimpleSecurityWebServiceClient("User");
             webServiceClient.AddExistingSession("User");

             string session = webServiceClient.Sessions[0].SessionId;
             string url = "http://localhost/?session=" + session;
             SimpleHttpContext context = new SimpleHttpContext(url);

             IAmplaUserService amplaUserService = new AmplaUserService(webServiceClient, new AmplaUserStore());
             IFormsAuthenticationService formsAuthenticationService = new FormsAuthenticationService(context, context);

             AmplaSessionMapper amplaSessionMapper = new AmplaSessionMapper(context, context, amplaUserService, formsAuthenticationService);

             amplaSessionMapper.Login();

             Assert.That(context.RequestCookies, Is.Empty);
             Assert.That(context.Url, Is.EqualTo(new Uri(url)));

             var ticket = formsAuthenticationService.GetUserTicket();

             Assert.That(ticket, Is.Null);
         }
    }
}