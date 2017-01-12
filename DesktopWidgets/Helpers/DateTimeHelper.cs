using System;
using System.Collections.Generic;
using System.Linq;

namespace DesktopWidgets.Helpers
{
    public static class DateTimeHelper
    {
        public static string ToReadableString(this TimeSpan span)
        {
            var formatted =
                $"{(span.Duration().Days > 0 ? $"{span.Days:0} day{(span.Days == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} hour{(span.Hours == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} minute{(span.Minutes == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Seconds > 0 ? $"{span.Seconds:0} second{(span.Seconds == 1 ? string.Empty : "s")}" : string.Empty)}";

            if (formatted.EndsWith(", "))
            {
                formatted = formatted.Substring(0, formatted.Length - 2);
            }

            if (string.IsNullOrEmpty(formatted))
            {
                formatted = "0 seconds";
            }

            return formatted;
        }

        private static string ParseCustomFormat(this DateTime dateTime, string format)
            =>
                StringHelper.ExtractFromString(format, "{", "}")
                    .Aggregate(format, (current, v) => current.Replace("{" + v + "}", dateTime.ToString(v)));

        private static string ParseCustomFormat(this TimeSpan dateTime, string format)
            =>
                StringHelper.ExtractFromString(format, "{", "}")
                    .Aggregate(format, (current, v) => current.Replace("{" + v + "}", dateTime.ToString(v)));

        public static string ParseCustomFormat(this DateTime dateTime, IEnumerable<string> format)
            => string.Join(Environment.NewLine, format.Select(v => dateTime.ParseCustomFormat(v)));

        public static string ParseCustomFormat(this TimeSpan dateTime, IEnumerable<string> format)
            => string.Join(Environment.NewLine, format.Select(v => dateTime.ParseCustomFormat(v)));
    }
}