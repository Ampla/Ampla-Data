using System.ComponentModel;

namespace AmplaData.Binding.MetaData
{
    public class AllowEmptyStringConverter<T> : TypeConverter
    {
        private readonly T defaultValue;
        private readonly TypeConverter typeConverter;

        public AllowEmptyStringConverter() : this(default(T))
        {
        }

        public AllowEmptyStringConverter(T defaultValue)
        {
            this.defaultValue = defaultValue;
            typeConverter = TypeDescriptor.GetConverter(typeof (T));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string strValue = (string) value;
            if (string.IsNullOrEmpty(strValue))
            {
                return defaultValue;
            }
            return typeConverter.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return typeConverter.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return typeConverter.CanConvertTo(context, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return typeConverter.CreateInstance(context, propertyValues);
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return typeConverter.GetCreateInstanceSupported(context);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return typeConverter.IsValid(context, value);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, System.Attribute[] attributes)
        {
            return typeConverter.GetProperties(context, value, attributes);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return typeConverter.GetPropertiesSupported(context);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return typeConverter.GetStandardValues(context);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return typeConverter.GetStandardValuesExclusive(context);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return typeConverter.GetStandardValuesSupported(context);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            return typeConverter.ConvertTo(context, culture, value, destinationType);
        }
    }
}