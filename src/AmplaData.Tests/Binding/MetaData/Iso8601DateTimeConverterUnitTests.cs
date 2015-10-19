using System;
using NUnit.Framework;

namespace AmplaData.Binding.MetaData
{
    [TestFixture]
    public class Iso8601DateTimeConverterUnitTests : TestFixture
    {
        private Iso8601DateTimeConverter typeConverter;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            typeConverter = new Iso8601DateTimeConverter();
        }

        [Test]
        public void ConvertToString()
        {
            DateTime today221 = DateTime.Today.AddHours(2).AddMinutes(21).ToLocalTime();
            DateTime utc = today221.ToUniversalTime();

            string value = typeConverter.ConvertToInvariantString(today221);
            Assert.That(value, Is.EqualTo(utc.ToString("yyyy-MM-ddTHH:mm:ssZ")));
        }

        [Test]
        public void ConvertFromString()
        {
            DateTime today221 = DateTime.Today.AddHours(2).AddMinutes(21).ToLocalTime();
            DateTime utc = today221.ToUniversalTime();


            string value = utc.ToString("yyyy-MM-ddTHH:mm:ssZ");
            Assert.That(typeConverter.ConvertFromInvariantString(value), Is.EqualTo(today221));
        }

        [Test]
        public void ConvertDateTimeToDateTime()
        {
            DateTime today221 = DateTime.Today.AddHours(2).AddMinutes(21).ToLocalTime();
            Assert.That(typeConverter.ConvertTo(today221, typeof (DateTime)), Is.EqualTo(today221));
        }

        [Test]
        public void ConvertToFromString()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(string)), Is.True);
            Assert.That(typeConverter.CanConvertTo(typeof(string)), Is.True);
        }

        [Test]
        public void ConvertToFromDateTime()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(DateTime)), Is.True);
            Assert.That(typeConverter.CanConvertTo(typeof(DateTime)), Is.True);
        }

        [Test]
        public void ConvertToFromInt()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(int)), Is.False);
            Assert.That(typeConverter.CanConvertTo(typeof(int)), Is.False);
        }

    }
}