using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Search
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Width = 150;
        }

        [Category("General")]
        [DisplayName("Base URL")]
        public string BaseUrl { get; set; }
    }
}