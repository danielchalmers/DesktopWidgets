using System;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Weather
{
    public class Settings : WidgetSettings
    {
        public TemperatureUnitType UnitType { get; set; }
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromHours(1);
        public int ZipCode { get; set; }
    }
}