using AmplaWeb.Data.Attributes;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ModelData
{
    [TestFixture]
    public class ModelPropertyNonDefaultUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
        [AmplaModule(Module = "Production")]
        public class ModelWithAmplaField
        {   
            [AmplaField("Full Name")]
            public string FullName
            {
                get; set;
            }
        }

        [Test]
        public void GetPropertiesReturnsAmplaField()
        {
            ModelProperties<ModelWithAmplaField> modelProperties = new ModelProperties<ModelWithAmplaField>();
            
            var properties = modelProperties.GetProperties();
            Assert.That(properties.Count, Is.EqualTo(1));
            Assert.That(properties[0], Is.EqualTo("Full Name"));
        }

        [Test]
        public void GetAmplaFieldProperty()
        {
            ModelProperties<ModelWithAmplaField> modelProperties = new ModelProperties<ModelWithAmplaField>();
            ModelWithAmplaField model = new ModelWithAmplaField {FullName = "John Doe"};

            string value;
            bool result = modelProperties.TryGetPropertyValue(model, "Full Name", out value);

            Assert.That(value, Is.EqualTo("John Doe"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void SetAmplaFieldProperty()
        {
            ModelProperties<ModelWithAmplaField> modelProperties = new ModelProperties<ModelWithAmplaField>();
            ModelWithAmplaField model = new ModelWithAmplaField { FullName = "John Doe" };

            bool result = modelProperties.TrySetValueFromString(model, "Full Name", "Jane Doe");

            Assert.That(model.FullName, Is.EqualTo("Jane Doe"));
            Assert.That(result, Is.True);
        }

    }
}