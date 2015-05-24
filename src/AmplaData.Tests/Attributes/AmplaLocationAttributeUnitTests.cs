using AmplaData.Binding.ModelData;
using NUnit.Framework;

namespace AmplaData.Attributes
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

        [AmplaLocation("Enterprise.Site with recurse")]
        public class ModelLocationWithRecurseViaConstructor
        {
        }

        [AmplaLocation("Enterprise.Site", WithRecurse = true)]
        public class ModelLocationViaConstructorWithRecurseProperty
        {
        }

        [AmplaLocation("Enterprise.Site", WithRecurse = false)]
        public class ModelLocationViaConstructorWithRecurseFalseProperty
        {
        }

        [AmplaLocation("Enterprise.Site with recurse", WithRecurse = false)]
        public class ModelLocationWithRecurseViaConstructorOverrideProperty
        {
        }


        [Test]
        public void TryGetWithLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithLocation>(out location);
        
            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Point"));

            Assert.That(location.Location, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(location.WithRecurse, Is.EqualTo(false));

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<InheritedModelWithLocation>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Point"));

            Assert.That(location.Location, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(location.WithRecurse, Is.EqualTo(false));

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithOverriddenLocation()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelWithOverriddenLocation>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site.Area.Overridden"));
            Assert.That(location.Location, Is.EqualTo("Enterprise.Site.Area.Overridden"));
            Assert.That(location.WithRecurse, Is.EqualTo(false));

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
            Assert.That(location.Location, Is.EqualTo("Enterprise.Site.Area.Point"));
            Assert.That(location.WithRecurse, Is.EqualTo(false));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithConstructorLocationWithRecurse()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelLocationWithRecurseViaConstructor>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site with recurse"));
            Assert.That(result, Is.True);
            Assert.That(location.Location, Is.EqualTo("Enterprise.Site"));
            Assert.That(location.WithRecurse, Is.EqualTo(true));
        }

        [Test]
        public void TryGetWithConstructorLocationWithRecurseProperty()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelLocationViaConstructorWithRecurseProperty>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site with recurse"));
            Assert.That(result, Is.True);
            Assert.That(location.Location, Is.EqualTo("Enterprise.Site"));
            Assert.That(location.WithRecurse, Is.EqualTo(true));
        }

        [Test]
        public void TryGetWithConstructorLocationWithRecurseFalseProperty()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelLocationViaConstructorWithRecurseFalseProperty>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site"));
            Assert.That(result, Is.True);
            Assert.That(location.Location, Is.EqualTo("Enterprise.Site"));
            Assert.That(location.WithRecurse, Is.EqualTo(false));
        }

        [Test]
        public void TryGetWithConstructorLocationWithOverriddenRecurseProperty()
        {
            LocationFilter location;
            bool result = AmplaLocationAttribute.TryGetLocation<ModelLocationWithRecurseViaConstructorOverrideProperty>(out location);

            Assert.That(location.Filter, Is.EqualTo("Enterprise.Site"));
            Assert.That(result, Is.True);
            Assert.That(location.Location, Is.EqualTo("Enterprise.Site"));
            Assert.That(location.WithRecurse, Is.EqualTo(false));
        }

    }
}