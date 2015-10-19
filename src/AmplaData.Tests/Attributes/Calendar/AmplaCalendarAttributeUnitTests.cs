using NUnit.Framework;

namespace AmplaData.Attributes.Calendar
{
    [TestFixture]
    public class AmplaCalendarAttributeUnitTests : TestFixture
    {
        [AmplaCalendar("Model.Area.")]
        public class ModelWithField
        {
            [AmplaCalendarValue("Tonnes.Target")]
            public double TargetTonnes { get; set; }
        }

        public class InheritedModelWithField : ModelWithField
        {
        }

        [AmplaCalendar("Model.Area.Overridden.")]
        public class ModelWithOverriddenBase : ModelWithField
        {
        }

        [AmplaCalendar]
        public class ModelWithNullBase
        {
        }

        [AmplaCalendar(BaseName = "Area.")]
        public class ModelWithAreaBase
        {
        }

        public class ModelWithNoAttribute
        {
            
        }


        [Test]
        public void TryGetWithField()
        {
            string baseName;
            bool result = TryGetBaseName<ModelWithField>(out baseName);
        
            Assert.That(baseName, Is.EqualTo("Model.Area."));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithField()
        {
            string baseName;
            bool result = TryGetBaseName<InheritedModelWithField>(out baseName);

            Assert.That(baseName, Is.EqualTo("Model.Area."));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithOverriddenField()
        {
            string baseName;
            bool result = TryGetBaseName<ModelWithOverriddenBase>(out baseName);

            Assert.That(baseName, Is.EqualTo("Model.Area.Overridden."));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithNoAttribute()
        {
            string baseName;
            bool result = TryGetBaseName<ModelWithNoAttribute>(out baseName);

            Assert.That(baseName, Is.Null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithNullBaseAttribute()
        {
            string field;
            bool result = TryGetBaseName<ModelWithNullBase>(out field);

            Assert.That(field, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        private bool TryGetBaseName<TModel>(out string baseName)
        {
            return AmplaCalendarAttribute.TryGetBaseName(typeof(TModel), out baseName);
        }
    }
}