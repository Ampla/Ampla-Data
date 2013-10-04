using AmplaWeb.Data.AmplaData2008;
using Autofac;

namespace AmplaWeb.Sample.Modules
{
    public class SimpleSecurityInjectionModule : Module
    {
        private readonly string userName;
        private readonly string password;

        public SimpleSecurityInjectionModule(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => CredentialsProvider.ForUsernameAndPassword(userName, password))
                   .As<ICredentialsProvider>();

        }
    }
}