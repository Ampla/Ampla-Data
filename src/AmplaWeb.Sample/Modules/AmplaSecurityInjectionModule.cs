using System.Web;
using AmplaData.Data;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Sessions;
using AmplaData.Data.Web.Interfaces;
using AmplaData.Data.Web.Wrappers;
using AmplaData.Security.Authentication;
using AmplaData.Security.Authentication.Forms;
using AmplaData.Security.Sessions;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaData.Web.Sample.Modules
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
            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<AmplaUserStore>().As<IAmplaUserStore>().SingleInstance();
            builder.RegisterType<AmplaUserService>().As<IAmplaUserService>();

            builder.RegisterControllers(typeof(Security.Controllers.AccountController).Assembly);
            builder.RegisterType<LoginAmplaSessionUsingQueryString>();

            // ensure the HttpSessions are aligned with Forms Authentication sessions
            builder.RegisterType<AlignSessionWithFormsAuthentication>();

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