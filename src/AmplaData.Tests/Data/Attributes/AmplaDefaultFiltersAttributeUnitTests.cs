using NUnit.Framework;

namespace AmplaData.Data.Attributes
{
    [TestFixture]
    public class AmplaDefaultFiltersAttributeUnitTests : TestFixture
    {
        [AmplaDefaultFilters("Sample Period = Current Shift")]
        public class ModelWithDefaultFilter
        {
        }

        [AmplaDefaultFilters("Sample Period ={Current Shift}")]
        public class ModelWithAlternateFormat
        {
        }

        public class InheritedModelWithDefaultFilter : ModelWithDefaultFilter
        {
        }

        [AmplaDefaultFilters("Sample Period = Current Shift", "Confirmed = True")]
        public class OverriddenModelWithDefaultFilter : ModelWithDefaultFilter
        {
        }

        [AmplaDefaultFilters]
        public class ModelWithNoDefaultFilter
        {
        }

        [AmplaDefaultFilters("")]
        public class ModelWithEmptyDefaultFilter
        {
        }

        [AmplaDefaultFilters("Invalid")]
        public class ModelWithInvalidDefaultFilter
        {
        }

        public class ModelNoAttribute
        {
        }

        [AmplaDefaultFilters("Deleted={}")]
        public class ModelWithEmptyAlternateDeletedFilter
        {
        }

        [AmplaDefaultFilters("Deleted=")]
        public class ModelWithEmptyDeletedFilter
        {
        }

        [Test]
        public void TryGetWithDefaultFilter()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithDefaultFilter>(out filterValues);

            Assert.That(filterValues, Is.Not.Empty);
            Assert.That(filterValues.Length, Is.EqualTo(1));
            Assert.That(filterValues[0].Name, Is.EqualTo("Sample Period"));
            Assert.That(filterValues[0].Value, Is.EqualTo("Current Shift"));

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithAlternateFilter()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithAlternateFormat>(out filterValues);

            Assert.That(filterValues, Is.Not.Empty);
            Assert.That(filterValues.Length, Is.EqualTo(1));
            Assert.That(filterValues[0].Name, Is.EqualTo("Sample Period"));
            Assert.That(filterValues[0].Value, Is.EqualTo("Current Shift"));

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithDefaultFilter()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<InheritedModelWithDefaultFilter>(out filterValues);

            Assert.That(filterValues, Is.Not.Empty);
            Assert.That(filterValues.Length, Is.EqualTo(1));
            Assert.That(filterValues[0].Name, Is.EqualTo("Sample Period"));
            Assert.That(filterValues[0].Value, Is.EqualTo("Current Shift"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetOverriddenModelWithDefaultFilter()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<OverriddenModelWithDefaultFilter>(out filterValues);

            Assert.That(filterValues, Is.Not.Empty);
            Assert.That(filterValues.Length, Is.EqualTo(2));
            Assert.That(filterValues[0].Name, Is.EqualTo("Sample Period"));
            Assert.That(filterValues[0].Value, Is.EqualTo("Current Shift"));
            Assert.That(filterValues[1].Name, Is.EqualTo("Confirmed"));
            Assert.That(filterValues[1].Value, Is.EqualTo("True"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetWithNoDefaultFilter()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithNoDefaultFilter>(out filterValues);

            Assert.That(filterValues, Is.Empty);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithNoAttribute()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelNoAttribute>(out filterValues);

            Assert.That(filterValues, Is.Empty);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithEmptyDefaultFilter()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithEmptyDefaultFilter>(out filterValues);

            Assert.That(filterValues, Is.Empty);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithInvalidAttribute()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithInvalidDefaultFilter>(out filterValues);

            Assert.That(filterValues, Is.Empty);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetWithEmptyDeletedAlternate()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithEmptyAlternateDeletedFilter>(out filterValues);

            Assert.That(filterValues, Is.Not.Empty);
            Assert.That(result, Is.True);
            Assert.That(filterValues[0].Name, Is.EqualTo("Deleted"));
            Assert.That(filterValues[0].Value, Is.EqualTo(""));
        }

        [Test]
        public void TryGetWithEmptyDeleted()
        {
            FilterValue[] filterValues;
            bool result = AmplaDefaultFiltersAttribute.TryGetFilter<ModelWithEmptyDeletedFilter>(out filterValues);

            Assert.That(filterValues, Is.Not.Empty);
            Assert.That(result, Is.True);
            Assert.That(filterValues[0].Name, Is.EqualTo("Deleted"));
            Assert.That(filterValues[0].Value, Is.EqualTo(""));
        }
    }
}