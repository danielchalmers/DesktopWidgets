using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Search
{
    public class Settings : WidgetSettings
    {
        public Settings()
        {
            Width = 150;
            SizeToContentMode = SizeToContent.Height;
        }

        public string BaseUrl { get; set; }
    }
}