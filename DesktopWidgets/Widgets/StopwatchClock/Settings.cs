using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.StopwatchClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public Settings()
        {
            Format = "mm'm 'ss's 'ff'ms'";
            UpdateInterval = 10;
        }
    }
}