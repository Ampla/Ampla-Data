using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.Attributes
{
    [TestFixture]
    public class AmplaLocationAttributeUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
        public class ModelWithLocation
        {
        }

        public class InheritedModelWithLocation : ModelWithLocation
        {
        }

        [AmplaLocation(Location = "Enterprise.Site.Area.Overridden")]
        public class ModelWithOverriddenLocation : ModelWithLocation
        {
        }

        [AmplaLocation]
        public class ModelWithNoLocation
        {
        }

        [AmplaLocation(Location = "")]
        public class ModelWithEmptyLocation
        {
        }

        [AmplaLocation("Enterprise.Site.Area.Point")]
        public class ModelLocationViaConstructor
        {
        }

        public class ModelNoAttribute
        {
        }

        [Test]
        public void TryGetWithLocation()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithLocation>(out location);
        
            Assert.That(location, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithLocation()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<InheritedModelWithLocation>(out location);

            Assert.That(location, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithOverriddenLocation()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithOverriddenLocation>(out location);

            Assert.That(location, Is.EqualTo("Enterprise.Site.Area.Overridden"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithNoLocation()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithNoLocation>(out location);

            Assert.That(location, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithNoAttribute()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelNoAttribute>(out location);

            Assert.That(location, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithEmptyAttribute()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithEmptyLocation>(out location);

            Assert.That(location, Is.EqualTo(""));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithConstructorLocation()
        {
            string location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelLocationViaConstructor>(out location);

            Assert.That(location, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(result, Is.True);
        }
    }
}