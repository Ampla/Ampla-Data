using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Modules.Production;
using Autofac;

namespace AmplaData.Web.Sample.Modules
{
    public class SimpleWebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            SimpleSecurityWebServiceClient securityClient = new SimpleSecurityWebServiceClient("User");
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production",
                                                                                         "Enterprise.Site.Area.Production", securityClient)
                {
                    GetViewFunc = ProductionViews.StandardView,
                };
            builder.RegisterInstance(webServiceClient).As<IDataWebServiceClient>().SingleInstance();
            builder.RegisterInstance(securityClient).As<ISecurityWebServiceClient>().SingleInstance();
        }
    }
}