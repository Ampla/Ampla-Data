using System;
using System.Globalization;

namespace AmplaWeb.Data.Records
{
    public static class PersistenceHelper
    {
        public static string ConvertToString<T>(T value)
        {
            if (typeof(T) == typeof(DateTime))
            {
                DateTime dt = (DateTime)(object)value;
                return dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            return value.ToString();
        }

        public static T ConvertFromString<T>(string value)
        {
            if (typeof(T) == typeof(DateTime))
            {
                return (T)(object)DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ssZ", null, DateTimeStyles.AdjustToUniversal);
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}