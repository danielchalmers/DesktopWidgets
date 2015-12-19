using System;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Weather
{
    public class Settings : WidgetSettingsBase
    {
        [Category("Style")]
        [DisplayName("Unit Type")]
        public TemperatureUnitType UnitType { get; set; }

        [Category("General")]
        [DisplayName("Refresh Interval")]
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromHours(1);

        [Category("General")]
        [DisplayName("Zip Code")]
        public int ZipCode { get; set; }

        [Category("Style")]
        [DisplayName("Show Icon")]
        public bool ShowIcon { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Temperature")]
        public bool ShowTemperature { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Temperature Min/Max")]
        public bool ShowTempMinMax { get; set; } = false;

        [Category("Style")]
        [DisplayName("Show Description")]
        public bool ShowDescription { get; set; } = true;
    }
}