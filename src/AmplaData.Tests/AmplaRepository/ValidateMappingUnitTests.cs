using System;
using System.Collections.Generic;
using System.Linq;
using AmplaData.Attributes;
using AmplaData.Modules.Production;
using NUnit.Framework;

namespace AmplaData.AmplaRepository
{
    [TestFixture]
    public class ValidateMappingUnitTests : AmplaRepositoryTestFixture<ValidateMappingUnitTests.InvalidModel>
    {
        [AmplaLocation(Location = "Enterprise.Site.Area.Production")]
        [AmplaModule(Module = "Production")]
        public class InvalidModel
        {
            public int Id { get; set; }

            [AmplaField(Field = "Sample Period")]
            public DateTime Sample { get; set; }

            [AmplaField(Field = "IncorrectSpelling")]
            public string IncorrectSpelling { get; set; }

            [AmplaField(Field = "Area")]
            public int Area { get; set; }

            [AmplaField(Field = "Value")]
            public DateTime Value { get; set; }
        }

        private const string module = "Production";
        private const string location = "Enterprise.Site.Area.Production";

        public ValidateMappingUnitTests() : base(module, location, ProductionViews.AreaValueModelView)
        {
        }

        [Test]
        public void ValidateInvalidField()
        {
            IList<string> messages = Repository.ValidateMapping(new InvalidModel());
            Assert.That(messages, Is.Not.Empty);

            string invalidMessage = messages.FirstOrDefault(message => message.Contains("IncorrectSpelling"));

            Assert.That(invalidMessage, Is.StringContaining("'IncorrectSpelling'"), string.Join("\r\n", messages));
        }

        [Test]
        public void ValidateAreaField()
        {
            IList<string> messages = Repository.ValidateMapping(new InvalidModel());
            Assert.That(messages, Is.Not.Empty);

            string invalidMessage = messages.FirstOrDefault(message => message.Contains("Area"));

            Assert.That(invalidMessage, Is.StringContaining("'Area'"), string.Join("\r\n", messages));
        }

        [Test]
        public void ValidateValueField()
        {
            IList<string> messages = Repository.ValidateMapping(new InvalidModel());
            Assert.That(messages, Is.Not.Empty);

            string invalidMessage = messages.FirstOrDefault(message => message.Contains("Value"));

            Assert.That(invalidMessage, Is.StringContaining("'Value'"), string.Join("\r\n", messages));
        }
    }
}