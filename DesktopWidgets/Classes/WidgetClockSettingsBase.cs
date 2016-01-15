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
        [DisplayName("Time Offset (Positive)")]
        public TimeSpan TimeOffsetPositive { get; set; }

        [Category("General")]
        [DisplayName("Time Offset (Negative)")]
        public TimeSpan TimeOffsetNegative { get; set; }
    }
}