using System.ComponentModel;
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

        [DisplayName("Saved Text")]
        public string Text { get; set; }
    }
}