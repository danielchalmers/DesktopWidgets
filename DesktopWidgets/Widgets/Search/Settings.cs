using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Search
{
    public class Settings : WidgetSettings
    {
        public Settings()
        {
            Width = 150;
        }

        public string BaseUrl { get; set; }
    }
}