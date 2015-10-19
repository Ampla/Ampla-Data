using System;
using NUnit.Framework;

namespace AmplaData.Binding.MetaData
{
    [TestFixture]
    public class AllowEmptyStringConverterUnitTests : TestFixture
    {
        private AllowEmptyStringConverter<int> intTypeConverter;
        private AllowEmptyStringConverter<string> stringTypeConverter; 

        protected override void OnSetUp()
        {
            base.OnSetUp();
            intTypeConverter = new AllowEmptyStringConverter<int>();
        }

        [Test]
        public void ConvertToString()
        {
            string value = intTypeConverter.ConvertToInvariantString(10);
            Assert.That(value, Is.EqualTo("10"));
        }

        [Test]
        public void ConvertFromString()
        {
            Assert.That(intTypeConverter.ConvertFromInvariantString("11"), Is.EqualTo(11));
        }

        [Test]
        public void ConvertToFromString()
        {
            Assert.That(intTypeConverter.CanConvertFrom(typeof(string)), Is.True);
            Assert.That(intTypeConverter.CanConvertTo(typeof(string)), Is.True);
        }

        [Test]
        public void ConvertToFromDateTime()
        {
            Assert.That(intTypeConverter.CanConvertFrom(typeof(DateTime)), Is.False);
            Assert.That(intTypeConverter.CanConvertTo(typeof(DateTime)), Is.False);
        }

        [Test]
        public void ConvertToFromInt()
        {
            Assert.That(intTypeConverter.CanConvertFrom(typeof(int)), Is.True);
            Assert.That(intTypeConverter.CanConvertTo(typeof(int)), Is.True);
        }

        [Test]
        public void ConvertIntToInt()
        {
            Assert.That(intTypeConverter.ConvertTo(10, typeof(int)), Is.EqualTo(10));
        }

    }
}