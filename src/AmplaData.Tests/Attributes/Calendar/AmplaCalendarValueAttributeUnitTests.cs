using System.Reflection;
using NUnit.Framework;

namespace AmplaData.Attributes.Calendar
{
    [TestFixture]
    [Ignore("Calendar still to be implemented")]
    public class AmplaCalendarValueAttributeUnitTests : TestFixture
    {
        public class ModelNoAttributes {}

        public class ModelWithField
        {
            [AmplaCalendarValue(Calendar = "Area.Tonnes.Target")]
            public virtual double TargetTonnes { get; set; }

            [AmplaCalendarValue("Area.Tonnes.Budget")]
            public double BudgetTonnes { get; set; }
        }

        public class InheritedModelWithField : ModelWithField
        {
        }

        public class ModelWithOverriddenField : ModelWithField
        {
            [AmplaCalendarValue("Another.Area.Tonnes.Target")]
            public override double TargetTonnes { get; set; }
        }

        public class ModelWithEmptyField
        {
            [AmplaCalendarValue]
            public virtual double PlannedTonnes { get; set; }
        }

        public class ModelWithDefaultField
        {
            [AmplaCalendarValue]
            public virtual double PlannedTonnes { get; set; }
        }

        [AmplaCalendar(BaseName = "Site.Area.")]
        public class BaseModelWithDefaultField
        {
            [AmplaCalendarValue]
            public virtual double PlannedTonnes { get; set; }
        }

        [Test]
        public void TryGetWithField()
        {
            string calendar;
            bool result = TryGetCalendar<ModelWithField>("TargetTonnes", out calendar);
        
            Assert.That(calendar, Is.EqualTo("Area.Tonnes.Target"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetInheritedWithField()
        {
            string calendar;
            bool result = TryGetCalendar<InheritedModelWithField>("TargetTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo("Area.Tonnes.Target"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithOverriddenField()
        {
            string calendar;
            bool result = TryGetCalendar<ModelWithOverriddenField>("TargetTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo("Another.Area.Tonnes.Target"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithField()
        {
            string calendar;
            bool result = TryGetCalendar<ModelWithField>("TargetTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo("Area.Tonnes.Target"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithEmptyField()
        {
            string calendar;
            bool result = TryGetCalendar<ModelWithEmptyField>("PlannedTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo("PlannedTonnes"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetModelWithDefaultField()
        {
            string calendar;
            bool result = TryGetCalendar<ModelWithDefaultField>("PlannedTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo("ModelWithDefaultField.PlannedTonnes"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetBaseModelWithDefaultField()
        {
            string calendar;
            bool result = TryGetCalendar<BaseModelWithDefaultField>("PlannedTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo("Site.Area.PlannedTonnes"));
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryGetBaseModelGetInvalidField()
        {
            string calendar;
            bool result = TryGetCalendar<BaseModelWithDefaultField>("InvalidProperty", out calendar);

            Assert.That(calendar, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetModelNoAttribute()
        {
            string calendar;
            bool result = TryGetCalendar<ModelNoAttributes>("PlannedTonnes", out calendar);

            Assert.That(calendar, Is.EqualTo(null));
            Assert.That(result, Is.False);
        }

        private bool TryGetCalendar<TModel>(string propertyName, out string calendar)
        {
            foreach (PropertyInfo property in typeof (TModel).GetProperties())
            {
                if (property.Name == propertyName)
                {
                    return AmplaCalendarValueAttribute.TryGetCalendar(property, out calendar);
                }
            }
            calendar = null;
            return false;
        }
    }
}