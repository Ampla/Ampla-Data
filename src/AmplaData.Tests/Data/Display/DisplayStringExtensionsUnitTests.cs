using NUnit.Framework;

namespace AmplaData.Data.Display
{
    [TestFixture]
    public class DisplayStringExtensionsUnitTests : TestFixture
    {
        [Test]
        public void ToSeparatedWords()
        {
            Assert.That("Model".ToSeparatedWords(), Is.EqualTo("Model"));
            Assert.That("model".ToSeparatedWords(), Is.EqualTo("model"));

            Assert.That("DowntimeModel".ToSeparatedWords(), Is.EqualTo("Downtime Model"));
            Assert.That("XMLModel".ToSeparatedWords(), Is.EqualTo("XML Model"));

        }
    }
}