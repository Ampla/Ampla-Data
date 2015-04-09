using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Database;
using AmplaData.Modules.Production;
using AmplaData.Records;
using Autofac;

namespace AmplaData.Web.Sample.Modules
{
    public class SimpleWebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            SimpleSecurityWebServiceClient securityClient = new SimpleSecurityWebServiceClient("User");
            SimpleAmplaDatabase amplaDatabase = new SimpleAmplaDatabase();
            amplaDatabase.EnableModule("Production");
            amplaDatabase.EnableModule("Quality");

            SimpleAmplaConfiguration configuration = new SimpleAmplaConfiguration();
            configuration.EnableModule("Production");
            configuration.AddLocation("Production", "Enterprise.Site.Area.Production");
            
            configuration.EnableModule("Quality");
            configuration.AddLocation("Quality", "Enterprise.Site.Area.Quality");

            builder.RegisterInstance(amplaDatabase).As<IAmplaDatabase>().SingleInstance();
            builder.RegisterInstance(configuration).As<IAmplaConfiguration>().SingleInstance();

            builder.RegisterInstance(securityClient).As<ISecurityWebServiceClient>().SingleInstance();
            builder.RegisterInstance(securityClient).SingleInstance();

            builder.RegisterType<SimpleDataWebServiceClient>().As<IDataWebServiceClient>();
            
        }
    }
}