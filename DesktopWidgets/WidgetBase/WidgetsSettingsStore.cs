using System.Collections.ObjectModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.WidgetBase
{
    public class WidgetsSettingsStore
    {
        public int Version { get; set; } = 1;
        public ObservableCollection<WidgetSettingsBase> Widgets { get; set; }
    }
}