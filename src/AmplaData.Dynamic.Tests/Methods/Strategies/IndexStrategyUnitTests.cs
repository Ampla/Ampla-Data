using System;
using NUnit.Framework;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public class IndexStrategyUnitTests : TestFixture
    {
        [Test]
        public void MatchesStringIndex()
        {
            IndexStrategy strategy = IndexStrategy.ForStringIndex();
            Assert.That(strategy.Matches(Binder.GetIndexBinder(1), new object[] {"Value"}), Is.True);
        }

        [Test]
        public void DoesNotMatchIntIndex()
        {
            IndexStrategy strategy = IndexStrategy.ForStringIndex();
            Assert.That(strategy.Matches(Binder.GetIndexBinder(1), new object[] { 100 }), Is.False);
        }
    }
}