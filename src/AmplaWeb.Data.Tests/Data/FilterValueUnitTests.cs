using System;
using NUnit.Framework;

namespace AmplaWeb.Data
{
    [TestFixture]
    public class FilterValueUnitTests : TestFixture
    {
        private void AssertParseIsValid(string filter, string name, string value)
        {
            FilterValue actual = FilterValue.Parse(filter);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Name, Is.EqualTo(name));
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void Parse()
        {
            AssertParseIsValid("Filter=Value", "Filter", "Value");
        }

        [Test]
        public void ParseAlternate()
        {
            AssertParseIsValid("Filter={Value}", "Filter", "Value");
        }

        [Test]
        public void ParseNull()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => FilterValue.Parse(null));
            Assert.That(exception.Message, Is.StringContaining("Filter=Value"));
        }

        [Test]
        public void ParseEmpty()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => FilterValue.Parse(""));
            Assert.That(exception.Message, Is.StringContaining("Filter=Value"));
        }

        [Test]
        public void ParseInvalid()
        {
            string[] invalidFilters = new[] {"=Value", " = Value", "=", " = "};
            foreach (string filter in invalidFilters)
            {
                ArgumentException exception = Assert.Throws<ArgumentException>(() => FilterValue.Parse(filter), filter);
                Assert.That(exception.Message, Is.StringContaining("Filter=Value"), filter);
            }
        }

        [Test]
        public void TryParse()
        {
            FilterValue filter;
            bool result = FilterValue.TryParse("Filter=Value", out filter);
            Assert.That(filter, Is.Not.Null);
            Assert.That(filter.Name, Is.EqualTo("Filter"));
            Assert.That(filter.Value, Is.EqualTo("Value"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryParseAlternate()
        {
            FilterValue filter;
            bool result = FilterValue.TryParse("Filter={Value}", out filter);
            Assert.That(filter, Is.Not.Null);
            Assert.That(filter.Name, Is.EqualTo("Filter"));
            Assert.That(filter.Value, Is.EqualTo("Value"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryParseInvalid()
        {
            string[] invalidFilters = new[]
                {null, string.Empty, "=Value", " = Value", "=", " = "};
            foreach (string filter in invalidFilters)
            {
                FilterValue filterValue;
                bool result = FilterValue.TryParse(filter, out filterValue);
                Assert.That(filterValue, Is.Null, filter);
                Assert.That(result, Is.False, filter);
            }
        }

        [Test]
        public void ParseEmptyValue()
        {
            AssertParseIsValid("Filter=", "Filter", "");
            AssertParseIsValid("Filter={}", "Filter", "");
            AssertParseIsValid("Filter = ", "Filter", "");
            AssertParseIsValid("Filter ={}", "Filter", "");
        }

        [Test]
        public void ParseDeleted()
        {
            AssertParseIsValid("Deleted=True", "Deleted", "True");
            AssertParseIsValid("Deleted=False", "Deleted", "False");
            AssertParseIsValid("Deleted=", "Deleted", "");
        }

        [Test]
        public void ParseDeletedAlternate()
        {
            AssertParseIsValid("Deleted={True}", "Deleted", "True");
            AssertParseIsValid("Deleted={False}", "Deleted", "False");
            AssertParseIsValid("Deleted={}", "Deleted", "");
        }

    }
}