using System;

namespace DesktopWidgets.Helpers
{
    public static class DateTimeSyncHelper
    {
        public static DateTime SyncNext(this DateTime dateTime, bool syncYear, bool syncMonth, bool syncDay,
            bool syncHour, bool syncMinute, bool syncSecond)
        {
            var endDateTime = dateTime;
            endDateTime = new DateTime(
                syncYear
                    ? DateTime.Now.Year
                    : endDateTime.Year,
                syncMonth
                    ? DateTime.Now.Month
                    : endDateTime.Month,
                syncDay
                    ? DateTime.Now.Day
                    : endDateTime.Day,
                syncHour
                    ? DateTime.Now.Hour
                    : endDateTime.Hour,
                syncMinute
                    ? DateTime.Now.Minute
                    : endDateTime.Minute,
                syncSecond
                    ? DateTime.Now.Second
                    : endDateTime.Second,
                endDateTime.Kind);

            if (syncYear && endDateTime < DateTime.Now)
                endDateTime = endDateTime.AddYears(1);

            if (syncMonth && endDateTime < DateTime.Now)
                endDateTime = endDateTime.AddMonths(1);

            if (syncDay && endDateTime < DateTime.Now)
                endDateTime = endDateTime.AddDays(1);

            if (syncHour && endDateTime < DateTime.Now)
                endDateTime = endDateTime.AddHours(1);

            if (syncMinute && endDateTime < DateTime.Now)
                endDateTime = endDateTime.AddMinutes(1);

            if (syncSecond && endDateTime < DateTime.Now)
                endDateTime = endDateTime.AddSeconds(1);

            return endDateTime;
        }
    }
}