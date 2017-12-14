using System;
using System.Collections.Generic;
using System.Text;

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

        public static string ToMultilineString(this DateTime dateTime, List<string> formatLines)
        {
            var formatted = new StringBuilder();
            foreach (var formatLine in formatLines)
            {
                try
                {
                    formatted.Append(dateTime.ToString(formatLine));
                }
                catch (FormatException)
                {
                    formatted.Append("Bad format");
                }
                if (formatLines.IndexOf(formatLine) < formatLines.Count - 1)
                {
                    formatted.AppendLine();
                }
            }
            return formatted.ToString();
        }

        public static string ToMultilineString(this TimeSpan timeSpan, List<string> formatLines)
        {
            var formatted = new StringBuilder();
            foreach (var formatLine in formatLines)
            {
                try
                {
                    formatted.Append(timeSpan.ToString(formatLine));
                }
                catch (FormatException)
                {
                    formatted.Append("Bad format");
                }
                if (formatLines.IndexOf(formatLine) < formatLines.Count - 1)
                {
                    formatted.AppendLine();
                }
            }
            return formatted.ToString();
        }
    }
}