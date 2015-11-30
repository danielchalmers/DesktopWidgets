using System;

namespace DesktopWidgets.Classes
{
    public class WidgetClockSettingsBase : WidgetSettings
    {
        public TimeSpan TickInterval { get; set; } = TimeSpan.FromMilliseconds(100);
        public string TimeFormat { get; set; } = "hh:mm:ss";
    }
}