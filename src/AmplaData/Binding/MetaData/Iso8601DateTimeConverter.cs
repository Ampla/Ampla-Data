
using System;
using System.ComponentModel;
using System.Globalization;

namespace AmplaData.Binding.MetaData
{
    /// <summary>
    /// DateTime Type Converter to store DateTime values as Iso8601 formatted strings. 
    /// </summary>
    public class Iso8601DateTimeConverter : DateTimeConverter
    {
        private static readonly Iso8601DateTimeConverter Converter = new Iso8601DateTimeConverter();

        /// <summary>
        /// Converts the given value object to a <see cref="T:System.DateTime" /> using the arguments.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (context == null && destinationType == typeof (string) && CultureInfo.InvariantCulture.Equals(culture))
            {
                DateTime localTime = (DateTime) value;
                return localTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Converts the given value object to a <see cref="T:System.DateTime" />.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if (stringValue != null && CultureInfo.InvariantCulture.Equals(culture))
            {
                DateTime utcTime = DateTime.ParseExact(stringValue, "yyyy-MM-ddTHH:mm:ssZ", null,
                                                       DateTimeStyles.AdjustToUniversal);
                return utcTime.ToLocalTime();
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the Isodate time.
        /// </summary>
        /// <param name="iso8601Date">The iso8601 date.</param>
        /// <returns></returns>
        public static DateTime ConvertToLocalDateTime(string iso8601Date)
        {
            object result = Converter.ConvertFromInvariantString(iso8601Date);

            if (result != null)
            {
                return (DateTime) result;
            }
            return DateTime.MinValue;
        }

        public static string ConvertFromLocalDateTime(DateTime value)
        {
            return Converter.ConvertToInvariantString(value);
        }
    }
}