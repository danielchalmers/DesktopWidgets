using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.WidgetBase.Settings;

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
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment ButtonHorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        [Category("Shortcut Style")]
        [DisplayName("Vertical Alignment")]
        public VerticalAlignment ButtonVerticalAlignment { get; set; } = VerticalAlignment.Center;

        [Category("Shortcut Style")]
        [DisplayName("Icon Scaling Mode")]
        public ImageScalingMode IconScalingMode { get; set; } = ImageScalingMode.LowQuality;

        [Category("Shortcut Style")]
        [DisplayName("Content Mode")]
        public ShortcutContentMode ShortcutContentMode { get; set; } = ShortcutContentMode.Both;

        [Category("Style")]
        [DisplayName("Orientation")]
        public ShortcutOrientation ShortcutOrientation { get; set; } = ShortcutOrientation.Vertical;

        [Category("Shortcut Style")]
        [DisplayName("Height")]
        public int ButtonHeight { get; set; } = 32;

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide on Shortcut Launch")]
        public bool HideOnExecute { get; set; } = true;

        [Category("General")]
        [DisplayName("Allow Drag Drop Files")]
        public bool AllowDropFiles { get; set; } = true;

        [DisplayName("Default Shortcuts Mode")]
        public DefaultShortcutsMode DefaultShortcutsMode { get; set; } = DefaultShortcutsMode.Preset;

        [Category("General")]
        [DisplayName("Parse Shortcut Files")]
        public bool ParseShortcutFiles { get; set; } = false;

        [Category("General")]
        [DisplayName("Enable Icon Cache")]
        public bool UseIconCache { get; set; } = true;
    }
}