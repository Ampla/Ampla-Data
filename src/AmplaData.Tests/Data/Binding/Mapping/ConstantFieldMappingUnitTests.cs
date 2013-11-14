using AmplaData.Attributes;
using AmplaData.Binding.ModelData;
using NUnit.Framework;

namespace AmplaData.Binding.Mapping
{
    [TestFixture]
    public class ConstantFieldMappingUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Point")]
        [AmplaModule("Production")]
        public class Model
        {
            public int Id { get; set; }
        }

        [Test]
        public void ConstantFieldMappingResolveValue()
        {
            ConstantFieldMapping fieldMapping = new ConstantFieldMapping("User", "John Doe");

            Model model = new Model { Id = 100 };

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo("John Doe"));
        }
    
    }
}