using System;
using AmplaData.Binding.MetaData;

namespace AmplaData.Attributes.Calendar
{
    /// <summary>
    ///     Attribute used to denote the AmplaCalendar that is used for Ampla Model
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AmplaCalendarAttribute : Attribute
    {
        public AmplaCalendarAttribute()
            : this(null)
        {
        }

        public AmplaCalendarAttribute(string baseName)
        {
            BaseName = baseName;
        }

        public string BaseName { get; set; }

        /// <summary>
        ///     Try to get the base name from the type specified
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseName">Name of the base.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool TryGetBaseName(Type type, out string baseName)
        {
            baseName = null;
            AmplaCalendarAttribute attribute;
            if (type.TryGetAttribute(out attribute))
            {
                baseName = attribute.BaseName;
            }

            return !string.IsNullOrEmpty(baseName);
        }
    }
}