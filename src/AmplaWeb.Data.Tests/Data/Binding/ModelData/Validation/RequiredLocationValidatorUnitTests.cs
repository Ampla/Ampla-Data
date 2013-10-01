using System;
using AmplaWeb.Data.Attributes;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ModelData.Validation
{
    [TestFixture]
    public class RequiredLocationValidatorUnitTests : TestFixture
    {
        [AmplaLocation(Location ="Enterprise", WithRecurse = true)]
        [AmplaModule(Module = "Production")]
        public class LocationModel
        {
            public string Location { get; set; }
        }
        
        [Test]
        public void ValidModel()
        {
            ModelProperties<LocationModel> modelProperties = new ModelProperties<LocationModel>();
            LocationModel model = new LocationModel {Location = "Enterprise.Site.Area"};

            RequiredLocationValidator<LocationModel> validator = new RequiredLocationValidator<LocationModel>("Enterprise.Site.Area");
            ValidationMessages messages = new ValidationMessages();
            bool isValid = validator.Validate(modelProperties, model, messages);

            Assert.That(isValid, Is.True);
            Assert.That(messages.Count, Is.EqualTo(0));
        }

        [Test]
        public void NullLocation()
        {
            ModelProperties<LocationModel> modelProperties = new ModelProperties<LocationModel>();
            LocationModel model = new LocationModel {Location = null};

            RequiredLocationValidator<LocationModel> validator = new RequiredLocationValidator<LocationModel>("Enterprise.Site.Area");
            ValidationMessages messages = new ValidationMessages();
            bool isValid = validator.Validate(modelProperties, model, messages);

            Assert.That(isValid, Is.False);
            Assert.That(messages.Count, Is.EqualTo(1));
        }

        [Test]
        public void EmptyLocation()
        {
            ModelProperties<LocationModel> modelProperties = new ModelProperties<LocationModel>();
            LocationModel model = new LocationModel { Location = "" };

            RequiredLocationValidator<LocationModel> validator = new RequiredLocationValidator<LocationModel>("Enterprise.Site.Area");
            ValidationMessages messages = new ValidationMessages();
            bool isValid = validator.Validate(modelProperties, model, messages);

            Assert.That(isValid, Is.False);
            Assert.That(messages.Count, Is.EqualTo(1));
        }

        [Test]
        public void DifferentLocation()
        {
            ModelProperties<LocationModel> modelProperties = new ModelProperties<LocationModel>();
            LocationModel model = new LocationModel { Location = "Enterprise.Site.Point" };

            RequiredLocationValidator<LocationModel> validator = new RequiredLocationValidator<LocationModel>("Enterprise.Site.Area");
            ValidationMessages messages = new ValidationMessages();
            bool isValid = validator.Validate(modelProperties, model, messages);

            Assert.That(isValid, Is.False);
            Assert.That(messages.Count, Is.EqualTo(1));
        }

    }
}