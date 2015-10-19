using System;
using System.ComponentModel;
using System.Globalization;

namespace AmplaData.Binding.MetaData
{
    /// <summary>
    ///     TimeSpan to integer converter used to convert TimeSpan values to and from integer values for use in Duration properties
    /// </summary>
    public class TimeSpanIntConverter : TimeSpanConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof (TimeSpan)
                   || destinationType == typeof (int)
                   || destinationType == typeof (double)
                   || base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (TimeSpan)
                   || sourceType == typeof (int)
                   || sourceType == typeof (double)
                   || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to a <see cref="T:System.TimeSpan" />.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if (stringValue != null && CultureInfo.InvariantCulture.Equals(culture))
            {
                double seconds;
                if (double.TryParse(stringValue, out seconds))
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
                    return timeSpan;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given object to another type.
        /// </summary>
        /// <param name="context">A formatter context.</param>
        /// <param name="culture">The culture into which <paramref name="value" /> will be converted.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destinationType">The type to convert the object to.</param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (context == null)
            {
                if (destinationType == typeof (string) && CultureInfo.InvariantCulture.Equals(culture))
                {
                    TimeSpan timeSpan = (TimeSpan) value;
                    int seconds = Convert.ToInt32(timeSpan.TotalSeconds);
                    return Convert.ToString(seconds);
                }

                if (value is TimeSpan)
                {
                    TimeSpan timeSpanValue = (TimeSpan) value;
                    if (destinationType == typeof (TimeSpan))
                    {
                        return TimeSpan.FromTicks(timeSpanValue.Ticks);
                    }
                    if (destinationType == typeof (double))
                    {
                        return timeSpanValue.TotalSeconds;
                    }
                    if (destinationType == typeof (int))
                    {
                        return Convert.ToInt32(timeSpanValue.TotalSeconds);
                    }
                }
                if (value is double || value is int)
                {
                    if (destinationType == typeof (TimeSpan))
                    {
                        return TimeSpan.FromSeconds(Convert.ToDouble(value));
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}