
using System;
using System.ComponentModel;
using System.Globalization;

namespace AmplaWeb.Data.Binding.MetaData
{
    /// <summary>
    /// DateTime Type Converter to store DateTime values 
    /// </summary>
    public class Iso8601DateTimeConverter : DateTimeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (context == null && destinationType == typeof (string) && CultureInfo.InvariantCulture.Equals(culture))
            {
                DateTime localTime = (DateTime)value;
                return localTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if (stringValue != null && CultureInfo.InvariantCulture.Equals(culture))
            {
                DateTime utcTime = DateTime.ParseExact(stringValue, "yyyy-MM-ddTHH:mm:ssZ", null, DateTimeStyles.AdjustToUniversal);
                return utcTime.ToLocalTime();
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}