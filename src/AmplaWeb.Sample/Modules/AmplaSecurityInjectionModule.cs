using System.Web;
using System.Web.Security;
using AmplaWeb.Data;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Sessions;
using AmplaWeb.Security.Web.Interfaces;
using AmplaWeb.Security.Web.Wrappers;
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
            builder.RegisterType<FormsAuthenticationCredentialsProvider>().As<ICredentialsProvider>();

            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<AmplaUserStore>().As<IAmplaUserStore>().SingleInstance();
            builder.RegisterType<AmplaUserService>().As<IAmplaUserService>();

            builder.RegisterControllers(typeof(Security.Controllers.AccountController).Assembly);
            builder.RegisterType<AmplaSessionMapper>().As<ISessionMapper>();

            builder.Register(c => new AmplaHttpRequestWrapper(HttpContext.Current.Request)).As<IHttpRequestWrapper>();
            builder.Register(c => new AmplaHttpResponseWrapper(HttpContext.Current.Response)).As<IHttpResponseWrapper>();

            // Register the generic decorator so it can wrap
            // the resolved named generics.
            builder.RegisterGenericDecorator(
                typeof(RenewSessionAdapter<>),
                typeof(IRepository<>), "repository").As(typeof(IRepository<>));

        }
    }
}