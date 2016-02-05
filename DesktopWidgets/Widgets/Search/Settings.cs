using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Search
{
    public class Settings : WidgetSettingsBase
    {
        [Category("General")]
        [DisplayName("URL Prefix")]
        public string BaseUrl { get; set; }

        [Category("General")]
        [DisplayName("URL Suffix")]
        public string URLSuffix { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide On Search")]
        public bool HideOnSearch { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            BaseUrl = "http://";
            URLSuffix = "";
            HideOnSearch = true;
            Width = 150;
        }
    }
}