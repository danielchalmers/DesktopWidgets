using System;
using System.ComponentModel;

namespace DesktopWidgets.Classes
{
    public class WidgetClockSettingsBase : WidgetSettingsBase
    {
        [Category("General")]
        [DisplayName("Refresh Interval")]
        public int UpdateInterval { get; set; } = -1;

        [Category("Style")]
        [DisplayName("Time Format")]
        public string TimeFormat { get; set; } = "hh:mm:ss";

        [Category("General")]
        [DisplayName("Time Offset")]
        public TimeSpan TimeOffset { get; set; }
    }
}