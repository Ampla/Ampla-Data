using System.Web;
using AmplaWeb.Data;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Session;
using AmplaWeb.Data.Web.Interfaces;
using AmplaWeb.Data.Web.Wrappers;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Authentication.Forms;
using AmplaWeb.Security.Sessions;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaWeb.Sample.Modules
{
    public class AmplaSecurityInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            // to register the forms credentials provider
            //builder.RegisterType<FormsAuthenticationCredentialsProvider>().As<ICredentialsProvider>();

            builder.RegisterType<AmplaSessionStorage>().As<IAmplaSessionStorage>();
            builder.RegisterType<SessionStateCredentialsProvider>().As<ICredentialsProvider>();
            builder.RegisterType<AmplaSessionStorage>().As<IAmplaSessionStorage>();

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<AmplaUserStore>().As<IAmplaUserStore>().SingleInstance();
            builder.RegisterType<AmplaUserService>().As<IAmplaUserService>();

            builder.RegisterControllers(typeof(Security.Controllers.AccountController).Assembly);
            builder.RegisterType<AmplaSessionMapper>().As<ISessionMapper>();

            builder.Register(c => new AmplaHttpRequestWrapper(HttpContext.Current.Request)).As<IHttpRequestWrapper>();
            builder.Register(c => new AmplaHttpResponseWrapper(HttpContext.Current.Response)).As<IHttpResponseWrapper>();
            builder.Register(c => new AmplaHttpSessionWrapper(HttpContext.Current.Session)).As<IHttpSessionWrapper>();

            // Register the generic decorator so it can wrap
            // the resolved named generics.
            builder.RegisterGenericDecorator(
                typeof(RenewSessionAdapter<>),
                typeof(IRepository<>), "repository").As(typeof(IRepository<>));

        }
    }
}