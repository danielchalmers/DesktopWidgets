using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.Weather.OptionsPages;

namespace DesktopWidgets.Widgets.Weather
{
    public static class Metadata
    {
        public const string FriendlyName = "Weather";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}