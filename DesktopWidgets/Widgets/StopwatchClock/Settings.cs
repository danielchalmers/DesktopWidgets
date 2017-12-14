using System.Collections.Generic;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.StopwatchClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public Settings()
        {
            DateTimeFormat = new List<string> { "mm'm 'ss's 'ff'ms'" };
            UpdateInterval = 10;
        }
    }
}