using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ModelData
{
    [TestFixture]
    public class ModelPropertyLocationUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Enterprise")]
        [AmplaModule(Module = "Production")]
        public class ModelWithLocationField
        {
            public string Location { get; set; }
        }

        [Test]
        public void GetPropertiesReturnsLocationField()
        {
            ModelProperties<ModelWithLocationField> modelProperties = new ModelProperties<ModelWithLocationField>();
            ModelWithLocationField model = new ModelWithLocationField ();

            var properties = modelProperties.GetProperties();
            Assert.That(properties.Count, Is.EqualTo(1));
            Assert.That(properties[0], Is.EqualTo("Location"));

            Assert.That(modelProperties.IsDefaultValue(model, "Location"), Is.True);

            model.Location = "Enterprise.Site";
            Assert.That(modelProperties.IsDefaultValue(model, "Location"), Is.False);
        }

        [Test]
        public void GetAmplaFieldProperty()
        {
            ModelProperties<ModelWithLocationField> modelProperties = new ModelProperties<ModelWithLocationField>();
            ModelWithLocationField model = new ModelWithLocationField { Location = "Enterprise.Site" };

            string value;
            bool result = modelProperties.TryGetPropertyValue(model, "Location", out value);

            Assert.That(value, Is.EqualTo("Enterprise.Site"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void SetAmplaFieldProperty()
        {
            ModelProperties<ModelWithLocationField> modelProperties = new ModelProperties<ModelWithLocationField>();
            ModelWithLocationField model = new ModelWithLocationField { Location = "Enterprise.Site" };

            bool result = modelProperties.TrySetValueFromString(model, "Location", "Enterprise.Site.Area");

            Assert.That(model.Location, Is.EqualTo("Enterprise.Site.Area"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void GetLocation()
        {
            ModelProperties<ModelWithLocationField> modelProperties = new ModelProperties<ModelWithLocationField>();
            ModelWithLocationField model = new ModelWithLocationField { Location = "Enterprise.Site" };

            Assert.That(modelProperties.GetLocation(null), Is.EqualTo("Enterprise"));
            Assert.That(modelProperties.GetLocation(model), Is.EqualTo("Enterprise.Site"));

            model.Location = "Enterprise.Site.Area";

            Assert.That(modelProperties.GetLocation(model), Is.EqualTo("Enterprise.Site.Area"));
        }

        [Test]
        public void GetLocationDefault()
        {
            ModelProperties<ModelWithLocationField> modelProperties = new ModelProperties<ModelWithLocationField>();
            ModelWithLocationField model = new ModelWithLocationField { Location = null };

            Assert.That(modelProperties.GetLocation(null), Is.EqualTo("Enterprise"));
            Assert.That(modelProperties.GetLocation(model), Is.EqualTo("Enterprise"));

            model.Location = "Enterprise.Site.Area";

            Assert.That(modelProperties.GetLocation(model), Is.EqualTo("Enterprise.Site.Area"));
        }

    }
}