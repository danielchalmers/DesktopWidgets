using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.TimeClock
{
    public class ViewModel : ClockViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromId(id) as Settings;
            if (Settings == null)
                return;
        }

        public Settings Settings { get; }
    }
}