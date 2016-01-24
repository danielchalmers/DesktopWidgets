using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

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

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide On Search")]
        public bool HideOnSearch { get; set; } = true;
    }
}