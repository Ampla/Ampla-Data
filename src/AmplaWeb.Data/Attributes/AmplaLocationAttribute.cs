using System;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Attributes
{
    /// <summary>
    ///     Attribute used to mark an class as an Ampla Model
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AmplaLocationAttribute : Attribute
    {
        public string Location { get; set; }

        /// <summary>
        ///     Tries to get the Location value from the specified type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static bool TryGetLocation<TModel>(out string location)
        {
            location = null;
            AmplaLocationAttribute attribute;
            if (typeof (TModel).TryGetAttribute(out attribute))
            {
                location = attribute.Location;
            }

            return !string.IsNullOrEmpty(location);
        }
    }
}