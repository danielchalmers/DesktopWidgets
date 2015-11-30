using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class ViewModel : ClockViewModelBase
    {
        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromId(guid) as Settings;
            if (Settings == null)
                return;
        }

        public Settings Settings { get; }
    }
}