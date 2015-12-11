using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.TimeClock.OptionsPages;

namespace DesktopWidgets.Widgets.TimeClock
{
    public static class Metadata
    {
        public const string FriendlyName = "Clock";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}