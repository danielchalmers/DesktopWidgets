using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Note
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.Width = 160;
            Style.Height = 132;
        }

        [DisplayName("Saved Text")]
        public string Text { get; set; }
    }
}