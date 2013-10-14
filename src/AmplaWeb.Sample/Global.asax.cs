using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AmplaWeb.Data;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Sample.App_Start;
using AmplaWeb.Sample.Controllers;
using AmplaWeb.Sample.Models;
using AmplaWeb.Sample.Modules;
using AmplaWeb.Security.AmplaSecurity2007;
using AmplaWeb.Security.Authentication;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaWeb.Sample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();

            new AmplaAuthenticationModule().Initialize(this);
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ControllerInjectionModule>();
            
            builder.RegisterModule<AmplaSecurityInjectionModule>();
            //builder.RegisterModule(new SimpleSecurityInjectionModule("User", "password"));

            SecurityWebServiceFactory.Factory = () => new SecurityWebServiceClient("BasicHttpBinding_ISecurityWebService");
            DataWebServiceFactory.Factory = () => new DataWebServiceClient("NetTcpBinding_IDataWebService");

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            NavigationRouteConfig.RegisterRoutes(RouteTable.Routes);

        }
    }
}