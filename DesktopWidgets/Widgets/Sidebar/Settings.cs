using System.Collections.ObjectModel;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Sidebar
{
    public class Settings : WidgetSettingsBase
    {
        [DisplayName("Shortcuts")]
        public ObservableCollection<Shortcut> Shortcuts { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Icon Position")]
        public IconPosition IconPosition { get; set; } = IconPosition.Left;

        [Category("Shortcut Style")]
        [DisplayName("Tooltip Type")]
        public ToolTipType ToolTipType { get; set; } = ToolTipType.None;

        [Category("Shortcut Style")]
        [DisplayName("Shortcut Alignment")]
        public ShortcutAlignment ButtonAlignment { get; set; } = ShortcutAlignment.Center;

        [Category("Shortcut Style")]
        [DisplayName("Icon Scaling Mode")]
        public ImageScalingMode IconScalingMode { get; set; } = ImageScalingMode.LowQuality;

        [Category("Shortcut Style")]
        [DisplayName("Shortcut Content Mode")]
        public ShortcutContentMode ShortcutContentMode { get; set; } = ShortcutContentMode.Both;

        [Category("Style")]
        [DisplayName("Scrollbar Visibility")]
        public ScrollBarVisibility ScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;

        [Category("Style")]
        [DisplayName("Shortcut Orientation")]
        public ShortcutOrientation ShortcutOrientation { get; set; } = ShortcutOrientation.Auto;

        [Category("Shortcut Style")]
        [DisplayName("Shortcut Height")]
        public int ButtonHeight { get; set; } = 32;

        [Category("Behavior")]
        [DisplayName("Hide on Shortcut Launch")]
        public bool HideOnExecute { get; set; } = true;

        [DisplayName("Allow Drag Drop Files")]
        public bool AllowDropFiles { get; set; } = true;

        [DisplayName("Default Shortcuts Mode")]
        public DefaultShortcutsMode DefaultShortcutsMode { get; set; } = DefaultShortcutsMode.Preset;

        [DisplayName("Parse Shortcut Files")]
        public bool ParseShortcutFiles { get; set; } = true;

        [DisplayName("Enable Icon Cache")]
        public bool UseIconCache { get; set; } = true;
    }
}