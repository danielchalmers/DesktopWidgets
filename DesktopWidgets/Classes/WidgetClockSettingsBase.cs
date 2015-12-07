namespace DesktopWidgets.Classes
{
    public class WidgetClockSettingsBase : WidgetSettingsBase
    {
        public int UpdateInterval { get; set; } = -1;
        public string TimeFormat { get; set; } = "hh:mm:ss";
    }
}