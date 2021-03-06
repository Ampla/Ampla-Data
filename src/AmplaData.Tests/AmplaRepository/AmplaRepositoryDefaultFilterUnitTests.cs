﻿using System;
using System.Collections.Generic;
using AmplaData.Attributes;
using AmplaData.Modules.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryDefaultFilterUnitTests : AmplaRepositoryTestFixture<AmplaRepositoryDefaultFilterUnitTests.AreaModel>
    {

        [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
        [AmplaModule(Module = "Production")]
        [AmplaDefaultFilters("Area=ROM")]
        public class AreaModel
        {
            public int Id { get; set; }

            [AmplaField(Field = "Sample Period")]
            public DateTime Sample { get; set; }

            public string Area { get; set; }
            public double Value { get; set; }
        }

        private const string module = "Production";
        private static readonly string[] Locations =  new [] {"Enterprise.Site.Area.Point"} ;

        public AmplaRepositoryDefaultFilterUnitTests() : base(module, Locations, ProductionViews.AreaValueModelView)
        {
        }

        [Test]
        public void GetAll()
        {
            AreaModel match = new AreaModel {Area = "ROM", Value = 100};
            AreaModel noMatch = new AreaModel {Area = "Mining", Value = 200};

            Repository.Add(match);
            Repository.Add(noMatch);

            Assert.That(Records.Count, Is.EqualTo(2));

            IList<AreaModel> models = Repository.GetAll();

            Assert.That(models, Is.Not.Empty);
            Assert.That(models.Count, Is.EqualTo(1));
        }

        [Test]
        public void FindById()
        {
            AreaModel match = new AreaModel { Area = "ROM", Value = 100 };
            AreaModel noMatch = new AreaModel { Area = "Mining", Value = 200 };

            Repository.Add(match);
            Repository.Add(noMatch);

            Assert.That(Records.Count, Is.EqualTo(2));
            int matchId = match.Id;
            int noMatchId = noMatch.Id;

            AreaModel areaModel = Repository.FindById(matchId);

            Assert.That(areaModel, Is.Not.Null);
            Assert.That(areaModel.Area, Is.EqualTo("ROM"));
            Assert.That(areaModel.Value, Is.EqualTo(100));

            areaModel = Repository.FindById(noMatchId);
            Assert.That(areaModel, Is.Null);
        }

        [Test]
        public void FindByFilter()
        {
            AreaModel match = new AreaModel { Area = "ROM", Value = 200 };
            AreaModel noMatch = new AreaModel { Area = "ROM", Value = 100 };
            AreaModel extra = new AreaModel { Area = "Mining", Value = 100 };

            Repository.Add(match);
            Repository.Add(noMatch);
            Repository.Add(extra);

            Assert.That(Records.Count, Is.EqualTo(3));
          
            IList<AreaModel> models = Repository.FindByFilter();

            Assert.That(models, Is.Not.Empty);
            Assert.That(models.Count, Is.EqualTo(2));
            Assert.That(models[0].Area, Is.EqualTo("ROM"));
            Assert.That(models[1].Area, Is.EqualTo("ROM"));

            models = Repository.FindByFilter(FilterValue.Parse("Value=100"));

            Assert.That(models, Is.Not.Empty);
            Assert.That(models.Count, Is.EqualTo(1));
            Assert.That(models[0].Area, Is.EqualTo("ROM"));
            Assert.That(models[0].Value, Is.EqualTo(100));
        }

        [Test]
        public void FindRecord()
        {
            AreaModel match = new AreaModel { Area = "ROM", Value = 200 };
            AreaModel noMatch = new AreaModel { Area = "ROM", Value = 100 };
            AreaModel extra = new AreaModel { Area = "Mining", Value = 100 };

            Repository.Add(match);
            Repository.Add(noMatch);
            Repository.Add(extra);

            Assert.That(Records.Count, Is.EqualTo(3));

            AmplaRecord record = Repository.FindRecord(match.Id);

            Assert.That(record, Is.Not.Null);
            Assert.That(record.GetValue("Area"), Is.EqualTo("ROM"));

            // it should not find a record that doesn't match the default filter 
            record = Repository.FindRecord(extra.Id);

            Assert.That(record, Is.Null);
        }

        [Test]
        public void GetAllIgnoresDeleted()
        {
            AreaModel match = new AreaModel { Area = "ROM", Value = 100 };
            AreaModel noMatch = new AreaModel { Area = "Mining", Value = 200 };
            AreaModel deleted = new AreaModel {Area = "ROM", Value = 300};

            Repository.Add(match);
            Repository.Add(noMatch);
            Repository.Add(deleted);

            Repository.Delete(deleted);

            Assert.That(Records.Count, Is.EqualTo(3));

            IList<AreaModel> models = Repository.GetAll();

            Assert.That(models, Is.Not.Empty);
            Assert.That(models.Count, Is.EqualTo(1));

            Assert.That(models[0].Value, Is.EqualTo(100));
        }

        [Test]
        public void GetVersions()
        {
            AreaModel model = new AreaModel {Area = "ROM", Value = 100};
            Repository.Add(model);

            int id = model.Id;

            AreaModel model1 = Repository.FindById(id);
            Assert.That(model1, Is.Not.Null);

            ModelVersions versions1 = Repository.GetVersions(id);
            Assert.That(versions1.Versions.Count, Is.EqualTo(1)); // current value

            AssertModelVersionProperty(versions1, 0, m => m.Value, Is.EqualTo(100));

            model1.Value = 150;
            Repository.Update(model1);

            ModelVersions versions2 = Repository.GetVersions(id);
            Assert.That(versions2.Versions.Count, Is.EqualTo(2)); // current value and old value
            AssertModelVersionProperty(versions2, 0, m => m.Value, Is.EqualTo(100));
            AssertModelVersionProperty(versions2, 1, m => m.Value, Is.EqualTo(150));
        }

        [Test]
        public void ValidateMappings()
        {
            IList<string> messages = Repository.ValidateMapping(new AreaModel());
            Assert.That(messages, Is.Empty);
        }

    }
}