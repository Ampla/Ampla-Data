using System;

namespace AmplaData.Data.Records
{
    public static class DateTimeExtensions
    {
        public static DateTime TrimToSeconds(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Kind);
        }

        public static DateTime TrimToHour(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Kind);
        }
    }
}