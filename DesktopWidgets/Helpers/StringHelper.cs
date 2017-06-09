using System;
using System.Collections.Generic;

namespace DesktopWidgets.Helpers
{
    public static class StringHelper
    {
        public static bool Contains(this string source, string value, bool caseInsensitive)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value))
            {
                return false;
            }
            return caseInsensitive
                ? source.IndexOf(value, 0, StringComparison.CurrentCultureIgnoreCase) != -1
                : source.Contains(value);
        }

        public static List<string> ExtractFromString(string text, string startString, string endString)
        {
            var matched = new List<string>();
            var exit = false;
            while (!exit)
            {
                var indexStart = text.IndexOf(startString, StringComparison.Ordinal);
                var indexEnd = text.IndexOf(endString, StringComparison.Ordinal);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(text.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                {
                    exit = true;
                }
            }
            return matched;
        }

        // https://stackoverflow.com/a/4975942/5889966
        public static string BytesToString(long byteCount, int decimalPlaces = 1)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
            {
                return "0" + suf[0];
            }
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), decimalPlaces);
            return Math.Sign(byteCount) * num + suf[place];
        }
    }
}