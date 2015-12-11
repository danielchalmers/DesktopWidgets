using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.StopwatchClock.OptionsPages;

namespace DesktopWidgets.Widgets.StopwatchClock
{
    public static class Metadata
    {
        public const string FriendlyName = "Stopwatch";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}