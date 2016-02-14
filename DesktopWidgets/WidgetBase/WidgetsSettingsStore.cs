using System.Collections.ObjectModel;
using DesktopWidgets.Classes;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.WidgetBase
{
    public class WidgetsSettingsStore
    {
        public int Version { get; set; } = 2;
        public ObservableCollection<WidgetSettingsBase> Widgets { get; set; }

        public ObservableCollection<EventActionPair> EventActionPairs { get; set; } =
            new ObservableCollection<EventActionPair>();
    }
}