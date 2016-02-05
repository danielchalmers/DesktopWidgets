using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.Note
{
    public class Settings : WidgetSettingsBase
    {
        [DisplayName("Saved Text")]
        public string Text { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Text = "";
            Width = 160;
            Height = 132;
        }
    }
}