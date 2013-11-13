using System;
using System.Collections.Generic;
using AmplaData.Data.Attributes;
using AmplaData.Data.Production;
using AmplaData.Data.Records;
using NUnit.Framework;

namespace AmplaData.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryDeletedFilterUnitTests :
        AmplaRepositoryTestFixture<AmplaRepositoryDeletedFilterUnitTests.DeletedModel>
    {

        [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
        [AmplaModule(Module = "Production")]
        [AmplaDefaultFilters("Deleted=")]
        public class DeletedModel
        {
            public int Id { get; set; }
            public bool Deleted { get; set; }
        }

        private const string module = "Production";
        private static readonly string[] Locations = new[] {"Enterprise.Site.Area.Point"};

        public AmplaRepositoryDeletedFilterUnitTests()
            : base(module, Locations, ProductionViews.AreaValueModelView)
        {
        }
        
        [Test]
        public void GetAllReturnsDeletedRecords()
        {
            DeletedModel match = new DeletedModel();
            DeletedModel deleted = new DeletedModel ();

            Repository.Add(match);
            Repository.Add(deleted);

            Assert.That(deleted.Id, Is.GreaterThan(0));

            Repository.Delete(deleted);
            Assert.That(Records.Count, Is.EqualTo(2));

            Assert.That(Records[0].IsDeleted(), Is.False);
            Assert.That(Records[1].IsDeleted(), Is.True);

            IList<DeletedModel> models = Repository.GetAll();

            Assert.That(models, Is.Not.Empty);
            Assert.That(models.Count, Is.EqualTo(2));

            Assert.That(models[0].Deleted, Is.False);
            Assert.That(models[1].Deleted, Is.True);
        }

    }
}