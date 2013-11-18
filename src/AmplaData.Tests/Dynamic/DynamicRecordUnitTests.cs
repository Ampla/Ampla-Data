using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace AmplaData.Dynamic
{
    [TestFixture]
    public class DynamicRecordUnitTests : TestFixture
    {
        [Test]
        public void LocationProperty()
        {
            dynamic record = new DynamicRecord("Production");

            string location = null;
            Assert.Throws<RuntimeBinderException>(() => location = record.Location);
            Assert.That(location, Is.Null);

            IRecordLoad recordLoad = record;
            recordLoad.AddColumn("Location", typeof (string));

            recordLoad.SetValue("Location", "Enterprise.Site.Area.Production");

            Assert.That(record.Location, Is.EqualTo("Enterprise.Site.Area.Production"));
        }

        [Test]
        public void LocationIndex()
        {
            dynamic record = new DynamicRecord("Production");

            string location = null;
            Assert.Throws<RuntimeBinderException>(() => location = record.Location);
            Assert.That(location, Is.Null);

            IRecordLoad recordLoad = record;
            recordLoad.AddColumn("Location", typeof(string));

            recordLoad.SetValue("Location", "Enterprise.Site.Area.Production");

            Assert.That(record["Location"], Is.EqualTo("Enterprise.Site.Area.Production"));
        }
        
        [Test]
        public void EmptyValueProperty()
        {
            dynamic record = new DynamicRecord("Production");

            int noValue = 100;
            Assert.Throws<RuntimeBinderException>(() => noValue = record.Value);
            Assert.That(noValue, Is.EqualTo(100));

            IRecordLoad recordLoad = record;
            recordLoad.AddColumn("Value", typeof(int));

            object value = record.Value;
            Assert.That(value, Is.EqualTo(0));
            
            int intValue = record.Value;
            Assert.That(intValue, Is.EqualTo(0));

            recordLoad.SetValue("Value", "100");

            Assert.That(record.Value, Is.EqualTo(100));
        }

        [Test]
        public void EmptyValueIndex()
        {
            dynamic record = new DynamicRecord("Production");

            int noValue = 100;
            Assert.Throws<RuntimeBinderException>(() => noValue = record["Value"]);
            Assert.That(noValue, Is.EqualTo(100));

            IRecordLoad recordLoad = record;
            recordLoad.AddColumn("Value", typeof(int));

            object value = record["Value"];
            Assert.That(value, Is.EqualTo(0));

            int intValue = record["Value"];
            Assert.That(intValue, Is.EqualTo(0));

            recordLoad.SetValue("Value", "100");

            Assert.That(record["Value"], Is.EqualTo(100));
        }

        [Test]
        public void IndexIsCaseSensitive()
        {
            dynamic record = new DynamicRecord("Production");

            int dummyValue = 100;
            Assert.Throws<RuntimeBinderException>(() => dummyValue = record["Value"]);
            Assert.That(dummyValue, Is.EqualTo(100));

            IRecordLoad recordLoad = record;
            recordLoad.AddColumn("Value", typeof(int));

            object value = record["Value"];
            Assert.That(value, Is.EqualTo(0));

            int intValue = record["Value"];
            Assert.That(intValue, Is.EqualTo(0));

            recordLoad.SetValue("Value", "100");
            Assert.That(record["Value"], Is.EqualTo(100));

            Assert.Throws<RuntimeBinderException>(() => dummyValue = record["value"]);
            Assert.Throws<RuntimeBinderException>(() => dummyValue = record["vALUE"]);
        }
    }
}