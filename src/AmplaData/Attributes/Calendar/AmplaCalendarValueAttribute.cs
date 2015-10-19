using System;
using System.Reflection;
using AmplaData.Binding.MetaData;

namespace AmplaData.Attributes.Calendar
{
    /// <summary>
    ///     Attribute used to mark a property as an Ampla Calendar Value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AmplaCalendarValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaFieldAttribute"/> class.
        /// </summary>
        public AmplaCalendarValueAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaCalendarValueAttribute" /> class.
        /// </summary>
        /// <param name="calendar"></param>
        public AmplaCalendarValueAttribute(string calendar)
        {
            Calendar = calendar;
        }

        /// <summary>
        /// Gets or sets the Calendar string.
        /// </summary>
        /// <value>
        /// The Calendar string.
        /// </value>
        public string Calendar { get; set; }

        /// <summary>
        /// Tries to get the Calendar value from the specified property.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="calendar">The field.</param>
        /// <returns></returns>
        public static bool TryGetCalendar(PropertyInfo propertyInfo, out string calendar)
        {
            calendar = null;
            AmplaCalendarValueAttribute attribute;
            if (propertyInfo.TryGetAttribute(out attribute))
            {
                calendar = attribute.Calendar;
                if (string.IsNullOrEmpty(calendar))
                {
                    calendar = propertyInfo.Name;
                }
            }
            if (string.IsNullOrEmpty(calendar))
            {
                calendar = propertyInfo.Name;
            }
            
            return !string.IsNullOrEmpty(calendar);
        }
    }
}