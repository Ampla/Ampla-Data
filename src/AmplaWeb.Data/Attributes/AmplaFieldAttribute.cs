using System;
using System.Reflection;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Attributes
{
    /// <summary>
    ///     Attribute used to mark a property as an Ampla Model
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AmplaFieldAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaFieldAttribute"/> class.
        /// </summary>
        public AmplaFieldAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaFieldAttribute" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public AmplaFieldAttribute(string field)
        {
            Field = field;
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Field { get; set; }

        /// <summary>
        /// Tries to get the Field value from the specified property.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static bool TryGetField(PropertyInfo propertyInfo, out string field)
        {
            field = null;
            AmplaFieldAttribute attribute;
            if (propertyInfo.TryGetAttribute(out attribute))
            {
                field = attribute.Field;
            }
            if (string.IsNullOrEmpty(field))
            {
                field = propertyInfo.Name;
            }
            
            return !string.IsNullOrEmpty(field);
        }
    }
}