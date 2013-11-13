using System.Reflection;
using NUnit.Framework;

namespace AmplaData.Data.Attributes
{
    [TestFixture]
    public class AmplaFieldAttributeUnitTests : TestFixture
    {
        public class ModelWithField
        {
            [AmplaField("Full Name")]
            public virtual string FullName { get; set; }

            public string FirstName { get; set; }
            
            [AmplaField]
            public string LastName { get; set; }
        }

        public class InheritedModelWithField : ModelWithField
        {
        }

        public class ModelWithOverriddenField : ModelWithField
        {
            [AmplaField("Another Full Name")]
            public override string FullName
            {
                get; set;
            }
        }

        public class ModelWithEmptyField
        {
            [AmplaField(Field = "")]
            public virtual string FullName { get; set; }
        }

        [Test]
        public void TryGetWithField()
        {
            string field;
            bool result = TryGetField<ModelWithField>("FullName", out field);
        
            Assert.That(field, Is.EqualTo("Full Name"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithField()
        {
            string field;
            bool result = TryGetField<InheritedModelWithField>("FullName", out field);

            Assert.That(field, Is.EqualTo("Full Name"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithOverriddenField()
        {
            string field;
            bool result = TryGetField<ModelWithOverriddenField>("FullName", out field);

            Assert.That(field, Is.EqualTo("Another Full Name"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithNoAttribute()
        {
            string field;
            bool result = TryGetField<ModelWithField>("FirstName", out field);

            Assert.That(field, Is.EqualTo("FirstName"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithEmptyAttribute()
        {
            string field;
            bool result = TryGetField<ModelWithField>("LastName", out field);

            Assert.That(field, Is.EqualTo("LastName"));
            Assert.That(result, Is.True);
        }

        private bool TryGetField<TModel>(string propertyName, out string field)
        {
            foreach (PropertyInfo property in typeof (TModel).GetProperties())
            {
                if (property.Name == propertyName)
                {
                    return AmplaFieldAttribute.TryGetField(property, out field);
                }
            }
            field = null;
            return false;
        }
    }
}