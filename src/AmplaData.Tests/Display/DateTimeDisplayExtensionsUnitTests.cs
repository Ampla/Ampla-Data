using System;
using NUnit.Framework;

namespace AmplaData.Display
{
    [TestFixture]
    public class DateTimeDisplayExtensionsUnitTests : TestFixture
    {
        [Test]
        public void OneHourAgo()
        {
            DateTime time = DateTime.UtcNow.AddHours(-1);
            string display = time.ToRelativeTimeMini();

            Assert.That(display, Is.EqualTo("1h ago"));
        }

        [Test]
        public void OneMinuteAgo()
        {
            DateTime time = DateTime.UtcNow.AddMinutes(-1);
            string display = time.ToRelativeTimeMini();

            Assert.That(display, Is.EqualTo("1m ago"));
        }

        [Test]
        public void OneSecondAgo()
        {
            DateTime time = DateTime.UtcNow.AddSeconds(-1);
            string display = time.ToRelativeTimeMini();

            Assert.That(display, Is.EqualTo("1s ago"));
        }

        [Test]
        public void OneSecondAgoLocal()
        {
            DateTime time = DateTime.Now.AddSeconds(-1);
            string display = time.ToRelativeTimeMini();

            Assert.That(display, Is.EqualTo("1s ago"));
        }
    }
}