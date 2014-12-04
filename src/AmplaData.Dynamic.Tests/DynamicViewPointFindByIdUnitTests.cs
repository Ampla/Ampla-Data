using AmplaData.Modules.Production;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace AmplaData.Dynamic
{
    [TestFixture]
    public class DynamicViewPointFindByIdUnitTests : DynamicViewPointTestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        private int id;

        public DynamicViewPointFindByIdUnitTests() : base(location, module)
        {
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            id = AddExisingRecord(ProductionRecords.NewRecord());
        }

        [Test]
        public void FindNamedArgument()
        {
            dynamic result = DynamicViewPoint.Find(Id: id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Location, Is.EqualTo(location));
        }

        [Test]
        public void FindById()
        {
            dynamic result = DynamicViewPoint.FindById(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Location, Is.EqualTo(location));
        }

        [Test]
        public void FindIgnoreArgCase()
        {
            dynamic result = DynamicViewPoint.Find(id: id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Location, Is.EqualTo(location));
        }

        [Test]
        public void LowercaseFind()
        {
            Assert.Throws<RuntimeBinderException>(() => DynamicViewPoint.find(id));
        }

        [Test]
        public void Find()
        {
            dynamic result = DynamicViewPoint.Find(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Location, Is.EqualTo(location));
        }

        [Test]
        public void FindWithSetId()
        {
            Assert.Throws<RuntimeBinderException>(() => DynamicViewPoint.Find(SetId: id));
        }

        [Test]
        public void FindByWithNamedAndPositional()
        {
            Assert.Throws<RuntimeBinderException>(() => DynamicViewPoint.Find(id, Id: 200));
        }

        [Test]
        public void FindWithStringArg()
        {
            Assert.Throws<RuntimeBinderException>(() => DynamicViewPoint.Find("200"));
        }
    }
}