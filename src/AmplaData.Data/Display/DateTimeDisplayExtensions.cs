using System;

namespace AmplaData.Data.Display
{
    public static class DateTimeDisplayExtensions
    {
        /// <summary>
        /// returns a very *small* humanized string indicating how long ago something happened, eg "3d ago"
        /// </summary>
        public static string ToRelativeTimeMini(this DateTime dt, bool includeTimeForOldDates = true, bool includeAgo = true)
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime time = dt.ToUniversalTime();

            TimeSpan ts = utcNow.Subtract(time);

            var delta = ts.TotalSeconds;

            if (delta < 60)
            {
                return ts.Seconds + "s" + (includeAgo ? " ago" : "");
            }
            if (delta < 3600) // 60 mins * 60 sec
            {
                return ts.Minutes + "m" + (includeAgo ? " ago" : "");
            }
            if (delta < 86400)  // 24 hrs * 60 mins * 60 sec
            {
                return ts.Hours + "h" + (includeAgo ? " ago" : "");
            }
            var days = ts.Days;
            if (days <= 2)
            {
                return days + "d" + (includeAgo ? " ago" : "");
            }
            if (days <= 330)
            {
                return dt.ToString(includeTimeForOldDates ? "MMM %d 'at' %H:mmm" : "MMM %d").ToLowerInvariant();
            }
            return dt.ToString(includeTimeForOldDates ? @"MMM %d \'yy 'at' %H:mmm" : @"MMM %d \'yy").ToLowerInvariant();
        }

    }
}