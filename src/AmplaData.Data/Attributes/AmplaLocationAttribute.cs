using System;
using AmplaData.Binding.MetaData;
using AmplaData.Binding.ModelData;

namespace AmplaData.Attributes
{
    /// <summary>
    ///     Attribute used to mark an class as an Ampla Model
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AmplaLocationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaLocationAttribute"/> class.
        /// </summary>
        public AmplaLocationAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaLocationAttribute"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public AmplaLocationAttribute(string location)
        {
            Location = location;
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [with recurse] is set
        /// </summary>
        /// <value>
        ///   <c>true</c> if [with recurse]; otherwise, <c>false</c>.
        /// </value>
        public bool WithRecurse { get; set; }

        /// <summary>
        ///     Tries to get the Location value from the specified type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static bool TryGetLocation<TModel>(out LocationFilter location)
        {
            location = null;
            AmplaLocationAttribute attribute;
            if (typeof (TModel).TryGetAttribute(out attribute))
            {
                location = new LocationFilter(attribute.Location, attribute.WithRecurse);
                if (string.IsNullOrEmpty(location.Location))
                {
                    location = null;
                }
            }

            return location != null;
        }
    }
}