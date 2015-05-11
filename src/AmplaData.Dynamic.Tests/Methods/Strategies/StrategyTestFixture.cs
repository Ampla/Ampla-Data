using System.Dynamic;
using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Database;
using AmplaData.Dynamic.Methods.Binders;
using AmplaData.Modules.Production;
using AmplaData.Records;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public abstract class StrategyTestFixture<T> : TestFixture where T : IMemberStrategy, new()
    {
        protected const string Location = "Enterprise.Site.Area.Production";
        protected const string Module = "Production";

        protected T Strategy { get; private set; }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            SimpleSecurityWebServiceClient securityWebService = new SimpleSecurityWebServiceClient("User");
            SimpleAmplaDatabase amplaDatabase = new SimpleAmplaDatabase();
            SimpleAmplaConfiguration configuration = new SimpleAmplaConfiguration();
            SimpleDataWebServiceClient client = new SimpleDataWebServiceClient(amplaDatabase, configuration, securityWebService);
            
            DataWebServiceFactory.Factory = () => client;
            Strategy = new T();
        }

        protected override void OnTearDown()
        {
            base.OnTearDown();
            DataWebServiceFactory.Factory = null;
            Strategy = default(T);
        }

        protected void AssertMemberBinder(string method, int argCount, string[] argNames, object[] args)
        {
            InvokeMemberBinder memberBinder = Binder.GetMemberBinder("Find", 1, "Id");
            IDynamicBinder dynamicBinder = Strategy.GetBinder(memberBinder, new object[] {100});
            Assert.That(dynamicBinder, Is.Not.Null);

        }

        protected void AssertBinder(Binder binder, IResolveConstraint resolveConstraint)
        {
            InvokeMemberBinder memberBinder = binder.GetMemberBinder();
            IDynamicBinder dynamicBinder = Strategy.GetBinder(memberBinder, binder.Args);
            Assert.That(dynamicBinder, resolveConstraint);
        }
    }
}