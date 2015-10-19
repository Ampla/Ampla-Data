using System;
using NUnit.Framework;

namespace AmplaData.Binding.MetaData
{
    [TestFixture]
    public class TimeSpanIntConverterUnitTests : TestFixture
    {
        private TimeSpanIntConverter typeConverter;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            typeConverter = new TimeSpanIntConverter();
        }

        [Test]
        public void ConvertToString()
        {
            TimeSpan from = TimeSpan.FromMinutes(45);
            string value = typeConverter.ConvertToInvariantString(from);
            Assert.That(value, Is.EqualTo("2700"));  // 45*60
        }

        [Test]
        public void ConvertFromString()
        {
            const string value = "2700"; //45*60;
            Assert.That(typeConverter.ConvertFromInvariantString(value), Is.EqualTo(TimeSpan.FromMinutes(45)));
        }

        [Test]
        public void ConvertTimeSpanToTimeSpan()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(45);
            Assert.That(typeConverter.ConvertTo(timeSpan, typeof(TimeSpan)), Is.EqualTo(TimeSpan.FromMinutes(45)));
        }

        [Test]
        public void ConvertTimeSpanToInt()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(45);
            Assert.That(typeConverter.ConvertTo(timeSpan, typeof(int)), Is.EqualTo(45 * 60));
        }

        [Test]
        public void ConvertTimeSpanToDouble()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(45);
            Assert.That(typeConverter.ConvertTo(timeSpan, typeof(double)), Is.EqualTo(45.0d * 60));
        }

        [Test]
        public void ConvertIntToTimeSpan()
        {
            const int value = 45*60;
            Assert.That(typeConverter.ConvertTo(value, typeof(TimeSpan)), Is.EqualTo(TimeSpan.FromMinutes(45)));
        }

        [Test]
        public void ConvertDoubleToTimeSpan()
        {
            const double value = 45 * 60;
            Assert.That(typeConverter.ConvertTo(value, typeof(TimeSpan)), Is.EqualTo(TimeSpan.FromMinutes(45)));
        }
        
        [Test]
        public void ConvertToFromString()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(string)), Is.True);
            Assert.That(typeConverter.CanConvertTo(typeof(string)), Is.True);
        }

        [Test]
        public void ConvertToFromTimeSpan()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(TimeSpan)), Is.True);
            Assert.That(typeConverter.CanConvertTo(typeof(TimeSpan)), Is.True);
        }

        [Test]
        public void ConvertToFromDouble()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(double)), Is.True);
            Assert.That(typeConverter.CanConvertTo(typeof(double)), Is.True);
        }


        [Test]
        public void ConvertToFromInt()
        {
            Assert.That(typeConverter.CanConvertFrom(typeof(int)), Is.True);
            Assert.That(typeConverter.CanConvertTo(typeof(int)), Is.True);
        } 
    }
}