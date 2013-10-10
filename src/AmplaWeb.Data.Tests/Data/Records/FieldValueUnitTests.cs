using System;
using NUnit.Framework;

namespace AmplaWeb.Data.Records
{
    [TestFixture]
    public class FieldValueUnitTests : TestFixture
    {
        [Test]
        public void ResolveValueWithId()
        {
            FieldValue fieldValue = new FieldValue("Field", "Value", 100);

            Assert.That(fieldValue.ResolveValue(true), Is.EqualTo("Value"));
            Assert.That(fieldValue.ResolveValue(false), Is.EqualTo("100"));
        }

        [Test]
        public void ResolveValue()
        {
            FieldValue fieldValue = new FieldValue("Field", "Value");

            Assert.That(fieldValue.ResolveValue(true), Is.EqualTo("Value"));
            Assert.That(fieldValue.ResolveValue(false), Is.EqualTo("Value"));
        }

        [Test]
        public void CloneWithId()
        {
            FieldValue fieldValue = new FieldValue("Field", "Value", 100);
            FieldValue clone = fieldValue.Clone();
            Assert.That(clone, Is.Not.Null);
            Assert.That(clone.Name, Is.EqualTo(fieldValue.Name));
            Assert.That(clone.Value, Is.EqualTo(fieldValue.Value));
            Assert.That(clone.Id, Is.EqualTo(fieldValue.Id));
        }

        [Test]
        public void Clone()
        {
            FieldValue fieldValue = new FieldValue("Field", "Value");
            FieldValue clone = fieldValue.Clone();
            Assert.That(clone, Is.Not.Null);
            Assert.That(clone.Name, Is.EqualTo(fieldValue.Name));
            Assert.That(clone.Value, Is.EqualTo(fieldValue.Value));
            Assert.That(clone.Id, Is.EqualTo(fieldValue.Id));
        }


    }
}