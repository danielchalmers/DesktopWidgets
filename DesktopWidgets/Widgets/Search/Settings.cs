using System.ComponentModel;
using System.Windows;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Search
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.Width = 150;
            Style.FramePadding = new Thickness(0);
        }

        [Category("General")]
        [DisplayName("URL Prefix")]
        public string BaseUrl { get; set; } = "http://";

        [Category("General")]
        [DisplayName("URL Suffix")]
        public string URLSuffix { get; set; }
    }
}