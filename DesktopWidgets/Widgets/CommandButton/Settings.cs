using System.ComponentModel;
using System.Windows.Input;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.CommandButton
{
    public class Settings : WidgetSettingsBase
    {
        [Category("General")]
        [DisplayName("Path")]
        public string FilePath { get; set; }

        [Category("General")]
        [DisplayName("Arguments")]
        public string FileArguments { get; set; }

        [Category("General")]
        [DisplayName("Hotkey")]
        public Key CommandHotKey { get; set; }

        [Category("General")]
        [DisplayName("Hotkey Modifiers")]
        public ModifierKeys CommandHotKeyModifiers { get; set; }

        [Category("Style")]
        [DisplayName("Button Text")]
        public string ButtonText { get; set; } = "Execute";

        [Category("Style")]
        [DisplayName("Button Width")]
        public double ButtonWidth { get; set; } = 96;

        [Category("Style")]
        [DisplayName("Button Height")]
        public double ButtonHeight { get; set; } = 96;
    }
}