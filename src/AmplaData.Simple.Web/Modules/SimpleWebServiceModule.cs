using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Database;
using AmplaData.Modules.Downtime;
using AmplaData.Modules.Quality;
using AmplaData.Views;
using Autofac;

namespace AmplaData.Simple.Web.Modules
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
            amplaDatabase.EnableModule("Downtime");

            SimpleAmplaConfiguration configuration = new SimpleAmplaConfiguration();
            configuration.EnableModule("Production");
            configuration.AddLocation("Production", "Enterprise.Site.Area.Production");
            configuration.SetDefaultView("Production", QualityViews.StandardViewPlus(
                StandardViews.Field<double>("Weight"),
                StandardViews.Field<string>("Material", "Material", false, true)
                ));
            
            configuration.EnableModule("Quality");
            configuration.AddLocation("Quality", "Enterprise.Site.Area.Quality");
            configuration.SetDefaultView("Quality", QualityViews.StandardViewPlus(
                StandardViews.Field<double>("Moisture"),
                StandardViews.Field<string>("SampleId", "SampleId", false, true),
                StandardViews.Field<double>("Silica", "Silica", false, true),
                StandardViews.Field<double>("Sodium", "Sodium", false, true)
                ));

            configuration.EnableModule("Downtime");
            configuration.AddLocation("Downtime", "Enterprise.Site.Area.Downtime");
            configuration.SetDefaultView("Downtime", DowntimeViews.StandardView());

            builder.RegisterInstance(amplaDatabase).As<IAmplaDatabase>().SingleInstance();
            builder.RegisterInstance(configuration).As<IAmplaConfiguration>().SingleInstance();

            builder.RegisterInstance(securityClient).As<ISecurityWebServiceClient>().SingleInstance();
            builder.RegisterInstance(securityClient).SingleInstance();

            builder.RegisterType<SimpleDataWebServiceClient>().As<IDataWebServiceClient>();
            
        }
    }
}