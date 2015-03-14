using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AmplaData.AmplaData2008;
using AmplaData.AmplaRepository;
using AmplaData.AmplaSecurity2007;
using AmplaData.Web.Authentication;
using AmplaData.Web.MetaData;
using AmplaData.Web.Sample.Models;
using AmplaData.Web.Sample.Modules;
using AmplaData.Web.Sample.App_Start;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaData.Web.Sample
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

            ModelMetadataProviders.Current = new AmplaModelMetadataProvider();
            ConfigurationData configData = ConfigurationData.InMemory;

            if (configData.ConnectToAmpla)
            {
                builder.RegisterModule<AmplaWebServiceModule>();
                SecurityWebServiceFactory.Factory = () => new SecurityWebServiceClient("BasicHttpBinding_ISecurityWebService");
                DataWebServiceFactory.Factory = () => new DataWebServiceClient("NetTcpBinding_IDataWebService");
            }
            else
            {
                builder.RegisterModule<SimpleWebServiceModule>();
            }

            builder.RegisterGeneric(typeof(AmplaRepository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(AmplaRepository<>)).Named("repository", typeof(IRepository<>));
            builder.RegisterGeneric(typeof(AmplaReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>));

            builder.RegisterModule<ControllerInjectionModule>();
            
            builder.RegisterModule<AmplaSecurityInjectionModule>();
            //builder.RegisterModule(new SimpleSecurityInjectionModule("User", "password"));

            var container = builder.Build();

            /*
            if (configData.AddSampleData) // add sample data
            {
                IRepository<IngotCastModel> castRepository = container.Resolve<IRepository<IngotCastModel>>();
                castRepository.Add(new IngotCastModel {CastNo = "Cast 123"});
                castRepository.Add(new IngotCastModel {CastNo = "Cast 234"});

                IRepository<IngotBundleModel> bundleRepository = container.Resolve<IRepository<IngotBundleModel>>();
                bundleRepository.Add(new IngotBundleModel {CastNo = "Cast 123"});
                bundleRepository.Add(new IngotBundleModel {CastNo = "Cast 123"});
                bundleRepository.Add(new IngotBundleModel {CastNo = "Cast 123"});
                bundleRepository.Add(new IngotBundleModel {CastNo = "Cast 234"});
            }
            */
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