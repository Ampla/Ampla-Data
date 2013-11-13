using NUnit.Framework;

namespace AmplaData.Data.Binding.ModelData
{
    [TestFixture]
    public class LocationFilterUnitTests : TestFixture
    {
        [Test]
        public void Location()
        {
            LocationFilter filter = new LocationFilter("Enterprise.Site", true);
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site"));
           
            Assert.That(filter.WithRecurse, Is.True);
        }

        [Test]
        public void WithRecurse()
        {
            LocationFilter filter = new LocationFilter("Enterprise.Site", true);
            Assert.That(filter.WithRecurse, Is.True);

            filter = new LocationFilter("Enterprise.Site", false);
            Assert.That(filter.WithRecurse, Is.False);
        }

        [Test]
        public void Filter()
        {
            LocationFilter filter = new LocationFilter("Enterprise.Site", true);
            Assert.That(filter.Filter, Is.EqualTo("Enterprise.Site with recurse"));

            filter = new LocationFilter("Enterprise.Site", false);
            Assert.That(filter.Filter, Is.EqualTo("Enterprise.Site"));            
        }
    }
}