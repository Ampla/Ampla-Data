using NUnit.Framework;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public class FindByIdStrategyUnitTests : StrategyTestFixture<FindByIdStrategy>
    {

        [Test]
        public void FindWithNamedIdInt()
        {
            var binder = Binder.Member.Named("Find").WithArguments(1, "Id").Passed(100);

            AssertBinder(binder, Is.Not.Null);
        }

        [Test]
        public void FindWithNoId()
        {
            var binder = Binder.Member.Named("Find").WithArguments(0).Passed();
            AssertBinder(binder, Is.Null);
        }

        [Test]
        public void FindByIdWithArg()
        {
            var binder = Binder.Member.Named("FindById").WithArguments(1).Passed(100);

            AssertBinder(binder, Is.Not.Null);
        }

        [Test]
        public void FindByIdWithNamedArg()
        {
            var binder = Binder.Member.Named("FindById").WithArguments(1, "Id").Passed(100);

            AssertBinder(binder, Is.Not.Null);
        }

        [Test]
        public void DeleteWithNamedId()
        {
            var binder = Binder.Member.Named("Delete").WithArguments(1, "Id").Passed(100);

            AssertBinder(binder, Is.Null);
        }

        [Test]
        public void LowercaseFind()
        {
            var binder = Binder.Member.Named("find").WithArguments(1, "Id").Passed(100);
            AssertBinder(binder, Is.Null);
        }
    }
}