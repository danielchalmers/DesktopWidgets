using System;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public DateTime EndDateTime { get; set; } = DateTime.Now;
    }
}