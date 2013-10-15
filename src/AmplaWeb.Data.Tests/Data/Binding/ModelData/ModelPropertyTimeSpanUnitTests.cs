using System;
using AmplaWeb.Data.Attributes;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ModelData
{
    [TestFixture]
    public class ModelPropertyTimeSpanUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
        [AmplaModule(Module = "Production")]
        public class ModelWithTimeSpanField
        {   
            [AmplaField("Duration")]
            public TimeSpan Display
            {
                get; set;
            }
        }

        [Test]
        public void GetPropertiesReturnsAmplaField()
        {
            ModelProperties<ModelWithTimeSpanField> modelProperties = new ModelProperties<ModelWithTimeSpanField>();
            
            var properties = modelProperties.GetProperties();
            Assert.That(properties.Count, Is.EqualTo(1));
            Assert.That(properties[0], Is.EqualTo("Duration"));
        }

        [Test]
        public void GetAmplaFieldProperty()
        {
            ModelProperties<ModelWithTimeSpanField> modelProperties = new ModelProperties<ModelWithTimeSpanField>();
            ModelWithTimeSpanField model = new ModelWithTimeSpanField { Display = TimeSpan.FromHours(1) };

            string value;
            bool result = modelProperties.TryGetPropertyValue(model, "Duration", out value);

            Assert.That(value, Is.EqualTo("3600"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void SetAmplaFieldProperty()
        {
            ModelProperties<ModelWithTimeSpanField> modelProperties = new ModelProperties<ModelWithTimeSpanField>();
            ModelWithTimeSpanField model = new ModelWithTimeSpanField { Display = TimeSpan.FromHours(1) };

            bool result = modelProperties.TrySetValueFromString(model, "Duration", "1800");

            Assert.That(model.Display, Is.EqualTo(TimeSpan.FromMinutes(30)));
            Assert.That(result, Is.True);
        }

    }
}