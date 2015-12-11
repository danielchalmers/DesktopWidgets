using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.CountdownClock.OptionsPages;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public static class Metadata
    {
        public const string FriendlyName = "Countdown";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}