using AmplaData.AmplaRepository;
using Autofac;

namespace AmplaData.Web.Modules
{
    public class AmplaDataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(AmplaRepository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(AmplaRepository<>)).Named("repository", typeof(IRepository<>));
            builder.RegisterGeneric(typeof(AmplaReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>));

        }
    }
}