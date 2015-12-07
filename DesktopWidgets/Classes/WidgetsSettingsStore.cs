using System.Collections.ObjectModel;

namespace DesktopWidgets.Classes
{
    public class WidgetsSettingsStore
    {
        public int Version { get; set; } = 1;
        public ObservableCollection<WidgetSettingsBase> Widgets { get; set; }
    }
}