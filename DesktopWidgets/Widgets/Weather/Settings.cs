using System;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Weather
{
    public class Settings : WidgetSettingsBase
    {
        public TemperatureUnitType UnitType { get; set; }
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromHours(1);
        public int ZipCode { get; set; }
        public bool ShowIcon { get; set; } = true;
        public bool ShowTemperature { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
    }
}