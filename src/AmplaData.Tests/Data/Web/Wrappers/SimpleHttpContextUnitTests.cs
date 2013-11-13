using System;
using System.Web;
using NUnit.Framework;

namespace AmplaData.Data.Web.Wrappers
{
    [TestFixture]
    public class SimpleHttpContextUnitTests : TestFixture
    {
         [Test]
         public void RedirectWillChangeRequestUrl()
         {
             SimpleHttpContext context = SimpleHttpContext.Create("http://localhost/Production/Index");
             Assert.That(context.Request.Url.ToString(), Is.EqualTo("http://localhost/Production/Index"));
             
             context.Response.Redirect("http://localhost/Quality/Index");
             Assert.That(context.Request.Url.ToString(), Is.EqualTo("http://localhost/Quality/Index"));
         }

         [Test]
         public void RedirectWillChangeTakeResponseCookiesToRequest()
         {
             SimpleHttpContext context = SimpleHttpContext.Create("http://localhost/Production/Index");
             Assert.That(context.Request.Url.ToString(), Is.EqualTo("http://localhost/Production/Index"));

             Assert.That(context.Request.Cookies, Is.Empty);
             Assert.That(context.Response.Cookies, Is.Empty);
             context.Response.Cookies.Add(new HttpCookie("Test", "UnitTest"));
             Assert.That(context.Response.Cookies,Is.Not.Empty);

             context.Response.Redirect("http://localhost/Quality/Index");
             Assert.That(context.Request.Url.ToString(), Is.EqualTo("http://localhost/Quality/Index"));
             Assert.That(context.Request.Cookies, Is.Not.Empty);
             Assert.That(context.Response.Cookies, Is.Empty);
         }

        [Test]
        public void OldRequestThrowsDisposed()
        {
            SimpleHttpContext context = SimpleHttpContext.Create("http://localhost/Production/Index");
            var oldRequest = context.Request;
            var oldResponse = context.Response;

            context.Response.Redirect("http://localhost/Quality/Index");
            Uri url = null;
            Assert.Throws<ObjectDisposedException>(() => { url = oldRequest.Url; });
            Assert.That(url, Is.Null);

            Assert.Throws<ObjectDisposedException>(() => oldResponse.Redirect("http://localhost"));
        }

        [Test]
        public void WithSessionsDisabled()
        {
            SimpleHttpContext context = SimpleHttpContext.Create("http://localhost").WithSessionsDisabled();
            Assert.That(context.Session.Enabled, Is.False);
        }
    }
}