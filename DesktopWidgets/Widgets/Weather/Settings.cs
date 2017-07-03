using System;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Weather
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.FontSettings.FontSize = 16;
        }

        [Category("Style")]
        [DisplayName("Unit Type")]
        public TemperatureUnitType UnitType { get; set; }

        [Category("General")]
        [DisplayName("Refresh Interval")]
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromHours(1);

        [Category("General")]
        [DisplayName("ZIP Code")]
        public string ZipCode { get; set; }

        [Category("General")]
        [DisplayName("API Key")]
        public string ApiKey { get; set; }

        [Category("Style")]
        [DisplayName("Show Icon")]
        public bool ShowIcon { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Temperature")]
        public bool ShowTemperature { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Temperature Range")]
        public bool ShowTempMinMax { get; set; } = false;

        [Category("Style")]
        [DisplayName("Show Description")]
        public bool ShowDescription { get; set; } = true;
    }
}