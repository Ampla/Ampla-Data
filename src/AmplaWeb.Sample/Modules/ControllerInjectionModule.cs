using AmplaWeb.Data;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.AmplaRepository;
using AmplaWeb.Data.InMemory;
using AmplaWeb.Sample.Controllers;
using AmplaWeb.Sample.Models;
using AmplaWeb.Security.AmplaSecurity2007;
using AmplaWeb.Security.Authentication;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaWeb.Sample.Modules
{
    public class ControllerInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            string type = "Ampla";

            if (type == "Ampla")
            {
                builder.Register(c => DataWebServiceFactory.Create()).As<IDataWebServiceClient>();
                builder.Register(c => SecurityWebServiceFactory.Create()).As<ISecurityWebServiceClient>();
                
                builder.RegisterType<AmplaCredentialsProvider>().As<ICredentialsProvider>();
                //builder.RegisterType<AmplaRepositorySet>().As<IRepositorySet>();
                builder.RegisterGeneric(typeof (AmplaRepository<>)).Named("repository", typeof (IRepository<>));
                builder.RegisterGeneric(typeof(AmplaReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>));

                // Register the generic decorator so it can wrap
                // the resolved named generics.
                builder.RegisterGenericDecorator(
                    typeof (RenewSessionAdapter<>),
                    typeof (IRepository<>), "repository").As(typeof(IRepository<>));

            }
            else
            {
                InMemoryRepositorySet repositorySet = new InMemoryRepositorySet();
                builder.RegisterInstance(repositorySet).As<IRepositorySet>().SingleInstance();
                IRepository<IngotCastModel> castRepository = repositorySet.GetRepository<IngotCastModel>();
                castRepository.Add(new IngotCastModel { CastNo = "Cast 123" });
                castRepository.Add(new IngotCastModel { CastNo = "Cast 234" });

                IRepository<IngotBundleModel> bundleRepository = repositorySet.GetRepository<IngotBundleModel>();
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 123" });
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 123" });
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 123" });
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 234" });
            }

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
        }
    }
}