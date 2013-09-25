using System;

using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping
{
    [TestFixture]
    public class DefaultValueFieldMappingUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Point")]
        [AmplaModule("Production")]
        public class Model
        {
            public int Id { get; set; }

            public DateTime Sample { get; set; }
        }

        [Test]
        public void ResolveValueWithInvalidValue()
        {
            DefaultValueFieldMapping fieldMapping = new DefaultValueFieldMapping("Field", () => "Default");

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
            DefaultValueFieldMapping fieldMapping = new DefaultValueFieldMapping("Sample", () => defaultValue);

            Model model = new Model {Id = 0};

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo(defaultValue));
        }


        [Test]
        public void ResolveValueWithModelValue()
        {
            DateTime localTime = new DateTime(2001, 01, 26);
            string utcTime = new Iso8601DateTimeConverter().ConvertToInvariantString(localTime);

            DefaultValueFieldMapping fieldMapping = new DefaultValueFieldMapping("Sample", () => "blah");
           
            Model model = new Model {Id = 0, Sample = localTime};

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo(utcTime));
        }

    }
}