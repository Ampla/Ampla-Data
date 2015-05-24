using NUnit.Framework;

namespace AmplaData.Binding.ModelData
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

        [Test]
        public void TryParseWithRecurse()
        {
            LocationFilter filter;

            bool result = LocationFilter.TryParse("Enterprise.Site with recurse", out filter);
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site"));
            Assert.That(filter.WithRecurse, Is.EqualTo(true));
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TryParse()
        {
            LocationFilter filter;

            bool result = LocationFilter.TryParse("Enterprise.Site", out filter);
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site"));
            Assert.That(filter.WithRecurse, Is.EqualTo(false));
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TryParseShortName()
        {
            LocationFilter filter;

            bool result = LocationFilter.TryParse("A.B", out filter);
            Assert.That(filter.Location, Is.EqualTo("A.B"));
            Assert.That(filter.WithRecurse, Is.EqualTo(false));
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void EmptyTryParse()
        {
            LocationFilter filter;

            bool result = LocationFilter.TryParse("", out filter);
            Assert.That(filter, Is.Null);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void NullTryParse()
        {
            LocationFilter filter;

            bool result = LocationFilter.TryParse(null, out filter);
            Assert.That(filter, Is.Null);
            Assert.That(result, Is.EqualTo(false));
        }

    }
}