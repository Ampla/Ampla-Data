﻿using System.Web;
using System.Web.Security;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Sessions;
using AmplaWeb.Security.Web.Interfaces;
using AmplaWeb.Security.Web.Wrappers;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaWeb.Sample.Modules
{
    public class SecurityInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => Membership.Provider).As<MembershipProvider>();
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<AmplaUserService>().As<IAmplaUserService>().SingleInstance();
            builder.RegisterControllers(typeof(Security.Controllers.AccountController).Assembly);

            builder.RegisterType<AmplaSessionMapper>().As<ISessionMapper>();

            builder.Register(c => new AmplaHttpRequestWrapper(HttpContext.Current.Request)).As<IHttpRequestWrapper>();
            builder.Register(c => new AmplaHttpResponseWrapper(HttpContext.Current.Response)).As<IHttpResponseWrapper>();

        }
    }
}