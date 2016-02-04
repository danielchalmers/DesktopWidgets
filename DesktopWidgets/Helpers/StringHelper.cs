using System;
using System.Collections.Generic;

namespace DesktopWidgets.Helpers
{
    public static class StringHelper
    {
        public static bool Contains(this string source, string value, bool caseInsensitive)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value))
                return false;
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
                    exit = true;
            }
            return matched;
        }
    }
}