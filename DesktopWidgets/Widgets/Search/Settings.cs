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
        [DisplayName("URL Prefix")]
        public string BaseUrl { get; set; } = "http://";

        [Category("General")]
        [DisplayName("URL Suffix")]
        public string URLSuffix { get; set; }
    }
}