using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaData.Web.Modules
{
    public class ControllerInjectionModule : Autofac.Module
    {
        private readonly Assembly assembly;
        public ControllerInjectionModule(Assembly assembly)
        {
            this.assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterControllers(assembly);
        }
    }
}