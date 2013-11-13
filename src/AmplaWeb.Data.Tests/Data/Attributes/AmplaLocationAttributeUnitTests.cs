using AmplaData.Data.Binding.ModelData;
using NUnit.Framework;

namespace AmplaData.Data.Attributes
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
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithLocation>(out location);
        
            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<InheritedModelWithLocation>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithOverriddenLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithOverriddenLocation>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Overridden"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithNoLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithNoLocation>(out location);

            Assert.That(location, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithNoAttribute()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelNoAttribute>(out location);

            Assert.That(location, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithEmptyAttribute()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithEmptyLocation>(out location);

            Assert.That(location, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithConstructorLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelLocationViaConstructor>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(result, Is.True);
        }
    }
}