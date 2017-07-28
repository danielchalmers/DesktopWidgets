using System.Text.RegularExpressions;

namespace DesktopWidgets.Helpers
{
    public static class LinkHelper
    {
        public static bool IsHyperlink(string link)
        {
            return Regex.IsMatch(link, @"^((http[s]?:\/\/)|(www.)).+$");
        }
    }
}