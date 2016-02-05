using System;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Weather
{
    public class Settings : WidgetSettingsBase
    {
        [Category("Style")]
        [DisplayName("Unit Type")]
        public TemperatureUnitType UnitType { get; set; }

        [Category("General")]
        [DisplayName("Refresh Interval")]
        public TimeSpan RefreshInterval { get; set; }

        [Category("General")]
        [DisplayName("Zip Code")]
        public int ZipCode { get; set; }

        [Category("Style")]
        [DisplayName("Show Icon")]
        public bool ShowIcon { get; set; }

        [Category("Style")]
        [DisplayName("Show Temperature")]
        public bool ShowTemperature { get; set; }

        [Category("Style")]
        [DisplayName("Show Temperature Range")]
        public bool ShowTempMinMax { get; set; }

        [Category("Style")]
        [DisplayName("Show Description")]
        public bool ShowDescription { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            UnitType = TemperatureUnitType.Celsius;
            RefreshInterval = TimeSpan.FromHours(1);
            ZipCode = 0;
            ShowIcon = true;
            ShowTemperature = true;
            ShowTempMinMax = false;
            ShowDescription = true;
        }
    }
}