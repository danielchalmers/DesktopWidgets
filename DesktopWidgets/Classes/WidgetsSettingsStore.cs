using System.Collections.ObjectModel;

namespace DesktopWidgets.Classes
{
    public class WidgetsSettingsStore
    {
        public int Version { get; set; } = 1;
        public ObservableCollection<WidgetSettings> Widgets { get; set; }
    }
}