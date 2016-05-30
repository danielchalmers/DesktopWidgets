using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.LatencyMonitor
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.Width = 225;
            Style.Height = 210;
        }

        [Category("General")]
        [DisplayName("Ping Interval")]
        public int PingInterval { get; set; } = 1000;

        [Category("Style")]
        [DisplayName("Show Date/Time")]
        public bool ShowTime { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Status")]
        public bool ShowStatus { get; set; } = true;

        [Category("Style")]
        [DisplayName("Date/Time Format")]
        public string DateTimeFormat { get; set; } = "HH:mm:ss";

        [Category("Style")]
        [DisplayName("Latency Padding")]
        public int LatencyPadding { get; set; } = 4;

        [Category("General")]
        [DisplayName("Host URL")]
        public string HostAddress { get; set; } = "";

        [Category("General")]
        [DisplayName("Max History")]
        public int MaxHistory { get; set; } = 99;

        [Category("General")]
        [DisplayName("Timeout")]
        public int Timeout { get; set; } = 5000;
    }
}