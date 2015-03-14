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
                
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
        }
    }
}