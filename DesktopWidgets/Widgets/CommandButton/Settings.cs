using System.ComponentModel;
using System.Windows.Input;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.CommandButton
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Width = 96;
            Height = 96;
        }

        [Category("General")]
        [DisplayName("Path")]
        public string Path { get; set; }

        [Category("General")]
        [DisplayName("Arguments")]
        public string Args { get; set; }

        [Category("General")]
        [DisplayName("Hotkey")]
        public Key HotKey { get; set; }

        [Category("General")]
        [DisplayName("Hotkey Modifiers")]
        public ModifierKeys HotKeyModifiers { get; set; }

        [Category("Style")]
        [DisplayName("Button Text")]
        public string ButtonText { get; set; } = "Execute";
    }
}