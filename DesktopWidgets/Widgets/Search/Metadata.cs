using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.Search.OptionsPages;

namespace DesktopWidgets.Widgets.Search
{
    public static class Metadata
    {
        public const string FriendlyName = "Search";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}