using System;

namespace DesktopWidgets.Helpers
{
    public static class DateTimeSyncHelper
    {
        public static DateTime SyncNext(this DateTime dateTime,
            DateTime currentDateTime,
            bool syncYear,
            bool syncMonth,
            bool syncDay,
            bool syncHour,
            bool syncMinute,
            bool syncSecond)
        {
            var endDateTime = dateTime;
            endDateTime = new DateTime(
                syncYear
                    ? currentDateTime.Year
                    : endDateTime.Year,
                syncMonth
                    ? currentDateTime.Month
                    : endDateTime.Month,
                syncDay
                    ? currentDateTime.Day
                    : endDateTime.Day,
                syncHour
                    ? currentDateTime.Hour
                    : endDateTime.Hour,
                syncMinute
                    ? currentDateTime.Minute
                    : endDateTime.Minute,
                syncSecond
                    ? currentDateTime.Second
                    : endDateTime.Second,
                endDateTime.Kind);

            if (syncSecond && endDateTime < currentDateTime)
                endDateTime = endDateTime.AddSeconds(1);

            if (syncMinute && endDateTime < currentDateTime)
                endDateTime = endDateTime.AddMinutes(1);

            if (syncHour && endDateTime < currentDateTime)
                endDateTime = endDateTime.AddHours(1);

            if (syncDay && endDateTime < currentDateTime)
                endDateTime = endDateTime.AddDays(1);

            if (syncMonth && endDateTime < currentDateTime)
                endDateTime = endDateTime.AddMonths(1);

            if (syncYear && endDateTime < currentDateTime)
                endDateTime = endDateTime.AddYears(1);

            return endDateTime;
        }
    }
}