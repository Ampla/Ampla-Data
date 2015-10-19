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

            public int Value { get; set; }

            public double DoubleValue { get; set; }
        }

        [Test]
        public void ResolveValue()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Sample", typeof(DateTime));

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
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Sample", typeof(DateTime));
            Assert.That(fieldMapping.CanWrite, Is.True);
        }

        [Test]
        public void CanMapField()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Value", typeof(int));
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.True);
            Assert.That(message, Is.Null.Or.Empty);
        }

        [Test]
        public void CanMapFieldString()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Value", typeof(string));
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.False);
            Assert.That(message, Is.StringContaining("'Value'").And.StringContaining("Int32").And.StringContaining("String"));
        }

        [Test]
        public void CanMapFieldDouble()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Value", typeof(double));
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.False);
            Assert.That(message, Is.StringContaining("'Value'").And.StringContaining("Int32").And.StringContaining("Double"));
        }

        [Test]
        public void CanMapFieldDoubleValue()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("DoubleValue", typeof(double));
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.True);
            Assert.That(message, Is.Null.Or.Empty);
        }

        [Test]
        public void CanMapFieldDoubleValueInt()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("DoubleValue", typeof(int));
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.False);
            Assert.That(message, Is.StringContaining("'DoubleValue'").And.StringContaining("Int32").And.StringContaining("Double"));

        }

        [Test]
        public void CanMapFieldDateTime()
        {
            ModelFieldMapping fieldMapping = new ModelFieldMapping("Value", typeof(DateTime));
            ModelProperties<Model> modelProperties = new ModelProperties<Model>();
            string message;
            Assert.That(fieldMapping.CanMapField(modelProperties, out message), Is.False);
            Assert.That(message, Is.StringContaining("'Value'").And.StringContaining("DateTime").And.StringContaining("Int32"));
        }
    }
}