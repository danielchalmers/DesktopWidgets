using System;

namespace DesktopWidgets.Classes
{
    public class WidgetClockSettingsBase : WidgetSettings
    {
        public int UpdateInterval { get; set; } = -1;
        public string TimeFormat { get; set; } = "hh:mm:ss";
    }
}