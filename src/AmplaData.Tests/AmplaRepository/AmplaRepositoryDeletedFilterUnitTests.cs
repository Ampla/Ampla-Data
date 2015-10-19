using System.Collections.Generic;
using AmplaData.Attributes;
using AmplaData.Modules.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.AmplaRepository
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
        
        [Test]
        public void FindByIdWithDeletedRecords()
        {
            DeletedModel model = new DeletedModel();

            Repository.Add(model);

            int id = model.Id;
            Assert.That(model.Id, Is.GreaterThan(0));

            DeletedModel find1 = Repository.FindById(id);
            Assert.That(find1, Is.Not.Null);
            Assert.That(find1.Deleted, Is.False);

            Repository.Delete(model);

            DeletedModel deleted = Repository.FindById(id);
            Assert.That(deleted, Is.Not.Null);
            Assert.That(deleted.Deleted, Is.True);
        }


        [Test]
        public void GetVersions()
        {
            DeletedModel model = new DeletedModel();
            Repository.Add(model);

            int id = model.Id;

            DeletedModel model1 = Repository.FindById(id) ;
            Assert.That(model1, Is.Not.Null);

            ModelVersions versions1 = Repository.GetVersions(id);
            Assert.That(versions1.Versions.Count, Is.EqualTo(1)); // current value

            AssertModelVersionProperty(versions1, 0, m => m.Deleted, Is.False);

            Repository.Delete(model1);
            DeletedModel model2 = Repository.FindById(id);
            Assert.That(model2, Is.Not.Null);
            Assert.That(model2.Deleted, Is.True);

            ModelVersions versions2 = Repository.GetVersions(id);
            Assert.That(versions2.Versions.Count, Is.EqualTo(2)); // current value and old value
            AssertModelVersionProperty(versions2, 0, m => m.Deleted, Is.EqualTo(false));
            AssertModelVersionProperty(versions2, 1, m => m.Deleted, Is.EqualTo(true));

            Assert.That(versions2.Versions[0].Display, Is.EqualTo("User created record"));
            Assert.That(versions2.Versions[1].Display, Is.EqualTo("User deleted record"));
        }

        [Test]
        public void ValidateMappings()
        {
            IList<string> messages = Repository.ValidateMapping(new DeletedModel());
            Assert.That(messages, Is.Empty);
        }

    }
}