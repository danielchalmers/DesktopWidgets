using System.ComponentModel;
using System.Windows.Media;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.LatencyMonitor
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.Width = double.NaN;
            Style.Height = double.NaN;
            Style.MinWidth = 150;
            Style.MinHeight = 200;
            Style.FontSettings.FontFamily = new FontFamily("Consolas");
        }

        [Category("General")]
        [DisplayName("Ping Interval")]
        public int PingInterval { get; set; } = 1000;

        [Category("Style")]
        [DisplayName("Show Date/Time")]
        public bool ShowTime { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Status")]
        public bool ShowStatus { get; set; } = false;

        [Category("Style")]
        [DisplayName("Show Download Usage")]
        public bool ShowDownloadUsage { get; set; } = false;

        [Category("Style")]
        [DisplayName("Show Upload Usage")]
        public bool ShowUploadUsage { get; set; } = false;

        [Category("Style")]
        [DisplayName("Date/Time Format")]
        public string DateTimeFormat { get; set; } = "HH:mm:ss";

        [Category("Style")]
        [DisplayName("Latency Padding")]
        public int LatencyPadding { get; set; } = 4;

        [Category("Style")]
        [DisplayName("Download/Upload Padding")]
        public int BandwidthPadding { get; set; } = 6;

        [Category("Style")]
        [DisplayName("Download/Upload Decimal Places")]
        public int BandwidthDecimalPlaces { get; set; } = 0;

        [Category("General")]
        [DisplayName("Host URL")]
        public string HostAddress { get; set; } = "8.8.8.8";

        [Category("General")]
        [DisplayName("Max History")]
        public int MaxHistory { get; set; } = 10;

        [Category("General")]
        [DisplayName("Timeout")]
        public int Timeout { get; set; } = 4000;

        [Category("General")]
        [DisplayName("Latency Good Max")]
        public int LatencyGoodMax { get; set; } = 100;

        [Category("General")]
        [DisplayName("Latency Good Since Last")]
        public int LatencyGoodSinceLast { get; set; } = 10;

        [Category("Style")]
        [DisplayName("Color Coding")]
        public bool ColorCoding { get; set; } = true;

        [Category("Style")]
        [DisplayName("Latency Default Color")]
        public Color LatencyDefaultColor { get; set; } = Colors.Black;

        [Category("Style")]
        [DisplayName("Latency Good Color")]
        public Color LatencyGoodColor { get; set; } = Colors.LimeGreen;

        [Category("Style")]
        [DisplayName("Latency Bad Color")]
        public Color LatencyBadColor { get; set; } = Colors.Red;

        [Category("Style")]
        [DisplayName("List Background Color")]
        public Color ListBackgroundColor { get; set; } = Colors.Transparent;

        [Category("Style")]
        [DisplayName("List Background Opacity")]
        public double ListBackgroundOpacity { get; set; } = 1.0;
    }
}