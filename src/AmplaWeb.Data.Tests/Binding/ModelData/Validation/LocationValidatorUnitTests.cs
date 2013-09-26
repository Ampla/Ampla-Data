using System;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ModelData.Validation
{
    [TestFixture]
    public class LocationValidatorUnitTests : TestFixture
    {
        [AmplaLocation("Enterprise.Site.Area")]
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

            LocationValidator<LocationModel> validator = new LocationValidator<LocationModel>();
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

            LocationValidator<LocationModel> validator = new LocationValidator<LocationModel>();
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

            LocationValidator<LocationModel> validator = new LocationValidator<LocationModel>();
            ValidationMessages messages = new ValidationMessages();
            bool isValid = validator.Validate(modelProperties, model, messages);

            Assert.That(isValid, Is.False);
            Assert.That(messages.Count, Is.EqualTo(1));
        }
    }
}