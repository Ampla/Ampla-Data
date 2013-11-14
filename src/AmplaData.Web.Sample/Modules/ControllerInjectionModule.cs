using AmplaData.AmplaData2008;
using AmplaData.AmplaRepository;
using AmplaData.AmplaSecurity2007;
using AmplaData.Web.InMemory;
using AmplaData.Web.Sample.Models;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaData.Web.Sample.Modules
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
                
                builder.RegisterGeneric(typeof(AmplaRepository<>)).As(typeof(IRepository<>));
                builder.RegisterGeneric(typeof(AmplaRepository<>)).Named("repository", typeof (IRepository<>));
                builder.RegisterGeneric(typeof(AmplaReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>));
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