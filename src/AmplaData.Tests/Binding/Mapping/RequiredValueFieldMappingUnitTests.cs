using System;

using AmplaData.Attributes;
using AmplaData.Binding.MetaData;
using AmplaData.Binding.ModelData;
using NUnit.Framework;

namespace AmplaData.Binding.Mapping
{
    [TestFixture]
    public class RequiredValueFieldMappingUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Point")]
        [AmplaModule("Production")]
        public class Model
        {
            public int Id { get; set; }

            public int Value { get; set; }
        }

        [Test]
        public void RequiredValueWithInvalidValue()
        {
            RequiredFieldMapping<string> fieldMapping = new RequiredFieldMapping<string>("Field", () => "Default");

            Model model = new Model();

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo("Default"));
        }

        [Test]
        public void ResolveValueWithDefaultValue()
        {
            string defaultValue = new Iso8601DateTimeConverter().ConvertToInvariantString(DateTime.UtcNow);
            RequiredFieldMapping<DateTime> fieldMapping = new RequiredFieldMapping<DateTime>("Sample", () => defaultValue);

            Model model = new Model {Id = 0};

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo(defaultValue));
        }

        [Test]
        public void CanWrite()
        {
            RequiredFieldMapping<string> fieldMapping = new RequiredFieldMapping<string>("Field", () => "Default");
            Assert.That(fieldMapping.CanWrite, Is.True);
        }

        [Test]
        public void CanMapField()
        {
            RequiredFieldMapping<string> fieldMapping = new RequiredFieldMapping<string>("Field", () => "Default");
            
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.True);
            Assert.That(message, Is.Null.Or.Empty);
        }


    }
}