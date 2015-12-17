namespace DesktopWidgets.Helpers
{
    internal static class LinkHelper
    {
        public static bool IsHyperlink(string link)
        {
            return link.StartsWith("http") || link.StartsWith("www.");
        }
    }
}