using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Note
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.MinWidth = 160;
            Style.MinHeight = 132;
        }

        [DisplayName("Saved Text")]
        public string Text { get; set; }

        [Category("Style")]
        [DisplayName("Read Only")]
        public bool ReadOnly { get; set; }
    }
}