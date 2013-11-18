using System.Dynamic;
using AmplaData.AmplaData2008;
using AmplaData.Dynamic.Methods.Binders;
using AmplaData.Modules.Production;
using NUnit.Framework;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public class FindByIdStrategyUnitTests : TestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        protected override void OnSetUp()
        {
            base.OnSetUp();
            SimpleDataWebServiceClient client = new SimpleDataWebServiceClient(module, location)
            {
                GetViewFunc = ProductionViews.StandardView
            };

            DataWebServiceFactory.Factory = () => client;
        }

        protected override void OnTearDown()
        {
            base.OnTearDown();
            DataWebServiceFactory.Factory = null;
        }

        [Test]
        public void GetBinder()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            InvokeMemberBinder memberBinder = Binder.GetMemberBinder("Find", 1, "Id");
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] {100});

            Assert.That(dynamicBinder, Is.Not.Null);
        }

        [Test]
        public void GetBinderWithOutId()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            InvokeMemberBinder memberBinder = Binder.GetMemberBinder("Find", 0);
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] { });

            Assert.That(dynamicBinder, Is.Null);
        }

        [Test]
        public void GetBinderWithFindById()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            InvokeMemberBinder memberBinder = Binder.GetMemberBinder("FindById", 1);
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] { 100 });

            Assert.That(dynamicBinder, Is.Not.Null);
        }

        [Test]
        public void GetBinderForWrongMethod()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            InvokeMemberBinder memberBinder = Binder.GetMemberBinder("Delete", 1, "Id");
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] { 100 });

            Assert.That(dynamicBinder, Is.Null);
        }

        [Test]
        public void GetBinderForLowercase()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            InvokeMemberBinder memberBinder = Binder.GetMemberBinder("", 1, "Id");
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] { 100 });

            Assert.That(dynamicBinder, Is.Null);
        }
    }
}