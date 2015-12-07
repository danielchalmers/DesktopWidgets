using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Search
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Width = 150;
        }

        public string BaseUrl { get; set; }
    }
}