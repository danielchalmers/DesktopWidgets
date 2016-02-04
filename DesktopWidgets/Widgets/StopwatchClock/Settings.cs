using System.Collections.Generic;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.StopwatchClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public Settings()
        {
            DateTimeFormat = new List<string>
            {
                "{mm}:{ss}.{fff}"
            };
            UpdateInterval = 10;
        }
    }
}