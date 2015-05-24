using AmplaData.AmplaData2008;
using AmplaData.AmplaRepository;
using AmplaData.AmplaSecurity2007;
using Autofac;

namespace AmplaData.Web.Modules
{
    public class AmplaWebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => DataWebServiceFactory.Create()).As<IDataWebServiceClient>();
            builder.Register(c => SecurityWebServiceFactory.Create()).As<ISecurityWebServiceClient>();

            builder.RegisterGeneric(typeof (AmplaRepository<>)).As(typeof (IRepository<>));
            builder.RegisterGeneric(typeof (AmplaRepository<>)).Named("repository", typeof (IRepository<>));
            builder.RegisterGeneric(typeof (AmplaReadOnlyRepository<>)).As(typeof (IReadOnlyRepository<>));
        }
    }
}