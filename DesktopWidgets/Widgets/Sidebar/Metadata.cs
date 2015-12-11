using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.Sidebar.OptionsPages;

namespace DesktopWidgets.Widgets.Sidebar
{
    public static class Metadata
    {
        public const string FriendlyName = "Sidebar";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}