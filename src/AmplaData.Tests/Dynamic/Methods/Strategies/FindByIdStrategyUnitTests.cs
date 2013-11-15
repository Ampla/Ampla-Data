using AmplaData.AmplaData2008;
using AmplaData.Dynamic.Methods.Binders;
using NUnit.Framework;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public class FindByIdStrategyUnitTests : TestFixture
    {
        private const string module = "Production";
        private const string location = "Enterprise.Site.Area.Production";

        protected override void OnFixtureSetUp()
        {
            base.OnFixtureSetUp();
            DataWebServiceFactory.Factory = () => new SimpleDataWebServiceClient(module, location);
        }

        protected override void OnFixtureTearDown()
        {
            DataWebServiceFactory.Factory = null;
            base.OnFixtureTearDown();
        }

        [Test]
        public void GetBinder()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            var memberBinder = Binder.GetMemberBinder("Find", 1, "Id");
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] {100});

            Assert.That(dynamicBinder, Is.Not.Null);
        }

        [Test]
        public void GetBinderWithOutId()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            var memberBinder = Binder.GetMemberBinder("Find", 0);
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] {});

            Assert.That(dynamicBinder, Is.Null);
        }

        [Test]
        public void GetBinderWithFindById()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            var memberBinder = Binder.GetMemberBinder("FindById", 1);
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] { 100 });

            Assert.That(dynamicBinder, Is.Not.Null);
        }

        [Test]
        public void GetBinderForWrongMethod()
        {
            FindByIdStrategy strategy = new FindByIdStrategy();
            var memberBinder = Binder.GetMemberBinder("Delete", 1, "Id");
            IDynamicBinder dynamicBinder = strategy.GetBinder(memberBinder, new object[] { 100 });

            Assert.That(dynamicBinder, Is.Null);
        }
    }
}