using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Binding.ModelData;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping
{
    [TestFixture]
    public class ValidatedModelFieldMappingUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Point")]
        [AmplaModule("Production")]
        public class Model
        {
            public int Id { get; set; }

            public string Location { get; set; }
        }

        [Test]
        public void ResolveValueWithNullValue()
        {
            ValidatedModelFieldMapping fieldMapping = new ValidatedModelFieldMapping("Location", s => !string.IsNullOrEmpty(s));

            Model model = new Model();

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.False);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void ResolveValueWithEmptyValue()
        {
            ValidatedModelFieldMapping fieldMapping = new ValidatedModelFieldMapping("Location", s => !string.IsNullOrEmpty(s));

            Model model = new Model {Location = ""};

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.False);
            Assert.That(value, Is.Null);
        }


        [Test]
        public void ResolveValueWithValue()
        {
            ValidatedModelFieldMapping fieldMapping = new ValidatedModelFieldMapping("Location", s => !string.IsNullOrEmpty(s));

            Model model = new Model() { Location = "Plant.Area.Point"};

            ModelProperties<Model> modelProperties = new ModelProperties<Model>();

            string value;
            Assert.That(fieldMapping.TryResolveValue(modelProperties, model, out value), Is.True);
            Assert.That(value, Is.EqualTo("Plant.Area.Point"));
        }


    }
}