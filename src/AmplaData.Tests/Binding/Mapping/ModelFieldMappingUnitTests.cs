using System;
using AmplaData.Attributes;
using AmplaData.Binding.ModelData;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Binding.Mapping
{
    [TestFixture]
    public class ModelFieldMappingUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Point")]
        [AmplaModule("Production")]
        public class Model
        {
            public int Id { get; set; }

            public DateTime Sample { get; set; }
        }

        [Test]
        public void ResolveValue()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Sample");

            string todayUtc = PersistenceHelper.ConvertToString(DateTime.Today.ToUniversalTime());
            Model model = new Model { Id = 100, Sample = DateTime.Today };

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo(todayUtc));
        }

        [Test]
        public void ReadWrite()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Sample");
            Assert.That(fieldMapping.CanWrite, Is.True);
        }
    }


}