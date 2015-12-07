using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Note
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Width = 160;
            Height = 200;
        }

        public string Text { get; set; }
    }
}