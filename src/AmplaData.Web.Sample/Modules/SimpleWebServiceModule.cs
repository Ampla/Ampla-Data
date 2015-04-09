using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
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

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(amplaDatabase, 
                "Production",
                new [] {"Enterprise.Site.Area.Production"},
                securityClient)
                {
                    GetViewFunc = ProductionViews.StandardView,
                };

            builder.RegisterInstance(amplaDatabase).As<SimpleAmplaDatabase>().SingleInstance();
            builder.RegisterInstance(webServiceClient).As<IDataWebServiceClient>().SingleInstance();
            builder.RegisterInstance(securityClient).As<ISecurityWebServiceClient>().SingleInstance();
        }
    }
}