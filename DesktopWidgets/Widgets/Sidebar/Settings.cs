using System.Collections.ObjectModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.Sidebar
{
    public class Settings : WidgetSettings
    {
        public ObservableCollection<Shortcut> Shortcuts { get; set; } = new ObservableCollection<Shortcut>();
        public IconPosition IconPosition { get; set; } = IconPosition.Left;
        public ToolTipType ToolTipType { get; set; } = ToolTipType.None;
        public ShortcutAlignment ButtonAlignment { get; set; } = ShortcutAlignment.Center;
        public ImageScalingMode IconScalingMode { get; set; } = ImageScalingMode.LowQuality;
        public ShortcutContentMode ShortcutContentMode { get; set; } = ShortcutContentMode.Both;
        public ScrollBarVisibility ScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;
        public ShortcutOrientation ShortcutOrientation { get; set; } = ShortcutOrientation.Auto;
        public int ButtonHeight { get; set; } = 32;
        public bool HideOnExecute { get; set; } = true;
        public bool AllowDropFiles { get; set; } = true;
        public int UseDefaults { get; set; } = 0;
    }
}