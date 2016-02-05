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
        public IconPosition IconPosition { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Tooltip Type")]
        public ToolTipType ToolTipType { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment ButtonHorizontalAlignment { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Vertical Alignment")]
        public VerticalAlignment ButtonVerticalAlignment { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Icon Scaling Mode")]
        public ImageScalingMode IconScalingMode { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Content Mode")]
        public ShortcutContentMode ShortcutContentMode { get; set; }

        [Category("Style")]
        [DisplayName("Orientation")]
        public ShortcutOrientation ShortcutOrientation { get; set; }

        [Category("Shortcut Style")]
        [DisplayName("Height")]
        public int ButtonHeight { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide on Shortcut Launch")]
        public bool HideOnExecute { get; set; }

        [Category("General")]
        [DisplayName("Allow Drag Drop Files")]
        public bool AllowDropFiles { get; set; }

        [DisplayName("Default Shortcuts Mode")]
        public DefaultShortcutsMode DefaultShortcutsMode { get; set; }

        [Category("General")]
        [DisplayName("Parse Shortcut Files")]
        public bool ParseShortcutFiles { get; set; }

        [Category("General")]
        [DisplayName("Enable Icon Cache")]
        public bool UseIconCache { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Shortcuts = null;
            IconPosition = IconPosition.Left;
            ToolTipType = ToolTipType.None;
            ButtonHorizontalAlignment = HorizontalAlignment.Center;
            ButtonVerticalAlignment = VerticalAlignment.Center;
            IconScalingMode = ImageScalingMode.LowQuality;
            ShortcutContentMode = ShortcutContentMode.Both;
            ShortcutOrientation = ShortcutOrientation.Vertical;
            ButtonHeight = 32;
            HideOnExecute = true;
            AllowDropFiles = true;
            DefaultShortcutsMode = DefaultShortcutsMode.Preset;
            ParseShortcutFiles = false;
            UseIconCache = true;
        }
    }
}