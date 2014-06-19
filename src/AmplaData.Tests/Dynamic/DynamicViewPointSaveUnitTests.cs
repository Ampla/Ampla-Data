using System;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Dynamic
{
    [TestFixture]
    public class DynamicViewPointSaveUnitTests : DynamicViewPointTestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        public DynamicViewPointSaveUnitTests() : base(location, module)
        {
        }

        [Test]
        public void SaveUsingAnonymousType()
        {
            var model = new {Location = location};
            DateTime before = DateTime.UtcNow.AddSeconds(-1);

            dynamic record = ViewPoint.Save(model);

            DateTime after = before.AddSeconds(5);

            Assert.That((object)record, Is.Not.Null);

            Assert.That(record.Location, Is.EqualTo(location));

            Assert.That(record.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord amplaRecord = Records[0];

            Assert.That(amplaRecord, Is.Not.Null);

            Assert.That(amplaRecord.Location, Is.EqualTo(location));
            Assert.That(amplaRecord.Module, Is.EqualTo(module));

            Assert.That(amplaRecord.GetFieldValue("Sample Period", DateTime.MinValue), Is.InRange(before, after));
        }

        public class ProductionModel
        {
            public string Location { get; set; }
        }

        [Test]
        public void SaveUsingModelClass()
        {
            ProductionModel model = new ProductionModel {Location = location};

            DateTime before = DateTime.UtcNow.AddSeconds(-1);

            dynamic record = ViewPoint.Save(model);

            DateTime after = before.AddSeconds(5);

            Assert.That((object)record, Is.Not.Null);

            Assert.That(record.Location, Is.EqualTo(location));

            Assert.That(record.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord amplaRecord = Records[0];

            Assert.That(amplaRecord, Is.Not.Null);

            Assert.That(amplaRecord.Location, Is.EqualTo(location));
            Assert.That(amplaRecord.Module, Is.EqualTo(module));

            Assert.That(amplaRecord.GetFieldValue("Sample Period", DateTime.MinValue), Is.InRange(before, after));
        }

        //[Test]
        //public void SaveWithSamplePeriod()
        //{
        //    DateTime local = DateTime.Today;
        //    DateTime utc = local.ToUniversalTime();

        //    var model = new { Location = location, SamplePeriod = local };
        //    dynamic record = ViewPoint.Save(model);

        //    Assert.That((object)record, Is.Not.Null);

        //    Assert.That(record.Location, Is.EqualTo(location));
        //    Assert.That(record.Id, Is.GreaterThan(0));

        //    Assert.That(Records, Is.Not.Empty);
        //    InMemoryRecord amplaRecord = Records[0];

        //    Assert.That(amplaRecord, Is.Not.Null);

        //    Assert.That(amplaRecord.Location, Is.EqualTo(location));
        //    Assert.That(amplaRecord.Module, Is.EqualTo(module));

        //    Assert.That(amplaRecord.GetFieldValue("Sample Period", DateTime.MinValue), Is.EqualTo(utc));
            
        //}
    }
}