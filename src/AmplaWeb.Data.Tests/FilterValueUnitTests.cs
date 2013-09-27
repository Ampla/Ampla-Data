using System;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data
{
    [TestFixture]
    public class FilterValueUnitTests : TestFixture
    {
         [Test]
         public void Parse()
         {
             FilterValue filter = FilterValue.Parse("Filter=Value");
             Assert.That(filter, Is.Not.Null);
             Assert.That(filter.Name, Is.EqualTo("Filter"));
             Assert.That(filter.Value, Is.EqualTo("Value"));
         }

         [Test]
         public void ParseAlternate()
         {
             FilterValue filter = FilterValue.Parse("Filter={Value}");
             Assert.That(filter, Is.Not.Null);
             Assert.That(filter.Name, Is.EqualTo("Filter"));
             Assert.That(filter.Value, Is.EqualTo("Value"));
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
             string[] invalidFilters = new [] {"Filter=", "Filter ={}", "=Value", " = Value", "=", " = "};
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
             string[] invalidFilters = new[] { null, string.Empty, "Filter=", "Filter ={}", "=Value", " = Value", "=", " = " };
             foreach (string filter in invalidFilters)
             {
                 FilterValue filterValue;
                 bool result = FilterValue.TryParse(filter, out filterValue);
                 Assert.That(filterValue, Is.Null, filter);
                 Assert.That(result, Is.False, filter);
             }
         }

    }
}