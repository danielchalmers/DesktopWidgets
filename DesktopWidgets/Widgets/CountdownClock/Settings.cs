using System;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public Settings()
        {
            TimeFormat = "dd:hh:mm:ss";
        }

        public DateTime EndDateTime { get; set; } = DateTime.Now;
    }
}