using System;

namespace AmplaWeb.Data.Binding.MetaData
{
    public static class DateTimeExtensions
    {
         public static string ToIso8601Format(this DateTime dateTime)
         {
             return string.Format("{0:yyyy-MM-ddTHH:mm:ssZ}", dateTime);
         }
    }
}