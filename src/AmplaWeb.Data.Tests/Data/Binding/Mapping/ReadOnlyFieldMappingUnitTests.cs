using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Binding.ModelData;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping
{
    [TestFixture]
    public class ReadOnlyFieldMappingUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Point")]
        [AmplaModule("Production")]
        public class Model
        {
            public Model() : this("EquipmentId"){ }
            public Model(string equipmentId)
            {
                EquipmentId = equipmentId;
            }

            public int Id { get; set; }

            public string EquipmentId { get; private set; }
        }

        [Test]
        public void ResolveValue()
        {
            ReadOnlyFieldMapping fieldMapping = new ReadOnlyFieldMapping("EquipmentId");

            Model model = new Model("EQ123") { Id = 100 };

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.False);
            Assert.That(value, Is.EqualTo(null));
        }

        [Test]
        public void ReadWrite()
        {
            ReadOnlyFieldMapping fieldMapping = new ReadOnlyFieldMapping("EquipmentId");
            Assert.That(fieldMapping.CanWrite, Is.False);
        }

    }
}