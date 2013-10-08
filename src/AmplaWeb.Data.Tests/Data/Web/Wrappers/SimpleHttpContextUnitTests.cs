using System;
using System.Web;
using NUnit.Framework;

namespace AmplaWeb.Data.Web.Wrappers
{
    [TestFixture]
    public class SimpleHttpContextUnitTests : TestFixture
    {
         [Test]
         public void RedirectWillChangeRequestUrl()
         {
             SimpleHttpContext context = new SimpleHttpContext("http://localhost/Production/Index");
             Assert.That(context.Request.Url.ToString(), Is.EqualTo("http://localhost/Production/Index"));
             
             context.Response.Redirect("http://localhost/Quality/Index");
             Assert.That(context.Request.Url.ToString(), Is.EqualTo("http://localhost/Quality/Index"));
         }

         [Test]
         public void RedirectWillChangeTakeResponseCookiesToRequest()
         {
             SimpleHttpContext context = new SimpleHttpContext("http://localhost/Production/Index");
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
    }
}